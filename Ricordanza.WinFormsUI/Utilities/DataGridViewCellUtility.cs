using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Ricordanza.Core.Utilities;

namespace Ricordanza.WinFormsUI.Utilities
{
    /// <summary>
    /// <c>System.Windows.Forms.DataGridViewCell</c>のユーティリティクラスです。
    /// </summary>
    /// <remarks>このクラスは<c>System.Windows.Forms.DataGridViewCell</c>を拡張します。</remarks>
    public static class DataGridViewCellUtility
    {
        #region public method

        /// <summary>
        /// セルが保持する値が空か判定します。
        /// セルが保持する値が<c>null</c>の場合は<c>true</c>を取得します。
        /// このメソッドは値を一度文字列化した後に変換を行います。
        /// </summary>
        /// <param name="cell">検査対象となるセル</param>
        /// <returns>空文字の場合は<c>true</c> それ以外の場合は<c>false</c></returns>
        public static bool IsEmpty(this DataGridViewCell cell)
        {
            if (cell == null || cell.Value == null)
                return true;

            return cell.Value.ToString().IsEmpty();
        }

        /// <summary>
        /// セルが保持する値が数値であるかどうかを返します。
        /// このメソッドは値を一度文字列化し、その後数値かどうか判定します。
        /// </summary>
        /// <param name="cell">検査対象となるセル<param>
        /// <returns>true - 数値 false - 数値ではない</returns>
        /// <remarks>
        /// このメソッドは<c>decimal</c>型を用いて値を検証します。
        /// 浮動小数を含む値であったとしても適切であれば<c>true</c>を返却します。
        /// </remarks>
        public static bool IsNumeric(this DataGridViewCell cell)
        {
            if (cell == null || cell.Value == null)
                return false;

            return cell.Value.ToString().IsNumeric();
        }

        /// <summary>
        /// セルが保持する値が日付型であるかどうかを返します。
        /// このメソッドは値を一度文字列化し、その後数値かどうか判定します。
        /// </summary>
        /// <param name="cell">検査対象となるセル<param>
        /// <returns>true - 日付型 false - 日付型ではない</returns>
        /// <remarks>
        /// このメソッドは<c>DateTime</c>型を用いて値を検証します。
        /// 浮動小数を含む値であったとしても適切であれば<c>true</c>を返却します。
        /// </remarks>
        public static bool IsDateTime(this DataGridViewCell cell)
        {
            if (cell == null || cell.Value == null)
                return false;

            return cell.Value.ToString().IsDateTime();
        }

        /// <summary>
        /// セルが保持する値をstring型に変換します。
        /// このメソッドは値を一度文字列化した後に変換を行います。
        /// </summary>
        /// <param name="cell">対象のセル</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// 拡張メソッドとして使用する場合は、
        /// <c>null</c>参照でない事が保障されている場合に使用して下さい。
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>0</c>を返却します。
        /// </remarks>
        public static string StringValue(this DataGridViewCell cell)
        {
            return StringValue(cell, string.Empty);
        }

        /// <summary>
        /// セルが保持する値をstring型に変換します。
        /// このメソッドは値を一度文字列化した後に変換を行います。
        /// </summary>
        /// <param name="cell">対象のセル</param>
        /// <param name="defaultValue">変換不可能時のデフォルト値</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>defaultValue</c>を返却します。
        /// </remarks>
        public static string StringValue(this DataGridViewCell cell, string defaultValue)
        {
            if (cell == null || cell.Value == null)
                return defaultValue;

            return cell.Value.ToString();
        }

        /// <summary>
        /// セルが保持する値をint型に変換します。
        /// このメソッドは値を一度文字列化した後に変換を行います。
        /// </summary>
        /// <param name="cell">対象のセル</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>0</c>を返却します。
        /// </remarks>
        public static int IntValue(this DataGridViewCell cell)
        {
            return IntValue(cell, int.MinValue);
        }

        /// <summary>
        /// セルが保持する値をint型に変換します。
        /// このメソッドは値を一度文字列化した後に変換を行います。
        /// </summary>
        /// <param name="cell">対象のセル</param>
        /// <param name="defaultValue">変換不可能時のデフォルト値</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>defaultValue</c>を返却します。
        /// </remarks>
        public static int IntValue(this DataGridViewCell cell, int defaultValue)
        {
            if (cell == null || cell.Value == null)
                return defaultValue;

            return cell.Value.ToString().ToInt(defaultValue);
        }

