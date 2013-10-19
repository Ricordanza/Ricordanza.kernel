using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

namespace Ricordanza.Core.Utilities
{
    /// <summary>
    /// 日付操作のユーティリティクラスです。
    /// </summary>
    /// <remarks>このクラスは<see cref="System.DateTime"/>を拡張します。</remarks>
    public static class DateTimeUtility
    {
        #region const

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region property

        #endregion

        #region public method

        /// <summary>
        /// 期間日数を取得します。
        /// </summary>
        /// <param name="startDay">基準日</param>
        /// <param name="arrivalDay">経過日</param>
        /// <returns>期間日数</returns>
        public static int Span(this DateTime startDay, DateTime arrivalDay)
        {
            return (arrivalDay - startDay).Days;
        }

        /// <summary>
        /// 元号yy年MM月dd日の書式文字列を取得します。
        /// </summary>
        /// <param name="value">変換したい日付</param>
        /// <returns>書式化した文字列</returns>
        public static string ToGGYYMMDD(this DateTime value)
        {
            return value.ToString("ggyy年MM月dd日", Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// 元号yy年MM月dd日の書式文字列を取得します。
        /// </summary>
        /// <param name="value">変換したい日付</param>
        /// <returns>書式化した文字列</returns>
        public static string ToGGYYMM(this DateTime value)
        {
            return value.ToString("ggyy年MM月", Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// yyyyMMddHHmmssの書式文字列を取得します。
        /// </summary>
        /// <param name="value">変換したい日付</param>
        /// <returns>書式化した文字列</returns>
        public static string ToYYYYMMDDHHMMSS(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// yyyy/MM/ddの書式文字列を取得します。
        /// </summary>
        /// <param name="value">変換したい日付</param>
        /// <returns>書式化した文字列</returns>
        public static string ToYYYYMMDD(this DateTime value)
        {
            return value.ToString("yyyy/MM/dd");
        }

        /// <summary>
        /// yyyy/MMの書式文字列を取得します。
        /// </summary>
        /// <param name="value">変換したい日付</param>
        /// <returns>書式化した文字列</returns>
        public static string ToYYYYMM(this DateTime value)
        {
            return value.ToString("yyyy/MM");
        }

        /// <summary>
        /// yyyyの書式文字列を取得します。
        /// </summary>
        /// <param name="value">変換したい日付</param>
        /// <returns>書式化した文字列</returns>
        public static string ToYYYY(this DateTime value)
        {
            return value.ToString("yyyy");
        }

        /// <summary>
        /// yyyyの書式の事業年度を取得します。
        /// </summary>
        /// <param name="value">変換したい日付</param>
        /// <returns>書式化した文字列</returns>
        public static string ToBusinessYYYY(this DateTime value)
        {
            if (value.Month < 4)
                return value.AddYears(-1).ToYYYY();
            else
                return value.ToYYYY();
        }

        /// <summary>
        /// 月末を取得します。
        /// </summary>
        /// <param name="value">変換したい日付</param>
        /// <returns>日を月末に変換した日付</returns>
        public static DateTime MonthEnd(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 閏年かどうかを示す値を返します。
        /// </summary>
        /// <param name="value">判定したい日付</param>
        /// <returns>閏年である場合は<c>true</c>。それ以外の場合は<c>false</c>。</returns>
        public static bool IsLeapYear(this DateTime value)
        {
            return DateTime.IsLeapYear(value.Year);
        }

        /// <summary>
        /// 誕生日から<see cref="System.DateTime.Now"/>までの経過日数から年齢を取得します。
        /// </summary>
        /// <param name="birthday">誕生日</param>
        /// <returns>年齢</returns>
        public static int Age(this DateTime birthday)
        {
            return Age(birthday, DateTime.Now);
        }

        /// <summary>
        /// 誕生日から<c>target</c>までの経過日数から年齢を取得します。
        /// </summary>
        /// <param name="birthday">誕生日</param>
        /// <param name="target">年齢を計算したい日付</param>
        /// <returns>年齢</returns>
        public static int Age(this DateTime birthday, DateTime target)
        {
            if (target.Month < birthday.Month || (target.Month == birthday.Month && target.Day < birthday.Day))
                return target.Year - birthday.Year - 1;
            else
                return target.Year - birthday.Year;
        }

        #endregion
    }

}
