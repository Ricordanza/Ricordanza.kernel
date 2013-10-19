using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Ricordanza.Core;
using Ricordanza.Core.Utilities;

namespace Ricordanza.Core.Utilities
{
    /// <summary>
    /// Control操作のユーティリティクラスです。
    /// </summary>
    /// <remarks>このクラスは<c>System.Windows.Forms.Control</c>を拡張します。</remarks>
    public static class ControlUtility
    {
        #region constant

        #endregion

        #region private variable

        #endregion

        #region public method

        /// <summary>
        /// コントロールの描画を中断します。
        /// </summary>
        /// <param name="self">コントロール</param>
        public static void SuspendDrawing(this Control self)
        {
            // 状態判定
            EffectiveInstance(self);

            SendMessage(self.Handle, WindowMessages.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// コントロールの描画を中断し、処理を実行します。
        /// このメソッドは<c>ResumeDrawing</c>を内部で実行する為、
        /// 別途<c>ResumeDrawing</c>を呼び出す必要はありません。
        /// </summary>
        /// <param name="self">コントロール</param>
        /// <param name="action">処理</param>
        /// <param name="cursor">カーソル変更</param>
        public static void SuspendDrawing(this Control self, MethodInvoker action, bool cursor = false)
        {
            // 状態判定
            EffectiveInstance(self);

            try
            {
                if (cursor)
                    self.Cursor = Cursors.WaitCursor;

                SuspendDrawing(self);
                (action ?? (() => { }))();
            }
            finally
            {
                ResumeDrawing(self);

                if (cursor)
                    self.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// コントロールの描画を中断し、処理を実行します。
        /// このメソッドは<c>SuspendLayout</c>、<c>ResumeLayout</c>を内部で実行する為、
        /// 別途<c>ResumeLayout</c>を呼び出す必要はありません。
        /// </summary>
        /// <param name="self">コントロール</param>
        /// <param name="action">処理</param>
        /// <param name="cursor">カーソル変更</param>
        public static void Suspend(this Control self, MethodInvoker action, bool cursor = false)
        {
            // 状態判定
            EffectiveInstance(self);

            try
            {
                if (cursor)
                    self.Cursor = Cursors.WaitCursor;

                self.SuspendLayout();
                (action ?? (() => { }))();
            }
            finally
            {
                self.ResumeLayout();

                if (cursor)
                    self.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// コントロールの描画を再開します。
        /// </summary>
        /// <param name="control">コントロール</param>
        public static void ResumeDrawing(this Control self)
        {
            // 状態判定
            EffectiveInstance(self);

            SendMessage(self.Handle, WindowMessages.WM_SETREDRAW, new IntPtr(0x0001), IntPtr.Zero);
            self.Refresh();
        }

        /// <summary>
        /// 親コンテナにスクロールのメッセージを送信します。
        /// </summary>
        /// <param name="self">コントロール</param>
        /// <param name="message">メッセージ</param>
        public static void SendScrollMessageToParent(this Control self, ref Message message)
        {
            // 状態判定
            EffectiveInstance(self);

            int beforeScrollPosiotion = GetScrollPos(self.Handle, WindowMessages.SB_VERT);

            // マウスホイールのメッセージではない場合、リターンする。
            if (message.Msg != WindowMessages.WM_MOUSEWHEEL)
                return;

            // 親コンテナにパネル、またはフォームが存在しない場合、リターンする。
            Control parent = GetParentPanelOrForm(self.Parent);
            if (parent == null)
                return;

            // スクロール処理後にスクロール位置が変化した場合、リターンする。
            if (beforeScrollPosiotion != GetScrollPos(self.Handle, WindowMessages.SB_VERT))
                return;

            // 親コンテナにスクロールのメッセージを送信する。
            SendMessage(parent.Handle, WindowMessages.WM_MOUSEWHEEL, message.WParam, message.LParam);
        }

        /// <summary>
        /// コントロールのイベントログを出力します。
        /// </summary>
        /// <param name="self">コントロール</param>
        /// <param name="action">イベントアクション</param>
        public static void Logging(this Control self, MethodInvoker action)
        {
            Logging(self, action, null, null);
        }

        /// <summary>
        /// コントロールのイベントログを出力します。
        /// </summary>
        /// <param name="self">コントロール</param>
        /// <param name="action">イベントアクション</param>
        /// <param name="before">事前アクション</param>
        /// <param name="after">事後アクション</param>
        public static void Logging(this Control self, MethodInvoker action, MethodInvoker before, MethodInvoker after)
        {
            // 状態判定
            EffectiveInstance(self);

            if (action == null)
                throw new ArgumentNullException("action is null.");

#if DEBUG
            Debug.WriteLine("{0}_{1} start.".DirectFormat(self.Name, action.Method.RealName()));
#endif
            SuspendDrawing(self,
                () =>
                {
                    (before ?? (() => { }))();
                    action();
                    (after ?? (() => { }))();
                }
            );

#if DEBUG
            Debug.WriteLine("{0}_{1} end.".DirectFormat(self.Name, action.Method.RealName()));
#endif
        }

        /// <summary>
        /// 指定された親に存在する対象の型のコントロールを取得します。
        /// </summary>
        /// <typeparam name="T">取得対象のコントロール型。</typeparam>
        /// <param name="self">親にあたるコントロール。</param>
        /// <param name="deep">すべての子コントロールを検索する場合は true。それ以外の場合は false。</param>
        /// <returns>取得対象コントロール配列。</returns>
        public static T[] FindByType<T>(this Control self, bool searchAllChildren = true) where T : Control
        {
            // 状態判定
            EffectiveInstance(self);

            List<T> buf = new List<T>();
            foreach (Control c in self.Controls)
            {
                T cb = c as T;
                if (cb != null)
                    buf.Add(cb);

                if (searchAllChildren)
                    buf.AddRange(FindByType<T>(c, searchAllChildren));
            }
            return buf.ToArray();
        }

        /// <summary>
        /// action実行時に対象の<see cref="System.Windows.Forms.Form"/>を一時的に非表示にします。
        /// </summary>
        /// <param name="self">一時的に非表示にしたい<see cref="System.Windows.Forms.Form"/></param>
        /// <param name="action">非表示中に実行したいaction</param>
        public static void HideAndShow(this Form self, MethodInvoker action)
        {
            // 状態判定
            EffectiveInstance(self);

            try
            {
                self.Hide();
                (action ?? (() => { }))();
            }
            finally
            {
                self.Visible = true;
            }
        }

        /// <summary>
        /// 対象コントロールの角を丸くします。
        /// </summary>
        /// <param name="self">角を丸くしたいコントロール。</param>
        /// <param name="radius">丸める半径。</param>
        /// <remarks>
        /// 事前に対象コントロールの枠を無くしておくなど、
        /// コントロール毎の個別対応が必要になります。
        /// あまりきれいには表示出来ないのです。
        /// </remarks>
        public static void SetRoundRect(this Control self, float radius)
        {
            // 状態判定
            EffectiveInstance(self);

            int width = self.Width;
            int height = self.Height;

            using (System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath())
            {
                // Line
                gp.AddLine(radius, 0, width - (radius * 2), 0);
                // Corner
                gp.AddArc(width - (radius * 2), 0, radius * 2, radius * 2, 270, 90);
                // Line
                gp.AddLine(width, radius, width, height - (radius * 2));
                // Corner
                gp.AddArc(width - (radius * 2), height - (radius * 2), radius * 2, radius * 2, 0, 90);
                // Line
                gp.AddLine(width - (radius * 2), height, radius, height);
                // Corner
                gp.AddArc(0, height - (radius * 2), radius * 2, radius * 2, 90, 90);
                // Line
                gp.AddLine(0, height - (radius * 2), 0, radius);
                // Corner
                gp.AddArc(0, 0, radius * 2, radius * 2, 180, 90);

                gp.CloseFigure();
                self.Region = new Region(gp);
            }
        }

        /// <summary>
        /// コントロールのキャプチャを取得します。
        /// </summary>
        /// <param name="self">キャプチャを取得したいコントロール</param>
        /// <returns>取得したキャプチャ</returns>
        public static Bitmap Capture(this Control self)
        {
            // 状態判定
            EffectiveInstance(self);

            Bitmap img = new Bitmap(self.Width, self.Height);
            using (Graphics memg = Graphics.FromImage(img))
            {
                IntPtr dc = memg.GetHdc();
                PrintWindow(self.Handle, dc, 0);
                memg.ReleaseHdc(dc);
            }
            return img;
        }

        /// <summary>
        /// コントロールに透かし文字を描画します。
        /// </summary>
        /// <param name="self">透かし文字を表示したいコントロール</param>
        /// <param name="font">コントロールが使用しているフォント</param>
        /// <param name="waterMark">透かし文字</param>
        /// <param name="waterMarkColor">透かし文字の色</param>
        /// <param name="location">透かし文字の描画座標</param>
        public static void DrawWaterMark(this Control self, Font font, string waterMark, Color waterMarkColor, Point location)
        {
            // 状態判定
            EffectiveInstance(self);

            // 透かし文字が定義されていない場合は無視
            if (waterMark.IsEmpty())
                return;

            // 描画を確定させる
            self.Update();

            // 透かし文字を描画
            using (Graphics g = self.CreateGraphics())
                g.DrawString(waterMark, new Font(font.FontFamily, font.Size, FontStyle.Italic), new SolidBrush(waterMarkColor), location);
        }

        /// <summary>
        /// クラスによって実装される場合に、コンポーネントがデザインモードかどうかを判断します。
        /// </summary>
        /// <param name="self">デザインモードか判定したいコンポーネント</param>
        /// <returns>コンポーネントがデザインモードの場合は true。それ以外の場合は false。</returns>
        /// <remarks>
        /// 入れ子になったユーザコントロールはDesignModeを正しく取得できない為、拡張メソッドで親要素を回帰的にチェック。
        /// </remarks>
        public static bool IsDesignMode(this Control self)
        {
            while (self != null)
            {
                if (self.Site != null && self.Site.DesignMode)
                    return true;
                self = self.Parent;
            }
            return false;
        }

        /// <summary>
        /// Invokeが必要か判断しアクションを実行します。
        /// </summary>
        /// <param name="self">コントロール</param>
        /// <param name="action">アクション</param>
        /// <remarks></remarks>
        public static void InvokeAction(this Control self, MethodInvoker action)
        {
            // 状態判定
            EffectiveInstance(self);

            action = (action ?? (() => { }));

            if (self.InvokeRequired)
                self.Invoke(action);
            else
                action();
        }

        /// <summary>
        /// 管理者権限が必要になるボタンにUAC(盾)のアイコンを表示する。
        /// </summary>
        /// <param name="self">UAC(盾)のアイコンを表示したいボタン</param>
        /// <param name="shield">アプリケーションが管理者権限で起動されている場合は<c>true</c>。それ以外の場合は<c>false</c></param>
        public static void AddUacShield(this Button self, bool shield)
        {
            // 状態判定
            EffectiveInstance(self);

            // 第2パラメータ：盾アイコンを設定するフラグ
            uint BCM_SETSHIELD = 0x0000160C;

            // ボタンの外観を「System」に変更
            self.FlatStyle = FlatStyle.System;

            if (shield)
                SendMessage(self.Handle, BCM_SETSHIELD, new IntPtr(0), new IntPtr(1));
            else
                SendMessage(self.Handle, BCM_SETSHIELD, new IntPtr(0), new IntPtr(0));
        }

        /// <summary>
        /// コントロールのインスタンスが破棄されているか確認します。
        /// </summary>
        /// <param name="self">破棄済みか確認したいコンポーネント</param>
        /// <returns>破棄済みの場合は<c>true</c>、それ以外の場合は<c>false</c></returns>
        public static bool IsDestroyed(this Control self)
        {
            return self == null || self.IsDisposed;
        }

        #endregion

        #region private method

        /// <summary>
        /// 1つまたは複数のウィンドウへ、指定されたメッセージを送信します。
        /// </summary>
        /// <param name="hWnd">送信先ウィンドウのハンドル</param>
        /// <param name="Msg">メッセージ</param>
        /// <param name="wParam">メッセージの最初のパラメータ</param>
        /// <param name="lParam">メッセージの 2 番目のパラメータ</param>
        /// <returns>メッセージ処理の結果が返ります</returns>
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 指定したスクロールバーの中のスクロールボックス（つまみ）の現在の位置を取得します。
        /// </summary>
        /// <param name="hWnd">ウィンドウのハンドル</param>
        /// <param name="nBar">スクロールバーのオプション</param>
        /// <returns>戻り値</returns>
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern Int32 GetScrollPos(IntPtr hWnd, Int32 nBar);

        /// <summary>
        /// 表示されているウィンドウを指定したデバイス コンテキストにコピーします。
        /// </summary>
        /// <param name="hwnd">ウィンドウのハンドル</param>
        /// <param name="hDC">デバイス コンテキストを識別するハンドル</param>
        /// <param name="nFlags">描画オプションを指定します</param>
        /// <returns>正常終了した場合はtrueを返します。それ以外の場合はfalseを返します。</returns>
        [DllImport("User32")]
        private extern static bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        /// <summary>
        /// 直近の親コンテナであるパネル、グループボックス、またはフォームを取得します。
        /// </summary>
        /// <param name="child">コントロール</param>
        /// <returns>取得したパネル、またはフォーム</returns>
        private static ScrollableControl GetParentPanelOrForm(Control child)
        {
            // 状態判定
            EffectiveInstance(child);

            // コントロールがパネル、またはフォームの場合、コントロールをリターンする。
            if (child is Panel || child is Form || child is GroupBox)
                return child as ScrollableControl;

            return GetParentPanelOrForm(child.Parent);
        }

        /// <summary>
        /// コントロールのインスタンスが有効かチェックします。
        /// </summary>
        /// <param name="control">インスタンスをチェックしたいコントロール</param>
        private static void EffectiveInstance(Control control)
        {
            if (control.IsDestroyed())
                throw new ArgumentNullException("instance is null or disposed.");
        }

        #endregion
    }
}
