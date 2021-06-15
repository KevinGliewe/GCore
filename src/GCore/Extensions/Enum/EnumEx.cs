using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Extensions.EnumEx {
    public static class EnumExtensions {
        /// <summary>
        /// Konvertiert zu dem Datentyp
        /// </summary>
        /// <typeparam name="ConvertType">
        /// Uterstützt:
        /// - int
        /// - long
        /// - short
        /// - String
        /// </typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static ConvertType To<ConvertType>(this Enum e) {
            object o = null;
            Type type = typeof(ConvertType);

            if (type == typeof(int)) {
                o = System.Convert.ToInt32(e);
            } else if (type == typeof(long)) {
                o = System.Convert.ToInt64(e);
            } else if (type == typeof(short)) {
                o = System.Convert.ToInt16(e);
            } else {
                o = System.Convert.ToString(e);
            }

            return (ConvertType)o;
        }

        /// <summary>
        /// Check to see if a flags enumeration has a specific flag set.
        /// </summary>
        /// <param key="variable">Flags enumeration to check</param>
        /// <param key="value">Flag to check for</param>
        /// <returns></returns>
        public static bool Flag<T>(this T variable, T value) {
            if (variable == null)
                return false;

            if (value == null)
                throw new ArgumentNullException("value");

            // Not as good as the .NET 4 version of this function, but should be good enough
            if (!Enum.IsDefined(variable.GetType(), value)) {
                throw new ArgumentException(string.Format(
                    "Enumeration type mismatch.  The flag is of type '{0}', was expecting '{1}'.",
                    value.GetType(), variable.GetType()));
            }

            ulong num = Convert.ToUInt64(value);
            return ((Convert.ToUInt64(variable) & num) == num);

        }
    }
}
