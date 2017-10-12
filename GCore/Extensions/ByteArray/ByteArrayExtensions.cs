using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace GCore.Extensions.ArrayEx {
    public static class ByteArrayExtensions {
        public enum DataFormatType {
            Binary,
            Soap,
            XML
        }

        /// <summary>
        /// Konvertiert in einen UTF8 String
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToUTF8String(this byte[] data) {
            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// Deserialisiert die Daten zu einem Object
        /// </summary>
        /// <param name="data"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static object DeSerialize(this byte[] data, DataFormatType format) {
            MemoryStream ms = new MemoryStream(data);
            object objectToSerialize = null;
            try {
                switch (format) {
                    case DataFormatType.Binary:
                        BinaryFormatter bFormatter = new BinaryFormatter();
                        objectToSerialize = bFormatter.Deserialize(ms);
                        break;
                    case DataFormatType.Soap:
                        throw new NotImplementedException();
                        //SoapFormatter sFormatter = new SoapFormatter();
                        //objectToSerialize = sFormatter.Deserialize(ms);
                        //break;
                    case DataFormatType.XML:
                        throw new NotImplementedException();
                        //XmlSerializer xFormatter = new XmlSerializer();
                        //objectToSerialize = xFormatter.Deserialize(ms);
                        //break;
                }

            #pragma warning disable 0168
            } catch (Exception ex) { }
            #pragma warning restore 0168

            ms.Close();
            return objectToSerialize;
        }

        /// <summary>
        /// Fügt den übergebenen Array ab.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static byte[] AppendByteArray(this byte[] data, byte[] a) {
            byte[] c = new byte[a.Length + data.Length]; // just one array allocation
            Buffer.BlockCopy(a, 0, c, data.Length, a.Length);
            Buffer.BlockCopy(data, 0, c, 0, data.Length);
            return c;
        }

        /// <summary>
        /// Schreibt die Daten in eine Datei.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool SaveToFile(this byte[] data, string file) {
            try {

                // Open file for reading
                System.IO.FileStream _FileStream = new System.IO.FileStream(file, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from a byte array.
                _FileStream.Write(data, 0, data.Length);
                // close file stream
                _FileStream.Close();

                return true;
            } catch (Exception _Exception) {
                // Error
                //Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
                Logging.Log.Exception("Cannot safe data to File", _Exception);
            }

            // error occured, return false
            return false;
        }

        /// <summary>
        /// Gibt den Hash-Wehrt der Array-Daten zurück.
        /// </summary>
        /// <param name="this_"></param>
        /// <returns></returns>
        public static string GetHash(this byte[] this_)
        {
            string hash;
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                hash = Convert.ToBase64String(sha1.ComputeHash(this_));
            }
            return hash;
        }
    }
}
