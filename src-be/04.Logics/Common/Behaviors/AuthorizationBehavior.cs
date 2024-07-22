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

        var requiredRoleNames = new List<string>();
        var isInRole = false;

        foreach (var authorizeAttribute in authorizeAttributes)
        {
            var requiredRoleName = authorizeAttribute.RoleName;

            // Kalau request ini tidak membutuhkan Role tertentu, maka cukup cek apakah user sudah login
            if (string.IsNullOrWhiteSpace(requiredRoleName))
            {
                var username = currentUserService.Username;

                if (string.IsNullOrWhiteSpace(username))
                {
                    logger.LogError("User is not authenticated.");

                    throw new ForbiddenException("User harus sudah login");
                }

                return Task.CompletedTask;
            }

            requiredRoleNames.Add(requiredRoleName);

            isInRole |= currentUserService.RoleNames
                .Any(roleName => roleName == requiredRoleName);
        }

        if (!isInRole)
        {
            if (requiredRoleNames.Count == 1)
            {
                logger.LogError("User {Username} does not have the role {RoleName}.",
                    currentUserService.Username, requiredRoleNames.Single());

                throw new ForbiddenException($"You are not authorized to perform this action because you don't have the role {requiredRoleNames.Single()}.");
            }
            else if (requiredRoleNames.Count > 1)
            {
                var roleNames = string.Join(", ", requiredRoleNames);

                logger.LogError("User {Username} does not have one of these roles {RoleNames}.",
                    currentUserService.Username, roleNames);

                throw new ForbiddenException($"You are not authorized to perform this action because you don't have one of these roles {roleNames}.");
            }
        }

        return Task.CompletedTask;
    }
}
