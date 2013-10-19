using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Ricordanza.Security
{
    /// <summary>
    /// AES形式の文字列暗号化モジュールです。
    /// </summary>
    /// <example>
    /// 秘匿化キー指定<br />
    /// <code>
    /// AesStringCryptor.CryptKey = "hogehoge";
    /// </code>
    /// 暗号化<br />
    /// <code>
    /// var enc = AesStringCryptor.Encrypt("hoge");
    /// var enc = "hoge".Encrypt();
    /// </code>
    /// カバーデータ取り出し<br />
    /// <code>
    /// 復号化<br />
    /// <code>
    /// var dec = AesStringCryptor.Decrypt(enc);
    /// var dec = "a1esc3ce8p72a6b88".Decrypt();
    /// </code>
    /// </example>
    public static class AesStringCryptor
    {
        #region const

        /// <summary>
        /// デフォルトで使用する演算の反復処理回数
        /// </summary>
        const int DEFAULT_ITERATIONS = 3146;

        #endregion

        #region private variable

        /// <summary>
        /// ロックオブジェクト
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// AES (Advanced Encryption Standard) アルゴリズム
        /// </summary>
        private static StringCryptor<AesCryptoServiceProvider> _cryptor;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        /// <remarks></remarks>
        static AesStringCryptor()
        {
        }

        #endregion

        #region property

        /// <summary>
        /// キーを設定します。
        /// </summary>
        public static string CryptKey
        {
            set
            {
                // キーが空文字 or nullの場合はキーとして処理しない。
                if (string.IsNullOrEmpty(value))
                    return;

                // 暗号化モジュールの構築
                lock (_lock)
                    _cryptor = new StringCryptor<AesCryptoServiceProvider>(value, typeof(AesStringCryptor).FullName, DEFAULT_ITERATIONS);
            }
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
        /// 対象文字列を暗号化します。
        /// </summary>
        /// <param name="val">暗号化した文字列</param>
        /// <returns>暗号化した文字列</returns>
        /// <remarks>
        /// 対象文字列が空文字 <c>or</c> <c>null</c>の場合や、暗号化に失敗した場合は引数<c>val</c>をそのまま返却します。
        /// </remarks>
        public static string Encrypt(this string val)
        {
            // キーが未定義の場合はvalをそのまま返却
            if (_cryptor == null)
                return string.Empty;

            return _cryptor.Encrypt(val);
        }

        /// <summary>
        /// 対象文字列を復号化します。
        /// </summary>
        /// <param name="val">復号化したい文字列</param>
        /// <returns>復号化した文字列</returns>
        /// <remarks>
        /// 対象文字列が空文字 <c>or</c> <c>null</c>の場合や、復号化に失敗した場合は引数<c>val</c>をそのまま返却します。
        /// </remarks>
        public static string Decrypt(this string val)
        {
            // キーが未定義の場合はvalをそのまま返却
            if (_cryptor == null)
                return val;

            return _cryptor.Decrypt(val);
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
