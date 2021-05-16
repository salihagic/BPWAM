﻿using BPWA.Common.Configuration;
using BPWA.DAL.Services;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public GuestAccountsDeleterService(
            ILogger<GuestAccountsDeleterService> logger,
            BackgroundServicesSettings backgroundServicesSettings,
            IServiceProvider serviceProvider
            )
        {
            _logger = logger;
            _backgroundServicesSettings = backgroundServicesSettings;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Guest accounts deleter service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                _backgroundServicesSettings.GuestAccountsDeleterServiceRepeatingPeriod);

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            using (var scope = _serviceProvider.CreateScope())
            {
                var accountsService = scope.ServiceProvider.GetService<IAccountsService>();
                await accountsService.DeleteExpiredGuestAccounts();
            }

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
