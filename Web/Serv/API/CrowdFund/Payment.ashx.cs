using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.CrowdFund
{
    /// <summary>
    /// 众筹支付
    /// </summary>
    public class Payment : BaseHandlerNeedLogin
    {
        /// <summary>
        /// 业务逻辑
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 支付BLL
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        /// <summary>
        /// 微信支付
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string BrandWcPayRequest(HttpContext context)
        {
            if (!bllPay.IsWeiXinBrowser)
            {
                resp.errcode = 1;
                resp.errmsg = "请用微信客户端访问";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            string orderId = context.Request["order_id"];
            var orderInfo = bll.Get<CrowdFundRecord>(string.Format(" RecordID={0}",orderId));
            if (orderInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "订单号不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (orderInfo.UserID != currentUserInfo.UserID)
            {
                resp.errcode = 1;
                resp.errmsg = "订单号无效";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (orderInfo.Status == 1)
            {
                resp.errcode = 1;
                resp.errmsg = "订单已经支付,不需重复支付";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            string appId = "";//appid
            string mchId = "";//mch_id
            string key = "";//key
            string openId = "";//openid
            string ip = "";//ip
            string notifyUrl = baseUrl + "/WxPayNotify/NotifyCrowdFundV2.aspx";//支付通知地址
            string body = "";//订单内容
            PayConfig payConfig = bllPay.GetPayConfig();
            appId = payConfig.WXAppId;
            mchId = payConfig.WXMCH_ID;
            key = payConfig.WXPartnerKey;
            openId = currentUserInfo.WXOpenId;
            ip = context.Request.UserHostAddress;
            string payReqStr = bllPay.GetBrandWcPayRequest(orderInfo.RecordID.ToString(),orderInfo.Amount, appId, mchId, key, openId, ip, notifyUrl, body);
            WXPayReq payReqModel = ZentCloud.Common.JSONHelper.JsonToModel<WXPayReq>(payReqStr);
            if (!string.IsNullOrEmpty(payReqModel.paySign))
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    pay_req = payReqModel
                });

            }
            resp.errcode = 1;
            resp.errmsg = "fail";
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 支付宝支付
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string BuildAlipayRequest(HttpContext context)
        {

            string orderId = context.Request["order_id"];
            if (bllPay.IsWeiXinBrowser)
            {
                resp.errcode = 1;
                resp.errmsg = "请不要在微信客户端中访问";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (!bllPay.IsMobile)
            {
                resp.errcode = 1;
                resp.errmsg = "请用手机浏览器访问";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            var orderInfo = bll.Get<CrowdFundRecord>(string.Format(" RecordID={0}", orderId));
            if (orderInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "订单号不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (orderInfo.UserID != currentUserInfo.UserID)
            {
                resp.errcode = 1;
                resp.errmsg = "订单号无效";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (orderInfo.Status == 1)
            {
                resp.errcode = 1;
                resp.errmsg = "订单已经支付,不需重复支付";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            PayConfig payConfig = bllPay.GetPayConfig();
            string notifyUrl = baseUrl + "/Alipay/NotifyCrowdFundV2.aspx";
            var payForm = bllPay.GetAliPayRequestMobile(orderInfo.RecordID.ToString(), (double)orderInfo.Amount, payConfig.Seller_Account_Name, payConfig.Partner, payConfig.PartnerKey, notifyUrl);


            if (!string.IsNullOrEmpty(payForm))
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    pay_req = payForm
                });

            }
            resp.errcode = 1;
            resp.errmsg = "fail";
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 解析远程模拟提交后返回的信息
        /// </summary>
        /// <param name="strText">要解析的字符串</param>
        /// <returns>解析结果</returns>
        public static Dictionary<string, string> ParseResponse(string strText)
        {
            //以“&”字符切割字符串
            string[] strSplitText = strText.Split('&');
            //把切割后的字符串数组变成变量与数值组合的字典数组
            Dictionary<string, string> dicText = new Dictionary<string, string>();
            for (int i = 0; i < strSplitText.Length; i++)
            {
                //获得第一个=字符的位置
                int nPos = strSplitText[i].IndexOf('=');
                //获得字符串长度
                int nLen = strSplitText[i].Length;
                //获得变量名
                string strKey = strSplitText[i].Substring(0, nPos);
                //获得数值
                string strValue = strSplitText[i].Substring(nPos + 1, nLen - nPos - 1);
                //放入字典类数组中
                dicText.Add(strKey, strValue);
            }

            if (dicText["res_data"] != null)
            {


                //token从res_data中解析出来（也就是说res_data中已经包含token的内容）
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                try
                {
                    xmlDoc.LoadXml(dicText["res_data"]);
                    string strRequest_token = xmlDoc.SelectSingleNode("/direct_trade_create_res/request_token").InnerText;
                    dicText.Add("request_token", strRequest_token);
                }
                catch (Exception exp)
                {
                    dicText.Add("request_token", exp.ToString());
                }
            }

            return dicText;
        }

        /// <summary>
        /// 微信支付请求模型
        /// </summary>
        private class WXPayReq
        {
            /// <summary>
            /// 公众号 appid
            /// </summary>
            public string appId { get; set; }
            /// <summary>
            /// 时间戳
            /// </summary>
            public string timeStamp { get; set; }
            /// <summary>
            /// 随机字符串
            /// </summary>
            public string nonceStr { get; set; }
            /// <summary>
            /// 订单详情扩展
            /// </summary>
            public string package { get; set; }
            /// <summary>
            /// 签名方式 MD5
            /// </summary>
            public string signType { get; set; }
            /// <summary>
            /// 支付签名
            /// </summary>
            public string paySign { get; set; }

        }




    }
}