using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvMazeScraper.Contracts;
using TvMazeScraper.Contracts.Entities;
using TvMazeScraper.DAL.Entities;

namespace TvMazeScraper.DAL
{
    public class ShowStore : IShowStore
    {
        private readonly IDatabase _db;
        private KeyNames _keys;

        public ShowStore(Transport transport)
        {
            _db = transport.GetChannel();
            _keys = new KeyNames();
        }

        public async Task<IShow> GetAsync(int id)
        {
            var redisValue = await _db.HashGetAsync(_keys.ShowList, id).ConfigureAwait(false);

            if (redisValue.HasValue)
                return JsonConvert.DeserializeObject<Show>(redisValue);

            return null;
        }

        public async Task<List<IShow>> GetAsync(int offset, int count)
        {
            var showIds = await _db.SortedSetRangeByRankAsync(_keys.ShowIdsOrdered, offset, offset + count).ConfigureAwait(false);
            var redisValues = await _db.HashGetAsync(_keys.ShowList, showIds).ConfigureAwait(false);

            var shows = new List<IShow>(redisValues.Length);

            foreach (var serializedShow in redisValues)
                shows.Add(JsonConvert.DeserializeObject<Show>(serializedShow));

            return shows;
        }

        public Task SetAsync(IShow show)
        {
            var tran = _db.CreateTransaction();

#pragma warning disable CS4014 // we have not to await here due to library design
            tran.SetAddAsync(_keys.ShowIds, show.Id);
            tran.SortedSetAddAsync(_keys.ShowIdsOrdered, show.Id, (double)show.Id);
            tran.HashSetAsync(_keys.ShowList, show.Id, JsonConvert.SerializeObject(show));
#pragma warning restore CS4014 // we have not to await here due to library design

            return tran.ExecuteAsync(CommandFlags.DemandMaster);
        }

        public async Task RemoveNotMatchedAsync(IEnumerable<int> ids)
        {
            await _db.KeyDeleteAsync(_keys.NewShowIds, CommandFlags.DemandMaster).ConfigureAwait(false);
            await _db.SetAddAsync(_keys.NewShowIds, ids.Select(t => (RedisValue)t).ToArray(), CommandFlags.DemandMaster).ConfigureAwait(false);
            var showIdsToRemove = await _db.SetCombineAsync(SetOperation.Difference, _keys.ShowIds, _keys.NewShowIds, CommandFlags.DemandMaster)
                .ConfigureAwait(false);

            var tran = _db.CreateTransaction();

            foreach (var id in showIdsToRemove)
            {
#pragma warning disable CS4014 // we have not to await here due to library design
                tran.SetRemoveAsync(_keys.ShowIds, id);
                tran.SortedSetRemoveAsync(_keys.ShowIdsOrdered, id);
                tran.HashDeleteAsync(_keys.ShowList, id);
#pragma warning restore CS4014 // we have not to await here due to library design
            }

            await tran.ExecuteAsync(CommandFlags.DemandMaster).ConfigureAwait(false);
        }
    }
}