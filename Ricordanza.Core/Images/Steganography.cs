using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;

namespace Ricordanza.Core.Images
{
    /// <summary>
    /// カバーデータを埋め込んだステゴオブジェクトを操作します。
    /// </summary>
    /// <example>
    /// カバーデータ埋め込み<br />
    /// <code>
    /// var data = Encoding.UTF8.GetBytes("coverdata");
    /// using (var inStream = new FileStream(@"a.jpg", FileMode.Open, FileAccess.Read))
    ///     using (var outStream = new FileStream(@"b.jpg", FileMode.Create, FileAccess.Write))
    ///         Steganography.Encrypt(inStream, outStream, data);
    /// </code>
    /// カバーデータ取り出し<br />
    /// <code>
    /// using (var inStream = new FileStream(@"b.jpg", FileMode.Open, FileAccess.Read))
    /// {
    ///     var data = Steganography.Decode(inStream);
    /// }
    /// </code>
    /// </example>
    public static class Steganography
    {
        #region const

        /// <summary>
        /// ヘッダデータのサイズ
        /// </summary>
        const int HEADER_SIZE = 54;

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
        /// ステゴオブジェクトを作成します。
        /// </summary>
        /// <param name="inStream">ステゴオブジェクトの元となるストリーム</param>
        /// <param name="outStream">ステゴオブジェクトになるストリーム</param>
        /// <param name="data">カバーデータ</param>
        public static void Encrypt(Stream inStream, Stream outStream, byte[] data)
        {
            if (inStream == null)
                throw new ArgumentNullException("inStream is null.");

            // ビットマップファイルか判定する
            //char b = (char)inStream.ReadByte();
            //char m = (char)inStream.ReadByte();
            //if (!(b == 'B' && m == 'M'))
            //    throw new Exception("The file is not a bitmap!");

            // 24bit形式化判定する
            //byte[] buffer = new byte[2];
            //inStream.Seek(28, 0);
            //inStream.Read(buffer, 0, 2);
            //Int16 nBit = BitConverter.ToInt16(buffer, 0);
            //if (nBit != 24)
            //    throw new Exception("The file is not a 24bit bitmap!");

            // ヘッダデータを読み込む		
            int offset = HEADER_SIZE;
            byte[] header = new byte[offset];
            inStream.Seek(0, 0);
            inStream.Read(header, 0, offset);

            // ステゴオブジェクトにヘッダ情報を書き込む
            outStream.Write(header, 0, offset);

            // データの頭に4バイトの長さを加える
            byte[] message = AddLengthAhead(data);

            // カバーデータを書きこむ
            inStream.Seek(offset, 0);
            Encode(inStream, outStream, message);
        }

        /// <summary>
        /// ステゴオブジェクトからカバーデータを取得します。
        /// </summary>
        /// <param name="image">カバーデータを保持しているステゴオブジェクトのストリーム</param>
        /// <returns>カバーデータ</returns>
        public static byte[] Decode(Stream inStream)
        {
            // ヘッダデータを読み込む
            int offset = HEADER_SIZE;
            inStream.Seek(offset, 0);

            // 最初の4バイトを読む
            byte[] bLen = Decode(inStream, 4);
            int len = BitConverter.ToInt32(bLen, 0);

            // メセージを読み込む
            inStream.Seek(offset + 4 * 8, 0);

            byte[] buffer;

            try
            {
                buffer = Decode(inStream, len);
            }
            catch
            {
                throw new Exception("Wrong password or not a stego file!");
            }

            return buffer;
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        /// <summary>
        /// カバーデータを書き込みます。
        /// </summary>
        /// <param name="inStream">ステゴオブジェクトの元となるストリーム</param>
        /// <param name="outStream">ステゴオブジェクトになるストリーム</param>
        /// <param name="data">カバーデータ</param>
        private static void Encode(Stream inStream, Stream outStream, byte[] data)
        {
            int byteRead;
            byte byteWrite;
            int i = 0;
            int j = 0;
            byte bit;

            while ((byteRead = inStream.ReadByte()) != -1)
            {
                byteWrite = (byte)byteRead;

                if (i < data.Length)
                {
                    // 地点jのビットを抽出
                    bit = BitExtract(data[i], j++);

                    // ビットを入れ替える
                    BitReplace(ref byteWrite, 0, bit);

                    // 8ビット毎にdata用の1バイトを処理
                    if (j == 8) { j = 0; i++; }
                }

                outStream.WriteByte(byteWrite);
            }

            if (i < data.Length)
                throw new Exception("The cover is too small to contain the message to hide");
        }

        /// <summary>
        /// ステゴオブジェクトからカバーデータを取得します。
        /// </summary>
        /// <param name="inStream">カバーデータを保持しているステゴオブジェクトのストリーム</param>
        /// <param name="length">データ長</param>
        /// <returns>カバーデータ</returns>
        private static byte[] Decode(Stream inStream, int length)
        {
            byte[] data = new byte[length];
            int i = 0;
            int j = 0;
            byte bit;
            int byteRead;

            while ((byteRead = inStream.ReadByte()) != -1)
            {
                // ビットを抽出
                bit = BitExtract((byte)byteRead, 0);

                // 地点jのビットを置換
                BitReplace(ref data[i], j++, bit);

                // 8ビット毎にdata用の1バイトを処理
                if (j == 8) { j = 0; i++; }

                // 残されたバイトをチェック
                if (i == length) break;
            }

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static byte[] AddLengthAhead(byte[] message)
        {
            int len = message.Length;

            byte[] bLen = BitConverter.GetBytes(len);
            byte[] ret = new byte[len + bLen.Length];

            for (int i = 0; i < bLen.Length; i++)
                ret[i] = bLen[i];

            for (int i = 0; i < message.Length; i++)
                ret[i + bLen.Length] = message[i];

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="pos"></param>
        /// <param name="value"></param>
        private static void BitReplace(ref byte b, int pos, byte value)
        {
            b = (byte)(value == 1 ? b | (1 << pos) : b & ~(1 << pos));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private static byte BitExtract(byte b, int pos)
        {
            return (byte)((b & (1 << pos)) >> pos);
        }

        #endregion

        #region delegate

        #endregion
    }
}
