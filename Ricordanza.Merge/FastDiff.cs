using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Ricordanza.Merge
{
    #region DiffOption

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public struct DiffOption{ public bool TrimSpace, IgnoreSpace, IgnoreCase; }

    #endregion

    #region DiffResult

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DiffResult
    {
        #region const

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modified"></param>
        /// <param name="orgStart"></param>
        /// <param name="orgLength"></param>
        /// <param name="modStart"></param>
        /// <param name="modLength"></param>
        public DiffResult(bool modified, int orgStart, int orgLength, int modStart, int modLength)
        {
            this.Modified = modified;
            this.OriginalStart = orgStart;
            this.OriginalLength = orgLength;
            this.ModifiedStart = modStart;
            this.ModifiedLength = modLength;
        }

        #endregion

        #region property

        /// <summary>
        /// 変更あり?
        /// </summary>
        public bool Modified { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int OriginalStart { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int OriginalLength { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int ModifiedStart { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int ModifiedLength { set; get; }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ((this.Modified) ? "Modified" : "Common")
              + ", OrgStart:" + this.OriginalStart + ", OrgLen:" + this.OriginalLength
              + ", ModStart:" + this.ModifiedStart + ", ModLen:" + this.ModifiedLength;
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

    #region FastDiff

    /// <summary>
    /// 比較処理を行います。
    /// </summary>
    /// <example>
    /// ■行単位diff
    /// <code>
    /// public static DiffResult[] Diff( string textA, string textB )
    /// public static DiffResult[] Diff( string textA, string textB, DiffOption option )
    /// </code>
    /// ■文字単位diff
    /// <code>
    /// public static DiffResult[] DiffChar( string textA, string textB )
    /// public static DiffResult[] DiffChar( string textA, string textB, DiffOption option )
    /// </code>
    /// </example>
    public class FastDiff
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// 
        /// </summary>
        private int[] dataA, dataB;

        /// <summary>
        /// 
        /// </summary>
        private string[] linesA, linesB;

        /// <summary>
        /// 
        /// </summary>
        private bool isSwap;

        /// <summary>
        /// 
        /// </summary>
        private Snake[] fp;

        /// <summary>
        /// 
        /// </summary>
        private IsSame isSame;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// 
        /// </summary>
        private FastDiff() { }

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

        /// <summary>複数行の文字列を行単位で比較します</summary>
        /// <param name="textA">元テキスト</param>
        /// <param name="textB">変更テキスト</param>
        /// <returns>比較結果</returns>
        public static DiffResult[] Diff(string textA, string textB)
        {
            DiffOption option = new DiffOption();
            return Diff(textA, textB, option);
        }

        /// <summary>複数行の文字列を行単位で比較します</summary>
        /// <param name="textA">元テキスト</param>
        /// <param name="textB">変更テキスト</param>
        /// <param name="option">オプション指定</param>
        /// <returns>比較結果</returns>
        public static DiffResult[] Diff(string textA, string textB, DiffOption option)
        {
            if (string.IsNullOrEmpty(textA) || string.IsNullOrEmpty(textB))
                return StringNullOrEmpty(textA, textB);

            FastDiff diff = new FastDiff();
            return diff.DiffCore(textA, textB, option);
        }

        /// <summary>単一行の各文字を比較します</summary>
        /// <param name="textA">元テキスト</param>
        /// <param name="textB">変更テキスト</param>
        /// <returns>比較結果</returns>
        public static DiffResult[] DiffChar(string textA, string textB)
        {
            DiffOption option = new DiffOption();
            return DiffChar(textA, textB, option);
        }

        /// <summary>単一行の各文字を比較します</summary>
        /// <param name="textA">元テキスト</param>
        /// <param name="textB">変更テキスト</param>
        /// <param name="option">オプション指定</param>
        /// <returns>比較結果</returns>
        public static DiffResult[] DiffChar(string textA, string textB, DiffOption option)
        {
            if (string.IsNullOrEmpty(textA) || string.IsNullOrEmpty(textB))
                return StringNullOrEmpty(textA, textB);

            FastDiff diff = new FastDiff();
            if (textA.Length <= textB.Length)
            {
                diff.SplitChar(textA, textB, option);
            }
            else
            {
                diff.isSwap = true;
                diff.SplitChar(textB, textA, option);
            }

            diff.isSame = delegate(int posA, int posB)
            {
                return diff.dataA[posA] == diff.dataB[posB];
            };

            return diff.DetectDiff();
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textA"></param>
        /// <param name="textB"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        private DiffResult[] DiffCore(string textA, string textB, DiffOption option)
        {
            this.linesA = SplitLine(textA, option);
            this.linesB = SplitLine(textB, option);

            if (this.linesB.Length < this.linesA.Length)
            {
                this.isSwap = true;

                string[] tmps = this.linesA;
                this.linesA = this.linesB;
                this.linesB = tmps;
            }
            this.dataA = MakeHash(this.linesA);
            this.dataB = MakeHash(this.linesB);

            this.isSame = delegate(int posA, int posB)
            {
                return (this.dataA[posA] == this.dataB[posB]) && (this.linesA[posA] == this.linesB[posB]);
            };

            return DetectDiff();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textA"></param>
        /// <param name="textB"></param>
        /// <returns></returns>
        private static DiffResult[] StringNullOrEmpty(string textA, string textB)
        {
            int lengthA = (string.IsNullOrEmpty(textA)) ? 0 : textA.Length;
            int lengthB = (string.IsNullOrEmpty(textB)) ? 0 : textB.Length;
            return PresentDiff(new CommonSubsequence(lengthA, lengthB, 0, null), true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textA"></param>
        /// <param name="textB"></param>
        /// <param name="option"></param>
        private void SplitChar(string textA, string textB, DiffOption option)
        {
            this.dataA = SplitChar(textA, option);
            this.dataB = SplitChar(textB, option);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        private static int[] SplitChar(string text, DiffOption option)
        {
            if (option.IgnoreCase)
                text = text.ToUpperInvariant();

            // TODO: FIXME! Optimize this
            if (option.IgnoreSpace)
                text = Regex.Replace(text, @"\s+", " ");

            if (option.TrimSpace)
                text = text.Trim();

            int[] result = new int[text.Length];
            for (int i = 0; i < text.Length; i++)
                result[i] = text[i];
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        private static string[] SplitLine(string text, DiffOption option)
        {
            if (option.IgnoreCase)
                text = text.ToUpperInvariant();

            string[] lines = text.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            // TODO: FIXME! Optimize this
            if (option.IgnoreSpace)
                for (int i = 0; i < lines.Length; ++i)
                    lines[i] = Regex.Replace(lines[i], @"\s+", " ");

            if (option.TrimSpace)
                for (int i = 0; i < lines.Length; ++i)
                    lines[i] = lines[i].Trim();

            return lines;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texts"></param>
        /// <returns></returns>
        private static int[] MakeHash(string[] texts)
        {
            int[] hashs = new int[texts.Length];

            for (int i = 0; i < texts.Length; ++i)
                hashs[i] = texts[i].GetHashCode();

            return hashs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DiffResult[] DetectDiff()
        {
            Debug.Assert(this.dataA.Length <= this.dataB.Length);

            this.fp = new Snake[this.dataA.Length + this.dataB.Length + 3];
            int d = this.dataB.Length - this.dataA.Length;
            int p = 0;
            do
            {
                //Debug.Unindent();
                //Debug.WriteLine( "p:" + p );
                //Debug.Indent();

                for (int k = -p; k < d; k++)
                    SearchSnake(k);

                for (int k = d + p; k >= d; k--)
                    SearchSnake(k);

                p++;
            }
            while (this.fp[this.dataB.Length + 1].posB != (this.dataB.Length + 1));

            // 末尾検出用のCommonSubsequence
            CommonSubsequence endCS = new CommonSubsequence(this.dataA.Length, this.dataB.Length, 0, this.fp[this.dataB.Length + 1].CS);
            CommonSubsequence result = CommonSubsequence.Reverse(endCS);

            if (this.isSwap)
                return PresentDiffSwap(result, true);
            else
                return PresentDiff(result, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="k"></param>
        private void SearchSnake(int k)
        {
            int kk = this.dataA.Length + 1 + k;
            CommonSubsequence previousCS = null;
            int posA = 0, posB = 0;

            int lk = kk - 1;
            int rk = kk + 1;

            // 論文のfp[n]は-1始まりだが、0始まりのほうが初期化の都合がよいため、
            // +1のゲタを履かせる。fpから読む際は-1し、書く際は+1する。
            int lb = this.fp[lk].posB;
            int rb = this.fp[rk].posB - 1;

            //Debug.Write( "fp[" + string.Format( "{0,2}", k ) + "]=Snake( " + string.Format( "{0,2}", k )
            //    + ", max( fp[" + string.Format( "{0,2}", ( k - 1 ) ) + "]+1= " + string.Format( "{0,2}", lb )
            //    + ", fp[" + string.Format( "{0,2}", ( k + 1 ) ) + "]= " + string.Format( "{0,2}", rb ) + " ))," );

            if (lb > rb)
            {
                posB = lb;
                previousCS = this.fp[lk].CS;
            }
            else
            {
                posB = rb;
                previousCS = this.fp[rk].CS;
            }
            posA = posB - k;

            int startA = posA;
            int startB = posB;

            //Debug.Write( "(x: " + string.Format( "{0,2}", startA ) + ", y: " + string.Format( "{0,2}", startB ) + " )" );

            while ((posA < this.dataA.Length)
              && (posB < this.dataB.Length)
              && this.isSame(posA, posB))
            {
                posA++;
                posB++;
            }

            if (startA != posA)
            {
                this.fp[kk].CS = new CommonSubsequence(startA, startB, posA - startA, previousCS);
            }
            else
            {
                this.fp[kk].CS = previousCS;
            }
            this.fp[kk].posB = posB + 1; // fpへ+1して書く。論文のfpに+1のゲタを履かせる。
        }

        /// <summary>
        /// 出力結果
        /// </summary>
        /// <param name="cs"></param>
        /// <param name="wantCommon"></param>
        /// <returns></returns>
        private static DiffResult[] PresentDiff(CommonSubsequence cs, bool wantCommon)
        {
            List<DiffResult> list = new List<DiffResult>();
            int originalStart = 0, modifiedStart = 0;

            while (true)
            {
                if (originalStart < cs.StartA
                  || modifiedStart < cs.StartB)
                {
                    DiffResult d = new DiffResult(
                      true,
                      originalStart, cs.StartA - originalStart,
                      modifiedStart, cs.StartB - modifiedStart);
                    list.Add(d);
                }

                // 末尾検出
                if (cs.Length == 0) break;

                originalStart = cs.StartA;
                modifiedStart = cs.StartB;

                if (wantCommon)
                {
                    DiffResult d = new DiffResult(
                      false,
                      originalStart, cs.Length,
                      modifiedStart, cs.Length);
                    list.Add(d);
                }
                originalStart += cs.Length;
                modifiedStart += cs.Length;

                cs = cs.Next;
            }
            return list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cs"></param>
        /// <param name="wantCommon"></param>
        /// <returns></returns>
        private static DiffResult[] PresentDiffSwap(CommonSubsequence cs, bool wantCommon)
        {
            List<DiffResult> list = new List<DiffResult>();
            int originalStart = 0, modifiedStart = 0;

            while (true)
            {
                if (originalStart < cs.StartB
                  || modifiedStart < cs.StartA)
                {
                    DiffResult d = new DiffResult(
                      true,
                      originalStart, cs.StartB - originalStart,
                      modifiedStart, cs.StartA - modifiedStart);
                    list.Add(d);
                }

                // 末尾検出
                if (cs.Length == 0) break;

                originalStart = cs.StartB;
                modifiedStart = cs.StartA;

                if (wantCommon)
                {
                    DiffResult d = new DiffResult(
                      false,
                      originalStart, cs.Length,
                      modifiedStart, cs.Length);
                    list.Add(d);
                }
                originalStart += cs.Length;
                modifiedStart += cs.Length;

                cs = cs.Next;
            }
            return list.ToArray();
        }

        #endregion

        #region delegate

        /// <summary>
        /// 
        /// </summary>
        /// <param name="posA"></param>
        /// <param name="posB"></param>
        /// <returns></returns>
        private delegate bool IsSame(int posA, int posB);

        #endregion

        #region inner class

        #region Snake

        /// <summary>
        /// 
        /// </summary>
        private struct Snake
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
            /// 
            /// </summary>
            public int posB { set; get; }

            /// <summary>
            /// 
            /// </summary>
            public CommonSubsequence CS { set; get; }

            #endregion

            #region event

            #endregion

            #region event handler

            #endregion

            #region event method

            #endregion

            #region public method

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "posB:" + this.posB + ", CS:" + ((this.CS == null) ? "null" : "exist");
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

        #region CommonSubsequence

        /// <summary>
        /// 
        /// </summary>
        private class CommonSubsequence
        {
            #region const

            #endregion

            #region private variable

            /// <summary>
            /// 
            /// </summary>
            private int startA_, startB_;
            
            /// <summary>
            /// 
            /// </summary>
            private int length_;

            #endregion

            #region static constructor

            #endregion

            #region constructor

            /// <summary>
            /// 
            /// </summary>
            public CommonSubsequence() { }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="startA"></param>
            /// <param name="startB"></param>
            /// <param name="length"></param>
            /// <param name="next"></param>
            public CommonSubsequence(int startA, int startB, int length, CommonSubsequence next)
            {
                this.startA_ = startA;
                this.startB_ = startB;
                this.length_ = length;
                this.Next = next;
            }

            #endregion

            #region property

            /// <summary>
            /// 
            /// </summary>
            public CommonSubsequence Next { set; get; }

            /// <summary>
            /// 
            /// </summary>
            public int StartA { get { return this.startA_; } }
            
            /// <summary>
            /// 
            /// </summary>
            public int StartB { get { return this.startB_; } }
            
            /// <summary>
            /// 
            /// </summary>
            public int Length { get { return this.length_; } }

            #endregion

            #region event

            #endregion

            #region event handler

            #endregion

            #region event method

            #endregion

            #region public method

            /// <summary>
            /// 
            /// </summary>
            /// <param name="old"></param>
            /// <returns></returns>
            public static CommonSubsequence Reverse(CommonSubsequence old)
            {
                CommonSubsequence newTop = null;
                while (old != null)
                {
                    CommonSubsequence next = old.Next;
                    old.Next = newTop;
                    newTop = old;
                    old = next;
                }
                return newTop;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "Length:" + this.Length + ", A:" + this.StartA.ToString()
                  + ", B:" + this.StartB.ToString() + ", Next:" + ((this.Next == null) ? "null" : "exist");
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

        #endregion
    }
 
    #endregion
}
