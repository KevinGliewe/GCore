﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCore.Extensions.DateTimeEx {
    /// <summary>
    /// DateTime Extensions
    /// </summary>
    public static class DateTimeExtensions {
        #region Elapsed extension
        /// <summary>
        /// Gibt die vergangene Zeit seit dem Zeitpunkt zurück.
        /// </summary>
        /// <param key="datetime">The datetime.</param>
        /// <returns>TimeSpan</returns>
        public static TimeSpan Elapsed(this DateTime datetime) {
            return DateTime.Now - datetime;
        }
        #endregion

        #region Week of year
        /// <summary>
        /// Weeks the of year.
        /// </summary>
        /// <param key="datetime">The datetime.</param>
        /// <param key="weekrule">The weekrule.</param>
        /// <param key="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime, System.Globalization.CalendarWeekRule weekrule, DayOfWeek firstDayOfWeek) {
            System.Globalization.CultureInfo ciCurr = System.Globalization.CultureInfo.CurrentCulture;
            return ciCurr.Calendar.GetWeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        /// <summary>
        /// Weeks the of year.
        /// </summary>
        /// <param key="datetime">The datetime.</param>
        /// <param key="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime, DayOfWeek firstDayOfWeek) {
            System.Globalization.DateTimeFormatInfo dateinf = new System.Globalization.DateTimeFormatInfo();
            System.Globalization.CalendarWeekRule weekrule = dateinf.CalendarWeekRule;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        /// <summary>
        /// Weeks the of year.
        /// </summary>
        /// <param key="datetime">The datetime.</param>
        /// <param key="weekrule">The weekrule.</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime, System.Globalization.CalendarWeekRule weekrule) {
            System.Globalization.DateTimeFormatInfo dateinf = new System.Globalization.DateTimeFormatInfo();
            DayOfWeek firstDayOfWeek = dateinf.FirstDayOfWeek;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        /// <summary>
        /// Weeks the of year.
        /// </summary>
        /// <param key="datetime">The datetime.</param>
        /// <param key="weekrule">The weekrule.</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime) {
            System.Globalization.DateTimeFormatInfo dateinf = new System.Globalization.DateTimeFormatInfo();
            System.Globalization.CalendarWeekRule weekrule = dateinf.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = dateinf.FirstDayOfWeek;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        #endregion

        #region Get Datetime for Day of Week
        /// <summary>
        /// Gets the date time for day of week.
        /// </summary>
        /// <param key="datetime">The datetime.</param>
        /// <param key="day">The day.</param>
        /// <param key="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        public static DateTime GetDateTimeForDayOfWeek(this DateTime datetime, DayOfWeek day, DayOfWeek firstDayOfWeek) {
            int current = DaysFromFirstDayOfWeek(datetime.DayOfWeek, firstDayOfWeek);
            int resultday = DaysFromFirstDayOfWeek(day, firstDayOfWeek);
            return datetime.AddDays(resultday - current);
        }
        public static DateTime GetDateTimeForDayOfWeek(this DateTime datetime, DayOfWeek day) {
            System.Globalization.DateTimeFormatInfo dateinf = new System.Globalization.DateTimeFormatInfo();
            DayOfWeek firstDayOfWeek = dateinf.FirstDayOfWeek;
            return GetDateTimeForDayOfWeek(datetime, day, firstDayOfWeek);
        }
        /// <summary>
        /// Firsts the date time of week.
        /// </summary>
        /// <param key="datetime">The datetime.</param>
        /// <returns></returns>
        public static DateTime FirstDateTimeOfWeek(this DateTime datetime) {
            System.Globalization.DateTimeFormatInfo dateinf = new System.Globalization.DateTimeFormatInfo();
            DayOfWeek firstDayOfWeek = dateinf.FirstDayOfWeek;
            return FirstDateTimeOfWeek(datetime, firstDayOfWeek);
        }
        /// <summary>
        /// Firsts the date time of week.
        /// </summary>
        /// <param key="datetime">The datetime.</param>
        /// <param key="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        public static DateTime FirstDateTimeOfWeek(this DateTime datetime, DayOfWeek firstDayOfWeek) {
            return datetime.AddDays(-DaysFromFirstDayOfWeek(datetime.DayOfWeek, firstDayOfWeek));
        }

        /// <summary>
        /// Dayses from first day of week.
        /// </summary>
        /// <param key="current">The current.</param>
        /// <param key="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        private static int DaysFromFirstDayOfWeek(DayOfWeek current, DayOfWeek firstDayOfWeek) {
            //Sunday = 0,Monday = 1,...,Saturday = 6
            int daysbetween = current - firstDayOfWeek;
            if (daysbetween < 0) daysbetween = 7 + daysbetween;
            return daysbetween;
        }
        #endregion
    }
}
