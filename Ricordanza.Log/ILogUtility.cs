using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Ricordanza.Log
{
    /// <summary>
    /// ILogの拡張メソッドクラス。</br>
    /// リリースモード時は子のメソッドの呼び出しが行われません。
    /// </summary>
    #pragma warning disable 612
    public static class ILogUtility
    {
        /// <summary>
        /// デバックログを出力します。
        /// </summary>
        /// <param name="self"><see cref="Ricordanza.Log.ILog"/>オブジェクト</param>
        /// <param name="message">ログメッセージ</param>
        [Conditional("DEBUG")]
        public static void Debug(this ILog self, object message)
        {
            self.X_DebugMessage(message);
        }

        /// <summary>
        /// デバックログを出力します。
        /// </summary>
        /// <param name="self"><see cref="Ricordanza.Log.ILog"/>オブジェクト</param>
        /// <param name="message">ログメッセージ</param>
        /// <param name="e">例外</param>
        [Conditional("DEBUG")]
        public static void Debug(this ILog self, object message, Exception e)
        {
            self.X_DebugException(message, e);
        }

        /// <summary>
        /// デバックログを出力します。
        /// </summary>
        /// <param name="self"><see cref="Ricordanza.Log.ILog"/>オブジェクト</param>
        /// <param name="format">0 個以上の書式項目を含んだ string</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ object 配列</param>
        [Conditional("DEBUG")]
        public static void DebugFormat(this ILog self, string format, params object[] args)
        {
            self.X_DebugFormat(format, args);
        }
    }
    #pragma warning restore 612
}