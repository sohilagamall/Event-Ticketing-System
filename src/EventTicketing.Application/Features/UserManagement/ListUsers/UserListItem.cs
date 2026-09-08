namespace EventTicketing.Application.Features.UserManagement.ListUsers;

public sealed record UserListItem(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    bool IsLockedOut,
    IReadOnlyList<string> Roles);