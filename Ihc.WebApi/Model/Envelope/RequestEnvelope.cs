using System.Xml.Serialization;

namespace Ihc.WebApi.Model.Envelope;

/// <summary>
/// This class represents a complete SOAP request for a specific request message (which unlike this class is is auto-generated).
/// Use dotnet XmlSerializer to generate the corresponding xml
/// NOTE: Remember to configure the serializer to use utcs namespace for the T data part.
/// </summary>
/// <typeparam name="T"></typeparam>
[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class RequestEnvelope<T>
{
    [XmlElement(Order = 1, IsNullable = false)]
    public string Header { get; set; }

    [XmlElement(Order = 2, IsNullable = false)]
    public T Body;

    private XmlSerializerNamespaces xmlns;

    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Xmlns
    {
        get { return xmlns; }
        set { xmlns = value; }
    }

    public RequestEnvelope(T body)
        : this()
    {
        Body = body;
        Header = string.Empty;
    }

    public RequestEnvelope()
    {
        xmlns = new XmlSerializerNamespaces();
        xmlns.Add("utcs", "utcs");
        xmlns.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
    }
};
