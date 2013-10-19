using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Ricordanza.Data
{
    /// <summary>
    /// 透過キャッシュを行うデータベースへの接続を表します。
    /// </summary>
    public class CacheSqlCommand
        : DbCommand
    {
        #region const

        #endregion

        #region private variabl

        /// <summary>
        /// <see cref="System.Data.SqlClient.SqlCommand"/>
        /// </summary>
        private SqlCommand _command;

        /// <summary>
        /// <see cref="Ricordanza.Data.IExecuteReaderProvider"/>
        /// </summary>
        private IExecuteReaderProvider _executeReaderProvider;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// 新しいインスタンスを構築します。
        /// </summary>
        /// <param name="command"><see cref="System.Data.SqlClient.SqlCommand"/>オブジェクト</param>
        /// <param name="executeReaderProvider"><see cref="Ricordanza.Data.IExecuteReaderProvider"/>オブジェクト</param>
        public CacheSqlCommand(SqlCommand command, IExecuteReaderProvider executeReaderProvider)
        {
            if (command == null) 
                throw new ArgumentNullException("command is null.");
            
            if (executeReaderProvider == null) 
                throw new ArgumentNullException("executeReaderProvider is null.");

            this._command = command;
            this._executeReaderProvider = executeReaderProvider;
        }

        /// <summary>
        /// 新しいインスタンスを構築します。
        /// </summary>
        /// <param name="command"><see cref="System.Data.SqlClient.SqlCommand"/>オブジェクト</param>
        /// <param name="executeReaderProvider"><see cref="Ricordanza.Data.IExecuteReaderProvider"/>オブジェクト</param>
        public CacheSqlCommand(IExecuteReaderProvider executeReaderProvider)
            : this(new SqlCommand(), executeReaderProvider)
        {
        }

        /// <summary>
        /// 新しいインスタンスを構築します。
        /// </summary>
        /// <param name="commandText">コマンド文字列</param>
        /// <param name="executeReaderProvider"><see cref="Ricordanza.Data.IExecuteReaderProvider"/>オブジェクト</param>
        public CacheSqlCommand(string commandText, IExecuteReaderProvider executeReaderProvider)
            : this(new SqlCommand(commandText), executeReaderProvider)
        {
        }

        /// <summary>
        /// 新しいインスタンスを構築します。
        /// </summary>
        /// <param name="commandText">コマンド文字列</param>
        /// <param name="connection"><see cref="System.Data.SqlClient.SqlConnection"/>オブジェクト</param>
        /// <param name="executeReaderProvider"><see cref="Ricordanza.Data.IExecuteReaderProvider"/>オブジェクト</param>
        public CacheSqlCommand(string commandText, SqlConnection connection, IExecuteReaderProvider executeReaderProvider)
            : this(new SqlCommand(commandText, connection), executeReaderProvider)
        {
        }

        /// <summary>
        /// 新しいインスタンスを構築します。
        /// </summary>
        /// <param name="commandText">コマンド文字列</param>
        /// <param name="connection"><see cref="System.Data.SqlClient.SqlConnection"/>オブジェクト</param>
        /// <param name="transaction"><see cref="System.Data.SqlClient.SqlTransaction"/>オブジェクト</param>
        /// <param name="executeReaderProvider"><see cref="Ricordanza.Data.IExecuteReaderProvider"/>オブジェクト</param>
        public CacheSqlCommand(string commandText, SqlConnection connection, SqlTransaction transaction, IExecuteReaderProvider executeReaderProvider)
            : this(new SqlCommand(commandText, connection, transaction), executeReaderProvider)
        {
        }

        #endregion

        #region property

        /// <summary>
        /// データ ソースで実行する<c>Transact-SQL</c>ステートメント、テーブル名、またはストアド プロシージャを取得または設定します。
        /// </summary>
        public override string CommandText
        {
            get { return this._command.CommandText; }
            set { this._command.CommandText = value; }
        }

        /// <summary>
        /// コマンドを実行する試みを終了してエラーが生成されるまでの待機時間を取得または設定します。
        /// </summary>
        /// <remarks>
        /// コマンドが実行されるまでの待機時間 (秒)。既定値は<c>30</c>秒です。
        /// </remarks>
        public override int CommandTimeout
        {
            get { return this._command.CommandTimeout; }
            set { this._command.CommandTimeout = value; }
        }

        /// <summary>
        /// <see cref="Ricordanza.Data.CacheSqlCommand"/>プロパティの解釈方法を示す値を取得または設定します。
        /// </summary>
        public override CommandType CommandType
        {
            get { return this._command.CommandType; }
            set { this._command.CommandType = value; }
        }

        /// <summary>
        /// コマンド オブジェクトを<c>Windows</c>フォーム デザイナー コントロールに表示する必要があるかどうかを示す値を取得または設定します。
        /// </summary>
        public override bool DesignTimeVisible
        {
            get { return this._command.DesignTimeVisible; }
            set { this._command.DesignTimeVisible = value; }
        }

        /// <summary>
        /// <see cref="System.Data.Common.DbDataAdapter"/>の<c>Update</c>メソッドで使用するときに、コマンドの結果を<see cref="System.Data.DataRow"/>に適用する方法を取得または設定します。
        /// </summary>
        public override UpdateRowSource UpdatedRowSource
        {
            get { return this._command.UpdatedRowSource; }
            set { this._command.UpdatedRowSource = value; }
        }

        /// <summary>
        /// <see cref="System.Data.SqlClient.SqlParameter"/>オブジェクトの新しいインスタンスを作成します。
        /// </summary>
        /// <returns></returns>
        protected override DbParameter CreateDbParameter()
        {
            return this._command.CreateParameter();
        }

        /// <summary>
        /// この<see cref="Ricordanza.Data.CacheSqlCommand"/>のインスタンスで使用する<see cref="System.Data.SqlClient.SqlConnection"/>を取得または設定します。
        /// </summary>
        protected override DbConnection DbConnection
        {
            get { return this._command.Connection; }
            set { this._command.Connection = value as SqlConnection; }
        }

        /// <summary>
        /// <see cref="System.Data.SqlClient.SqlParameterCollection"/>を取得します。
        /// </summary>
        protected override DbParameterCollection DbParameterCollection
        {
            get { return this._command.Parameters; }
        }

        /// <summary>
        /// <see cref="Ricordanza.Data.CacheSqlCommand"/>を実行する<see cref="System.Data.SqlClient.SqlTransaction"/>を取得または設定します。
        /// </summary>
        protected override DbTransaction DbTransaction
        {
            get { return this._command.Transaction; }
            set { this._command.Transaction = value as SqlTransaction; }
        }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// <see cref="Ricordanza.Data.CacheSqlCommand"/>の実行のキャンセルを試行します。
        /// </summary>
        public override void Cancel()
        {
            this._command.Cancel();
        }

        /// <summary>
        /// 接続に対して<c>Transact-SQL</c>ステートメントを実行し、影響を受けた行数を返します。
        /// </summary>
        /// <returns>影響を受けた行数。</returns>
        public override int ExecuteNonQuery()
        {
            return this._command.ExecuteNonQuery();
        }

        /// <summary>
        /// クエリを実行し、そのクエリが返す結果セットの最初の行にある最初の列を返します。残りの列または行は無視されます。
        /// </summary>
        /// <returns>結果セットの最初の行の最初の列。結果セットが空の場合は、<c>null</c>参照。最大<c>2033</c>文字を返します。</returns>
        public override object ExecuteScalar()
        {
            var reader = this.ExecuteDbDataReader(CommandBehavior.Default);

            reader.Read();

            return reader.GetValue(0);
        }

        /// <summary>
        /// <c>SQL Server</c>のインスタンスに対する準備済みのコマンドを作成します。
        /// </summary>
        public override void Prepare()
        {
            this._command.Prepare();
        }

        #endregion

        #region protected method

        /// <summary>
        /// <see cref="System.ComponentModel.Component"/>によって使用されているすべてのリソースを解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースとアンマネージ リソースの両方を解放する場合は<c>true</c>。アンマネージ リソースだけを解放する場合は<c>false</c>。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this._command.Dispose();

            base.Dispose(disposing);
        }

        /// <summary>
        /// <see cref="System.Data.Common.DbCommand.Connection"/>に対して<see cref="System.Data.Common.DbCommand.CommandText"/>を実行し、<br />
        /// <see cref="System.Data.CommandBehavior"/>値の<c>1</c>つを使用して<see cref="System.Data.Common.DbDataReader"/>を返します。
        /// </summary>
        /// <param name="behavior"><see cref="System.Data.CommandBehavior"/>値の<c>1</c>つ</param>
        /// <returns><see cref="System.Data.Common.DbDataReader"/>オブジェクト。</returns>
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return this._executeReaderProvider.ExecuteReader(this._command, behavior);
        }

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }
}
