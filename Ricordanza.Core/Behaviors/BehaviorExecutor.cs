using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ricordanza.Core.Behaviors
{
    /// <summary>
    /// <see cref="Ricordanza.Core.Behaviors.IBehavior"/>実行クラスです。
    /// 全ての<see cref="Ricordanza.Core.Behaviors.IBehavior"/>はこのクラスを介して実行されます。
    /// </summary>
    public static class BehaviorExecutor
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
        /// <see cref="Ricordanza.Core.Behaviors.IBehavior"/>を実行します。
        /// </summary>
        /// <param name="behavior">実行したい<see cref="Ricordanza.Core.Behaviors.IBehavior"/></param>
        public static void Execute(IBehavior behavior)
        {
            if (behavior == null)
                return;

            try
            {
                // 初期化処理の実行
                behavior.Initalize();

                // メイン処理の実行
                behavior.Invoke();
            }
            finally
            {
                // 終了処理の実行
                behavior.Terminate();
            }
        }

        /// <summary>
        /// <see cref="Ricordanza.Core.Behaviors.IBehavior"/>を実行します。
        /// </summary>
        /// <param name="behaviors">実行したい<see cref="Ricordanza.Core.Behaviors.IBehavior"/></param>
        public static void Execute(params IBehavior[] behaviors)
        {
            if (behaviors == null)
                return;

            behaviors.ToList().ForEach(behavior => Execute(behavior));
        }

        /// <summary>
        /// <see cref="Ricordanza.Core.Behaviors.IBehavior"/>を非同期実行します。
        /// </summary>
        /// <param name="behaviors">非同期実行したい<see cref="Ricordanza.Core.Behaviors.IBehavior"/></param>
        /// <returns>非同期実行オブジェクト。</returns>
        /// <remarks>
        /// このメソッドは実行順序を保証しません。
        /// このメソッドは<see cref="System.Threading.Tasks.Task"/>を使用して<see cref="Ricordanza.Core.Behaviors.IBehavior"/>を非同期実行します。
        /// </remarks>
        public static Task[] ExecuteAsync(params IBehavior[] behaviors)
        {
            List<Task> tasks = new List<Task>();

            if (behaviors == null)
                return tasks.ToArray();

            Parallel.ForEach<IBehavior>(behaviors, behavior => tasks.Add(Task.Factory.StartNew(() => Execute(behavior))));

            return tasks.ToArray();
        }

        /// <summary>
        /// <see cref="Ricordanza.Core.Behaviors.IBehavior"/>を同時実行します。
        /// </summary>
        /// <param name="behaviors">同時実行したい<see cref="Ricordanza.Core.Behaviors.IBehavior"/></param>
        /// <remarks>
        /// このメソッドは実行順序を保証しません。
        /// このメソッドは<see cref="System.Threading.Tasks.Task"/>を使用して<see cref="Ricordanza.Core.Behaviors.IBehavior"/>を同時実行します。
        /// </remarks>
        public static void ExecuteParallel(params IBehavior[] behaviors)
        {
            if (behaviors == null)
                return;

            List<Action> tasks = new List<Action>();
            behaviors.ToList().ForEach(behavior => tasks.Add(() => Execute(behavior)));

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
