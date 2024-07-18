using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Delta.Polling.Logics.Common.Behaviors;

public class LoggingBehavior<TRequest>(
    ILogger<TRequest> logger,
    ICurrentUserService currentUserService)
    : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var formattedRequest = request.ToPrettyJson();
        var username = currentUserService.Username;

        if (string.IsNullOrWhiteSpace(username))
        {
            username = "Anonymous";
        }

        logger.LogInformation("Processing {RequestName} for {Username}.\n{RequestName}\n{RequestObject}",
           requestName, username, requestName, formattedRequest);

        return Task.CompletedTask;
    }
}
