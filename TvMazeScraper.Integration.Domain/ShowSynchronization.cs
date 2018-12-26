using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Contracts;
using TvMazeScraper.Contracts.Entities;
using TvMazeScraper.Integration.Domain.Entities;

namespace TvMazeScraper.Integration.Domain
{
    public class ShowSynchronization : IDisposable
    {
        private readonly IKeyValueStore _keyValueStore;
        private readonly IShowStore _showStore;
        private readonly TvMazeApiClient _dataFetcher;

        public ShowSynchronization(IKeyValueStore keyValueStore, IShowStore showStore, TvMazeApiClient dataFetcher)
        {
            _keyValueStore = keyValueStore;
            _showStore = showStore;
            _dataFetcher = dataFetcher;
        }

        public async Task StartSynchronizationAsync(CancellationToken cancellationToken)
        {
            var updateList = await GetUpdateListAsync(cancellationToken).ConfigureAwait(false);
            await _showStore.RemoveNotMatchedAsync(updateList.Keys).ConfigureAwait(false);
            await UpdateShowsAsync(updateList, cancellationToken).ConfigureAwait(false);
        }

        private async Task UpdateShowsAsync(Dictionary<int, int> updateList, CancellationToken cancellationToken)
        {
            var statusKey = "JobStatus";

            var lastStatus = await _keyValueStore.GetAsync<ShowSynchronizationStatus>(statusKey) ?? new ShowSynchronizationStatus();

            var currentStatus = new ShowSynchronizationStatus();

            foreach (var showToUpdate in updateList.OrderBy(t => t.Value))
            {
                if (cancellationToken.CanBeCanceled && cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (showToUpdate.Value < lastStatus.Date ||
                    showToUpdate.Value == lastStatus.Date && lastStatus.ShowIds.Contains(showToUpdate.Key))
                {
                    continue;
                }

                var show = await GetShowInfoAsync(showToUpdate.Key, cancellationToken);
                await _showStore.SetAsync(show);

                if (currentStatus.Date == showToUpdate.Value)
                {
                    currentStatus.ShowIds.Add(showToUpdate.Key);
                }
                else
                {
                    currentStatus.Date = showToUpdate.Value;
                    currentStatus.ShowIds.Clear();
                    currentStatus.ShowIds.Add(showToUpdate.Key);
                }

                await _keyValueStore.SetAsync(statusKey, currentStatus);
            }
        }

        private async Task<Dictionary<int, int>> GetUpdateListAsync(CancellationToken cancellationToken)
        {
            var errorQty = 0;

            while (true)
            {
                if (cancellationToken.CanBeCanceled && cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                var result = await _dataFetcher.GetUpdateListAsync(cancellationToken).ConfigureAwait(false);

                if (result.IsRateLimitExceed)
                {
                    await Task.Delay(++errorQty * 200, cancellationToken);
                }
                else
                {
                    return result.data;
                }
            }
        }

        private async Task<IShow> GetShowInfoAsync(int id, CancellationToken cancellationToken)
        {
            var errorQty = 0;

            while (true)
            {
                var result = await _dataFetcher.GetShowInfoAsync(id, cancellationToken);

                if (result.IsRateLimitExceed)
                {
                    await Task.Delay(++errorQty * 200, cancellationToken);
                }
                else
                {
                    return result.data;
                }
            }
        }

        public void Dispose()
        {
            _dataFetcher.Dispose();
        }
    }
}