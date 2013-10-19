using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Ricordanza.Core.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    /// <summary>
    /// ビジュアルスタイルに対応したタブクラスです。
    /// </summary>
    public class CoreTabControl
        : TabControl
    {
        #region const

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// このクラスの新しいインスタンスを構築します。
        /// </summary>
        public CoreTabControl()
            : base()
        {
            // 描画スタイルを設定
            this.SetStyle(
                ControlStyles.DoubleBuffer |         // 描画をバッファで実行する
                ControlStyles.ResizeRedraw |         // リサイズ時に再描画を行う   
                ControlStyles.AllPaintingInWmPaint,  // WM_ERASEBKGND を無視する
                true                                 // 指定したスタイルを適用「する」
                );

            this.ItemSize = new Size(80, 18);
            this.Appearance = TabAppearance.Normal;
            this.Multiline = true;

            if (!this.DesignMode && Application.RenderWithVisualStyles && SystemUtility.OsWithUAC())
            {
                this.SetStyle(ControlStyles.UserPaint, true);
                // ControlStyles.UserPaintをTrueするとSizeModeは強制的にTabSizeMode.Fixedにされる
                this.SizeMode = TabSizeMode.Fixed;
            }
        }
        
        #endregion

        #region property

        #endregion

        #region event

        #endregion

        #region event handler

        /// <summary>
        /// Paintイベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータを格納している<see cref="System.Windows.Forms.PaintEventArgs"/>。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!this.DesignMode && Application.RenderWithVisualStyles && SystemUtility.OsWithUAC())
            {

                // TabControlの背景を塗る
                e.Graphics.FillRectangle(SystemBrushes.Control, this.ClientRectangle);

                if (this.TabPages.Count == 0)
                    return;

                // TabPageの枠を描画する
                TabPage page = this.TabPages[this.SelectedIndex];
                Rectangle pageRect = new Rectangle(
                    page.Bounds.X - 2,
                    page.Bounds.Y - 2,
                    page.Bounds.Width + 5,
                    page.Bounds.Height + 5);
                TabRenderer.DrawTabPage(e.Graphics, pageRect);

                // タブを描画する
                for (int i = 0; i < this.TabPages.Count; i++)
                {
                    page = this.TabPages[i];
                    Rectangle tabRect = this.GetTabRect(i);

                    // 表示するタブの状態を決定する
                    System.Windows.Forms.VisualStyles.TabItemState state;
                    if (!this.Enabled)
                        state = System.Windows.Forms.VisualStyles.TabItemState.Disabled;
                    else if (this.SelectedIndex == i)
                        state = System.Windows.Forms.VisualStyles.TabItemState.Selected;
                    else
                        state = System.Windows.Forms.VisualStyles.TabItemState.Normal;

                    // 選択されたタブとページの間の境界線を消すために、
                    // 描画する範囲を大きくする
                    if (this.SelectedIndex == i)
                    {
                        if (this.Alignment == TabAlignment.Top)
                            tabRect.Height += 1;
                        else if (this.Alignment == TabAlignment.Bottom)
                        {
                            tabRect.Y -= 2;
                            tabRect.Height += 2;
                        }
                        else if (this.Alignment == TabAlignment.Left)
                            tabRect.Width += 1;
                        else if (this.Alignment == TabAlignment.Right)
                        {
                            tabRect.X -= 2;
                            tabRect.Width += 2;
                        }
                    }

                    // 画像のサイズを決定する
                    Size imgSize;
                    if (this.Alignment == TabAlignment.Left ||
                        this.Alignment == TabAlignment.Right)
                        imgSize = new Size(tabRect.Height, tabRect.Width);
                    else
                        imgSize = tabRect.Size;

                    // Bottomの時はTextを表示しない（Textを回転させないため）
                    string tabText = page.Text;
                    if (this.Alignment == TabAlignment.Bottom)
                        tabText = string.Empty;

                    // タブの画像を作成する
                    using (Bitmap bmp = new Bitmap(imgSize.Width, imgSize.Height))
                    {
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            // 高さに1足しているのは、下にできる空白部分を消すため
                            TabRenderer.DrawTabItem(g,
                                new Rectangle(0, 0, bmp.Width, bmp.Height + 1),
                                tabText,
                                page.Font,
                                false,
                                state);
                        }

                        //画像を回転する
                        if (this.Alignment == TabAlignment.Bottom)
                            bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        else if (this.Alignment == TabAlignment.Left)
                            bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        else if (this.Alignment == TabAlignment.Right)
                            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);

                        //Bottomの時はTextを描画する
                        if (this.Alignment == TabAlignment.Bottom)
                        {
                            using (StringFormat sf = new StringFormat())
                            {
                                sf.Alignment = StringAlignment.Center;
                                sf.LineAlignment = StringAlignment.Center;
                                using (Graphics g = Graphics.FromImage(bmp))
                                {
                                    g.DrawString(page.Text,
                                        page.Font,
                                        SystemBrushes.ControlText,
                                        new RectangleF(0, 0, bmp.Width, bmp.Height),
                                        sf);
                                }
                            }
                        }

                        //画像を描画する
                        e.Graphics.DrawImage(bmp, tabRect.X, tabRect.Y, bmp.Width, bmp.Height);
                    }
                }
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
