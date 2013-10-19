using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ricordanza.Core.Comparer
{
    /// <summary>
    /// 文字列比較クラスです。
    /// </summary>
    /// <remarks>
    /// 数値を意識した文字列並べかえを行います。<br/>
    /// 以下を並べ替えると<br/>
    /// <c>
    /// a10<br/>
    /// a20<br/>
    /// a1<br/>
    /// a2<br/>
    /// </c>
    /// このように並びます。<br/>
    /// <c>
    /// a1<br/>
    /// a2<br/>
    /// a10<br/>
    /// a20<br/>
    /// </c>
    /// </remarks>
    public class StringNumberComparer
        : IComparer<string>
    {
        #region const

        /// <summary>
        /// 数値検出用正規表現
        /// </summary>
        private static Regex NUMBER_REGEX = new Regex(@"^(.*?)([0-9]+).*?$", RegexOptions.Compiled);

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public StringNumberComparer()
            : base()
        {
            NumberCheck = true;
        }

        #endregion

        #region property

        /// <summary>
        /// 数値チェック
        /// </summary>
        public bool NumberCheck { set; get; }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// 2つのオブジェクトを比較し、一方が他方より小さいか、等しいか、大きいかを示す値を返します。
        /// </summary>
        /// <param name="x">比較する最初のオブジェクトです</param>
        /// <param name="y">比較する 2 番目のオブジェクト</param>
        /// <returns>
        /// x と y の相対的な値を示す符号付き整数。<br />
        /// </returns>
        public int Compare(string x, string y)
        {
            string aorg = x;
            string borg = y;

            // 何もしなくても等しかったら0
            if (x == y)
                return 0;

            // 数字部分切り出し保存用
            int? ai = null;
            int? bi = null;

            // 数字チェックするなら
            if (NumberCheck)
            {
                // 正規表現で切り出す
                Match matchCol = NUMBER_REGEX.Match(x);

                // マッチしたら
                if (matchCol.Success)
                {
                    // 数字の前までの文字列と
                    x = matchCol.Groups[1].Value;
                    // 数字に分ける
                    ai = Convert.ToInt32(matchCol.Groups[2].Value);
                }

                // 正規表現
                matchCol = NUMBER_REGEX.Match(y);
                // マッチ
                if (matchCol.Success)
                {
                    // 文字列
                    y = matchCol.Groups[1].Value;
                    // 数字
                    bi = Convert.ToInt32(matchCol.Groups[2].Value);
                }
            }

            // 文字列の比較
            int t = string.Compare(x, y);

            // 等しければ
            if (NumberCheck && t == 0)
            {
                if (ai == null && bi != null)
                    t = -1;
                else if (ai != null && bi == null)
                    t = 1;
                else if (ai == null && bi == null)
                    t = string.Compare(aorg, borg);
                else
                {
                    t = (int)(ai - bi);
                    if (t == 0)
                        t = string.Compare(aorg, borg);
                }
            }

            return t;
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
