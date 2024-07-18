using IdentityModel.Client;

namespace Delta.Polling.Both.Common.Helpers;

public static class AccessTokenHelper
{
    public static async Task<string> GetAccessToken(GetAccessTokenInput input, CancellationToken cancellationToken = default)
    {
        var tokenRequest = new ClientCredentialsTokenRequest
        {
            Address = input.TokenUrl,
            ClientId = input.ClientId,
            ClientSecret = input.ClientSecret,
            Scope = string.Join(' ', input.Scopes)
        };

        var client = new HttpClient();
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(tokenRequest, cancellationToken);

        if (tokenResponse.IsError)
        {
            throw new GetAccessTokenFailedException(input.TokenUrl, tokenResponse.Error, tokenResponse.ErrorDescription);
        }

        var accessToken = tokenResponse.AccessToken;

        if (accessToken is null)
        {
            throw new NullException(nameof(accessToken), typeof(string));
        }

        return accessToken;
    }
}

public record GetAccessTokenInput
{
    public required string TokenUrl { get; init; }
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
    public required IEnumerable<string> Scopes { get; init; }
}
