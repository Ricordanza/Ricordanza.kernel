using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Ricordanza.Core.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    #region DataGridViewMaskedTextBoxColumn

    /// <summary>
    /// DataGridView用のRichTextBox列クラスです。
    /// </summary>
    public class DataGridViewRichTextBoxColumn : DataGridViewColumn
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
        /// これが複数行の<see cref="Ricordanza.WinFormsUI.Forms.CoreRichTextBox"/>コントロールかどうかを示す値を取得または設定します。
        /// </summary>
        [Category("Ricordanza")]
        [DefaultValue(false)]
        [Description("複数行のコントロールかどうかを示す値を取得または設定します。")]
        public bool Multiline { set; get; }

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
                // DataGridViewMaskedTextBoxCellしか
                // CellTemplateに設定できないようにする
                if (!(value is DataGridViewRichTextBoxCell))
                    throw new InvalidCastException("value is not Type DataGridViewRichTextBoxCell.");

                base.CellTemplate = value;
            }
        }

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public DataGridViewRichTextBoxColumn()
            : base(new DataGridViewRichTextBoxCell())
        {
            this.ImeMode = ImeMode.Off;
            this.Multiline = false;
        }

        #endregion

        #region public method

        /// <summary>
        /// このクラスのクローンを構築します。
        /// </summary>
        /// <returns>このクラスのクローン</returns>
        public override object Clone()
        {
            var col = base.Clone() as DataGridViewRichTextBoxColumn;
            col.ImeMode = this.ImeMode;
            col.Multiline = this.Multiline;
            return col;
        }

        #endregion
    }

    #endregion

    #region DataGridViewRichTextBoxCell

    /// <summary>
    /// RichTextBoxで編集できるテキスト情報を
    /// DataGridViewコントロールに表示します。
    /// </summary>
    public class DataGridViewRichTextBoxCell :
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
                return typeof(DataGridViewRichTextBoxEditingControl);
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
        public DataGridViewRichTextBoxCell()
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
            var rich = this.DataGridView.EditingControl as DataGridViewRichTextBoxEditingControl;

            if (rich == null)
                return;

            // Textを設定
            rich.Text = this.Value != null ? this.Value.ToString() : string.Empty;

            // カスタム列のプロパティを反映させる
            var column = this.OwningColumn as DataGridViewRichTextBoxColumn;
            if (column != null)
            {
                rich.ImeMode = column.ImeMode;
                rich.Multiline = column.Multiline;
            }
        }

        #endregion
    }

    #endregion

    #region DataGridViewMaskedTextBoxEditingControl

    /// <summary>
    /// DataGridViewMaskedTextBoxCellでホストされる
    /// RichTextBox,コントロールを表します。
    /// </summary>
    public class DataGridViewRichTextBoxEditingControl :
        CoreRichTextBox, IDataGridViewEditingControl
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
        public DataGridViewRichTextBoxEditingControl ()
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
            string val = this.Text;

            try
            {
                this.Clear();
                return (val == this.Text) ? string.Empty : val;
            }
            finally
            {
                this.Text = val;
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
            //Keys.Left、Right、Home、End、Enterの時は、Trueを返す
            //このようにしないと、これらのキーで別のセルにフォーカスが移ってしまう
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Return:
                    if ((((keyData & (Keys.Alt | Keys.Control | Keys.Shift)) == Keys.Shift) && this.Multiline))
                        return true;
                    break;
                case Keys.Right:
                case Keys.End:
                case Keys.Left:
                case Keys.Home:
                    return true;
            }

            return false;
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
                //挿入ポインタを末尾にする
                this.SelectionStart = this.TextLength;
        }

        #endregion

        #region protected method

        /// <summary>
        /// 指定されているキーが、入力キーであるか、またはプリプロセスを必要とする特殊なキーであるかを確認します。
        /// </summary>
        /// <param name="keyData">キーの値の<c>1</c>つ。</param>
        /// <returns>指定されているキーが入力キーである場合は<c>true</c>。それ以外の場合は<c>false</c>。</returns>
        protected override bool IsInputKey(Keys keyData)
        {
            Keys keys = keyData & Keys.KeyCode;

            if (keys == Keys.Return)
                return this.Multiline;

            return base.IsInputKey(keyData);
        } 

        #endregion
    }

    #endregion
}
