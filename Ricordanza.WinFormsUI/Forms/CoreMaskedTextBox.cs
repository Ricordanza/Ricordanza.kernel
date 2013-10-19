using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Ricordanza.Core;
using Ricordanza.Core.Utilities;
using Ricordanza.WinFormsUI.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    public class CoreMaskedTextBox : MaskedTextBox, IEditable
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// マスク定義時の未入力テキスト
        /// </summary>
        private string emptyText;

        #endregion

        #region property

        /// <summary>
        /// 透かし文字を取得または設定します。
        /// </summary>
        [Category("Ricordanza")]
        [Description("透かし文字を取得または設定します。")]
        [DefaultValue("")]
        public string WaterMark { set; get; }

        /// <summary>
        /// 透かし文字の色を取得または設定します。
        /// </summary>
        [Category("Ricordanza")]
        [Description("透かし文字の色を取得または設定します。")]
        [DefaultValue(typeof(Color), "Silver")]
        public Color WaterMarkColor { set; get; }

        /// <summary>
        /// 値が定義されているか判定します。
        /// </summary>
        /// <remarks>値が定義されていない場合はtrueを返却します。</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Ricordanza")]
        [Description("値が定義されているか判定します。")]
        public bool IsEmpty
        {
            get { return this.Text.IsEmpty(); }
        }

        /// <summary>
        /// 完全入力が必要かを取得または設定します。
        /// </summary>
        [Browsable(false)]
        [Category("Ricordanza")]
        [DefaultValue(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("完全入力が必要かを取得または設定します。")]
        public bool AllowFullInput { set; get; }

        /// <summary>
        /// 実行時に使用する入力マスクを取得または設定します。
        /// </summary>
        [DefaultValue("")]
        [MergableProperty(false)]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description("実行時に使用する入力マスクを取得または設定します。")]
        public new string Mask
        {
            get { return base.Mask; }
            set
            {
                base.Mask = value;
                emptyText = Text;
            }
        }

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// このクラスのインスタンスを構築します。
        /// </summary>
        public CoreMaskedTextBox()
        {
            WaterMark = string.Empty;
            WaterMarkColor = Color.Silver;
            Required = false;
            AllowFullInput = false;
            Mask = string.Empty;
            emptyText = string.Empty;
        }

        #endregion

        #region event

        #endregion

        #region event method

        #endregion

        #region public method

        #endregion

        #region protected method

        /// <summary>
        /// Windowsメッセージを処理します。
        /// </summary>
        /// <param name="m">処理対象の<see cref="System.Windows.Forms.Message"/></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WindowMessages.WM_PAINT:
                    if (!DesignMode && !ContainsFocus && !ReadOnly && Enabled && (Text == emptyText))
                        this.DrawWaterMark(Font, WaterMark, WaterMarkColor, new Point(1, 1));
                    break;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// 入力が完了していないか確認します。
        /// </summary>
        /// <param name="val">入力値</param>
        /// <param name="text">未入力時の値</param>
        protected bool InCompleteness(string val, string defVal)
        {
            // 全桁入力されているか検証
            return (!(val == defVal) && val.IndexOf(this.PromptChar) > -1);
        }

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion

        #region IEditable

        /// <summary>
        /// 必須入力項目を表す値を取得または設定します。
        /// </summary>
        [Category("Ricordanza")]
        [DefaultValue(false)]
        [Description("必須入力項目を表す値を取得または設定します。")]
        public bool Required { set; get; }

        /// <summary>
        /// カステム入力チェックを取得または設定します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Ricordanza")]
        [Description("カステム入力チェックを取得または設定します。")]
        public Func<bool> CustomValidate { set; get; }

        /// <summary>
        /// エラー通知に使用する<see cref="System.Windows.Forms.ErrorProvider"/>を設定します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Ricordanza")]
        [Description("エラー通知に使用するErrorProviderを設定します。")]
        public ErrorProvider ErrorProvider { set; private get; }

        /// <summary>
        /// エラープロバイダの通知をクリアします。
        /// </summary>
        public void ClearError()
        {
            RaiseError(null);
        }

        /// <summary>
        /// エラープロバイダで通知を行います。
        /// </summary>
        /// <param name="message">通知するメッセージ</param>
        public void RaiseError(string message)
        {
            if (ErrorProvider == null)
                return;

            ErrorProvider.SetError(this, message);
        }

        /// <summary>
        /// 入力値検証を行います。
        /// </summary>
        /// <returns>入力値が適切な場合はture。それ以外の場合はfalse</returns>
        public bool Validate()
        {
            // プロバイダを初期化
            ClearError();

            // 無効時は検証を行わない
            if (!Enabled && ReadOnly)
                return true;

            // 必須入力チェック
            if (Required && (this.Text == emptyText))
                return false;

            // 全桁入力されているか検証
            if (AllowFullInput && InCompleteness(this.Text, emptyText))
                return false;

            // 桁数チェック
            if (this.Text.Length > this.MaxLength)
                return false;

            // ユーザ定義型の入力チェックの実行
            return (CustomValidate ?? (() => { return true; }))();
        }

        #endregion
    }
}
