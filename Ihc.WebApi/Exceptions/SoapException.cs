namespace Ihc.WebApi.Exceptions;

/// <summary>
/// Represents errors that occur during SOAP communication.
/// </summary>
[Serializable]
public class SoapException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoapException"/> class.
    /// </summary>
    public SoapException() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SoapException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public SoapException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SoapException"/> class with a specified error message and communication error.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="error">The communication error associated with the exception.</param>
    public SoapException(string message, CommunicationError error) : base(message)
    {
        Error = error;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SoapException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public SoapException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SoapException"/> class with a specified error message, communication error, and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="error">The communication error associated with the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public SoapException(string message, CommunicationError error, Exception innerException) : base(message, innerException)
    {
        Error = error;
    }

    /// <summary>
    /// Gets or sets the communication error associated with the exception.
    /// </summary>
    public CommunicationError? Error { get; set; }
}
