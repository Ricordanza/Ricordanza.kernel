using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ricordanza.WinFormsUI.Forms
{
    /// <summary>
    /// キーバインドを行うコントロールが実現します。
    /// </summary>
    public interface IBindKey
    {
        #region const

        #endregion

        #region protected variable

        #endregion

        #region property

        /// <summary>
        /// イベントを関連付けるキーを設定または取得します。
        /// </summary>
        [DefaultValue(typeof(Keys), "None")]
        [Category("Ricordanza")]
        [Description("イベントを関連付けるキーを設定または取得します。")]
        Keys BindKey { set; get; }

        /// <summary>
        /// 有効か判定します。
        /// </summary>
        [DefaultValue(true)]
        [Category("Ricordanza")]
        [Description("有効か判定します。")]
        bool Effective { get; }

        #endregion

        #region static constructor

        #endregion

        #region constructor

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// キー押下イベントです。
        /// </summary>
        void KeyHook();

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }
}
