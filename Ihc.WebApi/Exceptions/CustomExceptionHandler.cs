using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Diagnostics;

namespace Ihc.WebApi.Exceptions
{
    /// <summary>
    /// Converts unhandled exceptions into HTTP problem details responses.
    /// </summary>
    /// <param name="problemDetailsService">Service for writing problem details responses.</param>
    public class CustomExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        /// <summary>
        /// Handles an unhandled exception by writing a problem details response.
        /// </summary>
        /// <param name="httpContext">The current HTTP request context.</param>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="cancellationToken">A token that can cancel writing the response.</param>
        /// <returns><see langword="true"/> if a response was written; otherwise, <see langword="false"/>.</returns>
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // The client disconnected, so there is nobody to write a response to.
            if (exception is OperationCanceledException && httpContext.RequestAborted.IsCancellationRequested)
            {
                return true;
            }

            // The handler is a singleton while the problem service is scoped to the request.
            var problemService = httpContext.RequestServices.GetRequiredService<IProblemService>();
            var problemDetails = problemService.GetProblemDetails(exception);
            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                Exception = exception,
                HttpContext = httpContext,
                ProblemDetails = problemDetails
            });
        }
    }

}
