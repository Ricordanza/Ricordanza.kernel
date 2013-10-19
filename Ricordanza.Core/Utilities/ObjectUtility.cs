using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Ricordanza.Core.Utilities
{
    /// <summary>
    /// オブジェクト操作のユーティリティクラスです。
    /// </summary>
    /// <remarks>このクラスは<see cref="System.object"/>を拡張します。</remarks>
    public static class ObjectUtility
    {
        #region const

        #endregion

        #region private variable

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
        /// 引数valがnullか判定します。
        /// </summary>
        /// <param name="val">判定したい文字列</param>
        /// <returns><c>null</c>の場合は<c>true</c>。それ以外の場合は<c>false</c></returns>
        public static bool IsNulll(this object val)
        {
            return val == null;
        }

        /// <summary>
        /// 引数valがnullもしくは空文字の場合はdefaultValを返却します。
        /// 引数valがnullもしくは空文字でない場合は<c>string.Empty</c>を返却します。
        /// </summary>
        /// <param name="val">判定したい文字列</param>
        /// <returns><c>val</c> もしくは <c>defaultVal</c></returns>
        public static string EmptyToStr(this object val)
        {
            return EmptyToStr(val, string.Empty);
        }

        /// <summary>
        /// 引数valがnullもしくは空文字の場合はdefaultValを返却します。
        /// 引数valがnullもしくは空文字でない場合はvalを返却します。
        /// </summary>
        /// <param name="str">判定したい文字列</param>
        /// <param name="defaultVal">nullもしくは空文字の場合の返却値</param>
        /// <returns><c>val</c> もしくは <c>defaultVal</c></returns>
        public static string EmptyToStr(this object val, string defaultVal)
        {
            if (val == null || string.IsNullOrEmpty(val.ToString()))
                return defaultVal;

            return val.ToString();
        }

        /// <summary>
        /// 引数valがnullもしくはint型でない場合は<c>0</c>を返却します。
        /// 引数valがint型に変換可能な場合はvalをint型にした値を返却します。
        /// </summary>
        /// <param name="val">判定したい文字列</param>
        /// <returns><c>val</c> もしくは <c>defaultVal</c></returns>
        public static int EmptyToInt(this object val)
        {
            return EmptyToInt(val, 0);
        }

        /// <summary>
        /// 引数valがnullもしくはint型でない場合はdefaultValを返却します。
        /// 引数valがint型に変換可能な場合はvalをint型にした値を返却します。
        /// </summary>
        /// <param name="val">判定したい文字列</param>
        /// <param name="defaultVal">nullもしくは空文字の場合の返却値</param>
        /// <returns><c>val</c> もしくは <c>defaultVal</c></returns>
        public static int EmptyToInt(this object val, int defaultVal)
        {
            return EmptyToStr(val, string.Empty).ToInt(defaultVal);
        }

        /// <summary>
        /// 引数valがnullもしくはdecimal型でない場合は<c>0</c>を返却します。
        /// 引数valがdecimal型に変換可能な場合はvalをdecimal型にした値を返却します。
        /// </summary>
        /// <param name="val">判定したい文字列</param>
        /// <param name="defaultVal">nullもしくは空文字の場合の返却値</param>
        /// <returns><c>val</c> もしくは <c>defaultVal</c></returns>
        public static decimal EmptyToDecimal(this object val)
        {
            return EmptyToDecimal(val, 0);
        }

        /// <summary>
        /// 引数valがnullもしくはdecimal型でない場合はdefaultValを返却します。
        /// 引数valがdecimal型に変換可能な場合はvalをdecimal型にした値を返却します。
        /// </summary>
        /// <param name="val">判定したい文字列</param>
        /// <param name="defaultVal">nullもしくは空文字の場合の返却値</param>
        /// <returns><c>val</c> もしくは <c>defaultVal</c></returns>
        public static decimal EmptyToDecimal(this object val, decimal defaultVal)
        {
            return EmptyToStr(val, string.Empty).ToDecimal(defaultVal);
        }

        /// <summary>
        /// 引数valがnullもしくはbool型でない場合は<c>false</c>を返却します。
        /// 引数valがbool型に変換可能な場合はvalをbool型にした値を返却します。
        /// </summary>
        /// <param name="val">判定したい文字列</param>
        /// <returns><c>val</c> もしくは <c>defaultVal</c></returns>
        public static bool EmptyToBoolean(this object val)
        {
            return EmptyToBoolean(val, false);
        }

        /// <summary>
        /// 引数valがnullもしくはbool型でない場合はdefaultValを返却します。
        /// 引数valがbool型に変換可能な場合はvalをbool型にした値を返却します。
        /// </summary>
        /// <param name="val">判定したい文字列</param>
        /// <param name="defaultVal">nullもしくは空文字の場合の返却値</param>
        /// <returns><c>val</c> もしくは <c>defaultVal</c></returns>
        public static bool EmptyToBoolean(this object val, bool defaultVal)
        {
            return EmptyToStr(val, string.Empty).ToBoolean(defaultVal);
        }

        /// <summary>
        /// 引数valがnullもしくはDateTime型でない場合はdefaultValを返却します。
        /// 引数valがDateTime型に変換可能な場合はvalをDateTime型にした値を返却します。
        /// </summary>
        /// <param name="val">判定したい文字列</param>
        /// <returns><c>val</c> もしくは <c>defaultVal</c></returns>
        public static DateTime EmptyToDateTime(this object val)
        {
            return EmptyToDateTime(val, DateTime.Now);
        }

        /// <summary>
        /// 引数valがnullもしくはDateTime型でない場合はdefaultValを返却します。
        /// 引数valがDateTime型に変換可能な場合はvalをDateTime型にした値を返却します。
        /// </summary>
        /// <param name="val">判定したい文字列</param>
        /// <param name="defaultVal">nullもしくは空文字の場合の返却値</param>
        /// <returns><c>val</c> もしくは <c>defaultVal</c></returns>
        public static DateTime EmptyToDateTime(this object val, DateTime defaultVal)
        {
            return EmptyToStr(val, string.Empty).ToDateTime(defaultVal);
        }

        /// <summary>
        /// 匿名型か判定します。
        /// </summary>
        /// <param name="self">匿名型か判定したいオブジェクト</param>
        /// <returns>匿名型の場合は<c>true</c>、それ以外の場合は<c>false</c>。</returns>
        public static bool IsAnonymous(this object self)
        {
            if (self == null)
                return false;

            return self.GetType().IsAnonymous();
        }

        /// <summary>
        /// 匿名型か判定します。
        /// </summary>
        /// <param name="type">匿名型か判定したい型</param>
        /// <returns>匿名型の場合は<c>true</c>、それ以外の場合は<c>false</c>。</returns>
        public static bool IsAnonymous(this Type type)
        {
            if (type == null)
                return false;

            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false) &&
                type.IsGenericType &&
                type.Name.Contains("AnonymousType") &&
                (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$")) &&
                ((type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic) &&
                ((type.Attributes & TypeAttributes.Sealed) == TypeAttributes.Sealed);
        }

        /// <summary>
        /// 変数名を取得します。
        /// </summary>
        /// <typeparam name="T">変数名を取得したい実態の型</typeparam>
        /// <typeparam name="R">戻り値のデータ型</typeparam>
        /// <param name="self">変数名を保持するオブジェクト</param>
        /// <param name="expression">取得したい変数</param>
        /// <returns>変数名</returns>
        /// <remarks>
        /// <code>
        /// public class Hoge
        /// {
        ///     public string Name { get; set; }
        /// }
        /// 
        /// Hoge hoge = new Hoge();
        /// string name = hoge.GetMemberName(_ =&gt; _.Name);
        /// </code>
        /// </remarks>
        public static string GetMemberName<T, R>(this T self, Expression<Func<T, R>> expression)
        {
            if (@self == null)
                return string.Empty;

            return ((MemberExpression)expression.Body).Member.Name;
        }

        /// <summary>
        /// 型変換を行います。
        /// </summary>
        /// <typeparam name="T">変換後の型</typeparam>
        /// <param name="self">変換を行いたいオブジェクト</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <code>
        /// object o = "ABC";
        /// string s = o.Cast&lt;string&gt;();
        /// </code>
        /// </remarks>
        public static T Cast<T>(this object self)
        {
            return (T) self;
        }

        /// <summary>
        /// 型変換を行います。
        /// </summary>
        /// <typeparam name="T">変換後の型</typeparam>
        /// <param name="self">変換を行いたいオブジェクト</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <code>
        /// object o = 1;
        /// string s = o.As&lt;string&gt;();
        /// </code>
        /// </remarks>
        public static T As<T>(this object self) where T : class
        {
            return self as T;
        }

        /// <summary>
        /// ディープコピーを行います。
        /// </summary>
        /// <param name="self">ディープコピーを行いたいオブジェクト</param>
        /// <returns>ディープコピー後のオブジェクト</returns>
        /// <remarks>
        /// </remarks>
        public static object DeepCopy(this object self)
        {
            object result;

            using (MemoryStream mem = new MemoryStream())
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(mem, self);
                mem.Position = 0;
                result = b.Deserialize(mem);
            }

            return result;
        }

        /// <summary>
        /// ディープコピーを行います。
        /// </summary>
        /// <typeparam name="T">変換後の型</typeparam>
        /// <param name="self">ディープコピーを行いたいオブジェクト</param>
        /// <returns>ディープコピー後のオブジェクト</returns>
        /// <remarks>
        /// </remarks>
        public static T DeepCopy<T>(this object self) where T : class 
        {
            return self.DeepCopy().As<T>();
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
