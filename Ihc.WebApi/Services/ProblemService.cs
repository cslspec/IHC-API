using Ihc.WebApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Ihc.WebApi.Services;

public interface IProblemService
{
    ProblemDetails GetProblemDetails(Exception exception);
}

public class ProblemService(IHttpContextAccessor contextAccessor) : IProblemService
{
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
