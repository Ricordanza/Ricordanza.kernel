using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Ricordanza.Resource
{
    #region ResourceManager

    /// <summary>
    /// リソース管理クラスです。
    /// </summary>
    public static class ResourceManager
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// ロックオブジェクト
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// リソース管理インスタンス
        /// </summary>
        private static Dictionary<string, Resources> _dictionary;

        /// <summary>
        /// 操作中のカテゴリー
        /// </summary>
        private static string _currentCategory;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        static ResourceManager()
        {
            CurrentCategory = string.Empty;
            _dictionary = new Dictionary<string, Resources>();
        }

        #endregion

        #region property

        /// <summary>
        /// 操作対象のカテゴリー
        /// </summary>
        public static string CurrentCategory
        {
            set
            {
                if (_dictionary.ContainsKey(value))
                    throw new ArgumentException(string.Format("{0} not found.", value));

                _currentCategory = value;
            }
            get
            {
                return _currentCategory;
            }
        }

        /// <summary>
        /// 管理中のカテゴリーのコレクション
        /// </summary>
        public static Dictionary<string, Resources>.KeyCollection Keys { get { return _dictionary.Keys; } }

        /// <summary>
        /// 管理中の<see cref="Ricordanza.Resource.Resources"/>のコレクション
        /// </summary>
        public static Dictionary<string, Resources>.ValueCollection Values { get { return _dictionary.Values; } }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// リソースファイルを読み込みます。
        /// </summary>
        /// <param name="path">リソースファイル格納パス</param>
        public static void Load(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path is null or empty.");

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(string.Format("{0} doesn't exists.", path));

            lock (_lock)
            {
                // 操作対象のカテゴリーを初期化
                _currentCategory = string.Empty;

                // 現在の読み込み状態をクリア
                Clear();

                // xmlを読み込む
                Directory.GetFiles(path, "*.xml").OrderBy(f => f).ToList().ForEach(
                    xml =>
                    {
                        Resources rs = new Resources();
                        XElement element = XElement.Load(xml);

                        // ノードが不正な場合は処理しない
                        if (element.Name != "resources")
                            return;

                        // ファイルパスの取得
                        rs.Path = xml;

                        // カテゴリー取得
                        rs.Category = element.Attribute("category").Value;

                        // resource要素を読み込む
                        var result = from node in element.Elements() where (node.Name == "resource") select node;
                        foreach (var ret in result)
                            rs.Add(ret.Attribute("key").Value, ret.Value);

                        // 追加
                        _dictionary.Add(rs.Category, rs);
                    });

                // 操作カテゴリーを設定
                foreach (var rs in _dictionary.Values)
                {
                    _currentCategory = rs.Category;
                    break;
                }
            }
        }

        /// <summary>
        /// 操作中の<c>category</c>、<c>key</c>に一致するリソースを取得します。
        /// </summary>
        /// <param name="key">リソースキー</param>
        /// <returns>リソース</returns>
        public static string Get(string key)
        {
            return Get(CurrentCategory, key);
        }

        /// <summary>
        /// <c>category</c>、<c>key</c>に一致するリソースを取得します。
        /// </summary>
        /// <param name="category">カテゴリー</param>
        /// <param name="key">リソースキー</param>
        /// <returns>リソース</returns>
        public static string Get(string category, string key)
        {
            return Get(category, key, new string[] { });
        }

        /// <summary>
        /// 操作中の<c>category</c>、<c>key</c>に一致するリソースを取得します。
        /// </summary>
        /// <param name="key">リソースキー</param>
        /// <param name="paramters">プレースフォルダを置換するパラメータ</param>
        /// <returns>リソース</returns>
        public static string Get(string key, params object[] paramters)
        {
            return Get(CurrentCategory, key, paramters);
        }

        /// <summary>
        /// <c>category</c>、<c>key</c>に一致するリソースを取得します。
        /// </summary>
        /// <param name="category">カテゴリー</param>
        /// <param name="key">リソースキー</param>
        /// <param name="paramters">プレースフォルダを置換するパラメータ</param>
        /// <returns>リソース</returns>
        public static string Get(string category, string key, params object[] paramters)
        {
            if (string.IsNullOrEmpty(category))
                throw new ArgumentException(string.Format("{0} is null or empty.", category));

            if (_dictionary.ContainsKey(category))
                throw new ArgumentException(string.Format("{0} not found.", category));

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(string.Format("{0} is null or empty.", key));

            if (_dictionary[category].ContainsKey(key))
                throw new ArgumentException(string.Format("{0} not found.", key));

            string msg = _dictionary[category][key];

            // パラメータが指定されている場合
            if (paramters != null && paramters.Length > 0)
                msg = string.Format(msg, paramters);

            return msg;
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

    #endregion

    #region Resources

    /// <summary>
    /// resources要素にマッピングするクラスです。
    /// </summary>
    public class Resources
        : Dictionary<string, string>
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
        public Resources()
        {
            Category = string.Empty;
            Path = string.Empty;
        }

        #endregion

        #region property

        /// <summary>
        /// category要素
        /// </summary>
        public string Category { protected internal set; get; }

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string Path { protected internal set; get; }

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
