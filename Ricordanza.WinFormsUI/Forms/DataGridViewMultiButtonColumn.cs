using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Ricordanza.Core.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    #region DataGridViewMultiButtonColumn

    /// <summary>
    /// DataGridView用の複数ボタン列です。
    /// </summary>
    public class DataGridViewMultiButtonColumn
        : DataGridViewColumn
    {
        #region property

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
                // DataGridViewMultiButtonCellしか
                // CellTemplateに設定できないようにする
                if (!(value is DataGridViewMultiButtonCell))
                    throw new InvalidCastException("value is not Type DataGridViewMultiButtonCell.");

                base.CellTemplate = value;
            }
        }

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public DataGridViewMultiButtonColumn()
            : base(new DataGridViewMultiButtonCell())
        {
        }

        #endregion

        #region public method

        /// <summary>
        /// このクラスのクローンを構築します。
        /// </summary>
        /// <returns>このクラスのクローン</returns>
        public override object Clone()
        {
            var col = base.Clone() as DataGridViewMultiButtonColumn;
            return col;
        }

        #endregion

        #region protected method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected internal void PerformClick(DataGridViewMultiButtonEventArgs e)
        {
            OnButtonClick(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnButtonClick(DataGridViewMultiButtonEventArgs e)
        {
            if (ButtonClick != null)
                ButtonClick(this, e);
        }

        #endregion

        #region event handler

        /// <summary>
        /// 
        /// </summary>
        public EventHandler<DataGridViewMultiButtonEventArgs> ButtonClick;

        #endregion
    }

    #endregion

    #region DataGridViewMultiButtonCell

    /// <summary>
    /// 
    /// </summary>
    public class DataGridViewMultiButtonCell :
        DataGridViewCell
    {
        #region property

        /// <summary>
        /// 編集コントロールの型を指定する
        /// </summary>
        public override Type EditType
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// セルの値のデータ型を指定する
        /// </summary>
        public override Type ValueType
        {
            get
            {
                return typeof(string);
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

        /// <summary>
        /// 
        /// </summary>
        public List<DataGridViewMultiButton> Items { protected internal set; get; }

        /// <summary>
        /// 
        /// </summary>
        protected DataGridViewMultiButtonColumn MultiButtonColumn
        {
            get
            {
                return (base.DataGridView.Columns[base.ColumnIndex] as DataGridViewMultiButtonColumn);
            }
        }


        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public DataGridViewMultiButtonCell()
            : base()
        {
            Items = new List<DataGridViewMultiButton>();
        }

        #endregion

        #region public method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="clipBounds"></param>
        /// <param name="cellBounds"></param>
        /// <param name="rowIndex"></param>
        /// <param name="elementState"></param>
        /// <param name="value"></param>
        /// <param name="formattedValue"></param>
        /// <param name="errorText"></param>
        /// <param name="cellStyle"></param>
        /// <param name="advancedBorderStyle"></param>
        /// <param name="paintParts"></param>
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            // 背景の描画
            graphics.FillRectangle(elementState.HasFlag(DataGridViewElementStates.Selected) ? new SolidBrush(cellStyle.SelectionBackColor) : new SolidBrush(cellStyle.BackColor), cellBounds);

            // ボーダーの描画
            base.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);

            Items.Clear();
            if (!StringUtility.IsEmpty(value))
            {
                int x = cellBounds.X;

                var buttons = value.ToString().Split(',');
                for (int i = 0; i < buttons.Length; i++)
                {
                    string b = buttons[i];
                    if (string.IsNullOrEmpty(b))
                        continue;

                    // テキストの幅を取得
                    int textWidth = (int)graphics.MeasureString(b, cellStyle.Font).Width + 4;

                    // アングルを取得
                    Rectangle angle = new Rectangle(x, cellBounds.Y, textWidth, cellBounds.Height);

                    // アイテムに追加
                    Items.Add(new DataGridViewMultiButton(i, b, angle));

                    // ボタンを描画
                    ButtonRenderer.DrawButton(graphics, new Rectangle(x, cellBounds.Y, textWidth, cellBounds.Height), b, cellStyle.Font, base.DataGridView.CurrentCell == this, PushButtonState.Normal);

                    // 次のボタンを表示するx座標を設定
                    x += textWidth + 2;
                }
            }
        }

        #endregion

        #region event handler

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = 0;
                for (int i = 0; i < Items.Count; i++)
                {
                    DataGridViewMultiButton button = Items[i];
                    Rectangle rect = button.Rectangle;
                    var angle = new Rectangle(x, 0, rect.Width, rect.Height);
                    if (angle.Contains(e.Location))
                    {
                        MultiButtonColumn.PerformClick(new DataGridViewMultiButtonEventArgs(this.RowIndex, this.ColumnIndex, button));
                        break;
                    }

                    // 次のボタンを表示するx座標を設定
                    x += rect.Width;
                }
            }

            base.OnMouseUp(e);
        }


        #endregion
    }

    #endregion

    #region DataGridViewMultiButton

    /// <summary>
    /// 
    /// </summary>
    public class DataGridViewMultiButton
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

        /// <summary>
        /// 
        /// </summary>
        public int ButtonIndex { protected set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { protected set; get; }

        /// <summary>
        /// 
        /// </summary>
        protected internal Rectangle Rectangle { set; get; }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonIndex"></param>
        /// <param name="value"></param>
        /// <param name="rectangle"></param>
        public DataGridViewMultiButton(int buttonIndex, string value, Rectangle rectangle)
            : base()
        {
            ButtonIndex = buttonIndex;
            Value = value;
            Rectangle = rectangle;
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

    #region DataGridViewMultiButtonEventArgs

    /// <summary>
    /// 
    /// </summary>
    public class DataGridViewMultiButtonEventArgs
        : EventArgs
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

        /// <summary>
        /// 
        /// </summary>
        public int RowIndex { protected set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int ColumnIndex { protected set; get; }

        /// <summary>
        /// 
        /// </summary>
        public DataGridViewMultiButton Button { protected set; get; }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="button"></param>
        public DataGridViewMultiButtonEventArgs(int rowIndex, int columnIndex, DataGridViewMultiButton button)
            : base()
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Button = button;
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
}
