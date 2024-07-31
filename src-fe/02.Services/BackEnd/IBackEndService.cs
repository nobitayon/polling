using Delta.Polling.Both.Common.Responses;
using RestSharp;

namespace Delta.Polling.FrontEnd.Services.BackEnd;

public interface IBackEndService
{
    Task<ResponseResult<T>> SendRequestAsync<T>(RestRequest restRequest, CancellationToken cancellationToken = default);
}
