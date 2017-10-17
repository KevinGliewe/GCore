using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Extensions.IDictionaryEx {
    public static class IDictionaryExtensions {
        /// <summary>
        /// Gibt den Wehrt für den Key zurück oder den Standartwehrt,
        /// sollte er nicht im Dictionary sein.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="_this_"></param>
        /// <param name="key"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static TVal GetOrDefault<TKey, TVal>(this IDictionary<TKey, TVal> _this_, TKey key, TVal defaultVal) {
            if (_this_.ContainsKey(key))
                return _this_[key];
            return defaultVal;
        }
    }
}
