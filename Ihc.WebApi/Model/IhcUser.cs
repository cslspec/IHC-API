namespace Ihc.WebApi.Model;

public class IhcUser
{
    public required string Username { get; init; }

    public required string Password { get; init; }

    public string? Email { get; init; }

    public required string Firstname { get; init; }

    public required string Lastname { get; init; }

    public required string Phone { get; init; }

    public required string Group { get; init; }

    public required string Project { get; init; }

    public required DateTimeOffset CreatedDate { get; init; }

    public required DateTimeOffset LoginDate { get; init; }

    public string? AuthToken { get; set; }
}
