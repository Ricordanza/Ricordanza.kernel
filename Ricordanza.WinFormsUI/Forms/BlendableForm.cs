using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Ricordanza.WinFormsUI.Forms
{
    /// <summary>
    /// 背景画像でブレンドし、画面を表示するフォームクラスです。
    /// </summary>
    public partial class BlendableForm
        : CoreForm
    {
        #region Instance variables and constants

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="dwTime"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hWnd, int dwTime, AnimateStyles flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hDC"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hdc"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool DeleteDC(IntPtr hdc);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hDC"></param>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GetDC(IntPtr hWnd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hDC"></param>
        /// <returns></returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool UpdateLayeredWindow(
            IntPtr hwnd,
            IntPtr hdcDst,
            [System.Runtime.InteropServices.In()]
            ref Point pptDst,
            [System.Runtime.InteropServices.In()]
            ref Size psize,
            IntPtr hdcSrc,
            [System.Runtime.InteropServices.In()]
            ref Point pptSrc,
            Int32 crKey,
            [System.Runtime.InteropServices.In()] 
            ref BLENDFUNCTION pblend,
            Int32 dwFlags
            );

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        /// <summary>
        /// 
        /// </summary>
        private const int WS_EX_LAYERED = 0x80000;

        /// <summary>
        /// 
        /// </summary>
        private const int WS_BORDER = 0x800000;

        /// <summary>
        /// 
        /// </summary>
        private const int WS_THICKFRAME = 0x40000;

        /// <summary>
        /// 
        /// </summary>
        private const byte AC_SRC_OVER = 0;

        /// <summary>
        /// 
        /// </summary>
        private const byte AC_SRC_ALPHA = 1;

        /// <summary>
        /// 
        /// </summary>
        private const int ULW_ALPHA = 2;

        /// <summary>
        /// 
        /// </summary>
        private enum AnimateStyles
        {
            AW_SHOW = 0x00000000, // ウインドウを表示する
            AW_SLIDE = 0x00040000,
            AW_ACTIVATE = 0x00020000,
            AW_BLEND = 0x00080000, // 透明度を操作してウインドウを消したり表示する
            AW_HIDE = 0x00010000, // ウインドウを消す
            AW_CENTER = 0x00000010,
            AW_HOR_POSITIVE = 0x00000001, // 左から右へ ( AW_HOR_POSITIVE )
            AW_HOR_NEGATIVE = 0x00000002, // 右から左へ ( AW_HOR_NEGATIVE )
            AW_VER_POSITIVE = 0x00000004, // 上から下に向かって ( AW_VER_POSITIVE )
            AW_VER_NEGATIVE = 0x00000008  // 下から上に向かって ( AW_VER_NEGATIVE )
        }

        /// <summary>
        /// アルファ値
        /// </summary>
        private int _alpha = 0;

        /// <summary>
        /// ブレンド表示用フォーム
        /// </summary>
        private BlendCoreForm _blendForm = null;

        /// <summary>
        /// マウスクリックで画面を閉じるか否か
        /// </summary>
        private bool _mouseClickToClose = true;

        #endregion

        #region Constructors

        /// <summary>
        /// このクラスのインスタンスを生成します。
        /// </summary>
        public BlendableForm()
        {
            if (IsDesignMode)
            {
                return;
            }

            // 初期化
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            _blendForm = new BlendCoreForm();
            _blendForm.MouseClick += new MouseEventHandler(_blendForm_MouseClick);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// マウスクリックで画面を閉じるか否かを設定・取得します。
        /// </summary>
        [DefaultValue(true)]
        public bool MouseClickToClose
        {
            get { return _mouseClickToClose; }
            set { _mouseClickToClose = value; }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// ロード時の処理を行います。
        /// </summary>
        /// <param name="e">イベント情報</param>
        protected override void OnLoad(EventArgs e)
        {
            if (IsDesignMode)
            {
                return;
            }

            // 画面サイズを画像サイズに設定
            Image image = this.BackgroundImage;
            if (image != null)
            {
                this.Size = image.Size;
                _blendForm.Size = image.Size;
            }

            // 領域なし
            GraphicsPath path = new GraphicsPath();
            this.Region = new Region(path);

            // アルファ0で描画
            SetBackGroundLayer(0);

            // ロード処理を実行
            base.OnLoad(e);

            // ブレンド表示
            _blendForm.Show();
            _blendForm.Owner = this.Owner;
            this.Owner = _blendForm;

        }

        /// <summary>
        /// 画面表示時の処理を行います。
        /// </summary>
        /// <param name="e">イベント情報</param>
        protected override void OnShown(EventArgs e)
        {
            if (IsDesignMode)
            {
                return;
            }

            // フェードイン
            while (true)
            {
                System.Threading.Thread.Sleep(20);

                _alpha += 255 / 15;
                if (_alpha > 255)
                {
                    _alpha = 255;
                }

                SetBackGroundLayer(_alpha);

                if (_alpha >= 255)
                {
                    break;
                }
            }

            // コントロールのみの領域に
            GraphicsPath path = new GraphicsPath();
            foreach (Control control in this.Controls)
            {
                if (control.Parent == this)
                {
                    path.AddRectangle(new Rectangle(control.Location, control.Size));
                }
            }
            this.Region = new Region(path);

            // イベント通知
            base.OnShown(e);
        }

        /// <summary>
        /// 画面終了時の処理を行います。
        /// </summary>
        /// <param name="e">イベント情報</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 領域なし
            GraphicsPath path = new GraphicsPath();
            this.Region = new Region(path);

            // フェードアウト
            while (true)
            {
                SetBackGroundLayer(_alpha);

                _alpha -= 255 / 15;
                if (_alpha <= 0)
                {
                    break;
                }

                System.Threading.Thread.Sleep(20);
            }
            _blendForm.Hide();

            // イベント通知
            base.OnFormClosing(e);
        }

        /// <summary>
        /// サイズ変更時の処理を行います。
        /// </summary>
        /// <param name="e">イベント情報</param>
        protected override void OnResize(EventArgs e)
        {
            // サイズ変更時の処理を行う
            if (_blendForm != null)
            {
                _blendForm.Size = this.Size;
            }
            base.OnResize(e);
        }

        /// <summary>
        /// 画面移動時の処理を行います。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLocationChanged(EventArgs e)
        {
            // 位置変更時の処理を行う
            if (_blendForm != null)
            {
                _blendForm.Location = this.Location;
            }
            base.OnLocationChanged(e);
        }

        /// <summary>
        /// カーソル変更時の処理を行います。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCursorChanged(EventArgs e)
        {
            base.OnCursorChanged(e);
            if (_blendForm != null)
            {
                _blendForm.Cursor = this.Cursor;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// マウスクリック時の処理を行います。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント情報</param>
        private void _blendForm_MouseClick(object sender, MouseEventArgs e)
        {
            // 画面を閉じる
            if (_mouseClickToClose)
            {
                this.Close();
            }
        }

        /// <summary>
        /// デザインモードか否かを取得します。
        /// </summary>
        private bool IsDesignMode
        {
            get
            {
                return System.Reflection.Assembly.GetEntryAssembly() == null;
            }
        }

        /// <summary>
        /// 画面を背景画像でブレンド表示します。
        /// </summary>
        /// <param name="alpha">アルファ値</param>
        private void SetBackGroundLayer(int alpha)
        {

            // デバイスコンテキストを取得 
            Bitmap bmp = base.BackgroundImage as Bitmap;
            if (bmp == null)
            {
                return;
            }

            IntPtr screenDc = GetDC(IntPtr.Zero);
            IntPtr memDc = CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr hOldBitmap = IntPtr.Zero;

            try
            {
                hBitmap = bmp.GetHbitmap(Color.FromArgb(0));
                hOldBitmap = SelectObject(memDc, hBitmap);

                // BLENDFUNCTION を初期化 
                BLENDFUNCTION blend = new BLENDFUNCTION();
                blend.BlendOp = AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = (byte)alpha;
                blend.AlphaFormat = AC_SRC_ALPHA;

                Size size = new Size(bmp.Width, bmp.Height);
                Point pointDst = this.Location;
                Point pointSrc = new Point(0, 0);

                // レイヤードウィンドウを更新 
                bool r = UpdateLayeredWindow(_blendForm.Handle, screenDc, ref pointDst, ref size, memDc, ref pointSrc, 0, ref blend, ULW_ALPHA);
            }
            finally
            {
                ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    SelectObject(memDc, hOldBitmap);
                    DeleteObject(hBitmap);
                }
                DeleteDC(memDc);
            }

        }

        #endregion

        #region Event handlers

        #endregion

        #region BlendCoreForm

        /// <summary>
        /// ブレンド処理を行うための画面クラスです。
        /// </summary>
        private class BlendCoreForm : CoreForm
        {
            #region Instance variables and constants

            #endregion

            #region Constructors

            /// <summary>
            /// このクラスのインスタンスを生成します。
            /// </summary>
            public BlendCoreForm()
                : base()
            {
                // ボーダーなし
                this.FormBorderStyle = FormBorderStyle.None;
                this.ShowInTaskbar = false;
            }

            #endregion

            #region Public methods

            #endregion

            #region Protected methods

            /// <summary>
            /// ウィンドウスタイルを取得します。
            /// </summary>
            protected override CreateParams CreateParams
            {
                get
                {
                    // レイヤードウィンドウスタイルを適用 
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle = cp.ExStyle | WS_EX_LAYERED;
                    //cp.Style = cp.Style ^ WS_BORDER;
                    //cp.Style = cp.Style ^ WS_THICKFRAME;
                    return cp;
                }
            }
            #endregion

            #region Private methods

            #endregion

            #region Event handlers

            #endregion

        }

        #endregion
    }
}
