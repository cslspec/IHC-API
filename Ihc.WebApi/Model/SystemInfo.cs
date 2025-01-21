namespace Ihc.WebApi.Model;

/// <summary>
/// Represents system information for the LK IHC controller.
/// This model is used in API responses to provide detailed metadata
/// about the controller's hardware, software, and operational state.
/// </summary>
public class SystemInfo
{
    /// <summary>
    /// Gets the uptime of the LK IHC controller.
    /// Represents the duration for which the controller has been running without interruption.
    /// </summary>
    public required Uptime Uptime { get; init; }

    /// <summary>
    /// Gets the current system time on the LK IHC controller.
    /// </summary>
    public required DateTime Time { get; init; }

    /// <summary>
    /// Gets the serial number of the LK IHC controller.
    /// This value uniquely identifies the hardware unit.
    /// </summary>
    public required string SerialNumber { get; init; }

    /// <summary>
    /// Gets the production date of the LK IHC controller.
    /// Represents when the unit was manufactured.
    /// </summary>
    public required DateOnly ProductionDate { get; init; }

    /// <summary>
    /// Gets the branding information of the LK IHC controller.
    /// Indicates the brand under which the controller is marketed.
    /// </summary>
    public required string Brand { get; init; }

    /// <summary>
    /// Gets the software version currently installed on the LK IHC controller.
    /// Example: "2.7.220".
    /// </summary>
    public required string SoftwareVersion { get; init; }

    /// <summary>
    /// Gets the hardware version of the LK IHC controller.
    /// Indicates the revision or version of the physical hardware.
    /// </summary>
    public required string HardwareVersion { get; init; }

    /// <summary>
    /// Gets the date of the installed software on the LK IHC controller.
    /// This represents the build or release date of the software.
    /// </summary>
    public required DateOnly SoftwareDate { get; init; }

    /// <summary>
    /// Gets the IO version of the LK IHC controller.
    /// Also referred to as the "date line version" for input/output modules.
    /// Example: "0.0.20".
    /// </summary>
    public required string IoVersion { get; init; }

    /// <summary>
    /// Gets the RF version of the LK IHC controller.
    /// Represents the software version of the RF (radio frequency) module.
    /// Example: "1.01a".
    /// </summary>
    public required string RfVersion { get; init; }

    /// <summary>
    /// Gets the serial number of the RF module.
    /// This uniquely identifies the radio frequency hardware component.
    /// </summary>
    public required string RFModuleSerialNumber { get; init; }

    /// <summary>
    /// Indicates whether the LK IHC controller is running an application without a viewer.
    /// A value of <c>true</c> means the application does not include a viewer interface; otherwise, <c>false</c>.
    /// </summary>
    public required bool ApplicationIsWithoutViewer { get; init; }
}
