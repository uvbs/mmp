using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.Shop
{
    public partial class WxPay : System.Web.UI.Page
    {
        /// <summary>
        /// 支付成功跳转地址
        /// </summary>
        public string ReturnUrlSuccess = string.Empty;
        /// <summary>
        /// 支付失败跳转地址
        /// </summary>
        public string ReturnUrlFail = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            ReturnUrlSuccess = Request["return_url_success"];
            ReturnUrlFail = Request["return_url_fail"];

            if (!string.IsNullOrWhiteSpace(ReturnUrlSuccess))
            {
                ReturnUrlSuccess = HttpUtility.UrlDecode(ReturnUrlSuccess);

            }

            if (!string.IsNullOrWhiteSpace(ReturnUrlFail))
            {
                ReturnUrlFail = HttpUtility.UrlDecode(ReturnUrlFail);
            }

        }
    }
}