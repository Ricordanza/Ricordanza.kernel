using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ricordanza.Core.Utilities
{
    /// <summary>
    /// 文字列操作のユーティリティクラスです。
    /// </summary>
    /// <remarks>このクラスは<see cref="System.Exception"/>を拡張します。</remarks>
    public static class ExceptionUtility
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
        /// 例外メッセージをダイアログに表示します。
        /// </summary>
        /// <param name="ex">例外メッセージを表示したい<see cref="System.Exception"/></param>
        public static void ShowDialog(this Exception ex)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("[Application stoped operation]");
            s.AppendLine();
            s.AppendLine(ex.ToString());

            MessageBox.Show(s.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
