using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ricordanza.Property
{
    /// <summary>
    /// 指定されたオブジェクトから<see cref="Ricordanza.Property.Filed&lt;T&gt;"/>を取得します。
    /// </summary>
    /// <example>
    /// <code>
    /// foreach(var f in FieldFinder.Find&lt;int&gt;(this))
    /// {
    ///     ...
    /// }
    /// </code>
    /// </example>
    public static class FieldFinder
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
        /// 指定された型を情報を持つ<see cref="Ricordanza.Property.Filed&lt;T&gt;"/>を取得します。
        /// </summary>
        /// <typeparam name="T">取得したい型情報</typeparam>
        /// <param name="self"><see cref="Ricordanza.Property.Filed&lt;T&gt;"/>を取得したいオブジェクト</param>
        /// <returns><see cref="Ricordanza.Property.Filed&lt;T&gt;"/>反復子</returns>
        public static IEnumerable<Field<T>> Find<T>(this object self)
        {
            var list = new List<Field<T>>();

            if (self == null)
                return list;

            foreach (var f in self.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (f.FieldType == typeof(Field<T>))
                    list.Add(f.GetValue(self) as Field<T>);
            }

            return list;
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
