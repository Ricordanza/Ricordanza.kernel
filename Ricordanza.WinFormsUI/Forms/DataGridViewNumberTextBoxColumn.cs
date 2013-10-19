using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Ricordanza.Core.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    #region DataGridViewNumberTextBoxColumn

    /// <summary>
    /// DataGridView用のNumberTextBox列クラスです。
    /// </summary>
    public class DataGridViewNumberTextBoxColumn : DataGridViewColumn
    {
        #region private variable

        /// <summary>
        /// 浮動小数の桁数
        /// </summary>
        private int _decimalLendth = 0;

        #endregion

        #region property

        /// <summary>
        /// 浮動小数の桁数
        /// </summary>
        [Category("Ricordanza")]
        [DefaultValue(0)]
        [Description("小数点以下の桁数を取得または設定します。")]
        public int DecimalLength
        {
            get { return _decimalLendth; }
            set
            {
                _decimalLendth = value;
                this.DefaultCellStyle.Format = (value > 0) ? "#,##0.".PadRight("#,##0.".Length + DecimalLength, '0') : "#,##0";
            }
        }

        /// <summary>
        /// 最小値
        /// </summary>
        [Category("Ricordanza")]
        [DefaultValue(-2147483648)]
        [Description("最小値を取得または設定します。")]
        public decimal MinValue { set; get; }

        /// <summary>
        /// 最大値
        /// </summary>
        [Category("Ricordanza")]
        [DefaultValue(2147483647)]
        [Description("最大値を取得または設定します。")]
        public decimal MaxValue { set; get; }

        /// <summary>
        /// 最大桁数
        /// </summary>
        [Category("Ricordanza")]
        [DefaultValue(20)]
        [Description("最大桁数を取得または設定します。")]
        public int MaxLength { set; get; }

        /// <summary>
        /// カンマ編集フラグ
        /// </summary>
        [Category("Ricordanza")]
        [DefaultValue(true)]
        [Description("カンマ編集の可否を表す値を取得または設定します。")]
        public bool CommaFormat { set; get; }

        /// <summary>
        /// IMEの状態フラグ
        /// </summary>
        [Category("Ricordanza")]
        [DefaultValue(ImeMode.Off)]
        [Description("IMEの状態を取得または設定します。")]
        public ImeMode ImeMode { set; get; }

        /// <summary>
        /// CellTemplate
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // DataGridViewNumberTextBoxCellしか
                // CellTemplateに設定できないようにする
                if (!(value is DataGridViewNumberTextBoxCell))
                    throw new InvalidCastException("value is not Type DataGridViewNumberTextBoxCell.");

                base.CellTemplate = value;
            }
        }

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public DataGridViewNumberTextBoxColumn()
            : base(new DataGridViewNumberTextBoxCell())
        {
            this.DecimalLength = 0;
            this.MinValue = -2147483648;
            this.MaxValue = 2147483647;
            this.MaxLength = 20;
            this.CommaFormat = true;
            this.ImeMode = ImeMode.Off;
        }

        #endregion

        #region public method

        /// <summary>
        /// このクラスのクローンを構築します。
        /// </summary>
        /// <returns>このクラスのクローン</returns>
        public override object Clone()
        {
            var col = base.Clone() as DataGridViewNumberTextBoxColumn;
            col.DecimalLength = this.DecimalLength;
            col.MinValue = this.MinValue;
            col.MaxValue = this.MaxValue;
            col.MaxLength = this.MaxLength;
            col.CommaFormat = this.CommaFormat;
            col.ImeMode = this.ImeMode;
            return col;
        }

        #endregion
    }

    #endregion

    #region DataGridViewNumberTextBoxCell

    /// <summary>
    /// NumbertBoxで編集できるテキスト情報を
    /// DataGridViewコントロールに表示します。
    /// </summary>
    public class DataGridViewNumberTextBoxCell :
        DataGridViewTextBoxCell
    {
        #region property

        /// <summary>
        /// 編集コントロールの型を指定する
        /// </summary>
        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewNumberTextBoxEditingControl);
            }
        }

        /// <summary>
        /// セルの値のデータ型を指定する
        /// </summary>
        public override Type ValueType
        {
            get
            {
                return typeof(decimal);
            }
        }

        /// <summary>
        /// 新しいレコード行のセルの既定値を指定する
        /// </summary>
        public override object DefaultNewRowValue
        {
            get
            {
                return base.DefaultNewRowValue;
            }
        }

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public DataGridViewNumberTextBoxCell()
            : base()
        {
        }

        #endregion

        #region public method

        /// <summary>
        /// 編集コントロールを初期化します。
        /// 
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="initialFormattedValue">初期化する値</param>
        /// <param name="dataGridViewCellStyle">セルのスタイル</param>
        /// <remarks>編集コントロールは別のセルや列でも使いまわされるため初期化の必要があります。</remarks>
        public override void InitializeEditingControl(
            int rowIndex, object initialFormattedValue,
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex,
                initialFormattedValue, dataGridViewCellStyle);

            // 編集コントロールの取得
            var numberText = this.DataGridView.EditingControl as DataGridViewNumberTextBoxEditingControl;

            if (numberText == null)
                return;

            // カスタム列のプロパティを反映させる
            var column = this.OwningColumn as DataGridViewNumberTextBoxColumn;
            if (column != null)
            {
                numberText.DecimalLength = column.DecimalLength;
                numberText.MaxValue = column.MaxValue;
                numberText.MinValue = column.MinValue;
                numberText.MaxLength = column.MaxLength;
                numberText.CommaFormat = column.CommaFormat;
                numberText.ImeMode = column.ImeMode;
            }

            // Textを設定
            if (this.Value == null || this.Value.ToString().IsEmpty())
                numberText.Clear();
            else
                numberText.Text = this.Value.ToString();
        }

        /// <summary>
        /// 表示用に書式設定された値を、実際のセル値に変換します。
        /// </summary>
        /// <param name="formattedValue">セルの表示値。</param>
        /// <param name="cellStyle">セルに反映される<c>DataGridViewCellStyle</c></param>
        /// <param name="formattedValueTypeConverter">表示値の型の<c>TypeConverter</c>。既定のコンバータを使用する場合は<c>null</c></param>
        /// <param name="valueTypeConverter">セル値の型の<c>TypeConverter</c>。既定のコンバータを使用する場合は<c>null</c></param>
        /// <returns>セル値。</returns>
        public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
        {
            return FormatComma(formattedValue);
        }

        /// カンマ編集を行います。
        /// </summary>
        /// <param name="value">カンマ編集したい値</param>
        /// <returns>カンマ編集後の値</returns>
        protected object FormatComma(object value)
        {
            // カスタム列のプロパティを反映させる
            var column = this.OwningColumn as DataGridViewNumberTextBoxColumn;
            if (column == null)
                return value;

            string target = ObjectUtility.EmptyToStr(value, string.Empty);

            if (target.IsEmpty())
                return string.Empty;

            if (column.CommaFormat)
                return target.ToDecimal().ToString((column.DecimalLength > 0) ? "#,##0.".PadRight("#,##0.".Length + column.DecimalLength, '0') : "#,##0");
            else
                return target.ToDecimal().ToString((column.DecimalLength > 0) ? "###0.".PadRight("###0.".Length + column.DecimalLength, '0') : "###0");
        }

        #endregion
    }

    #endregion

    #region DataGridViewNumberTextBoxEditingControl

    /// <summary>
    /// DataGridViewNumberTextBoxCellでホストされる
    /// NumberTextBoxコントロールを表します。
    /// </summary>
    public class DataGridViewNumberTextBoxEditingControl :
        NumberTextBox, IDataGridViewEditingControl
    {
        #region property

        /// <summary>
        /// 編集するセルがあるDataGridViewを設定または取得します。
        /// </summary>
        public DataGridView EditingControlDataGridView { set; get; }

        /// <summary>
        /// 編集している行のインデックスを設定または取得します。
        /// </summary>
        public int EditingControlRowIndex { set; get; }

        /// <summary>
        /// 値が変更されたかどうかを設定または取得します。
        /// </summary>
        public bool EditingControlValueChanged { set; get; }

        /// <summary>
        /// 値を取得または設定します。
        /// </summary>
        public new string Text
        {
            get { return base.Text; }
            set
            {
                if (value.IsNumeric())
                    base.Value = value.ToDecimal();
                else
                    base.Text = value;
            }
        }

        /// <summary>
        /// 編集コントロールで変更されたセルの値
        /// </summary>
        public object EditingControlFormattedValue
        {
            get
            {
                return this.GetEditingControlFormattedValue(
                    DataGridViewDataErrorContexts.Formatting);
            }
            set
            {
                this.Text = (string)value;
            }
        }

        /// <summary>
        /// マウスカーソルがEditingPanel上にあるときのカーソルを指定する
        /// EditingPanelは編集コントロールをホストするパネルで、
        /// 編集コントロールがセルより小さいとコントロール以外の部分がパネルとなる
        /// </summary>
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        /// <summary>
        /// 値が変更した時に、セルの位置を変更するかどうか
        /// 値が変更された時に編集コントロールの大きさが変更される時はTrue
        /// </summary>
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public DataGridViewNumberTextBoxEditingControl()
            : base()
        {
            this.TabStop = false;
        }

        #endregion

        #region event method

        /// <summary>
        /// TextBoxのTextChangedイベントです。
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            // 値が変更されたことをDataGridViewに通知する
            this.EditingControlValueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
        }

        #endregion

        #region public method

        /// <summary>
        /// 編集コントロールで変更されたセルの値
        /// </summary>
        /// <param name="context"><c>DataGridViewDataErrorContexts</c></param>
        /// <returns>編集コントロールで変更されたセルの値</returns>
        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            if (this.Text.IsEmpty())
                return string.Empty;

            if (this.Text.IsNumeric())
            {
                if (this.MinValue > this.Text.ToDecimal() || this.Text.ToDecimal() > this.MaxValue)
                    return string.Empty;
                else
                    return this.Value.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// セルスタイルを編集コントロールに適用する
        /// 編集コントロールの前景色、背景色、フォントなどをセルスタイルに合わせる
        /// </summary>
        /// <param name="dataGridViewCellStyle">セルのスタイル</param>
        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
            switch (dataGridViewCellStyle.Alignment)
            {
                case DataGridViewContentAlignment.BottomCenter:
                case DataGridViewContentAlignment.MiddleCenter:
                case DataGridViewContentAlignment.TopCenter:
                    this.TextAlign = HorizontalAlignment.Center;
                    break;
                case DataGridViewContentAlignment.BottomRight:
                case DataGridViewContentAlignment.MiddleRight:
                case DataGridViewContentAlignment.TopRight:
                    this.TextAlign = HorizontalAlignment.Right;
                    break;
                default:
                    this.TextAlign = HorizontalAlignment.Left;
                    break;
            }
        }

        /// <summary>
        /// 指定されたキーをDataGridViewが処理するか、編集コントロールが処理するか
        /// Trueを返すと、編集コントロールが処理する
        /// dataGridViewWantsInputKeyがTrueの時は、DataGridViewが処理できる
        /// </summary>
        /// <param name="keyData">キー情報</param>
        /// <param name="dataGridViewWantsInputKey">DataGridViewに処理させるかどうか</param>
        /// <returns>true 未ハンドル false ハンドル</returns>
        public bool EditingControlWantsInputKey(
            Keys keyData, bool dataGridViewWantsInputKey)
        {
            // Keys.Left、Right、Home、Endの時は、Trueを返す
            // このようにしないと、これらのキーで別のセルにフォーカスが移ってしまう
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Right:
                case Keys.End:
                case Keys.Left:
                case Keys.Home:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// コントロールで編集する準備をする
        /// テキストを選択状態にしたり、挿入ポインタを末尾にしたりする
        /// </summary>
        /// <param name="selectAll">true 全選択 false 挿入</param>
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            if (selectAll)
                // 選択状態にする
                this.SelectAll();
            else
                // 挿入ポインタを末尾にする
                this.SelectionStart = this.TextLength;
        }

        #endregion
    }

    #endregion
}
