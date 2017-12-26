using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ZentCloud.Common
{
    public class ImportDataHelper
    {
        /// <summary>
        /// Bulk大容量导入数据
        /// </summary>
        /// <param name="connStr">数据库链接字符串</param>
        /// <param name="filePath">导入的文件路径</param>
        /// <param name="xmlPath">xml格式文件路径</param>
        /// <param name="tableName">表名</param>
        /// <param name="intoField">入库表字段名(多字段以逗号分隔)</param>
        /// <param name="selectField">入库格式字段名(多字段以逗号分隔，特殊处理参照Sql语句)</param>
        /// <returns></returns>
        public int BulkImport(string connStr, string filePath, string xmlPath, string tableName, string intoField, string selectField, string strWhere = "", int timeOut = 80000, int firstRow = 1)
        {
            int result = 0;

            StringBuilder sqlText = new StringBuilder();

            sqlText.AppendFormat(" INSERT INTO {0}({1}) ", tableName, intoField);
            sqlText.AppendFormat(" SELECT {0} ", selectField);
            sqlText.AppendFormat(" FROM  OPENROWSET( BULK '{0}',FORMATFILE= '{1}',FIRSTROW={2} ) as t1 ", filePath, xmlPath, firstRow);

            if (!string.IsNullOrEmpty(strWhere))
            {
                sqlText.AppendFormat(" WHERE {0} ", strWhere);
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlText.ToString(), conn);
                    cmd.CommandTimeout = timeOut;
                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex){
                var message = ex.Message;
            
            }

            return result;
        }
    }
}
