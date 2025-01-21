using System.Xml.Serialization;

namespace Ihc.WebApi.Model.Envelope;

/// <summary>
/// This class represents a complete SOAP response for a specific response message (which unlike this class is is auto-generated).
/// Use dotnet XmlSerializer to generate the corresponding xml.
/// NOTE: Remember to configure the serializer to use utcs namespace for the T data part.
/// </summary>
/// <typeparam name="T"></typeparam>
[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class ResponseEnvelope<T>
{
    [XmlElement(Order = 1, IsNullable = false)]
    public T Body;

    private XmlSerializerNamespaces xmlns;

    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Xmlns
    {
        get { return xmlns; }
        set { xmlns = value; }
    }

    public ResponseEnvelope(T body)
        : this()
    {
        Body = body;
    }

    public ResponseEnvelope()
    {
        xmlns = new XmlSerializerNamespaces();
        xmlns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
        xmlns.Add("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/");
        xmlns.Add("utcs", "utcs");
        xmlns.Add("xsd", "http://www.w3.org/2001/XMLSchema");
    }
};
