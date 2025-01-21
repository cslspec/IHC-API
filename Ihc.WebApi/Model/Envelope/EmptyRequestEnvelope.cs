using System.Xml.Serialization;

namespace Ihc.WebApi.Model.Envelope;

/// <summary>
/// This class represents an empty SOAP request message. Use dotnet XmlSerializer to generate the correspondong xml.
/// </summary>
[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class EmptyRequestEnvelope
{
    [XmlElement(Order = 1, IsNullable = false)]
    public string Header { get; set; }

    [XmlElement(Order = 2, IsNullable = false)]
    public string Body { get; set; }

    private XmlSerializerNamespaces xmlns;

    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Xmlns
    {
        get { return xmlns; }
        set { xmlns = value; }
    }

    public EmptyRequestEnvelope()
    {
        Body = string.Empty;
        Header = string.Empty;

        xmlns = new XmlSerializerNamespaces();
        xmlns.Add("utcs", "utcs");
        xmlns.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
    }
};
