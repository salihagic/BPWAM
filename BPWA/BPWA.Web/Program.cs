using BPWA.DAL.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NpgsqlTypes;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;
using System;
using System.Collections.Generic;
using System.IO;

namespace BPWA
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .Build();

        static void ConfigureLogger()
        {
            var columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                //{"Id", new SinglePropertyColumnWriter("Id", PropertyWriteMethod.Raw, NpgsqlDbType.Integer) },
                {"Message", new RenderedMessageColumnWriter() },
                {"MessageTemplate", new MessageTemplateColumnWriter() },
                {"Level", new LevelColumnWriter() },
                {"CreatedAt", new TimestampColumnWriter() },
                {"Exception", new ExceptionColumnWriter() },
                {"Properties", new LogEventSerializedColumnWriter() },
                {"MachineName", new SinglePropertyColumnWriter("MachineName", format: "l") }
            };

            Log.Logger = new LoggerConfiguration()
                                .WriteTo.PostgreSQL(
                                Configuration.GetSection("DatabaseSettings:ConnectionString").Value,
                                "Logs",
                                columnWriters,
                                respectCase: true,
                                restrictedToMinimumLevel: LogEventLevel.Error
                                )
                                .WriteTo.Console()
                                .CreateLogger();
        }

        public static void Main(string[] args)
        {
            ConfigureLogger();
            Serilog.Debugging.SelfLog.Enable(Console.Error);

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    DatabaseSeeder.Seed(services).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            try
            {
                Log.Information("Starting the BPWA Web application...");

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "BPWA Web application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(Configuration)
                              .UseSerilog()
                              .UseStartup<Startup>();
                });
    }
}
