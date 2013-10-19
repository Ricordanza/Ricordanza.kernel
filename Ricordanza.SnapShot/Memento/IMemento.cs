using System;
using System.Collections.Generic;
using System.Text;

namespace Ricordanza.ShapShot.Memento
{
    /// <summary>
    /// スナップショット機能を実装します。
    /// </summary>
    /// <typeparam name="T">スナップショット機能の実体型</typeparam>
    public interface IMemento<T>
    {
        /// <summary>
        /// スナップショットからリストア処理を行います。
        /// </summary>
        /// <param name="target">リストア対象</param>
        /// <returns>リストア後のスナップショット</returns>
        IMemento<T> Restore(T target);

        /// <summary>
        /// スナップショットとして保存する為のクローンを作成します。
        /// </summary>
        /// <returns>スナップショットとして保存する為のクローン</returns>
        IMemento<T> Clone();
    }
}
