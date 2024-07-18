namespace Delta.Polling.Infrastructure.UserRole.SimpleTor;

public record SimpleTorUserRoleOptions
{
    public const string SectionKey = $"{nameof(UserRole)}:{nameof(SimpleTor)}";

    public required string ApiBaseUrl { get; init; }
    public required string ApplicationId { get; init; }
    public required string TokenUrl { get; init; }
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
    public required string[] Scopes { get; init; } = ["SimpleTor.Application.GetRoleUsers", "SimpleTor.Application.GetUserRoles"];
}
