using EventTicketing.Application.Features.Authentication.Login;
using EventTicketing.Application.Features.Authentication.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventTicketing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IRegistrationService registrationService, ILoginService loginService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCustomerRequest request , CancellationToken cancellationToken)
    {
        var result = await registrationService.RegisterCustomerAsync(request, cancellationToken);
        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                errors = result.Errors
            });
        }
        return StatusCode(StatusCodes.Status201Created, new
        {
            userId = result.UserId
        });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await loginService.LoginAsync(request, cancellationToken);
        if(!result.Succeeded)
        {
            return Unauthorized(new
            {
                errors = result.Errors
            });
        }
        return Ok(new
        {
            accessToken = result.AccessToken,
            expiresAt = result.ExpiresAt
        });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = User.FindFirstValue(ClaimTypes.Email);
        var roles = User.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
        return Ok(new
        {
            userId ,
            email ,
            roles 
        });
    }


}
