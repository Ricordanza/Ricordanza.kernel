using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.Core.Utilities
{
    /// <summary>
    /// ファイル操作のユーティリティクラスです。
    /// </summary>
    /// <remarks>このクラスは<see cref="System.Collections.Generic.IEnumerable&lt;out T&gt;"/>を拡張します。</remarks>
    public static class EnumerableUtility
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
        /// インデックス番号取得可能な拡張<c>For</c>ループを提供します。
        /// </summary>
        /// <typeparam name="T">列挙する値のデータ型</typeparam>
        /// <param name="source">インデックス番号を取得したい反復子</param>
        /// <param name="action">各要素に対して実行する<see cref="System.Action&lt;in T1, in T2, in T3&gt;"/>デリゲート</param>
        public static void ForEach<T>(this IEnumerable <T> source, Action<T, int, int> action)
        {
            ForEach<T>(source, action, _ => true);
        }

        /// <summary>
        /// インデックス番号とループ回数を取得可能な拡張<c>For</c>ループを提供します。
        /// </summary>
        /// <typeparam name="T">列挙する値のデータ型</typeparam>
        /// <param name="source">インデックス番号を取得したい反復子</param>
        /// <param name="action">各要素に対して実行する<see cref="System.Action&lt;in T1, in T2, in T3&gt;"/>デリゲート</param>
        /// <param name="predicate">各要素が条件を満たしているかどうかをテストする関数。</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int, int> action, Func<T, bool> predicate)
        {
            int index = 0;
            int count = 1;
            foreach (var x in source)
            {
                // 条件をみたしている場合のみ処理を実行
                if (predicate(x))
                    action(x, index, count++);
                index++;
            }
        }

        /// <summary>
        /// インデックス番号とループ回数を取得可能な拡張<c>For</c>ループを提供します。
        /// </summary>
        /// <typeparam name="T">列挙する値のデータ型</typeparam>
        /// <param name="source">インデックス番号を取得したい反復子</param>
        /// <param name="action">各要素に対して実行する<see cref="System.Action&lt;in T1, in T2, in T3, out TResult&gt;"/>デリゲート</param>
        public static void ForEach<T>(this IEnumerable<T> source, Func<T, int, int, bool> action)
        {
            ForEach<T>(source, action, _ => true);
        }

        /// <summary>
        /// インデックス番号取得可能な拡張<c>For</c>ループを提供します。
        /// </summary>
        /// <typeparam name="T">列挙する値のデータ型</typeparam>
        /// <param name="source">インデックス番号を取得したい反復子</param>
        /// <param name="action">各要素に対して実行する<see cref="System.Action&lt;in T1, in T2, in T3, out TResult&gt;"/>デリゲート</param>
        /// <param name="predicate">各要素が条件を満たしているかどうかをテストする関数。</param>
        public static void ForEach<T>(this IEnumerable<T> source, Func<T, int, int, bool> action, Func<T, bool> predicate)
        {
            int index = 0;
            int count = 1;
            foreach (var x in source)
            {
                // 条件をみたしている場合のみ処理を実行
                if (predicate(x))
                    // falseが返却された場合はループをbreakする
                    if (!action(x, index, count++))
                        break;
                index++;
            }
        }

        /// <summary>
        /// 漸化式を提供します。
        /// </summary>
        /// <typeparam name="T">処理対象のデータ型</typeparam>
        /// <param name="state">状態</param>
        /// <param name="generator">式</param>
        /// <returns>式を実行した反復子</returns>
        /// <example>
        /// <code>
        /// var flipflop = true.Unfold(b =&gt; !b);
        /// foreach (var r in flipflop.Take(10))
        ///     Console.WriteLine(r);
        /// </code>
        /// </example>
        public static IEnumerable<T> Unfold<T>(this T state, Func<T, T> generator)
        {
            yield return state;
            var newState = generator(state);
            while (newState != null)
            {
                yield return newState;
                newState = generator(newState);
            }
        }

        /// <summary>
        /// 漸化式を提供します。
        /// </summary>
        /// <typeparam name="T">処理対象のデータ型</typeparam>
        /// <param name="state">状態</param>
        /// <param name="generator">式</param>
        /// <param name="until">繰り返し条件</param>
        /// <returns>式を実行した反復子</returns>
        /// <example>
        /// <code>
        /// var seq = 1.Unfold(n =&gt; n * 2, n =&gt; n &lt; 1000);
        /// foreach (var r in seq)
        ///     Console.WriteLine(r);
        /// </code>
        /// <c>1 2 4 8 16 32 64 128 256 512</c>
        /// </example>
        public static IEnumerable<T> Unfold<T>(this T state, Func<T, T> generator, Predicate<T> until)
        {
            yield return state;
            var newState = generator(state);
            while (until(newState))
            {
                yield return newState;
                newState = generator(newState);
            }
        }

        /// <summary>
        /// 漸化式を提供します。
        /// </summary>
        /// <typeparam name="S">初期値に与える型</typeparam>
        /// <typeparam name="T">戻り値の各要素の型</typeparam>
        /// <param name="state">状態</param>
        /// <param name="generator">式</param>
        /// <returns>式を実行した反復子</returns>
        /// <example>
        /// <code>
        /// var flipflop = true.Unfold(b =&gt; Tuple.Create(b, !b));
        /// foreach (var r in flipflop.Take(6))
        ///     Console.WriteLine(r);
        /// </code>
        /// </example>
        public static IEnumerable<T> Unfold<S, T>(this S state, Func<S, Tuple<T, S>> generator)
        {
            var newState = generator(state);
            while (newState != null)
            {
                yield return newState.Item1;
                newState = generator(newState.Item2);
            }
        }

        /// <summary>
        /// <c>2</c>つのシーケンスをマージします。
        /// </summary>
        /// <typeparam name="T1"><c>1</c>番目の入力シーケンスの要素の型。</typeparam>
        /// <typeparam name="T2"><c>2</c>番目の入力シーケンスの要素の型。</typeparam>
        /// <param name="first">マージする<c>1</c>番目のシーケンス(キー要素)。</param>
        /// <param name="second">マージする<c>2</c>番目のシーケンス(値要素)。</param>
        /// <returns><c>2</c>つの入力シーケンスのマージされた要素が格納されている<see cref="System.Collections.Generic.Zip&lt;TKey, TValue&gt;"/></returns>
        /// <example>
        /// <code>
        /// foreach (var x in new [] { "A" , "B" , "C" }.Zip(new [] {  "1" , "2" , "3" }))
        ///     Console.WriteLine(x.Key + " : " + x.Value);
        /// </code>
        /// </example>
        public static Dictionary<T1, T2> Zip<T1, T2>(this IEnumerable<T1> keys, IEnumerable<T2> values)
        {
            var dic = new Dictionary<T1, T2>();
            foreach (var x in Enumerable.Zip(keys, values, (key, val) => new { key, val }))
                dic.Add(x.key, x.val);

            return dic;
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
