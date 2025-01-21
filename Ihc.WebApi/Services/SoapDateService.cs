namespace Ihc.WebApi.Services
{
    public interface ISoapDateService
    {
        DateTime GetDateTime(Soap.Authentication.WSDate value);
        DateTime GetDateTime(Soap.Configuration.WSDate value);
        DateTime GetDateTime(Soap.Controller.WSDate value);
        DateTime GetDateTime(Soap.Module.WSDate value);
        DateTime GetDateTime(Soap.MessageLog.WSDate value);
        DateTime GetDateTime(Soap.Notification.WSDate value);
        DateTime GetDateTime(Soap.UserManager.WSDate value);
        DateTime GetDateTime(Soap.OpenApi.WSDate value);
        DateTime GetDateTime(Soap.TimeManager.WSDate value, DateTimeKind? dateTimeKind = null);
    }

    public class SoapDateService : ISoapDateService
    {
        private static readonly DateTimeKind DefaultDateTimeKind = DateTimeKind.Unspecified;

        public DateTime GetDateTime(Soap.Authentication.WSDate value)
        {
            return new DateTime(
                value.year,
                value.monthWithJanuaryAsOne,
                value.day,
                value.hours,
                value.minutes,
                value.seconds,
                DefaultDateTimeKind);
        }

        public DateTime GetDateTime(Soap.Configuration.WSDate value)
        {
            return new DateTime(
                value.year,
                value.monthWithJanuaryAsOne,
                value.day,
                value.hours,
                value.minutes,
                value.seconds,
                DefaultDateTimeKind);
        }

        public DateTime GetDateTime(Soap.Controller.WSDate value)
        {
            return new DateTime(
                value.year,
                value.monthWithJanuaryAsOne,
                value.day,
                value.hours,
                value.minutes,
                value.seconds,
                DefaultDateTimeKind);
        }

        public DateTime GetDateTime(Soap.Module.WSDate value)
        {
            return new DateTime(
                value.year,
                value.monthWithJanuaryAsOne,
                value.day,
                value.hours,
                value.minutes,
                value.seconds,
                DefaultDateTimeKind);
        }

        public DateTime GetDateTime(Soap.MessageLog.WSDate value)
        {
            return new DateTime(
                value.year,
                value.monthWithJanuaryAsOne,
                value.day,
                value.hours,
                value.minutes,
                value.seconds,
                DefaultDateTimeKind);
        }

        public DateTime GetDateTime(Soap.Notification.WSDate value)
        {
            return new DateTime(
                value.year,
                value.monthWithJanuaryAsOne,
                value.day,
                value.hours,
                value.minutes,
                value.seconds,
                DefaultDateTimeKind);
        }

        public DateTime GetDateTime(Soap.UserManager.WSDate value)
        {
            return new DateTime(
                value.year,
                value.monthWithJanuaryAsOne,
                value.day,
                value.hours,
                value.minutes,
                value.seconds,
                DefaultDateTimeKind);
        }

        public DateTime GetDateTime(Soap.OpenApi.WSDate value)
        {
            return new DateTime(
                value.year,
                value.monthWithJanuaryAsOne,
                value.day,
                value.hours,
                value.minutes,
                value.seconds,
                DefaultDateTimeKind);
        }

        public DateTime GetDateTime(Soap.TimeManager.WSDate value, DateTimeKind? dateTimeKind = null)
        {
            return new DateTime(
                value.year,
                value.monthWithJanuaryAsOne,
                value.day,
                value.hours,
                value.minutes,
                value.seconds,
                dateTimeKind ?? DefaultDateTimeKind);
        }
    }
}
