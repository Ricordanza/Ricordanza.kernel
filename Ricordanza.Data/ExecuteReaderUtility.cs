using System.Data;
using System.Linq;
using System.Text;

namespace Ricordanza.Data
{
    /// <summary>
    /// <c>Ricordanza.Data</c>名前空間の共通ユーティリティクラスです。
    /// </summary>
    public class ExecuteReaderUtility
    {
        /// <summary>
        /// キャッシュ用のキーを作成します。
        /// </summary>
        /// <param name="command">キーを作成したい<see cref="System.Data.IDbCommand"/></param>
        /// <returns>作成したキー</returns>
        public static string CreateKey(IDbCommand command)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"SqlCmd:[{0}]", command.CommandText);

            if (command.Parameters.Count > 0)
            {
                sb.Append("?");

                var parameters = command.Parameters.Cast<IDataParameter>()
                    .Select(_ => (_.Value == null)
                        ? string.Format(@"[{0}:{1}]", _.ParameterName, _.DbType.ToString())
                        : string.Format(@"[{0}:{1}={2}]", _.ParameterName, _.DbType.ToString(), ExecuteReaderUtility.ToString(_.Value, _.DbType)));

                sb.Append(string.Join("&", parameters.ToArray()));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 対象オブジェクトをデータ型に合わせた文字列に変換しています。
        /// </summary>
        /// <param name="value">文字列化したい値</param>
        /// <param name="type">データ型</param>
        /// <returns>文字列化した値</returns>
        /// <remarks>
        /// データ型毎の文字列化処理は未実装。<br />
        /// <c>value.ToString()</c>で処理を統一。
        /// </remarks>
        private static string ToString(object value, DbType type)
        {
            if (value == null)
                return string.Empty;

            return value.ToString();
        }
    }
}
