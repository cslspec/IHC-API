namespace Ihc.WebApi.Exceptions;

/// <summary>
/// Exception thrown when a requested item does not exist on the IHC controller.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NotFoundException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
