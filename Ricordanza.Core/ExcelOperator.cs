using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Office.Interop.Excel;

using Ricordanza.Core.Utilities;

namespace Ricordanza.Core
{
    /// <summary>
    /// Excel操作のユーティリティクラスです。
    /// </summary>
    /// <example>
    /// <code>
    /// ExcelOperator.Invoke("xxx.xls", e =>
    /// {
    /// 	e.CurrentSheetIndex = 0;
    /// 	e[1, 1] = "123";
    /// 	e.Save();
    /// });
    /// </code>
    /// </example>
    public sealed class ExcelOperator : IDisposable
    {
        #region constant

        /// <summary>
        /// セル内の改行コード
        /// </summary>
        private const char CELL_LINEBREAK = '\n';

        /// <summary>
        /// テンポラリファイル名のフォーマット
        /// </summary>
        private const string TEMPFILENAME_FORMAT = "{0}_{1}_{2}.xls";

        /// <summary>
        /// 標準のセル表示形式
        /// </summary>
        private const string NUMBER_FORMAT_LOCAL_STANDARD = "G/標準";

        #endregion

        #region private variable

        /// <summary>
        /// 操作中のExcelのインスタンス
        /// </summary>
        private Application xlApplication;

        /// <summary>
        /// 操作中のワークシート
        /// </summary>
        private Workbook workbook;

        /// <summary>
        /// 操作中の複数シート
        /// </summary>
        private Sheets sheets;

        /// <summary>
        /// 操作中のシート
        /// </summary>
        private Worksheet sheet;

        /// <summary>
        /// 操作中のコピーシート
        /// </summary>
        private Worksheet copySheet;

        /// <summary>
        /// 範囲
        /// </summary>
        private Range range;

        /// <summary>
        /// 操作対象のシートのインデックス
        /// </summary>
        private int currentSheetIndex;

        /// <summary>
        /// コピー元ファイルパス
        /// </summary>
        private string source;

        /// <summary>
        /// コピー先ファイルパス
        /// </summary>
        private string dist;

        #endregion

        #region static constractor

        #endregion

        #region constractor

        /// <summary>
        /// 隠蔽化したコンストラクタ
        /// </summary>
        private ExcelOperator()
            : base()
        {
            xlApplication = null;
            workbook = null;
            sheets = null;
            sheet = null;
            copySheet = null;
            range = null;
            currentSheetIndex = -1;
            source = string.Empty;
            dist = string.Empty;
        }

        /// <summary>
        /// 操作するExcelを引数にこのクラスのオブジェクトを構築します。
        /// </summary>
        /// <param name="path">操作するExcelファイルの絶対パス</param>
        private ExcelOperator(string path)
            : this()
        {
            FileInfo info = new FileInfo(path);
            if (!info.Exists)
                throw new FileNotFoundException(Properties.Resources.MSG001.DirectFormat(path), path);
            else if (info.IsReadOnly)
                throw new FileLoadException(Properties.Resources.MSG002.DirectFormat(path), path);

            // 引数をメンバ変数に展開
            this.source = path;

            // テンポラリファイルを作成
            CreateTempFile();

            // Excelを開く
            xlApplication = CreateExcelApplication();
            workbook = xlApplication.Workbooks.Open(dist, ReadOnly: false);
        }

        #endregion

        #region property

        /// <summary>
        /// 操作対象のシートのインデックスを設定／取得します。
        /// </summary>
        public int CurrentSheetIndex
        {
            set
            {
                currentSheetIndex = value;
                this.sheet = workbook.Sheets[currentSheetIndex] as Worksheet;
            }
            get
            {
                return currentSheetIndex;
            }
        }

        /// <summary>
        /// セルの値の設定／取得を行います。
        /// シート、行、列のインデックスは1始まりです。
        /// </summary>
        /// <param name="row">行インデックス</param>
        /// <param name="column">列インデックス</param>
        /// <returns>セルの値</returns>
        public object this[int row, int column]
        {
            set
            {
                this.sheet = workbook.Sheets[currentSheetIndex] as Worksheet;
                range = this.sheet.Cells[row, column] as Range;

                if (range == null)
                    return;

                range.Value = value;
            }
            get
            {
                this.sheet = workbook.Sheets[currentSheetIndex] as Worksheet;
                range = this.sheet.Cells[row, column] as Range;

                if (range == null)
                    return string.Empty;

                return range.Value;
            }
        }

