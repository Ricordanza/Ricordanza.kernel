using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Ricordanza.Data
{
    /// <summary>
    /// <c>LINQ to SQL</c>のクエリ結果をほぼ透過的にキャッシュするクラスです。
    /// </summary>
    /// <example>
    /// <code>
    /// using (var connection = new CacheSqlConnection(Properties.Settings.Default.ConnectionString, new MemoryCache()))
    /// using (var db = new Data1DataContext(connection))
    ///     var array = db.Products.Where(__ =&gt; __.Price &gt; 500).OrderBy(_ =&gt; _.Price).Take(200).ToArray();
    /// </code>
    /// </example>
    public class MemoryCache
        : IExecuteReaderProvider
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// DataTableのキャッシュ
        /// </summary>
        private Dictionary<string, DataTable> _dictionary;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// このクラスのインスタンスを構築します。
        /// </summary>
        public MemoryCache()
            : base()
        {
            _dictionary = new Dictionary<string, DataTable>();
        }

        #endregion

        #region property

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// <see cref="System.Data.Common.DbCommand.Connection"/>に対して<see cref="System.Data.Common.DbCommand.CommandText"/>を実行し、<br />
        /// <see cref="System.Data.CommandBehavior"/>値の<c>1</c>つを使用して<see cref="System.Data.Common.DbDataReader"/>を返します。
        /// </summary>
        /// <param name="command"><see cref="System.Data.Common.DbCommand.Connection"/></param>
        /// <param name="behavior"><see cref="System.Data.CommandBehavior"/>値の<c>1</c>つ</param>
        /// <returns><see cref="System.Data.Common.DbDataReader"/>オブジェクト。</returns>
        public DbDataReader ExecuteReader(DbCommand command, CommandBehavior behavior)
        {
            var key = ExecuteReaderUtility.CreateKey(command);
            var value = this.Get(key);

            if (value == null)
            {
                using (var reader = command.ExecuteReader(behavior))
                {
                    value = new DataTable();
                    value.Load(reader);
                    value = this.AddOrGetExisting(key, value);
                }
            }

            return new DataTableReader(value);
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        /// <summary>
        /// キと一致する<see cref="System.Data.DataTable"/>を取得します。
        /// </summary>
        /// <param name="key"><see cref="System.Data.DataTable"/>のキー</param>
        /// <returns>
        /// キーと一致する<see cref="System.Data.DataTable"/>。<br />
        /// 一致する<see cref="System.Data.DataTable"/>が存在しない場合は<c>default(<see cref="System.Data.DataTable"/>)</c>
        /// </returns>
        private DataTable Get(string key)
        {
            var value = default(DataTable);

            lock (this._dictionary)
                return this._dictionary.TryGetValue(key, out value) ? value : null;
        }

        /// <summary>
        /// キーと一致する<see cref="System.Data.DataTable"/>が存在しない場合は追加します。
        /// </summary>
        /// <param name="key"><see cref="System.Data.DataTable"/>のキー</param>
        /// <param name="value">追加したい<see cref="System.Data.DataTable"/></param>
        /// <returns>追加した<see cref="System.Data.DataTable"/></returns>
        private DataTable AddOrGetExisting(string key, DataTable value)
        {
            lock (this._dictionary)
            {
                if (!this._dictionary.ContainsKey(key))
                    this._dictionary[key] = value;
            }

            return value;
        }

        #endregion

        #region delegate

        #endregion
    }
}
