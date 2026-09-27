using System.Xml.Serialization;

namespace Ihc.WebApi.Model.Envelope;

/// <summary>
/// Represents a SOAP response envelope containing a typed response body.
/// </summary>
/// <typeparam name="T">The type of the SOAP response body.</typeparam>
[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class ResponseEnvelope<T>
{
    /// <summary>Gets or sets the typed SOAP response body.</summary>
    [XmlElement(Order = 1, IsNullable = false)]
    public T Body;

    private XmlSerializerNamespaces xmlns;

    /// <summary>Gets or sets the XML namespace declarations used when serializing the envelope.</summary>
    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Xmlns
    {
        get { return xmlns; }
        set { xmlns = value; }
    }

    /// <summary>Initializes a response envelope with the specified body and required SOAP namespaces.</summary>
    /// <param name="body">The response body to include in the envelope.</param>
    public ResponseEnvelope(T body)
        : this()
    {
        Body = body;
    }

    /// <summary>Initializes an empty response envelope with the required SOAP namespaces.</summary>
    public ResponseEnvelope()
    {
        xmlns = new XmlSerializerNamespaces();
        xmlns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
        xmlns.Add("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/");
        xmlns.Add("utcs", "utcs");
        xmlns.Add("xsd", "http://www.w3.org/2001/XMLSchema");
    }
};
