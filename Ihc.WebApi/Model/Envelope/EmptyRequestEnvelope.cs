using System.Xml.Serialization;

namespace Ihc.WebApi.Model.Envelope;

/// <summary>
/// Represents a SOAP envelope with an empty request body.
/// </summary>
[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class EmptyRequestEnvelope
{
    /// <summary>Gets or sets the SOAP header content.</summary>
    [XmlElement(Order = 1, IsNullable = false)]
    public string Header { get; set; }

    /// <summary>Gets or sets the empty SOAP body content.</summary>
    [XmlElement(Order = 2, IsNullable = false)]
    public string Body { get; set; }

    private XmlSerializerNamespaces xmlns;

    /// <summary>Gets or sets the XML namespace declarations used when serializing the envelope.</summary>
    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Xmlns
    {
        get { return xmlns; }
        set { xmlns = value; }
    }

    /// <summary>Initializes an empty SOAP request envelope with the required namespaces.</summary>
    public EmptyRequestEnvelope()
    {
        Body = string.Empty;
        Header = string.Empty;

        xmlns = new XmlSerializerNamespaces();
        xmlns.Add("utcs", "utcs");
        xmlns.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
    }
};
