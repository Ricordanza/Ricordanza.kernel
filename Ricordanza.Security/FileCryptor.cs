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
    /// ファイル暗号化モジュールです。
    /// </summary>
    /// <typeparam name="T">対称アルゴリズム</typeparam>
    public class FileCryptor<T> where T : SymmetricAlgorithm
    {
        #region const

        /// <summary>
        /// デフォルトで使用する演算の反復処理回数
        /// </summary>
        const int DEFAULT_ITERATIONS = 1882;

        #endregion

        #region private variable

        /// <summary>
        /// サービスプロバイダ
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
        public FileCryptor(string key)
            : this(key, typeof(FileCryptor<>).FullName)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key">暗号キー</param>
        /// <param name="salt">キーを派生させるために使用するキー。</param>
        public FileCryptor(string key, string salt)
            : this(key, Encoding.UTF8.GetBytes(salt))
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key">暗号キー</param>
        /// <param name="salt">キーを派生させるために使用するキー。</param>
        public FileCryptor(string key, byte[] salt)
            : this(key, salt, DEFAULT_ITERATIONS)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key">暗号キー</param>
        /// <param name="salt">キーを派生させるために使用するキー。</param>
        /// <param name="iterations">演算の反復処理回数。</param>
        public FileCryptor(string key, string salt, int iterations)
            : this(key, Encoding.UTF8.GetBytes(salt), iterations)
        {
        } 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key">暗号キー</param>
        /// <param name="salt">キーを派生させるために使用するキー。</param>
        /// <param name="iterations">演算の反復処理回数。</param>
        public FileCryptor(string key, byte[] salt, int iterations)
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
        /// 暗号化を行います。
        /// </summary>
        /// <param name="sourcePath">暗号化するファイル</param>
        /// <param name="destFile">暗号化ファイルの出力先</param>
        public void Encrypt(string sourceFile, string destFile)
        {
            if (string.IsNullOrEmpty(sourceFile))
                throw new ArgumentNullException("sourceFile is null or empty.");

            if (!File.Exists(sourceFile))
                throw new FileNotFoundException(string.Format("{0} doesn't exists.", sourceFile));

            // 暗号化するファイルを読み込む
            byte[] source;
            using (var fsIn = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
            {
                source = new byte[fsIn.Length];
                fsIn.Read(source, 0, source.Length);
            }

            // ファイルを暗号化
            using(var ms = new MemoryStream())
            {
                using(var cs = new CryptoStream(ms, _provider.CreateEncryptor(), CryptoStreamMode.Write))
                    cs.Write(source, 0, source.Length);

                using(var fsOut = new FileStream(destFile, FileMode.OpenOrCreate, FileAccess.Write))
                    fsOut.Write(ms.ToArray(), 0, ms.ToArray().Length);
            }
        }

        /// <summary>
        /// ファイルの複合化を行います。
        /// </summary>
        /// <param name="fileName">複合化したいファイル</param>
        /// <param name="destFile">複合化ファイルの出力先</param>
        public void Decrypt(string sourceFile, string destFile)
        {
            if (string.IsNullOrEmpty(sourceFile))
                throw new ArgumentNullException("sourceFile is null or empty.");

            if (!File.Exists(sourceFile))
                throw new FileNotFoundException(string.Format("{0} doesn't exists.", sourceFile));

            // 複合化するファイルを読み込む
            byte[] source;
            using (var fsIn = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
            {
                source = new byte[fsIn.Length];
                fsIn.Read(source, 0, source.Length);
            }

            // ファイルを暗号化
            using(var ms = new MemoryStream())
            {
                using(var cs = new CryptoStream(ms, _provider.CreateDecryptor(), CryptoStreamMode.Write))
                    cs.Write(source, 0, source.Length);

                using(var fsOut = new FileStream(destFile, FileMode.OpenOrCreate, FileAccess.Write))
                    fsOut.Write(ms.ToArray(), 0, ms.ToArray().Length);
            }
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
