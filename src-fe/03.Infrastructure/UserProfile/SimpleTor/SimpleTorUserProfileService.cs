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

    public async Task<UserProfileItem> GetMyProfileAsync(string jwt, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("Users/My");
        _ = restRequest.AddHeader(KnownHeaders.Authorization, $"Bearer {jwt}");

        var restResponse = await _restClient.ExecuteAsync<UserProfileItem>(restRequest, cancellationToken);

        if (restResponse.Data is null)
        {
            throw new NullException(nameof(restResponse.Data), typeof(UserProfileItem));
        }

        return restResponse.Data;
    }
}
