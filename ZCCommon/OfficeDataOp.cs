using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.OleDb;

namespace ZentCloud.Common
{
    public abstract class OfficeDataOp
    {

        public static DataSet ExcelToDataSet(string fullName, string sheetName)
        {
            string strConn = GetConnectionStr(fullName);
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(strConn);
            System.Data.OleDb.OleDbDataAdapter oada = new System.Data.OleDb.OleDbDataAdapter("select * from [" + sheetName + "]", strConn);
            DataSet ds = new DataSet();
            oada.Fill(ds);
            return ds;
        }


        public static List<DataSet> ExcelToDataSetList(string fullName)
        {
            List<DataSet> dsList = new List<DataSet>();

            List<string> sheetList = GetExcelSheetNames(fullName).ToList();
            //ArrayList sheetList = GetSheetName(fullName);

            if (sheetList.Count>0)
            {
                sheetList = sheetList.Select(p => p.Replace("$_", "$")).Distinct().ToList();
            }
            else
            {

            }
           


            foreach (var sheet in sheetList)
            {
                dsList.Add(ExcelToDataSet(fullName, sheet.ToString()));
            }
            return dsList;
        }

        public static ArrayList GetSheetName(string filePath)
        {
            ArrayList al = new ArrayList();

            OleDbConnection conn = new OleDbConnection(GetConnectionStr(filePath));
            conn.Open();
            DataTable sheetNames = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            conn.Close();
            foreach (DataRow dr in sheetNames.Rows)
            {
                al.Add(dr[2]);
            }

            return al;
        }

        public static String[] GetExcelSheetNames(string excelFile)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;

            try
            {

                objConn = new OleDbConnection(GetConnectionStr(excelFile));
                objConn.Open();
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                {
                    return null;
                }
                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }

                return excelSheets;
            }
            catch(Exception ex)
            {

                return null;
            }
            finally
            {
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }

        private static string GetConnectionStr(string filepath)
        {
            if (filepath.ToLower().EndsWith(".xlsx"))
            {
                return "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + filepath + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'";
            }
            return string.Format("Provider = Microsoft.Jet.OLEDB.4.0; Data Source = {0}; Extended Properties = Excel 8.0", filepath);

        }
    }
}
