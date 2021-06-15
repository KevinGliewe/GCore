using System;
using System.Collections.Generic;

namespace GCore.Extensions.IListEx
{
    public static class IListExtension
    {
        public static bool AddOnce<T>(this IList<T> @this, T item) {
            if (@this.Contains(item))
                return false;
            @this.Add(item);
            return true;
        }

        public static T GetOrDefault<T>(this IList<T> @this, int index, T default_ = default(T)) {
            lock(@this) {
                if (@this.IsInRange(index))
                    return @this[index];
                return default_;
            }
        }

        public static bool IsInRange<T>(this IList<T> @this, int index) {
            return index >= 0 && index < @this.Count;
        }
    }
}
