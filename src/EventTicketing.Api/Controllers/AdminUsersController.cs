using EventTicketing.Application.Features.Authorization;
using EventTicketing.Application.Features.UserManagement.ListUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicketing.Api.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = ApplicationRoles.Admin)]
public sealed class AdminUsersController(
    IUserQueryService userQueryService)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResult<UserListItem>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PagedResult<UserListItem>>> GetUsers(
        [FromQuery] ListUsersQuery query,
        CancellationToken cancellationToken)
    {
        var result = await userQueryService.ListAsync(
            query,
            cancellationToken);

        return Ok(result);
    }
}