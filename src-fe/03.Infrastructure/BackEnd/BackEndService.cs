using Delta.Polling.FrontEnd.Services.BackEnd;
using Delta.Polling.FrontEnd.Services.CurrentUser;

namespace Delta.Polling.FrontEnd.Infrastructure.BackEnd;

public class BackEndService(
    IOptions<BackEndOptions> backEndOptions,
    ICurrentUserService currentUserService)
    : IBackEndService
{
    private readonly RestClient _restClient = new(backEndOptions.Value.ApiBaseUrl);

    public async Task<ResponseResult<T>> SendRequestAsync<T>(RestRequest restRequest, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(currentUserService.AccessToken))
        {
            _ = restRequest.AddHeader(KnownHeaders.Authorization, $"Bearer {currentUserService.AccessToken}");
        }

        var restResponse = await _restClient.ExecuteAsync(restRequest, cancellationToken);

        return restResponse.ToResponseResult<T>();
    }
}
