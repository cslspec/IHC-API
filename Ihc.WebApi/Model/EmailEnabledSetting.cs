namespace Ihc.WebApi.Model;

/// <summary>
/// Represents the email notification settings.
/// </summary>
public class EmailEnabledSetting
{
    /// <summary>
    /// Indicates whether email notifications are enabled.
    /// </summary>
    public required bool IsEmailEnabled { get; set; }
}
