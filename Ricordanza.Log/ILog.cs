using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.Log
{
    /// <summary>
    /// ログインターフェースです。
    /// </summary>
    public interface ILog
    {
        #region constant

        #endregion

        #region private variable

        #endregion

        #region property

        /// <summary>
        /// Debugが有効か判定します。
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// Infoが有効か判定します。
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        /// Warnが有効か判定します。
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        /// Errorが有効か判定します。
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// Fatalが有効か判定します。
        /// </summary>
        bool IsFatalEnabled { get; }

        #endregion

        #region public method

        /// <summary>
        /// Debugログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        void Debug(object message);

        /// <summary>
        /// Debugログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="exception">例外</param>
        void Debug(object message, Exception exception);

        /// <summary>
        /// Debugログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg">書式設定対象オブジェクト</param>
        void DebugFormat(string format, object arg0);

        /// <summary>
        /// Debugログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        void DebugFormat(string format, params object[] args);

        /// <summary>
        /// Debugログを出力します。
        /// </summary>
        /// <param name="provider">書式を制御するオブジェクト</param>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        void DebugFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Debugログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        void DebugFormat(string format, object arg0, object arg1);

        /// <summary>
        /// Debugログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        /// <param name="arg2">書式設定対象オブジェクト</param>
        void DebugFormat(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Infoログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        void Info(object message);

        /// <summary>
        /// Infoログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="exception">例外</param>
        void Info(object message, Exception exception);

        /// <summary>
        /// Infoログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg">書式設定対象オブジェクト</param>
        void InfoFormat(string format, object arg0);

        /// <summary>
        /// Infoログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        void InfoFormat(string format, params object[] args);

        /// <summary>
        /// Infoログを出力します。
        /// </summary>
        /// <param name="provider">書式を制御するオブジェクト</param>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        void InfoFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Infoログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        void InfoFormat(string format, object arg0, object arg1);

        /// <summary>
        /// Infoログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        /// <param name="arg2">書式設定対象オブジェクト</param>
        void InfoFormat(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Warnログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        void Warn(object message);

        /// <summary>
        /// Warnログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="exception">例外</param>
        void Warn(object message, Exception exception);

        /// <summary>
        /// Warnログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg">書式設定対象オブジェクト</param>
        void WarnFormat(string format, object arg0);

        /// <summary>
        /// Warnログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        void WarnFormat(string format, params object[] args);

        /// <summary>
        /// Warnログを出力します。
        /// </summary>
        /// <param name="provider">書式を制御するオブジェクト</param>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        void WarnFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Warnログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        void WarnFormat(string format, object arg0, object arg1);

        /// <summary>
        /// Warnログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        /// <param name="arg2">書式設定対象オブジェクト</param>
        void WarnFormat(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Errorログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        void Error(object message);

        /// <summary>
        /// Errorログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="exception">例外</param>
        void Error(object message, Exception exception);

        /// <summary>
        /// Errorログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg">書式設定対象オブジェクト</param>
        void ErrorFormat(string format, object arg0);

        /// <summary>
        /// Errorログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        void ErrorFormat(string format, params object[] args);

        /// <summary>
        /// Errorログを出力します。
        /// </summary>
        /// <param name="provider">書式を制御するオブジェクト</param>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        void ErrorFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Errorログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        void ErrorFormat(string format, object arg0, object arg1);

        /// <summary>
        /// Errorログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        /// <param name="arg2">書式設定対象オブジェクト</param>
        void ErrorFormat(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Fatalログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        void Fatal(object message);

        /// <summary>
        /// Fatalログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="exception">例外</param>
        void Fatal(object message, Exception exception);

        /// <summary>
        /// Fatalログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg">書式設定対象オブジェクト</param>
        void FatalFormat(string format, object arg0);

        /// <summary>
        /// Fatalログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        void FatalFormat(string format, params object[] args);

        /// <summary>
        /// Fatalログを出力します。
        /// </summary>
        /// <param name="provider">書式を制御するオブジェクト</param>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        void FatalFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Fatalログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        void FatalFormat(string format, object arg0, object arg1);

        /// <summary>
        /// Fatalログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        /// <param name="arg2">書式設定対象オブジェクト</param>
        void FatalFormat(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// デバックログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        [Obsolete]
        void X_DebugMessage(object message);

        /// <summary>
        /// デバックログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="e">例外</param>
        [Obsolete]
        void X_DebugException(object message, Exception e);

        /// <summary>
        /// デバックログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        [Obsolete]
        void X_DebugFormat(string format, params object[] args);

        #endregion

        #region private method

        #endregion
    }
}
