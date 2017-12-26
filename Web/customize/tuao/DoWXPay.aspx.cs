using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.tuao
{

    public partial class DoWXPay : TuAoBase
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        public WXMallOrderInfo Model = new WXMallOrderInfo();
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        /// <summary>
        /// 微信支付请求字符串
        /// </summary>
        public string WxPayReq = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                #region 检查订单是否可以支付
                if (Request["oid"] == null)
                {
                    Response.Write("订单无效");
                    Response.End();
                }
                int OrderId;
                if (!int.TryParse(Request["oid"], out OrderId))
                {
                    Response.Write("订单无效");
                    Response.End();
                }
                Model = bllMall.GetOrderInfo(OrderId.ToString());
                if (Model == null)
                {
                    Response.Write("订单无效");
                    Response.End();

                }
                if (!Model.PaymentStatus.Equals(0))
                {
                    Response.Write("订单不是未付款状态");
                    Response.End();
                }

                #endregion

                #region 生成预支付ID
                BLLJIMP.Model.PayConfig PayConfig = bllPay.GetPayConfig();
                WxPayReq = bllPay.GetBrandWcPayRequest(Model.OrderID, Model.TotalAmount, PayConfig.WXAppId, PayConfig.WXMCH_ID, PayConfig.WXPartnerKey, bllMall.GetCurrentUserInfo().WXOpenId, Request.UserHostAddress, string.Format("http://{0}/Customize/Tuao/WxPayNotify.aspx",Request.Url.Host));
                #endregion





            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
                Response.End();


            }




        }

    }
}