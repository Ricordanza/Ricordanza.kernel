using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace Ricordanza.ShapShot.Memento
{
    /// <summary>
    /// スナップショットを管理します。
    /// このクラスによりスナップショットのUndo、Redoが行なわれます。
    /// </summary>
    /// <typeparam name="T"><see cref="IMemento&lt;T&gt;"/> の実体型</param>
    /// <example>
    /// <code>
    /// public class User : IMemento&lt;User&gt;
    /// {
    ///     public string Name { set; get; }
    /// 
    ///     public IMemento&lt;User&gt;Restore(User target)
    ///     {
    ///         target.Name = this.Name;
    ///         return target;
    ///     }
    /// 
    ///     public IMemento&lt;User&gt; Clone()
    ///     {
    ///         return new User() { Name = this.Name };
    ///     }
    /// }
    /// 
    /// public class MementoSample
    /// {
    ///     protected User _user;
    ///     protected Caretaker&lt;User&gt; _caretaker;
    ///     
    ///     public MementoSample()
    ///     {
    ///         _user = new User();
    ///         _caretaker = new Caretaker&lt;User&gt;(_user);
    ///     }
    /// 
    ///     public void Do()
    ///     {
    ///         // Run Example
    ///         _user.Name = "田中";
    ///         Console.WriteLine(_user.Name);
    ///         _caretaker.Save(_user);
    /// 
    ///         _user.Name = "斉藤";
    ///         Console.WriteLine(_user.Name);
    /// 
    ///         _caretaker.Undo();
    ///         Console.WriteLine(_user.Name);
    ///     }
    /// }
    /// </code>
    /// </example>
    [Serializable]
    public class Caretaker<T> where T : IMemento<T>
    {
        # region const

        /// <summary>
        /// 履歴管理を行なうデフォルトサイズ
        /// </summary>
        private const int DEFAULT_CAPACITY = 100;

        #endregion

        #region private variable

        /// <summary>
        /// Undo、Redo中か判定するフラグ
        /// </summary>
        private bool inUndoRedo = false;

        #endregion

        #region protected variable

        /// <summary>
        /// 操作対象のオブジェクト
        /// </summary>
        protected T subject;

        /// <summary>
        /// Undo操作のスタック
        /// </summary>
        protected RoundStack<IMemento<T>> undoStack;

        /// <summary>
        /// Redo操作のスタック
        /// </summary>
        protected RoundStack<IMemento<T>> redoStack;

        #endregion

        #region property

        /// <summary>
        /// Undo、Redo中か判定します。
        /// </summary>
        public bool InUndoRedo
        {
            get { return inUndoRedo; }
        }

        /// <summary>
        /// Undo可能な回数を取得します。
        /// </summary>
        public int UndoCount
        {
            get { return undoStack.Count; }
        }

        /// <summary>
        /// Redo可能な回数を取得します。
        /// </summary>
        public int RedoCount
        {
            get { return redoStack.Count; }
        }

        /// <summary>
        /// Undo可能か判定します。
        /// </summary>
        public bool CanUndo
        {
            get { return (undoStack.Count != 0); }
        }

        /// <summary>
        /// Redo可能か判定します。
        /// </summary>
        public bool CanRedo
        {
            get { return (redoStack.Count != 0); }
        }

        #endregion

        #region constractor

        /// <summary>
        /// 新しい <see cref="Caretaker&lt;T&gt;"/> を構築します。
        /// </summary>
        /// <param name="subject">スナップショットの実体型</param>
        public Caretaker(T subject)
            : this(subject, DEFAULT_CAPACITY)
        {
        }

        /// <summary>
        /// 新しい <see cref="Caretaker&lt;T&gt;"/> を構築します。
        /// </summary>
        /// <param name="subject"> <see cref="IMemento&lt;T&gt;"/> の実体型</param>
        /// <param name="capacity">履歴の最大容量</param>
        public Caretaker(T subject, int capacity)
        {
            this.subject = subject;
            undoStack = new RoundStack<IMemento<T>>(capacity);
            redoStack = new RoundStack<IMemento<T>>(capacity);
        }

        #endregion

        #region public method

        /// <summary>
        /// ストックを行ないます。 
        /// </summary>
        /// <param name="m">ストックする <see cref="IMemento&lt;T&gt;"/></param>
        public void Save(IMemento<T> m)
        {
            if (inUndoRedo)
                throw new InvalidOperationException("Involking do within an undo/redo action.");

            _Do(m.Clone());
        }

        /// <summary>
        /// Undoスタックの上位の <see cref="IMemento&lt;T&gt;"/> の状態を反映します。
        /// </summary>
        /// <seealso cref="Redo()"/>
        public void Undo()
        {
            inUndoRedo = true;
            var top = undoStack.Pop();
            redoStack.Push(top.Restore(subject));

            inUndoRedo = false;
        }

        /// <summary>
        /// Redoスタックの上位の <see cref="IMemento&lt;T&gt;"/> の状態を反映します。
        /// </summary>
        /// <seealso cref="Undo()"/>
        public void Redo()
        {
            inUndoRedo = true;
            var top = redoStack.Pop();
            undoStack.Push(top.Restore(subject));

            inUndoRedo = false;
        }

        /// <summary>
        /// Undoスタック、Redoスタックを削除します。
        /// </summary>
        public void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
        }

        /// <summary>
        /// Undoスタックから <see cref="IMemento&lt;T&gt;"/> を削除する事無く取得します。
        /// </summary>
        /// <returns>Undoスタックの上位の <see cref="IMemento&lt;T&gt;"/> </returns>
        public IMemento<T> PeekUndo()
        {
            if (undoStack.Count > 0)
                return undoStack.Peek();
            else
                return null;
        }

        /// <summary>
        /// Redoスタックから <see cref="IMemento&lt;T&gt;"/> を削除する事無く取得します。
        /// </summary>
        /// <returns>Redoスタックの上位の <see cref="IMemento&lt;T&gt;"/> </returns>
        public IMemento<T> PeekRedo()
        {
            if (redoStack.Count > 0)
                return redoStack.Peek();
            else
                return null;
        }

        #endregion

        #region private method

        /// <summary>
        /// <see cref="IMemento&lt;T&gt;"/> をUndoスタックに追加します。
        /// </summary>
        /// <param name="m">ストックする <see cref="IMemento&lt;T&gt;"/><</param>
        private void _Do(IMemento<T> m)
        {
            redoStack.Clear();
            undoStack.Push(m);
        }

        #endregion
    }
}