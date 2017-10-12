using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Extensions.IntEx {
    /// <summary>
    /// Integer Extensions
    /// </summary>
    public static class IntExtensions {
        #region PercentageOf calculations

        /// <summary>
        /// The numbers percentage
        /// </summary>
        /// <param key="number">The number.</param>
        /// <param key="percent">The percent.</param>
        /// <returns>The result</returns>
        public static decimal PercentageOf(this int number, int percent) {
            return (decimal)(number * percent / 100);
        }

        /// <summary>
        /// Percentage of the number.
        /// </summary>
        /// <param key="percent">The percent</param>
        /// <param key="number">The Number</param>
        /// <returns>The result</returns>
        public static decimal PercentOf(this int position, int total) {
            decimal result = 0;
            if (position > 0 && total > 0)
                result = (decimal)position / (decimal)total * 100;
            return result;
        }
        public static decimal PercentOf(this int? position, int total) {
            if (position == null) return 0;

            decimal result = 0;
            if (position > 0 && total > 0)
                result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }

        /// <summary>
        /// The numbers percentage
        /// </summary>
        /// <param key="number">The number.</param>
        /// <param key="percent">The percent.</param>
        /// <returns>The result</returns>
        public static decimal PercentageOf(this int number, float percent) {
            return (decimal)(number * percent / 100);
        }

        /// <summary>
        /// Percentage of the number.
        /// </summary>
        /// <param key="percent">The percent</param>
        /// <param key="number">The Number</param>
        /// <returns>The result</returns>
        public static decimal PercentOf(this int position, float total) {
            decimal result = 0;
            if (position > 0 && total > 0)
                result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }

        /// <summary>
        /// The numbers percentage
        /// </summary>
        /// <param key="number">The number.</param>
        /// <param key="percent">The percent.</param>
        /// <returns>The result</returns>
        public static decimal PercentageOf(this int number, double percent) {
            return (decimal)(number * percent / 100);
        }

        /// <summary>
        /// Percentage of the number.
        /// </summary>
        /// <param key="percent">The percent</param>
        /// <param key="number">The Number</param>
        /// <returns>The result</returns>
        public static decimal PercentOf(this int position, double total) {
            decimal result = 0;
            if (position > 0 && total > 0)
                result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }

        /// <summary>
        /// The numbers percentage
        /// </summary>
        /// <param key="number">The number.</param>
        /// <param key="percent">The percent.</param>
        /// <returns>The result</returns>
        public static decimal PercentageOf(this int number, decimal percent) {
            return (decimal)(number * percent / 100);
        }

        /// <summary>
        /// Percentage of the number.
        /// </summary>
        /// <param key="percent">The percent</param>
        /// <param key="number">The Number</param>
        /// <returns>The result</returns>
        public static decimal PercentOf(this int position, decimal total) {
            decimal result = 0;
            if (position > 0 && total > 0)
                result = (decimal)position / (decimal)total * 100;
            return result;
        }

        /// <summary>
        /// The numbers percentage
        /// </summary>
        /// <param key="number">The number.</param>
        /// <param key="percent">The percent.</param>
        /// <returns>The result</returns>
        public static decimal PercentageOf(this int number, long percent) {
            return (decimal)(number * percent / 100);
        }

        /// <summary>
        /// Percentage of the number.
        /// </summary>
        /// <param key="percent">The percent</param>
        /// <param key="number">The Number</param>
        /// <returns>The result</returns>
        public static decimal PercentOf(this int position, long total) {
            decimal result = 0;
            if (position > 0 && total > 0)
                result = (decimal)position / (decimal)total * 100;
            return result;
        }

        #endregion

        public static string ToString(this int? value, string defaultvalue) {
            if (value == null) return defaultvalue;
            return value.Value.ToString();
        }

        /// <summary>
        /// Gibt zurück ob der Wehrt in den Bereich passt.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static int FitInRange(this int value, int max, int min = 0) {
            if (value > max)
                return max;
            if (value < min)
                return min;
            return value;
        }
    }
}
