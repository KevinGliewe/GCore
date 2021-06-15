using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Extensions.FloatEx {
    /// <summary>
    /// Float Extensions
    /// </summary>
    public static class FloatExtensions {
        #region PercentageOf calculations

        /// <summary>
        /// Toes the percent.
        /// </summary>
        /// <param key="value">The value.</param>
        /// <param key="percentOf">The percent of.</param>
        /// <returns></returns>
        public static decimal PercentageOf(this float value, int percentOf) {
            return (decimal)(value / percentOf * 100);
        }
        /// <summary>
        /// Toes the percent.
        /// </summary>
        /// <param key="value">The value.</param>
        /// <param key="percentOf">The percent of.</param>
        /// <returns></returns>
        public static decimal PercentageOf(this float value, float percentOf) {
            return (decimal)(value / percentOf * 100);
        }
        /// <summary>
        /// Toes the percent.
        /// </summary>
        /// <param key="value">The value.</param>
        /// <param key="percentOf">The percent of.</param>
        /// <returns></returns>
        public static decimal PercentageOf(this float value, double percentOf) {
            return (decimal)(value / percentOf * 100);
        }
        /// <summary>
        /// Toes the percent.
        /// </summary>
        /// <param key="value">The value.</param>
        /// <param key="percentOf">The percent of.</param>
        /// <returns></returns>
        public static decimal PercentageOf(this float value, long percentOf) {
            return (decimal)(value / percentOf * 100);
        }

        #endregion
    }
}
