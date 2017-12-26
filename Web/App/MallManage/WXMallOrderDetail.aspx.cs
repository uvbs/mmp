using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.App.MallManage
{
    public partial class WXMallOrderDetail : System.Web.UI.Page
    {
        /// <summary>
        /// 商城 bll
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 用户bll
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public WXMallOrderInfo orderInfo;
        public List<WXMallOrderDetailsInfo> orderDetailList;
        /// <summary>
        /// 订单状态列表
        /// </summary>
        public System.Text.StringBuilder sbOrderStatuList = new System.Text.StringBuilder();
        /// <summary>
        /// 配送员列表
        /// </summary>
        //public System.Text.StringBuilder sbDeliveryStaffList = new System.Text.StringBuilder();

        public string orderId;
        /// <summary>
        /// 订单用户信息
        /// </summary>
        //public UserInfo OrderUserInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            orderId = Request["oid"];
            if (string.IsNullOrEmpty(orderId))
            {
                Response.End();
            }
            orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo == null)
            {
                Response.End();
            }
            orderDetailList = bllMall.GetOrderDetailsList(orderId);
            foreach (var item in bllMall.GetOrderStatuList())
            {
                sbOrderStatuList.AppendFormat("<option  value=\"{0}\" >{1}</option>", item.OrderStatu, item.OrderStatu);

            }
            //foreach (var item in bll.GetWXMallDeliveryStaff())
            //{
            //    sbDeliveryStaffList.AppendFormat("<option  value=\"{0}\" >{1}</option>", item.AutoID, item.StaffName);

            //}
            //OrderUserInfo = new BLLJIMP.BLLUser("").GetUserInfo(orderInfo.OrderUserID,bllMall.WebsiteOwner);

        }
    }

}