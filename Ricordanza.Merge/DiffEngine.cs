using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.Merge
{
    #region DiffEngineLevel

    /// <summary>
    /// 比較方法レベル。
    /// </summary>
    public enum DiffEngineLevel
    {
        /// <summary>
        /// 早い(不完全)
        /// </summary>
        FastImperfect,

        /// <summary>
        /// 標準
        /// </summary>
        Medium,

        /// <summary>
        /// ゆっくり(完全)
        /// </summary>
        SlowPerfect
    }
    #endregion

    #region DiffEngine

    /// <summary>
    /// 
    /// </summary>
    public class DiffEngine
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// 
        /// </summary>
        private IDiffList _source;

        /// <summary>
        /// 
        /// </summary>
        private IDiffList _destination;

        /// <summary>
        /// 
        /// </summary>
        private DiffReport _matchList;

        /// <summary>
        /// 
        /// </summary>
        private DiffEngineLevel _level;

        /// <summary>
        /// 
        /// </summary>
        private DiffStateList _stateList;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// 
        /// </summary>
        public DiffEngine()
        {
            _source = null;
            _destination = null;
            _matchList = null;
            _stateList = null;
            _level = DiffEngineLevel.FastImperfect;
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
        /// 比較処理を実行します。
        /// </summary>
        /// <param name="source">比較元リスト</param>
        /// <param name="destination">比較先リスト</param>
        /// <returns>経過時間(ms)</returns>
        public double ProcessDiff(IDiffList source, IDiffList destination)
        {
            DateTime dt = DateTime.Now;
            _source = source;
            _destination = destination;
            _matchList = new DiffReport();

            int dcount = _destination.Count;
            int scount = _source.Count;

            if ((dcount > 0) && (scount > 0))
            {
                _stateList = new DiffStateList(dcount);
                ProcessRange(0, dcount - 1, 0, scount - 1);
            }

            TimeSpan ts = DateTime.Now - dt;
            return ts.TotalSeconds;
        }

        /// <summary>
        /// 比較処理を実行します。
        /// </summary>
        /// <param name="source">比較元リスト</param>
        /// <param name="destination">比較先リスト</param>
        /// <param name="level">比較レベル</param>
        /// <returns>経過時間(ms)</returns>
        public double ProcessDiff(IDiffList source, IDiffList destination, DiffEngineLevel level)
        {
            _level = level;
            return ProcessDiff(source, destination);
        }

        /// <summary>
        /// 比較結果を取得します。
        /// </summary>
        /// <returns>比較結果</returns>
        public DiffReport DiffReport()
        {
            DiffReport retval = new DiffReport();
            retval.Source = _source;
            retval.Destination = _destination;

            int dcount = _destination.Count;
            int scount = _source.Count;

            //Deal with the special case of empty files
            if (dcount == 0)
            {
                if (scount > 0)
                    retval.Add(DiffResultSpan.CreateDeleteSource(0, scount));
                return retval;
            }
            else
            {
                if (scount == 0)
                {
                    retval.Add(DiffResultSpan.CreateAddDestination(0, dcount));
                    return retval;
                }
            }

            _matchList.Sort();
            int curDest = 0;
            int curSource = 0;
            DiffResultSpan last = null;

            foreach (DiffResultSpan drs in _matchList)
            {
                if ((!AddChanges(retval, curDest, drs.DestIndex, curSource, drs.SourceIndex)) && (last != null))
                    last.AddLength(drs.Length);
                else
                    retval.Add(drs);

                curDest = drs.DestIndex + drs.Length;
                curSource = drs.SourceIndex + drs.Length;
                last = drs;
            }

            AddChanges(retval, curDest, dcount, curSource, scount);

            return retval;
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destIndex"></param>
        /// <param name="sourceIndex"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        private int GetSourceMatchLength(int destIndex, int sourceIndex, int maxLength)
        {
            int matchCount;
            for (matchCount = 0; matchCount < maxLength; matchCount++)
            {
                if (_destination.GetByIndex(destIndex + matchCount).CompareTo(_source.GetByIndex(sourceIndex + matchCount)) != 0)
                    break;
            }
            return matchCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="curItem"></param>
        /// <param name="destIndex"></param>
        /// <param name="destEnd"></param>
        /// <param name="sourceStart"></param>
        /// <param name="sourceEnd"></param>
        private void GetLongestSourceMatch(DiffState curItem, int destIndex, int destEnd, int sourceStart, int sourceEnd)
        {
            int maxDestLength = (destEnd - destIndex) + 1;
            int curLength = 0;
            int curBestLength = 0;
            int curBestIndex = -1;
            int maxLength = 0;
            for (int sourceIndex = sourceStart; sourceIndex <= sourceEnd; sourceIndex++)
            {
                maxLength = Math.Min(maxDestLength, (sourceEnd - sourceIndex) + 1);
                if (maxLength <= curBestLength)
                    break;

                curLength = GetSourceMatchLength(destIndex, sourceIndex, maxLength);
                if (curLength > curBestLength)
                {
                    curBestIndex = sourceIndex;
                    curBestLength = curLength;
                }

                sourceIndex += curBestLength;
            }

            if (curBestIndex == -1)
                curItem.SetNoMatch();
            else
                curItem.SetMatch(curBestIndex, curBestLength);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destStart"></param>
        /// <param name="destEnd"></param>
        /// <param name="sourceStart"></param>
        /// <param name="sourceEnd"></param>
        private void ProcessRange(int destStart, int destEnd, int sourceStart, int sourceEnd)
        {
            int curBestIndex = -1;
            int curBestLength = -1;
            int maxPossibleDestLength = 0;
            DiffState curItem = null;
            DiffState bestItem = null;
            for (int destIndex = destStart; destIndex <= destEnd; destIndex++)
            {
                maxPossibleDestLength = (destEnd - destIndex) + 1;
                if (maxPossibleDestLength <= curBestLength)
                    break;

                curItem = _stateList.GetByIndex(destIndex);

                if (!curItem.HasValidLength(sourceStart, sourceEnd, maxPossibleDestLength))
                    GetLongestSourceMatch(curItem, destIndex, destEnd, sourceStart, sourceEnd);

                if (curItem.Status == DiffStatus.Matched)
                {
                    switch (_level)
                    {
                        case DiffEngineLevel.FastImperfect:
                            if (curItem.Length > curBestLength)
                            {
                                curBestIndex = destIndex;
                                curBestLength = curItem.Length;
                                bestItem = curItem;
                            }
                            destIndex += curItem.Length - 1;
                            break;
                        case DiffEngineLevel.Medium:
                            if (curItem.Length > curBestLength)
                            {
                                curBestIndex = destIndex;
                                curBestLength = curItem.Length;
                                bestItem = curItem;
                                destIndex += curItem.Length - 1;
                            }
                            break;
                        default:
                            if (curItem.Length > curBestLength)
                            {
                                curBestIndex = destIndex;
                                curBestLength = curItem.Length;
                                bestItem = curItem;
                            }
                            break;
                    }
                }
            }
            if (curBestIndex < 0)
            {
                // No Operation.
            }
            else
            {
                int sourceIndex = bestItem.StartIndex;
                _matchList.Add(DiffResultSpan.CreateNoChange(curBestIndex, sourceIndex, curBestLength));
                if (destStart < curBestIndex)
                {
                    if (sourceStart < sourceIndex)
                        ProcessRange(destStart, curBestIndex - 1, sourceStart, sourceIndex - 1);
                }
                int upperDestStart = curBestIndex + curBestLength;
                int upperSourceStart = sourceIndex + curBestLength;
                if (destEnd > upperDestStart)
                {
                    if (sourceEnd > upperSourceStart)
                        ProcessRange(upperDestStart, destEnd, upperSourceStart, sourceEnd);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="report"></param>
        /// <param name="curDest"></param>
        /// <param name="nextDest"></param>
        /// <param name="curSource"></param>
        /// <param name="nextSource"></param>
        /// <returns></returns>
        private bool AddChanges(DiffReport report, int curDest, int nextDest, int curSource, int nextSource)
        {
            bool retval = false;
            int diffDest = nextDest - curDest;
            int diffSource = nextSource - curSource;
            int minDiff = 0;
            if (diffDest > 0)
            {
                if (diffSource > 0)
                {
                    minDiff = Math.Min(diffDest, diffSource);
                    report.Add(DiffResultSpan.CreateReplace(curDest, curSource, minDiff));
                    if (diffDest > diffSource)
                    {
                        curDest += minDiff;
                        report.Add(DiffResultSpan.CreateAddDestination(curDest, diffDest - diffSource));
                    }
                    else
                    {
                        if (diffSource > diffDest)
                        {
                            curSource += minDiff;
                            report.Add(DiffResultSpan.CreateDeleteSource(curSource, diffSource - diffDest));
                        }
                    }
                }
                else
                    report.Add(DiffResultSpan.CreateAddDestination(curDest, diffDest));
                retval = true;
            }
            else
            {
                if (diffSource > 0)
                {
                    report.Add(DiffResultSpan.CreateDeleteSource(curSource, diffSource));
                    retval = true;
                }
            }
            return retval;
        }

        #endregion

        #region delegate

        #endregion
    }

    #endregion

    #region DiffReport

    /// <summary>
    /// 比較結果クラスです。
    /// </summary>
    public class DiffReport
        : List<DiffResultSpan>
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
        /// 編集元
        /// </summary>
        public IDiffList Source { internal set; get; }

        /// <summary>
        /// 編集先
        /// </summary>
        public IDiffList Destination { internal set; get; }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

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
