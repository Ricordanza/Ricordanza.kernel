using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ricordanza.Resource
{
    /// <summary>
    /// リソースベースのメッセージボックスです。
    /// </summary>
    public static class ResourceBasedMessageBox
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
        /// メッセージボックスを表示します。
        /// </summary>
        /// <param name="resourceKey">リソースキー</param>
        /// <param name="paramters">プレースフォルダを置換するパラメータ</param>
        /// <returns>ボタン押下結果</returns>
        public static DialogResult Show(string resourceKey, params string[] prameters)
        {
            return Show(resourceKey, string.Empty, prameters);
        }

        /// <summary>
        /// メッセージボックスを表示します。
        /// </summary>
        /// <param name="resourceKey">リソースキー</param>
        /// <param name="caption">タイトル</param>
        /// <param name="paramters">プレースフォルダを置換するパラメータ</param>
        /// <returns>ボタン押下結果</returns>
        public static DialogResult Show(string resourceKey, string caption, params string[] prameters)
        {
            return Show(resourceKey, caption, MessageBoxButtons.OK, prameters);
        }

        /// <summary>
        /// メッセージボックスを表示します。
        /// </summary>
        /// <param name="resourceKey">リソースキー</param>
        /// <param name="caption">タイトル</param>
        /// <param name="button">表示するボタン</param>
        /// <param name="paramters">プレースフォルダを置換するパラメータ</param>
        /// <returns>ボタン押下結果</returns>
        public static DialogResult Show(string resourceKey, string caption, MessageBoxButtons button, params string[] prameters)
        {
            return Show(null, resourceKey, caption, button, prameters);
        }

        /// <summary>
        /// メッセージボックスを表示します。
        /// </summary>
        /// <param name="owner">オーナーウィンドウ</param>
        /// <param name="resourceKey">リソースキー</param>
        /// <param name="paramters">プレースフォルダを置換するパラメータ</param>
        /// <returns>ボタン押下結果</returns>
        public static DialogResult Show(IWin32Window owner, string resourceKey, params string[] prameters)
        {
            return Show(owner, resourceKey, string.Empty, prameters);
        }

        /// <summary>
        /// メッセージボックスを表示します。
        /// </summary>
        /// <param name="owner">オーナーウィンドウ</param>
        /// <param name="resourceKey">リソースキー</param>
        /// <param name="caption">タイトル</param>
        /// <param name="paramters">プレースフォルダを置換するパラメータ</param>
        /// <returns>ボタン押下結果</returns>
        public static DialogResult Show(IWin32Window owner, string resourceKey, string caption, params string[] prameters)
        {
            return Show(owner, resourceKey, caption, MessageBoxButtons.OK, prameters);
        }

        /// <summary>
        /// メッセージボックスを表示します。
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="resourceKey">リソースキー</param>
        /// <param name="caption">タイトル</param>
        /// <param name="button">表示するボタン</param>
        /// <param name="paramters">プレースフォルダを置換するパラメータ</param>
        /// <returns>ボタン押下結果</returns>
        public static DialogResult Show(IWin32Window owner, string resourceKey, string caption, MessageBoxButtons button, params string[] prameters)
        {
            // リソースキーが未定義の場合は処理しない
            if (string.IsNullOrEmpty(resourceKey))
                return DialogResult.Ignore;

            string icon = resourceKey.Substring(0, 1);
            MessageBoxIcon mIcon = MessageBoxIcon.None;
            switch (icon.ToLower())
            {
                case "i":
                    mIcon = MessageBoxIcon.Information;
                    break;
                case "e":
                    mIcon = MessageBoxIcon.Error;
                    break;
                case "w":
                    mIcon = MessageBoxIcon.Warning;
                    break;
                case "c":
                    mIcon = MessageBoxIcon.Question;
                    break;
            }

            return MessageBox.Show(owner, ResourceManager.Get(resourceKey, prameters), caption, button, mIcon);
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
