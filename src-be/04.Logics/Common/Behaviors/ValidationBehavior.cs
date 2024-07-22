using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Delta.Polling.Logics.Common.Behaviors;

public class ValidationBehavior<TRequest>(
    ILogger<TRequest> logger,
    ICurrentUserService currentUserService,
    IEnumerable<IValidator<TRequest>> validators)
    : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

            var errorMessages = validationResults
                .Where(result => result.Errors.Count is not 0)
                .SelectMany(result => result.Errors)
                .Select(failure => failure.ErrorMessage);

            if (errorMessages.Any())
            {
                var requestName = typeof(TRequest).Name;
                var username = currentUserService.Username;
                var exception = new ModelValidationException(errorMessages);

                logger.LogError("Validation failed when processing {RequestName} for {Username}.\n{Summary}",
                    requestName, username, exception.Summary);

                throw exception;
            }
        }
    }
}
