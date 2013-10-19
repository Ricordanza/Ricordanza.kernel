using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Ricordanza.Core.Utilities
{
    /// <summary>
    /// <see cref="System.Data.SqlClient.SqlDataReader"/>のユーティリティクラスです。
    /// </summary>
    /// <remarks>このクラスは<see cref="System.Data.SqlClient.SqlDataReader"/>を拡張します。</remarks>
    public static class SqlDataReaderUtility
    {
        /// <summary>
        /// 指定した列の値をstring型として取得します。
        /// </summary>
        /// <param name="reader"><see cref="System.Data.SqlClient.SqlDataReader"/></param>
        /// <param name="key">列名</param>
        /// <returns>値</returns>
        /// <remarks>
        /// <c>key</c>に対応する値が<c>DBNull</c>の場合は<c>string.Empty</c>を返却します。
        /// </remarks>
        public static string GetString(this SqlDataReader reader, string key)
        {
            return GetString(reader, key, string.Empty);
        }

        /// <summary>
        /// 指定した列の値をstring型として取得します。
        /// </summary>
        /// <param name="reader"><see cref="System.Data.SqlClient.SqlDataReader"/></param>
        /// <param name="key">列名</param>
        /// <param name="defalutValue">変換不可能時のデフォルト値</param>
        /// <returns>値</returns>
        /// <remarks>
        /// <c>key</c>に対応する値が<c>DBNull</c>の場合は<c>defaultValue</c>を返却します。
        /// </remarks>
        public static string GetString(this SqlDataReader reader, string key, string defalutValue)
        {
            if (reader == null || reader.IsClosed)
                throw new ArgumentNullException("reader is null or closed.");

            // カラム名(key)の精査をかねてデータが取得可能か検証
            if (reader[key] == null || reader[key] is DBNull)
                return defalutValue;

            return reader[key].ToString();
        }

        /// <summary>
        /// 指定した列の値をint型として取得します。
        /// </summary>
        /// <param name="reader"><see cref="System.Data.SqlClient.SqlDataReader"/></param>
        /// <param name="key">列名</param>
        /// <returns>値</returns>
        /// <remarks>
        /// <c>key</c>に対応する値が<c>DBNull</c>の場合は<c>int.MinValue</c>を返却します。
        /// </remarks>
        public static int GetInt(this SqlDataReader reader, string key)
        {
            return GetInt(reader, key, int.MinValue);
        }

        /// <summary>
        /// 指定した列の値をint型として取得します。
        /// </summary>
        /// <param name="reader"><see cref="System.Data.SqlClient.SqlDataReader"/></param>
        /// <param name="key">列名</param>
        /// <returns>値</returns>
        /// <remarks>
        /// <c>key</c>に対応する値が<c>DBNull</c>の場合は<c>defalutValue</c>を返却します。
        /// </remarks>
        public static int GetInt(this SqlDataReader reader, string key, int defalutValue)
        {
            return GetString(reader, key).ToInt(defalutValue);
        }

        /// <summary>
        /// 指定した列の値をstring型として取得します。
        /// </summary>
        /// <param name="reader"><see cref="System.Data.SqlClient.SqlDataReader"/></param>
        /// <param name="key">列名</param>
        /// <param name="format">フォーマット</param>
        /// <returns>値</returns>
        /// <remarks>
        /// <c>key</c>に対応する値が<c>DBNull</c>の場合は<c>defalutValue</c>を返却します。
        /// </remarks>
        public static string GetIntFormat(this SqlDataReader reader, string key, string format)
        {
            if (reader[key] == DBNull.Value)
                return string.Empty;

            return GetInt(reader, key).ToString(format);
        }

        /// <summary>
        /// 指定した列の値をstring型として取得します。
        /// </summary>
        /// <param name="reader"><see cref="System.Data.SqlClient.SqlDataReader"/></param>
        /// <param name="key">列名</param>
        /// <param name="format">フォーマット</param>
        /// <returns>値</returns>
        /// <remarks>
        /// <c>key</c>に対応する値が<c>DBNull</c>の場合は<c>defalutValue</c>を返却します。
        /// </remarks>
        public static string GetDecimalFormat(this SqlDataReader reader, string key, string format)
        {
            if (reader[key] == DBNull.Value)
                return string.Empty;

            return GetDecimal(reader, key).ToString(format);
        }

        /// <summary>
        /// 指定した列の値をdecimal型として取得します。
        /// </summary>
        /// <param name="reader"><see cref="System.Data.SqlClient.SqlDataReader"/></param>
        /// <param name="key">列名</param>
        /// <returns>値</returns>
        /// <remarks>
        /// <c>key</c>に対応する値が<c>DBNull</c>の場合は<c>decimal.MinValue</c>を返却します。
        /// </remarks>
        public static decimal GetDecimal(this SqlDataReader reader, string key)
        {
            return GetDecimal(reader, key, decimal.MinValue);
        }

        /// <summary>
        /// 指定した列の値をdecimal型として取得します。
        /// </summary>
        /// <param name="reader"><see cref="System.Data.SqlClient.SqlDataReader"/></param>
        /// <param name="key">列名</param>
        /// <returns>値</returns>
        /// <remarks>
        /// <c>key</c>に対応する値が<c>DBNull</c>の場合は<c>defalutValue</c>を返却します。
        /// </remarks>
        public static decimal GetDecimal(this SqlDataReader reader, string key, decimal defalutValue)
        {
            return GetString(reader, key).ToDecimal(defalutValue);
        }

        /// <summary>
        /// 指定した列の値をDateTime型として取得します。
        /// </summary>
        /// <param name="reader"><see cref="System.Data.SqlClient.SqlDataReader"/></param>
        /// <param name="key">列名</param>
        /// <returns>値</returns>
        /// <remarks>
        /// <c>key</c>に対応する値が<c>DBNull</c>の場合は<c>DateTime.MinValue</c>を返却します。
        /// </remarks>
        public static DateTime GetDateTime(this SqlDataReader reader, string key)
        {
            return GetDateTime(reader, key, DateTime.MinValue);
        }

        /// <summary>
        /// 指定した列の値をDateTime型として取得します。
        /// </summary>
        /// <param name="reader"><see cref="System.Data.SqlClient.SqlDataReader"/></param>
        /// <param name="key">列名</param>
        /// <returns>値</returns>
        /// <remarks>
        /// <c>key</c>に対応する値が<c>DBNull</c>の場合は<c>defalutValue</c>を返却します。
        /// </remarks>
        public static DateTime GetDateTime(this SqlDataReader reader, string key, DateTime defalutValue)
        {
            return GetString(reader, key).ToDateTime(defalutValue);
        }

        /// <summary>
        /// 指定した列の値が<see cref="System.DBNull"/>か判定します。
        /// </summary>
        /// <param name="reader"><see cref="System.Data.SqlClient.SqlDataReader"/></param>
        /// <param name="key">列名</param>
        /// <returns><c>DBNull</c>の場合は<c>true</c>、それ以外の場合は<c>false</c></returns>
        public static bool IsNull(this SqlDataReader reader, string key)
        {
            return (reader[key] == null || reader[key] is DBNull);
        }
    }
}
