using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Ricordanza.Core;
using Ricordanza.Core.Utilities;
using Ricordanza.WinFormsUI.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    #region CoreComboBox

    /// <summary>
    /// 全てのコンボボックスの基底クラスです。
    /// </summary>
    public class CoreComboBox : ComboBox, IEditable
    {
        #region const

        #endregion

        #region protected variable

        #endregion

        #region property

        /// <summary>
        /// 透かし文字を取得または設定します。
        /// </summary>
        [Category("Ricordanza")]
        [Description("透かし文字を取得または設定します。この属性はDropDownStyleがDropDownListの場合のみ有効です。")]
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

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// このクラスのインスタンスを構築します。
        /// </summary>
        public CoreComboBox()
        {
            WaterMark = string.Empty;
            WaterMarkColor = Color.Silver;
            Required = false;
        }

        #endregion

        #region event method

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
                    if (!DesignMode && !ContainsFocus && DropDownStyle == ComboBoxStyle.DropDownList && Enabled && Text.IsEmpty())
                        this.DrawWaterMark(Font, WaterMark, WaterMarkColor, new Point(1, 3));
                    break;
            }
            base.WndProc(ref m);
        }

        #endregion

        #region private method

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
        /// 未選択選択状態にします。
        /// </summary>
        public void Clear()
        {
            if (DropDownStyle != ComboBoxStyle.DropDownList)
                Text = string.Empty;

            SelectedIndex = -1;
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
            if (Required && this.Text.IsEmpty())
            {
                RaiseError(global::Ricordanza.WinFormsUI.Properties.Resources.MSG001);
                return false;
            }

            // ユーザ定義型の入力チェックの実行
            return (CustomValidate ?? (() => { return true; }))();
        }

        #endregion
    }

    #endregion

    #region SelectItem

    /// <summary>
    /// <see cref="CoreComboBox"/> 用の選択項目です。
    /// </summary>
    public class SelectItem<T>
    {
        #region property

        /// <summary>
        /// 項目名称を取得または設定します。
        /// </summary>
        public string Text { protected set; get; }

        /// <summary>
        /// 項目値を取得または設定します。
        /// </summary>
        public T Value { protected set; get; }

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        protected SelectItem()
        {
            Text = string.Empty;
            Value = default(T);
        }

        /// <summary>
        /// 項目名称、項目値を基にこのクラスのインスタンスを構築します。
        /// </summary>
        /// <param name="text">項目名称</param>
        /// <param name="value">項目値</param>
        public SelectItem(string text, T value)
            : this()
        {
            Text = text.EmptyToStr(string.Empty);
            Value = value;
        }

        #endregion

        #region public method

        /// <summary>
        /// このクラスを表す文字列を取得します。
        /// </summary>
        /// <returns>項目名称</returns>
        public override string ToString()
        {
            return Text;
        }

        #endregion
    }

    #endregion
}
