using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Ricordanza.Core.Utilities
{
    /// <summary>
    /// <see cref="System.Data.SqlClient.SqlCommand"/>のユーティリティクラスです。
    /// </summary>
    /// <remarks>このクラスは<see cref="System.Data.SqlClient.SqlCommand"/>を拡張します。</remarks>
    public static class SqlCommandUtility
    {
        /// <summary>
        /// <c>Sql</c>文を解析し、プレースホルダに<see cref="System.Data.SqlClient.SqlParameter"/>を割り当てた実行形式の文字列を取得します。
        /// </summary>
        /// <param name="command">対象の<see cref="System.Data.SqlClient.SqlCommand"/></param>
        /// <returns>実行形式の文字列</returns>
        /// <remarks>
        /// <c>SqlDbType</c>によるプレースフォルダの置換方法の変更は行いません。
        /// 値が<c>null</c>の場合は、<c>[NULL]</c>という文字列に置き換えます。
        /// </remarks>
        public static string ExecutableText(this SqlCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command is null.");

            string query = command.CommandText;

            foreach (SqlParameter param in command.Parameters)
                query = query.Replace(param.ParameterName, ObjectUtility.EmptyToStr(param.SqlValue, "[NULL]"));

            return query;
        }
    }
}
