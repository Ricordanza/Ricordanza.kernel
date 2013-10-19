using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Diagnostics;

namespace Ricordanza.Data
{
    /// <summary>
    /// コネクション管理クラスです。
    /// </summary>
    /// <example>
    /// トランザクション無しbr />
    /// <code>
    /// ConnectionManager.Execute(cmd =>
    /// {
    ///     cmd.CommandText = "select * from m_user where valid_flg = 1 order by seq";
    /// 
    ///     using (MySqlDataReader reader = cmd.ExecuteReader())
    ///     {
    ///         if (!reader.HasRows)
    ///             return;
    /// 
    ///         while (reader.Read())
    ///             Console.WriteLine(reader["user_name"]);
    ///     }
    /// });
    /// </code>
    /// トランザクション有り<br />
    /// <code>
    /// ConnectionManager.ExecuteTran(cmd =>
    /// {
    ///     cmd.CommandText = "insert into m_user (user_id) values (@user_id)";
    ///     cmd.Parameters.AddWithValue("@user_id", "id");
    ///     cmd.ExecuteNonQuery();
    /// 
    ///     return true;
    /// });
    /// </code>
    /// </example>
    public static class ConnectionManager
    {
        #region const

        /// <summary>
        /// OracleLike検索用のスケープ文字
        /// </summary>
        public const string ORACLE_LIKE_ESCAPE = @"\";

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor

        #endregion

        #region property

        /// <summary>
        /// 接続文字列
        /// </summary>
        public static string ConnectionString { set; get; }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// トランザクションが必要なDBアクセスのアクションを実行します。
        /// </summary>
        /// <param name="action">トランザクションが必要なDBアクセスのアクション。<c>true</c>返却時にトランザクションをコミットします。</param>
        /// <remarks><c>Insert</c>,<c>Update</c>,<c>Delete</c>をサポートします。</remarks>
        public static void InvokeTran(Func<SqlCommand, bool> action)
        {
            InvokeTran(action, ConnectionString);
        }

        /// <summary>
        /// トランザクションが必要なDBアクセスのアクションを実行します。
        /// </summary>
        /// <param name="action">トランザクションが必要なDBアクセスのアクション。<c>true</c>返却時にトランザクションをコミットします。</param>
        /// <param name="connectionString">データベースを開くために使用する文字列を取得または設定します。</param>
        /// <remarks><c>Insert</c>,<c>Update</c>,<c>Delete</c>をサポートします。</remarks>
        public static void InvokeTran(Func<SqlCommand, bool> action, string connectionString)
        {
            if (action == null)
                throw new ArgumentNullException("action is null.");

            using (SqlConnection connection = CreateConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction sqlTran = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.Transaction = sqlTran;
                            if (action(command))
                                sqlTran.Commit();
                            else
                                sqlTran.Rollback();
                        }
                    }
#if DEBUG
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        sqlTran.Rollback();
                        throw;
                    }
#else
                    catch
                    {
                        sqlTran.Rollback();
                        throw;
                    }   
#endif

                }
            }
        }

        /// <summary>
        /// トランザクションが不要なDBアクセスのアクションを実行します。
        /// </summary>
        /// <param name="action">トランザクションが不要なDBアクセスのアクション</param>
        /// <remarks><c>Select</c>をサポートします。</remarks>
        public static void Invoke(Action<SqlCommand> action)
        {
            Invoke(action, ConnectionString);
        }

        /// <summary>
        /// トランザクションが不要なDBアクセスのアクションを実行します。
        /// </summary>
        /// <param name="action">トランザクションが不要なDBアクセスのアクション</param>
        /// <param name="connectionString">データベースを開くために使用する文字列を取得または設定します。</param>
        /// <remarks><c>Select</c>をサポートします。</remarks>
        public static void Invoke(Action<SqlCommand> action, string connectionString)
        {
            if (action == null)
                throw new ArgumentNullException("action is null.");

            using (SqlConnection connection = CreateConnection(connectionString))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = connection.CreateCommand())
                        action(command);
                }
#if DEBUG
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    throw;
                }
#else
                catch
                {
                    throw;
                }
#endif
            }
        }

        /// <summary>
        /// トランザクションが必要なSqlDataAdapterのアクションを実行します。
        /// </summary>
        /// <param name="adapter">トランザクション管理を行うSqlDataAdapter</param>
        /// <param name="action">トランザクションが必要なDBアクセスのアクション</param>
        /// <remarks>SqlDataAdapterのInsertCommand,UpdateCommand,DeleteCommandのトランザクションをサポートします。</remarks>
        public static void InvokeTableAdapterTran(SqlDataAdapter adapter, Func<bool> action)
        {
            if (adapter == null)
                throw new ArgumentNullException("adapter is null.");

            if (action == null)
                throw new ArgumentNullException("action is null.");

            // コネクションを取得
            SqlConnection con = GetConnection(adapter);

            // コネクションをオープン
            if (con.State != ConnectionState.Open)
                con.Open();

            // トランザクションの開始
            using (SqlTransaction tran = con.BeginTransaction())
            {
                // トランザクションを対象テーブルに反映
                SetTransactionCommands(adapter, tran);

                try
                {
                    if (action())
                        tran.Commit();
                    else
                        tran.Rollback();
                }
#if DEBUG
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    tran.Rollback();
                    throw;
                }
#else
                catch
                {
                    tran.Rollback();
                    throw;
                }
#endif
            }
        }

        #endregion

        #region protected method

        #endregion

        #region internal method

        /// <summary>
        /// 新しい<c>SqlConnection</c>を構築します。
        /// </summary>
        /// <param name="connectionString">データベースを開くために使用する文字列を取得または設定します。</param>
        /// <returns><see cref="System.Data.SqlClient.SqlConnection"/></returns>
        internal static SqlConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(ConnectionString);
        }

        #endregion

        #region private method

        /// <summary>
        /// <c>SqlDataAdapter</c>に対してこのクラスが管理する<c>SqlConnection</c>を割り当てます。
        /// </summary>
        /// <param name="adapter"><c>SqlConnection</c>を取得したい<c>SqlDataAdapter</c></param>
        /// <returns><c>SqlDataAdapter</c>から取得した<c>SqlConnection</c></returns>
        private static SqlConnection GetConnection(SqlDataAdapter adapter)
        {
            return adapter.SelectCommand.Connection;
        }

        /// <summary>
        /// <c>SqlTransaction</c>を<c>SqlCommand</c>に割り当てます。
        /// </summary>
        /// <param name="adapter">トランザクションを割り当てたい<c>SqlDataAdapter</c></param>
        /// <param name="transaction"><c>SqlCommand</c>に割り当てる<c>SqlTransaction</c></param>
        private static void SetTransactionCommands(SqlDataAdapter adapter, SqlTransaction transaction)
        {
            if (adapter.InsertCommand != null)
                adapter.InsertCommand.Transaction = transaction;

            if (adapter.UpdateCommand != null)
                adapter.UpdateCommand.Transaction = transaction;

            if (adapter.DeleteCommand != null)
                adapter.DeleteCommand.Transaction = transaction;
        }

        #endregion

        #region delegate

        #endregion
    }
}
