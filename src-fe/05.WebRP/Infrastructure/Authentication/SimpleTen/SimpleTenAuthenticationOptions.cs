using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Polling.WebRP.Infrastructure.Authentication.SimpleTen;

public record SimpleTenAuthenticationOptions
{
    public const string SectionKey = $"{nameof(Authentication)}:{nameof(SimpleTen)}";

    public required string Authority { get; init; }
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }

    public required string[] Scopes { get; init; } =
    [
        OpenIdConnectScope.OpenId,
        "SimpleTor.User.GetMyUser",
        "SimpleTor.Application.GetMyRoles",
        "Polling.All.Scopes"
    ];
}
