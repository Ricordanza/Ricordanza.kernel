using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.SnapShot.Command
{
    /// <summary>
    /// 履歴抽象クラス
    /// </summary>
    /// <typeparam name="T1">履歴管理を行うデータの型</typeparam>
    /// <typeparam name="T2">データ反映対象オブジェクトの型</typeparam>
    public abstract class CommandHistory<T1, T2>
    {
        #region const

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor

        #endregion

        #region property

        /// <summary>
        /// 履歴データを取得または設定します。
        /// </summary>
        public T1 Data { get; protected set; }

        /// <summary>
        /// データ反映対象オブジェクトを取得または設定します。
        /// </summary>
        protected T2 Target { get; set; }

        /// <summary>
        /// 履歴データを反映させます。
        /// </summary>
        /// <param name="data">履歴データ</param>
        public abstract void SetMemento(T1 data);

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
}
