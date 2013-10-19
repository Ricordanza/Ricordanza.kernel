using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Ricordanza.Core.Behaviors;

namespace Ricordanza.Core.Behaviors
{
    /// <summary>
    /// <see cref="System.Action"/>実行クラスです。
    /// 全ての<see cref="System.Action"/>はこのクラスを介して実行されます。
    /// </summary>
    public static class ActionExecutor
    {
        #region constant

        #endregion

        #region protected variable

        #endregion

        #region property

        #endregion

        #region static constractor

        #endregion

        #region constractor

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// <see cref="System.Action"/>を実行します。
        /// </summary>
        /// <param name="action">実行したい<see cref="System.Action"/></param>
        public static void Invoke(Action action)
        {
            if (action == null)
                return;

            action.Invoke();
        }

        /// <summary>
        /// <see cref="System.Action"/>を実行します。
        /// </summary>
        /// <param name="actions">実行したい<see cref="System.Action"/></param>
        public static void Invoke(params Action[] actions)
        {
            if (actions == null)
                return;

            // ビジネスロジックを実行
            actions.ToList().ForEach(action => Invoke(action));
        }

        /// <summary>
        /// <see cref="System.Action"/>を非同期実行します。
        /// </summary>
        /// <param name="actions">非同期実行したい<see cref="System.Action"/></param>
        /// <returns>非同期実行オブジェクト。</returns>
        /// <remarks>
        /// このメソッドは実行順序を保証しません。
        /// このメソッドは<see cref="System.Threading.Tasks.Task"/>を使用して<see cref="System.Action"/>を非同期実行します。
        /// </remarks>
        public static Task[] InvokeAsync(params Action[] actions)
        {
            List<Task> tasks = new List<Task>();

            if (actions == null)
                return tasks.ToArray();

            Parallel.ForEach<Action>(actions, action => tasks.Add(Task.Factory.StartNew(() => Invoke(action))));

            return tasks.ToArray();
        }

        /// <summary>
        /// <see cref="System.Action"/>を同時実行します。
        /// </summary>
        /// <param name="actions">同時実行したい<see cref="System.Action"/></param>
        /// <remarks>
        /// このメソッドは実行順序を保証しません。
        /// このメソッドは<see cref="System.Threading.Tasks.Task"/>を使用して<see cref="System.Action"/>を同時実行します。
        /// </remarks>
        public static void InvokeParallel(params Action[] actions)
        {
            if (actions == null)
                return;

            List<Action> tasks = new List<Action>();
            actions.ToList().ForEach(action => tasks.Add(() => Invoke(action)));

            Parallel.Invoke(tasks.ToArray());
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
