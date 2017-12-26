using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class MyScoreOrderDetails : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 当前订单
        /// </summary>
        public WXMallScoreOrderInfo OrderInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            string OrderId = null;
            OrderId = Request["oid"];
            if (string.IsNullOrEmpty(OrderId))
            {
                Response.End();
            }
            if (bllMall.IsLogin)
            {
                OrderInfo = bllMall.GetScoreOrderInfo(OrderId);
                if (OrderInfo == null)
                {
                    Response.End();
                }
                if (!OrderInfo.OrderUserID.Equals(bllMall.GetCurrUserID()))
                {
                    Response.End();
                }
                if (!OrderInfo.WebsiteOwner.Equals(bllMall.WebsiteOwner))
                {
                    Response.End();
                }


            }
            else
            {

                Response.Redirect("/App/Cation/Wap/Login.aspx?redirecturl=/App/Cation/Wap/Mall/MyCenter.aspx");
            }

        }

    }
}