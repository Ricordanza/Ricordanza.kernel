using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.ComponentModel;

using Ricordanza.Core.Utilities;
using Ricordanza.WinFormsUI.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    /// <summary>
    /// 全てのボタン基底クラスです。
    /// </summary>
    public class CoreButton : Button, IBindKey
    {
        #region const

        #endregion

        #region protected variable

        #endregion

        #region property

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// このクラスのインスタンスを構築します。
        /// </summary>
        public CoreButton()
        {
            this.BindKey = Keys.None;
        }

        #endregion

        #region event method

        #endregion

        #region public method

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion

        #region IBindKey

        /// <summary>
        /// イベントを関連付けるキーを設定または取得します。
        /// </summary>
        [DefaultValue(typeof(Keys), "None")]
        [Category("Ricordanza")]
        [Description("イベントを関連付けるキーを設定または取得します。")]
        public Keys BindKey { set; get; }

        /// <summary>
        /// 有効か判定します。
        /// </summary>
        [DefaultValue(true)]
        [Category("Ricordanza")]
        [Description("有効か判定します。")]
        public bool Effective { get { return (Enabled && Visible); } }

        /// <summary>
        /// キー押下イベントです。
        /// </summary>
        public void KeyHook()
        {
            this.PerformClick();
        }

        #endregion
    }
}