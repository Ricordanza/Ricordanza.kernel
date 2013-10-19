using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.Property
{
    /// <summary>
    /// ワンライナー形式のフィールドオブジェクトです。<br />
    /// このクラスは値の妥当性検証と変更通知機能を有しています。
    /// </summary>
    /// <typeparam name="T">管理するデータ型</typeparam>
    /// <example>
    /// <code>
    /// public class InvoiceLineItem
    /// {
    ///     public readonly Field&lt;int&gt; LineNumber = new Field&lt;int&gt;(10);
    ///     public readonly Field&lt;decimal&gt; Quantity = new Field&lt;decimal&gt;(10, new NumberRangeFieldValidator(1, 100));
    ///     public readonly Field&lt;decimal&gt; Tax = new Field&lt;decimal&gt;(new NumberRangeFieldValidator(1, 10));
    ///     public readonly Field&lt;bool&gt; Taxable = new Field&lt;bool&gt;();
    ///
    ///     public InvoiceLineItem()
    ///     {
    ///         LineNumber.ValueChanged += (f, e) =&gt; { Console.WriteLine(e.OldValue + " : " + e.NewValue); };
    ///     }
    /// }
    /// </code>
    /// </example>
    public class Field<T>
    {
        #region const

        #endregion

        #region protected variable

        /// <summary>
        /// 入力検証機能
        /// </summary>
        protected readonly FieldValidator<T> _validator;

        /// <summary>
        /// フィールドとして保持している値
        /// </summary>
        protected T _value;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// 新しいこのクラスのインスタンスを構築します。
        /// </summary>
        public Field()
            : this(new FieldValidator<T>())
        {
        }

        /// <summary>
        /// 新しいこのクラスのインスタンスを構築します。
        /// </summary>
        /// <param name="value">初期値</param>
        public Field(T value)
            : this(value, new FieldValidator<T>())
        {
        }

        /// <summary>
        /// 新しいこのクラスのインスタンスを構築します。
        /// </summary>
        /// <param name="validator">入力検証機能</param>
        public Field(FieldValidator<T> validator)
            : base()
        {
            _validator = (validator ?? new FieldValidator<T>());
        }

        /// <summary>
        /// 新しいこのクラスのインスタンスを構築します。
        /// </summary>
        /// <param name="value">初期値</param>
        /// <param name="validator">入力検証機能</param>
        public Field(T value, FieldValidator<T> validator)
            : this(validator)
        {
            Value = value;
        }

        #endregion

        #region property

        /// <summary>
        /// このクラスが保持している値を設定または取得します。
        /// </summary>
        public T Value
        {
            get { return _value; }
            set
            {
                // 値が変更されているか？
                if (!Equals(_value, value))
                {
                    // 値は妥当か？
                    if (_validator.Validate(value))
                    {
                        T oldValue = _value;
                        _value = value;

                        // 変更イベントを通知
                        OnValueChanged(oldValue, _value);
                    }
                }
            }
        }

        #endregion

        #region event

        /// <summary>
        /// 値が変更された場合に発生します。
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        #endregion

        #region event handler

        #endregion

        #region event method

        /// <summary>
        /// ValueChangedイベントを発生させます。
        /// </summary>
        /// <param name="oldValue">変更前の値</param>
        /// <param name="newValue">変更後の値</param>
        protected virtual void OnValueChanged(T oldValue, T newValue)
        {
            if (ValueChanged != null)
                ValueChanged(this, new ValueChangedEventArgs(oldValue, newValue));
        }

        #endregion

        #region public method

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion

        #region ValueChangedEventArgs

        /// <summary>
        /// 値変更イベントデータが格納されているクラスの基本クラス
        /// </summary>
        public class ValueChangedEventArgs
            : EventArgs
        {
            #region const

            #endregion

            #region private variable

            #endregion

            #region static constructor

            #endregion

            #region constructor

            /// <summary>
            /// 新しいこのクラスのインスタンスを構築します。
            /// </summary>
            /// <param name="oldValue">変更前の値</param>
            /// <param name="newValue">変更後の値</param>
            internal ValueChangedEventArgs(T oldValue, T newValue)
                : base()
            {
                OldValue = oldValue;
                NewValue = newValue;
            }

            #endregion

            #region property

            /// <summary>
            /// 変更前の値
            /// </summary>
            public readonly T OldValue;

            /// <summary>
            /// 変更後の値
            /// </summary>
            public readonly T NewValue;

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

            #endregion

            #region delegate

            #endregion
        }

        #endregion
    }
}
