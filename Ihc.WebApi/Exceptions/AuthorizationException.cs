namespace Ihc.WebApi.Exceptions;

/// <summary>
/// Represents errors that occur during authorization.
/// </summary>
[Serializable]
public class AuthorizationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationException"/> class.
    /// </summary>
    public AuthorizationException() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public AuthorizationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationException"/> class with a specified error message and a communication error.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="error">The communication error associated with the exception.</param>
    public AuthorizationException(string message, CommunicationError error) : base(message)
    {
        Error = error;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public AuthorizationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Gets or sets the communication error associated with the exception.
    /// </summary>
    public CommunicationError? Error { get; set; }
}
