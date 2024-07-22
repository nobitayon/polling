namespace Delta.Polling.Services.UserRole;

public interface IUserRoleService
{
    Task<IEnumerable<string>> GetMyRolesAsync(string jwt, CancellationToken cancellationToken = default);
}
