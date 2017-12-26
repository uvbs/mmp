using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using System.Xml;
using Payment.WeiXin;

namespace ZentCloud.JubitIMP.Web.WxPayNotify
{
    /// <summary>
    /// 微信支付通知商城 通过支付配置验证
    /// </summary>
    public partial class Notify : System.Web.UI.Page
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        public WXMallOrderInfo orderInfo = new WXMallOrderInfo();
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Request.InputStream);
                xmlDoc.Save(string.Format("C:\\WXPay\\Notify{0}.xml",DateTime.Now.ToString("yyyyMMddHHmmssfff")));//写入日志
                //全部参数
                Dictionary<string, string> parametersAll = new Dictionary<string, string>();
                foreach (XmlElement item in xmlDoc.DocumentElement.ChildNodes)
                {
                    string key = item.Name;
                    string value = item.InnerText;
                    if ((!string.IsNullOrEmpty(key))&&(!string.IsNullOrEmpty(value)))
                    {
                        parametersAll.Add(key, value);
 
                    }

                    
                }

                parametersAll = (from entry in parametersAll
                       orderby entry.Key ascending
                       select entry).ToDictionary(pair => pair.Key, pair => pair.Value);//全部参数排序

                //验签参数 不包括 sign 参数
                Dictionary<string, string> ParametersSign = (from entry in parametersAll
                                                   where !entry.Key.Equals("sign")
                                                   orderby entry.Key ascending
                                                   select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                orderInfo = bllMall.GetOrderInfo(parametersAll["out_trade_no"]);
                WXMallPaymentType PayMentType = bllMall.GetPaymentType(int.Parse(orderInfo.PaymentTypeAutoId));
                string strSign = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(ParametersSign, false);
                if (!MD5SignUtil.VerifySignature(strSign, parametersAll["sign"], PayMentType.WXPartnerKey))//验证签名
                {

                    Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");
                    return;
                } 
                 if (orderInfo == null)
                {
                    Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");

                    return;
                }
                 if (orderInfo.PaymentStatus.Equals(1))
                {
                    Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code></xml>");
                    return;
                }
                 //更新订单状态
                 if (parametersAll["return_code"].Equals("SUCCESS") && parametersAll["result_code"].Equals("SUCCESS"))//交易成功
                {
                    orderInfo.PaymentStatus = 1;
                    orderInfo.Status = "待发货";
                    if (bllMall.GetWebsiteInfoModelFromDataBase().IsDistributionMall.Equals(1))
                    {
                        orderInfo.DistributionStatus = 1;
                    }
                    if (bllMall.Update(orderInfo))
                    {
                        Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code></xml>");
                        return;
                    }
                    else
                    {
                        Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");
                        return;
                    }
                }
                 Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");



            }
            catch (Exception)
            {
                Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");

            }
           
            

        }
    }
}