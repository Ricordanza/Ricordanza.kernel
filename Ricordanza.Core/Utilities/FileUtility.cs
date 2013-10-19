using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Ricordanza.Core.Utilities
{
    /// <summary>
    /// ファイル操作のユーティリティクラスです。
    /// </summary>
    /// <remarks>このクラスは<see cref="System.IO.FileSystemInfo"/>派生クラスを中心に拡張します。</remarks>
    public static class FileUtility
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
        /// ディレクトリのコピーを行います。
        /// </summary>
        /// <param name="source">コピー元ディレクトリ</param>
        /// <param name="dest">コピー先ディレクトリ</param>
        public static void DirectoryCopy(string source, string dest)
        {
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
                File.SetAttributes(dest, File.GetAttributes(source));
            }

            if (dest[dest.Length - 1] != Path.DirectorySeparatorChar)
                dest = dest + Path.DirectorySeparatorChar;

            foreach (string file in Directory.GetFiles(source))
                File.Copy(file, dest + Path.GetFileName(file), true);

            foreach (string dir in Directory.GetDirectories(source))
                DirectoryCopy(dir, dest + Path.GetFileName(dir));
        }

        /// <summary>
        /// ファイルを移動させます。
        /// </summary>
        /// <param name="source">移動元</param>
        /// <param name="dest">移動先</param>
        /// <param name="createDirectory">移動先ディレクトリを作成する場合は<c>true</c>。それ以外の場合は<c>false</c>。</param>
        public static void FileMove(string source, string dest, bool createDirectory)
        {
            if (createDirectory)
                Directory.CreateDirectory(Path.GetDirectoryName(dest));

            if (File.Exists(source))
            {
                if (File.Exists(dest))
                    SafetyDelete(dest);
                File.Move(source, dest);
            }
        }

        /// <summary>
        /// 指定条件を元にファイルを列挙します。
        /// </summary>
        /// <param name="directory">列挙したいディレクトリ</param>
        /// <param name="pattern">
        /// <c>dir</c> 内のファイル名と対応させる検索文字列。<br />
        /// このパラメーターは、<c>2</c>つのピリオド<c>("..")</c>で終了することはできません。<br />
        /// また、<c>2</c>つのピリオド<c>("..")</c>に続けて <see cref="System.IO.Path.DirectorySeparatorChar"/> または <see cref="System.IO.Path.AltDirectorySeparatorChar"/> を指定したり、<br />
        /// <see cref="System.IO.Path.InvalidPathChars"/> の文字を含めたりすることはできません。
        /// </param>
        /// <param name="option">
        /// 検索操作にすべてのサブディレクトリを含めるのか、または現在のディレクトリのみを含めるのかを指定する <see cref="System.IO.SearchOption"/> 値の 1 つ。
        /// </param>
        /// <returns>検索結果の反復子</returns>
        public static IEnumerable<FileSystemInfo> FindFiles(string directory, string pattern, SearchOption option)
        {
            return FindFiles(new DirectoryInfo(directory),  pattern, option);
        }

        /// <summary>
        /// 指定条件を元にファイルを列挙します。
        /// </summary>
        /// <param name="self">列挙したいディレクトリ</param>
        /// <param name="pattern">
        /// <c>dir</c> 内のファイル名と対応させる検索文字列。<br />
        /// このパラメーターは、<c>2</c>つのピリオド<c>("..")</c>で終了することはできません。<br />
        /// また、<c>2</c>つのピリオド<c>("..")</c>に続けて <see cref="System.IO.Path.DirectorySeparatorChar"/> または <see cref="System.IO.Path.AltDirectorySeparatorChar"/> を指定したり、<br />
        /// <see cref="System.IO.Path.InvalidPathChars"/> の文字を含めたりすることはできません。
        /// </param>
        /// <param name="option">
        /// 検索操作にすべてのサブディレクトリを含めるのか、または現在のディレクトリのみを含めるのかを指定する <see cref="System.IO.SearchOption"/> 値の 1 つ。
        /// </param>
        /// <returns>検索結果の反復子</returns>
        public static IEnumerable<FileSystemInfo> FindFiles(this DirectoryInfo self, string pattern, SearchOption option)
        {
            if (self == null)
                throw new ArgumentException("self was null.");

            if (!self.Exists)
                throw new ArgumentException("self was not exists.");

            var list = new List<FileSystemInfo>();
            GetFilesFromDirectories(self.FullName, pattern, option, list);

            return list;
        }

        /// <summary>
        /// ディレクトリが空か判定します。
        /// </summary>
        /// <param name="directory">空か判定したいディレクトリ</param>
        /// <returns>ディレクトリが空の場合は<c>true</c>。それ以外の場合は<c>false</c>。</returns>
        public static bool IsEmpty(string directory)
        {
            try
            {
                return IsEmpty(new DirectoryInfo(directory));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ディレクトリが空か判定します。
        /// </summary>
        /// <param name="directory">空か判定したいディレクトリ</param>
        /// <returns>ディレクトリが空の場合は<c>true</c>。それ以外の場合は<c>false</c>。</returns>
        public static bool IsEmpty(this DirectoryInfo self)
        {
            if (self == null)
                throw new ArgumentException("self was null.");

            if (!self.Exists)
                return false;

            try
            {
                return (Directory.GetFileSystemEntries(self.FullName).Length == 0);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 対象ディレクトリをExceptionを発生させずに削除します。
        /// </summary>
        /// <param name="source">削除したいファイル<c>or</c>ディレクトリのパス</param>
        /// <returns>削除できた場合は<c>true</c> それ以外の場合は<c>false</c></returns>
        public static bool SafetyDelete(string source)
        {
            try
            {
                if (File.Exists(source))
                    return SafetyDelete(new FileInfo(source));
                else if (Directory.Exists(source))
                    return SafetyDelete(new DirectoryInfo(source));
                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 対象ファイル<c>or</c>ディレクトリを<c>Exception</c>を発生させずに削除します。
        /// </summary>
        /// <param name="self">削除したいファイル<c>or</c>ディレクトリ</param>
        /// <returns>削除できた場合は<c>true</c> それ以外の場合は<c>false</c></returns>
        public static bool SafetyDelete(this FileSystemInfo self)
        {
            if (self == null)
                throw new ArgumentException("self was null.");

            if (!self.Exists)
                return true;

            try
            {
                self.Delete();
                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 対象ファイルを<c>Base64</c>文字列に変換します。
        /// </summary>
        /// <param name="self"><c>Base64</c>文字列に変換したいファイル</param>
        /// <returns>対象ファイルを変換した<c>Base64</c>文字列</returns>
        public static string ToBase64(this FileInfo self)
        {
            if (self == null)
                throw new ArgumentException("self was null.");

            if (!self.Exists)
                throw new ArgumentException("self was not exists.");

            // ファイルを開く
            using (var stream = new FileStream(self.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                // バイナリーを読み込む
                var bs = new byte[stream.Length];
                int readBytes = stream.Read(bs, 0, (int)stream.Length);

                // Base64で文字列に変換
                return Convert.ToBase64String(bs);
            }
        }

        /// <summary>
        /// <c>Base64</c>文字列化したファイルを復元します。
        /// </summary>
        /// <param name="base64">ファイルの<c>Base64</c></param>
        /// <param name="savePath">復元ファイルの保存パス</param>
        public static void FromBase64(string base64, string savePath)
        {
            if (string.IsNullOrEmpty(base64))
                throw new ArgumentException("base64 was null or empty.");

            if (string.IsNullOrEmpty(savePath))
                throw new ArgumentException("savePath was null or empty.");

            // バイト型配列に戻す
            byte[] bs = Convert.FromBase64String(base64);

            // ファイルを開く
            using (var stream = new FileStream(savePath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                stream.Write(bs, 0, bs.Length);
        }

        /// <summary>
        /// ファイルのMD5を取得します。
        /// </summary>
        /// <param name="self">MD5を取得したいファイル</param>
        /// <returns>対象ファイルのMD5</returns>
        public static string MD5(this FileInfo self)
        {
            return Hash<MD5CryptoServiceProvider>(self);
        }

        /// <summary>
        /// ファイルのSHA1を取得します。
        /// </summary>
        /// <param name="self">SHA1を取得したいファイル</param>
        /// <returns>対象ファイルのSHA1</returns>
        public static string SHA1(this FileInfo self)
        {
            return Hash<SHA1CryptoServiceProvider>(self);
        }

        /// <summary>
        /// ファイルのSHA512を取得します。
        /// </summary>
        /// <param name="self">SHA512を取得したいファイル</param>
        /// <returns>対象ファイルのSHA512</returns>
        public static string SHA512(this FileInfo self)
        {
            return Hash<SHA512CryptoServiceProvider>(self);
        }

        /// <summary>
        /// ファイルのHashを取得します。
        /// </summary>
        /// <typeparam name="T">Hashを計算するアルゴリズム</typeparam>
        /// <param name="self">Hashを取得したいファイル</param>
        /// <returns>対象ファイルのHash</returns>
        public static string Hash<T>(this FileInfo self) where T : HashAlgorithm
        {
            if (self == null || !self.Exists)
                throw new ArgumentException("self was null or not exists.");

            using (var hash = Activator.CreateInstance(typeof(T)) as HashAlgorithm)
                using (var fs = new FileStream(self.FullName,FileMode.Open))
                    return BitConverter.ToString(hash.ComputeHash(fs));
        }

        /// <summary>
        /// 対象ファイルのSHA1が一致するか判定します。
        /// </summary>
        /// <typeparam name="T">Hashを計算するアルゴリズム</typeparam>
        /// <param name="source">比較先ファイル</param>
        /// <param name="dest">比較元ファイル</param>
        /// <returns>空文字の場合は<c>true</c> それ以外の場合は<c>false</c></returns>
        public static bool CompareHash<T>(this FileInfo source, FileInfo dest) where T : HashAlgorithm
        {
            if (source == null || !source.Exists)
                throw new ArgumentException("source was null or not exists.");

            if (dest == null || !dest.Exists)
                throw new ArgumentException("dest was null or not exists.");

            using (var hash = Activator.CreateInstance(typeof(T)) as HashAlgorithm)
            {
                using (FileStream a = new FileStream(source.FullName, FileMode.Open), b = new FileStream(dest.FullName, FileMode.Open))
                {
                    byte[] hashA = hash.ComputeHash(a);
                    byte[] hashB = hash.ComputeHash(b);

                    return BitConverter.ToString(hashA) == BitConverter.ToString(hashB);
                }
            }
        }

        /// <summary>
        /// 対象ファイルのMD5が一致するか判定します。
        /// </summary>
        /// <param name="source">比較先ファイル</param>
        /// <param name="dest">比較元ファイル</param>
        /// <returns>空文字の場合は<c>true</c> それ以外の場合は<c>false</c></returns>
        public static bool CompareMD5(this string source, string dest)
        {
            return CompareMD5(new FileInfo(source), new FileInfo(dest));
        }

        /// <summary>
        /// 対象ファイルのMD5が一致するか判定します。
        /// </summary>
        /// <param name="source">比較先ファイル</param>
        /// <param name="dest">比較元ファイル</param>
        /// <returns>空文字の場合は<c>true</c> それ以外の場合は<c>false</c></returns>
        public static bool CompareMD5(this FileInfo source, FileInfo dest)
        {
            return CompareHash<MD5CryptoServiceProvider>(source, dest);
        }

        /// <summary>
        /// 対象ファイルのSHA1が一致するか判定します。
        /// </summary>
        /// <param name="source">比較先ファイル</param>
        /// <param name="dest">比較元ファイル</param>
        /// <returns>空文字の場合は<c>true</c> それ以外の場合は<c>false</c></returns>
        public static bool CompareSHA1(this string source, string dest)
        {
            return CompareSHA1(new FileInfo(source), new FileInfo(dest));
        }

        /// <summary>
        /// 対象ファイルのSHA1が一致するか判定します。
        /// </summary>
        /// <param name="source">比較先ファイル</param>
        /// <param name="dest">比較元ファイル</param>
        /// <returns>空文字の場合は<c>true</c> それ以外の場合は<c>false</c></returns>
        public static bool CompareSHA1(this FileInfo source, FileInfo dest)
        {
            return CompareHash<SHA1CryptoServiceProvider>(source, dest);
        }

        /// <summary>
        /// 対象ファイルのSHA512が一致するか判定します。
        /// </summary>
        /// <param name="source">比較先ファイル</param>
        /// <param name="dest">比較元ファイル</param>
        /// <returns>空文字の場合は<c>true</c> それ以外の場合は<c>false</c></returns>
        public static bool CompareSHA512(this string source, string dest)
        {
            return CompareSHA512(new FileInfo(source), new FileInfo(dest));
        }

        /// <summary>
        /// 対象ファイルのSHA512が一致するか判定します。
        /// </summary>
        /// <param name="source">比較先ファイル</param>
        /// <param name="dest">比較元ファイル</param>
        /// <returns>空文字の場合は<c>true</c> それ以外の場合は<c>false</c></returns>
        public static bool CompareSHA512(this FileInfo source, FileInfo dest)
        {
            return CompareHash<SHA512CryptoServiceProvider>(source, dest);
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        /// <summary>
        /// 指定したディレクトリ内の指定した検索パターンに一致するファイル名を返します。
        /// </summary>
        /// <param name="dir">検索するディレクトリ</param>
        /// <param name="filter">
        /// <c>dir</c> 内のファイル名と対応させる検索文字列。<br />
        /// このパラメーターは、<c>2</c>つのピリオド<c>("..")</c>で終了することはできません。<br />
        /// また、<c>2</c>つのピリオド<c>("..")</c>に続けて <see cref="System.IO.Path.DirectorySeparatorChar"/> または <see cref="System.IO.Path.AltDirectorySeparatorChar"/> を指定したり、<br />
        /// <see cref="System.IO.Path.InvalidPathChars"/> の文字を含めたりすることはできません。
        /// </param>
        /// <param name="option">
        /// 検索操作にすべてのサブディレクトリを含めるのか、または現在のディレクトリのみを含めるのかを指定する <see cref="System.IO.SearchOption"/> 値の 1 つ。
        /// </param>
        /// <param name="queue">検索結果を格納する可変長配列</param>
        private static void GetFilesFromDirectories(string dir, string filter, SearchOption option, List<FileSystemInfo> queue)
        {
            foreach (string file in Directory.GetFiles(dir, filter))
            {
                try
                {
                    queue.Add(new FileInfo(file));
                }
                catch (UnauthorizedAccessException)
                {
                }
            }

            if (option == SearchOption.AllDirectories)
            {
                foreach (string directory in Directory.GetDirectories(dir))
                {
                    try
                    {
                        GetFilesFromDirectories(directory, filter, option, queue);
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                }
            }
        }

        #endregion

        #region delegate

        #endregion
    }
}
