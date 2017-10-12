using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Extensions.IEnumerableEx {
    public static class Extensions {
        /// <summary>
        /// Gibt einen Bereich zurück.
        /// Ich der Index kleiner 0, zählt er von hinten:
        /// -1 => das letzte Element
        /// -2 => das vorletzte
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static IEnumerable<T> Slice<T>(this IEnumerable<T> source, int? start) {
            return Slice<T>(source, start, null, null);
        }

        /// <summary>
        /// Gibt einen Bereich zurück zwischen "start" und "stop" zurück.
        /// Ich ein Index kleiner 0, zählt er von hinten:
        /// -1 => das letzte Element
        /// -2 => das vorletzte
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static IEnumerable<T> Slice<T>(this IEnumerable<T> source, int? start, int? stop) {
            return Slice<T>(source, start, stop, null);
        }

        /// <summary>
        /// Gibt einen Bereich zurück zwischen "start" und "stop" mit der Schrittweite "step" zurück.
        /// Ich ein Index kleiner 0, zählt er von hinten:
        /// -1 => das letzte Element
        /// -2 => das vorletzte
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static IEnumerable<T> Slice<T>(this IEnumerable<T> source, int? start, int? stop, int? step) {
            if (source == null) throw new ArgumentNullException("source");

            if (step == 0) throw new ArgumentException("Step cannot be zero.", "step");

            IList<T> sourceCollection = source as IList<T>;
            if (sourceCollection == null) {
                source = new List<T>(source);
                sourceCollection = source as IList<T>;
            }

            // nothing to slice
            if (sourceCollection.Count == 0) yield break;

            // set defaults for null arguments
            int stepCount = step ?? 1;
            int startIndex = start ?? ((stepCount > 0) ? 0 : sourceCollection.Count - 1);
            int stopIndex = stop ?? ((stepCount > 0) ? sourceCollection.Count : -1);

            // start from the end of the list if start is negitive
            if (start < 0) startIndex = sourceCollection.Count + startIndex;

            // end from the start of the list if stop is negitive
            if (stop < 0) stopIndex = sourceCollection.Count + stopIndex;

            // ensure indexes keep within collection bounds
            startIndex = Math.Max(startIndex, (stepCount > 0) ? 0 : int.MinValue);
            startIndex = Math.Min(startIndex, (stepCount > 0) ? sourceCollection.Count : sourceCollection.Count - 1);
            stopIndex = Math.Max(stopIndex, -1);
            stopIndex = Math.Min(stopIndex, sourceCollection.Count);

            for (int i = startIndex; (stepCount > 0) ? i < stopIndex : i > stopIndex; i += stepCount) {
                yield return sourceCollection[i];
            }

            yield break;
        }

        /// <summary>
        /// Ruft die Action für jedes Element auf.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="callback"></param>
        public static void Foreach<T>(this IEnumerable<T> source, Action<T> callback) {
            foreach (T element in source)
                callback(element);
        }

        /// <summary>
        /// Ruft die Func für jedes Element auf und
        /// gibt die Rückgabewehrte zurück.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="source"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static TR[] Foreach<T, TR>(this IEnumerable<T> source, Func<T,TR> callback) {
            List<TR> ret = new List<TR>();
            foreach (T element in source)
                ret.Add(callback(element));
            return ret.ToArray();
        }

        /// <summary>
        /// Gibt zurück ob der Wehrt enthalten ist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool ContainsVal<T>(this IEnumerable<T> source, T val) {
            foreach (T item in source)
                if (item.Equals(val))
                    return true;
            return false;
        }

        /// <summary>
        /// ToString Unterstützung um alle Unterelemente
        /// zu einem String zu konvertieren und zusammen zu fügen.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToStringPretty<T>(this IEnumerable<T> source) {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(string.Join(", ", source.Foreach(new Func<T, string>((item) => { return item.ToString(); }))));
            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// Ruft für jedes Element die Func auf und bricht beim ersten true Rückgabewehrt mit true ab.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool ForeachOneTrue<T>(this IEnumerable<T> source, Func<T, bool> callback) {
            foreach (T item in source)
                if (callback(item))
                    return true;
            return false;
        }

        /// <summary>
        /// Ruft für jedes Element die Func auf und bricht beim ersten false Rückgabewehrt mit false ab.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool ForeachAllTrue<T>(this IEnumerable<T> source, Func<T, bool> callback) {
            foreach (T item in source)
                if (!callback(item))
                    return false;
            return true;
        }

        /// <summary>
        /// Gibt das erste Element zurück bei der die Func true zurück gibt.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static T ReturnFirstTrue<T>(this IEnumerable<T> source, Func<T, bool> callback) {
            foreach (T t in source)
                if (callback(t))
                    return t;
            return default(T);
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////// NON GENERIC ///////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////

        //      _   ______  _   __   _____________   ____________  __________
        //     / | / / __ \/ | / /  / ____/ ____/ | / / ____/ __ \/  _/ ____/
        //    /  |/ / / / /  |/ /  / / __/ __/ /  |/ / __/ / /_/ // // /     
        //   / /|  / /_/ / /|  /  / /_/ / /___/ /|  / /___/ _, _// // /___   
        //  /_/ |_/\____/_/ |_/   \____/_____/_/ |_/_____/_/ |_/___/\____/  

        /// <summary>
        /// Ruft für jedes Element die Func auf und bricht beim ersten true Rückgabewehrt mit true ab.
        /// </summary>
        public static bool ForeachOneTrue(this System.Collections.IEnumerable source, Func<object, bool> callback) {
            foreach (object item in source)
                if (callback(item))
                    return true;
            return false;
        }

        /// <summary>
        /// Ruft für jedes Element die Func auf und bricht beim ersten false Rückgabewehrt mit false ab.
        /// </summary>
        public static bool ForeachAllTrue(this System.Collections.IEnumerable source, Func<object, bool> callback) {
            foreach (object item in source)
                if (!callback(item))
                    return false;
            return true;
        }

        /// <summary>
        /// Ruft die Action für jedes Element auf.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="callback"></param>
        public static void Foreach<T>(this System.Collections.IEnumerable source, Action<T> callback)
        {
            foreach (var element in source)
                callback((T)element);
        }

        /// <summary>
        /// Ruft die Func für jedes Element auf und
        /// gibt die Rückgabewehrte zurück.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="source"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static TR[] Foreach<T, TR>(this System.Collections.IEnumerable source, Func<T, TR> callback)
        {
            List<TR> ret = new List<TR>();
            foreach (var element in source)
                ret.Add(callback((T)element));
            return ret.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> MakeGeneric<T>(this System.Collections.IEnumerable source)
        {
            foreach (var element in source)
                yield return (T)element;
        }
    }
}
