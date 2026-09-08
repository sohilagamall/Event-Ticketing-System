using System.ComponentModel.DataAnnotations;

namespace EventTicketing.Application.Features.UserManagement.ListUsers;

public sealed class ListUsersQuery
{
    [StringLength(100)]
    public string? Search { get; init; }

    public UserRoleFilter? Role { get; init; }

    public bool? IsLockedOut { get; init; }

    [Range(1, 100_000)]
    public int Page { get; init; } = 1;

    [Range(1, 100)]
    public int PageSize { get; init; } = 20;

    public UserSortBy SortBy { get; init; } = UserSortBy.CreatedAt;

    public SortDirection SortDirection { get; init; }
        = SortDirection.Descending;
}