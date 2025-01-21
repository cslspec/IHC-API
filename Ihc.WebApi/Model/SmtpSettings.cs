namespace Ihc.WebApi.Model;

/// <summary>
/// Represents SMTP settings used for configuring email sending functionality in the LK IHC controller.
/// This model is part of the API request or response payload and defines the server details and
/// credentials required for SMTP communication.
/// </summary>
public class SmtpSettings
{
    /// <summary>
    /// The address of the SMTP server.  
    /// This is the hostname or IP address of the outgoing mail server.  
    /// Example: "smtp.example.com".
    /// </summary>
    public string? ServerAddress { get; set; }

    /// <summary>
    /// The port number for connecting to the SMTP server.  
    /// Standard port values include:  
    /// - 25: Default (non-secure)  
    /// - 465: Secure (SSL)  
    /// - 587: Secure (STARTTLS)  
    /// Example: 587.
    /// </summary>
    public int? ServerPortNumber { get; set; }

    /// <summary>
    /// The username for SMTP server authentication.  
    /// Typically, this is the email address or account name used to log into the mail server.  
    /// Example: "user@example.com".
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// The password for SMTP server authentication.  
    /// This field contains sensitive information and should be handled securely.  
    /// </summary>
    public string? Password { get; set; }
}
