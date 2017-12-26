using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using Payment.Alipay;
using System.Xml;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Alipay
{
    /// <summary>
    /// 商城支付宝同步通知
    /// </summary>
    public partial class mallcall_back_url : System.Web.UI.Page
    {
        public string payresult = "false";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
                BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
                Dictionary<string, string> dicAll = bllPay.GetRequestParameter();
                //商户订单号
                //string out_trade_no = Request.QueryString["out_trade_no"];
                //支付宝交易号
               // string trade_no = Request.QueryString["trade_no"];
                //交易状态
               // string result = Request.QueryString["result"];
                var order = bllMall.GetOrderInfo(dicAll["out_trade_no"]);
                WXMallPaymentType paymentType = bllMall.GetPaymentType(int.Parse(order.PaymentTypeAutoId));
                if (dicAll.Count > 0)//判断是否有带返回参数
                {
                    //Notify aliNotify = new Notify();
                    //bool verifyResult = aliNotify.VerifyReturnMall(DicAll, Request.QueryString["sign"], PaymentType.AlipayPartnerKey);
                    if (bllPay.VerifySignatureAlipay(dicAll,paymentType.AlipayPartner,paymentType.AlipayPartnerKey))//验证成功
                    {
                        if (order.PaymentStatus.Equals(1))
                        {
                            payresult = "true";
                        }
                        else
                        {
                            order.PaymentStatus = 1;
                            order.Status = "待发货";
                            if (bllMall.GetWebsiteInfoModelFromDataBase().IsDistributionMall.Equals(1))
                            {
                                order.DistributionStatus = 1;
                            }
                            if (bllMall.Update(order))
                            {
                                payresult = "true";
                            }
                            else
                            {
                                msg.InnerHtml = "更新支付状态失败!</br>等待异步通知更新..";
                            }
                        }
                    }
                    else//验证失败
                    {

                        msg.InnerHtml = "验证失败";
                    }
                }
                else
                {

                    msg.InnerHtml = "无返回参数";

                }
            }
            catch (Exception ex)
            {

                msg.InnerHtml = ex.Message;
               
            }
        }


        ///// <summary>
        ///// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        ///// </summary>
        ///// <returns>request回来的信息组成的数组</returns>
        //public Dictionary<string, string> GetRequestGet()
        //{
        //    int i = 0;
        //    Dictionary<string, string> sArray = new Dictionary<string, string>();
        //    NameValueCollection coll;
        //    //Load Form variables into NameValueCollection variable.
        //    coll = Request.QueryString;

        //    // Get names of all forms into a string array.
        //    String[] requestItem = coll.AllKeys;

        //    for (i = 0; i < requestItem.Length; i++)
        //    {
        //        sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
        //    }

        //    return sArray;
        //}


    }
}