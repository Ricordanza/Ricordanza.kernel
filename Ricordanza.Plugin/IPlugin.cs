using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.Plugin
{
    /// <summary>
    /// プラグインが実装するインターフェイス
    /// </summary>
    /// <remarks>
    /// 引数なしコンストラクタを定義する必要があります。
    /// </remarks>
    public interface IPlugin
        : ICloneable
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
        /// プラグインを識別する一意のとなる値
        /// </summary>
        string PluginId { get; }

        /// <summary>
        /// プラグインの名前
        /// </summary>
        string PluginName { get; }

        /// <summary>
        /// プラグインのバージョン
        /// </summary>
        string Version { get; }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// <see cref="Ricordanza.Plugin.IPlugin"/>実行前の初期化処理を行います。
        /// </summary>
        void Initalize();

        /// <summary>
        /// <see cref="Ricordanza.Plugin.IPlugin"/>を実行します。
        /// </summary>
        void Invoke();

        /// <summary>
        /// <see cref="Ricordanza.Plugin.IPlugin"/>実行後の後処理を行います。
        /// </summary>
        void Terminate();

        /// <summary>
        /// パラメータを設定します。
        /// </summary>
        /// <typeparam name="T">パラメータの型</typeparam>
        /// <param name="paremter">パラメータ</param>
        void Parameter<T>(T paremter);

        /// <summary>
        /// 処理結果を取得します。
        /// </summary>
        /// <typeparam name="T">戻り値の型</typeparam>
        /// <returns>処理結果</returns>
        T Result<T>();

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }
}
