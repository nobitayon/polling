namespace Delta.Polling.Services.UserRole;

public interface IUserRoleService
{
    public Task<IEnumerable<string>> GetMyRolesAsync(string jwt, CancellationToken cancellationToken = default);
}
