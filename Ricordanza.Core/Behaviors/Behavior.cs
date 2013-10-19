using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.Core.Behaviors
{
    #region IBehavior

    /// <summary>
    /// ビジネスロジックのインターフェイスです。
    /// </summary>
    public interface IBehavior
    {
        #region constant

        #endregion

        #region private variable

        #endregion

        #region protected variable

        #endregion

        #region property

        /// <summary>
        /// <see cref="Ricordanza.Core.Behaviors.IBehavior"/>で発生した例外を取得します。
        /// </summary>
        Exception Exception { get; }

        /// <summary>
        /// 例外が発生したかどうかを判定します。
        /// </summary>
        bool HasError { get; }

        #endregion

        #region static constractor

        #endregion

        #region constractor

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// <see cref="Ricordanza.Core.Behaviors.IBehavior"/>実行前の初期化処理を行います。
        /// </summary>
        void Initalize();

        /// <summary>
        /// <see cref="Ricordanza.Core.Behaviors.IBehavior"/>を実行します。
        /// </summary>
        void Invoke();

        /// <summary>
        /// <see cref="Ricordanza.Core.Behaviors.IBehavior"/>実行後の後処理を行います。
        /// </summary>
        void Terminate();

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }

    #endregion

    #region Behavior

    /// <summary>
    /// ビジネスロジックの基底クラスです。
    /// </summary>
    /// <typeparam name="T1">パラメータの総称型</typeparam>
    /// <typeparam name="T2">実行結果の総称型</typeparam>
    public abstract class Behavior
        : IBehavior
    {
        #region constant

        #endregion

        #region private variable

        #endregion

        #region private variable

        #endregion

        #region property

        /// <summary>
        /// <see cref="Ricordanza.Core.Behaviors.IBehavior"/>で発生した例外を設定または取得します。
        /// </summary>
        public Exception Exception { protected set; get; }

        /// <summary>
        /// 例外が発生したかどうかを判定します。
        /// </summary>
        public bool HasError
        {
            get { return Exception != null; }
        }

        #endregion

        #region static constractor

        #endregion

        #region constractor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Behavior()
            : base()
        {
            Exception = null;
        }

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// <see cref="Ricordanza.Core.Behaviors.IBehavior"/>実行前の初期化処理を行います。
        /// </summary>
        public virtual void Initalize()
        {
        }

        /// <summary>
        /// <see cref="Ricordanza.Core.Behaviors.IBehavior"/>を実行します。
        /// </summary>
        public abstract void Invoke();

        /// <summary>
        ///<see cref="Ricordanza.Core.Behaviors.IBehavior"/>実行後の後処理を行います。
        /// </summary>
        public virtual void Terminate()
        {
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

    #region ParamBehavior

    /// <summary>
    /// ビジネスロジックの基底クラスです。
    /// </summary>
    /// <typeparam name="T">パラメータの総称型</typeparam>
    public abstract class ParamBehavior<T>
        : Behavior
    {
        #region constant

        #endregion

        #region private variable

        #endregion

        #region private variable

        #endregion

        #region property

        /// <summary>
        /// <see cref="Ricordanza.Core.Behaviors.ParamBehavior"/>のパラメータ。
        /// </summary>
        public T Parameter { set; get; }

        #endregion

        #region static constractor

        #endregion

        #region constractor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ParamBehavior()
            : base()
        {
            Parameter = default(T);
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

    #region ResultBehavior

    /// <summary>
    /// ビジネスロジックの基底クラスです。
    /// </summary>
    /// <typeparam name="TResult">実行結果の総称型</typeparam>
    public abstract class ResultBehavior<TResult>
        : Behavior
    {
        #region constant

        #endregion

        #region private variable

        #endregion

        #region private variable

        #endregion

        #region property

        #endregion

        #region static constractor

        #endregion

        #region constractor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ResultBehavior()
            : base()
        {
        }

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        ///<see cref="Ricordanza.Core.Behaviors.ResultBehavior"/>実行結果を取得します。
        /// </summary>
        /// <returns><see cref="Ricordanza.Core.Behaviors.ResultBehavior"/>実行結果</returns>
        public virtual TResult Result()
        {
            return default(TResult);
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

    #region ParamResultBehavior

    /// <summary>
    /// ビジネスロジックの基底クラスです。
    /// </summary>
    /// <typeparam name="T">パラメータの総称型</typeparam>
    /// <typeparam name="TResult">実行結果の総称型</typeparam>
    public abstract class ParamResultBehavior<T, TResult>
        : Behavior
    {
        #region constant

        #endregion

        #region private variable

        #endregion

        #region private variable

        #endregion

        #region property

        /// <summary>
        /// <see cref="Ricordanza.Core.Behaviors.ParamResultBehavior"/>のパラメータ。
        /// </summary>
        public T Parameter { set; get; }

        #endregion

        #region static constractor

        #endregion

        #region constractor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ParamResultBehavior()
            : base()
        {
            Parameter = default(T);
        }

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        ///<see cref="Ricordanza.Core.Behaviors.ParamResultBehavior"/>実行結果を取得します。
        /// </summary>
        /// <returns><see cref="Ricordanza.Core.Behaviors.ParamResultBehavior"/>実行結果</returns>
        public virtual TResult Result()
        {
            return default(TResult);
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
