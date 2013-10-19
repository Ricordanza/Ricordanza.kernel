using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.Property
{
    #region FieldValidator

    /// <summary>
    /// フィールドに検証機能を追加します。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FieldValidator<T>
    {
        #region const

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor
        
        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public FieldValidator()
            : base()
        {
        }

        #endregion

        #region property

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// フィールドに検証機能を行います。
        /// </summary>
        /// <param name="value">検証を行いたい値</param>
        /// <rereturns>妥当な値の場合は<c>true</c>、それ以外の場合は<c>false</c>。</rereturns>
        public virtual bool Validate(T value) { return true; }

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }

    #endregion

    #region NumberRangeFieldValidator

    /// <summary>
    /// 数値範囲検証機能クラスです。
    /// </summary>
    public class NumberRangeFieldValidator : FieldValidator<decimal>
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// 最小値
        /// </summary>
        private readonly decimal _min;

        /// <summary>
        /// 最大値
        /// </summary>
        private readonly decimal _max;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        #endregion

        #region property

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

        /// <summary>
        /// 新しいこのクラスのインスタンスを構築します。
        /// </summary>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        public NumberRangeFieldValidator(decimal min, decimal max)
            : base()
        {
            _min = min;
            _max = max;
        }

        /// <summary>
        /// フィールドに数値範囲検証機能を行います。
        /// </summary>
        /// <param name="value">検証を行いたい値</param>
        /// <rereturns>妥当な値の場合は<c>true</c>、それ以外の場合は<c>false</c>。</rereturns>
        public override bool Validate(decimal value)
        {
            if ((value < _min) || (value > _max))
                return false;

            return true;
        }
    }

    #endregion
}
