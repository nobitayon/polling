using System.Reflection;
using Delta.Polling.FrontEnd.Logics.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Delta.Polling.FrontEnd.Logics;

public static class ConfigureLogics
{
    public static void AddLogics(this IServiceCollection services)
    {
        _ = services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        _ = services.AddMediatR(configuration =>
        {
            _ = configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            _ = configuration.AddOpenRequestPreProcessor(typeof(LoggingBehavior<>));
            _ = configuration.AddOpenRequestPreProcessor(typeof(ValidationBehavior<>));
        });
    }
}
