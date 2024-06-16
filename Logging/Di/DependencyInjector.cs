using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace Logging.Di
{
    public static class DependencyInjector
    {
        public static void AddLoggerDependencies(this IHostApplicationBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                        .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            builder.Services.AddSerilog();
            builder.Services.AddSingleton(Log.Logger);
        }
    }
}
