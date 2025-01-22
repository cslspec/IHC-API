namespace Ihc.WebApi.Exceptions;

/// <summary>
/// Exception thrown when a response is expected but is empty.
/// </summary>
public class EmptyResponseException : System.Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmptyResponseException"/> class.
    /// </summary>
    public EmptyResponseException() : base("The response is empty.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmptyResponseException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public EmptyResponseException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmptyResponseException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public EmptyResponseException(string message, System.Exception innerException) : base(message, innerException) { }
}
