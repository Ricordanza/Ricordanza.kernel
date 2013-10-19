using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.SnapShot.Command
{
    /// <summary>
    /// スナップショットを管理します。
    /// このクラスによりスナップショットのUndo、Redoが行なわれます。
    /// </summary>
    /// <example>
    /// <code>
    /// public class User
    /// {
    ///     public string Name { set; get; }
    /// }
    /// 
    /// public sealed class UserHistory : CommandHistory&lt;string, User&gt;
    /// {
    ///     public UserHistory(string data, User target)
    ///     {
    ///         base.Data = data;
    ///         base.Target = target;
    ///     }
    /// 
    ///     public override void SetMemento(string data)
    ///     {
    ///         base.Data = data;
    ///         base.Target.Name = data;
    ///     }
    /// }
    /// 
    /// public class CommandSample
    /// {
    ///     protected User _user;
    ///     protected CommandHistory&lt;string, User&gt; _memento;
    ///     protected Caretaker _caretaker;
    /// 
    ///     public CommandSample()
    ///     {
    ///         _user = new User();
    ///         _memento = new UserHistory(_user.Name, _user);
    ///         _caretaker = new Caretaker();
    ///     }
    /// 
    ///     public void Do()
    ///     {
    ///         // Run Example
    ///         _user.Name = "田中";
    ///         Save();
    ///         Console.WriteLine(_user.Name);
    /// 
    ///         _user.Name = "斉藤";
    ///         Save();
    ///         Console.WriteLine(_user.Name);
    /// 
    ///         Undo();
    ///         Console.WriteLine(_user.Name);
    /// 
    ///         _user.Name = "井上";
    ///         Save();
    ///         Console.WriteLine(_user.Name);
    /// 
    ///         _user.Name = "加藤";
    ///         Save();
    ///         Console.WriteLine(_user.Name);
    /// 
    ///         Undo();
    ///         Console.WriteLine(_user.Name);
    ///     }
    /// 
    ///     protected void Save()
    ///     {
    ///         var current = new UserHistory(_user.Name, _user);
    ///         var cmd = new CommandAction&lt;string, User&gt;(_memento, current);
    ///         if (!_caretaker.Save(cmd))
    ///         {
    ///             MessageBox.Show("状態の最大保存数を超えました。");
    ///             return;
    ///         }
    ///         _memento = current;
    ///     }
    /// 
    ///     protected void Refresh()
    ///     {
    ///         _caretaker.Refresh();
    ///     }
    /// 
    ///     protected void Undo()
    ///     {
    ///         _caretaker.Undo();
    ///     }
    /// 
    ///     protected void Redo()
    ///     {
    ///         _caretaker.Redo();
    ///     }
    /// }
    /// </code>
    /// </example>
    public class Caretaker
    {
        #region const

        /// <summary>
        /// 履歴管理を行なうデフォルトサイズ
        /// </summary>
        private const int DEFAULT_CAPACITY = 100;

        #endregion

        #region private variable

        /// <summary>
        /// 履歴の最大容量
        /// </summary>
        private int _maxStack = int.MaxValue;

        /// <summary>
        /// Undo操作のスタック
        /// </summary>
        private Stack<ICommand> _undoStack;

        /// <summary>
        /// Redo操作のスタック
        /// </summary>
        private Stack<ICommand> _redoStack;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// 新しい <see cref="Ricordanza.SnapShot.Command.Caretaker"/> を構築します。
        /// </summary>
        public Caretaker()
            : this(DEFAULT_CAPACITY)
        {
        }

        /// <summary>
        /// 新しい <see cref="Ricordanza.SnapShot.Command.CommandHistoryManager"/> を構築します。
        /// </summary>
        /// <param name="maxStack">履歴の最大容量</param>
        public Caretaker(int maxStack)
            : base()
        {
            _maxStack = maxStack;
            _undoStack = new Stack<ICommand>();
            _redoStack = new Stack<ICommand>();
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
        /// ストックを行ないます
        /// </summary>
        /// <param name="command">コマンド</param>
        public bool Save(ICommand command)
        {
            if (_undoStack.Count >= _maxStack)
                return false;

            command.Save();

            _redoStack.Clear();
            _undoStack.Push(command);
            return true;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        public void Undo()
        {
            if (_undoStack.Count == 0)
                return;

            var command = _undoStack.Pop();
            command.Undo();

            _redoStack.Push(command);
        }

        /// <summary>
        /// やり直し
        /// </summary>
        public void Redo()
        {
            if (_redoStack.Count == 0)
                return;

            var command = _redoStack.Pop();
            command.Redo();

            _undoStack.Push(command);
        }

        /// <summary>
        /// 最後の履歴状態に戻す
        /// </summary>
        public void Refresh()
        {
            if (_undoStack.Count == 0)
                return;

            var command = _undoStack.Peek();
            command.Redo();
        }

        /// <summary>
        /// Undoスタック、Redoスタックを削除します。
        /// </summary>
        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
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
