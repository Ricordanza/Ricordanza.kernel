using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Ricordanza.WinFormsUI.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    /// <summary>
    /// グラデーションで背景を描画するパネルです。
    /// </summary>
    public class GradientionPanel : Panel
    {
        #region protected variable

        /// <summary>
        /// グラデーション開始色
        /// </summary>
        protected Color _backColor;

        /// <summary>
        /// グラデーション終了色
        /// </summary>
        protected Color _backColor2;

        /// <summary>
        /// コンポーネントの背景色のグラデーションの方向
        /// </summary>
        protected LinearGradientMode _GradientMode;

        #endregion

        #region property

        /// <summary>
        /// グラデーション開始色を設定または取得します。
        /// </summary>
        [Category("Ricordanza")]
        [Description("グラデーション開始色を設定または取得します。")]
        [DefaultValue(typeof(Color), "Transparent")]
        public new Color BackColor
        {
            get
            {
                if (this._backColor != Color.Empty)
                    return this._backColor;

                if (this.Parent != null)
                    return this.Parent.BackColor;

                return Control.DefaultBackColor;
            }
            set
            {
                this._backColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// グラデーション終了色を設定または取得します。
        /// </summary>
        [Category("Ricordanza")]
        [Description("グラデーション終了色を設定または取得します。")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color BackColor2
        {
            get
            {
                if (this._backColor2 != Color.Empty)
                    return this._backColor2;

                if (this.Parent != null)
                    return this.Parent.BackColor;

                return Control.DefaultBackColor;
            }
            set
            {
                this._backColor2 = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// コンポーネントの背景色のグラデーション方向を設定または取得します。
        /// </summary>
        [Category("Ricordanza")]
        [Description("コンポーネントの背景色のグラデーション方向を設定または取得します。")]
        [DefaultValue(typeof(LinearGradientMode), "Horizontal")]
        public LinearGradientMode GradientMode
        {
            get { return this._GradientMode; }
            set
            {
                this._GradientMode = value;
                this.Invalidate();
            }
        }

        #endregion

        #region constractor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public GradientionPanel()
        {
            // 描画スタイルを設定
            this.SetStyle(
                ControlStyles.DoubleBuffer |         // 描画をバッファで実行する
                ControlStyles.UserPaint |            // 描画は独自に行う
                ControlStyles.ResizeRedraw |         // リサイズ時に再描画を行う   
                ControlStyles.AllPaintingInWmPaint,  // WM_ERASEBKGND を無視する
                true                                 // 指定したスタイルを適用「する」
                );

            base.BackColor = Color.Transparent;
            this.BackColor = base.BackColor;
            this.BackColor2 = base.BackColor;
            this._GradientMode = LinearGradientMode.Horizontal;
        }

        #endregion

        #region event handler

        /// <summary>
        /// PaintBackgroundイベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータを格納している<see cref="System.Windows.Forms.PaintEventArgs"/>。</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if ((this.BackColor == Color.Transparent) || (this.BackColor2 == Color.Transparent))
                base.OnPaintBackground(e);

            if (this.ClientRectangle.Width <= 0 || this.ClientRectangle.Height <= 0)
                return;

            using (LinearGradientBrush lgb = new LinearGradientBrush(this.ClientRectangle, this.BackColor, this.BackColor2, this.GradientMode))
                e.Graphics.FillRectangle(lgb, this.ClientRectangle);
        }

        #endregion

        #region public method

        /// <summary>
        /// グラデーション開始色をクリアします。
        /// </summary>
        public override void ResetBackColor()
        {
            this.BackColor = Color.Empty;
        }

        /// <summary>
        /// グラデーション開始色が定義済みか判定します。
        /// </summary>
        /// <returns>true - 定義済み false - 未定義</returns>
        private Boolean ShouldSerializeBackColor()
        {
            return this._backColor != Color.Empty;
        }

        /// <summary>
        /// グラデーション終了色をクリアします。
        /// </summary>
        public void ResetBackColor2()
        {
            this.BackColor2 = Color.Empty;
        }

        /// <summary>
        /// グラデーション終了色が定義済みか判定します。
        /// </summary>
        /// <returns>true - 定義済み false - 未定義</returns>
        private Boolean ShouldSerializeBackColor2()
        {
            return this._backColor2 != Color.Empty;
        }

        #endregion
    }
}