        /// <summary>
        /// セルの値の設定／取得を行います。
        /// シートのインデックスは1始まりです。
        /// </summary>
        /// <param name="cell">セル名"A1"など</param>
        /// <returns>セルの値</returns>
        public object this[string cell]
        {
            set
            {
                this.sheet = workbook.Sheets[currentSheetIndex] as Worksheet;
                range = this.sheet.Cells.get_Range(cell, Type.Missing);

                if (range == null)
                    return;

                range.Value = value;
            }
            get
            {
                this.sheet = workbook.Sheets[currentSheetIndex] as Worksheet;
                range = this.sheet.get_Range(cell, Type.Missing);

                if (range == null)
                    return string.Empty;

                return range.Value;
            }
        }

        /// <summary>
        /// シート数を取得します。
        /// </summary>
        public int SheetCount
        {
            get
            {
                this.sheets = workbook.Sheets;
                return this.sheets.Count;
            }
        }

        /// <summary>
        /// ページヘッダの左側の値を設定します。
        /// </summary>
        public string LeftHeader
        {
            set
            {
                this.sheet.PageSetup.LeftHeader = value;
            }
        }

        /// <summary>
        /// ページヘッダの中央の値を設定します。
        /// </summary>
        public string CenterHeader
        {
            set
            {
                this.sheet.PageSetup.CenterHeader = value;
            }
        }

        /// <summary>
        /// ページヘッダの右側の値を設定します。
        /// </summary>
        public string RightHeader
        {
            set
            {
                this.sheet.PageSetup.RightHeader = value;
            }
        }

        /// <summary>
        /// ページフッタの左側の値を設定します。
        /// </summary>
        public string LeftFooter
        {
            set
            {
                this.sheet.PageSetup.LeftFooter = value;
            }
        }

        /// <summary>
        /// ページフッタの中央の値を設定します。
        /// </summary>
        public string CenterFooter
        {
            set
            {
                this.sheet.PageSetup.CenterFooter = value;
            }
        }

        /// <summary>
        /// ページフッタの右側の値を設定します。
        /// </summary>
        public string RightFooter
        {
            set
            {
                this.sheet.PageSetup.RightFooter = value;
            }
        }

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// Excel生成処理が必要な処理のアクションを実行します。
        /// </summary>
        /// <param name="path">操作するExcelファイルの絶対パス</param>
        /// <param name="action">Excel出力処理</param>
        public static void Invoke(string path, Action<ExcelOperator> action)
        {
            if (action == null)
                throw new ArgumentNullException("action is null.");

            using (ExcelOperator eo = new ExcelOperator(path))
                action(eo);
        }

        /// <summary>
        /// 指定された範囲を枠として罫線を引きます。
        /// </summary>
        /// <param name="row">開始行インデックス</param>
        /// <param name="column">開始列インデックス</param>
        /// <param name="top">上罫線</param>
        /// <param name="left">左罫線</param>
        /// <param name="right">右罫線</param>
        /// <param name="bottom">下罫線</param>
        /// <remarks>
        /// 罫線に<see cref="Microsoft.Office.Interop.Excel.XlLineStyle#xlLineStyleNone"/>を定義するとその位置に罫線は引きません。
        /// </remarks>
        public void DrowLines(int row, int column,
            XlLineStyle top = XlLineStyle.xlLineStyleNone,
            XlLineStyle left = XlLineStyle.xlLineStyleNone,
            XlLineStyle right = XlLineStyle.xlLineStyleNone,
            XlLineStyle bottom = XlLineStyle.xlLineStyleNone)
        {
            // オブジェクト参照カウンタの解放の為、使い回しは行わない
            Range range1 = null;

            try
            {
                this.sheet = workbook.Sheets[currentSheetIndex] as Worksheet;
                range1 = this.sheet.Cells[row, column] as Range;

                if (range1 == null)
                    return;

                DrowLines(range1, null, top, left, right, bottom);
            }
            finally
            {
                // COMオブジェクトを削除し参照カウンタをデクリメントする
                ReleaseComObject(range1);
            }      
        }

