using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Distribution.m
{
    public partial class Withdraw : System.Web.UI.Page
    {
        /// <summary>
        /// 是否显示微信到账
        /// </summary>
        public bool isShowWeixin;
        /// <summary>
        /// 支付配置
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        protected void Page_Load(object sender, EventArgs e)
        {
            var payConfig = bllPay.GetPayConfig();
            if (payConfig != null)
            {
                if ((!string.IsNullOrEmpty(payConfig.WXAppId)) && (!string.IsNullOrEmpty(payConfig.WXMCH_ID)) && (!string.IsNullOrEmpty(payConfig.WXPartnerKey)))
                {
                    isShowWeixin = true;
                }
            }
        }
    }
}