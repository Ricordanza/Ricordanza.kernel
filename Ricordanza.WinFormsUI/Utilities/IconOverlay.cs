using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Ricordanza.WinFormsUI.Utilities
{
    #region IconOverlay

    /// <summary>
    /// IconOverlayを行うユーティリティクラスです。
    /// </summary>
    /// <example>
    /// オーバーレイ用のアイコンの追加<br />
    /// <code>
    /// // imageList1.Image[1] のイメージをオーバーレイアイコンインデックス値1で登録
    /// IconOverlay.AddOverLayImage(imageList, 1, 1);
    /// // imageList1.Image[3] のイメージをオーバーレイアイコンインデックス値2で登録
    /// IconOverlay.AddOverLayImage(imageList, 3, 2);
    /// 
    /// // インデックス値1のオーバーレイアイコンでアイコンオーバーレイを表示する
    /// treeNode.Overlay(1);
    /// // インデックス値2のオーバーレイアイコンでアイコンオーバーレイを表示する
    /// listViewItem.Overlay(2);
    /// </code>
    /// </example>
    public static class IconOverlay
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
        /// オーバーレイ用のアイコンのイメージを登録します。
        /// </summary>
        /// <param name="list">オーバーレイイメージを保持した<see cref="System.Windows.Forms.ImageList"/></param>
        /// <param name="imageListIndex">イメージリストのインデックス(開始値 0)</param>
        /// <param name="overlayIndex">オーバーレイインデックス(開始値 1)</param>
        public static void AddOverLayImage(ImageList list, int imageListIndex, int overlayIndex)
        {
            NativeMethods.ImageList_SetOverlayImage(list.Handle, imageListIndex, overlayIndex);
        }

        /// <summary>
        /// アイコンのオーバーレイを表示します。
        /// </summary>
        /// <param name="self">アイコンオーバーレイを表示させたい<see cref="System.Windows.Forms.TreeNode"/></param>
        /// <param name="overlayIndex">オーバーレイインデックス(開始値 1)</param>
        public static void Overlay(this TreeNode self, int overlayIndex)
        {
            if (self == null)
                throw new ArgumentNullException("self is null.");

            // TreeView_SetItemState(node.TreeView.Handle, node.Handle,
            //     overlayIndex << 8, TVIS_OVERLAYMASK); 相当の処理

            TVITEM tvi = new TVITEM();
            tvi.mask = NativeMethods.TVIF_STATE;
            tvi.hItem = self.Handle;
            tvi.stateMask = NativeMethods.TVIS_OVERLAYMASK;
            tvi.state = ((uint)overlayIndex << 8);

            NativeMethods.SendMessage(self.TreeView.Handle,
                NativeMethods.TVM_SETITEMW, 0, ref tvi);
        }

        /// <summary>
        /// アイコンのオーバーレイを表示します。
        /// </summary>
        /// <param name="self">アイコンオーバーレイを表示させたい<see cref="System.Windows.Forms.ListViewItem"/></param>
        /// <param name="overlayIndex">オーバーレイインデックス(開始値 1)</param>
        public static void Overlay(this ListViewItem self, int overlayIndex)
        {
            // ListView_SetItemState(listItem.ListView.Handle, listItem.Index,
            //     overlayIndex << 8, LVIS_OVERLAYMASK); 相当の処理

            LVITEM lvi = new LVITEM();
            lvi.stateMask = NativeMethods.LVIS_OVERLAYMASK;
            lvi.state = ((uint)overlayIndex << 8);

            NativeMethods.SendMessage(self.ListView.Handle,
                NativeMethods.LVM_SETITEMSTATE, self.Index, ref lvi);
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

    #region LVITEM

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct LVITEM
    {
        public uint mask;
        public int iItem;
        public int iSubItem;
        public uint state;
        public uint stateMask;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string pszText;
        public int cchTextMax;
        public int iImage;
        public uint lParam;
        public int iIndent;
    }

    #endregion

    #region TVITEM

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct TVITEM
    {
        public uint mask;
        public IntPtr hItem;
        public uint state;
        public uint stateMask;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string pszText;
        public int cchTextMax;
        public int iImage;
        public int iSelectedImage;
        public int cChildren;
        public uint lParam;
        public int iIntegral;
    }

    #endregion

    #region TVITEM

    /// <summary>
    /// Win32API及びその定数の定義クラス
    /// </summary>
    internal class NativeMethods
    {
        public const int TVIF_STATE = 0x0008;
        public const int TVIS_OVERLAYMASK = 0x0F00;
        public const int TVM_SETITEMW = 0x113F;
        public const int LVSIL_SMALL = 1;
        public const int LVIS_OVERLAYMASK = 0x0F00;
        public const int LVM_SETIMAGELIST = 0x1003;
        public const int LVM_SETITEMSTATE = 0x102B;
        [DllImport("comctl32.dll")]
        public static extern int ImageList_SetOverlayImage(IntPtr himl, int iImage, int iOverlay);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, ref TVITEM lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, ref LVITEM lParam);
        public NativeMethods()
        {
        }
    }

    #endregion
}
