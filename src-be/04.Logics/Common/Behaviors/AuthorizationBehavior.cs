using System.Reflection;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Delta.Polling.Logics.Common.Behaviors;

public class AuthorizationBehavior<TRequest>(
    ICurrentUserService currentUserService,
    ILogger<TRequest> logger)
    : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        // Kalau request ini tidak ada attribute Authorize-nya
        if (authorizeAttributes.Count() is 0)
        {
            return Task.CompletedTask;
        }

        var authorizeAttribute = authorizeAttributes.Single();
        var roleName = authorizeAttribute.RoleName;

        // Kalau request ini tidak membutuhkan Role tertentu, maka cukup cek apakah user sudah login
        if (string.IsNullOrWhiteSpace(roleName))
        {
            var username = currentUserService.Username;

            if (string.IsNullOrWhiteSpace(username))
            {
                logger.LogError("User is not authenticated.");

                throw new ForbiddenException("User harus sudah login");
            }

            return Task.CompletedTask;
        }

        var isInRole = currentUserService.RoleNames
            .Any(currentRoleName =>
            {
                return currentRoleName == roleName;
            });

        if (!isInRole)
        {
            logger.LogError("User {Username} is not in role {RoleName}.",
                currentUserService.Username, roleName);

            throw new ForbiddenException($"You are not authorized to perform this action because you are not in role {roleName}.");
        }

        return Task.CompletedTask;
    }
}
