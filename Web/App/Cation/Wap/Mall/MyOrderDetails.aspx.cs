using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class MyOrderDetails : System.Web.UI.Page
    {

        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        /// <summary>
        /// 当前订单
        /// </summary>
        public WXMallOrderInfo OrderInfo;
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo userInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            string orderid=null;
            orderid=Request["oid"];
            if (string.IsNullOrEmpty(orderid))
	        {
		        Response.End();
	        }

            if (bll.IsLogin)
            {
                 userInfo = bll.GetCurrentUserInfo();
                 OrderInfo = bll.GetOrderInfo(orderid);
                 if (OrderInfo==null)
	             {
		            Response.End();
	             }

                 bool CanAccess = false;
                 if (!OrderInfo.OrderUserID.Equals(userInfo.UserID))
                 {
                     try
                     {
                         if (bll.GetWebsiteInfoModel().IsDistributionMall.Equals(1))
                         {
                         //检查是否是上下级关系
                         BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();
                         BLLJIMP.BLLUser bllUser=new BLLJIMP.BLLUser("");
                         if (bllDis.GetUserBetweenLevel(userInfo,bllUser.GetUserInfo(OrderInfo.OrderUserID))>0)
                         {
                             CanAccess = true;
                         }
                         }

                     }
                     catch (Exception)
                     {
                         
                         
                     }
                 }
                 else
                 {
                     CanAccess = true;
                 }
                 if (!CanAccess)
                 {
                     Response.End();
                 }

            }
            else
            {

                Response.Redirect("/App/Cation/Wap/Login.aspx?redirecturl=/App/Cation/Wap/Mall/Index.aspx");
            }

        }
    }
}