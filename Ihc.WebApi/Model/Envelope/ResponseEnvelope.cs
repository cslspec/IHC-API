using System.Xml.Serialization;

namespace Ihc.WebApi.Model.Envelope;

/// <summary>
/// Represents a SOAP response envelope containing a typed response body.
/// </summary>
/// <typeparam name="T">The type of the SOAP response body.</typeparam>
[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class ResponseEnvelope<T>
{
    /// <summary>
    /// The XML namespace declarations used when serializing the envelope.
    /// </summary>
    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Xmlns { get; set; }

    /// <summary>
    /// The typed SOAP response body.
    /// </summary>
    [XmlElement(Order = 1, IsNullable = false)]
    public T Body { get; set; }

    /// <summary>
    /// Initializes a response envelope with the specified body and required SOAP namespaces.
    /// </summary>
    /// <param name="body">The response body to include in the envelope.</param>
    public ResponseEnvelope(T body)
        : this()
    {
        Body = body;
    }

    /// <summary>
    /// Initializes an empty response envelope with the required SOAP namespaces.
    /// </summary>
    public ResponseEnvelope()
    {
        Xmlns = new XmlSerializerNamespaces();
        Xmlns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
        Xmlns.Add("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/");
        Xmlns.Add("utcs", "utcs");
        Xmlns.Add("xsd", "http://www.w3.org/2001/XMLSchema");

        // NOTICE: Assume that T is a reference type and can be initialized to null.
        Body = default!;
    }
};
