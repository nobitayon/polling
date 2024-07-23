using System.Net;
using Delta.Polling.Both.Common.Helpers;
using Delta.Polling.Services.UserProfile;
using RestSharp;

namespace Delta.Polling.Infrastructure.UserProfile.SimpleTor;

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

        if (!restResponse.IsSuccessful)
        {
            if (restResponse.ErrorException is not null)
            {
                throw restResponse.ErrorException;
            }

            throw new HttpRequestException(restResponse.ErrorMessage, restResponse.ErrorException, restResponse.StatusCode);
        }

        if (restResponse.Data is null)
        {
            throw new NullException(nameof(restResponse.Data), typeof(UserProfileItem));
        }

        return restResponse.Data;
    }

    public async Task<UserProfileItem?> GetUserProfileAsync(string username, CancellationToken cancellationToken = default)
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

        if (!restResponse.IsSuccessful)
        {
            if (restResponse.StatusCode is HttpStatusCode.NotFound)
            {
                return null;
            }

            if (restResponse.ErrorException is not null)
            {
                throw restResponse.ErrorException;
            }

            throw new HttpRequestException(restResponse.ErrorMessage, restResponse.ErrorException, restResponse.StatusCode);
        }

        if (restResponse.Data is null)
        {
            throw new NullException(nameof(restResponse.Data), typeof(UserProfileItem));
        }

        return restResponse.Data;
    }
}
