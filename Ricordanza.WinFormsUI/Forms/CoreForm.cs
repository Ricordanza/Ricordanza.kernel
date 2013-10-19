using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Ricordanza.Core;
using Ricordanza.WinFormsUI.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    #region CoreForm

    /// <summary>
    /// 全てのFormの規定クラスです。
    /// </summary>
    public partial class CoreForm : Form
    {
        #region const

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// 新しいインスタンスを構築します。
        /// </summary>
        public CoreForm()
            : base()
        {
            // 初期化
            KeyMap = new Dictionary<Keys, IBindKey>();
            IEditables = new List<IEditable>();
            ErrorProvider = new ErrorProvider()
            {
                BlinkRate = 500,
                BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink,
                ContainerControl = this,
                Icon = global::Ricordanza.WinFormsUI.Properties.Resources.Error
            };
        }

        #endregion

        #region property

        /// <summary>
        /// キーと<see cref="Ricordanza.WinFormsUI.Forms.IBindKey"/>を関連付ける連想配列を取得または設定します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Ricordanza")]
        [Description("キーとIBindKeyを関連付ける連想配列を取得または設定します。")]
        protected internal Dictionary<Keys, IBindKey> KeyMap { private set; get; }

        /// <summary>
        ///  <see cref="Ricordanza.WinFormsUI.Forms.IEditable"/>コントロール配列を取得または設定します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Ricordanza")]
        [Description("IValidatableコントロール配列を取得または設定します。")]
        protected internal List<IEditable> IEditables { private set; get; }

        /// <summary>
        /// 入力チェック不正時のプロバイダを取得または設定します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Ricordanza")]
        [Description("入力チェック不正時のプロバイダを取得または設定します。")]
        protected internal ErrorProvider ErrorProvider { private set; get; }

        #endregion

        #region event

        /// <summary>
        /// ウィンドウサイズ変更イベント。
        /// </summary>
        [Category("Ricordanza")]
        [Description("ウィンドウサイズの値がコントロールで変更される直前に発生します。")]
        public event EventHandler<WindowStateChangingEventArgs> WindowStateChanging;

        /// <summary>
        /// ウィンドウサイズ変更後イベント。
        /// </summary>
        [Category("Ricordanza")]
        [Description("ウィンドウサイズの値がコントロールで変更される直後に発生します。")]
        public event EventHandler<WindowStateChangedEventArgs> WindowStateChanged;

        #endregion

        #region event method

        /// <summary>
        /// Shownイベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータを格納している<see cref="System.EventArgs"/>。</param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // Loadでは起動が遅くなる(Formの表示が遅くなる為)Form表示後に初期化を行う。
            InitForm();
        }

        /// <summary>
        /// KeyDownイベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータを格納している<see cref="System.Windows.Forms.KeyEventArgs"/>。</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // 押下されたキーの構築
            Keys key = e.Modifiers | e.KeyCode;

            // 押下されたキーに対応するIBindKeyが存在する場合は処理実行
            if (KeyMap.ContainsKey(key))
            {
                IBindKey ib = KeyMap[key] as IBindKey;

                // IBindKeyが有効な場合は実行
                if (ib.Effective)
                {
                    ib.KeyHook();
                    e.Handled = true;
                }
            }

            // 未ハンドルの場合は上位にイベントを通知
            if (!e.Handled)
                base.OnKeyDown(e);
        }

        /// <summary>
        /// WindowStateChangingイベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータを格納している<see cref="Ricordanza.WinFormsUI.Forms.WindowStateChangingEventArgs"/>。</param>
        /// <returns>ウィンドウの変更を許容する場合はtrue。それ以外の場合はfalse。</returns>
        protected virtual bool OnWindowStateChanging(WindowStateChangingEventArgs e)
        {
            if (WindowStateChanging != null)
                WindowStateChanging(this, e);

            return e.Cancel;
        }

        /// <summary>
        /// WindowStateChangedイベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータを格納している<see cref="Ricordanza.WinFormsUI.Forms.WindowStateChangedEventArgs"/>。</param>
        protected virtual void OnWindowStateChanged(WindowStateChangedEventArgs e)
        {
            if (WindowStateChanged != null)
                WindowStateChanged(this, e);
        }

        #endregion

        #region public method

        #endregion

        #region protected method

        /// <summary>
        /// Windowsメッセージを処理します。
        /// </summary>
        /// <param name="m">処理対象の<see cref="System.Windows.Forms.Message"/></param>
        [System.Security.Permissions.SecurityPermission(
        System.Security.Permissions.SecurityAction.LinkDemand,
        Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WindowMessages.WM_SYSCOMMAND)
            {
                int wparam = m.WParam.ToInt32() & 0xfff0;
                switch (wparam)
                {
                    case WindowMessages.SC_MINIMIZE:
                        if (this.OnWindowStateChanging(new WindowStateChangingEventArgs(FormWindowState.Minimized)))
                            return;
                        base.WndProc(ref m);
                        this.OnWindowStateChanged(new WindowStateChangedEventArgs(FormWindowState.Minimized));
                        return;

                    case WindowMessages.SC_MAXIMIZE:
                        if (this.OnWindowStateChanging(new WindowStateChangingEventArgs(FormWindowState.Maximized)))
                            return;
                        base.WndProc(ref m);
                        this.OnWindowStateChanged(new WindowStateChangedEventArgs(FormWindowState.Maximized));
                        return;

                    case WindowMessages.SC_RESTORE:
                        if (this.OnWindowStateChanging(new WindowStateChangingEventArgs(FormWindowState.Normal)))
                            return;
                        base.WndProc(ref m);
                        this.OnWindowStateChanged(new WindowStateChangedEventArgs(FormWindowState.Normal));
                        return;
                }
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// フォームの共通初期化処理を行います。
        /// </summary>
        protected virtual void InitForm()
        {
            // キーフック用のマップ作成
            KeyMap.Clear();
            FindIBindKey(this).ToList().ForEach(ib =>
            {
                // キー割当なしの場合
                if (ib.BindKey == Keys.None)
                    return;

                // キーの割り当ての優先順位は出現順とする
                if (!KeyMap.ContainsKey(ib.BindKey))
                    KeyMap[ib.BindKey] = ib;
            });

            // キーフックの定義が素材する場合はキーをフックできるようにする。
            if ( KeyMap.Count > 0)
                this.KeyPreview = true;

            // 入力インターフェースにErrorProviderを設定
            IEditables.Clear();
            IEditables.AddRange(FindIEditable(this));
            IEditables.ForEach(ie => ie.ErrorProvider = ErrorProvider);
        }

        /// <summary>
        /// 全ての<see cref="Ricordanza.WinFormsUI.Forms.IEditable"/>が保有するエラープロバイダーの状態を初期値に戻します。
        /// </summary>
        protected virtual void ClearError()
        {
            IEditables.ForEach(ie => ie.ClearError());
        }

        /// <summary>
        /// 全ての<see cref="Ricordanza.WinFormsUI.Forms.IEditable"/>に対して<see cref="Ricordanza.WinFormsUI.Forms.IEditable.Clear"/>を実行します。
        /// </summary>
        protected virtual void ClearAll()
        {
            IEditables.ForEach(ie => ie.Clear());
        }

        /// <summary>
        /// 全ての<see cref="Ricordanza.WinFormsUI.Forms.IEditable"/>に対して入力チェックを実行します。
        /// </summary>
        /// <returns>入力NGの項目が存在する場合はtrue、それ以外の場合はfalse。</returns>
        protected virtual bool ValidateAll()
        {
            bool returnVal = false;
            IEditables.ForEach(
                ie =>
                {
                    bool ret = ie.Validate();
                    if (!returnVal)
                        returnVal = ret;
                });
            return returnVal;
        }

        #endregion

        #region private method

        /// <summary>
        /// <see cref="Ricordanza.WinFormsUI.Forms.IEditable"/>コントロールを取り出します。
        /// </summary>
        /// <param name="control">親にあたるコントロール。</param>
        /// <returns>取得対象コントロール配列。</returns>
        private IEditable[] FindIEditable(Control control)
        {
            List<IEditable> buf = new List<IEditable>();
            foreach (Control c in control.Controls)
            {
                IEditable iv = c as IEditable;
                if (iv != null)
                    buf.Add(iv);

                buf.AddRange(FindIEditable(c));
            }
            return buf.ToArray();
        }

        /// <summary>
        /// <see cref="Ricordanza.WinFormsUI.Forms.IBindKey"/>コントロールを取り出します。
        /// </summary>
        /// <param name="control">親にあたるコントロール。</param>
        /// <returns>取得対象コントロール配列。</returns>
        private IBindKey[] FindIBindKey(Control control)
        {
            List<IBindKey> buf = new List<IBindKey>();
            foreach (Control c in control.Controls)
            {
                IBindKey iv = c as IBindKey;
                if (iv != null)
                    buf.Add(iv);

                buf.AddRange(FindIBindKey(c));
            }
            return buf.ToArray();
        }

        /// <summary>
        /// コンポーネントを初期化します。
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CoreForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "CoreForm";
            this.ResumeLayout(false);
        }

        #endregion

        #region delegate

        #endregion
    }

    #endregion

    #region WindowStateChangingEventArgs

    /// <summary>
    /// WindowStateChanging イベントのデータを提供します。
    /// </summary>
    public class WindowStateChangingEventArgs : CancelEventArgs
    {
        #region const

        #endregion

        #region protected variable

        #endregion

        #region property

        /// <summary>
        /// 新しいウィンドウ状態を取得します。
        /// </summary>
        public FormWindowState WindowState { private set; get; }

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// このクラスの新しいインスタンスを取得します。
        /// </summary>
        /// <param name="windowState">フォームウィンドウの表示サイズ</param>
        public WindowStateChangingEventArgs(FormWindowState windowState)
        {
            WindowState = windowState;
        }

        #endregion

        #region event method

        #endregion

        #region public method

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }

    #endregion

    #region WindowStateChangedEventArgs

    /// <summary>
    /// WindowStateChanged イベントのデータを提供します。
    /// </summary>
    public class WindowStateChangedEventArgs : EventArgs
    {
        #region const

        #endregion

        #region protected variable

        #endregion

        #region property

        /// <summary>
        /// 新しいウィンドウ状態を取得します。
        /// </summary>
        public FormWindowState WindowState { private set; get; }

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// このクラスの新しいインスタンスを取得します。
        /// </summary>
        /// <param name="windowState">フォームウィンドウの表示サイズ</param>
        public WindowStateChangedEventArgs(FormWindowState windowState)
        {
            WindowState = windowState;
        }

        #endregion

        #region event method

        #endregion

        #region public method

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