        /// <summary>
        /// 指定された範囲を枠として罫線を引きます。
        /// </summary>
        /// <param name="cell">セル名<c>A1</c>など</param>
        /// <param name="top">上罫線</param>
        /// <param name="left">左罫線</param>
        /// <param name="right">右罫線</param>
        /// <param name="bottom">下罫線</param>
        /// <remarks>
        /// 罫線に<see cref="Microsoft.Office.Interop.Excel.XlLineStyle#xlLineStyleNone"/>を定義するとその位置に罫線は引きません。
        /// </remarks>
        public void DrowLines(string cell,
            XlLineStyle top = XlLineStyle.xlLineStyleNone,
            XlLineStyle left = XlLineStyle.xlLineStyleNone,
            XlLineStyle right = XlLineStyle.xlLineStyleNone,
            XlLineStyle bottom = XlLineStyle.xlLineStyleNone)
        {
            // オブジェクト参照カウンタの解放の為、使い回しは行わない
            Range range1 = null;

            try
            {
                this.sheet = workbook.Sheets[currentSheetIndex] as Worksheet;
                range1 = this.sheet.Cells.get_Range(cell, Type.Missing);

                if (range1 == null)
                    return;

                DrowLines(range1, null, top, left, right, bottom);
            }
            finally
            {
                // COMオブジェクトを削除し参照カウンタをデクリメントする
                ReleaseComObject(range1);
            }
        }

        /// <summary>
        /// 指定された範囲を枠として罫線を引きます。
        /// </summary>
        /// <param name="startRow">開始行インデックス</param>
        /// <param name="startColumn">開始列インデックス</param>
        /// <param name="endRow">終了行インデックス</param>
        /// <param name="endColumn">終了列インデックス</param>
        /// <param name="top">上罫線</param>
        /// <param name="left">左罫線</param>
        /// <param name="right">右罫線</param>
        /// <param name="bottom">下罫線</param>
        /// <remarks>
        /// 罫線に<see cref="Microsoft.Office.Interop.Excel.XlLineStyle#xlLineStyleNone"/>を定義するとその位置に罫線は引きません。
        /// </remarks>
        public void DrowLines(int startRow, int startColumn,
            int endRow, int endColumn,
            XlLineStyle top = XlLineStyle.xlLineStyleNone,
            XlLineStyle left = XlLineStyle.xlLineStyleNone,
            XlLineStyle right = XlLineStyle.xlLineStyleNone,
            XlLineStyle bottom = XlLineStyle.xlLineStyleNone)
        {
            // オブジェクト参照カウンタの解放の為、使い回しは行わない
            Range range1 = null;
            Range range2 = null;

            try
            {
                this.sheet = workbook.Sheets[currentSheetIndex] as Worksheet;
                range1 = this.sheet.Cells[startRow, startColumn] as Range;
                range2 = this.sheet.Cells[endRow, endColumn] as Range;

                if (range1 == null)
                    return;

                DrowLines(range1, range2, top, left, right, bottom);
            }
            finally
            {
                // COMオブジェクトを削除し参照カウンタをデクリメントする
                ReleaseComObject(range1);
                ReleaseComObject(range2);
            }
        }

        /// <summary>
        /// 指定された範囲を枠として罫線を引きます。
        /// </summary>
        /// <param name="startCell">セル名<c>A1</c>など</param>
        /// <param name="endCell">セル名<c>A1</c>など</param>
        /// <param name="top">上罫線</param>
        /// <param name="left">左罫線</param>
        /// <param name="right">右罫線</param>
        /// <param name="bottom">下罫線</param>
        /// <remarks>
        /// 罫線に<see cref="Microsoft.Office.Interop.Excel.XlLineStyle#xlLineStyleNone"/>を定義するとその位置に罫線は引きません。
        /// </remarks>
        public void DrowLines(string startCell, string endCell,
            XlLineStyle top = XlLineStyle.xlLineStyleNone,
            XlLineStyle left = XlLineStyle.xlLineStyleNone,
            XlLineStyle right = XlLineStyle.xlLineStyleNone,
            XlLineStyle bottom = XlLineStyle.xlLineStyleNone)
        {
            // オブジェクト参照カウンタの解放の為、使い回しは行わない
            Range range1 = null;
            Range range2 = null;

            try
            {
                this.sheet = workbook.Sheets[currentSheetIndex] as Worksheet;
                range1 = this.sheet.Cells.get_Range(startCell, Type.Missing);
                range2 = this.sheet.Cells.get_Range(endCell, Type.Missing);

                if (range1 == null)
                    return;

                DrowLines(range1, range2, top, left, right, bottom);
            }
            finally
            {
                // COMオブジェクトを削除し参照カウンタをデクリメントする
                ReleaseComObject(range1);
                ReleaseComObject(range2);
            }
        }

