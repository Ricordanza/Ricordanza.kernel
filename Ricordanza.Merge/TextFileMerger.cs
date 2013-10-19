using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ricordanza.Merge
{
    /// <summary>
    /// ファイルマージクラスです。
    /// </summary>
    /// <example>
    /// ■比較結果を取得
    /// <code>
    /// var merger = TextFileMerger(@"text1.txt", @"text2.txt");
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
    /// var merger = TextFileMerger(@"text1.txt", @"text2.txt");
    /// merger.Merge(@"text3.txt");
    /// </code>
    /// </example>
    public class TextFileMarger
    {
        #region const

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// 新しいこのクラスのインスタンスを構築します。
        /// </summary>
        /// <param name="sourcePath">比較元ファイルパス</param>
        /// <param name="destinationPath">比較先ファイルパス</param>
        public TextFileMarger(string sourcePath, string destinationPath)
             : this(sourcePath, destinationPath, Encoding.UTF8)
        {
        }

        /// <summary>
        /// 新しいこのクラスのインスタンスを構築します。
        /// </summary>
        /// <param name="sourcePath">比較元ファイルパス</param>
        /// <param name="destinationPath">比較先ファイルパス</param>
        /// <param name="encode">ファイルのエンコード</param>
        public TextFileMarger(string sourcePath, string destinationPath, Encoding encode)
             : base()
        {
            SourcePath = sourcePath;
            DestinationPath = destinationPath;
            Encoding = encode;
        }

        #endregion

        #region property

        /// <summary>
        /// 比較元ファイルパス
        /// </summary>
        public string SourcePath { protected set; get; }

        /// <summary>
        /// 比較先ファイルパス
        /// </summary>
        public string DestinationPath { protected set; get; }

        /// <summary>
        /// エンコード
        /// </summary>
        public Encoding Encoding { protected set; get; }

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
        /// <param name="level">比較レベル</param>
        /// <returns>比較結果</returns>
        public DiffReport Report(DiffEngineLevel level = DiffEngineLevel.SlowPerfect)
        {
            CheckFileExist(SourcePath);
            CheckFileExist(DestinationPath);

            var d1 = new TextLineCollection(SourcePath);
            var d2 = new TextLineCollection(DestinationPath);
            DiffEngine de = new DiffEngine();
            de.ProcessDiff(d1, d2, level);

            return de.DiffReport();
        }

        /// <summary>
        /// マージ処理を行います。
        /// </summary>
        /// <param name="mergedPath">マージ結果を保存するファイルのパス</param>
        /// <param name="level">比較レベル</param>
        public void Merge(string mergedPath, DiffEngineLevel level = DiffEngineLevel.SlowPerfect)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            DiffReport report = Report(level);
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

            File.WriteAllText(mergedPath, ret, Encoding);
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        /// <summary>
        /// ファイルの存在チェックを行います。
        /// </summary>
        /// <param name="path"></param>
        private void CheckFileExist(string path)
        {
            // ファイルが存在しない場合
            if (!File.Exists(path))
                throw new FileNotFoundException("File Not Found.", path);
        }

        #endregion

        #region delegate

        #endregion
    }
}
