using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.Log
{
    /// <summary>
    /// 文字列操作のユーティリティクラスです。
    /// </summary>
    /// <remarks>このクラスは<see cref="System.Exception"/>を拡張します。</remarks>
    public static class ExceptionUtility
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// ログオブジェクト
        /// </summary>
        static readonly ILog _log = LogPool.GetLog();

        #endregion

        #region static constructor

        #endregion

        #region constructor

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
        /// 例外メッセージをログに出力します。
        /// </summary>
        /// <param name="ex">例外メッセージをログ出力したい<see cref="System.Exception"/></param>
        public static void WriteFatal(this Exception ex)
        {
            _log.Fatal(ex);
        }

        /// <summary>
        /// エラーメッセージをログに出力します。
        /// </summary>
        /// <param name="ex">エラーメッセージをログ出力したい<see cref="System.Exception"/></param>
        public static void WriteError(this Exception ex)
        {
            _log.Error(ex);
        }

        /// <summary>
        /// 警告メッセージをログに出力します。
        /// </summary>
        /// <param name="ex">警告メッセージをログ出力したい<see cref="System.Exception"/></param>
        public static void WriteWarn(this Exception ex)
        {
            _log.Warn(ex);
        }

        /// <summary>
        /// 情報メッセージをログに出力します。
        /// </summary>
        /// <param name="ex">情報メッセージをログ出力したい<see cref="System.Exception"/></param>
        public static void WriteInfo(this Exception ex)
        {
            _log.Info(ex);
        }

        /// <summary>
        /// デバックメッセージをログに出力します。
        /// </summary>
        /// <param name="ex">デバックメッセージをログ出力したい<see cref="System.Exception"/></param>
        public static void WriteDebug(this Exception ex)
        {
            _log.Debug(ex);
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }
}
