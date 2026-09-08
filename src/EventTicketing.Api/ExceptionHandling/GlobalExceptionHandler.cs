using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EventTicketing.Api.ExceptionHandling;

// it translates a .net exception into an HTTP response with a problem details payload.
// It also logs the exception using the provided logger.
// IExceptionHandler is an ASP.NET Core contract.
// It tells the framework: "this class knows how to handle unhandled exceptions."
public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    // ASP.NET Core calls this method whenever an unhandled exception
    // reaches app.UseExceptionHandler().
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "Unhandled exception. TraceId: {TraceId}",
            httpContext.TraceIdentifier);

        // ProblemDetails is the standard API error-response format
        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred."
        };

        problem.Extensions["traceId"] = httpContext.TraceIdentifier;

        // Set the actual HTTP response status to 500.
        httpContext.Response.StatusCode = problem.Status.Value;

        // Convert the ProblemDetails object to JSON and send it back.
        // The cancellation token stops work if the client disconnects.
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}
