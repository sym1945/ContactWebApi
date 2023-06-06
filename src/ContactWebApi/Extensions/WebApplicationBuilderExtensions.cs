using Serilog;

namespace ContactWebApi.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void RegisterSerilog(this WebApplicationBuilder builder)
        {
            long fileLimitSize = 100L * 1024 * 1024;    // 100 MB
            var logFilePath = Path.Combine(AppContext.BaseDirectory, "log/log-.txt");

            builder.Host.UseSerilog((context, services, configuration) => 
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Async(config => config.File(logFilePath, rollingInterval: RollingInterval.Hour, fileSizeLimitBytes: fileLimitSize))
                    .WriteTo.Async(config => config.Console()));
        }
    }
}
