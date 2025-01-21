namespace Ihc.WebApi.Model;

/// <summary>
/// Represents the network settings for the LK IHC controller.  
/// This model is part of the API payload and specifies the controller's
/// IP configuration and network ports.
/// </summary>
public class NetworkSetting
{
    /// <summary>
    /// The IP address assigned to the LK IHC controller.  
    /// Example: "192.168.1.100".
    /// </summary>
    public required string IpAddress { get; set; }

    /// <summary>
    /// The subnet mask for the LK IHC controller.  
    /// Defines the network range to which the controller belongs.  
    /// Example: "255.255.255.0".
    /// </summary>
    public required string Netmask { get; set; }

    /// <summary>
    /// The default gateway address for the LK IHC controller.  
    /// Specifies the IP address of the router or gateway used for external network communication.  
    /// Example: "192.168.1.1".
    /// </summary>
    public required string Gateway { get; set; }

    /// <summary>
    /// The port number used for HTTP communication.  
    /// This is the non-secure web server port.  
    /// Example: 80.
    /// </summary>
    public required int HttpPort { get; set; }

    /// <summary>
    /// The port number used for HTTPS communication.  
    /// This is the secure web server port.  
    /// Example: 443.
    /// </summary>
    public required int HttpsPort { get; set; }
}
