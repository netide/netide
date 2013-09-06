using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace NetIde.Xml
{
    public static class XmlValidation
    {
        public static void Validate(string fileName, string schemaFileName, string targetNamespace)
        {
            if (schemaFileName == null)
                throw new ArgumentNullException("schemaFileName");

            using (var schema = File.OpenRead(schemaFileName))
            {
                Validate(fileName, schema, targetNamespace);
            }
        }

        public static void Validate(string fileName, Stream schema, string targetNamespace)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");
            if (schema == null)
                throw new ArgumentNullException("schema");
            if (targetNamespace == null)
                throw new ArgumentNullException("targetNamespace");

            using (var reader = XmlReader.Create(fileName, CreateSettings(schema, targetNamespace)))
            {
                while (reader.Read()) ;
            }
        }

        public static void Validate(Stream file, Stream schema, string targetNamespace)
        {
            if (file == null)
                throw new ArgumentNullException("file");
            if (schema == null)
                throw new ArgumentNullException("schema");
            if (targetNamespace == null)
                throw new ArgumentNullException("targetNamespace");

            using (var reader = XmlReader.Create(file, CreateSettings(schema, targetNamespace)))
            {
                while (reader.Read()) ;
            }
        }

        private static XmlReaderSettings CreateSettings(Stream schema, string targetNamespace)
        {
            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                ValidationFlags = XmlSchemaValidationFlags.ProcessInlineSchema | XmlSchemaValidationFlags.ProcessSchemaLocation | XmlSchemaValidationFlags.ReportValidationWarnings
            };

            using (var reader = XmlReader.Create(schema))
            {
                settings.Schemas.Add(targetNamespace, reader);
            }

            settings.ValidationEventHandler += (s, e) =>
            {
                throw new XmlValidationException(e.Message);
            };

            return settings;
        }
    }
}
