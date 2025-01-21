namespace Ihc.WebApi.Model;

/// <summary>
/// Represents the configuration settings for connecting to an email server via the POP3 protocol.
/// </summary>
public class EmailSettings
{
    /// <summary>
    /// The address of the POP3 email server.
    /// Example: "pop3.example.com".
    /// </summary>
    public string? ServerAddress { get; set; }

    /// <summary>
    /// The port number to use for the POP3 server connection.
    /// Common values: 110 for non-secure, 995 for SSL/TLS connections.
    /// </summary>
    public int? ServerPortNumber { get; set; }

    /// <summary>
    /// The email address associated with the POP3 account.
    /// Example: "user@example.com".
    /// </summary>
    public string? EmailAddress { get; set; }

    /// <summary>
    /// The username used for authenticating with the POP3 server.
    /// This is often the same as the email address.
    /// </summary>
    public string? Pop3Username { get; set; }

    /// <summary>
    /// The password used for authenticating with the POP3 server.
    /// Ensure this value is handled securely and not exposed in logs or API responses.
    /// </summary>
    public string? Pop3Password { get; set; }

    /// <summary>
    /// The interval, in seconds, for polling the POP3 server to check for new emails.
    /// Example: 300 for a 5-minute polling interval.
    /// </summary>
    public int? PollInterval { get; set; }

    /// <summary>
    /// Indicates whether emails should be deleted from the server after being retrieved.
    /// Set to <c>true</c> to remove emails after retrieval; otherwise, <c>false</c>.
    /// </summary>
    public bool RemoveEmailsAfterUsage { get; set; }
}
