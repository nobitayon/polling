namespace Delta.Polling.WebRP.Infrastructure.UserRole;

public record UserRoleOptions
{
    public const string SectionKey = $"{nameof(UserRole)}";

    public required string Provider { get; init; } = UserRoleProvider.SimpleTor;
}

public static class UserRoleProvider
{
    public const string SimpleTor = nameof(SimpleTor);
}
