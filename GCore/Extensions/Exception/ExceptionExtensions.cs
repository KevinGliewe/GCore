using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Extensions.ExceptionEx {
    public static class ExceptionExtensions {
        private static List<int> cachedExeptions = new List<int>();

        /// <summary>
        /// Gibt nur beim ersten Auftreten der Exception false zurück.
        /// Kann dazu verwendet werden um Spam zu verhindern.
        /// </summary>
        /// <param name="thisX"></param>
        /// <returns></returns>
        public static bool IsCached(this Exception @thisX) {
            int hashKey = @thisX.ToString().GetHashCode();
            if (cachedExeptions.Contains(hashKey))
                return true;

            cachedExeptions.Add(hashKey);
            return false;
        }
    }
}
