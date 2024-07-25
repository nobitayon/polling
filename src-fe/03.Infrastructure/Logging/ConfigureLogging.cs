using Serilog;
using Serilog.Debugging;
using Serilog.Sinks.SystemConsole.Themes;

namespace Delta.Polling.FrontEnd.Infrastructure.Logging;

public static class ConfigureLogging
{
    public static IHostBuilder AddLogging(this IHostBuilder hostBuilder)
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

    public static ILoggerFactory CreateLoggerFactory(this IConfiguration configuration)
    {
        return LoggerFactory.Create(loggingBuilder =>
        {
            var serilog = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            _ = loggingBuilder.AddSerilog(serilog);
        });
    }
}
