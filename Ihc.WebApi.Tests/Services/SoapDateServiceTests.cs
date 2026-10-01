using Ihc.Soap.TimeManager;
using Ihc.WebApi.Services;

namespace Ihc.WebApi.Tests.Services;

[TestClass]
public class SoapDateServiceTests
{
    private readonly SoapDateService service = new();

    [TestMethod]
    public void GetDateTime_ConvertsSoapDateAndUsesUnspecifiedKindByDefault()
    {
        var value = new WSDate
        {
            year = 2025,
            monthWithJanuaryAsOne = 3,
            day = 14,
            hours = 9,
            minutes = 26,
            seconds = 53
        };

        var result = service.GetDateTime(value);

        Assert.AreEqual(new DateTime(2025, 3, 14, 9, 26, 53), result);
        Assert.AreEqual(DateTimeKind.Unspecified, result.Kind);
    }

    [TestMethod]
    public void GetDateTime_UsesRequestedDateTimeKind()
    {
        var value = new WSDate
        {
            year = 2025,
            monthWithJanuaryAsOne = 3,
            day = 14,
            hours = 9,
            minutes = 26,
            seconds = 53
        };

        var result = service.GetDateTime(value, DateTimeKind.Utc);

        Assert.AreEqual(new DateTime(2025, 3, 14, 9, 26, 53, DateTimeKind.Utc), result);
    }

    [TestMethod]
    public void GetDateTime_ThrowsForInvalidSoapDate()
    {
        var value = new WSDate
        {
            year = 2025,
            monthWithJanuaryAsOne = 13,
            day = 14
        };

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => service.GetDateTime(value));
    }
}
