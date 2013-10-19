using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Ricordanza.Core.Behaviors;
using Ricordanza.WinFormsUI.Forms;

namespace Ricordanza.WinFormsUI.Behaviors
{
    /// <summary>
    /// <see cref=" Ricordanza.WinFormsUI.Forms.ProgressWindow"/>を使用して<see cref="Ricordanza.Core.Behaviors.IBehavior"/>を実行するクラスです。
    /// </summary>
    public static class ProgressBehaviorExecutor
    {
        #region constant

        #endregion

        #region private variable

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
        /// <param name="behaviors">実行したい<see cref="Ricordanza.Core.Behaviors.IBehavior"/></param>
        /// <remarks>
        /// このメソッドは<see cref="System.Threading.Tasks.Task"/>を使用して<see cref="Ricordanza.Core.Behaviors.IBehavior"/>を実行します。
        /// このメソッドを実行するとプログレスウィンドウが表示されます。
        /// </remarks>
        public static void Invoke(params IBehavior[] behaviors)
        {
            ProgressWindow.Invoke((pw) =>
            {
                // 定義されているビジネスロジックを実行する
                BehaviorExecutor.Execute(behaviors);
            });
        }

        /// <summary>
        /// <see cref="Ricordanza.Core.Behaviors.IBehavior"/>を非同期実行します。
        /// </summary>
        /// <param name="behaviors">非同期実行したい<see cref="Ricordanza.Core.Behaviors.IBehavior"/></param>
        /// <returns>非同期実行オブジェクト。</returns>
        /// <remarks>
        /// このメソッドは実行順序を保証しません。
        /// このメソッドは<see cref="System.Threading.Tasks.Task"/>を使用して<see cref="Ricordanza.Core.Behaviors.IBehavior"/>を非同期実行します。
        /// このメソッドを実行するとプログレスウィンドウが表示されます。
        /// </remarks>
        public static Task[] InvokeAsync(params IBehavior[] behaviors)
        {
            Task[] tasks = new Task[] { };

            ProgressWindow.Invoke((pw) =>
            {
                // 定義されているビジネスロジックを実行する
                tasks = BehaviorExecutor.ExecuteAsync(behaviors);
            });

            return tasks;
        }

        /// <summary>
        /// <see cref="Ricordanza.Core.Behaviors.IBehavior"/>を同時実行します。
        /// </summary>
        /// <param name="behaviors">同時実行したい<see cref="Ricordanza.Core.Behaviors.IBehavior"/></param>
        /// <remarks>
        /// このメソッドは実行順序を保証しません。
        /// このメソッドは<see cref="System.Threading.Tasks.Task"/>を使用して<see cref="Ricordanza.Core.Behaviors.IBehavior"/>を同時実行します。
        /// このメソッドを実行するとプログレスウィンドウが表示されます。
        /// </remarks>
        public static void InvokeParallel(params IBehavior[] behaviors)
        {
            ProgressWindow.Invoke((pw) =>
            {
                // 定義されているビジネスロジックを実行する
                BehaviorExecutor.ExecuteParallel(behaviors);
            });
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
