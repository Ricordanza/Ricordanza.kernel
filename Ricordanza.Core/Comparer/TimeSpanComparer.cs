using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ricordanza.Core.Comparer
{
    /// <summary>
    /// タイム・スタンプ順にソートクラスです。
    /// </summary>
    public class TimeSpanComparer
        : IComparer<FileSystemInfo>
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
        public TimeSpanComparer()
            : base()
        {
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
        /// 2つのオブジェクトを比較し、一方が他方より小さいか、等しいか、大きいかを示す値を返します。
        /// </summary>
        /// <param name="x">比較する最初のオブジェクトです</param>
        /// <param name="y">比較する 2 番目のオブジェクト</param>
        /// <returns>
        /// x と y の相対的な値を示す符号付き整数。<br />
        /// </returns>
        public int Compare(FileSystemInfo x, FileSystemInfo y)
        {
            return x.LastAccessTime.CompareTo(y.LastAccessTime);
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