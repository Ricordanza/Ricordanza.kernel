using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ricordanza.WinFormsUI.Forms
{
    /// <summary>
    /// 複数行表示形式のTreeViewコントロールです。
    /// </summary>
    public class CoreTreeView
        : TreeView
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// ラベルを複数行表示するかどうかのフラグ
        /// </summary>
        private bool _multiLineLabel;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public CoreTreeView()
            : base()
        {
            MultiLineLabel = false;
        }

        #endregion

        #region property

        /// <summary>
        /// ラベルを複数行表示するかどうかのフラグ
        /// </summary>
        [Category("Ricordanza")]
        [Description("ラベルの複数行表示を取得または設定します。")]
        [DefaultValue(false)]
        public bool MultiLineLabel
        {
            set
            {
                _multiLineLabel = value;
                if (_multiLineLabel)
                    this.DrawMode = TreeViewDrawMode.OwnerDrawText;
                else
                    this.DrawMode = TreeViewDrawMode.Normal;
            }
            get
            {
                return _multiLineLabel;
            }
        }

        #endregion

        #region event

        #endregion

        #region event handler

        /// <summary>
        /// DrawNodeイベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータを格納している<see cref="System.Windows.Forms.DrawTreeNodeEventArgs"/>。</param>
        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            if (!MultiLineLabel)
                base.OnDrawNode(e);
            else
                TextRenderer.DrawText(e.Graphics, e.Node.Text, this.Font, e.Bounds, this.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
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
    }
}
