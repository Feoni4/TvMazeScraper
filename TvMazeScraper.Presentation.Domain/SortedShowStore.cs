using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Contracts;
using TvMazeScraper.Contracts.Entities;
using TvMazeScraper.DAL;

namespace TvMazeScraper.Presentation.Domain
{
    public class SortedShowStore : ISortedShowStore
    {
        private readonly ShowStore _showStore;

        public SortedShowStore(ShowStore showStore)
        {
            _showStore = showStore;
        }

        public Task SetAsync(IShow show)
        {
            return _showStore.SetAsync(show);
        }

        public async Task<IShow> GetAsync(int id)
        {
            var show = await _showStore.GetAsync(id);
            show.Cast.Sort(CastComparison);
            return show;
        }

        public async Task<List<IShow>> GetAsync(int offset, int count)
        {
            var showList = await _showStore.GetAsync(offset, count);

            foreach (var show in showList)
            {
                show.Cast.Sort(CastComparison);
            }

            return showList;
        }

        public Task RemoveNotMatchedAsync(IEnumerable<int> ids)
        {
            return _showStore.RemoveNotMatchedAsync(ids);
        }

        private int CastComparison(ICast x, ICast y)
        {
            if (x.Birthday == null && y.Birthday == null)
            {
                return 0;

            }
            else if (x.Birthday == null)
            {
                return -1;
            }
            else if (y.Birthday == null)
            {
                return 1;
            }
            else
            {
                return x.Birthday.Value > y.Birthday.Value ? 0 : 1;
            }
        }
    }

    public interface ISortedShowStore : IShowStore
    {
    }
}