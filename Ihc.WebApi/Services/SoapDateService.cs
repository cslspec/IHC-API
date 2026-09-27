namespace Ihc.WebApi.Services
{
    /// <summary>
    /// Converts SOAP date values from IHC service models to <see cref="DateTime"/> values.
    /// </summary>
    public interface ISoapDateService
    {
        /// <summary>Converts an authentication-service date to a date and time.</summary>
        /// <param name="value">The SOAP date value.</param>
        /// <returns>The converted date and time with an unspecified kind.</returns>
        DateTime GetDateTime(Soap.Authentication.WSDate value);
        /// <summary>Converts a configuration-service date to a date and time.</summary>
        /// <param name="value">The SOAP date value.</param>
        /// <returns>The converted date and time with an unspecified kind.</returns>
        DateTime GetDateTime(Soap.Configuration.WSDate value);
        /// <summary>Converts a controller-service date to a date and time.</summary>
        /// <param name="value">The SOAP date value.</param>
        /// <returns>The converted date and time with an unspecified kind.</returns>
        DateTime GetDateTime(Soap.Controller.WSDate value);
        /// <summary>Converts a module-service date to a date and time.</summary>
        /// <param name="value">The SOAP date value.</param>
        /// <returns>The converted date and time with an unspecified kind.</returns>
        DateTime GetDateTime(Soap.Module.WSDate value);
        /// <summary>Converts a message-log date to a date and time.</summary>
        /// <param name="value">The SOAP date value.</param>
        /// <returns>The converted date and time with an unspecified kind.</returns>
        DateTime GetDateTime(Soap.MessageLog.WSDate value);
        /// <summary>Converts a notification-service date to a date and time.</summary>
        /// <param name="value">The SOAP date value.</param>
        /// <returns>The converted date and time with an unspecified kind.</returns>
        DateTime GetDateTime(Soap.Notification.WSDate value);
        /// <summary>Converts a user-manager date to a date and time.</summary>
        /// <param name="value">The SOAP date value.</param>
        /// <returns>The converted date and time with an unspecified kind.</returns>
        DateTime GetDateTime(Soap.UserManager.WSDate value);
        /// <summary>Converts an Open API date to a date and time.</summary>
        /// <param name="value">The SOAP date value.</param>
        /// <returns>The converted date and time with an unspecified kind.</returns>
        DateTime GetDateTime(Soap.OpenApi.WSDate value);
        /// <summary>Converts a time-manager date to a date and time.</summary>
        /// <param name="value">The SOAP date value.</param>
        /// <param name="dateTimeKind">The kind to assign, or <see langword="null"/> to use <see cref="DateTimeKind.Unspecified"/>.</param>
        /// <returns>The converted date and time.</returns>
        DateTime GetDateTime(Soap.TimeManager.WSDate value, DateTimeKind? dateTimeKind = null);
    }

    /// <summary>
    /// Converts SOAP date fields into .NET date and time values.
    /// </summary>
    public class SoapDateService : ISoapDateService
    {
        private static readonly DateTimeKind DefaultDateTimeKind = DateTimeKind.Unspecified;

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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
