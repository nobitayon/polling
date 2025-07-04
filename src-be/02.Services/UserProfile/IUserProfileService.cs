﻿namespace Delta.Polling.Services.UserProfile;

public interface IUserProfileService
{
    public Task<UserProfileItem> GetMyProfileAsync(string jwt, CancellationToken cancellationToken = default);
    public Task<UserProfileItem?> GetUserProfileAsync(string username, CancellationToken cancellationToken = default);
    public Task<IEnumerable<FullUserProfileItem>> GetUsersAsync(CancellationToken cancellationToken = default);
}

public record UserProfileItem
{
    public required string Email { get; init; }
    public required string Name { get; init; }
}

public record FullUserProfileItem
{
    public required string Email { get; init; }
    public required string Name { get; init; }
    public required string Username { get; init; }
}
