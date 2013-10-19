using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Ricordanza.Core;
using Ricordanza.Core.Utilities;
using Ricordanza.WinFormsUI.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    #region CoreTextBox

    /// <summary>
    /// 全てのテキストボックスの基底クラスです。
    /// </summary>
    public class CoreTextBox : TextBox, IEditable
    {
        #region const

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// このクラスのインスタンスを構築します。
        /// </summary>
        public CoreTextBox()
        {
            WaterMark = string.Empty;
            WaterMarkColor = Color.Silver;
            Required = false;
        }

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

        #endregion

        #region event

        /// <summary>
        /// テキスト変更イベント。
        /// </summary>
        [Category("Ricordanza")]
        [Description("Textプロパティの値がコントロールで変更される直前に発生します。")]
        public event TextChangingEventHandler TextChanging;

        #endregion

        #region event method

        /// <summary>
        /// TextChangingを発生させます。
        /// </summary>
        /// <param name="e">イベントデータを格納している<see cref="Ricordanza.WinFormsUI.Forms.TextChangingEventArg"/>。</param>
        protected virtual void OnTextChanging(TextChangingEventArgs e)
        {
            if (TextChanging != null)
                TextChanging(this, e);
        }

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
                    if (!DesignMode && !ContainsFocus && !ReadOnly && Enabled && Text.IsEmpty())
                        this.DrawWaterMark(Font, WaterMark, WaterMarkColor, new Point(1, 1));
                    break;

                case WindowMessages.WM_KEYDOWN:
                    if (m.WParam.ToInt32() == (int)Keys.Delete)
                    {
                        TextChangingEventArgs e1 = new TextChangingEventArgs(this.CreateNewText(string.Empty, true));
                        this.OnTextChanging(e1);
                        if (e1.Cancel)
                        {
                            m.Result = new IntPtr(1); // true
                            return;
                        }
                    }
                    break;

                case WindowMessages.WM_CHAR:
                    TextChangingEventArgs e2 = new TextChangingEventArgs(CreateNewText(Convert.ToString((char)(m.WParam.ToInt32()))));
                    this.OnTextChanging(e2);
                    if (e2.Cancel)
                        return;
                    break;

                case WindowMessages.WM_PASTE:
                    string clipText = Clipboard.GetDataObject().GetData(DataFormats.Text) as string;
                    if (clipText != null)
                    {
                        TextChangingEventArgs e3 = new TextChangingEventArgs(this.CreateNewText(clipText));
                        this.OnTextChanging(e3);
                        if (e3.Cancel)
                            return;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        #endregion

        #region private method

        /// <summary>
        /// 擬似的に入力後のテキストを作成します。
        /// </summary>
        /// <param name="inputText">テキスト。</param>
        /// <returns>擬似的に作成した入力後のテキスト。</returns>
        private string CreateNewText(string inputText)
        {
            return this.CreateNewText(inputText, false);
        }

        /// <summary>
        /// 擬似的に入力後のテキストを作成します。
        /// </summary>
        /// <param name="inputText">入力された値。</param>
        /// <param name="deleteKey">デリートキーが押下された場合はtrue それ以外の場合はfalse。</param>
        /// <returns>擬似的に作成した入力後のテキスト。</returns>
        private string CreateNewText(string inputText, bool deleteKey)
        {
            string newText = this.Text.Remove(this.SelectionStart, this.SelectionLength);

            // Delete キーを入力
            if (deleteKey)
            {
                if (this.SelectionLength == 0 && this.SelectionStart < this.TextLength)
                    newText = newText.Remove(this.SelectionStart, 1);

                return newText;
            }

            if (!this.Multiline)
            {
                int crIndex = inputText.IndexOf("\r");
                if (crIndex > 0)
                    inputText = inputText.Remove(crIndex);

                int lrIndex = inputText.IndexOf("\n");
                if (lrIndex > 0)
                    inputText = inputText.Remove(lrIndex);
            }

            if (inputText.IsEmpty())
                return newText;

            // BackSpace キーを入力
            if (Convert.ToInt32(inputText[0]) == (int)Keys.Back)
            {
                if (this.SelectionLength == 0 && this.SelectionStart > 0)
                    newText = newText.Remove(this.SelectionStart - 1, 1);
            }
            else
                newText = newText.Insert(this.SelectionStart, inputText);

            return newText;
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
        /// エラープロバイダの通知をクリアします。
        /// </summary>
        public void ClearError()
        {
            RaiseError(null);
        }

        /// <summary>
        /// エラープロバイダで通知を行います。
        /// </summary>
        /// <param name="message">通知するメッセージ。</param>
        public void RaiseError(string message)
        {
            if (ErrorProvider == null)
                return;

            ErrorProvider.SetError(this, message);
        }

        /// <summary>
        /// 入力値検証を行います。
        /// </summary>
        /// <returns>入力値が適切な場合はture、それ以外の場合はfalse。</returns>
        public virtual bool Validate()
        {
            // プロバイダを初期化
            ClearError();

            // 無効時は検証を行わない
            if (!Enabled && ReadOnly)
                return true;

            // 必須入力チェック
            if (Required && Text.IsEmpty())
            {
                RaiseError(global::Ricordanza.WinFormsUI.Properties.Resources.MSG001);
                return false;
            }

            // 桁数チェック
            if (Text.Length > MaxLength)
            {
                RaiseError(global::Ricordanza.WinFormsUI.Properties.Resources.MSG002);
                return false;
            }

            // ユーザ定義型の入力チェックの実行
            return (CustomValidate ?? (() => { return true; }))();
        }

        #endregion
    }

    #endregion

    #region TextChangingEventHandler

    /// <summary>
    /// テキスト変更時のdelegate。
    /// </summary>
    /// <param name="sender">イベントのソース。</param>
    /// <param name="e">イベントデータを格納している<see cref="Ricordanza.WinFormsUI.Forms.TextChangingEventArgs"/>。</param>
    public delegate void TextChangingEventHandler(object sender, TextChangingEventArgs e);

    #endregion

    #region TextChangingEventArgs

    /// <summary>
    /// テキスト変更中を示すイベントです。
    /// </summary>
    public class TextChangingEventArgs : CancelEventArgs
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// 擬似的に作成した入力後のテキスト。
        /// </summary>
        private string newText;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// 新しいこのクラスのインスタンスを構築します。
        /// </summary>
        public TextChangingEventArgs()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// 新しいこのクラスのインスタンスを構築します。
        /// </summary>
        /// <param name="text">擬似的に作成した入力後のテキスト。</param>
        public TextChangingEventArgs(string text)
            : base()
        {
            this.newText = text;
        }

        #endregion

        #region property

        /// <summary>
        /// 擬似的に作成した入力後のテキストを取得します。
        /// </summary>
        public string NewText
        {
            get { return this.newText; }
        }

        #endregion

        #region event method

        #region form

        #endregion

        #region context menu

        #endregion

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
