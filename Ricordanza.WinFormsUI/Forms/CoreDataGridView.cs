using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Ricordanza.Core.Utilities;
using Ricordanza.WinFormsUI.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    /// <summary>
    /// 全てのDataGridViewの基底クラスです。
    /// </summary>
    public class CoreDataGridView : DataGridView, IEditable
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// 
        /// </summary>
        private int _ownBeginGrabRowIndex;

        /// <summary>
        /// 
        /// </summary>
        private int _dropDestinationRowIndex;

        /// <summary>
        /// 
        /// </summary>
        private bool _dropDestinationIsValid;

        /// <summary>
        /// 
        /// </summary>
        private bool _dropDestinationIsNextRow;

        /// <summary>
        /// 
        /// </summary>
        private bool _allowUserMoveRow;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// このクラスのインスタンスを構築します。
        /// </summary>
        public CoreDataGridView()
            : base()
        {
            _ownBeginGrabRowIndex = -1;
            _dropDestinationRowIndex = -1;
            _dropDestinationIsValid = false;
            _dropDestinationIsNextRow = false;
            _allowUserMoveRow = false;

            this.AllowDrop = true;
        }

        #endregion

        #region property

        /// <summary>
        /// ユーザが行の入れ替えをサポートするかを取得または設定します。
        /// </summary>
        [Category("Ricordanza")]
        [Description("ユーザが行の入れ替えをサポートするかを取得または設定します。この属性はDataSourceにDataTableがバインドされている場合のみ有効です。")]
        [DefaultValue(false)]
        public bool AllowUserMoveRow
        {
            set
            {
                _allowUserMoveRow = value;

                if (DesignMode)
                    return;

                // 設定済みのイベントを一度消す
                this.MouseDown -= _MouseDown;
                this.MouseMove -= _MouseMove;
                this.DragOver -= _DragOver;
                this.DragLeave -= _DragLeave;
                this.DragDrop -= _DragDrop;
                this.RowPostPaint -= _RowPostPaint;
                this.MouseDown -= _MouseDown;
                this.MouseDown -= _MouseDown;
                this.MouseDown -= _MouseDown;

                // 再設定
                if (_allowUserMoveRow)
                {
                    this.MouseDown += _MouseDown;
                    this.MouseMove += _MouseMove;
                    this.DragOver += _DragOver;
                    this.DragLeave += _DragLeave;
                    this.DragDrop += _DragDrop;
                    this.RowPostPaint += _RowPostPaint;
                    this.MouseDown += _MouseDown;
                    this.MouseDown += _MouseDown;
                    this.MouseDown += _MouseDown;

                    // ソートすると正しく動かせなくなるのでソート機能をここで一度無くす
                    foreach (DataGridViewColumn col in this.Columns)
                        col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            get
            {
                return _allowUserMoveRow;
            }
        }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        #endregion

        #region protected method

        #endregion

        #region private method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _MouseDown(object sender, MouseEventArgs e)
        {
            _ownBeginGrabRowIndex = -1;

            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;

            // DataTableがバインドされていない場合は操作を行わない。
            if (!(this.DataSource is DataTable))
                return;

            DataGridView.HitTestInfo hit = HitTest(e.X, e.Y);
            if (hit.Type != DataGridViewHitTestType.RowHeader) 
                return;

            // クリック時などは -1 に戻らないが問題なし
            _ownBeginGrabRowIndex = hit.RowIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left) 
                return;
            
            if (_ownBeginGrabRowIndex == -1)
                return;

            // ドラッグ＆ドロップの開始
            DoDragDrop(_ownBeginGrabRowIndex, DragDropEffects.Move);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

            int from, to;
            bool next;
 
            bool valid = DecideDropDestinationRowIndex(
                this, e, out from, out to, out next);

            // ドロップ先マーカーの表示・非表示の制御
            bool needRedraw = (valid != _dropDestinationIsValid);
            if (valid)
            {
                needRedraw = needRedraw
                    || (to != _dropDestinationRowIndex)
                    || (next != _dropDestinationIsNextRow);
            }

            if (needRedraw)
            {
                if (_dropDestinationIsValid)
                    this.InvalidateRow(_dropDestinationRowIndex);
                if (valid)
                    this.InvalidateRow(to);
            }

            _dropDestinationIsValid = valid;
            _dropDestinationRowIndex = to;
            _dropDestinationIsNextRow = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _DragLeave(object sender, EventArgs e)
        {
            if (_dropDestinationIsValid)
            {
                _dropDestinationIsValid = false;
                this.InvalidateRow(_dropDestinationRowIndex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _DragDrop(object sender, DragEventArgs e)
        {
            int from, to; bool next;
            if (!DecideDropDestinationRowIndex(
                    this, e, out from, out to, out next))
                return;

            _dropDestinationIsValid = false;

            // データの移動
            to = MoveDataValue(from, to, next);

            if ( to >= 0)
                this.CurrentCell = this[this.CurrentCell.ColumnIndex, to];

            this.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _RowPostPaint(
           object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // ドロップ先のマーカーを描画
            if (_dropDestinationIsValid
                && e.RowIndex == _dropDestinationRowIndex)
            {
                using (Pen pen = new Pen(Color.Black, 2))
                {
                    int y =
                        !_dropDestinationIsNextRow
                        ? e.RowBounds.Y + 1 : e.RowBounds.Bottom - 1;

                    e.Graphics.DrawLine(
                        pen, e.RowBounds.X, y, e.RowBounds.X + this.Width, y);
                }
            }
        }

        /// <summary>
        /// ドロップ先の行の決定
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="e"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        private bool DecideDropDestinationRowIndex(
            DataGridView grid, DragEventArgs e,
            out int from, out int to, out bool next)
        {
            from = (int)e.Data.GetData(typeof(int));
            // 元の行が追加用の行であれば、常に false
            if (grid.NewRowIndex != -1 && grid.NewRowIndex == from)
            {
                to = 0; next = false;
                return false;
            }

            Point clientPoint = grid.PointToClient(new Point(e.X, e.Y));

            // 上下のみに着目するため、横方向は無視する
            clientPoint.X = 1;
            DataGridView.HitTestInfo hit =
                grid.HitTest(clientPoint.X, clientPoint.Y);

            to = hit.RowIndex;
            if (to == -1)
            {
                int top = grid.ColumnHeadersVisible ? grid.ColumnHeadersHeight : 0;
                top += 1; // ...

                if (top > clientPoint.Y)
                    // ヘッダへのドロップ時は表示中の先頭行とする
                    to = grid.FirstDisplayedCell.RowIndex;
                else
                    // 最終行へ
                    to = grid.Rows.Count - 1;
            }

            // 追加用の行は無視
            if (to == grid.NewRowIndex) to--;

            next = (to > from);
            return (from != to);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        private int MoveDataValue(int from, int to, bool next)
        {
            var table = this.DataSource as DataTable;

            if (table == null)
                return -1;

            // 移動するデータの退避（計算列があればたぶんダメ）
            object[] rowData = table.Rows[from].ItemArray;
            DataRow row = table.NewRow();
            row.ItemArray = rowData;

            // 移動元から削除
            table.Rows.RemoveAt(from);
            if (to > from) to--;

            // 移動先へ追加
            if (next) to++;
            if (to <= table.Rows.Count)
                table.Rows.InsertAt(row, to);
            else
                table.Rows.Add(row);

            return table.Rows.IndexOf(row);
        }

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
        /// 未選択状態にします。
        /// </summary>
        public void Clear()
        {
            this.Clear();
        }

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
            if (!Enabled)
                return true;

            // 必須入力チェック
            if (Required && this.Rows.Count == 0)
            {
                RaiseError(global::Ricordanza.WinFormsUI.Properties.Resources.MSG001);
                return false;
            }

            // ユーザ定義型の入力チェックの実行
            return (CustomValidate ?? (() => { return true; }))();
        }

        #endregion
    }
}
