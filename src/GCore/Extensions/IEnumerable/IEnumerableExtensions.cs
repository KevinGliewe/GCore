
using System;
using System.Collections.Generic;
using System.Data;
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

            IList<T> sourceCollection = source.ToIList();
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

            for (int i = startIndex; (stepCount > 0) ? i <= stopIndex : i > stopIndex; i += stepCount) {
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IList<T> ToIList<T>(this IEnumerable<T> source) {
            List<T> ret = new List<T>();
            foreach(var i in source)
                ret.Add(i);
            return ret;
        }

        public static T[] ToArr<T>(this IEnumerable<T> source) {
            List<T> ret = new List<T>();
            foreach(var i in source)
                ret.Add(i);
            return ret.ToArray();
        }

        /// Ruft die Func für jedes Element auf und
        /// gibt die Rückgabewehrte zurück.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="source"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static TR[] ForeachReverse<T, TR>(this IEnumerable<T> source, Func<T, TR> callback)
        {
            List<TR> ret = new List<TR>();
            foreach (var element in source.Reverse())
                ret.Add(callback((T)element));
            return ret.ToArray();
        }

        /// Ruft die Func für jedes Element auf und
        /// gibt die Rückgabewehrte zurück.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="source"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static void ForeachReverse<T>(this IEnumerable<T> source, Action<T> callback)
        {
            foreach (var element in source.Reverse())
                callback((T)element);
        }

        // https://stackoverflow.com/a/10629938/1251423

        /// <summary>
        /// new int[] { 1, 2, 3, 4 }.GetPermutationsWithRept(2)
        /// => {1,1} {1,2} {1,3} {1,4} {2,1} {2,2} {2,3} {2,4} {3,1} {3,2} {3,3} {3,4} {4,1} {4,2} {4,3} {4,4}
        /// https://stackoverflow.com/a/10629938/1251423
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>>
            GetPermutationsWithRept<T>(this IEnumerable<T> list, int length = 0) {
            if (length == 0) length = list.Count();
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutationsWithRept(list, length - 1)
                .SelectMany(t => list,
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        /// <summary>
        /// new int[] { 1, 2, 3, 4 }.GetPermutations(2)
        /// => {1,2} {1,3} {1,4} {2,1} {2,3} {2,4} {3,1} {3,2} {3,4} {4,1} {4,2} {4,3}
        /// https://stackoverflow.com/a/10629938/1251423
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>>
            GetPermutations<T>(this IEnumerable<T> list, int length = 0) {
            if (length == 0) length = list.Count();
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(o => !t.Contains(o)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        /// <summary>
        /// new int[] { 1, 2, 3, 4 }.GetKCombsWithRept(2)
        /// => {1,1} {1,2} {1,3} {1,4} {2,2} {2,3} {2,4} {3,3} {3,4} {4,4}
        /// https://stackoverflow.com/a/10629938/1251423
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>>
            GetKCombsWithRept<T>(this IEnumerable<T> list, int length = 0) where T : IComparable {
            if (length == 0) length = list.Count();
            if (length == 1) return list.Select(t => new T[] { t });
            return GetKCombsWithRept(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) >= 0),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        /// <summary>
        /// new int[] { 1, 2, 3, 4 }.GetKCombs(2)
        /// {1,2} {1,3} {1,4} {2,3} {2,4} {3,4}
        /// https://stackoverflow.com/a/10629938/1251423
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>>
            GetKCombs<T>(this IEnumerable<T> list, int length = 0) where T : IComparable {
            if (length == 0) length = list.Count();
            if (length == 1) return list.Select(t => new T[] { t });
            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> self)
        {
            var properties = typeof(T).GetProperties();

            var dataTable = new DataTable();
            foreach (var info in properties)
                dataTable.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) 
                    ?? info.PropertyType);

            foreach (var entity in self)
                dataTable.Rows.Add(properties.Select(p => p.GetValue(entity)).ToArray());

            return dataTable;
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

        /// <summary>
        /// Returns true if there is at least one item in it.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool Any(this System.Collections.IEnumerable source)
        {
            if (source is null) return false;
            foreach (var i in source)
                return true;
            return false;
        }


        public static IEnumerable<Tuple<T, int, bool>> IterIndexLast<T>(this IEnumerable<T> source)
        {
            var arr = source.ToArray();

            for (int i = 0; i < arr.Length; i++)
                yield return new Tuple<T, int, bool>(arr[i], i, i == arr.Length - 1);
        }
    }
}
