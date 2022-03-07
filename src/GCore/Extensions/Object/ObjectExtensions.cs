using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCore.Extensions.Array;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using GCore.Extensions.ArrayEx;
using System.Data;

namespace GCore.Extensions.ObjectEx {
    public static class ObjectExtensions {
        /// <summary>
        /// Serialisiert das Objekt
        /// </summary>
        /// <param name="objectToSerialize"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static byte[] Serialize(this System.Object objectToSerialize, ByteArrayExtensions.DataFormatType format) {
            MemoryStream ms = new MemoryStream();
            //try {
            switch (format) {
                case ByteArrayExtensions.DataFormatType.Binary:
                    BinaryFormatter bFormatter = new BinaryFormatter();
                    bFormatter.Serialize(ms, objectToSerialize);
                    break;
                case ByteArrayExtensions.DataFormatType.Soap:
                    throw new NotImplementedException();
                    /*SoapFormatter sFormatter = new SoapFormatter();
                    sFormatter.Serialize(ms, objectToSerialize);
                    break;*/
                case ByteArrayExtensions.DataFormatType.XML:
                    XmlSerializer xFormatter = new XmlSerializer(objectToSerialize.GetType());
                    xFormatter.Serialize(ms, objectToSerialize);
                    break;
            }
            //} catch (Exception ex) { }
            return ms.ToArray();
        }

        /// <summary>
        /// Castet des Objekt zu dem Typ oder gibt den Standartwehrt zurück.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this_"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T CastToOrDefault<T>(this System.Object _this_, T defaultValue) {
            if (_this_.IsCastableTo(typeof(T)))
                return (T)_this_;
            return defaultValue;
        }

        /// <summary>
        /// Gibt an ob das Objekt zu dem Typ gecastet werden kann.
        /// </summary>
        /// <param name="_this_"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsCastableTo(this System.Object _this_, Type t) {
            Type thisType = _this_.GetType();
            return t.Equals(thisType) || thisType.IsSubclassOf(t) || t.IsAssignableFrom(thisType);
        }

        // https://stackoverflow.com/a/1130705
        public static bool IsNumber(this object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }

        public static string Dump(this System.Object this_)
        {
            return GCore.Data.Dump.ToDump(this_);
        }

        public static string Dump(this System.Object this_, Type type)
        {
            return GCore.Data.Dump.ToDump(this_, type);
        }

        public static string Dump(this System.Object this_, GCore.Data.DumpSettings settings)
        {
            return GCore.Data.Dump.ToDump(this_, settings);
        }

        public static string Dump(this System.Object this_, Type type, string name)
        {
            return GCore.Data.Dump.ToDump(this_, type, name);
        }

        public static string ToStringSafe(this System.Object this_)
        {
            if (this_ is null) return "NULL";
            return this_.ToString();
        }

        public static DataTable ToDataTableO(this System.Object this_)
        {
            var dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(object));

            foreach (var p in this_.GetType().GetProperties())
            {
                dt.Rows.Add(p.Name, p.GetValue(this_));
            }

            return dt;
        }
    }
}
