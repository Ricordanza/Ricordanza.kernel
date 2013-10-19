using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace Ricordanza.Security
{
    /// <summary>
    /// 文字列暗号化モジュールです。
    /// </summary>
    /// <typeparam name="T">対称アルゴリズム</typeparam>
    public class StringCryptor<T> where T : SymmetricAlgorithm
    {
        #region const

        /// <summary>
        /// デフォルトで使用する演算の反復処理回数
        /// </summary>
        const int DEFAULT_ITERATIONS =  1687;

        #endregion

        #region private variable

        /// <summary>
        /// 対称アルゴリズム
        /// </summary>
        protected readonly SymmetricAlgorithm _provider;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key">暗号キー</param>
        public StringCryptor(string key)
            : this(key, typeof(StringCryptor<>).FullName)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key">暗号キー</param>
        /// <param name="salt">キーを派生させるために使用するキー。</param>
        public StringCryptor(string key, string salt)
            : this(key, Encoding.UTF8.GetBytes(salt))
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key">暗号キー</param>
        /// <param name="salt">キーを派生させるために使用するキー。</param>
        public StringCryptor(string key, byte[] salt)
            : this(key, salt, DEFAULT_ITERATIONS)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key">暗号キー</param>
        /// <param name="salt">キーを派生させるために使用するキー。</param>
        /// <param name="iterations">演算の反復処理回数。</param>
        public StringCryptor(string key, string salt, int iterations)
            : this(key, Encoding.UTF8.GetBytes(salt), iterations)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key">暗号キー</param>
        /// <param name="salt">キーを派生させるために使用するキー。</param>
        /// <param name="iterations">演算の反復処理回数。</param>
        public StringCryptor(string key, byte[] salt, int iterations)
            : base()
        {
            // キーが不正な場合
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key is null or empty.");

            // 対称アルゴリズムのインスタンスを構築
            _provider = Activator.CreateInstance(typeof(T)) as SymmetricAlgorithm;

            // 共有キーと初期化ベクタを設定
            GenerateVector(key, salt, iterations);
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
        /// 対象文字列を暗号化します。
        /// </summary>
        /// <param name="val">暗号化した文字列</param>
        /// <returns>暗号化した文字列</returns>
        /// <remarks>
        /// 対象文字列が空文字 <c>or</c> <c>null</c>の場合や、暗号化に失敗した場合は引数<c>val</c>をそのまま返却します。
        /// </remarks>
        public virtual string Encrypt(string val)
        {
            // 未定義の場合はvalをそのまま返却
            if (string.IsNullOrEmpty(val))
                return val;

            // 文字列をバイト型配列にする
            byte[] bytesIn = Encoding.UTF8.GetBytes(val);

            // 暗号化されたデータを書き出すためのMemoryStream
            byte[] bytesOut = null;
            using (var msOut = new MemoryStream())
            {
                // CryptoStreamの作成
                using (var cryptStreem = new CryptoStream(msOut, _provider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    // 書き込む
                    cryptStreem.Write(bytesIn, 0, bytesIn.Length);
                    cryptStreem.FlushFinalBlock();

                    // 暗号化されたデータを取得
                    bytesOut = msOut.ToArray();
                }
            }

            // Base64で文字列に変更して結果を返す
            return Convert.ToBase64String(bytesOut);
        }

        /// <summary>
        /// 対象文字列を復号化します。
        /// </summary>
        /// <param name="val">復号化したい文字列</param>
        /// <returns>復号化した文字列</returns>
        /// <remarks>
        /// 対象文字列が空文字 <c>or</c> <c>null</c>の場合や、復号化に失敗した場合は引数<c>val</c>をそのまま返却します。
        /// </remarks>
        public virtual string Decrypt(string val)
        {
            // 未定義の場合はvalをそのまま返却
            if (string.IsNullOrEmpty(val))
                return val;

            // Base64で文字列をバイト配列に戻す
            byte[] bytesIn = Convert.FromBase64String(val);

            // 暗号化されたデータを読み込むためのMemoryStream
            string result = string.Empty;
            using (var msIn = new MemoryStream(bytesIn))
            {
                // 読み込むためのCryptoStreamの作成
                using (var cryptStreem = new CryptoStream(msIn, _provider.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    // 復号化されたデータを取得する
                    using (var srOut = new StreamReader(cryptStreem, Encoding.UTF8))
                        result = srOut.ReadToEnd();
                }
            }

            // 復号化結果を返却
            return result;
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        /// <summary>
        /// パスワードから共有キーと初期化ベクタを生成する
        /// </summary>
        /// <param name="password">基になるパスワード</param>
        /// <param name="salt">キーを派生させるために使用するキー。</param>
        /// <param name="iterations">演算の反復処理回数。</param>
        protected virtual void GenerateVector(string password, byte[] salt, int iterations)
        {
            // Rfc2898DeriveBytesオブジェクトを作成する
            var deriveBytes = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), salt, iterations);

            // 共有キーと初期化ベクタを生成する
            _provider.Key = deriveBytes.GetBytes(_provider.KeySize / _provider.FeedbackSize);
            _provider.IV = deriveBytes.GetBytes(_provider.BlockSize / _provider.FeedbackSize);
        }

        #endregion

        #region delegate

        #endregion
    }
}
