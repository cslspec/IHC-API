namespace Ihc.WebApi.Exceptions;

[Serializable]
public class SoapException : Exception
{
    public SoapException() : base()
    {
    }

    public SoapException(string message) : base(message)
    {
    }

    public SoapException(string message, CommunicationError error) : base(message)
    {
        Error = error;
    }

    public SoapException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public SoapException(string message, CommunicationError error, Exception innerException) : base(message, innerException)
    {
        Error = error;
    }

    public CommunicationError? Error { get; set; }
}
