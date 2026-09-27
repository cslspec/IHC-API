using System.Xml.Serialization;

namespace Ihc.WebApi.Model.Envelope;

/// <summary>
/// Represents a SOAP request envelope containing a typed request body.
/// </summary>
/// <typeparam name="T">The type of the SOAP request body.</typeparam>
[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class RequestEnvelope<T>
{
    /// <summary>Gets or sets the SOAP header content.</summary>
    [XmlElement(Order = 1, IsNullable = false)]
    public string Header { get; set; }

    /// <summary>Gets or sets the typed SOAP request body.</summary>
    [XmlElement(Order = 2, IsNullable = false)]
    public T Body;

    private XmlSerializerNamespaces xmlns;

    /// <summary>Gets or sets the XML namespace declarations used when serializing the envelope.</summary>
    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Xmlns
    {
        get { return xmlns; }
        set { xmlns = value; }
    }

    /// <summary>Initializes a request envelope with the specified body and required SOAP namespaces.</summary>
    /// <param name="body">The request body to include in the envelope.</param>
    public RequestEnvelope(T body)
        : this()
    {
        Body = body;
        Header = string.Empty;
    }

    /// <summary>Initializes an empty request envelope with the required SOAP namespaces.</summary>
    public RequestEnvelope()
    {
        xmlns = new XmlSerializerNamespaces();
        xmlns.Add("utcs", "utcs");
        xmlns.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
    }
};
