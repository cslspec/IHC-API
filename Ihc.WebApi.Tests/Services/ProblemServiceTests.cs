using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Http;

namespace Ihc.WebApi.Tests.Services;

[TestClass]
public class ProblemServiceTests
{
    private static readonly string RequestPath = "/api/resources/42/runtime";

    [TestMethod]
    public void GetProblemDetails_ForNotFoundException_Returns404AndRequestPath()
    {
        var result = CreateService().GetProblemDetails(new NotFoundException("Resource not found."));

        Assert.AreEqual(StatusCodes.Status404NotFound, result.Status);
        Assert.AreEqual("Not found", result.Title);
        Assert.AreEqual("Resource not found.", result.Detail);
        Assert.AreEqual(RequestPath, result.Instance);
    }

    [TestMethod]
    public void GetProblemDetails_ForArgumentException_Returns400()
    {
        var result = CreateService().GetProblemDetails(new ArgumentException("Invalid input."));

        Assert.AreEqual(StatusCodes.Status400BadRequest, result.Status);
        Assert.AreEqual("Invalid request", result.Title);
        Assert.AreEqual("Invalid input.", result.Detail);
        Assert.AreEqual(RequestPath, result.Instance);
    }

    [TestMethod]
    public void GetProblemDetails_ForConnectionFailure_Returns503()
    {
        var result = CreateService().GetProblemDetails(new HttpRequestException("Controller unavailable."));

        Assert.AreEqual(StatusCodes.Status503ServiceUnavailable, result.Status);
        Assert.AreEqual("Connection error", result.Title);
        Assert.AreEqual("Controller unavailable.", result.Detail);
        Assert.AreEqual(RequestPath, result.Instance);
    }

    [TestMethod]
    public void GetProblemDetails_ForControllerHttpError_Returns500()
    {
        var exception = new HttpRequestException(
            "Controller returned an error.",
            null,
            System.Net.HttpStatusCode.BadGateway);

        var result = CreateService().GetProblemDetails(exception);

        Assert.AreEqual(StatusCodes.Status500InternalServerError, result.Status);
        Assert.AreEqual("IHC controller error", result.Title);
        Assert.AreEqual("Controller returned an error.", result.Detail);
    }

    [TestMethod]
    public void GetProblemDetails_ForUnexpectedException_Returns500()
    {
        var result = CreateService().GetProblemDetails(new InvalidOperationException("Unexpected failure."));

        Assert.AreEqual(StatusCodes.Status500InternalServerError, result.Status);
        Assert.AreEqual("Unexpected error", result.Title);
        Assert.AreEqual("Unexpected failure.", result.Detail);
    }

    [TestMethod]
    public void GetProblemDetails_ForAuthorizationError_UsesCommunicationErrorTitle()
    {
        var exception = new AuthorizationException("Login denied.", CommunicationErrors.AccountInvalid);

        var result = CreateService().GetProblemDetails(exception);

        Assert.AreEqual(StatusCodes.Status503ServiceUnavailable, result.Status);
        Assert.AreEqual(CommunicationErrors.AccountInvalid.Title, result.Title);
        Assert.AreEqual("Login denied.", result.Detail);
    }

    private static ProblemService CreateService()
    {
        var context = new DefaultHttpContext();
        context.Request.Path = RequestPath;
        return new ProblemService(new HttpContextAccessor { HttpContext = context });
    }
}
