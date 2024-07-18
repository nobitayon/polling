using Delta.Polling.FrontEnd.Services.UserProfile;

namespace Delta.Polling.FrontEnd.Infrastructure.UserProfile.SimpleTor;

public class SimpleTorUserProfileService : IUserProfileService
{
    private readonly SimpleTorUserProfileOptions _simpleTorUserProfileOptions;
    private readonly RestClient _restClient;

    public SimpleTorUserProfileService(IOptions<SimpleTorUserProfileOptions> simpleTorUserProfileOptions)
    {
        _simpleTorUserProfileOptions = simpleTorUserProfileOptions.Value;
        _restClient = new RestClient(_simpleTorUserProfileOptions.ApiBaseUrl);
    }

    public async Task<UserProfileItem> GetUserProfileAsync(string username, CancellationToken cancellationToken = default)
    {
        var getAccessTokenInput = new GetAccessTokenInput
        {
            TokenUrl = _simpleTorUserProfileOptions.TokenUrl,
            ClientId = _simpleTorUserProfileOptions.ClientId,
            ClientSecret = _simpleTorUserProfileOptions.ClientSecret,
            Scopes = _simpleTorUserProfileOptions.Scopes
        };

        var accessToken = await AccessTokenHelper.GetAccessToken(getAccessTokenInput, cancellationToken);
        var restRequest = new RestRequest($"Users/{username}");
        _ = restRequest.AddHeader(KnownHeaders.Authorization, $"Bearer {accessToken}");

        var restResponse = await _restClient.ExecuteAsync<UserProfileItem>(restRequest, cancellationToken);

        if (restResponse.Data is null)
        {
            throw new NullException(nameof(restResponse.Data), typeof(UserProfileItem));
        }

        return restResponse.Data;
    }
}
