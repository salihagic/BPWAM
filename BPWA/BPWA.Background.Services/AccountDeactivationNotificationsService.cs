using BPWA.Common.Configuration;
using BPWA.DAL.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BPWA.Background.Services
{
    public class AccountDeactivationNotificationsService : IHostedService, IDisposable
    {
        private Timer _timer;
        private int executionCount = 0;
        private readonly IServiceProvider _serviceProvider;
        private readonly BackgroundServicesSettings _backgroundServicesSettings;
        private readonly ILogger<AccountDeactivationNotificationsService> _logger;

        public AccountDeactivationNotificationsService(
            IServiceProvider serviceProvider,
            BackgroundServicesSettings backgroundServicesSettings,
            ILogger<AccountDeactivationNotificationsService> logger
            )
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _backgroundServicesSettings = backgroundServicesSettings;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Account deactivation notifications service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                _backgroundServicesSettings.AccountDeactivationNotificationsServiceRepeatingPeriod);

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            using (var scope = _serviceProvider.CreateScope())
            {
                var accountsService = scope.ServiceProvider.GetService<IAccountsService>();
                await accountsService.SendAccountDeactivationWarningNotifications();
            }

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
