using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ricordanza.Merge
{
    /// <summary>
    /// マージ作業を行うダイアログです。
    /// </summary>
    public partial class MargeDialog
        : Form
    {
        #region const

        /// <summary>
        /// 編集対象 Source
        /// </summary>
        public const string SOURCE = "Source";

        /// <summary>
        /// 編集対象 Destination
        /// </summary>
        public const string DESTINATION = "Destination";

        /// <summary>
        /// 編集対象 Empty
        /// </summary>
        public const string EMPTY = "Empty";

        /// <summary>
        /// 編集対象 Delete
        /// </summary>
        public const string DELETE = "Delete";

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        /// <param name="report">比較結果</param>
        public MargeDialog(DiffReport report)
            : base()
        {
            InitializeComponent();


            if (DesignMode)
                return;

            // コンボボックスのアイテムを積む
            actionColumn.Items.Clear();
            actionColumn.Items.AddRange(SOURCE, DESTINATION, EMPTY, DELETE);

            DiffReport = report;
        }

        #endregion

        #region property

        /// <summary>
        /// 比較結果
        /// </summary>
        protected DiffReport DiffReport { set; get; } 

        #endregion

        #region event

        #endregion

        #region event handler

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 確認タブに変更された場合
            if (tabControl.SelectedTab == tabPage2)
            {
                textBox.Text = GetMargedString();
            }
            else
                textBox.Clear();
        }

        #endregion

        #region public method

        /// <summary>
        /// 編集結果を取得します。
        /// </summary>
        /// <returns></returns>
        public string GetMargedString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                switch (row.Cells[actionColumn.Index].Value.ToString())
                {
                    case SOURCE:
                        sb.AppendLine(row.Cells[srcColumn.Index].Value.ToString());
                        break;
                    case DESTINATION:
                        sb.AppendLine(row.Cells[destColumn.Index].Value.ToString());
                        break;
                    case EMPTY:
                        sb.AppendLine(string.Empty);
                        break;
                    case DELETE:
                        break;
                }
            }

            // 終端の改行コードはロジック上で付加した物なので削除する
            string ret = sb.ToString();
            if (ret.EndsWith(Environment.NewLine))
                ret = ret.Remove(ret.LastIndexOf(Environment.NewLine), Environment.NewLine.Length);

            return sb.ToString();
        }

        #endregion

        #region protected method

        /// <summary>
        /// Loadイベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータを格納している<see cref="System.EventArgs"/>。</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DiffReport == null)
                return;

            dataGridView.SuspendLayout();
            dataGridView.Rows.Clear();
            int cnt = 1;
            int i;

            // 比較結果をDataGridViewに反映
            DiffReport.ForEach(s =>
            {
                switch (s.Status)
                {
                    case DiffResultSpanStatus.DeleteSource:
                        for (i = 0; i < s.Length; i++)
                        {
                            string no = cnt.ToString("00000");
                            string src = ((TextLine)DiffReport.Source.GetByIndex(s.SourceIndex + i)).Line;
                            string dest = string.Empty;
                            dataGridView.Rows.Add(no, src, dest, DESTINATION);

                            dataGridView[srcColumn.DisplayIndex, dataGridView.Rows.Count - 1].Style.BackColor = Color.Red;
                            dataGridView[destColumn.DisplayIndex, dataGridView.Rows.Count - 1].Style.BackColor = Color.LightGray;
                            cnt++;
                        }

                        break;
                    case DiffResultSpanStatus.NoChange:
                        for (i = 0; i < s.Length; i++)
                        {
                            string no = cnt.ToString("00000");
                            string src = ((TextLine)DiffReport.Source.GetByIndex(s.SourceIndex + i)).Line;
                            string dest = ((TextLine)DiffReport.Destination.GetByIndex(s.DestIndex + i)).Line;
                            dataGridView.Rows.Add(no, src, dest, DESTINATION);

                            dataGridView[srcColumn.DisplayIndex, dataGridView.Rows.Count - 1].Style.BackColor = Color.White;
                            dataGridView[destColumn.DisplayIndex, dataGridView.Rows.Count - 1].Style.BackColor = Color.White;
                            cnt++;
                        }

                        break;
                    case DiffResultSpanStatus.AddDestination:
                        for (i = 0; i < s.Length; i++)
                        {
                            string no = cnt.ToString("00000");
                            string src = string.Empty;
                            string dest = ((TextLine)DiffReport.Destination.GetByIndex(s.DestIndex + i)).Line;
                            dataGridView.Rows.Add(no, src, dest, DESTINATION);

                            dataGridView[srcColumn.DisplayIndex, dataGridView.Rows.Count - 1].Style.BackColor = Color.LightGray;
                            dataGridView[destColumn.DisplayIndex, dataGridView.Rows.Count - 1].Style.BackColor = Color.LightGreen;
                            cnt++;
                        }

                        break;
                    case DiffResultSpanStatus.Replace:
                        for (i = 0; i < s.Length; i++)
                        {
                            string no = cnt.ToString("00000");
                            string src = ((TextLine)DiffReport.Source.GetByIndex(s.SourceIndex + i)).Line;
                            string dest = ((TextLine)DiffReport.Destination.GetByIndex(s.DestIndex + i)).Line;
                            dataGridView.Rows.Add(no, src, dest, DESTINATION);

                            dataGridView[srcColumn.DisplayIndex, dataGridView.Rows.Count - 1].Style.BackColor = Color.Red;
                            dataGridView[destColumn.DisplayIndex, dataGridView.Rows.Count - 1].Style.BackColor = Color.LightGreen;
                            cnt++;
                        }

                        break;
                } 
            });

            dataGridView.ResumeLayout();
        }

        /// <summary>
        /// Windows メッセージを処理します。
        /// </summary>
        /// <param name="m">処理対象の<see cref="System.Windows.Forms.Message"/></param>
        [System.Security.Permissions.SecurityPermission(
        System.Security.Permissions.SecurityAction.LinkDemand,
        Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            // Alt + F4 等によるウィンドウを閉じる動作を抑制
            if (m.Msg == 0x112 && m.WParam.ToInt32() == 0xF060)
                return;

            base.WndProc(ref m);
        }

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }
}
