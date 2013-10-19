using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ricordanza.Core.Utilities
{
    /// <summary>
    /// ダイアログボックスの戻り値ユーティリティクラスです。
    /// </summary>
    /// <remarks>このクラスは<see cref="System.Windows.Forms.DialogResult"/>を拡張します。</remarks>
    /// <example>
    /// <code>
    /// var form = new Form();
    /// if(form.ShowDialog().IsOK())
    /// {
    ///     ･･･
    /// }
    /// </code>
    /// </example>
    public static class DialogResultUtility
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
        /// ダイアログボックスの戻り値を示す識別子が<c>Abort</c>か判定します。
        /// </summary>
        /// <param name="result">ダイアログボックスの戻り値</param>
        /// <returns><c>Abort</c>の場合は<c>true</c>、それ以外の場合は<c>false</c>。</returns>
        public static bool IsAbort(this DialogResult result)
        {
            return result == DialogResult.Abort;
        }

        /// <summary>
        /// ダイアログボックスの戻り値を示す識別子が<c>Cancel</c>か判定します。
        /// </summary>
        /// <param name="result">ダイアログボックスの戻り値</param>
        /// <returns><c>Cancel</c>の場合は<c>true</c>、それ以外の場合は<c>false</c>。</returns>
        public static bool IsCancel(this DialogResult result)
        {
            return result == DialogResult.Cancel;
        }

        /// <summary>
        /// ダイアログボックスの戻り値を示す識別子が<c>Ignore</c>か判定します。
        /// </summary>
        /// <param name="result">ダイアログボックスの戻り値</param>
        /// <returns><c>Ignore</c>の場合は<c>true</c>、それ以外の場合は<c>false</c>。</returns>
        public static bool IsIgnore(this DialogResult result)
        {
            return result == DialogResult.Ignore;
        }

        /// <summary>
        /// ダイアログボックスの戻り値を示す識別子が<c>No</c>か判定します。
        /// </summary>
        /// <param name="result">ダイアログボックスの戻り値</param>
        /// <returns><c>No</c>の場合は<c>true</c>、それ以外の場合は<c>false</c>。</returns>
        public static bool IsNo(this DialogResult result)
        {
            return result == DialogResult.No;
        }

        /// <summary>
        /// ダイアログボックスの戻り値を示す識別子が<c>None</c>か判定します。
        /// </summary>
        /// <param name="result">ダイアログボックスの戻り値</param>
        /// <returns><c>None</c>の場合は<c>true</c>、それ以外の場合は<c>false</c>。</returns>
        public static bool IsNone(this DialogResult result)
        {
            return result == DialogResult.None;
        }

        /// <summary>
        /// ダイアログボックスの戻り値を示す識別子が<c>OK</c>か判定します。
        /// </summary>
        /// <param name="result">ダイアログボックスの戻り値</param>
        /// <returns><c>OK</c>の場合は<c>true</c>、それ以外の場合は<c>false</c>。</returns>
        public static bool IsOK(this DialogResult result)
        {
            return result == DialogResult.OK;
        }

        /// <summary>
        /// ダイアログボックスの戻り値を示す識別子が<c>Retry</c>か判定します。
        /// </summary>
        /// <param name="result">ダイアログボックスの戻り値</param>
        /// <returns><c>Retry</c>の場合は<c>true</c>、それ以外の場合は<c>false</c>。</returns>
        public static bool IsRetry(this DialogResult result)
        {
            return result == DialogResult.Retry;
        }

        /// <summary>
        /// ダイアログボックスの戻り値を示す識別子が<c>Yes</c>か判定します。
        /// </summary>
        /// <param name="result">ダイアログボックスの戻り値</param>
        /// <returns><c>Yes</c>の場合は<c>true</c>、それ以外の場合は<c>false</c>。</returns>
        public static bool IsYes(this DialogResult result)
        {
            return result == DialogResult.Yes;
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
