using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.CodeDom;
using System.CodeDom.Compiler;
using GCore.Data.Coding;

namespace GCore.Extensions.StringEx {
    public static class StringExtensions {
        public static string ToLiteral(this string data) {
            using (var writer = new System.IO.StringWriter()) {
                using (var provider = CodeDomProvider.CreateProvider("CSharp")) {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(data), writer, null);
                    return writer.ToString();
                }
            }
        }

        public static string Escape(this string data) {
            return data.ToLiteral();
        }

        /// <summary>
        /// Konvertiert den String in einen Byte Array mithilfe der UTF8 Codierung.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string data) {
            return Encoding.UTF8.GetBytes(data);
        }
        /// <summary>
        /// Compares the string against a given pattern.
        /// </summary>
        /// <param key="str">The string.</param>
        /// <param key="wildcard">The wildcard, where "*" means any sequence of characters, and "?" means any single character.</param>
        /// <returns><c>true</c> if the string matches the given pattern; otherwise <c>false</c>.</returns>
        public static bool Like(this string str, string wildcard)
        {
            return new Regex(
                "^" + Regex.Escape(wildcard).Replace(@"\*", ".*").Replace(@"\?", ".") + "$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            ).IsMatch(str);
        }

        /// <summary>
        /// Gibt den MD5 Hash zurück
        /// </summary>
        /// <param name="TextToHash"></param>
        /// <returns></returns>
        public static string GetMD5Hash(this string TextToHash) {
            //Prüfen ob Daten übergeben wurden.
            if ((TextToHash == null) || (TextToHash.Length == 0)) {
                return string.Empty;
            }

            //MD5 Hash aus dem String berechnen. Dazu muss der string in ein Byte[]
            //zerlegt werden. Danach muss das Resultat wieder zurück in ein string.
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(TextToHash);
            byte[] result = md5.ComputeHash(textToHash);

            return System.BitConverter.ToString(result);
        }

        public static int GetDeterministicHashCode(this string str)
        {
            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }

        /// <summary>
        /// Filtert einzelne Zeichen aus dem String.
        /// Kann im Blacklist und Whitelist Modus betrieben werden:
        /// Whitelist-Modus:
        ///   whitelist: true
        ///   Es werden nur Zeichen übernommen,
        ///   welche im filter enthalten sind.
        /// Blacklist-Modus:
        ///   whitelist: false
        ///   Es werden nur Zeichen übernommen,
        ///   welche NICHT im filter enthalten sind.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="filter"></param>
        /// <param name="whitelist"></param>
        /// <returns></returns>
        public static string Filter(this string str, char[] filter, bool whitelist) {
            IEnumerable<char> chars;

            if (whitelist)
                chars = str.Where(c => filter.Contains(c));
            else
                chars = str.Where(c => !filter.Contains(c));

            return new string(chars.ToArray());
        }

        #region Truncate
        /// <summary>
        /// Schneidet den String ab einer bestimmten Länge ab und ersetzt die letzten Zeichen mit "..."
        /// </summary>
        /// <param key="maxLength">total length of characters to maintain before the truncate happens</param>
        /// <returns>truncated string</returns>
        public static string Truncate(this string text, int maxLength) {
            // replaces the truncated string to a ...
            const string suffix = "...";
            string truncatedString = text;

            if (maxLength <= 0) return truncatedString;
            int strLength = maxLength - suffix.Length;

            if (strLength <= 0) return truncatedString;

            if (text == null || text.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();
            truncatedString += suffix;
            return truncatedString;
        }
        #endregion

        #region RepeatToLen
        /// <summary>
        /// Wiederholt den String um die vorgegebene Länge zu erreichen.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string RepeatToLen(this string text, int len) {
            string tmp = text;
            while (tmp.Length < len)
                tmp += text;
            return tmp.Substring(0, len);
        }
        #endregion

        #region Fill
        public enum TextAlighnment {
            Left,
            Right,
            Center
        }

        /// <summary>
        /// Füllt den String bis zu der vorgegebenen länge mit Leerzeichen auf.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="len"></param>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public static string Fill(this string text, int len, TextAlighnment alignment = TextAlighnment.Left) {
            if (text.Length > len)
                return text.Truncate(len);
            if (text.Length == len)
                return text;
            if (alignment == TextAlighnment.Left)
                return text + " ".RepeatToLen(len - text.Length);
            else if(alignment == TextAlighnment.Right)
                return " ".RepeatToLen(len - text.Length) + text;

            int spaceToFill = len - text.Length;
            int spaceToFillR = spaceToFill/ 2;
            int spaceToFillL = spaceToFill - spaceToFillR;

            return " ".RepeatToLen(spaceToFillL) + text + " ".RepeatToLen(spaceToFillR);
        }
        #endregion

        public static string Encrypt(this string str, string password = null) {
            return GCore.Data.Crypto.EncDec.Encrypt(str, (password != null) ? password : GCore.Data.Crypto.EncDec.GeneratePassword());
        }

        public static string Decrypt(this string str, string password = null) {
            return GCore.Data.Crypto.EncDec.Decrypt(str, (password != null) ? password : GCore.Data.Crypto.EncDec.GeneratePassword());
        }

        /// <summary>
        /// Creates a deterministic Guid based on a String and a namespace.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="namespaceId"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static Guid GuidCreate(this string self, Guid? namespaceId = null, int version = 5)
        {
            return GuidUtility.Create(namespaceId ?? GuidUtility.GCoreNamespace, self, version);
        }
    }
}
