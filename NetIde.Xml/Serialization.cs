using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml
{
    public static class Serialization
    {
        public static T DeserializeXml<T>(string xml)
        {
            if (xml == null)
                throw new ArgumentNullException("xml");

            using (var reader = new StringReader(xml))
            {
                return Deserialize<T>(reader);
            }
        }

        public static T Deserialize<T>(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            using (var stream = File.OpenRead(fileName))
            {
                return Deserialize<T>(stream);
            }
        }

        public static T Deserialize<T>(Stream stream)
        {
            return (T)CreateSerializer<T>().Deserialize(stream);
        }

        public static T Deserialize<T>(TextReader reader)
        {
            return (T)CreateSerializer<T>().Deserialize(reader);
        }

        private static XmlSerializer CreateSerializer<T>()
        {
            var serializer = new XmlSerializer(typeof(T));

            serializer.UnknownAttribute += serializer_UnknownAttribute;
            serializer.UnknownElement += serializer_UnknownElement;
            serializer.UnknownNode += serializer_UnknownNode;
            serializer.UnreferencedObject += serializer_UnreferencedObject;

            return serializer;
        }

        static void serializer_UnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        {
            throw new SerializationException("Unreferenced object");
        }

        static void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            throw new SerializationException("Unknown node");
        }

        static void serializer_UnknownElement(object sender, XmlElementEventArgs e)
        {
            throw new SerializationException("Unknown element");
        }

        static void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            throw new SerializationException("Unknown attribute");
        }
    }
}
