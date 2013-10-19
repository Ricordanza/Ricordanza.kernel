using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ricordanza.Resource
{
    /// <summary>
    /// コントロールにリソースのバインド機能を提供します。<br />
    /// この機能を有効にする為には<c>control</c>という<c>category</c>が必要です。<br />
    /// <br />
    /// リソースの定義方法<br />
    /// &lt;/key&gtに<c>Form</c>の<c>GetType().FullName</c> + <c>.</c> + <c>Control</c>の<c>Name</c>を定義する。
    /// <example>
    /// &lt;resources category="conrol"&gt;
    ///   &lt;resource key="Ricordanza.Mock.Forms.SampleForm.nameLabel"&gt;Name&lt;/resource&gt;
    /// &lt;/resources&gt;
    /// </example>
    /// </summary>
    public static class ResourceBind
    {
        #region const

        /// <summary>
        /// バインド用リソースカテゴリーキー
        /// </summary>
        public const string RESOURCE_BIND_CATEGORY = "control";

        /// <summary>
        /// バインド用リソースキーフォーマット
        /// </summary>
        public const string RESOURCE_BIND_FORMAT = "{0}.{1}";

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
        /// コントロールに項目のリソースを反映します。 
        /// </summary>
        /// <param name="owner">オーナーコントロール</param>
        /// <param name="controls">Control.ControlCollection</param>
        /// <remarks></remarks>
        public static void BindResource(this Form owner, Control.ControlCollection controls)
        {
            // コントロールへのリソースの反映
            foreach (Control control in controls)
            {
                string value = ResourceManager.Get(RESOURCE_BIND_CATEGORY,
                    string.Format(RESOURCE_BIND_CATEGORY, owner.GetType().FullName, control.Name));

                if (!string.IsNullOrEmpty(value))
                    control.Text = value;

                if (control.Controls.Count > 0)
                    BindResource(owner, control.Controls);

                // ToolStripは子コントロールの保持方法が固有である為、個別の呼び出しを行なう。
                if (control is ToolStrip)
                {
                    ToolStrip toolStrip = control as ToolStrip;

                    if (toolStrip.Items.Count > 0)
                        BindToolStripResource(owner, toolStrip.Items);
                }
                // ContextMenuStripは子コントロールの保持方法が固有である為、個別の呼び出しを行なう。
                else if (control is ContextMenuStrip)
                {
                    ToolStrip contextStrip = control as ToolStrip;

                    if (contextStrip.Items.Count > 0)
                        BindToolStripResource(owner, contextStrip.Items);
                }
                // MenuStripは子コントロールの保持方法が固有である為、個別の呼び出しを行なう。
                else if (control is MenuStrip)
                {
                    MenuStrip menuStrip = control as MenuStrip;

                    if (menuStrip.Items.Count > 0)
                        BindToolStripResource(owner, menuStrip.Items);
                }
                // TabControlは子コントロールの保持方法が固有である為、個別の呼び出しを行なう。
                else if (control is TabControl)
                {
                    TabControl tab = control as TabControl;

                    if (tab.TabPages.Count > 0)
                        BindTabResource(owner, tab.TabPages);
                }
            }
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        /// <summary>
        /// ToolStripコントロールに項目のリソースを反映します
        /// </summary>
        /// <param name="owner">オーナーコントロール</param>
        /// <param name="controls">ToolStripItemCollection</param>
        /// <remarks></remarks>
        private static void BindToolStripResource(this Form owner, ToolStripItemCollection controls)
        {
            foreach (ToolStripItem control in controls)
            {
                string value = ResourceManager.Get(RESOURCE_BIND_CATEGORY,
                    string.Format(RESOURCE_BIND_CATEGORY, owner.GetType().FullName, control.Name));

                if (!string.IsNullOrEmpty(value))
                    control.Text = value;

                if (control is ToolStripDropDownItem)
                {
                    ToolStripDropDownItem dropDownItem = control as ToolStripDropDownItem;

                    if (dropDownItem.DropDownItems.Count > 0)
                        BindToolStripResource(owner, dropDownItem.DropDownItems);
                }
            }
        }

        /// <summary>
        /// TabPageコントロールに項目のリソースを反映します
        /// </summary>
        /// <param name="owner">オーナーコントロール</param>
        /// <param name="controls">TabControl.TabPageCollection</param>
        /// <remarks></remarks>
        private static void BindTabResource(this Form owner, TabControl.TabPageCollection controls)
        {
            foreach (TabPage control in controls)
            {
                string value = ResourceManager.Get(RESOURCE_BIND_CATEGORY,
                    string.Format(RESOURCE_BIND_CATEGORY, owner.GetType().FullName, control.Name));

                if (!string.IsNullOrEmpty(value))
                    control.Text = value;

                if (control.Controls.Count > 0)
                    BindResource(owner, control.Controls);
            }
        }

        #endregion

        #region delegate

        #endregion
    }
}
