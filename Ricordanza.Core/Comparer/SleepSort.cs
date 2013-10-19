using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace Ricordanza.Core.Comparer
{
    /// <summary>
    /// <c>Sleep</c>を使用してソートを行うアルゴリズムです。
    /// </summary>
    /// <example>
    /// <code>
    /// var xs = new[] { 11, 215, 12, 1985, 12, 1203, 12, 152 };
    /// var sorted = Sort(xs, x => x);
    /// </code>
    /// </example>
    public class SleepSort
    {
        #region const

        /// <summary>
        /// スレット待機時間の基本値
        /// </summary>
        const int WEIGHT = 40000;

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
        /// SleepSrotアルゴリズムでソートを行います。
        /// </summary>
        /// <typeparam name="T">ソート対象の実態型</typeparam>
        /// <param name="xs">ソートを行いたい列挙子</param>
        /// <param name="conv">ソート比較子</param>
        /// <returns>ソート後の列挙子</returns>
        /// <exsaexample>
        /// <code>
        /// #!/bin/bash
        /// function f() {
        ///     sleep $(echo "$1 / 10" |bc -l)
        ///     echo "$1"
        /// }
        /// while [ -n "$1" ]
        /// do
        ///     f "$1" &
        ///     shift
        /// done
        /// wait
        /// </code>
        /// </exsaexample>
        public static IEnumerable<T> Invoke<T>(IEnumerable<T> xs, Func<T, int> conv)
        {
            var ev = new EventWaitHandle(false, EventResetMode.ManualReset);
            var q = new Queue<T>();
            ParameterizedThreadStart f = x => { ev.WaitOne(); Thread.SpinWait(conv((T)x) * WEIGHT); lock (q) { q.Enqueue((T)x); } };
            var ts = xs.Select(x => { var t = new Thread(f); t.Start(x); return t; }).ToArray();
            ev.Set();
            foreach (var t in ts)
                t.Join();
            return q;
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
