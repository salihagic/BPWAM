using BPWA.Common.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BPWA.Background.Services
{
    public class AccountDeactivationNotificationsService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<AccountDeactivationNotificationsService> _logger;
        private readonly BackgroundServicesSettings _backgroundServicesSettings;
        private Timer _timer;

        public AccountDeactivationNotificationsService(
            ILogger<AccountDeactivationNotificationsService> logger,
            BackgroundServicesSettings backgroundServicesSettings
            )
        {
            _logger = logger;
            _backgroundServicesSettings = backgroundServicesSettings;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Account deactivation notifications service running.");

            _timer = new Timer(
                DoWork, 
                null, 
                TimeSpan.Zero,
                _backgroundServicesSettings.AccountDeactivationNotificationsServiceRepeatingPeriod);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            //Execute work here

            _logger.LogInformation("Account deactivation notifications service is working. Count: {Count}", count);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Account deactivation notifications service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
