namespace Ihc.WebApi.Model;

public interface IControllerConfiguration
{
    string Address { get; set; }

    string UserName { get; set; }

    string Password { get; set; }

    string? Application { get; set; }
}

public class ControllerConfiguration : IControllerConfiguration
{
    public const string Name = "controller";

    public required string Address { get; set; }

    public required string UserName { get; set; }

    public required string Password { get; set; }

    public string? Application { get; set; }
}
