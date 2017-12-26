using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Payment.WeiXin;
using ZentCloud.BLLJIMP.Model;
using System.Xml;

namespace ZentCloud.JubitIMP.Web.customize.tuao
{
    public partial class WxPayNotify : System.Web.UI.Page
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        public WXMallOrderInfo OrderInfo = new WXMallOrderInfo();
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.Load(Request.InputStream);
                XmlDoc.Save(string.Format("C:\\WXPay\\Notify{0}.xml", DateTime.Now.ToString("yyyyMMddHHmmssfff")));//写入日志
                //全部参数
                Dictionary<string, string> ParametersAll = new Dictionary<string, string>();
                foreach (XmlElement item in XmlDoc.DocumentElement.ChildNodes)
                {
                    string key = item.Name;
                    string value = item.InnerText;
                    if ((!string.IsNullOrEmpty(key)) && (!string.IsNullOrEmpty(value)))
                    {
                        ParametersAll.Add(key, value);

                    }


                }

                ParametersAll = (from entry in ParametersAll
                                 orderby entry.Key ascending
                                 select entry).ToDictionary(pair => pair.Key, pair => pair.Value);//全部参数排序

                //验签参数 不包括 sign 参数
                Dictionary<string, string> ParametersSign = (from entry in ParametersAll
                                                             where !entry.Key.Equals("sign")
                                                             orderby entry.Key ascending
                                                             select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                OrderInfo = bllMall.GetOrderInfo(ParametersAll["out_trade_no"]);
                string StrSign = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(ParametersSign, false); 
                PayConfig Config = bllPay.GetPayConfig();

                if (!bllPay.VerifySignatureWx(ParametersAll,Config.WXPartnerKey))//验证签名
                {

                    Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");
                    return;
                }
                if (OrderInfo == null)
                {
                    Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");

                    return;
                }
                if (OrderInfo.PaymentStatus.Equals(1))
                {
                    Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code></xml>");
                    return;
                }
                //更新订单状态
                if (ParametersAll["return_code"].Equals("SUCCESS") && ParametersAll["result_code"].Equals("SUCCESS"))//交易成功
                {
                    OrderInfo.PaymentStatus = 1;
                    if (bllMall.GetWebsiteInfoModel().IsDistributionMall.Equals(1))
                    {
                        OrderInfo.DistributionStatus = 1;
                    }
                    if (bllMall.Update(OrderInfo))
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