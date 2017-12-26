using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Booking.Doctor.Order
{
    public partial class List : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 扩展列
        /// </summary>
        public string Columns = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("  WebsiteOwner='{0}' And TableName ='ZCJ_WXMallOrderInfo' Order by Sort DESC", bllMall.WebsiteOwner);
            var fieldList = bllMall.GetList<TableFieldMapping>(sbWhere.ToString());
            for (int i = 0; i < fieldList.Count; i++)
            {
                Columns += "{ field: '" + fieldList[i].Field + "', title: '" + fieldList[i].MappingName + "', width: 100, align: 'left' }";
                if (i < fieldList.Count - 1)
                {
                    Columns += ",";
                }
            }

        }

    }
}