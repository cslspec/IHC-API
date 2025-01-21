namespace Ihc.WebApi.Exceptions;

/**
 * IHC communication related exception with a HTTP/IHC/Communication error code.
 */
public sealed class ErrorWithCodeException : Exception
{
    public readonly int ErrorCode;

    public ErrorWithCodeException()
    {
    }

    public ErrorWithCodeException(int errorCode, string message)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public ErrorWithCodeException(int errorCode, string message, Exception inner)
        : base(message, inner)
    {
        ErrorCode = errorCode;
    }

    public override string ToString() => ErrorCode + " : " + Message;
};
