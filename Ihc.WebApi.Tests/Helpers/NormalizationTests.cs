using Ihc.WebApi.Extensions;
using Ihc.WebApi.Util;

namespace Ihc.WebApi.Tests.Helpers;

[TestClass]
public class NormalizationTests
{
    [TestMethod]
    public void StringClean_TrimsNonWhitespaceValues()
    {
        Assert.AreEqual("value", "  value \t".Clean());
    }

    [TestMethod]
    public void StringClean_ReturnsNullForNullEmptyAndWhitespaceValues()
    {
        Assert.IsNull(((string?)null).Clean());
        Assert.IsNull(string.Empty.Clean());
        Assert.IsNull(" \t\r\n".Clean());
    }

    [TestMethod]
    public void Int_ReturnsPositiveValues()
    {
        Assert.AreEqual(1, Clean.Int(1));
        Assert.AreEqual(int.MaxValue, Clean.Int(int.MaxValue));
    }

    [TestMethod]
    public void Int_ReturnsNullForZeroAndNegativeValues()
    {
        Assert.IsNull(Clean.Int(0));
        Assert.IsNull(Clean.Int(-1));
        Assert.IsNull(Clean.Int(int.MinValue));
    }
}
