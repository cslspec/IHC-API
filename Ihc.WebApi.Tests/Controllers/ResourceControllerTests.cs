using Ihc.WebApi.Controllers;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model.Resource;
using Ihc.WebApi.Services;
using NSubstitute;

namespace Ihc.WebApi.Tests.Controllers;

[TestClass]
public class ResourceControllerTests
{
    private IResourceService service = null!;
    private ResourceController controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        service = Substitute.For<IResourceService>();
        controller = new ResourceController(service);
    }

    [TestMethod]
    public async Task GetRuntimeValue_ForwardsResourceIdAndReturnsServiceValue()
    {
        var expected = new ResourceValue
        {
            ResourceId = 42,
            IsRuntimeValue = true,
            TypeString = "integer",
            Value = new IntegerValueData { Value = 5, MinimumValue = 0, MaximumValue = 10 }
        };
        service.GetRuntimeValue(42).Returns(expected);

        var result = await controller.GetRuntimeValue(42);

        Assert.AreSame(expected, result.Value);
        await service.Received(1).GetRuntimeValue(42);
    }

    [TestMethod]
    public async Task GetRuntimeValue_WhenResourceDoesNotExist_PropagatesNotFoundException()
    {
        service.GetRuntimeValue(42).Returns<Task<ResourceValue>>(
            _ => throw new NotFoundException("No value found for resource 42."));

        await Assert.ThrowsExactlyAsync<NotFoundException>(() => controller.GetRuntimeValue(42));
        await service.Received(1).GetRuntimeValue(42);
    }
}
