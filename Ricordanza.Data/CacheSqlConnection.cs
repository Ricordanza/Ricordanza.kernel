using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Ricordanza.Data
{
    /// <summary>
    /// 透過キャッシュを行うデータ ソースに対して実行する<c>SQL</c>ステートメントまたはストアド プロシージャを表します。<br />
    /// コマンドを表すデータベース固有のクラスの基本クラスを提供します。
    /// </summary>
    public class CacheSqlConnection
        : DbConnection
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// <see cref="System.Data.SqlClient.SqlConnection"/>
        /// </summary>
        private SqlConnection _connection;

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
        /// <param name="connection"><see cref="System.Data.SqlClient.SqlConnection"/>オブジェクト</param>
        /// <param name="executeReaderProvider"><see cref="Ricordanza.Data.IExecuteReaderProvider"/>オブジェクト</param>
        public CacheSqlConnection(SqlConnection connection, IExecuteReaderProvider executeReaderProvider)
        {
            if (connection == null)
                throw new ArgumentNullException("connection is null.");
            
            if (executeReaderProvider == null)
                throw new ArgumentNullException("executeReaderProvider is null.");

            this._connection = connection;
            this._executeReaderProvider = executeReaderProvider;
        }

        /// <summary>
        /// 新しいインスタンスを構築します。
        /// </summary>
        /// <param name="executeReaderProvider"><see cref="Ricordanza.Data.IExecuteReaderProvider"/>オブジェクト</param>
        public CacheSqlConnection(IExecuteReaderProvider executeReaderProvider)
            : this(new SqlConnection(), executeReaderProvider)
        {
        }

        /// <summary>
        /// 新しいインスタンスを構築します。
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <param name="executeReaderProvider"><see cref="Ricordanza.Data.IExecuteReaderProvider"/>オブジェクト</param>
        public CacheSqlConnection(string connectionString, IExecuteReaderProvider executeReaderProvider)
            : this(new SqlConnection(connectionString), executeReaderProvider)
        {
        }

        #endregion

        #region property

        /// <summary>
        /// データベースを開くために使用する文字列を取得または設定します。
        /// </summary>
        public override string ConnectionString
        {
            get { return this._connection.ConnectionString; }
            set { this._connection.ConnectionString = value; }
        }

        /// <summary>
        /// 接続するデータソースのインスタンスの名前を取得します。
        /// </summary>
        public override string DataSource
        {
            get { return this._connection.DataSource; }
        }

        /// <summary>
        /// 現在のデータベース、または接続が開いてから使用するデータベースの名前を取得します。
        /// </summary>
        public override string Database
        {
            get { return this._connection.Database; }
        }

        /// <summary>
        /// <see cref="Ricordanza.Data.CacheSqlConnection.ConnectionString"/>で指定したプロパティ設定を使用して、データベース接続を開きます。
        /// </summary>
        public override void Open()
        {
            this._connection.Open();
        }

        /// <summary>
        /// クライアントが接続しているデータベースのインスタンスのバージョンを示す文字列を取得します。
        /// </summary>
        public override string ServerVersion
        {
            get { return this._connection.ServerVersion; }
        }

        /// <summary>
        /// <see cref="Ricordanza.Data.CacheSqlConnection"/>の状態を示します。
        /// </summary>
        public override ConnectionState State
        {
            get { return this._connection.State; }
        }

        /// <summary>
        /// <see cref="Ricordanza.Data.CacheSqlConnection"/>に関連付けられている<see cref="Ricordanza.Data.CacheSqlCommand"/>オブジェクトを作成し、返します。
        /// </summary>
        /// <returns></returns>
        protected override DbCommand CreateDbCommand()
        {
            var command = this._connection.CreateCommand();
            return new CacheSqlCommand(command, this._executeReaderProvider);
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
        /// 分離レベルを指定して、データベース トランザクションを開始します。
        /// </summary>
        /// <param name="isolationLevel">トランザクションを実行する分離レベル。</param>
        /// <returns>新しいトランザクションを表すオブジェクト。</returns>
        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return this._connection.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// 開いている<see cref="Ricordanza.Data.CacheSqlConnection"/>の現在のデータベースを変更します。
        /// </summary>
        /// <param name="databaseName">現在のデータベースの代わりに使用するデータベースの名前。</param>
        public override void ChangeDatabase(string databaseName)
        {
            this._connection.ChangeDatabase(databaseName);
        }

        /// <summary>
        /// データベースへの接続を閉じます。このメソッドは、開いている接続を閉じるための最も好ましいメソッドです。
        /// </summary>
        public override void Close()
        {
            this._connection.Close();
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
                this._connection.Dispose();

            base.Dispose(disposing);
        }

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }
}
