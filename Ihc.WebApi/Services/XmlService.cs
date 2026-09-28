using Ihc.WebApi.Exceptions;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Ihc.WebApi.Services;

/// <summary>
/// Provides functionality for serializing and deserializing XML content.
/// </summary>
public interface IXmlService
{
    /// <summary>
    /// Serializes an object to an XML string.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize. Must be a reference type.</typeparam>
    /// <param name="x">The object instance to serialize.</param>
    /// <returns>An XML string representation of the object.</returns>
    /// <exception cref="SoapException">Thrown when an error occurs during XML serialization.</exception>
    string SerializeXml<T>(T x) where T : class;

    /// <summary>
    /// Deserializes an XML string to an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the XML into. Must be a reference type.</typeparam>
    /// <param name="xml">The XML string to deserialize.</param>
    /// <returns>An instance of the specified type, or <c>null</c> if deserialization fails gracefully.</returns>
    /// <exception cref="SoapException">Thrown when an error occurs during XML deserialization.</exception>
    T? DeserializeXml<T>(string xml) where T : class;
}

/// <summary>
/// Service for XML serialization and deserialization operations.
/// </summary>
/// <remarks>
/// This service handles the conversion of objects to and from XML format,
/// with support for custom namespace handling and attribute overrides.
/// XML is serialized with UTF-8 encoding and without declaration by default.
/// </remarks>
public class XmlService : IXmlService
{
    /// <summary>
    /// Serializes an object to an XML string.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize. Must be a reference type.</typeparam>
    /// <param name="x">The object instance to serialize.</param>
    /// <returns>An XML string representation of the object with UTF-8 encoding and indentation.</returns>
    /// <exception cref="SoapException">Thrown when an error occurs during XML serialization.</exception>
    /// <remarks>
    /// The serialization process:
    /// - Applies the "utcs" namespace to the object and its generic type arguments
    /// - Retrieves custom namespaces from the object if defined via <see cref="XmlNamespaceDeclarationsAttribute"/>
    /// - Omits the XML declaration and duplicate namespace declarations
    /// - Uses UTF-8 encoding without BOM (Byte Order Mark)
    /// </remarks>
    public string SerializeXml<T>(T x) where T : class
    {
        try
        {
            var attrs = new XmlAttributeOverrides();
            var attr = new XmlAttributes();
            var typ = new XmlTypeAttribute() { Namespace = "utcs" };
            attr.XmlType = typ;

            var genericTypes = typeof(T).GetGenericArguments();
            foreach (var genericType in genericTypes)
            {
                attrs.Add(genericType, attr);
            }

            // Retrieve namespace specification from object if it exists
            // Explicitly for the serializer - as a side effect this will also
            // cause built-in xsi and xsd namespaces to be omitted by default 
            // as we would like for simple requests.

            var xmlSerializer = new XmlSerializer(typeof(T), attrs);
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                Encoding = new UTF8Encoding(false),
                NamespaceHandling = NamespaceHandling.OmitDuplicates
            };

            using var stream = new MemoryStream();
            using var writer = XmlWriter.Create(stream, settings);
            if (typeof(T).GetProperties()
                        .Where(p => Attribute.IsDefined(p, typeof(XmlNamespaceDeclarationsAttribute)))
                        .Take(1)
                        .Select(p => p.GetValue(x))
                        .FirstOrDefault(p => true) is XmlSerializerNamespaces optNs)
            {
                xmlSerializer.Serialize(writer, x, optNs);
            }
            else
            {
                xmlSerializer.Serialize(writer, x);
            }

            writer.Flush();
            var result = Encoding.UTF8.GetString(stream.ToArray());
            return result;
        }
        catch (Exception ex)
        {
            const string message = "An error occurred during XML serialization.";
            throw new SoapException(message, CommunicationErrors.XmlSerializeError, ex);
        }
    }

    /// <summary>
    /// Deserializes an XML string to an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the XML into. Must be a reference type.</typeparam>
    /// <param name="xml">The XML string to deserialize.</param>
    /// <returns>An instance of the specified type if deserialization succeeds; otherwise, <c>null</c>.</returns>
    /// <exception cref="SoapException">Thrown when an error occurs during XML deserialization.</exception>
    /// <remarks>
    /// The deserialization process:
    /// - Applies the "utcs" namespace to the type and its generic type arguments
    /// - Attempts to reconstruct the object from the XML structure
    /// </remarks>
    public T? DeserializeXml<T>(string xml) where T : class
    {
        try
        {
            var attrs = new XmlAttributeOverrides();
            var attr = new XmlAttributes();
            var typ = new XmlTypeAttribute { Namespace = "utcs" };

            attr.XmlType = typ;

            var genericTypes = typeof(T).GetGenericArguments();
            foreach (var genericType in genericTypes)
            {
                attrs.Add(genericType, attr);
            }

            var xmlSerializer = new XmlSerializer(typeof(T), attrs, genericTypes, null, null);
            using var stream = new MemoryStream(Encoding.ASCII.GetBytes(xml));
            var result = xmlSerializer.Deserialize(stream);

            return result as T;
        }
        catch (Exception ex)
        {
            const string message = "An error occurred during XML deserialization.";
            throw new SoapException(message, CommunicationErrors.XmlDeserializeError, ex);
        }
    }
}