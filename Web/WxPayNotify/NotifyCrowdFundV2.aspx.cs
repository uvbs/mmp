using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using System.Xml;

namespace ZentCloud.JubitIMP.Web.WxPayNotify
{
    public partial class NotifyCrowdFundV2 : System.Web.UI.Page
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 支付BLL
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Request.InputStream);
                xmlDoc.Save(string.Format("C:\\WXPay\\Notify{0}.xml", DateTime.Now.ToString("yyyyMMddHHmmssfff")));//写入日志
                //全部参数
                Dictionary<string, string> parametersAll = new Dictionary<string, string>();
                foreach (XmlElement item in xmlDoc.DocumentElement.ChildNodes)
                {
                    string key = item.Name;
                    string value = item.InnerText;
                    if ((!string.IsNullOrEmpty(key)) && (!string.IsNullOrEmpty(value)))
                    {
                        parametersAll.Add(key, value);

                    }


                }
                parametersAll = (from entry in parametersAll
                                 orderby entry.Key ascending
                                 select entry).ToDictionary(pair => pair.Key, pair => pair.Value);//全部参数排序
                
                var orderInfo = bll.Get<CrowdFundRecord>(string.Format(" RecordID={0}", parametersAll["out_trade_no"]));
                PayConfig payConfig = bllPay.GetPayConfig();
                if (!bllPay.VerifySignatureWx(parametersAll, payConfig.WXPartnerKey))//验证签名
                {

                    Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");
                    return;
                }
                if (orderInfo == null)
                {
                    Response.Write("<xml><return_code><![CDATA[FAIL]]></return_code></xml>");

                    return;
                }
                if (orderInfo.Status.Equals(1))
                {
                    
                    Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code></xml>");
                    return;
                }
                //更新订单状态
                if (parametersAll["return_code"].Equals("SUCCESS") && parametersAll["result_code"].Equals("SUCCESS"))//交易成功
                {
                    orderInfo.Status = 1;
                    orderInfo.OrderStatus = "待发货";
                    orderInfo.PayTime = DateTime.Now;
                    if (bll.Update(orderInfo))
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