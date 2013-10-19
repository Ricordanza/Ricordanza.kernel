using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Ricordanza.Plugin
{
    /// <summary>
    /// プラグイン管理クラスです。
    /// </summary>
    public static class PluginManager
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// ロックオブジェクト
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// プラグイン管理インスタンス
        /// </summary>
        private static Dictionary<string, IPlugin> _dictionary;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        static PluginManager()
        {
            _dictionary = new Dictionary<string, IPlugin>();
        }

        #endregion

        #region property

        /// <summary>
        /// 読み込んだ<see cref="Ricordanza.Plugin.IPlugin"/>形式のインスタンス
        /// </summary>
        public static IEnumerable<IPlugin> IPlugins
        {
            get { return _dictionary.Values.OfType<IPlugin>(); }
        }

        /// <summary>
        /// 読み込んだ<see cref="Ricordanza.Plugin.IDialogBasedPlugin"/>形式のインスタンス
        /// </summary>
        public static IEnumerable<IDialogBasedPlugin> IDialogBasedPlugins
        {
            get { return _dictionary.Values.OfType<IDialogBasedPlugin>(); }
        }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// プラグインを読み込みます。
        /// </summary>
        /// <param name="path">プラグイン格納パス</param>
        public static void Load(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path is null or empty.");

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(string.Format("{0} doesn't exists.", path));

            lock (_lock)
            {
                // 現在の読み込み状態をクリア
                Clear();

                Directory.GetFiles(path, "*.dll").OrderBy(f => f).ToList().ForEach(
                    dll =>
                    {
                        // アセンブリを読み込む
                        Assembly asm = Assembly.LoadFrom(dll);

                        // アセンブリからクラスの一覧を取得する
                        asm.GetTypes().ToList().ForEach(
                             t =>
                             {
                                 // アセンブリ内のすべての型についてプラグインとして有効か調べる
                                 if (t.IsClass
                                     && t.IsPublic
                                     && !t.IsAbstract
                                     && t.GetInterface(typeof(IPlugin).FullName) != null)
                                 {
                                     // デフォルトコンストラクタを取得
                                     ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);
                                     // デフォルトコンストラクタ未定義の場合は処理を行わない
                                     if (ci == null)
                                         return;

                                     // インスタンス作成
                                     IPlugin instance = ci.Invoke(new object[] { }) as IPlugin;
                                     // IPlugin型のインスタンスが構築出来なかった場合は処理を行わない
                                     if (instance == null)
                                         return;

                                     // 追加
                                     _dictionary.Add(instance.PluginId, instance);
                                 }
                             });
                    });
            }
        }

        /// <summary>
        /// 全てのプラググイン情報を削除します。
        /// </summary>
        public static void Clear()
        {
            _dictionary.Clear();
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }
}
