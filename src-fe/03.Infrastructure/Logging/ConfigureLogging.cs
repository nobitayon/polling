using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Sinks.SystemConsole.Themes;

namespace Delta.Polling.FrontEnd.Infrastructure.Logging;

public static class ConfigureLogging
{
    public static IHostBuilder UseLoggingService(this IHostBuilder hostBuilder)
    {
        _ = hostBuilder.UseSerilog((hostBuilderContext, loggerConfiguration) =>
        {
            _ = loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration)
                .WriteTo.Console
                (
                    theme: AnsiConsoleTheme.Code,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Hour);
        });

        SelfLog.Enable(Console.WriteLine);

        return hostBuilder;
    }
}
