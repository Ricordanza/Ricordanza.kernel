using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ricordanza.Merge
{
    /// <summary>
    /// 文字列マージクラスです。
    /// </summary>
    /// <example>
    /// ■比較結果を取得
    /// <code>
    /// string a = File.ReadAllText(@"text1.txt");
    /// string b = File.ReadAllText(@"text2.txt");
    /// var merger = StringMerger.Report(a, b);
    /// DiffReport result = merger.Report();
    /// foreach (DiffResultSpan drs in report)
    /// {
    ///     switch (drs.Status)
    ///     {
    ///         case DiffResultSpanStatus.DeleteSource:
    ///             // No Operation.
    ///             break;
    ///         case DiffResultSpanStatus.NoChange:
    ///             for (i = 0; i &gt; drs.Length; i++)
    ///                 sb.AppendLine(report.Source.GetByIndex(drs.SourceIndex + i).ToString());
    ///             break;
    ///         case DiffResultSpanStatus.AddDestination:
    ///             for (i = 0; i &gt; drs.Length; i++)
    ///                 sb.AppendLine(report.Destination.GetByIndex(drs.DestIndex + i).ToString());
    ///             break;
    ///         case DiffResultSpanStatus.Replace:
    ///             for (i = 0; i &gt; drs.Length; i++)
    ///                 sb.AppendLine(report.Destination.GetByIndex(drs.DestIndex + i).ToString());
    ///             break;
    ///     }
    /// }
    /// </code>
    /// ■組み込みアルゴリズムでマージ
    /// <code>
    /// string a = File.ReadAllText(@"text1.txt");
    /// string b = File.ReadAllText(@"text2.txt");
    /// var merged = StringMerger.Merge(a, b);
    /// </code>
    /// </example>
    public static class StringMerger
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
        /// 比較結果を取得します。
        /// </summary>
        /// <param name="source">比較元文字列</param>
        /// <param name="destination">比較先文字列</param>
        /// <param name="level">比較レベル</param>
        /// <returns>比較結果</returns>
        public static DiffReport Report(string source, string destination, DiffEngineLevel level = DiffEngineLevel.SlowPerfect)
        {
            var d1 = new StringLineCollection(source);
            var d2 = new StringLineCollection(destination);
            DiffEngine de = new DiffEngine();
            de.ProcessDiff(d1, d2, level);

            return de.DiffReport();
        }

        /// <summary>
        /// マージ処理を行います。
        /// </summary>
        /// <param name="source">比較元文字列</param>
        /// <param name="destination">比較先文字列</param>
        /// <param name="level">比較レベル</param>
        /// <returns>マージ結果の文字列</returns>
        public static string Merge(string source, string destination, DiffEngineLevel level = DiffEngineLevel.SlowPerfect)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            DiffReport report = Report(source, destination, level);
            foreach (DiffResultSpan drs in report)
            {
                switch (drs.Status)
                {
                    case DiffResultSpanStatus.DeleteSource:
                        // No Operation.
                        break;
                    case DiffResultSpanStatus.NoChange:
                        for (i = 0; i < drs.Length; i++)
                            sb.AppendLine(report.Source.GetByIndex(drs.SourceIndex + i).ToString());
                        break;
                    case DiffResultSpanStatus.AddDestination:
                        for (i = 0; i < drs.Length; i++)
                            sb.AppendLine(report.Destination.GetByIndex(drs.DestIndex + i).ToString());
                        break;
                    case DiffResultSpanStatus.Replace:
                        for (i = 0; i < drs.Length; i++)
                            sb.AppendLine(report.Destination.GetByIndex(drs.DestIndex + i).ToString());
                        break;
                }
            }

            // 終端の改行コードはロジック上で付加した物なので削除する
            string ret = sb.ToString();
            if (ret.EndsWith(Environment.NewLine))
                ret = ret.Remove(ret.LastIndexOf(Environment.NewLine), Environment.NewLine.Length);

            return ret;
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
