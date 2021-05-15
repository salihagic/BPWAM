using BPWA.Common.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BPWA.Background.Services
{
    public class AccountDeactivationService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<AccountDeactivationService> _logger;
        private readonly BackgroundServicesSettings _backgroundServicesSettings;
        private Timer _timer;

        public AccountDeactivationService(
            ILogger<AccountDeactivationService> logger,
            BackgroundServicesSettings backgroundServicesSettings
            )
        {
            _logger = logger;
            _backgroundServicesSettings = backgroundServicesSettings;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Account deactivation service running.");

            _timer = new Timer(
                DoWork, 
                null, 
                TimeSpan.Zero,
                _backgroundServicesSettings.AccountDeactivationServiceRepeatingPeriod);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            //Execute work here

            _logger.LogInformation("Account deactivation service is working. Count: {Count}", count);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Account deactivation service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
