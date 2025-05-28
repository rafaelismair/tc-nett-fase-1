using Serilog;
using Serilog.Events;

namespace Fiap.CloudGames.Fase1.API.Middleware.Logging
{
    public class SerilogConfiguration
    {
        public static Serilog.Core.Logger ConfigureSerilog()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
                .WriteTo.Console(
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [CorrelationId: {CorrelationId}] {Message}{NewLine}{Exception}")
                .WriteTo.File("logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [CorrelationId: {CorrelationId}] {Message}{NewLine}{Exception}")
                .CreateLogger();

            //.MinimumLevel.Information()
            //.Enrich.FromLogContext()
            //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
            //.WriteTo.Console(
            //    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [CorrelationId: {CorrelationId}] {Message}{NewLine}{Exception}")
            //.WriteTo.File("logs/log-.txt",
            //    rollingInterval: RollingInterval.Day,
            //    retainedFileCountLimit: 7,
            //    restrictedToMinimumLevel: LogEventLevel.Information,
            //    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [CorrelationId: {CorrelationId}] {Message}{NewLine}{Exception}")
            //.CreateLogger()
        }
    }
}
