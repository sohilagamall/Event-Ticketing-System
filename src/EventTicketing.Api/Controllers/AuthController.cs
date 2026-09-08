using EventTicketing.Application.Features.Authentication.Register;
using Microsoft.AspNetCore.Mvc;

namespace EventTicketing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IRegistrationService registrationService) : ControllerBase
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
        
    
}
