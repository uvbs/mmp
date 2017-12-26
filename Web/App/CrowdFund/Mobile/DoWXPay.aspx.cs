using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Payment.WeiXin;
using System.Xml.Linq;
using System.Net;
using System.IO;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.CrowdFund.Mobile
{
    /// <summary>
    /// 众筹微信支付
    /// </summary>
    public partial class DoWXPay : MobileBase
    {

        /// <summary>
        /// 微信支付请求字符串
        /// </summary>
        public string WxPayReq = "";
        /// <summary>
        /// 基类BLL
        /// </summary>
        BLLJIMP.BLL bllBase = new BLLJIMP.BLL();
        /// <summary>
        /// 支付BLL
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region 检查订单是否可以支付
                int recordId = int.Parse(Request["recordid"]);
                CrowdFundRecord model = bllBase.Get<CrowdFundRecord>(string.Format(" RecordID={0}", recordId));
                if (model == null)
                {
                    Response.Write("订单无效");
                    Response.End();
                }
                if (model.Status.Equals(1))
                {
                    Response.Write("订单已经付款");
                    Response.End();
                } 
                #endregion

                #region 获取预支付ID
                PayConfig payConfig = bllPay.GetPayConfig();
                var nonStr = Payment.WeiXin.CommonUtil.CreateNoncestr();//随机串
                //
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("appid", payConfig.WXAppId);

                dic.Add("body", "订单号" + model.RecordID);

                dic.Add("mch_id", payConfig.WXMCH_ID);

                dic.Add("nonce_str", nonStr);

                dic.Add("out_trade_no", model.RecordID.ToString());

                dic.Add("openid", DataLoadTool.GetCurrUserModel().WXOpenId);


                dic.Add("spbill_create_ip", Request.UserHostAddress);

                dic.Add("total_fee", (model.Amount * 100).ToString("F0"));

                dic.Add("notify_url", string.Format("http://{0}/WxPayNotify/NotifyCrowdFund.aspx", Request.Url.Host));

                dic.Add("trade_type", "JSAPI");
                string strtemp = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(dic, false);
                string sign = MD5SignUtil.Sign(strtemp, payConfig.WXPartnerKey);

                dic = (from entry in dic
                       orderby entry.Key ascending
                       select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                dic.Add("sign", sign);

                string postData = Payment.WeiXin.CommonUtil.ArrayToXml(dic);
                string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(postData);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = requestBytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();

                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
                string backStr = sr.ReadToEnd();
                sr.Close();
                res.Close();
                var result = XDocument.Parse(backStr);
                var returnCode = result.Element("xml").Element("return_code").Value;

                var prepayId = "";
                var rusultCode = result.Element("xml").Element("result_code").Value;
                if (returnCode.ToUpper().Equals("SUCCESS") && (rusultCode.ToUpper().Equals("SUCCESS")))
                {
                    prepayId = result.Element("xml").Element("prepay_id").Value;


                } 
                #endregion

                #region 生成支付请求
                WXPayReq reqwx = new WXPayReq();
                string timeStamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
                reqwx.appId = payConfig.WXAppId;
                reqwx.nonceStr = nonStr;
                reqwx.package = "prepay_id=" + prepayId;
                reqwx.signType = "MD5";
                reqwx.timeStamp = timeStamp;

                Dictionary<string, string> dicNew = new Dictionary<string, string>();
                dicNew.Add("appId", reqwx.appId);
                dicNew.Add("timeStamp", reqwx.timeStamp);
                dicNew.Add("nonceStr", reqwx.nonceStr);
                dicNew.Add("package", reqwx.package);
                dicNew.Add("signType", "MD5");
                string strTemp1 = Payment.WeiXin.CommonUtil.FormatQueryParaMap(dicNew);
                string paySign = MD5SignUtil.Sign(strTemp1, payConfig.WXPartnerKey);
                reqwx.paySign = paySign;
                WxPayReq = ZentCloud.Common.JSONHelper.ObjectToJson(reqwx); 
                #endregion
                //


            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
                Response.End();


            }




        }
        /// <summary>
        /// 微信支付请求模型
        /// </summary>
        private class WXPayReq
        {
            /// <summary>
            /// 微信公众号appid
            /// </summary>
            public string appId { get; set; }
            /// <summary>
            ///时间戳
            /// </summary>
            public string timeStamp { get; set; }
            /// <summary>
            /// 随机字符串
            /// </summary>
            public string nonceStr { get; set; }
            /// <summary>
            /// 数据包
            /// </summary>
            public string package { get; set; }
            /// <summary>
            /// 签名类型 MD5
            /// </summary>
            public string signType { get; set; }
            /// <summary>
            /// 支付签名
            /// </summary>
            public string paySign { get; set; }

        }
    }
}