using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GCore.Extensions.ArrayEx {
    public static class ArrayExtesions {
        /// <summary>
        /// Erstellt einen neuen Array als Untermänge
        /// </summary>
        /// <typeparam name="T">Arraytyp</typeparam>
        /// <param name="data">orginal Array</param>
        /// <param name="index">Anfangsindex</param>
        /// <param name="length">Länge</param>
        /// <returns></returns>
        public static T[] SubArray<T>(this T[] data, int index, int length) {
            T[] result = new T[length];
            System.Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>
        /// Clont Alle Elemente im Array in die Tiefe
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] SubArrayDeepClone<T>(this T[] data, int index, int length) {
            T[] arrCopy = new T[length];
            System.Array.Copy(data, index, arrCopy, 0, length);
            using (MemoryStream ms = new MemoryStream()) {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, arrCopy);
                ms.Position = 0;
                return (T[])bf.Deserialize(ms);
            }
        }

        /// <summary>
        /// Gibt einen Bereich zwischen den beiden Indexen zurück.
        /// ich ein Index kleiner 0, zählt er von hinten:
        /// -1 => das letzte Element
        /// -2 => das vorletzte
        /// </summary>
        public static T[] Slice<T>(this T[] source, int start, int end) {
            // Handles negative ends.
            if (end < 0) {
                end = source.Length + end;
            }
            if (start < 0) {
                start = source.Length - start;
            }
            int len = end - start;

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++) {
                res[i] = source[i + start];
            }
            return res;
        }

        /// <summary>
        /// Gibt das Element am Index zurück.
        /// ich der Index kleiner 0, zählt er von hinten:
        /// -1 => das letzte Element
        /// -2 => das vorletzte
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T Get<T>(this T[] source, int index){
            if (index >= 0)
                return source[index];
            return source[source.Length + index];
        }
    }
}
