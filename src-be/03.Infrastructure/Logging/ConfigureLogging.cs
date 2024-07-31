using Serilog;
using Serilog.Debugging;
using Serilog.Sinks.SystemConsole.Themes;

namespace Delta.Polling.Infrastructure.Logging;

public static class ConfigureLogging
{
    public static IHostBuilder AddLogging(this IHostBuilder hostBuilder)
    {
        _ = hostBuilder.UseSerilog((hostBuilderContext, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(hostBuilderContext.Configuration));

        SelfLog.Enable(Console.WriteLine);

        return hostBuilder;
    }

    public static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(loggingBuilder =>
        {
            var serilog = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console
                (
                    theme: AnsiConsoleTheme.Code,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();

            _ = loggingBuilder.AddSerilog(serilog);
        });
    }
}
