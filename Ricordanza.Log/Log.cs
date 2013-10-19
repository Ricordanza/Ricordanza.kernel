using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.Log
{
    /// <summary>
    /// ログオブジェクト
    /// </summary>
    internal sealed class Log : ILog
    {
        #region constant

        #endregion

        #region private variable

        /// <summary>
        /// log4net.ILogオブジェクト
        /// </summary>
        internal readonly log4net.ILog logger;

        #endregion

        #region property

        /// <summary>
        /// Debugが有効か判定します。
        /// </summary>
        public bool IsDebugEnabled
        {
            get
            {
                if (logger == null)
                    return false;

                return logger.IsDebugEnabled;
            }
        }

        /// <summary>
        /// Infoが有効か判定します。
        /// </summary>
        public bool IsInfoEnabled
        {
            get
            {
                if (logger == null)
                    return false;

                return logger.IsInfoEnabled;
            }
        }

        /// <summary>
        /// Warnが有効か判定します。
        /// </summary>
        public bool IsWarnEnabled
        {
            get
            {
                if (logger == null)
                    return false;

                return logger.IsWarnEnabled;
            }
        }

        /// <summary>
        /// Errorが有効か判定します。
        /// </summary>
        public bool IsErrorEnabled
        {
            get
            {
                if (logger == null)
                    return false;

                return logger.IsErrorEnabled;
            }
        }

        /// <summary>
        /// Fatalが有効か判定します。
        /// </summary>
        public bool IsFatalEnabled
        {
            get
            {
                if (logger == null)
                    return false;

                return logger.IsFatalEnabled;
            }
        }

        #endregion

        #region constructor

        /// <summary>
        /// log4net.ILogオブジェクトを元にログ出力クラスを作成します。
        /// </summary>
        /// <param name="logger">log4net.ILogオブジェクト</param>
        internal Log(log4net.ILog logger)
        {
            this.logger = logger;
        }

        #endregion

        #region public method

        #region debug

        /// <summary>
        /// Debugログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void Debug(object message)
        {
            if (logger == null)
                return;

            logger.Debug(message);
        }

        /// <summary>
        /// Debugログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="exception">例外</param>
        public void Debug(object message, Exception exception)
        {
            if (logger == null)
                return;

            logger.Debug(message, exception);
        }

        /// <summary>
        /// Debugログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg">書式設定対象オブジェクト</param>
        public void DebugFormat(string format, object arg0)
        {
            if (logger == null)
                return;

            logger.DebugFormat(format, arg0);
        }

        /// <summary>
        /// Debugログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        public void DebugFormat(string format, params object[] args)
        {
            if (logger == null)
                return;

            logger.DebugFormat(format, args);
        }

        /// <summary>
        /// Debugログを出力します。
        /// </summary>
        /// <param name="provider">書式を制御するオブジェクト</param>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (logger == null)
                return;

            logger.DebugFormat(provider, format, args);
        }

        /// <summary>
        /// Debugログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        public void DebugFormat(string format, object arg0, object arg1)
        {
            if (logger == null)
                return;

            logger.DebugFormat(format, arg0, arg1);
        }

        /// <summary>
        /// Debugログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        /// <param name="arg2">書式設定対象オブジェクト</param>
        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            if (logger == null)
                return;

            logger.DebugFormat(format, arg0, arg1, arg2);
        }

        #endregion

        #region info

        /// <summary>
        /// Infoログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void Info(object message)
        {
            if (logger == null)
                return;

            logger.Info(message);
        }

        /// <summary>
        /// Infoログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="exception">例外</param>
        public void Info(object message, Exception exception)
        {
            if (logger == null)
                return;

            logger.Info(message, exception);
        }

        /// <summary>
        /// Infoログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg">書式設定対象オブジェクト</param>
        public void InfoFormat(string format, object arg0)
        {
            if (logger == null)
                return;

            logger.InfoFormat(format, arg0);
        }

        /// <summary>
        /// Infoログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        public void InfoFormat(string format, params object[] args)
        {
            if (logger == null)
                return;

            logger.InfoFormat(format, args);
        }

        /// <summary>
        /// Infoログを出力します。
        /// </summary>
        /// <param name="provider">書式を制御するオブジェクト</param>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (logger == null)
                return;

            logger.InfoFormat(provider, format, args);
        }

        /// <summary>
        /// Infoログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        public void InfoFormat(string format, object arg0, object arg1)
        {
            if (logger == null)
                return;

            logger.InfoFormat(format, arg0, arg1);
        }

        /// <summary>
        /// Infoログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        /// <param name="arg2">書式設定対象オブジェクト</param>
        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            if (logger == null)
                return;

            logger.InfoFormat(format, arg0, arg1, arg2);
        }

        #endregion

        #region warn

        /// <summary>
        /// Warnログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void Warn(object message)
        {
            if (logger == null)
                return;

            logger.Warn(message);
        }

        /// <summary>
        /// Warnログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="exception">例外</param>
        public void Warn(object message, Exception exception)
        {
            if (logger == null)
                return;

            logger.Warn(message, exception);
        }

        /// <summary>
        /// Warnログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg">書式設定対象オブジェクト</param>
        public void WarnFormat(string format, object arg0)
        {
            if (logger == null)
                return;

            logger.WarnFormat(format, arg0);
        }

        /// <summary>
        /// Warnログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        public void WarnFormat(string format, params object[] args)
        {
            if (logger == null)
                return;

            logger.WarnFormat(format, args);
        }

        /// <summary>
        /// Warnログを出力します。
        /// </summary>
        /// <param name="provider">書式を制御するオブジェクト</param>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (logger == null)
                return;

            logger.WarnFormat(provider, format, args);
        }

        /// <summary>
        /// Warnログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        public void WarnFormat(string format, object arg0, object arg1)
        {
            if (logger == null)
                return;

            logger.WarnFormat(format, arg0, arg1);
        }

        /// <summary>
        /// Warnログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        /// <param name="arg2">書式設定対象オブジェクト</param>
        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            if (logger == null)
                return;

            logger.WarnFormat(format, arg0, arg1, arg2);
        }

        #endregion

        #region error

        /// <summary>
        /// Errorログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void Error(object message)
        {
            if (logger == null)
                return;

            logger.Error(message);
        }

        /// <summary>
        /// Errorログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="exception">例外</param>
        public void Error(object message, Exception exception)
        {
            if (logger == null)
                return;

            logger.Error(message, exception);
        }

        /// <summary>
        /// Errorログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg">書式設定対象オブジェクト</param>
        public void ErrorFormat(string format, object arg0)
        {
            if (logger == null)
                return;

            logger.ErrorFormat(format, arg0);
        }

        /// <summary>
        /// Errorログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        public void ErrorFormat(string format, params object[] args)
        {
            if (logger == null)
                return;

            logger.ErrorFormat(format, args);
        }

        /// <summary>
        /// Errorログを出力します。
        /// </summary>
        /// <param name="provider">書式を制御するオブジェクト</param>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (logger == null)
                return;

            logger.ErrorFormat(provider, format, args);
        }

        /// <summary>
        /// Errorログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        public void ErrorFormat(string format, object arg0, object arg1)
        {
            if (logger == null)
                return;

            logger.ErrorFormat(format, arg0, arg1);
        }

        /// <summary>
        /// Errorログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        /// <param name="arg2">書式設定対象オブジェクト</param>
        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            if (logger == null)
                return;

            logger.ErrorFormat(format, arg0, arg1, arg2);
        }

        #endregion

        #region fatal

        /// <summary>
        /// Fatalログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void Fatal(object message)
        {
            if (logger == null)
                return;

            logger.Fatal(message);
        }

        /// <summary>
        /// Fatalログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="exception">例外</param>
        public void Fatal(object message, Exception exception)
        {
            if (logger == null)
                return;

            logger.Fatal(message, exception);
        }

        /// <summary>
        /// Fatalログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg">書式設定対象オブジェクト</param>
        public void FatalFormat(string format, object arg0)
        {
            if (logger == null)
                return;

            logger.FatalFormat(format, arg0);
        }

        /// <summary>
        /// Fatalログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        public void FatalFormat(string format, params object[] args)
        {
            if (logger == null)
                return;

            logger.FatalFormat(format, args);
        }

        /// <summary>
        /// Fatalログを出力します。
        /// </summary>
        /// <param name="provider">書式を制御するオブジェクト</param>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (logger == null)
                return;

            logger.FatalFormat(provider, format, args);
        }

        /// <summary>
        /// Fatalログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        public void FatalFormat(string format, object arg0, object arg1)
        {
            if (logger == null)
                return;

            logger.FatalFormat(format, arg0, arg1);
        }

        /// <summary>
        /// Fatalログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="arg0">書式設定対象オブジェクト</param>
        /// <param name="arg1">書式設定対象オブジェクト</param>
        /// <param name="arg2">書式設定対象オブジェクト</param>
        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            if (logger == null)
                return;

            logger.FatalFormat(format, arg0, arg1, arg2);
        }

        #endregion

        #region x_

        /// <summary>
        /// デバックログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void X_DebugMessage(object message)
        {
            if (logger == null)
                return;

            logger.Debug(message);
        }

        /// <summary>
        /// デバックログを出力します。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="e">例外</param>
        public void X_DebugException(object message, Exception exception)
        {
            if (logger == null)
                return;

            logger.Debug(message, exception);
        }

        /// <summary>
        /// デバックログを出力します。
        /// </summary>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        public void X_DebugFormat(string format, params object[] args)
        {
            if (logger == null)
                return;

            logger.DebugFormat(format, args);
        }

        #endregion

        #endregion

        #region private method

        #endregion
    }
}
