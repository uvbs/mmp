using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Payment.Alipay;
using System.Xml;
using ZentCloud.BLLJIMP.Model;
using System.IO;
using System.Text;
using ZentCloud.Common;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Alipay
{
    public partial class MallNotifyUrlV2 : System.Web.UI.Page
    {
        /// <summary>
        /// 支付BLL
        /// </summary>
        BllPay bllPay = new BllPay();
        /// <summary>
        /// 成功xml
        /// </summary>
        private string successStr = "success";
        /// <summary>
        /// 失败xml
        /// </summary>
        private string failStr= "fail";
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                Dictionary<string, string> parametersAll = bllPay.GetRequestParameter();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(parametersAll["notify_data"]);
                xmlDoc.Save(string.Format("C:\\Alipay\\mallnotify{0}.xml", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                //商户订单号
                string outTradeNo = xmlDoc.SelectSingleNode("/notify/out_trade_no").InnerText;
                //支付宝交易号
                string tradeNo = xmlDoc.SelectSingleNode("/notify/trade_no").InnerText;
                //交易状态
                string tradeStatus = xmlDoc.SelectSingleNode("/notify/trade_status").InnerText;
                string baseUrl = HttpContext.Current.Request.Url.Authority;
                string msg = "";
                bool payResult = bllPay.AliPayMallNotify(tradeStatus, tradeNo, outTradeNo, parametersAll, baseUrl, out msg);
                if (payResult)
                {
                    Response.Write(successStr);
                }
                else
                {
                    //Tolog(msg);
                    Response.Write(failStr);
                }
            }
            catch (Exception)
            {
                Response.Write("fail");
            }
        }


        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="msg"></param>
        private void Tolog(string msg)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"D:\alipaylog.txt", true, Encoding.GetEncoding("GB2312")))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "  " + msg);
                }
            }
            catch { }
        }
    }
}