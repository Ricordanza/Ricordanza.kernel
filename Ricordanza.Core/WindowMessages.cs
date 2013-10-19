using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ricordanza.Core
{
    /// <summary>
    /// WindowMessage定数クラスです。
    /// </summary>
    public static class WindowMessages
    {
        /// <summary>
        /// 非システムキーが押された場合のメッセージ
        /// </summary>
        public const int WM_KEYDOWN = 0x100;

        /// <summary>
        /// キー操作が非システムキャラクタに変換されたときのメッセージ
        /// </summary>
        public const int WM_CHAR = 0x102;

        /// <summary>
        /// エディットコントロールまたはコンボボックスの現在のキャレットの位置にクリップボードの内容をコピーするときのメッセージ
        /// </summary>
        public const int WM_PASTE = 0x302;

        /// <summary>
        /// ウィンドウのクライアント領域を再描画する必要があるときに発行されるメッセージ
        /// </summary>
        public const int WM_PAINT = 0x000F;

        /// <summary>
        /// CreateWindow() または
        /// CreateWindowEx() 呼び出し要求
        /// </summary>
        public const int WM_CREATE = 0x0001;

        /// <summary>
        /// ウィンドウの破棄要求
        /// </summary>
        public const int WM_DESTROY = 0x0002;

        /// <summary>
        /// ウィンドウ、またはアプリケーション終了要求
        /// </summary>
        public const int WM_CLOSE = 0x0010;

        /// <summary>
        /// ウィンドウの背景を消去する必要があるとき(ウィンドウがサイズ変更されたときなど) に送られる要求
        /// </summary>
        public const int WM_ERASEBKGND = 0x14;

        /// <summary>
        /// セッション終了要求
        /// </summary>
        public const int WM_ENDSESSION = 0x16;

        /// <summary>
        /// システムメニュー（コントロールメニュー）のアイテムが選択された場合や、ウィンドウ右上の「最大化」「最小化」「元のサイズに戻す」「閉じる」ボタンが押された場合に、ウィンドウに送信される要求。
        /// </summary>
        public const int WM_SYSCOMMAND = 0x112;

        /// <summary>
        /// コントロールの描画を制御するメッセージを表します。
        /// </summary>
        public const int WM_SETREDRAW = 0x000B;

        /// <summary>
        /// マウスホイールのメッセージを表します。
        /// </summary>
        public const int WM_MOUSEWHEEL = 0x020A;

        /// <summary>
        /// 垂直スクロールバーのオプションを表します。
        /// </summary>
        public const int SB_VERT = 0x0001;

        /// <summary>
        /// ウィンドウを閉じる
        /// </summary>
        public const int SC_CLOSE = 0xF060;

        /// <summary>
        /// ウインドウのサイズを元に戻すWParam
        /// </summary>
        public const int SC_RESTORE = 0xf120;

        /// <summary>
        /// ウインドウを最小化するWParam
        /// </summary>
        public const int SC_MINIMIZE = 0xf020;

        /// <summary>
        /// ウインドウを最大化するWParam
        /// </summary>
        public const int SC_MAXIMIZE = 0xf030;
    }
}
