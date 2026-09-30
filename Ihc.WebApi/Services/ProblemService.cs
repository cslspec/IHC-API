using Ihc.WebApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Ihc.WebApi.Services;

/// <summary>
/// Creates HTTP problem details for exceptions raised by the API.
/// </summary>
public interface IProblemService
{
    /// <summary>
    /// Maps an exception to an HTTP problem details response.
    /// </summary>
    /// <param name="exception">The exception to describe.</param>
    /// <returns>Problem details containing the appropriate status and request path.</returns>
    ProblemDetails GetProblemDetails(Exception exception);
}

/// <summary>
/// Creates problem details responses for API exceptions.
/// </summary>
/// <param name="contextAccessor">Accessor for the current HTTP request context.</param>
public class ProblemService(IHttpContextAccessor contextAccessor) : IProblemService
{
    /// <inheritdoc />
    public ProblemDetails GetProblemDetails(Exception exception)
    {
        ProblemDetails result;

        if (exception is AuthorizationException authorizationException)
        {
            result = new ProblemDetails
            {
                Title = authorizationException.Error?.Title ?? "Connection error",
                Detail = exception.Message,
                Status = StatusCodes.Status503ServiceUnavailable,
                Instance = contextAccessor?.HttpContext?.Request.Path,
            };
        }
        else if (exception is HttpRequestException httpRequestException)
        {
            var connectionFailed = httpRequestException.StatusCode == null;
            result = new ProblemDetails
            {
                Title = connectionFailed ? "Connection error" : "IHC controller error",
                Detail = httpRequestException.Message,
                Status = connectionFailed
                    ? StatusCodes.Status503ServiceUnavailable
                    : StatusCodes.Status500InternalServerError,
                Instance = contextAccessor?.HttpContext?.Request.Path,
            };
        }
        else if (exception is NotFoundException)
        {
            result = new ProblemDetails
            {
                Title = "Not found",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound,
                Instance = contextAccessor?.HttpContext?.Request.Path,
            };
        }
        else if (exception is ArgumentException)
        {
            result = new ProblemDetails
            {
                Title = "Invalid request",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest,
                Instance = contextAccessor?.HttpContext?.Request.Path,
            };
        }
        else
        {
            result = new ProblemDetails
            {
                Title = "Unexpected error",
                Detail = exception.Message,
                Status = StatusCodes.Status500InternalServerError,
                Instance = contextAccessor?.HttpContext?.Request.Path,
            };
        }

        return result;
    }
}
