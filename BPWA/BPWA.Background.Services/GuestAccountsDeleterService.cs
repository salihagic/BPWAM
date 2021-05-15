using BPWA.Common.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BPWA.Background.Services
{
    public class GuestAccountsDeleterService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<GuestAccountsDeleterService> _logger;
        private readonly BackgroundServicesSettings _backgroundServicesSettings;
        private Timer _timer;

        public GuestAccountsDeleterService(
            ILogger<GuestAccountsDeleterService> logger,
            BackgroundServicesSettings backgroundServicesSettings
            )
        {
            _logger = logger;
            _backgroundServicesSettings = backgroundServicesSettings;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Guest accounts deleter service running.");

            _timer = new Timer(
                DoWork, 
                null, 
                TimeSpan.Zero,
                _backgroundServicesSettings.GuestAccountsDeleterServiceRepeatingPeriod);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            //Execute work here

            _logger.LogInformation("Guest accounts deleter service is working. Count: {Count}", count);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Guest accounts deleter service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
