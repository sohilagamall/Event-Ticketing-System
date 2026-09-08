namespace EventTicketing.Application.Features.UserManagement.ListUsers;

public interface IUserQueryService
{
    Task<PagedResult<UserListItem>> ListAsync(
        ListUsersQuery query,
        CancellationToken cancellationToken = default);
}