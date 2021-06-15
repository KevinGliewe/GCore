﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Extensions.DecimalEx {
    /// <summary>
    /// Decimal Extensions
    /// </summary>
    public static class DecimalExtensions {
        #region PercentageOf calculations

        /// <summary>
        /// The numbers percentage
        /// </summary>
        /// <param key="number">The number.</param>
        /// <param key="percent">The percent.</param>
        /// <returns>The result</returns>
        public static decimal PercentageOf(this decimal number, int percent) {
            return (decimal)(number * percent / 100);
        }

        /// <summary>
        /// Percentage of the number.
        /// </summary>
        /// <param key="percent">The percent</param>
        /// <param key="number">The Number</param>
        /// <returns>The result</returns>
        public static decimal PercentOf(this decimal position, int total) {
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
        public static decimal PercentageOf(this decimal number, decimal percent) {
            return (decimal)(number * percent / 100);
        }

        /// <summary>
        /// Percentage of the number.
        /// </summary>
        /// <param key="percent">The percent</param>
        /// <param key="number">The Number</param>
        /// <returns>The result</returns>
        public static decimal PercentOf(this decimal position, decimal total) {
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
        public static decimal PercentageOf(this decimal number, long percent) {
            return (decimal)(number * percent / 100);
        }

        /// <summary>
        /// Percentage of the number.
        /// </summary>
        /// <param key="percent">The percent</param>
        /// <param key="number">The Number</param>
        /// <returns>The result</returns>
        public static decimal PercentOf(this decimal position, long total) {
            decimal result = 0;
            if (position > 0 && total > 0)
                result = (decimal)position / (decimal)total * 100;
            return result;
        }

        #endregion
    }
}
