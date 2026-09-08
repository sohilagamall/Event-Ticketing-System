using EventTicketing.Application.Features.UserManagement.ListUsers;
using EventTicketing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventTicketing.Infrastructure.Identity.Services;

public sealed class IdentityUserQueryService(
    AppDbContext dbContext,
    TimeProvider timeProvider)
    : IUserQueryService
{
    public async Task<PagedResult<UserListItem>> ListAsync(
        ListUsersQuery query,
        CancellationToken cancellationToken = default)
    {
        IQueryable<ApplicationUser> users = dbContext.Users
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();

            users = users.Where(user =>
                user.FirstName.Contains(search) ||
                user.LastName.Contains(search) ||
                (user.Email != null && user.Email.Contains(search)));
        }

        if (query.Role.HasValue)
        {
            var normalizedRoleName = query.Role.Value
                .ToString()
                .ToUpperInvariant();

            var roleId = await dbContext.Roles
                .AsNoTracking()
                .Where(role => role.NormalizedName == normalizedRoleName)
                .Select(role => role.Id)
                .SingleOrDefaultAsync(cancellationToken);

            users = users.Where(user =>
                dbContext.UserRoles.Any(userRole =>
                    userRole.UserId == user.Id &&
                    userRole.RoleId == roleId));
        }

        var now = timeProvider.GetUtcNow();

        if (query.IsLockedOut is true)
        {
            users = users.Where(user =>
                user.LockoutEnd.HasValue &&
                user.LockoutEnd > now);
        }
        else if (query.IsLockedOut is false)
        {
            users = users.Where(user =>
                !user.LockoutEnd.HasValue ||
                user.LockoutEnd <= now);
        }

        users = (query.SortBy, query.SortDirection) switch
        {
            (UserSortBy.Email, SortDirection.Ascending) =>
                users.OrderBy(user => user.Email)
                    .ThenBy(user => user.Id),

            (UserSortBy.Email, SortDirection.Descending) =>
                users.OrderByDescending(user => user.Email)
                    .ThenBy(user => user.Id),

            (UserSortBy.FirstName, SortDirection.Ascending) =>
                users.OrderBy(user => user.FirstName)
                    .ThenBy(user => user.Id),

            (UserSortBy.FirstName, SortDirection.Descending) =>
                users.OrderByDescending(user => user.FirstName)
                    .ThenBy(user => user.Id),

            (UserSortBy.CreatedAt, SortDirection.Ascending) =>
                users.OrderBy(user => user.CreatedAt)
                    .ThenBy(user => user.Id),

            _ =>
                users.OrderByDescending(user => user.CreatedAt)
                    .ThenBy(user => user.Id)
        };

        var totalCount = await users.CountAsync(cancellationToken);

        var skip = (query.Page - 1) * query.PageSize;

        var pageUsers = await users
            .Skip(skip)
            .Take(query.PageSize)
            .Select(user => new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.CreatedAt,
                user.UpdatedAt,
                IsLockedOut = user.LockoutEnd.HasValue &&
                              user.LockoutEnd > now
            })
            .ToListAsync(cancellationToken);

        var userIds = pageUsers.Select(user => user.Id).ToList();

        var roleAssignments = await (
            from userRole in dbContext.UserRoles.AsNoTracking()
            join role in dbContext.Roles.AsNoTracking()
                on userRole.RoleId equals role.Id
            where userIds.Contains(userRole.UserId)
            select new
            {
                userRole.UserId,
                RoleName = role.Name
            })
            .ToListAsync(cancellationToken);

        var rolesByUserId = roleAssignments
            .GroupBy(assignment => assignment.UserId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<string>)group
                    .Select(assignment => assignment.RoleName ?? string.Empty)
                    .OrderBy(roleName => roleName)
                    .ToList());

        var items = pageUsers
            .Select(user => new UserListItem(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email ?? string.Empty,
                user.CreatedAt,
                user.UpdatedAt,
                user.IsLockedOut,
                rolesByUserId.GetValueOrDefault(user.Id, [])))
            .ToList();

        return new PagedResult<UserListItem>(
            items,
            query.Page,
            query.PageSize,
            totalCount);
    }

}