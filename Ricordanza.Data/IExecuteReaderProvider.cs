using System.Data;
using System.Data.Common;

namespace Ricordanza.Data
{
    /// <summary>
    /// 透過キャッシュを行う規定のインターフェースです。
    /// </summary>
    public interface IExecuteReaderProvider
    {
        /// <summary>
        /// <see cref="System.Data.Common.DbCommand.Connection"/>に対して<see cref="System.Data.Common.DbCommand.CommandText"/>を実行し、<br />
        /// <see cref="System.Data.CommandBehavior"/>値の<c>1</c>つを使用して<see cref="System.Data.Common.DbDataReader"/>を返します。
        /// </summary>
        /// <param name="command"><see cref="System.Data.Common.DbCommand.Connection"/></param>
        /// <param name="behavior"><see cref="System.Data.CommandBehavior"/>値の<c>1</c>つ</param>
        /// <returns><see cref="System.Data.Common.DbDataReader"/>オブジェクト。</returns>
        DbDataReader ExecuteReader(DbCommand command, CommandBehavior behavior);
    }
}
