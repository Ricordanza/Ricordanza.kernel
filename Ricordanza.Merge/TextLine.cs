using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ricordanza.Merge
{
    #region TextLine

    /// <summary>
    /// 
    /// </summary>
    public class TextLine
        : IComparable
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
        /// <param name="value"></param>
        public TextLine(string value)
        {
            Line = value.Replace("\t", "    ");
            Hash = value.GetHashCode();
        }

        #endregion

        #region property

        /// <summary>
        /// 一行分のデータ
        /// </summary>
        public string Line{ protected set; get; }

        /// <summary>
        /// 行データのハッシュ値
        /// </summary>
        public int Hash { protected set; get; }

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
            return Line;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return Hash.CompareTo(((TextLine)obj).Hash);
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

    #region TextLines

    /// <summary>
    /// 
    /// </summary>
    public class TextLineCollection
        : List<TextLine>, IDiffList
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
        /// <param name="fileName"></param>
        public TextLineCollection(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    this.Add(new TextLine(line));
            }
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

        public IComparable GetByIndex(int index)
        {
            return this[index];
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

    #region StringLineCollection

    /// <summary>
    /// 
    /// </summary>
    public class StringLineCollection
        : List<TextLine>, IDiffList
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
        /// <param name="value"></param>
        public StringLineCollection(string value)
        {
            new List<string>(value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)).ForEach(
                s =>
                {
                    this.Add(new TextLine(s));
                });
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

        public IComparable GetByIndex(int index)
        {
            return this[index];
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
}
