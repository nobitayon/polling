using System.Reflection;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Delta.Polling.FrontEnd.Logics.Common.Behaviors;

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

        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            logger.LogError("User is not authenticated.");

            throw new ForbiddenException("Current User is not authenticated.");
        }

        var requiredRoleNames = authorizeAttributes
            .Where(authorizeAttribute => !string.IsNullOrWhiteSpace(authorizeAttribute.RoleName))
            .Select(authorizeAttribute => authorizeAttribute.RoleName!)
            .DistinctBy(roleName => roleName)
            .ToList();

        // Kalau request ini tidak ada membutuhkan Role
        if (requiredRoleNames.Count is 0)
        {
            return Task.CompletedTask;
        }

        var isInRole = requiredRoleNames.Intersect(currentUserService.RoleNames).Any();

        if (isInRole)
        {
            return Task.CompletedTask;
        }

        if (requiredRoleNames.Count > 1)
        {
            var roleNames = string.Join(", ", requiredRoleNames);

            logger.LogError("User {Username} does not have one of these roles {RoleNames}.",
                currentUserService.Username, roleNames);

            throw new ForbiddenException($"You are not authorized to perform this action because you don't have one of these roles: {roleNames}.");
        }

        logger.LogError("User {Username} does not have the role {RoleName}.",
            currentUserService.Username, requiredRoleNames.Single());

        throw new ForbiddenException($"You are not authorized to perform this action because you don't have this role: {requiredRoleNames.Single()}.");
    }
}
