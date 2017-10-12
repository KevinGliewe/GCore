using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Extensions.IConvertibleEx {
    public static class Extensions {
        /// <summary>
        /// Convertiert zu dem Typ.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T To<T>(this System.IConvertible obj) {
            return (T)Convert.ChangeType(obj, typeof(T));
        }

        /// <summary>
        /// Convertiert zu dem Typ oder gibt
        /// den Standartwehrt zurück.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ToOrDefault<T>
                     (this System.IConvertible obj) {
            try {
                return To<T>(obj);
            } catch {
                return default(T);
            }
        }

        /// <summary>
        /// Convertiert zu dem Typ oder übergibt
        /// den Standartwehrt in der newObj Variable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="newObj"></param>
        /// <returns>Gibt an ob die KOnvertierung erfolgreich war</returns>
        public static bool ToOrDefault<T>
                            (this System.IConvertible obj,
                             out T newObj) {
            try {
                newObj = To<T>(obj);
                return true;
            } catch {
                newObj = default(T);
                return false;
            }
        }
    }
}
