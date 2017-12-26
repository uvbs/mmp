using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class OrderDeliveryV2 : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        /// <summary>
        /// 当前站点所有者信息
        /// </summary>
        public WebsiteInfo currentWebSiteInfo;
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo currentUserInfo;

        public System.Text.StringBuilder sbDelivery = new System.Text.StringBuilder();
        public System.Text.StringBuilder sbPaymentType = new System.Text.StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (bll.IsLogin)
            {
                currentUserInfo = DataLoadTool.GetCurrUserModel();
                currentWebSiteInfo = bll.GetWebsiteInfoModel();
            }
            else
            {
                Response.Redirect(string.Format("/App/Cation/Wap/Login.aspx?redirecturl={0}", Request.FilePath));

            }
            foreach (var item in bll.GetDeliveryList())
            {
                sbDelivery.AppendFormat("<option value=\"{0}\">{1}</option>",item.AutoId,item.DeliveryName);
                
            }
            foreach (var item in bll.GetPaymentTypeList())
            {
                sbPaymentType.AppendFormat("<option value=\"{0}\">{1}</option>", item.AutoId, item.PaymentTypeName);

            }
        }

    }
}