        /// <summary>
        /// セルが保持する値をDateTime型に変換します。
        /// このメソッドは値を一度文字列化した後に変換を行います。
        /// </summary>
        /// <param name="cell">対象のセル</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>DateTime.MinValue</c>を返却します。
        /// </remarks>
        public static DateTime DateTimeValue(this DataGridViewCell cell)
        {
            return DateTimeValue(cell, DateTime.MinValue);
        }

        /// <summary>
        /// セルが保持する値をDateTime型に変換します。
        /// このメソッドは値を一度文字列化した後に変換を行います。
        /// </summary>
        /// <param name="cell">対象のセル</param>
        /// <param name="defaultValue">変換不可能時のデフォルト値</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>defaultValue</c>を返却します。
        /// </remarks>
        public static DateTime DateTimeValue(this DataGridViewCell cell, DateTime defaultValue)
        {
            if (cell == null || cell.Value == null)
                return defaultValue;

            return cell.Value.ToString().ToDateTime(defaultValue);
        }

        /// <summary>
        /// セルが保持する値をdecimal型に変換します。
        /// このメソッドは値を一度文字列化した後に変換を行います。
        /// </summary>
        /// <param name="cell">対象のセル</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>int.MinValue</c>を返却します。
        /// </remarks>
        public static decimal DecimalValue(this DataGridViewCell cell)
        {
            return DecimalValue(cell, int.MinValue);
        }

        /// <summary>
        /// セルが保持する値をdecimal型に変換します。
        /// このメソッドは値を一度文字列化した後に変換を行います。
        /// </summary>
        /// <param name="cell">対象のセル</param>
        /// <param name="defaultValue">変換不可能時のデフォルト値</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>defaultValue</c>を返却します。
        /// </remarks>
        public static decimal DecimalValue(this DataGridViewCell cell, decimal defaultValue)
        {
            if (cell == null || cell.Value == null)
                return defaultValue;

            return cell.Value.ToString().ToDecimal(defaultValue);
        }

        /// <summary>
        /// セルが保持する値をbool型に変換します。
        /// このメソッドは値を一度文字列化した後に変換を行います。
        /// </summary>
        /// <param name="cell">対象のセル</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>false</c>を返却します。
        /// </remarks>
        public static bool BooleanValue(this DataGridViewCell cell)
        {
            return BooleanValue(cell, false);
        }

        /// <summary>
        /// セルが保持する値をbool型に変換します。
        /// このメソッドは値を一度文字列化した後に変換を行います。
        /// </summary>
        /// <param name="cell">対象のセル</param>
        /// <param name="defaultValue">変換不可能時のデフォルト値</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>defaultValue</c>を返却します。
        /// </remarks>
        public static bool BooleanValue(this DataGridViewCell cell, bool defaultValue)
        {
            if (cell == null || cell.Value == null)
                return defaultValue;

            bool? result = cell.Value.ToString().ToNullable<bool>();
            return result ?? defaultValue;
        }

        /// <summary>
        /// 文字列が指定した型のTryParseに成功した場合はその値を、失敗した場合はnullを返却します。
        /// </summary>
        /// <typeparam name="T">変換対象の型</typeparam>
        /// <param name="value">変換したい値</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// TryParseをサポートしない場合は、NotSupportedException例外を発生させます。
        /// </remarks>
        public static Nullable<T> ToNullable<T>(this DataGridViewCell cell) where T : struct
        {
            string value = cell.StringValue();

            if (string.IsNullOrEmpty(value))
                return null;

            var type = typeof(T);
            var types = new[] { typeof(string), GetReferenceType(type) };
            var method = type.GetMethod("TryParse", types);

            if (method == null)
                throw new NotSupportedException(string.Format("TryParse Not Supported Type[{0}]", type.FullName));

            var parameters = new object[] { value, new T() };
            if ((bool)method.Invoke(null, parameters))
                return (T)parameters[1];
            else
                return null;
        }

        #endregion

        #region private method

        /// <summary>
        /// 型情報を取得します。
        /// </summary>
        /// <typeparam name="T">情報を取得したい型</typeparam>
        /// <returns>型情報</returns>
        private static Type GetReferenceType<T>()
        {
            return GetReferenceType(typeof(T));
        }

        /// <summary>
        /// 型情報を取得します。
        /// </summary>
        /// <typeparam name="T">情報を取得したい型</typeparam>
        /// <returns>型情報</returns>
        private static Type GetReferenceType(Type type)
        {
            return Type.GetType(type.FullName + "&");
        }

        #endregion
    }
}
