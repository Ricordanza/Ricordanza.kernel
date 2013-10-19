using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Ricordanza.Core;
using Ricordanza.Core.Utilities;
using Ricordanza.WinFormsUI.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    /// <summary>
    ///  全てのDateTimePickerの基底クラスです。
    /// </summary>
    public class CoreDateTimePicker : DateTimePicker, IEditable
    {
        #region const

        /// <summary>
        /// 値が未定義であると判定する時に使用する文字列
        /// </summary>
        const string EMPTY_VALUE = " ";

        #endregion

        #region private variable

        /// <summary>
        /// 旧書式
        /// </summary>
        private DateTimePickerFormat format;

        /// <summary>
        /// 旧カスタム書式
        /// </summary>
        private string customFormat;

        /// <summary>
        /// 書式化した日付文字列
        /// </summary>
        private string formatAsString;

        /// <summary>
        /// 値が空か判定フラグ
        /// </summary>
        private bool isNull;

        /// <summary>
        /// null時の表示文字
        /// </summary>
        private string nullValue;

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
            get
            {
                if (StringUtility.IsEmpty(Value))
                    return true;

                return object.Equals(Value, EMPTY_VALUE);
            }
        }

        /// <summary>
        /// このコントロールの現在の日付／時間の値です。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Ricordanza")]
        [Description("このコントロールの現在の日付／時間の値です。")]
        public new object Value
        {
            get
            {
                if (isNull)
                    return null;
                else
                    return base.Value;
            }
            set
            {
                if (value == null || value == DBNull.Value)
                    SetToNullValue();
                else
                {
                    SetToDateTimeValue();

                    DateTime dt;
                    if (DateTime.TryParse(value.ToString(), out dt))
                        base.Value = dt;
                }
            }
        }

        /// <summary>
        /// 書式を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DateTimePickerFormat.Short), TypeConverter(typeof(Enum))]
        public new DateTimePickerFormat Format
        {
            get { return format; }
            set
            {
                format = value;
                SetFormat();
                OnFormatChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// カスタム書式を取得または設定します。
        /// </summary>
        public new String CustomFormat
        {
            get { return customFormat; }
            set { customFormat = value; }
        }

        /// <summary>
        /// Null時の表示文字列を取得または設定します。
        /// </summary>
        [Browsable(false)]
        [Category("Ricordanza")]
        [Description("Null時の表示文字列を取得または設定します。")]
        [DefaultValue(" ")]
        protected String NullValue
        {
            get { return nullValue; }
            set { nullValue = value; }
        }

        /// <summary>
        /// 書式化した値を取得または設定します。
        /// </summary>
        [Browsable(true)]
        [Category("Ricordanza")]
        [Description("Null時の表示文字列を取得または設定します。")]
        private string FormatAsString
        {
            get { return formatAsString; }
            set
            {
                formatAsString = value;
                base.CustomFormat = value;
            }
        }

        ///// <summary>
        ///// コントロールの背景色を示す値を取得または設定します。
        ///// </summary>
        //[Browsable(true)]
        //[Category("Ricordanza")]
        //[EditorBrowsable()]
        //[Description("コントロールの背景色を示す値を取得または設定します。")]
        //public override Color BackColor
        //{
        //    get { return base.BackColor; }
        //    set { base.BackColor = value; }
        //}

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// このクラスのインスタンスを構築します。
        /// </summary>
        public CoreDateTimePicker()
            : base()
        {
            WaterMark = string.Empty;
            WaterMarkColor = Color.Silver;
            Required = false;

            base.Format = DateTimePickerFormat.Custom;
            NullValue = EMPTY_VALUE;
            Format = DateTimePickerFormat.Short;

            isNull = false;
            Required = false;

            // 空にする
            this.Value = null;
        }

        #endregion

        #region event method

        /// <summary>
        /// CloseUpを発生させます。
        /// </summary>
        /// <param name="eventargs">イベントデータを格納している<see cref="System.EventArgs"/>。</param>
        protected override void OnCloseUp(EventArgs eventargs)
        {
            if (Control.MouseButtons == MouseButtons.None)
            {
                if (isNull)
                {
                    SetToDateTimeValue();
                    isNull = false;
                }
            }
            base.OnCloseUp(eventargs);
        }

        /// <summary>
        /// KeyUpを発生させます。
        /// </summary>
        /// <param name="eventargs">イベントデータを格納している<see cref="System.Windows.Forms.KeyEventArgs"/>。</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.Value = null;
                OnValueChanged(EventArgs.Empty);
            }
            base.OnKeyUp(e);
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
            switch (m.Msg)
            {
                case WindowMessages.WM_PAINT:
                    if (!DesignMode && !ContainsFocus && Enabled && IsEmpty && (Text == EMPTY_VALUE))
                        this.DrawWaterMark(Font, WaterMark, WaterMarkColor, new Point(1, 3));
                    break;
                //case WindowMessages.WM_ERASEBKGND:
                //    using (Graphics g = Graphics.FromHdc(m.WParam))
                //    {
                //        using (SolidBrush backBrush = new SolidBrush(this.BackColor))
                //            g.FillRectangle(backBrush, base.ClientRectangle);
                //    }
                //    return;
            }

            base.WndProc(ref m);
        }

        #endregion

        #region private method

        /// <summary>
        /// 書式を設定します。
        /// </summary>
        private void SetFormat()
        {
            DateTimeFormatInfo dtf = Thread.CurrentThread.CurrentCulture.DateTimeFormat;
            switch (format)
            {
                case DateTimePickerFormat.Long:
                    FormatAsString = dtf.LongDatePattern;
                    break;
                case DateTimePickerFormat.Short:
                    FormatAsString = dtf.ShortDatePattern;
                    break;
                case DateTimePickerFormat.Time:
                    FormatAsString = dtf.ShortTimePattern;
                    break;
                case DateTimePickerFormat.Custom:
                    FormatAsString = this.CustomFormat;
                    break;
            }
        }

        /// <summary>
        /// null時の値を設定します。
        /// </summary>
        private void SetToNullValue()
        {
            isNull = true;
            base.CustomFormat = (string.IsNullOrEmpty(NullValue)) ? EMPTY_VALUE : "'" + NullValue + "'";
        }

        /// <summary>
        /// 値を設定します。
        /// </summary>
        private void SetToDateTimeValue()
        {
            if (isNull)
            {
                SetFormat();
                isNull = false;
                base.OnValueChanged(EventArgs.Empty);
            }
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
        /// テキスト ボックス コントロールからすべてのテキストを削除します。
        /// </summary>
        public void Clear()
        {
            SetToNullValue();
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
            if (!Enabled)
                return true;

            // 必須入力チェック
            if (Required && IsEmpty)
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
