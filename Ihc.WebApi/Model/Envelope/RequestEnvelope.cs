using System.Xml.Serialization;

namespace Ihc.WebApi.Model.Envelope;

/// <summary>
/// Represents a SOAP request envelope containing a typed request body.
/// </summary>
/// <typeparam name="T">The type of the SOAP request body.</typeparam>
[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class RequestEnvelope<T>
{
    /// <summary>
    /// The SOAP header content.
    /// </summary>
    [XmlElement(Order = 1, IsNullable = false)]
    public string Header { get; set; }

    /// <summary>
    /// The typed SOAP request body.
    /// </summary>
    [XmlElement(Order = 2, IsNullable = false)]
    public T Body { get; set; }

    /// <summary>
    /// XML namespace declarations used when serializing the envelope.
    /// </summary>
    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Xmlns { get; set; }

    /// <summary>
    /// Initializes a request envelope with the specified body and required SOAP namespaces.
    /// </summary>
    /// <param name="body">The request body to include in the envelope.</param>
    public RequestEnvelope(T body)
        : this()
    {
        Header = string.Empty;
        Body = body;
    }

    /// <summary>
    /// Initializes an empty request envelope with the required SOAP namespaces.
    /// </summary>
    public RequestEnvelope()
    {
        Xmlns = new XmlSerializerNamespaces();
        Xmlns.Add("utcs", "utcs");
        Xmlns.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");

        Header = string.Empty;

        // NOTICE: Assume that T is a reference type and can be initialized to null.
        // If T is a value type, this will set Body to its default value.
        Body = default!;
    }
};
