using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ricordanza.Plugin
{
    /// <summary>
    /// ダイアログ形式のプラグインが実装するインターフェイス
    /// </summary>
    /// <remarks>
    /// 引数なしコンストラクタを定義する必要があります。
    /// </remarks>
    public interface IDialogBasedPlugin
        : IPlugin, IDisposable
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

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// <see cref="Ricordanza.Plugin.IPlugin"/>を実行します。
        /// </summary>
        /// <param name="owner">モーダル ダイアログ ボックスを所有するトップレベル ウィンドウを表す<see cref="System.Windows.Forms.IWin32Window"/>を実装しているオブジェクト。</param>
        void Invoke(IWin32Window owner);

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }
}
