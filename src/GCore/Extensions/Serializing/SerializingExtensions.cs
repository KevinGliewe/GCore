using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;

namespace GCore.Extensions.SerializingEx {
    public static class SerializingExtensions {
        public enum Serializer {
            XML,
            PrettyXML,
            Binary,
            Soap
        }

        public static string Serialize<T>(this T @thisX, Serializer serializer) {
            switch (serializer) {
                case Serializer.XML:
                    return @thisX.SerializeXML(false);
                case Serializer.PrettyXML:
                    return @thisX.SerializeXML(true);
                case Serializer.Soap:
                    return @thisX.SerializeSoap();
                case Serializer.Binary:
                    return @thisX.SerializeBinary();
            }

            return @thisX.SerializeSoap();
        }

        public static T Deserialize<T>(this string @thisX, Serializer serializer) {
            switch (serializer) {
                case Serializer.XML:
                    return @thisX.DeserializeXML<T>();
                case Serializer.PrettyXML:
                    return @thisX.DeserializeXML<T>();
                case Serializer.Soap:
                    return @thisX.DeserializeSoap<T>();
                case Serializer.Binary:
                    return @thisX.DeserializeBinary<T>();
            }

            return @thisX.DeserializeSoap<T>();
        }

        public static string SerializeBinary<T>(this T @thisX) {
            if (@thisX == null) {
                return null;
            }

            using (MemoryStream memoryStream = new MemoryStream()) {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, @thisX);
                return Encoding.Default.GetString(memoryStream.ToArray());
            }
        }

        public static T DeserializeBinary<T>(this string @thisX) {
            if (@thisX == null) {
                return default(T);
            }

            using (MemoryStream memoryStream = new MemoryStream(Encoding.Default.GetBytes(@thisX))) {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return (T)binaryFormatter.Deserialize(memoryStream);
            }
        }

        public static string SerializeSoap<T>(this T @thisX) {
            throw new NotImplementedException();
            /*if (@thisX == null) {
                return null;
            }

            using (MemoryStream memoryStream = new MemoryStream()) {
                SoapFormatter soapFormatter = new SoapFormatter();
                soapFormatter.Serialize(memoryStream, @thisX);
                return Encoding.Default.GetString(memoryStream.ToArray());
            }*/
        }

        public static T DeserializeSoap<T>(this string @thisX) {
            throw new NotImplementedException();
            /*if (@thisX == null) {
                return default(T);
            }

            using (MemoryStream memoryStream = new MemoryStream(Encoding.Default.GetBytes(@thisX))) {
                SoapFormatter soapFormatter = new SoapFormatter();
                return (T)soapFormatter.Deserialize(memoryStream);
            }*/
        }

        public static string SerializeXML<T>(this T @thisX, bool pretty = true) {

            if (@thisX == null) {
                return null;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
            settings.Indent = false;
            settings.OmitXmlDeclaration = false;

            using (StringWriter textWriter = new StringWriter()) {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings)) {
                    serializer.Serialize(xmlWriter, @thisX);
                }
                if (pretty)
                    return textWriter.ToString().PrettyXML();
                else
                    return textWriter.ToString();
            }
        }

        public static T DeserializeXML<T>(this string @thisX) {

            if (string.IsNullOrEmpty(@thisX)) {
                return default(T);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlReaderSettings settings = new XmlReaderSettings();
            // No settings need modifying here

            using (StringReader textReader = new StringReader(@thisX)) {
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings)) {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }

        public static string PrettyXML(this string @thisX) {
            //return @thisX;
            var stringBuilder = new StringBuilder();

            var element = System.Xml.Linq.XElement.Parse(@thisX);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings)) {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }
    }
}
