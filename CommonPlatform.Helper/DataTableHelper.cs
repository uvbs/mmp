using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CommonPlatform.Helper
{
    public class DataTableHelper
    {
        /// <summary>
        /// 获取DataTable第一行某字段的值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Pm"></param>
        /// <returns></returns>
        public object GetValueByDataTableTop(DataTable dt, string Pm)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0][Pm];
            }
        }
    }
}
