using System.IO;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace ZentCloud.Common
{
    public class NPOIHelper
    {
        public static MemoryStream RenderToExcel(DataTable table, string sheetName = "Sheet1")
        {
            MemoryStream ms = new MemoryStream();

            using (table)
            {
                IWorkbook workBook = new HSSFWorkbook();
                ISheet sheet = workBook.CreateSheet(sheetName);
                IRow headerRow = sheet.CreateRow(0);
                // handling header.
                foreach (DataColumn column in table.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value
                // handling value.
                int rowIndex = 1;
                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                }
                workBook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }
            return ms;
        }

        public static void SaveToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
                data = null;
            }
        }
        /// <summary>
        /// 表格直接存到本地文件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="filePath">本地路径</param>
        /// <param name="sheetName"></param>
        public static void DtToXls(DataTable table, string filePath, string sheetName = "Sheet1")
        {
            SaveToFile(RenderToExcel(table, sheetName), filePath);
        }

    }
}
