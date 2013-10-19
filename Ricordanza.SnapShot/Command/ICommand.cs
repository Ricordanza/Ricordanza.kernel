using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.SnapShot.Command
{
    /// <summary>
    /// ICommandインターフェイス
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// ストックを行ないます
        /// </summary>
        void Save();

        /// <summary>
        /// 元に戻す
        /// </summary>
        void Undo();

        /// <summary>
        /// やり直し
        /// </summary>
        void Redo();
    }
}
