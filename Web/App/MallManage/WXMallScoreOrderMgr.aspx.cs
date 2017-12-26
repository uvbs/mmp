using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.MallManage
{
    public partial class WXMallScoreOrderMgr : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        /// <summary>
        /// 订单状态列表
        /// </summary>
        public System.Text.StringBuilder sbOrderStatuList = new System.Text.StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {

            foreach (var item in bll.GetOrderStatuList())
            {
                sbOrderStatuList.AppendFormat("<option  value=\"{0}\" >{1}</option>", item.OrderStatu, item.OrderStatu);

            }

        }

    }
}