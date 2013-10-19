using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Ricordanza.Core.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    /// <summary>
    /// <c>Shell</c>のテーマを引き継いだ<see cref="System.Windows.Forms.TreeView"/>クラスです。
    /// </summary>
    public class ShellStyleTreeView
        : TreeView
    {
        #region const

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="pszSubAppName"></param>
        /// <param name="pszSubIdList"></param>
        /// <returns></returns>
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// 新しいこのクラスのインスタンスを構築します。
        /// </summary>
        public ShellStyleTreeView()
        {
            // 描画スタイルを設定
            this.SetStyle(
                ControlStyles.DoubleBuffer,          // 描画をバッファで実行する
                true                                 // 指定したスタイルを適用「する」
                );
        }

        #endregion

        #region property

        #endregion

        #region event

        #endregion

        #region event handler

        /// <summary>
        /// <c>System.Windows.Forms.Control.OnHandleCreated(System.EventArgs)</c>をオーバーライドします。
        /// </summary>
        /// <param name="e">イベントデータを格納している<see cref="System.EventArgs"/></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (!SystemUtility.IsDesignMode && Application.RenderWithVisualStyles && SystemUtility.OsWithUAC())
            {
                this.ShowLines = false;
                SetWindowTheme(this.Handle, "explorer", null);
            }
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
