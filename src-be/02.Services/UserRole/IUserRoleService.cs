namespace Delta.Polling.Services.UserRole;

public interface IUserRoleService
{
    Task<IEnumerable<UserItem>> GetRoleUsersAsync(string roleName, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetUserRolesAsync(string username, CancellationToken cancellationToken = default);
}

public record UserItem
{
    public required string Username { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
}