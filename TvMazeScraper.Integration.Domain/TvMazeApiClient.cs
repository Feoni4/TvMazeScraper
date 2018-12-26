using AutoMapper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Contracts.Entities;
using TvMazeScraper.Integration.Domain.Entities;
using TvMazeScraper.Integration.Domain.Extensions;

namespace TvMazeScraper.Integration.Domain
{
    public class TvMazeApiClient : IDisposable
    {
        private readonly IMapper _mapper;
        private readonly HttpClient _client;

        public TvMazeApiClient(IMapper mapper)
        {
            _mapper = mapper;
            _client = new HttpClient();
        }

        public Task<(bool IsRateLimitExceed, Dictionary<int, int> data)> GetUpdateListAsync()
        {
            return GetUpdateListAsync(CancellationToken.None);
        }

        public async Task<(bool IsRateLimitExceed, Dictionary<int, int> data)> GetUpdateListAsync(CancellationToken cancellationToken)
        {
            var response = await _client.GetAsync(@"http://api.tvmaze.com/updates/shows", cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<Dictionary<int, int>>(cancellationToken).ConfigureAwait(false);
                return (false, data);
            }

            if (response.IsRateLimitExceed())
            {
                return (true, null);
            }

            response.EnsureSuccessStatusCode();
            return (false, null);
        }

        public Task<(bool IsRateLimitExceed, IShow data)> GetShowInfoAsync(int id)
        {
            return GetShowInfoAsync(id, CancellationToken.None);
        }

        public async Task<(bool IsRateLimitExceed, IShow data)> GetShowInfoAsync(int id, CancellationToken cancellationToken)
        {
            var response = await _client.GetAsync($@"http://api.tvmaze.com/shows/{id}?embed=cast", cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<TvMazeShow>(cancellationToken);
                return (false, _mapper.Map<Show>(data));
            }

            if (response.IsRateLimitExceed())
            {
                return (true, null);
            }

            response.EnsureSuccessStatusCode();
            return (false, null);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}