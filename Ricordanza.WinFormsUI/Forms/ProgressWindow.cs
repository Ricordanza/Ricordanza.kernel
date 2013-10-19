using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Ricordanza.Core;
using Ricordanza.Core.Utilities;
using Ricordanza.WinFormsUI.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    /// <summary>
    /// プログレスウィンドウです。
    /// </summary>
    public partial class ProgressWindow : CoreForm
    {
        #region constant

        #endregion

        #region private variable

        #endregion

        #region protected variable

        #endregion

        #region property

        /// <summary>
        /// メッセージを設定します。
        /// </summary>
        public string Message
        {
            set 
            {
                if (InvokeRequired)
                    this.Invoke(new MethodInvoker(() => { msg.Text = value.EmptyToStr(string.Empty); }));
                else
                    msg.Text = value.EmptyToStr(string.Empty);
            }
        }

        #endregion

        #region static constractor

        #endregion

        #region constractor

        /// <summary>
        /// 新しいインスタンスを構築します。
        /// </summary>
        public ProgressWindow()
        {
            InitializeComponent();

            // 透過フォームとして扱う
            this.TransparencyKey = Color.White;

            // 描画スタイルを設定
            this.SetStyle(
                ControlStyles.DoubleBuffer |         // 描画をバッファで実行する
                ControlStyles.UserPaint |            // 描画は独自に行う
                ControlStyles.ResizeRedraw |         // リサイズ時に再描画を行う   
                ControlStyles.AllPaintingInWmPaint,  // WM_ERASEBKGND を無視する
                true                                 // 指定したスタイルを適用「する」
                );
        }

        #endregion

        #region event method

        /// <summary>
        /// Shownイベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータを格納している<see cref="System.EventArgs"/>。</param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // プログレスの起動開始
            circularProgress.Start();
        }

        #endregion

        #region public method

        /// <summary>
        /// プログレスウィンドウを表示してactionを実行します。
        /// </summary>
        /// <param name="action">実行したい処理</param>
        public static void Invoke(Action<ProgressWindow> action)
        {
            Invoke(null, action);
        }

        /// <summary>
        /// プログレスウィンドウを表示してactionを実行します。
        /// </summary>
        /// <param name="owner">プログレスウィンドウのオーナー</param>
        /// <param name="action">実行したい処理</param>
        public static void Invoke(IWin32Window owner, Action<ProgressWindow> action)
        {
            using (ProgressWindow pw = new ProgressWindow())
            {
                // 非同期で処理を実行
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        // 指定された処理の実行
                        action.Invoke(pw);
                    }
                    finally
                    {
                        // プログレスウィンドウを閉じる
                        pw.SafetyClose();
                    }
                });

                // オーナーを指定
                if (owner != null)
                    pw.Owner = owner as Form;

                // プログレスウィンドウを表示
                pw.ShowDialog(owner);
            }
        }

        /// <summary>
        /// 安全にこのクラスを閉じます。
        /// </summary>
        public void SafetyClose()
        {
            // 終了アクション
            MethodInvoker action = new MethodInvoker(() => this.Close());

            // Invokeが必要か検証
            if (InvokeRequired)
                this.Invoke(action);
            else
            {
                // ハンドルが作成されるまで待つ。
                while (!IsHandleCreated)
                    Thread.Sleep(200);

                // ハンドルがどのスレッドから作成されたかわからない為、Invokeする。
                this.InvokeAction(action);
            }
        }

        #endregion

        #region protected method

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
            if (m.Msg == WindowMessages.WM_SYSCOMMAND &&
                m.WParam.ToInt32() == WindowMessages.SC_CLOSE)
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
