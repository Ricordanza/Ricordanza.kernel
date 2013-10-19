using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.Core.Collections
{
    /// <summary>
    /// バッファを物理的にリング状に配置することはできないので、インデックス（添え数）をバッファサイズで割って剰余を取る正規化をし、<br />
    /// 一定の範囲に限定することで、直線状のバッファの両端を論理的に繋げる。<br />
    /// 正規化により、インデックスがバッファの最後を超えると最初に戻り、また負数が適切に処理されていれば、バッファの最初より前になると最後に進む。<br />
    /// 正規化の内容は剰余演算だが、実際には、バッファサイズを2の冪に切り上げ、「バッファサイズ－1」とのビットごとの論理積を求めることが多い（ソースコードでは剰余のままであっても、<br />
    /// 現在のコンパイラの多くは、2の冪での剰余を自動的にビットごとの論理積に最適化する）。<br />
    /// ただしバッファサイズを切り上げると余分なメモリが必要になるため、<br />
    /// メモリ使用量の制約が強いときはバッファサイズを半端なままにしておき、一般的な方法で剰余を求めたり、バッファの端に達したかどうかで条件分岐したりする。<br />
    /// ただしこれらは、インデックスが0オフセット（始まりが0）の場合の話である。<br />
    /// 1オフセットなどオフセットがある場合は、0オフセットのインデックスに換算して正規化する必要がある。<br />
    /// リングバッファのインデックスは、数論的には剰余類をなす。
    /// </summary>
    /// <typeparam name="T">バッファの実態型</typeparam>
    public class RingBuffer<T>
        : IEnumerable<T>
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// バッファサイズ
        /// </summary>
        private int _size;
        
        /// <summary>
        /// バッファ
        /// </summary>
        private T[] _buffer;
        
        /// <summary>
        /// バッファの存在フラグ
        /// </summary>
        private bool[] _existence;

        /// <summary>
        /// 書き終わった位置
        /// </summary>
        private int _writeIndex;

        /// <summary>
        /// これから読み込む位置
        /// </summary>
        private int _readIndex;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        /// <param name="size">バッファサイズ</param>
        public RingBuffer(int size)
        {
            _size = size;
            _buffer = new T[size];
            _existence = new bool[size];
            _writeIndex = -1;
            _readIndex = 0;
        }

        #endregion

        #region property

        /// <summary>
        /// 存在するバッファ数を取得します。
        /// </summary>
        public int Count { get { return _existence.Count(b => b == true); } }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// バッファを追加します。
        /// </summary>
        /// <param name="value">追加したいバッファ</param>
        public void Add(T value)
        {
            _writeIndex = NextIndex(_writeIndex);
            _buffer[_writeIndex] = value;

            if (_existence[_readIndex] && _writeIndex == _readIndex)
                _readIndex = NextIndex(_readIndex);

            _existence[_writeIndex] = true;
        }

        /// <summary>
        /// バッファを取得します。
        /// </summary>
        /// <returns>取得したバッファ</returns>
        public T Get()
        {
            if (!Exists())
                throw new InvalidOperationException("There is no data in the buffer.");

            T val = _buffer[_readIndex];
            _existence[_readIndex] = false;
            _readIndex = NextIndex(_readIndex);

            return val;
        }

        /// <summary>
        /// バッファが存在するか判定します。
        /// </summary>
        /// <returns>バッファが存在する場合は<c>true</c>。それ以外の場合は<c>false</c></returns>
        public bool Exists()
        {
            return _existence.Any(b => b == true);
        }

        /// <summary>
        /// 全てのバッファをクリアします。
        /// </summary>
        public void Clear()
        {
            _writeIndex = -1;
            _readIndex = 0;

            for (int i = 0; i < _size; i++)
                _existence[i] = false;
        }

        /// <summary>
        /// ジェネリック コレクション反復子を取得します。
        /// </summary>
        /// <returns>ジェネリック コレクション反復子</returns>
        public IEnumerator<T> GetEnumerator()
        {
            while (Exists())
                yield return Get();
        }

        /// <summary>
        /// 非ジェネリック コレクションに対する単純な反復子を取得します。
        /// </summary>
        /// <returns>非ジェネリック コレクションに対する単純な反復子</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        /// <summary>
        /// 次のインデックスを取得します。
        /// </summary>
        /// <param name="ix">インデックス</param>
        /// <returns>次のインデックス</returns>
        private int NextIndex(int ix)
        {
            return ++ix % _size;
        }

        #endregion

        #region delegate

        #endregion
    }
}
