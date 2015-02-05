using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data.OleDb;
using System.Data;
//using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace JLQ.Common
{
    public class ExcelCenter
    {
        #region 用connection读取，特点：速度快，不灵活。根据excel版本不一样需要系统有相应的Microsoft.Jet.Oledb.4.0/12.0
        //获取连接字符串
        static string GetExcelConnectionString(string excelFile, bool hasTitle)
        {
            string fileType = System.IO.Path.GetExtension(excelFile);
            if (string.IsNullOrEmpty(fileType)) return null;

            return string.Format("Provider=Microsoft.{0}.OLEDB.{1}.0;" +
                                 "Extended Properties=\"Excel {2};" +
                                 "HDR={3};\";" +
                                 "Mode=ReadWrite;" +
                //"IMEX={4};\";" +
                                 "data source={4};",
                                 (fileType == ".xls" ? "Jet" : "Ace"),
                                 (fileType == ".xls" ? 4 : 12),
                                 (fileType == ".xls" ? "8.0" : "12.0 XML"),
                                 (hasTitle ? "Yes" : "NO"),
                //(readOrWrite ? "1" : "0"),
                                 excelFile);
        }

        //根据Excel物理路径获取Excel文件中所有表名
        static List<string> GetExcelSheetNames(string excelFile, bool hasTitle)
        {
            List<string> res = new List<string>();
            try
            {
                if (!File.Exists(excelFile)) return res;//如果文件不存在
                string strConn = GetExcelConnectionString(excelFile, hasTitle);
                if (string.IsNullOrWhiteSpace(strConn)) return res;

                using (OleDbConnection objConn = new OleDbConnection(strConn))
                {
                    objConn.Open();
                    using (DataTable dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null))
                    {
                        if (dt == null) return res;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var one = dt.Rows[i]["TABLE_NAME"].ToString();
                            //后面没有$，当成临时表处理，跳过
                            if (one.LastIndexOf('$') == -1) continue;//表格后面的$表示已存在//http://support.microsoft.com/kb/316934/zh-cn
                            res.Add(one);
                        }
                        return res;
                    }
                }
            }
            catch (Exception ee) { throw ee; }
        }

        //在一個Excel文件中創建Sheet,如果文件不是一個已存在的文件，會自動創建文件
        static bool CreateSheet(OleDbCommand cmd, bool hasTitle, string sheetName, List<string> cols)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sheetName)) throw new Exception("请提供表名。");
                if (cols.Equals(null)) throw new Exception("请提供列名集合。");

                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 3600;
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("CREATE TABLE [{0}] (", sheetName);
                StringBuilder sqlClear = new StringBuilder();
                sqlClear.AppendFormat("update [{0}$] set", sheetName);
                for (int i = 0; i < cols.Count; i++)
                {
                    sql.AppendFormat(" [{0}] string,", cols[i]);
                    sqlClear.AppendFormat(" F{0} = \"\",", i + 1);
                }
                sql = sql.Remove(sql.Length - 1, 1).Append(")");
                cmd.CommandText = sql.ToString();
                cmd.ExecuteNonQuery();

                sqlClear = sqlClear.Remove(sqlClear.Length - 1, 1);//清空第一行
                cmd.CommandText = sqlClear.ToString();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch { return false; }
        }

        //用connection读取，特点：速度快，不灵活。根据excel版本不一样需要系统有相应的Microsoft.Jet.Oledb.4.0/12.0
        public static DataTable GetDataFromExcelByConn(string excelFilePath, bool hasTitle = false)
        {
            try
            {
                var tableNames = GetExcelSheetNames(excelFilePath, hasTitle);
                if (tableNames == null || tableNames.Count <= 0) return null;

                string strConn = GetExcelConnectionString(excelFilePath, hasTitle);
                if (string.IsNullOrWhiteSpace(strConn)) return null;
                using (DataSet ds = new DataSet())
                {
                    string strCom = string.Format(" SELECT * FROM [{0}]", tableNames[0]);//读第一个表
                    using (OleDbConnection myConn = new OleDbConnection(strConn))
                    using (OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn))
                    {
                        myConn.Open();
                        myCommand.Fill(ds);
                    }
                    if (ds == null || ds.Tables.Count <= 0) return null;
                    return ds.Tables[0];
                }
            }
            catch (Exception ee) { MessageBox.Show("读取excel数据失败，错误内容：\r\n" + ee.Message); return null; }
        }

        public static bool SaveDataToExcelByConn(string excelFilePath, string[,] data, bool hasTitle = false, string sheetName = "DataOutput")
        {
            try
            {
                if (sheetName.Trim() == "") throw new ArgumentException("请提供表名。");

                string strConn = GetExcelConnectionString(excelFilePath, hasTitle);
                if (string.IsNullOrWhiteSpace(strConn)) throw new ArgumentException("未知问题...");

                int rowCount = data.GetUpperBound(0) + 1;
                int columnCount = data.GetUpperBound(1) + 1;

                var tableNames = GetExcelSheetNames(excelFilePath, hasTitle);//先获取地址下文件的表哥名称集合
                using (OleDbConnection conn = new OleDbConnection(strConn))
                {
                    conn.Open();//此处如果没有excel会自动创建
                    using (OleDbCommand cmd = new OleDbCommand() { Connection = conn })
                    {
                        List<string> cols = Enumerable.Range(0, columnCount).Select(x => "F" + (x + 1)).ToList();
                        if (!tableNames.Contains(sheetName + "$"))//不存在工作簿，先创建
                        {
                            if (!(CreateSheet(cmd, hasTitle, sheetName, cols)))//创建Excel和表
                                throw new ArgumentException(string.Format("在{0}上创建表{1}失败.", excelFilePath, sheetName));
                        }

                        string strfield = string.Join(",", cols);
                        StringBuilder strvalue = new StringBuilder();

                        for (int i = 0; i < rowCount; i++)
                        {
                            strvalue.Clear();
                            for (int j = 0; j < columnCount; j++)
                                strvalue.AppendFormat(",'{0}'", data[i, j].Replace("'", "''"));
                            strvalue = strvalue.Remove(0, 1);

                            cmd.CommandText = string.Format("insert into [{0}$] ({1}) values ({2})", sheetName, strfield, strvalue);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                return true;
            }
            catch (Exception ee) { MessageBox.Show("保存数据到excel失败，错误内容：\r\n" + ee.Message); return false; }
        }
        #endregion

        #region Com组件读写，速度很慢，也要求客户端有excel程序
        ////用com组件读取,特点：只需要调用的客户端有excel程序即可。速度慢，而且数据量很大时，可能很慢。
        //public static DataTable GetDataFromExcelByCom(string excelFilePath, bool hasTitle = false)
        //{
        //    try
        //    {
        //        Excel.Application app = new Excel.Application();
        //        object oMissiong = System.Reflection.Missing.Value;
        //        Excel.Workbook workbook = null;
        //        DataTable dt = new DataTable();
        //        Excel.Sheets sheets = null;

        //        try
        //        {
        //            if (app == null) return null;
        //            workbook = app.Workbooks.Open(excelFilePath, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong,
        //                oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong);
        //            sheets = workbook.Worksheets;

        //            //将数据读入到DataTable中
        //            Excel.Worksheet worksheet = (Excel.Worksheet)sheets.get_Item(1);//读取第一张表  
        //            if (worksheet == null) return null;

        //            int iRowCount = worksheet.UsedRange.Rows.Count;
        //            int iColCount = worksheet.UsedRange.Columns.Count;
        //            //生成列头
        //            for (int i = 0; i < iColCount; i++)
        //            {
        //                var name = "column" + i;
        //                if (hasTitle)
        //                {
        //                    var txt = ((Excel.Range)worksheet.Cells[1, i + 1]).Text.ToString();
        //                    if (!string.IsNullOrWhiteSpace(txt)) name = txt;
        //                }
        //                while (dt.Columns.Contains(name)) name = name + "_1";//重复行名称会报错。
        //                dt.Columns.Add(new DataColumn(name, typeof(string)));
        //            }
        //            //生成行数据
        //            Excel.Range range;
        //            int rowIdx = hasTitle ? 2 : 1;
        //            for (int iRow = rowIdx; iRow <= iRowCount; iRow++)
        //            {
        //                DataRow dr = dt.NewRow();
        //                for (int iCol = 1; iCol <= iColCount; iCol++)
        //                {
        //                    range = (Excel.Range)worksheet.Cells[iRow, iCol];
        //                    dr[iCol - 1] = (range.Value2 == null) ? "" : range.Text.ToString();
        //                }
        //                dt.Rows.Add(dr);
        //            }
        //            return dt;
        //        }
        //        catch { return null; }
        //        finally
        //        {
        //            dt.Dispose();
        //            workbook.Close(false, oMissiong, oMissiong);
        //            workbook = null;
        //            app.Workbooks.Close();
        //            app.Quit();
        //            app = null;
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(oMissiong);
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets);
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
        //        }
        //    }
        //    catch { return null; }
        //}

        ////用com组件写入,特点：只需要调用的客户端有excel程序即可。速度慢，而且数据量很大时，可能很慢。
        //public static bool SaveDataToExcelByCom(string excelFilePath, string[,] data)
        //{
        //    try
        //    {
        //        Excel.Application app = new Excel.ApplicationClass();
        //        app.Application.Workbooks.Add(true);
        //        Excel.Workbook book = (Excel.Workbook)app.ActiveWorkbook;
        //        Excel.Worksheet sheet = (Excel.Worksheet)book.ActiveSheet;
        //        object oMissiong = System.Reflection.Missing.Value;
        //        try
        //        {
        //            for (int i = 0; i < data.GetUpperBound(0) + 1; i++)
        //                for (int j = 0; j < data.GetUpperBound(1) + 1; j++)
        //                    sheet.Cells[i + 1, j + 1] = data[i, j];
        //            //保存excel文件
        //            book.SaveCopyAs(excelFilePath);
        //            return true;
        //        }
        //        catch { return false; }
        //        finally
        //        {
        //            book.Close(false, oMissiong, oMissiong);
        //            book = null;
        //            app.Workbooks.Close();
        //            app.Quit();
        //            app = null;
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(oMissiong);
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
        //        }
        //    }
        //    catch { return false; }
        //}
        #endregion
    }
}