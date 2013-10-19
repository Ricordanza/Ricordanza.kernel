using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ricordanza.Core.Utilities
{
    /// <summary>
    /// メソッド操作のユーティリティクラスです。
    /// </summary>
    /// <remarks>このクラスは<see cref="System.Reflection.MemberInfo"/>を拡張します。</remarks>
    public static class MethodUtility
    {
        #region public method

        /// <summary>
        /// 不要文字を除去したメソッド名を取得します。
        /// </summary>
        /// <param name="method">不要文字を除去したいメソッド</param>
        /// <returns>不要文字を除去したメソッド</returns>
        public static string RealName(this MethodInfo method)
        {
            if (method == null)
                return string.Empty;

            string methodName = method.Name;

            // 括弧の除去
            int pos = method.Name.IndexOf('<');
            if (pos > -1)
            {
                methodName = method.Name.Substring(pos + 1);

                pos = methodName.IndexOf('>');
                if (pos > -1)
                    methodName = methodName.Substring(0, pos);
            }

            return methodName;
        }

        #endregion
    }

}
