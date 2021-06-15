using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Extensions.IComparableEx {
    public static class IComparableExtensions {
        /// <summary>
        /// Prüft ob der Wehrt in den angegebenen Bereich passt.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="min">Bereichsminimum</param>
        /// <param name="max">Bereichsmaximum</param>
        /// <returns></returns>
        public static T FitInto<T>(this T value, T min, T max) where T : IComparable {
            if (value.CompareTo(min) < 0)
                return min;
            if (value.CompareTo(max) > 0)
                return max;
            return value;
        }
    }
}
