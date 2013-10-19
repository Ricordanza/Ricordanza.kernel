using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ricordanza.Resource
{
    #region ConfirmDialog

    /// <summary>
    /// 確認ダイアログです。
    /// </summary>
    internal partial class ConfirmDialog
        : Form
    {
        #region const

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        internal ConfirmDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region property

        /// <summary>
        /// 表示メッセージ
        /// </summary>
        public string Msg
        {
            set { msgLbl.Text = value; }
            get { return msgLbl.Text; }
        }

        /// <summary>
        /// 項目一覧
        /// </summary>
        public ListView.ListViewItemCollection Items
        {
            get { return listView.Items; }
        }

        #endregion

        #region event

        #endregion

        #region event handler

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

    #endregion

    #region ResouceBasedConfirm

    /// <summary>
    /// リソースベースの確認ダイアログです。
    /// </summary>
    public static class ResouceBasedConfirm
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
        /// 確認ダイアログを表示します。
        /// </summary>
        /// <param name="caption">タイトル</param>
        /// <param name="items">項目一覧</param>
        /// <param name="resourceKey">リソースキー</param>
        /// <param name="paramters">プレースフォルダを置換するパラメータ</param>
        /// <returns>ボタン押下結果</returns>
        public static DialogResult Show(string caption, string[] items, string resourceKey, params string[] prameters)
        {
            return Show(null, caption, items, resourceKey, prameters);
        }

        /// <summary>
        /// 確認ダイアログを表示します。
        /// </summary>
        /// <param name="owner">オーナーウィンドウ</param>
        /// <param name="caption">タイトル</param>
        /// <param name="items">項目一覧</param>
        /// <param name="resourceKey">リソースキー</param>
        /// <param name="paramters">プレースフォルダを置換するパラメータ</param>
        /// <returns>ボタン押下結果</returns>
        public static DialogResult Show(IWin32Window owner, string caption, string[] items, string resourceKey, params string[] prameters)
        {
            using (ConfirmDialog dialog = new ConfirmDialog())
            {
                dialog.Msg = ResourceManager.Get(resourceKey, prameters);
                dialog.Text = caption;
                items = items ?? (items = new string[] { });
                (items ?? (items = new string[] { })).ToList().ForEach(item => dialog.Items.Add(item));
                return dialog.ShowDialog();
            }
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }

    #endregion
}
