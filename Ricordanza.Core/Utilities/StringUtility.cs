using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Ricordanza.Core.Utilities
{
    /// <summary>
    /// 文字列操作のユーティリティクラスです。
    /// </summary>
    /// <remarks>このクラスは<see cref="System.String"/>を拡張します。</remarks>
    public static class StringUtility
    {
        #region const

        /// <summary>
        /// 半角スペース、全角スペース、タブ文字削除用の正規表現
        /// </summary>
        private static readonly Regex WHITESPACE_REGEX = new Regex(@"[ 　\t]+");

        /// <summary>
        /// 全角スペース、タブ文字削除用の正規表現
        /// </summary>
        private static readonly Regex FULLWHITESPACE_REGEX = new Regex(@"[　\t]+");

        #endregion

        #region public method

        #region type check

        /// <summary>
        /// 文字列が数値であるかどうかを返します。
        /// </summary>
        /// <param name="self">検査対象となる文字列。</param>
        /// <returns>true - 数値 false - 数値ではない</returns>
        /// <remarks>
        /// このメソッドは<c>decimal</c>型を用いて値を検証します。
        /// 浮動小数を含む値であったとしても適切であれば<c>true</c>を返却します。
        /// </remarks>
        public static bool IsNumeric(this string self)
        {
            decimal? result = ToNullable<decimal>(self);
            return result != null;
        }

        /// <summary>
        /// 文字列が日付型であるかどうかを返します。
        /// </summary>
        /// <param name="value">検査対象となる文字列。<param>
        /// <returns>true - 日付型 false - 日付型ではない</returns>
        /// <remarks>
        /// このメソッドは<c>DateTime</c>型を用いて値を検証します。
        /// 浮動小数を含む値であったとしても適切であれば<c>true</c>を返却します。
        /// </remarks>
        public static bool IsDateTime(this string value)
        {
            DateTime? result = ToNullable<DateTime>(value);

            if (!result.HasValue)
                return false;

            if (result.Value < System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                return false;

            return true;
        }

        #endregion

        #region cast

        /// <summary>
        /// 対象の文字列をint型に変換します。
        /// </summary>
        /// <param name="self">変換したい文字列</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>0</c>を返却します。
        /// </remarks>
        public static int ToInt(this string self)
        {
            return ToInt(self, 0);
        }

        /// <summary>
        /// 対象の文字列をint型に変換します。
        /// </summary>
        /// <param name="self">変換したい文字列</param>
        /// <param name="defaultValue">変換不可能時のデフォルト値</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>defaultValue</c>を返却します。
        /// </remarks>
        public static int ToInt(this string self, int defaultValue)
        {
            int? result = ToNullable<int>(self);
            return result ?? defaultValue;
        }

        /// <summary>
        /// 対象の文字列をdecimal型に変換します。
        /// </summary>
        /// <param name="self">変換したい文字列</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>0</c>を返却します。
        /// </remarks>
        public static decimal ToDecimal(this string self)
        {
            return ToDecimal(self, 0);
        }

        /// <summary>
        /// 対象の文字列をdecimal型に変換します。
        /// </summary>
        /// <param name="self">変換したい文字列</param>
        /// <param name="defaultValue">変換不可能時のデフォルト値</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>defaultValue</c>を返却します。
        /// </remarks>
        public static decimal ToDecimal(this string self, decimal defaultValue)
        {
            decimal? result = ToNullable<decimal>(self);
            return result ?? defaultValue;
        }

        /// <summary>
        /// 対象の文字列をbool型に変換します。
        /// </summary>
        /// <param name="self">変換したい文字列</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>false</c>を返却します。
        /// </remarks>
        public static bool ToBoolean(this string self)
        {
            return ToBoolean(self, false);
        }

        /// <summary>
        /// 対象の文字列をbool型に変換します。
        /// </summary>
        /// <param name="self">変換したい文字列</param>
        /// <param name="defaultValue">変換不可能時のデフォルト値</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>defaultValue</c>を返却します。
        /// </remarks>
        public static bool ToBoolean(this string self, bool defaultValue)
        {
            bool? result = ToNullable<bool>(self);
            return result ?? defaultValue;
        }

        /// <summary>
        /// 対象の文字列をDateTime型に変換します。
        /// </summary>
        /// <param name="self">変換したい文字列</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>DateTime.Now</c>を返却します。
        /// </remarks>
        public static DateTime ToDateTime(this string self)
        {
            return ToDateTime(self, DateTime.Now);
        }

        /// <summary>
        /// 対象の文字列をDateTime型に変換します。
        /// </summary>
        /// <param name="value">変換したい文字列</param>
        /// <param name="defaultValue">変換不可能時のデフォルト値</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// <c>value</c>が<c>null</c>又は空文字の場合か変換不可能な場合は<c>defaultValue</c>を返却します。
        /// </remarks>
        public static DateTime ToDateTime(this string value, DateTime defaultValue)
        {
            DateTime? result = ToNullable<DateTime>(value);
            return result ?? defaultValue;
        }

        /// <summary>
        /// 文字列が指定した型のTryParseに成功した場合はその値を、失敗した場合はnullを返却します。
        /// </summary>
        /// <typeparam name="T">変換対象の型</typeparam>
        /// <param name="self">変換したい値</param>
        /// <returns>変換後の値</returns>
        /// <remarks>
        /// TryParseをサポートしない場合は、NotSupportedException例外を発生させます。
        /// </remarks>
        public static Nullable<T> ToNullable<T>(this string self) where T : struct
        {
            if (string.IsNullOrEmpty(self))
                return null;

            var type = typeof(T);
            var types = new[] { typeof(string), GetReferenceType(type) };
            var method = type.GetMethod("TryParse", types);

            if (method == null)
                throw new NotSupportedException(string.Format("TryParse Not Supported Type[{0}]", type.FullName));

            var parameters = new object[] { self, new T() };
            if ((bool)method.Invoke(null, parameters))
                return (T)parameters[1];
            else
                return null;
        }

        #endregion

        #region operation

        /// <summary>
        /// 先頭の一文字目を大文字に変換して取得します。
        /// </summary>
        /// <param name="self">先頭の一文字目を大文字化したい文字列</param>
        /// <returns>先頭の一文字目を大文字化したtarget</returns>
        /// <remarks>
        /// <c>value</c>が空文字か<c>null</c>の場合は<c>value</c>をそのまま取得します。
        /// </remarks>
        public static string ToUpperBegin(this string self)
        {
            // 空文字かnullの場合はそのまま返却
            if (string.IsNullOrEmpty(self))
                return self;

            return char.ToUpper(self[0]) + self.Substring(1);
        }

        /// <summary>
        /// 先頭の一文字目を小文字に変換して取得します。
        /// </summary>
        /// <param name="self">先頭の一文字目を小文字化したい文字列</param>
        /// <returns>先頭の一文字目を小文字化したtarget</returns>
        /// <remarks>
        /// <c>value</c>が空文字か<c>null</c>の場合は<c>value</c>をそのまま取得します。
        /// </remarks>
        public static string ToLowerBegin(this string self)
        {
            // 空文字かnullの場合はそのまま返却
            if (string.IsNullOrEmpty(self))
                return self;

            return char.ToLower(self[0]) + self.Substring(1);
        }

        /// <summary>
        /// 文字の先端に<c>start</c>を付加します。
        /// </summary>
        /// <param name="self"><c>start</c>を先端に付加したい文字</param>
        /// <returns>付加結果</returns>
        public static string StartWith(this string self, string start)
        {
            if (string.IsNullOrEmpty(self))
                return start;

            if (!self.StartsWith(start))
                return start + self;

            return self;
        }

        /// <summary>
        /// 文字の終端に<c>end</c>を付加します。
        /// </summary>
        /// <param name="self"><c>end</c>を終端に付加したい文字</param>
        /// <returns>付加結果</returns>
        public static string EndWith(this string self, string end)
        {
            if (string.IsNullOrEmpty(self))
                return end;

            if (!self.EndsWith(end))
                return self + end;

            return self;
        }

        /// <summary>
        /// 指定された文字が文字列の先端に存在する場合は除去します。
        /// </summary>
        /// <param name="self">検証文字列</param>
        /// <param name="end">削除したい文字列</param>
        /// <returns>処理結果</returns>
        public static string RemoveStartWith(this string self, string start)
        {
            if (string.IsNullOrEmpty(self))
                return string.Empty;

            // 最後の改行文字を削除
            string ret = self;
            if (ret.StartsWith(start))
                ret = ret.Remove(0, start.Length);

            return ret;
        }

        /// <summary>
        /// 指定された文字が文字列の終端に存在する場合は除去します。
        /// </summary>
        /// <param name="self">検証文字列</param>
        /// <param name="end">削除したい文字列</param>
        /// <returns>処理結果</returns>
        public static string RemoveEndWith(this string self, string end)
        {
            if (string.IsNullOrEmpty(self))
                return string.Empty;

            // 最後の改行文字を削除
            string ret = self;
            if (ret.EndsWith(end))
                ret = ret.Remove(ret.LastIndexOf(end), end.Length);

            return ret;
        }

        /// <summary>
        /// 文字列から全てのスペース、タブを削除します。
        /// </summary>
        /// <param name="self">スペース、タブを除去したい文字列</param>
        public static string ClearWhiteSpace(this string self)
        {
            if (string.IsNullOrEmpty(self))
                return string.Empty;

            return WHITESPACE_REGEX.Replace(self, string.Empty);
        }

        /// <summary>
        /// 書式の項目を指定したオブジェトに置換します。
        /// </summary>
        /// <param name="self"><c>0</c>個以上の書式を含んだ文字列</param>
        /// <param name="args">書式設定する値</param>
        /// <returns>置換された値</returns>
        public static string DirectFormat(this string self, params object[] args)
        {
            return string.Format(self, args);
        }

        /// <summary>
        /// 文字列の左端から指定された文字数分の文字列を返します。
        /// </summary>
        /// <param name="self">取り出す元になる文字列</param>
        /// <param name="count">取り出す文字数</param>
        /// <returns>左端から指定された文字数分の文字列</returns>
        /// <remarks> 文字数を超えた場合は、文字列全体を返却</remarks>
        public static string Left(this string self, int count)
        {
            if (string.IsNullOrEmpty(self))
                return string.Empty;

            if (count <= self.Length)
                return self.Substring(0, count);

            return self;
        }

        /// <summary>
        /// 文字列の指定された位置以降のすべての文字列を返します。
        /// </summary>
        /// <param name="self">取り出す元になる文字列</param>
        /// <param name="start">取り出しを開始する位置</param>
        /// <returns>指定された位置以降のすべての文字列を返却</returns>
        public static string Mid(this string self, int start)
        {
            if (string.IsNullOrEmpty(self))
                return string.Empty;

            if (start <= self.Length)
                return self.Substring(start - 1);

            return string.Empty;
        }

        /// <summary>
        /// 文字列の指定された位置から、指定された文字数分の文字列を返します。
        /// </summary>
        /// <param name="self">取り出す元になる文字列</param>
        /// <param name="start">取り出しを開始する位置</param>
        /// <param name="count">取り出す文字数</param>
        /// <returns>指定された位置から指定された文字数分の文字列</returns>
        /// <remarks>文字数を超えた場合は、指定された位置からすべての文字列を返却</remarks>
        public static string Mid(this string self, int start, int count)
        {
            if (string.IsNullOrEmpty(self))
                return string.Empty;

            if (start <= self.Length)
            {
                if (start + count - 1 <= self.Length)
                    return self.Substring(start - 1, count);

                return self.Substring(start - 1);
            }

            return string.Empty;
        }

        /// <summary>
        /// 文字列の右端から指定された文字数分の文字列を返します。
        /// </summary>
        /// <param name="self">取り出す元になる文字列</param>
        /// <param name="count">取り出す文字数</param>
        /// <returns>右端から指定された文字数分の文字列</returns>
        /// <remarks>文字数を超えた場合は、文字列全体を返却</remarks>
        public static string Right(this string self, int count)
        {
            if (string.IsNullOrEmpty(self))
                return string.Empty;

            if (count <= self.Length)
                return self.Substring(self.Length - count);

            return self;
        }

        /// <summary>
        /// 最初に発見した区切り文字より左にある文字列を取得します。
        /// </summary>
        /// <param name="self">取り出す元になる文字列</param>
        /// <param name="delim">区切り文字</param>
        /// <returns>切り出した文字列</returns>
        public static string Left(this string self, char delim)
        {
            if (string.IsNullOrEmpty(self))
                return string.Empty;

            foreach (string v in self.Split(delim))
                return v;

            return self;
        }

        /// <summary>
        /// 最後に発見した区切り文字より右にある文字列を取得します。
        /// </summary>
        /// <param name="self">取り出す元になる文字列</param>
        /// <param name="delim">区切り文字</param>
        /// <returns>切り出した文字列</returns>
        public static string Right(this string self, char delim)
        {
            if (string.IsNullOrEmpty(self))
                return string.Empty;

            var split = self.Split(delim);
            foreach (string v in split)
                return split[split.Length - 1];

            return self;
        }

        #endregion

        #region empty check

        /// <summary>
        /// 空文字か判定します。
        /// </summary>
        /// <param name="value"><see cref="System.string"/> 参照</param>
        /// <returns>空文字の場合は<c>true</c> それ以外の場合は<c>false</c></returns>
        public static bool IsEmpty(dynamic value)
        {
            if (value == null)
                return true;

            return IsEmpty(value.ToString());
        }

        /// <summary>
        /// 空文字か判定します。
        /// </summary>
        /// <param name="self"><see cref="System.string"/> 参照</param>
        /// <returns>空文字の場合は<c>true</c> それ以外の場合は<c>false</c></returns>
        public static bool IsEmpty(this string self)
        {
            if (self == null)
                return true;

            return string.IsNullOrEmpty(self.Trim());
        }

        #endregion

        #region has ...

        /// <summary>
        /// 半角スペース、全角スペース、タブを含む文字列か判定します。
        /// </summary>
        /// <param name="self"><see cref="System.string"/> 参照</param>
        /// <returns>スペース、タブを含む場合は<c>true</c> それ以外の場合は<c>false</c></returns>
        public static bool HasWhiteSpace(this string self)
        {
            if (string.IsNullOrEmpty(self))
                return false;

            return WHITESPACE_REGEX.Match(self).Success;
        }

        /// <summary>
        /// 全角スペース、タブを含む文字列か判定します。
        /// </summary>
        /// <param name="self"><see cref="System.string"/> 参照</param>
        /// <returns>スペース、タブを含む場合は<c>true</c> それ以外の場合は<c>false</c></returns>
        public static bool HasFullWhiteSpace(this string self)
        {
            if (string.IsNullOrEmpty(self))
                return false;

            return FULLWHITESPACE_REGEX.Match(self).Success;
        }

        #endregion

        #region bytes

        /// <summary>
        /// <c>Shift_JIS</c>でのバイト数を取得します。
        /// </summary>
        /// <param name="self">バイト数を取得したい文字列</param>
        /// <returns><c>Shift_JIS</c>でのバイト数</returns>
        public static int SjisByte(this string self)
        {
            if (string.IsNullOrEmpty(self))
                return 0;

            return Encoding.GetEncoding("Shift_JIS").GetByteCount(self);
        }

        /// <summary>
        /// <c>UTF-8</c>でのバイト数を取得します。
        /// </summary>
        /// <param name="self">バイト数を取得したい文字列</param>
        /// <returns><c>UTF-8</c>でのバイト数</returns>
        public static int UTF8Byte(this string self)
        {
            if (string.IsNullOrEmpty(self))
                return 0;

            return Encoding.UTF8.GetByteCount(self);
        }

        #endregion

        #region escape

        /// <summary>
        /// 文字列内の無効な<c>XML</c>文字を等価の有効な<c>XML</c>に置き換えます。
        /// </summary>
        /// <param name="self">エスケープする無効な文字を含む文字列</param>
        /// <returns>置き換えられた無効な文字を含む入力文字列</returns>
        public static string EscapeXml(this string self)
        {
            if (string.IsNullOrEmpty(self))
                return string.Empty;

            return SecurityElement.Escape(self);
        }

        /// <summary>
        /// OracleのLike検索用のスケープ処理を行います。
        /// </summary>
        /// <param name="self">エスケープしたい文字列</param>
        /// <param name="partial">true - 部分一致 false - 後方一致</param>
        /// <returns>変換後の文字列</returns>
        public static string EscapeOracleLikeParam(this string self, bool partial)
        {
            string val = Regex.Replace(self, @"([\\%_％＿])", string.Concat(@"\", "$1"));
            if (partial)
                val = "%" + val;

            return (val + "%");
        }

        /// <summary>
        ///  DataTableの<c>=</c>検索句に合わせたエスケープ処理を実装します。
        /// </summary>
        /// <param name="self">エスケープしたい文字列</param>
        /// <returns>変換後の文字列</returns>
        public static string EscapeDataTableParam(this string self)
        {
            if (string.IsNullOrEmpty(self))
                return string.Empty;

            return self.Replace("'", "''");
        }

        /// <summary>
        /// DataTableのLike検索用のスケープ処理を行います。
        /// </summary>
        /// <param name="self">エスケープしたい文字列</param>
        /// <param name="partial">true - 部分一致 false - 後方一致</param>
        /// <returns>変換後の文字列</returns>
        public static string EscapeDataTableLikeParam(this string self, bool partial)
        {
            string val = Regex.Replace(EscapeDataTableParam(self), @"([\[\]*%_])", "[$1]");
            if (partial)
                val = "%" + val;

            return (val + "%");
        }

        #endregion

        #region hash

        /// <summary>
        /// 文字列のMD5を取得します。
        /// </summary>
        /// <param name="self">MD5を取得したい文字列</param>
        /// <returns>対象の文字列を<c>UTF-8</c>エンコードでバイト配列化して計算したMD5</returns>
        public static string MD5(this string self)
        {
            return Hash<MD5CryptoServiceProvider>(self);
        }

        /// <summary>
        /// 文字列のSHA1を取得します。
        /// </summary>
        /// <param name="self">SHA1を取得したい文字列</param>
        /// <returns>対象の文字列を<c>UTF-8</c>エンコードでバイト配列化して計算したSHA1</returns>
        public static string SHA1(this string self)
        {
            return Hash<SHA1CryptoServiceProvider>(self);
        }

        /// <summary>
        /// 文字列のSHA512を取得します。
        /// </summary>
        /// <param name="self">SHA512を取得したい文字列</param>
        /// <returns>対象の文字列を<c>UTF-8</c>エンコードでバイト配列化して計算したSHA512</returns>
        public static string SHA512(this string self)
        {
            return Hash<SHA512CryptoServiceProvider>(self);
        }

        /// <summary>
        /// 文字列のHashを取得します。
        /// </summary>
        /// <typeparam name="T">Hashを計算するアルゴリズム</typeparam>
        /// <param name="self">Hashを取得したい文字列</param>
        /// <returns>対象の文字列を<c>UTF-8</c>エンコードでバイト配列化して計算したHash</returns>
        public static string Hash<T>(this string self) where T : HashAlgorithm
        {
            if (string.IsNullOrEmpty(self))
                return string.Empty;

            using (var hash = Activator.CreateInstance(typeof(T)) as HashAlgorithm)
                return BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(self)));
        }

        #endregion

        #endregion

        #region private method

        /// <summary>
        /// 型情報を取得します。
        /// </summary>
        /// <typeparam name="T">情報を取得したい型</typeparam>
        /// <returns>型情報</returns>
        private static Type GetReferenceType<T>()
        {
            return GetReferenceType(typeof(T));
        }

        /// <summary>
        /// 型情報を取得します。
        /// </summary>
        /// <typeparam name="T">情報を取得したい型</typeparam>
        /// <returns>型情報</returns>
        private static Type GetReferenceType(Type type)
        {
            return Type.GetType(type.FullName + "&");
        }

        #endregion
    }
}
