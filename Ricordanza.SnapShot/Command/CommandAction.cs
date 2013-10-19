using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.SnapShot.Command
{
    /// <summary>
    /// 履歴操作コマンド
    /// </summary>
    /// <typeparam name="T1">履歴管理を行うデータの型</typeparam>
    /// <typeparam name="T2">データ反映対象オブジェクトの型</typeparam>
    public sealed class CommandAction<T1, T2> : ICommand
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// 履歴
        /// </summary>
        private CommandHistory<T1, T2> _memento;
        
        /// <summary>
        /// 前の履歴データ
        /// </summary>
        private T1 _prev;
        
        /// <summary>
        /// 次の履歴データ
        /// </summary>
        private T1 _next;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="prev">前の履歴データ</param>
        /// <param name="next">次の履歴データ</param>
        public CommandAction(CommandHistory<T1, T2> prev, CommandHistory<T1, T2> next)
        {
            _memento = prev;
            _prev = prev.Data;
            _next = next.Data;
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

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion

        #region ICommand メンバ

        /// <summary>
        /// ストックを行ないます
        /// </summary>
        void ICommand.Save()
        {
            _prev = _memento.Data;
            _memento.SetMemento(_next);
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        void ICommand.Undo()
        {
            _memento.SetMemento(_prev);
        }

        /// <summary>
        /// やり直し
        /// </summary>
        void ICommand.Redo()
        {
            _memento.SetMemento(_next);
        }

        #endregion
    }
}