        /// <summary>
        /// シートをコピーします。
        /// </summary>
        /// <param name="from">コピー元シートのインデックス</param>
        /// <param name="sheetName">コピー先シートのインデックス</param>
        public void CopySheet(int from, string sheetName)
        {
            // 最後尾にシートをコピー
            sheets = workbook.Sheets;
            copySheet = sheets[workbook.Sheets.Count] as Worksheet;

            // コピー処理を実行
            sheet = sheets[from] as Worksheet;
            sheet.Copy(Type.Missing, copySheet);

            // コピーしたシートオブジェクト
            copySheet = sheets[workbook.Sheets.Count] as Worksheet;

            // シート名を設定
            copySheet.Name = sheetName;
        }

        /// <summary>
        /// 引数indexsで指定されたシートを選択状態にします。
        /// indexは1始まりです。
        /// </summary>
        /// <param name="indexs">選択したいシートのインデックス配列</param>
        public void SelectedSheets(int[] indexs)
        {
            sheets = workbook.Worksheets;
            bool isSelectMode = true;
            for (int i = 0; i < indexs.Length; i++)
            {
                try
                {
                    sheet = sheets[indexs[i]] as Worksheet;
                    if (sheet != null)
                    {
                        sheet.Select(isSelectMode);

                        // 複数のシートを選択する場合は一枚目以降はfalseを設定する必要があるのでフラグを更新
                        if (isSelectMode)
                            isSelectMode = false;
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 指定されたシートの行をコピーします。
        /// </summary>
        /// <param name="formSheetIndex">コピー元シートインデックス</param>
        /// <param name="startRowIndex">コピー元行開始インデックス</param>
        /// <param name="endRowIndex">コピー元行終了インデックス</param>
        /// <param name="toSheetIndex">コピー先シートインデックス</param>
        /// <param name="insertRowIndex">コピー先行インデックス</param>
        public void CopyRow(int formSheetIndex, int startRowIndex, int endRowIndex, int toSheetIndex, int insertRowIndex)
        {
            // オブジェクト参照カウンタの解放の為、使い回しは行わない
            Range range1 = null;
            Range range2 = null;
            Range range3 = null;
            Range range4 = null;

            // 操作対象のインデックスを覚えておく
            int nowIndex = currentSheetIndex;

            try
            {
                // コピー元のシートを選択
                CurrentSheetIndex = formSheetIndex;

                // コピー範囲の取得
                range1 = sheet.Cells[startRowIndex, 1] as Range;
                range2 = sheet.Cells[endRowIndex, sheet.Cells.Count] as Range;
                range3 = sheet.get_Range(range1, range2) as Range;

                // 範囲をコピーする
                object o = range3.Copy(Type.Missing);

                // コピー先のシートの設定
                CurrentSheetIndex = toSheetIndex;

                // 挿入先の範囲を取得
                range4 = sheet.Rows[insertRowIndex, Type.Missing] as Range;

                // データをコピー
                range4.Insert(XlInsertShiftDirection.xlShiftDown);
            }
            finally
            {
                // COMオブジェクトを削除し参照カウンタをデクリメントする
                ReleaseComObject(range1);
                ReleaseComObject(range2);
                ReleaseComObject(range3);
                ReleaseComObject(range4);

                // 操作対象のインデックスを元に戻す
                CurrentSheetIndex = nowIndex;
            }
        }

        /// <summary>
        /// 行を削除します。
        /// </summary>
        /// <param name="startRowIndex">削除開始行インデックス</param>
        /// <param name="endRowIndex">削除終了行インデックス</param>
        public void DeleteRow(int startRowIndex, int endRowIndex)
        {
            Range delRange = null;

            try
            {
                // 削除対象範囲の取得
                delRange = sheet.get_Range(startRowIndex.ToString() + ":" + endRowIndex.ToString(), Type.Missing) as Range;
                delRange.Delete(Type.Missing);

            }
            finally
            {
                ReleaseComObject(delRange);
            }
        }

        /// <summary>
        /// 操作したExcelファイルを保存します。
        /// </summary>
        /// <returns>ファイルパス</returns>
        public string Save()
        {
            workbook.Save();
            return dist;
        }

        /// <summary>
        /// 操作したExcelファイルを保存し、指定したパスにコピーします。
        /// </summary>
        /// <param name="savePath">保存先のパス</param>
        /// <returns>保存先のパス</returns>
        public string SaveAndCopy(string savePath)
        {
            Save();

            Directory.CreateDirectory(Directory.GetParent(savePath).ToString());
            File.Copy(dist, savePath, true);

            return savePath;
        }

        /// <summary>
        /// Excelオブジェクトを開放します。
        /// </summary>
        public void Close()
        {
            ReleaseExcel();
        }

        /// <summary>
        /// アンマネージ リソースの解放およびリセットタスクを実行します。  
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        /// <summary>
        /// ExcelApplicationの取得
        /// </summary>
        /// <returns>生成したExcelApplication</returns>
        private Application CreateExcelApplication()
        {
            xlApplication = new Application();

            // Excel画面を表示しない
            xlApplication.Visible = false;

            // 画面表示の停止
            xlApplication.ScreenUpdating = false;

            // 警告メッセージを表示しない
            xlApplication.DisplayAlerts = false;

            return xlApplication;
        }

        /// <summary>
        /// Excelオブジェクトを解放します。
        /// </summary>
        private void ReleaseExcel()
        {
            // 以下、COMの開放処理
            try
            {
                // 画面表示再開 
                xlApplication.ScreenUpdating = true;
            }
            catch { }

            try
            {
                if (workbook != null)
                    workbook.Close(SaveChanges: false);

            }
            catch { }

            try
            {
                if (xlApplication != null)
                    xlApplication.Workbooks.Close();
            }
            catch { }

            ReleaseComObject(range);
            ReleaseComObject(copySheet);
            ReleaseComObject(sheet);
            ReleaseComObject(sheets);
            ReleaseComObject(workbook);

            try
            {
                if (xlApplication != null)
                    xlApplication.Quit();
            }
            catch { }

            ReleaseComObject(xlApplication);

            // ガベージコレクション実行指示 
            GC.Collect();
        }

        /// <summary>
        /// COMオブジェクトを解放します。
        /// </summary>
        /// <param name="o">COMオブジェクト</param>
        private void ReleaseComObject(object o)
        {
            try
            {
                if (o != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }
            catch { }
            finally
            {
                o = null;
            }
        }

        /// <summary>
        /// テンポラリファイルを作成します。
        /// </summary>
        private void CreateTempFile()
        {
            // テンポラリフォルダのパスを取得
            string tempDir = Path.GetTempPath();

            // 拡張子のないファイル名を作成
            FileInfo info = new FileInfo(source);
            string name = info.Name.Replace(info.Extension, string.Empty);

            // テンポラリファイル名を構築
            string tempFileName = TEMPFILENAME_FORMAT.DirectFormat(System.Windows.Forms.Application.ProductName, name, DateTime.Now.ToYYYYMMDDHHMMSS());

            // 絶対パスの構築
            dist = Path.Combine(tempDir, tempFileName);

            // テンポラリにファイルをコピー
            File.Copy(source, dist, true);
        }

        /// <summary>
        /// 指定された範囲を枠として罫線を引きます。
        /// </summary>
        /// <param name="startRange">開始範囲</param>
        /// <param name="endRange">終了範囲</param>
        /// <param name="endRow">終了行インデックス</param>
        /// <param name="endColumn">終了列インデックス</param>
        /// <param name="top">上罫線</param>
        /// <param name="left">左罫線</param>
        /// <param name="right">右罫線</param>
        /// <param name="bottom">下罫線</param>
        /// <remarks>
        /// 罫線に<see cref="Microsoft.Office.Interop.Excel.XlLineStyle#xlLineStyleNone"/>を定義するとその位置に罫線は引きません。
        /// </remarks>
        private void DrowLines(Range startRange, Range endRange,
            XlLineStyle top = XlLineStyle.xlLineStyleNone,
            XlLineStyle left = XlLineStyle.xlLineStyleNone,
            XlLineStyle right = XlLineStyle.xlLineStyleNone,
            XlLineStyle bottom = XlLineStyle.xlLineStyleNone)
        {
            // オブジェクト参照カウンタの解放の為、使い回しは行わない
            Range range1 = null;

            try
            {
                // 終了範囲が存在しない場合は自分で開始範囲として処理する。
                if (endRange == null)
                    endRange = startRange;

                this.sheet = workbook.Sheets[currentSheetIndex] as Worksheet;
                range1 = this.sheet.Cells.get_Range(startRange, endRange);

                if (range1 == null)
                    return;

                if (top != XlLineStyle.xlLineStyleNone)
                    range1.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = top;
                if (left != XlLineStyle.xlLineStyleNone)
                    range1.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = left;
                if (right != XlLineStyle.xlLineStyleNone)
                    range1.Borders.get_Item(XlBordersIndex.xlEdgeRight).LineStyle = right;
                if (bottom != XlLineStyle.xlLineStyleNone)
                    range1.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = bottom;
            }
            finally
            {
                // COMオブジェクトを削除し参照カウンタをデクリメントする
                ReleaseComObject(range1);
            }
        }

        #endregion

        #region delegate

        #endregion
    }
}