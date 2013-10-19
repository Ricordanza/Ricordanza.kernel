using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;

using log4net;

namespace Ricordanza.Log
{
    /// <summary>
    /// ログオブジェクト管理クラス。</br>
    /// ログオブジェクトを隠蔽化して可変性を確保する。
    /// </summary>
    /// <example>
    /// <code>
    /// static readonly ILog _log = LogPool.GetLog();
    /// </code>
    /// </example>
    public static class LogPool
    {
        /// <summary>
        /// スタックを戻る数。
        /// </summary>
        const int callerFrameIndex = 2;

        /// <summary>
        /// ログオブジェクトを取得します。
        /// </summary>
        /// <returns><see cref="Ricordanza.Log.ILog"/>オブジェクト</returns>
        [DynamicSecurityMethod]
        public static ILog GetLog()
        {
            // 呼び出し元情報を構築
            StackFrame callerFrame = new StackFrame(callerFrameIndex);
            MethodBase callerMethod = callerFrame.GetMethod();

            // 呼び出し元情報を元にログオブジェクトを構築する。
            return new Log(LogManager.GetLogger(Assembly.GetCallingAssembly(), callerMethod.DeclaringType.FullName));
        }
    }
}

namespace System.Security
{
    /// <summary>
    /// セキュリティ対策用カスタム属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    internal sealed class DynamicSecurityMethodAttribute : Attribute
    {
    }
}