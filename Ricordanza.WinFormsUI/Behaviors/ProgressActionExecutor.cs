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
    /// <see cref=" Ricordanza.WinFormsUI.Forms.ProgressWindow"/>を使用して<see cref="System.Action"/>を実行するクラスです。
    /// </summary>
    public class ProgressActionExecutor
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
        /// <param name="actions">実行したい<see cref="System.Action"/></param>
        /// <remarks>
        /// このメソッドは<see cref="System.Threading.Tasks.Task"/>を使用して<see cref="System.Action"/>を実行します。
        /// このメソッドを実行するとプログレスウィンドウが表示されます。
        /// </remarks>
        public static void Invoke(params Action[] actions)
        {
            ProgressWindow.Invoke((pw) =>
            {
                // 定義されているビジネスロジックを実行する
                ActionExecutor.Invoke(actions);
            });
        }

        /// <summary>
        /// <see cref="System.Action"/>を実行します。
        /// </summary>
        /// <param name="actions">実行したい<see cref="System.Action"/></param>
        /// <remarks>
        /// このメソッドは<see cref="System.Threading.Tasks.Task"/>を使用して<see cref="System.Action"/>を実行します。
        /// このメソッドを実行するとプログレスウィンドウが表示されます。
        /// </remarks>
        public static void InvokeAndWait(params Action[] actions)
        {
            InvokeAndWait(null, actions);
        }

        /// <summary>
        /// <see cref="System.Action"/>を実行します。
        /// </summary>
        /// <param name="owner">プログレスウィンドウのオーナー</param>
        /// <param name="actions">実行したい<see cref="System.Action"/></param>
        /// <remarks>
        /// このメソッドは<see cref="System.Threading.Tasks.Task"/>を使用して<see cref="System.Action"/>を実行します。
        /// このメソッドを実行するとプログレスウィンドウが表示されます。
        /// </remarks>
        public static void InvokeAndWait(IWin32Window owner, params Action[] actions)
        {
            ProgressWindow.Invoke(owner, pw =>
            {
                System.Threading.Thread.Sleep(50);
                // 定義されているビジネスロジックを実行する
                ActionExecutor.Invoke(actions);
                System.Threading.Thread.Sleep(50);
            });
        }

        /// <summary>
        /// <see cref="System.Action"/>を非同期実行します。
        /// </summary>
        /// <param name="actions">非同期実行したい<see cref="System.Action"/></param>
        /// <returns>非同期実行オブジェクト。</returns>
        /// <remarks>
        /// このメソッドは実行順序を保証しません。
        /// このメソッドは<see cref="System.Threading.Tasks.Task"/>を使用して<see cref="System.Action"/>を非同期実行します。
        /// このメソッドを実行するとプログレスウィンドウが表示されます。
        /// </remarks>
        public static Task[] InvokeAsync(params Action[] actions)
        {
            Task[] tasks = new Task[] { };

            ProgressWindow.Invoke((pw) =>
            {
                // 定義されているビジネスロジックを実行する
                tasks = ActionExecutor.InvokeAsync(actions);
            });

            return tasks;
        }

        /// <summary>
        /// <see cref="System.Action"/>を同時実行します。
        /// </summary>
        /// <param name="actions">同時実行したい<see cref="System.Action"/></param>
        /// <remarks>
        /// このメソッドは実行順序を保証しません。
        /// このメソッドは<see cref="System.Threading.Tasks.Task"/>を使用して<see cref="System.Action"/>を同時実行します。
        /// このメソッドを実行するとプログレスウィンドウが表示されます。
        /// </remarks>
        public static void InvokeParallel(params Action[] actions)
        {
            ProgressWindow.Invoke((pw) =>
            {
                // 定義されているビジネスロジックを実行する
                ActionExecutor.InvokeParallel(actions);
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
