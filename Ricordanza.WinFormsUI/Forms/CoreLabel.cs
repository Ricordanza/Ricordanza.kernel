using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Ricordanza.Core.Utilities;
using Ricordanza.WinFormsUI.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    /// <summary>
    /// 全てのラベルの規定クラスです。
    /// </summary>
    public class CoreLabel : Label
    {
        #region constant

        #endregion

        #region private variable

        #endregion

        #region protected variable

        #endregion

        #region property

        #endregion

        #region static constractor

        #endregion

        #region constractor

        #endregion

        #region event method

        /// <summary>
        /// TextChangedイベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している<see cref="System.EventArgs"/>。</param>
        protected override void OnTextChanged(EventArgs e)
        {
            if (!this.DesignMode && this.BackColor == Color.Transparent)
                this.UpdateRegion();

            if (InvokeRequired)
                this.Invoke(new MethodInvoker(() => base.OnTextChanged(e)));
            else
                base.OnTextChanged(e);
        }

        /// <summary>
        /// SizeChangedイベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している<see cref="System.EventArgs"/>。</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            if (!this.DesignMode && this.BackColor == Color.Transparent)
                this.UpdateRegion();

            if (InvokeRequired)
                this.Invoke(new MethodInvoker(() => base.OnSizeChanged(e)));
            else
                base.OnSizeChanged(e);
        }

        /// <summary>
        /// PaddingChangedイベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している<see cref="System.EventArgs"/>。</param>
        protected override void OnPaddingChanged(EventArgs e)
        {
            if (!this.DesignMode && this.BackColor == Color.Transparent)
                this.UpdateRegion();

            if (InvokeRequired)
                this.Invoke(new MethodInvoker(() => base.OnPaddingChanged(e)));
            else
                base.OnPaddingChanged(e);
        }

        /// <summary>
        /// PaintBackgroundイベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している<see cref="System.Windows.Forms.PaintEventArgs"/>。</param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (this.BackColor != Color.Transparent)
            {
                if (InvokeRequired)
                    this.Invoke(new MethodInvoker(() => base.OnPaintBackground(pevent)));
                else
                    base.OnPaintBackground(pevent);
            }
            else
            {
                if (this.DesignMode)
                {
                    if (InvokeRequired)
                        this.Invoke(new MethodInvoker(() => base.OnPaintBackground(pevent)));
                    else
                        base.OnPaintBackground(pevent);
                }
            }

            if (InvokeRequired)
                this.Invoke(new MethodInvoker(() => base.OnPaintBackground(pevent)));
            else
                base.OnPaintBackground(pevent);
        }

        /// <summary>
        /// Paintイベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している<see cref="System.Windows.Forms.PaintEventArgs"/>。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.BackColor != Color.Transparent)
            {
                if (InvokeRequired)
                    this.Invoke(new MethodInvoker(() => base.OnPaint(e)));
                else
                    base.OnPaint(e);
            }
            else
                this.DrawForeground(e.Graphics);
        }

        #endregion

        #region public method

        #endregion

        #region protected method

        #endregion

        #region private method

        /// <summary>
        /// 形状を更新します。
        /// </summary>
        private void UpdateRegion()
        {
            // コントロールの ClientSize と同じ大きさの Bitmap クラスを生成します。
            int width = (this.ClientSize.Width > 0) ? this.ClientSize.Width : 1;
            Bitmap foregroundBitmap = new Bitmap(width, this.ClientSize.Height);

            // 文字列などの背景以外の部分を描画します。
            using (Graphics g = Graphics.FromImage(foregroundBitmap))
                this.DrawForeground(g);

            int w = foregroundBitmap.Width;
            int h = foregroundBitmap.Height;

            Rectangle rect = new Rectangle(0, 0, w, h);
            Region region = new Region(rect);

            // できた Bitmap クラスからピクセルの色情報を取得します。
            BitmapData bd = foregroundBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = bd.Stride;
            int bytes = stride * h;
            byte[] bgraValues = new byte[bytes];
            Marshal.Copy(bd.Scan0, bgraValues, 0, bytes);
            foregroundBitmap.UnlockBits(bd);
            foregroundBitmap.Dispose();

            // 描画された部分だけの領域を作成します。
            int line;
            for (int y = 0; y < h; y++)
            {
                line = stride * y;
                for (int x = 0; x < w; x++)
                {
                    // アルファ値が 0 は背景
                    if (bgraValues[line + x * 4 + 3] == 0)
                        region.Exclude(new Rectangle(x, y, 1, 1));
                }
            }

            // Region に描画された領域を設定します。
            this.Region = region;
        }

        /// <summary>
        /// 前面を描画します。
        /// </summary>
        /// <param name="g">描画サーフェイス</param>
        private void DrawForeground(Graphics g)
        {
            using (SolidBrush sb = new SolidBrush(this.ForeColor))
            {
                Rectangle r = new Rectangle(
                    this.Padding.Left,
                    this.Padding.Top,
                    this.ClientRectangle.Width - this.Padding.Left - this.Padding.Right,
                    this.ClientRectangle.Height - this.Padding.Top - this.Padding.Bottom
                );

                g.DrawString(this.Text, this.Font, sb, r);
            }
        }

        #endregion

        #region delegate

        #endregion
    }
}
