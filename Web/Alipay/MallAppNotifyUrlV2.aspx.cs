using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Alipay
{
    public partial class MallAppNotifyUrlV2 : System.Web.UI.Page
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
        private string failStr = "fail";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                Dictionary<string, string> parametersAll = bllPay.GetRequestParameter();
                string json = JsonConvert.SerializeObject(parametersAll);
                WritePayLog(json);
                //商户订单号
                string outTradeNo = parametersAll["out_trade_no"];
                //支付宝交易号
                string tradeNo = parametersAll["trade_no"];
                //交易状态
                string tradeStatus = parametersAll["trade_status"];
                //支付宝开放平台应用Id
                string app_id = parametersAll["app_id"];
                string baseUrl = HttpContext.Current.Request.Url.Authority;
                string msg = "";
                bool payResult = bllPay.AliPayAppMallNotify(tradeStatus, app_id, tradeNo, outTradeNo, parametersAll, baseUrl, out msg);
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
        private void WritePayLog(string json)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"C:\Alipay\appmallnotify"+DateTime.Now.ToString("yyyyMMddHHmmssfff")+".txt", true, Encoding.GetEncoding("GB2312")))
                {
                    sw.WriteLine(json);
                }
            }
            catch { }
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