using Delta.Polling.FrontEnd.Services.UserProfile;

namespace Delta.Polling.FrontEnd.Infrastructure.UserProfile.Another;

public static class ConfigureAnotherUserProfile
{
    public static IServiceCollection AddAnotherUserProfileService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<AnotherUserProfileOptions>(configuration.GetSection(AnotherUserProfileOptions.SectionKey));
        _ = services.AddScoped<IUserProfileService, AnotherUserProfileService>();

        return services;
    }
}
