using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.MallManage
{
    public partial class WXMallScoreOrderDetail : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        public WXMallScoreOrderInfo orderInfo;
        public List<WXMallScoreOrderDetailsInfo> orderDetailList;
        /// <summary>
        /// 订单状态列表
        /// </summary>
        public System.Text.StringBuilder sbOrderStatuList = new System.Text.StringBuilder();
        /// <summary>
        /// 配送员列表
        /// </summary>
        public System.Text.StringBuilder sbDeliveryStaffList = new System.Text.StringBuilder();
        public string orderId;
        protected void Page_Load(object sender, EventArgs e)
        {
            orderId = Request["oid"];
            if (string.IsNullOrEmpty(orderId))
            {
                Response.End();
            }
            orderInfo = bll.GetScoreOrderInfo(orderId);
            if (orderInfo == null)
            {
                Response.End();
            }
            orderDetailList = bll.GetScoreOrderDetailsList(orderId);
            foreach (var item in bll.GetOrderStatuList())
            {
                sbOrderStatuList.AppendFormat("<option  value=\"{0}\" >{1}</option>", item.OrderStatu, item.OrderStatu);

            }
            foreach (var item in bll.GetDeliveryStaffList())
            {
                sbDeliveryStaffList.AppendFormat("<option  value=\"{0}\" >{1}</option>", item.AutoID, item.StaffName);

            }
        }

    }
}