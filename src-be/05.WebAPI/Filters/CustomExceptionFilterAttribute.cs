using Microsoft.AspNetCore.Mvc.Filters;

namespace Delta.Polling.WebAPI.Filters;

public class CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger)
    : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        if (exception is ForbiddenException)
        {
            var details = new ProblemDetails()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
                Title = "You don't have the correct role",
                Status = StatusCodes.Status403Forbidden,
                Detail = exception.Message
            };

            context.Result = new ObjectResult(details);
            context.ExceptionHandled = true;

            return;
        }

        if (exception is EntityNotFoundException)
        {
            var details = new ProblemDetails()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Entity could not be found",
                Status = StatusCodes.Status404NotFound,
                Detail = exception.Message
            };

            context.Result = new ObjectResult(details);
            context.ExceptionHandled = true;

            return;
        }

        if (exception is ModelValidationException mve)
        {
            var details = new ProblemDetails()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc4918#section-11.2",
                Title = "There is one or more validation error",
                Status = StatusCodes.Status422UnprocessableEntity,
                Detail = mve.Summary
            };

            context.Result = new ObjectResult(details);
            context.ExceptionHandled = true;

            return;
        }

        context.Result = new ObjectResult(new ProblemDetails
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Title = "Oops.. something went wrong",
            Status = StatusCodes.Status500InternalServerError,
            Detail = context.Exception.Message
        });

		logger.LogInformation("Exception Message: {Message}", context.Exception.Message);
        logger.LogError(context.Exception, "Oops.. something went wrong");

        context.ExceptionHandled = true;
    }
}
