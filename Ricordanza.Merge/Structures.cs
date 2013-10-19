//#define USE_HASH_TABLE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.Merge
{
    #region IDiffList

    /// <summary>
    /// 比較情報リスト
    /// </summary>
    public interface IDiffList
	{
        /// <summary>
        /// 
        /// </summary>
		int Count{ get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		IComparable GetByIndex(int index);
	}

    #endregion

    #region DiffStatus

    /// <summary>
    /// 
    /// </summary>
    internal enum DiffStatus 
	{
        /// <summary>
        /// 
        /// </summary>
		Matched = 1,

        /// <summary>
        /// 
        /// </summary>
        NoMatch = -1,

        /// <summary>
        /// 
        /// </summary>
		Unknown = -2
	}

    #endregion

    #region DiffState

    /// <summary>
    /// 
    /// </summary>
    internal class DiffState
	{
        #region const

        /// <summary>
        /// 
        /// </summary>
        private const int BAD_INDEX = -1;

        #endregion

        #region private variable

        /// <summary>
        /// 
        /// </summary>
        private int _startIndex;
        
        /// <summary>
        /// 
        /// </summary>
        private int _length;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// 
        /// </summary>
        public DiffState()
        {
            SetToUnkown();
        }

        #endregion

        #region property

        /// <summary>
        /// 
        /// </summary>
        public int StartIndex { get { return _startIndex; } }

        /// <summary>
        /// 
        /// </summary>
        public int EndIndex { get { return ((_startIndex + _length) - 1); } }

        /// <summary>
        /// 
        /// </summary>
        public int Length
        {
            get
            {
                int len;
                if (_length > 0)
                    len = _length;
                else
                {
                    if (_length == 0)
                        len = 1;
                    else
                        len = 0;
                }
                return len;
            }
        }

        public DiffStatus Status
        {
            get
            {
                DiffStatus stat;
                if (_length > 0)
                    stat = DiffStatus.Matched;
                else
                {
                    switch (_length)
                    {
                        case -1:
                            stat = DiffStatus.NoMatch;
                            break;
                        default:
                            stat = DiffStatus.Unknown;
                            break;
                    }
                }
                return stat;
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
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        public void SetMatch(int start, int length)
        {
            _startIndex = start;
            _length = length;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetNoMatch()
        {
            _startIndex = BAD_INDEX;
            _length = (int)DiffStatus.NoMatch;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newStart"></param>
        /// <param name="newEnd"></param>
        /// <param name="maxPossibleDestLength"></param>
        /// <returns></returns>
        public bool HasValidLength(int newStart, int newEnd, int maxPossibleDestLength)
        {
            if (_length > 0) //have unlocked match
            {
                if ((maxPossibleDestLength < _length) ||
                    ((_startIndex < newStart) || (EndIndex > newEnd)))
                    SetToUnkown();
            }
            return (_length != (int)DiffStatus.Unknown);
        }

        #endregion

        #region protected method

        /// <summary>
        /// 
        /// </summary>
        protected void SetToUnkown()
        {
            _startIndex = BAD_INDEX;
            _length = (int)DiffStatus.Unknown;
        }

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
	}

    #endregion

    #region DiffStateList

    /// <summary>
    /// 
    /// </summary>
    internal class DiffStateList
	{
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// 
        /// </summary>
        private DiffState[] _array;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destCount"></param>
        public DiffStateList(int destCount)
        {
            _array = new DiffState[destCount];
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
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DiffState GetByIndex(int index)
        {
            DiffState retval = _array[index];
            if (retval == null)
            {
                retval = new DiffState();
                _array[index] = retval;
            }
            return retval;
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

    #region DiffResultSpanStatus

    /// <summary>
    /// 
    /// </summary>
    public enum DiffResultSpanStatus
	{
        /// <summary>
        /// 
        /// </summary>
		NoChange,
		
        /// <summary>
        /// 
        /// </summary>
        Replace,
		
        /// <summary>
        /// 
        /// </summary>
        DeleteSource,
		
        /// <summary>
        /// 
        /// </summary>
        AddDestination
	}

    #endregion

    #region DiffResultSpan

    /// <summary>
    /// 
    /// </summary>
    public class DiffResultSpan : IComparable
	{
        #region const

        /// <summary>
        /// 
        /// </summary>
        private const int BAD_INDEX = -1;

        #endregion

        #region private variable
        
        /// <summary>
        /// 
        /// </summary>
        private int _destIndex;
        
        /// <summary>
        /// 
        /// </summary>
        private int _sourceIndex;
        
        /// <summary>
        /// 
        /// </summary>
        private int _length;
        
        /// <summary>
        /// 
        /// </summary>
        private DiffResultSpanStatus _status;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="destIndex"></param>
        /// <param name="sourceIndex"></param>
        /// <param name="length"></param>
        protected DiffResultSpan( DiffResultSpanStatus status, int destIndex, int sourceIndex, int length)
        {
            _status = status;
            _destIndex = destIndex;
            _sourceIndex = sourceIndex;
            _length = length;
        }

        #endregion

        #region property

        /// <summary>
        /// 
        /// </summary>
        public int DestIndex { get { return _destIndex; } }

        /// <summary>
        /// 
        /// </summary>
        public int SourceIndex { get { return _sourceIndex; } }
        
        /// <summary>
        /// 
        /// </summary>
        public int Length { get { return _length; } }
        
        /// <summary>
        /// 
        /// </summary>
        public DiffResultSpanStatus Status { get { return _status; } }

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
        /// <param name="destIndex"></param>
        /// <param name="sourceIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static DiffResultSpan CreateNoChange(int destIndex, int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.NoChange, destIndex, sourceIndex, length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destIndex"></param>
        /// <param name="sourceIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static DiffResultSpan CreateReplace(int destIndex, int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.Replace, destIndex, sourceIndex, length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static DiffResultSpan CreateDeleteSource(int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.DeleteSource, BAD_INDEX, sourceIndex, length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static DiffResultSpan CreateAddDestination(int destIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.AddDestination, destIndex, BAD_INDEX, length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        public void AddLength(int i)
        {
            _length += i;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} (Dest: {1},Source: {2}) {3}",
                _status.ToString(),
                _destIndex.ToString(),
                _sourceIndex.ToString(),
                _length.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return _destIndex.CompareTo(((DiffResultSpan)obj)._destIndex);
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