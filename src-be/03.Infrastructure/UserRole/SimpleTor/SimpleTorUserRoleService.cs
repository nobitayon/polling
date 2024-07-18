using Delta.Polling.Services.UserRole;
using RestSharp;

namespace Delta.Polling.Infrastructure.UserRole.SimpleTor;

public class SimpleTorUserRoleService : IUserRoleService
{
    private readonly SimpleTorUserRoleOptions _simpleTorUserRoleOptions;
    private readonly RestClient _restClient;

    public SimpleTorUserRoleService(IOptions<SimpleTorUserRoleOptions> simpleTorUserRoleOptions)
    {
        _simpleTorUserRoleOptions = simpleTorUserRoleOptions.Value;
        _restClient = new RestClient(_simpleTorUserRoleOptions.ApiBaseUrl);
    }

    public async Task<IEnumerable<UserItem>> GetRoleUsersAsync(string roleName, CancellationToken cancellationToken = default)
    {
        var accessToken = await AccessTokenHelper.GetAccessToken(new GetAccessTokenInput
        {
            TokenUrl = _simpleTorUserRoleOptions.TokenUrl,
            ClientId = _simpleTorUserRoleOptions.ClientId,
            ClientSecret = _simpleTorUserRoleOptions.ClientSecret,
            Scopes = _simpleTorUserRoleOptions.Scopes,
        }, cancellationToken);

        var restRequest = new RestRequest($"Applications/{_simpleTorUserRoleOptions.ApplicationId}/Roles/{roleName}/Users", Method.Get);
        _ = restRequest.AddHeader(KnownHeaders.Authorization, $"Bearer {accessToken}");

        var restResponse = await _restClient.ExecuteAsync<IEnumerable<UserItem>>(restRequest, cancellationToken);

        if (restResponse.Data is null)
        {
            throw new NullException(nameof(restResponse.Data), typeof(IEnumerable<UserItem>));
        }

        return restResponse.Data;
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(string username, CancellationToken cancellationToken = default)
    {
        var accessToken = await AccessTokenHelper.GetAccessToken(new GetAccessTokenInput
        {
            TokenUrl = _simpleTorUserRoleOptions.TokenUrl,
            ClientId = _simpleTorUserRoleOptions.ClientId,
            ClientSecret = _simpleTorUserRoleOptions.ClientSecret,
            Scopes = _simpleTorUserRoleOptions.Scopes,
        }, cancellationToken);

        var restRequest = new RestRequest($"Applications/{_simpleTorUserRoleOptions.ApplicationId}/Users/{username}/Roles", Method.Get);
        _ = restRequest.AddHeader(KnownHeaders.Authorization, $"Bearer {accessToken}");

        var restResponse = await _restClient.ExecuteAsync<IEnumerable<string>>(restRequest, cancellationToken);

        if (restResponse.Data is null)
        {
            throw new NullException(nameof(restResponse.Data), typeof(IEnumerable<string>));
        }

        return restResponse.Data;
    }
}
