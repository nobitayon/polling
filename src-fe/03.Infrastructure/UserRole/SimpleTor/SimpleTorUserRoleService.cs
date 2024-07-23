using Delta.Polling.FrontEnd.Services.UserRole;

namespace Delta.Polling.FrontEnd.Infrastructure.UserRole.SimpleTor;

public class SimpleTorUserRoleService : IUserRoleService
{
    private readonly SimpleTorUserRoleOptions _simpleTorUserRoleOptions;
    private readonly RestClient _restClient;

    public SimpleTorUserRoleService(IOptions<SimpleTorUserRoleOptions> simpleTorUserRoleOptions)
    {
        _simpleTorUserRoleOptions = simpleTorUserRoleOptions.Value;
        _restClient = new RestClient(_simpleTorUserRoleOptions.ApiBaseUrl);
    }

    public async Task<IEnumerable<string>> GetMyRolesAsync(string jwt, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest($"Applications/{_simpleTorUserRoleOptions.ApplicationId}/Roles/My", Method.Get);
        _ = restRequest.AddHeader(KnownHeaders.Authorization, $"Bearer {jwt}");

        var restResponse = await _restClient.ExecuteAsync<IEnumerable<string>>(restRequest, cancellationToken);

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
            throw new NullException(nameof(restResponse.Data), typeof(IEnumerable<string>));
        }

        return restResponse.Data;
    }
}
