namespace Ihc.WebApi.Exceptions;

[Serializable]
public class AuthorizationException : Exception
{
    public AuthorizationException() : base()
    {
    }

    public AuthorizationException(string message) : base(message)
    {
    }

    public AuthorizationException(string message, CommunicationError error) : base(message)
    {
        Error = error;
    }

    public AuthorizationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public CommunicationError? Error { get; set; }
}
