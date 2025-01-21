namespace Ihc.WebApi.Model;

public interface IAuthToken
{
    Guid Id { get; init; }

    DateTime LoginTime { get; init; }

    DateTime? LogoutTime { get; set; }

    DateTime ExpireTime { get; init; }

    string? Token { get; set; }

    IhcUser User { get; init; }
}

public class AuthToken : IAuthToken
{
    public required Guid Id { get; init; }

    public required DateTime LoginTime { get; init; }

    public DateTime? LogoutTime { get; set; }

    public required DateTime ExpireTime { get; init; }

    public string? Token { get; set; }

    public required IhcUser User { get; init; }
}
