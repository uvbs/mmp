using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.MallManage
{
    public partial class QianWeiStockStatistics : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        //public System.Text.StringBuilder sbStores = new System.Text.StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (var item in bll.GetCategoryList())
            {
                sbCategory.AppendFormat("<option value=\"{0}\">{1}</option>", item.AutoID, item.CategoryName);

            }
            //foreach (var item in bll.GetWXMallStoreListByWebSite())
            //{
            //    sbStores.AppendFormat("<option value=\"{0}\">{1}</option>", item.AutoID, item.StoreName);

            //}
        }
    }
}