using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class MyOrderList : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 当前用户所有订单
        /// </summary>
        public List<WXMallOrderInfo> orderList;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (bllMall.IsLogin)
            {
                orderList = bllMall.GetList<WXMallOrderInfo>(string.Format("OrderUserID='{0}' And WebsiteOwner='{1}' Order By InsertDate DESC", bllMall.GetCurrUserID(),bllMall.WebsiteOwner));
            }
            else
            {

                Response.Redirect(string.Format("/App/Cation/Wap/Login.aspx?redirecturl={0}",Request.FilePath));
            }
        }
    }
}