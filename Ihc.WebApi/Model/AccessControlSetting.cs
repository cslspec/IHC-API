namespace Ihc.WebApi.Model;

public class AccessControlSetting
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public required bool Usb { get; set; }

    public required bool Internal { get; set; }

    public required bool External { get; set; }
}
