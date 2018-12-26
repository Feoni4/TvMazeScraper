using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Integration.Domain;

namespace TvMazeScraper.Integration.Jobs
{
    internal class ShowSynchronizationHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly ShowSynchronization _showSynchronization;
        private Timer _timer;

        private bool _workInProgress = false;

        public ShowSynchronizationHostedService(ILogger<ShowSynchronizationHostedService> logger, ShowSynchronization showSynchronization)
        {
            _logger = logger;
            _showSynchronization = showSynchronization;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Show Synchronization Service is starting.");

            _timer = new Timer(DoWork, cancellationToken, TimeSpan.Zero, TimeSpan.FromSeconds(120));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            lock (_timer)
            {
                if (_workInProgress)
                {
                    return;
                }
                _workInProgress = true;
            }

            _logger.LogInformation("Show synchronization started.");

            var cancellationToken = (CancellationToken)state;

            try
            {
                var task = _showSynchronization.StartSynchronizationAsync(cancellationToken);
                task.Wait(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured during show synchronization.");
            }
            finally
            {
                lock (_timer)
                {
                    _workInProgress = false;
                }
            }

            _logger.LogInformation("Show synchronization finished.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Show Synchronization Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _showSynchronization.Dispose();
        }
    }
}