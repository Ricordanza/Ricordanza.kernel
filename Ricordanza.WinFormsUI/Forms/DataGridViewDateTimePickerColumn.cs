using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Ricordanza.Core.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    #region DataGridViewMaskedTextBoxColumn

    /// <summary>
    /// DataGridView用のDateTimePicker列クラスです。
    /// </summary>
    public class DataGridViewDateTimePickerColumn : DataGridViewColumn
    {
        #region property

        /// <summary>
        /// IMEの状態フラグ
        /// </summary>
        [Category("Ricordanza")]
        [DefaultValue(ImeMode.Off)]
        [Description("IMEの状態を取得または設定します。")]
        public ImeMode ImeMode { set; get; }

        /// <summary>
        /// 書式を取得または設定します。
        /// </summary>
        [Category("Ricordanza")]
        [Browsable(true)]
        [DefaultValue(DateTimePickerFormat.Short), TypeConverter(typeof(Enum))]
        public DateTimePickerFormat Format { set; get; }

        /// <summary>
        /// カスタム書式を取得または設定します。
        /// </summary>
        [Category("Ricordanza")]
        [Browsable(true)]
        [DefaultValue("")]
        public string CustomFormat { set; get; }

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
                // DataGridDateTimePickerCellしか
                // CellTemplateに設定できないようにする
                if (!(value is DataGridDateTimePickerCell))
                    throw new InvalidCastException("value is not Type DataGridDateTimePickerCell.");

                base.CellTemplate = value;
            }
        }

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public DataGridViewDateTimePickerColumn()
            : base(new DataGridDateTimePickerCell())
        {
            this.Format = DateTimePickerFormat.Short;
            this.ImeMode = ImeMode.Off;
            this.CustomFormat = string.Empty;
        }

        #endregion

        #region public method

        /// <summary>
        /// このクラスのクローンを構築します。
        /// </summary>
        /// <returns>このクラスのクローン</returns>
        public override object Clone()
        {
            var col = base.Clone() as DataGridViewDateTimePickerColumn;
            col.Format = this.Format;
            col.ImeMode = this.ImeMode;
            col.CustomFormat = this.CustomFormat;
            return col;
        }

        #endregion
    }

    #endregion

    #region DataGridDateTimePickerCell

    /// <summary>
    /// MaskedTextBoxで編集できるテキスト情報を
    /// DataGridViewコントロールに表示します。
    /// </summary>
    public class DataGridDateTimePickerCell :
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
                return typeof(DataGridViewDateTimePickerEditingControl);
            }
        }

        /// <summary>
        /// セルの値のデータ型を指定する
        /// </summary>
        public override Type ValueType
        {
            get
            {
                return typeof(object);
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
        public DataGridDateTimePickerCell()
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
            var picker = this.DataGridView.EditingControl as DataGridViewDateTimePickerEditingControl;

            if (picker == null)
                return;

            // Textを設定
            picker.Value = this.Value;

            // カスタム列のプロパティを反映させる
            var column = this.OwningColumn as DataGridViewDateTimePickerColumn;
            if (column != null)
            {
                picker.Format = column.Format;
                picker.ImeMode = column.ImeMode;
                picker.CustomFormat = column.CustomFormat;
            }
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
            if (formattedValue == null)
                return null;

            // カスタム列のプロパティを反映させる
            var column = this.OwningColumn as DataGridViewDateTimePickerColumn;
            if (column == null)
                return formattedValue;

            DateTime d;
            if (!DateTime.TryParse(formattedValue.ToString(), out d))
                return formattedValue;

            var dtf = Thread.CurrentThread.CurrentCulture.DateTimeFormat;
            string format = string.Empty;
            switch (column.Format)
            {
                case DateTimePickerFormat.Long:
                    format = dtf.LongDatePattern;
                    break;
                case DateTimePickerFormat.Short:
                    format = dtf.ShortDatePattern;
                    break;
                case DateTimePickerFormat.Time:
                    format = dtf.ShortTimePattern;
                    break;
                case DateTimePickerFormat.Custom:
                    format = column.CustomFormat;
                    break;
            }

            return d.ToString(format);
        }

        #endregion
    }

    #endregion

    #region DataGridViewDateTimePickerEditingControl

    /// <summary>
    /// DataGridViewMaskedTextBoxCellでホストされる
    /// DateTimePickerコントロールを表します。
    /// </summary>
    public class DataGridViewDateTimePickerEditingControl :
        CoreDateTimePicker, IDataGridViewEditingControl
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
                this.Value = value;
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
        public DataGridViewDateTimePickerEditingControl()
            : base()
        {
            this.TabStop = false;
        }

        #endregion

        #region event method

        /// <summary>
        /// MaskedTextBoxのTextChangedイベントです。
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
            object val = this.Value;

            try
            {
                this.Clear();
                return (val == this.Value) ? string.Empty : val;
            }
            finally
            {
                this.Value = val;
            }            
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
            //Keys.Left、Right、Home、Endの時は、Trueを返す
            //このようにしないと、これらのキーで別のセルにフォーカスが移ってしまう
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
        }

        #endregion
    }

    #endregion
}
