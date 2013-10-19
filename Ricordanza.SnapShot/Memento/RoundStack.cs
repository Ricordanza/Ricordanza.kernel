using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Ricordanza.ShapShot.Memento
{
    /// <summary>
    /// オブジェクトは先頭に追加し保持します。
    /// 最大容量を超えた下部項目は捨てられます。
    /// </summary>
    /// <typeparam name="T">保持したいオブジェクトの実体型</typeparam>
    [Serializable]
    public class RoundStack<T>
    {
        #region private variable

        /// <summary>
        /// オブジェクトをを保持します。
        /// </summary>
        private T[] items;

        /// <summary>
        /// 上位
        /// </summary>
        private int top = 1;

        /// <summary>
        /// 下位
        /// </summary>
        private int bottom = 0;

        #endregion

        #region property

        /// <summary>
        /// 最大容量か判定します。
        /// </summary>
        public bool IsFull
        {
            get
            {
                return top == bottom;
            }
        }

        /// <summary>
        /// 現在のオブジェクト数を取得します。
        /// </summary>
        public int Count
        {
            get
            {
                int count = top - bottom - 1;
                if (count < 0)
                    count += items.Length;
                return count;
            }
        }

        /// <summary>
        /// 最大容量を取得します。
        /// </summary>
        public int Capacity
        {
            get
            {
                return items.Length - 1;
            }
        }

        #endregion

        #region constractor

        /// <summary>
        /// 新しい <see cref="RoundStack&lt;T&gt;"/> を構築します。
        /// </summary>
        /// <param name="capacity">最大容量</param>
        public RoundStack(int capacity)
        {
            if (capacity < 1)
                throw new ArgumentOutOfRangeException("Capacity need to be at least 1");

            items = new T[capacity + 1];
        }

        #endregion

        #region public method

        /// <summary>
        /// 先端にあるオブジェクトを取り除いて返します。
        /// </summary>
        /// <returns>先頭にあるオブジェクト</returns>
        public T Pop()
        {
            if (Count > 0)
            {
                T removed = items[top];
                items[top--] = default(T);
                if (top < 0)
                    top += items.Length;
                return removed;
            }
            else
                throw new InvalidOperationException("Cannot pop from emtpy stack");
        }

        /// <summary>
        /// 先端にオブジェクトを挿入します。
        /// </summary>
        /// <param name="item">オブジェクト</param>
        public void Push(T item)
        {
            if (IsFull)
            {
                bottom++;
                if (bottom >= items.Length)
                    bottom -= items.Length;
            }
            if (++top >= items.Length)
                top -= items.Length;

            if (item is ICloneable)
            {
                ICloneable cloneable = item as ICloneable;
                items[top] = (T) cloneable.Clone();
            }
            else
                items[top] = item;
        }

        /// <summary>
        /// 先端のオブジェクトを返します。
        /// このメソッドは先頭のオブジェクトを削除しません。
        /// </summary>
        public T Peek()
        {
            return items[top];
        }

        /// <summary>
        /// 全てのオブジェクトを削除します。
        /// </summary>
        public void Clear()
        {
            if (Count > 0)
            {
                for (int i = 0; i < items.Length; i++)
                    items[i] = default(T);

                top = 1;
                bottom = 0;
            }
        }

        #endregion
    }
}
