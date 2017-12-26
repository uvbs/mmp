using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class success : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public WebsiteInfo currentWebsiteInfo;
        public string GoPage = "Index.aspx";
        public string OrderDetailPage = "MyOrderDetails.aspx";
        /// <summary>
        /// 订单信息
        /// </summary>
        public WXMallOrderInfo Order = new WXMallOrderInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            currentWebsiteInfo = bllMall.GetWebsiteInfoModel();
            if (Request["gopage"]!=null)
            {
                GoPage = Request["gopage"];
            }
            if (Request["orderdetailpage"]!=null)
            {
                OrderDetailPage = Request["orderdetailpage"];
            }
            Order = bllMall.GetOrderInfo(Request["oid"].ToString());

        }
    }
}