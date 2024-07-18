using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Delta.Polling.Both;

public static class ConfigureShared
{
    public static void AddShared(this IServiceCollection services)
    {
        _ = services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
