using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ricordanza.Core.Utilities;
using Ricordanza.WinFormsUI.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    /// <summary>
    /// 数値型テキストボックスクラスです。
    /// </summary>
    public class NumberTextBox : CoreTextBox
    {
        #region property

        /// <summary>
        /// 値を取得または設定します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Ricordanza")]
        [Description("値を取得または設定します。")]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = FormatComma(value); }
        }

        /// <summary>
        /// 浮動小数の桁数
        /// </summary>
        [Category("Ricordanza")]
        [DefaultValue(0)]
        [Description("小数点以下の桁数を取得または設定します。")]
        public int DecimalLength { set; get; }

        /// <summary>
        /// 最小値
        /// </summary>
        [Category("Ricordanza")]
        [Description("最小値を取得または設定します。")]
        public decimal MinValue { set; get; }

        /// <summary>
        /// 最大値
        /// </summary>
        [Category("Ricordanza")]
        [Description("最大値を取得または設定します。")]
        public decimal MaxValue { set; get; }

        /// <summary>
        /// カンマ編集フラグ
        /// </summary>
        [Category("Ricordanza")]
        [DefaultValue(true)]
        [Description("カンマ編集の可否を表す値を取得または設定します。")]
        public bool CommaFormat { set; get; }

        /// <summary>
        /// 数値データを取得または設定します。
        /// </summary>
        /// <remarks>入力値が数値として不適切な場合は<c>0</c>を取得します。</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Ricordanza")]
        [Description("数値データを取得または設定します。")]
        public decimal Value
        {
            get { return this.Text.ToDecimal(); }
            set { this.Text = value.ToString(); }
        }

        #endregion

        #region constructor

        /// <summary>
        /// コンストラクター
        /// </summary>
        public NumberTextBox()
            : base()
        {
            DecimalLength = 0;
            MaxLength = 20;
            MaxValue = decimal.MaxValue;
            MinValue = decimal.MinValue;
            CommaFormat = true;
            TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        }

        #endregion

        #region event method

        /// <summary>
        /// Enterイベントを発生させます。
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnEnter(EventArgs e)
        {
            base.Text = base.Text.Replace(",", string.Empty);
        }

        /// <summary>
        /// Leaveイベントを発生させます。
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnLeave(EventArgs e)
        {
            if (!IsEmpty)
                base.Text = FormatComma(base.Text);
        }

        #endregion

        #region public method

        #endregion

        #region protected method

        /// <summary>
        /// 文字がコントロールによって認識される入力文字かどうかを判断します。
        /// </summary>
        /// <param name="charCode">テスト対象の文字</param>
        /// <returns>字をコントロールに直接送信する必要があり、プリプロセスしない場合は true。それ以外の場合は、false</returns>
        protected override bool IsInputChar(char charCode)
        {
            if (!Enabled && ReadOnly)
                return true;

            if (!base.IsInputChar(charCode))
                return false;

            // バックスペースキー
            if (charCode == '\b')
                return true;

            // 浮動少数ありの場合
            if (DecimalLength > 0 && charCode == '.')
                return true;

            // マイナスを許容している場合
            if (MinValue < 0 && charCode == '-')
                return true;

            return char.IsNumber(charCode);
        }

        /// <summary>
        /// ダイアログ文字を処理します。
        /// </summary>
        /// <param name="charCode">処理対象の文字</param>
        /// <returns>文字がコントロールによって処理された場合は true。それ以外の場合は false。</returns>
        protected override bool ProcessDialogChar(char charCode)
        {
            base.ProcessDialogChar(charCode);
            return !char.IsNumber(charCode);
        }

        /// <summary>
        /// カンマ編集を行います。
        /// </summary>
        /// <param name="value">カンマ編集したい値</param>
        /// <returns>カンマ編集後の値</returns>
        protected string FormatComma(string value)
        {
            string target = value ?? string.Empty;

            if (CommaFormat)
                return target.ToDecimal().ToString((DecimalLength > 0) ? "#,##0.".PadRight("#,##0.".Length + DecimalLength, '0') : "#,##0");
            else
                return target.ToDecimal().ToString((DecimalLength > 0) ? "###0.".PadRight("###0.".Length + DecimalLength, '0') : "###0");
        }

        #endregion

        #region IEditable

        /// <summary>
        /// 入力値検証を行います。
        /// </summary>
        /// <returns>入力値が適切な場合はture、それ以外の場合はfalse。</returns>
        public override bool Validate()
        {
            // プロバイダを初期化
            ClearError();

            // 無効時は検証を行わない
            if (!Enabled && ReadOnly)
                return true;

            // 必須入力チェック
            if (Required && IsEmpty)
            {
                RaiseError(global::Ricordanza.WinFormsUI.Properties.Resources.MSG001);
                return false;
            }

            if (!IsEmpty)
            {
                // 数値入力チェック
                if (!Text.IsNumeric())
                {
                    RaiseError(global::Ricordanza.WinFormsUI.Properties.Resources.MSG005);
                    return false;
                }

                // 最大値、最小値チェック
                if (MinValue > Text.ToDecimal() || Text.ToDecimal() > MaxValue)
                {
                    RaiseError(
                        global::Ricordanza.WinFormsUI.Properties.Resources.MSG004.DirectFormat(this.MaxValue, this.MinValue)
                        );
                    return false;
                }
            }

            // ユーザ定義型の入力チェックの実行
            return (CustomValidate ?? (() => { return true; }))();
        }

        #endregion
    }
}
