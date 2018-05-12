﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GCore.Extensions.SerializingEx;
using GCore.Extensions.IEnumerableEx;
using System.Threading.Tasks;

namespace GCore.Extensions.FileInfoEx {
    public static class FileInfoExtensions {

        #region Sync
        /// <summary>
        /// Schreibt den Strine in die Datei.
        /// </summary>
        /// <param name="thisX"></param>
        /// <param name="content"></param>
        public static void SetString(this FileInfo @thisX, string content) {
            FileStream fileStream = @thisX.OpenWrite();
            using(StreamWriter streamWriter = new StreamWriter(fileStream))
                streamWriter.Write(content);
        }

        /// <summary>
        /// Liest die Datei ein und gibt sie als String zurück.
        /// </summary>
        public static string GetString(this FileInfo @thisX) {
            FileStream fileStream = @thisX.OpenRead();
            using (StreamReader streamReader = new StreamReader(fileStream))
                return streamReader.ReadToEnd();
        }

        /// <summary>
        /// Serialisiert das Objekt und schreibt es in die Datei.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisX"></param>
        /// <param name="content"></param>
        /// <param name="serializer"></param>
        public static void SetObject<T>(this FileInfo @thisX, T content, SerializingExtensions.Serializer serializer) {
            @thisX.SetString(content.Serialize(serializer));
        }

        /// <summary>
        /// Liest die Datei ein und Deserialisiert den Inhalt zu einem Objekt.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisX"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public static T GetObject<T>(this FileInfo @thisX, SerializingExtensions.Serializer serializer) {
            return @thisX.GetString().Deserialize<T>(serializer);
        }

        /// <summary>
        /// Serialisiert das Objekt mithilfe das "BinaryFormatterr"
        /// und schreibt es in die Datei.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisX"></param>
        /// <param name="content"></param>
        public static void SetObjectBinary<T>(this FileInfo @thisX, T content) {
            @thisX.SetString(content.SerializeBinary());
        }

        /// <summary>
        /// Liest die Datei ein und Deserialisiert
        /// den Inhalt mithilfe des "BinaryFormatterr"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisX"></param>
        /// <returns></returns>
        public static T GetObjectBinary<T>(this FileInfo @thisX) {
            return @thisX.GetString().DeserializeBinary<T>();
        }

        /// <summary>
        /// Serialisiert das Objekt mithilfe das "SoapFormatterr"
        /// und schreibt es in die Datei.
        /// </summary>
        public static void SetObjectSoap<T>(this FileInfo @thisX, T content) {
            @thisX.SetString(content.SerializeSoap());
        }

        /// <summary>
        /// Liest die Datei ein und Deserialisiert
        /// den Inhalt mithilfe des "SoapFormatterr"
        /// </summary>
        public static T GetObjectSoap<T>(this FileInfo @thisX) {
            return @thisX.GetString().DeserializeSoap<T>();
        }

        public static void SetObjectXML<T>(this FileInfo @thisX, T content) {
            @thisX.SetString(content.SerializeXML());
        }

        public static T GetObjectXML<T>(this FileInfo @thisX) {
            return @thisX.GetString().DeserializeXML<T>();
        }
        #endregion

        #region Async
        /// <summary>
        /// Schreibt den Strine in die Datei.
        /// </summary>
        /// <param name="thisX"></param>
        /// <param name="content"></param>
        public static async Task SetStringAsync(this FileInfo @thisX, string content) {
            FileStream fileStream = @thisX.OpenWrite();
            using(StreamWriter streamWriter = new StreamWriter(fileStream))
                await streamWriter.WriteAsync(content);
        }

        /// <summary>
        /// Liest die Datei ein und gibt sie als String zurück.
        /// </summary>
        public static async Task<string> GetStringAsync(this FileInfo @thisX) {
            FileStream fileStream = @thisX.OpenRead();
            using(StreamReader streamReader = new StreamReader(fileStream))
                return await streamReader.ReadToEndAsync();
        }

        /// <summary>
        /// Serialisiert das Objekt und schreibt es in die Datei.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisX"></param>
        /// <param name="content"></param>
        /// <param name="serializer"></param>
        public static void SetObjectAsync<T>(this FileInfo @thisX, T content, SerializingExtensions.Serializer serializer) {
            @thisX.SetStringAsync(content.Serialize(serializer));
        }

        /// <summary>
        /// Liest die Datei ein und Deserialisiert den Inhalt zu einem Objekt.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisX"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public static async Task<T> GetObjectAsync<T>(this FileInfo @thisX, SerializingExtensions.Serializer serializer) {
            return (await @thisX.GetStringAsync()).Deserialize<T>(serializer);
        }

        /// <summary>
        /// Serialisiert das Objekt mithilfe das "BinaryFormatterr"
        /// und schreibt es in die Datei.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisX"></param>
        /// <param name="content"></param>
        public static Task SetObjectBinaryAsync<T>(this FileInfo @thisX, T content) {
            return @thisX.SetStringAsync(content.SerializeBinary());
        }

        /// <summary>
        /// Liest die Datei ein und Deserialisiert
        /// den Inhalt mithilfe des "BinaryFormatterr"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisX"></param>
        /// <returns></returns>
        public static async Task<T> GetObjectBinaryAsync<T>(this FileInfo @thisX) {
            return (await @thisX.GetStringAsync()).DeserializeBinary<T>();
        }

        /// <summary>
        /// Serialisiert das Objekt mithilfe das "SoapFormatterr"
        /// und schreibt es in die Datei.
        /// </summary>
        public static Task SetObjectSoapAsync<T>(this FileInfo @thisX, T content) {
            return @thisX.SetStringAsync(content.SerializeSoap());
        }

        /// <summary>
        /// Liest die Datei ein und Deserialisiert
        /// den Inhalt mithilfe des "SoapFormatterr"
        /// </summary>
        public static async Task<T> GetObjectSoapAsync<T>(this FileInfo @thisX) {
            return (await @thisX.GetStringAsync()).DeserializeSoap<T>();
        }

        public static Task SetObjectXMLAsync<T>(this FileInfo @thisX, T content) {
            return @thisX.SetStringAsync(content.SerializeXML());
        }

        public static async Task<T> GetObjectXMLAsync<T>(this FileInfo @thisX) {
            return (await @thisX.GetStringAsync()).DeserializeXML<T>();
        }
        #endregion

        public static string GetFilenameWithoutExtension(this FileInfo @thisX) {
            string[] parts = @thisX.Name.Split('.');
            if (parts.Length < 2)
                return parts[0];
            return string.Join(".", parts.Slice(0, -1));
        }
    }
}
