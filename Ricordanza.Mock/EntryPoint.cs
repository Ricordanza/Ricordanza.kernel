using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Windows.Forms;

using Ricordanza.Core.Utilities;
using Ricordanza.Log;

namespace Ricordanza.Mock
{
    /// <summary>
    /// エントリーポイント
    /// </summary>
    static class EntryPoint
    {
        /// <summary>
        /// Windowの表示
        /// </summary>
        /// <param name="hWnd">ウィンドウハンドル</param>
        /// <param name="nCmdShow">表示状態</param>
        /// <returns>ウィンドウが以前に表示されていた場合は 0 以外の値、ウィンドウが以前に表示されていなかった場合は 0</returns>
        [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
        static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// 前面表示
        /// </summary>
        /// <param name="hWnd">ウィンドウハンドル</param>
        /// <returns>ウィンドウがフォアグラウンドになった場合は 0 以外の値、ウィンドウがフォアグラウンドにならなかった場合は 0</returns>
        [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 標準
        /// </summary>
        const int SW_NORMAL = 1;

        /// <summary>
        /// 多重起動を防止するMutex
        /// </summary>
        static Mutex mutex;

        /// <summary>
        /// アプリケーション名
        /// </summary>
        static string APP_NAME = Application.ProductName;

        /// <summary>
        /// Logger
        /// </summary>
        static readonly ILog _log = LogPool.GetLog();

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // 多重起動制御のMutexを生成する
            mutex = new Mutex(false, APP_NAME);

            // Mutexのシグナルを受信できないか判断する
            if (!mutex.WaitOne(0, false))
            {
                // プロダクト名と一致するプロセスを取得し、前面にする
                foreach(var processes in Process.GetProcessesByName(APP_NAME))
                {
                    ShowWindow(processes.MainWindowHandle, SW_NORMAL);
                    SetForegroundWindow(processes.MainWindowHandle);

                    break;
                }

                return;
            }

            // Windowsフォームの未処理例外のハンドル定義
            Application.ThreadException += new ThreadExceptionEventHandler(ThreadExceptionEvent);
            // 未処理例外のハンドル定義
            Thread.GetDomain().UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionEvent);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // アプリケーション実行
            try
            {
                Application.Run(new MockForm());
            }
            finally
            {
                // テンポラリフォルダの一時Excelファイルを削除します。
                SystemUtility.RemoveExcelTemp();
            }

            // ガベージ コレクション対象から除外
            GC.KeepAlive(mutex);

            // Mutexを閉じる
            mutex.Close();
        }

        /// <summary>
        /// 未処理例外をキャッチするイベントハンドラ（Windowsアプリケーション用）
        /// </summary>
        /// <param name="sender">イベントソース</param>
        /// <param name="e">イベントデータを格納している<see cref="System.Threading.ThreadExceptionEventArgs"/>。</param>
        static void ThreadExceptionEvent(object sender, ThreadExceptionEventArgs e)
        {
            ErrorProcess(e.Exception);
        }

        /// <summary>
        /// 未処理例外をキャッチするイベントハンドラ（主にコンソールアプリケーション用）
        /// </summary>
        /// <param name="sender">イベントソース</param>
        /// <param name="e">イベントデータを格納している<see cref="System.UnhandledExceptionEventArgs"/>。</param>
        static void UnhandledExceptionEvent(object sender, UnhandledExceptionEventArgs e)
        {
            ErrorProcess(e.ExceptionObject as Exception);
        }

        /// <summary>
        /// 例外発生時の共通処理を実行
        /// </summary>
        /// <param name="ex">Exceptionオブジェクト</param>
        static void ErrorProcess(Exception ex)
        {
            if (ex != null)
                _log.Fatal(ex);

            // システムの終了
            Environment.Exit(-1);
        }
    }
}

