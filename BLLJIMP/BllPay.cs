using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using System.Web;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Net;
using System.IO;
using Payment.Alipay;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Domain;
using Aop.Api.Response;
using Aop.Api.Util;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    ///支付逻辑
    /// </summary>
    public class BllPay : BLL
    {
        /// <summary>
        /// 支付相关逻辑
        /// </summary>
        public BllPay()
            : base()
        {

        }
        /// <summary>
        /// 获取支付配置信息
        /// </summary>
        /// <returns></returns>
        public PayConfig GetPayConfig(string websiteOwner = "")
        {
            if (string.IsNullOrWhiteSpace(websiteOwner)) websiteOwner = WebsiteOwner;
            return Get<PayConfig>(string.Format("WebsiteOwner='{0}'", WebsiteOwner));
        }
        /// <summary>
        /// 是否配置微信支付
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool IsWeixinPay(string websiteOwner = "")
        {
            PayConfig payConfig = GetPayConfig(websiteOwner);
            if (payConfig == null)
            {
                return false;
            }
            return IsWeixinPay(payConfig);
        }
        /// <summary>
        /// 是否配置微信支付
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool IsWeixinPay(PayConfig payConfig)
        {
            if (!string.IsNullOrEmpty(payConfig.WXAppId) &&
                !string.IsNullOrEmpty(payConfig.WXMCH_ID) &&
                !string.IsNullOrEmpty(payConfig.WXPartnerKey))
                return true;
            return false;
        }
        /// <summary>
        /// 是否配置支付宝支付
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool IsAliPay(string websiteOwner = "")
        {
            PayConfig payConfig = GetPayConfig(websiteOwner);
            if (payConfig == null)
            {
                return false;
            }
            return IsAliPay(payConfig);
        }
        /// <summary>
        /// 是否配置支付宝支付
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool IsAliPay(PayConfig payConfig)
        {
            if (!string.IsNullOrEmpty(payConfig.Seller_Account_Name) &&
                !string.IsNullOrEmpty(payConfig.Partner) &&
                !string.IsNullOrEmpty(payConfig.PartnerKey))
                return true;
            return false;
        }

        /// <summary>
        /// 获取微信JSAPI支付字符串
        /// </summary>
        /// <param name="appId">微信公众号AppId</param>
        /// <param name="mchId">商户 MCH_ID</param>
        /// <param name="key">商户 Key</param>
        /// <param name="orderId">订单号</param>
        /// <param name="openId">下单用户的OpenId</param>
        /// <param name="ip">用户IP</param>
        /// <param name="totalAmount">订单总金额(元)</param>
        /// <param name="notifyUrl">支付通知地址</param>
        /// <param name="body">支付时显示的内容可留空</param>
        /// <returns></returns>
        public string GetBrandWcPayRequest(string orderId, decimal totalAmount, string appId, string mchId, string key, string openId, string ip, string notifyUrl, string body = "", string tradeType = "")
        {
            string backStr = "";
            try
            {
                if (string.IsNullOrWhiteSpace(openId))
                {
                    //openid空，则绑定当前openid到当前账号
                    //var currUser = GetCurrentUserInfo();
                    //string.IsNullOrWhiteSpace(currUser.WXOpenId) && 
                    if (HttpContext.Current.Session["currWXOpenId"] != null)
                    {
                        //currUser.WXOpenId = HttpContext.Current.Session["currWXOpenId"].ToString();
                        openId = HttpContext.Current.Session["currWXOpenId"].ToString();

                    }

                }

                string nonStr = Payment.WeiXin.CommonUtil.CreateNoncestr();//随机串
                #region 获取微信支付预支付ID
                //第一次签名
                Dictionary<string, string> dicStep1 = new Dictionary<string, string>();
                dicStep1.Add("appid", appId);
                dicStep1.Add("body", !string.IsNullOrEmpty(body) ? body : string.Format("订单号:{0}", orderId));
                dicStep1.Add("mch_id", mchId);
                dicStep1.Add("nonce_str", nonStr);
                dicStep1.Add("out_trade_no", orderId);
                dicStep1.Add("openid", openId);
                dicStep1.Add("spbill_create_ip", ip);
                dicStep1.Add("total_fee", (totalAmount * 100).ToString("F0"));
                dicStep1.Add("notify_url", notifyUrl);
                if (!string.IsNullOrEmpty(tradeType))
                {
                    dicStep1.Add("trade_type", tradeType);
                }
                else
                {
                    dicStep1.Add("trade_type", "JSAPI");
                }
                string strTemp1 = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(dicStep1, false);
                string sign = Payment.WeiXin.MD5SignUtil.Sign(strTemp1, key);
                dicStep1 = (from entry in dicStep1
                            orderby entry.Key ascending
                            select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                dicStep1.Add("sign", sign);
                string postData = Payment.WeiXin.CommonUtil.ArrayToXml(dicStep1);
                string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(postData);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = requestBytes.Length;
                System.IO.Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
                backStr = sr.ReadToEnd();
                sr.Close();
                res.Close();
                var result = System.Xml.Linq.XDocument.Parse(backStr);
                var returnCode = result.Element("xml").Element("return_code").Value;
                string preId = "";
                var rusultCode = result.Element("xml").Element("result_code").Value;
                //using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\logwxpay.txt", true, Encoding.GetEncoding("GB2312")))
                //{
                //    sw.WriteLine(backStr);
                //} 
                if (returnCode.ToUpper().Equals("SUCCESS") && (rusultCode.ToUpper().Equals("SUCCESS")))
                {
                    preId = result.Element("xml").Element("prepay_id").Value;
                }
                #endregion
                #region 生成微信支付请求
                WXPayReq wxPayReq = new WXPayReq();
                string timesStamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
                wxPayReq.appId = appId;
                wxPayReq.nonceStr = nonStr;
                wxPayReq.package = "prepay_id=" + preId;
                wxPayReq.signType = "MD5";
                wxPayReq.timeStamp = timesStamp;
                //第二次签名
                Dictionary<string, string> dicStep2 = new Dictionary<string, string>();
                dicStep2.Add("appId", wxPayReq.appId);
                dicStep2.Add("timeStamp", wxPayReq.timeStamp);
                dicStep2.Add("nonceStr", wxPayReq.nonceStr);
                dicStep2.Add("package", wxPayReq.package);
                dicStep2.Add("signType", "MD5");
                string strTemp2 = Payment.WeiXin.CommonUtil.FormatQueryParaMap(dicStep2);
                string paySign = Payment.WeiXin.MD5SignUtil.Sign(strTemp2, key);
                wxPayReq.paySign = paySign;
                return ZentCloud.Common.JSONHelper.ObjectToJson(wxPayReq);
                #endregion
            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\paylog.txt", true, Encoding.GetEncoding("UTF-8")))
                {
                    sw.WriteLine(backStr);
                }
                return System.Xml.Linq.XDocument.Parse(backStr).Element("xml").Element("return_msg").Value;
            }
        }


        /// <summary>
        /// 微信付款
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="amount">退款金额（单位 元）</param>
        /// <param name="appId">公众号AppId</param>
        /// <param name="mchId">商户号</param>
        /// <param name="key">api密钥</param>
        /// <param name="openId">用户微信OpenId</param>
        /// <param name="ip">用户ip</param>
        /// <param name="desc">说明</param>
        /// <returns></returns>
        private bool WeixinTransfers(string orderId, decimal amount, string appId, string mchId, string key, string openId, string ip, out string msg, string desc = "打款")
        {
            msg = "false";
            try
            {

                X509Certificate2 cert = new X509Certificate2(string.Format("D:\\ApiClientCer\\{0}\\apiclient_cert.p12", WebsiteOwner), mchId);
                string nonStr = Payment.WeiXin.CommonUtil.CreateNoncestr();//随机串
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("mch_appid", appId);
                dic.Add("mchid", mchId);
                dic.Add("nonce_str", nonStr);
                dic.Add("partner_trade_no", orderId);
                dic.Add("openid", openId);
                dic.Add("check_name", "NO_CHECK");
                dic.Add("amount", (amount * 100).ToString("F0"));
                dic.Add("desc", desc);
                dic.Add("spbill_create_ip", ip);
                string strTemp = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(dic, false);
                string sign = Payment.WeiXin.MD5SignUtil.Sign(strTemp, key);
                dic = (from entry in dic
                       orderby entry.Key ascending
                       select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                dic.Add("sign", sign);
                string postData = Payment.WeiXin.CommonUtil.ArrayToXml(dic);
                string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(postData);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ClientCertificates.Add(cert);
                req.ContentLength = requestBytes.Length;
                System.IO.Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
                string xmlResult = sr.ReadToEnd();
                sr.Close();
                res.Close();
                var xml = System.Xml.Linq.XDocument.Parse(xmlResult);
                var returnCode = xml.Element("xml").Element("return_code").Value;
                var resultCode = xml.Element("xml").Element("result_code").Value;
                msg = xml.Element("xml").Element("return_msg").Value;
                if (returnCode.ToUpper().Equals("SUCCESS") && (resultCode.ToUpper().Equals("SUCCESS")))
                {
                    return true;

                }



            }
            catch (Exception ex)
            {
                msg = ex.Message;

            }
            return false;
        }


        /// <summary>
        /// 微信打款
        /// </summary>
        /// <param name="orderId">订单号，流水号</param>
        /// <param name="amount">金额</param>
        /// <param name="openId">用户OpenId</param>
        /// <param name="ip">用户Ip</param>
        /// <param name="msg">提示信息</param>
        /// <param name="desc">说明</param>
        /// <returns></returns>
        public bool WeixinTransfers(string orderId, decimal amount, string openId, string ip, out string msg, string desc = "打款")
        {
            msg = "false";
            try
            {

                PayConfig payConfig = GetPayConfig();
                if (payConfig == null)
                {
                    msg = "支付未配置";
                    return false;
                }
                if ((string.IsNullOrEmpty(payConfig.WXAppId)) || (string.IsNullOrEmpty(payConfig.WXMCH_ID)) || (string.IsNullOrEmpty(payConfig.WXPartnerKey)))
                {
                    msg = "支付配置不完整";
                    return false;

                }
                X509Certificate2 cert = new X509Certificate2(string.Format("D:\\ApiClientCer\\{0}\\apiclient_cert.p12", WebsiteOwner), payConfig.WXMCH_ID);
                string nonStr = Payment.WeiXin.CommonUtil.CreateNoncestr();//随机串
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("mch_appid", payConfig.WXAppId);
                dic.Add("mchid", payConfig.WXMCH_ID);
                dic.Add("nonce_str", nonStr);
                dic.Add("partner_trade_no", orderId);
                dic.Add("openid", openId);
                dic.Add("check_name", "NO_CHECK");
                dic.Add("amount", (amount * 100).ToString("F0"));
                dic.Add("desc", desc);
                dic.Add("spbill_create_ip", ip);
                string strTemp = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(dic, false);
                string sign = Payment.WeiXin.MD5SignUtil.Sign(strTemp, payConfig.WXPartnerKey);
                dic = (from entry in dic
                       orderby entry.Key ascending
                       select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                dic.Add("sign", sign);
                string postData = Payment.WeiXin.CommonUtil.ArrayToXml(dic);
                string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(postData);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ClientCertificates.Add(cert);
                req.ContentLength = requestBytes.Length;
                System.IO.Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
                string xmlResult = sr.ReadToEnd();
                ToLog("微信打款结果:"+xmlResult);
                sr.Close();
                res.Close();
                var xml = System.Xml.Linq.XDocument.Parse(xmlResult);
                var returnCode = xml.Element("xml").Element("return_code").Value;
                var resultCode = xml.Element("xml").Element("result_code").Value;
                msg = xml.Element("xml").Element("return_msg").Value;
                if (returnCode.ToUpper().Equals("SUCCESS") && (resultCode.ToUpper().Equals("SUCCESS")))
                {
                    return true;

                }



            }
            catch (Exception ex)
            {
                msg = ex.Message;

            }
            return false;
        }
        /// <summary>
        /// 微信退款
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="refundNumber">退款单号</param>
        /// <param name="totalAmount">订单总金额</param>
        /// <param name="refundAmount">退款金额</param>
        /// <param name="appId">appId</param>
        /// <param name="mchId">微信商户号</param>
        /// <param name="key">key</param>
        /// <param name="msg">提示信息</param>
        /// <param name="weixinRefundId">微信退款单号</param>
        /// <returns></returns>
        public bool WeixinRefund(string orderId, string refundNumber, decimal totalAmount, decimal refundAmount, string appId, string mchId, string key, out string msg, out string weixinRefundId, string websiteOwner = "")
        {
            msg = "false";
            weixinRefundId = "";
            try
            {
                if (string.IsNullOrEmpty(websiteOwner))
                {
                    websiteOwner = WebsiteOwner;
                }
                X509Certificate2 cert = new X509Certificate2(string.Format("D:\\ApiClientCer\\{0}\\apiclient_cert.p12", websiteOwner), mchId);
                string nonStr = Payment.WeiXin.CommonUtil.CreateNoncestr();//随机串
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("appid", appId);
                dic.Add("mch_id", mchId);
                dic.Add("nonce_str", nonStr);
                dic.Add("out_trade_no", orderId);
                dic.Add("out_refund_no", refundNumber);
                dic.Add("total_fee", (totalAmount * 100).ToString("F0"));
                dic.Add("refund_fee", (refundAmount * 100).ToString("F0"));
                dic.Add("op_user_id", mchId);
                string strTemp = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(dic, false);
                string sign = Payment.WeiXin.MD5SignUtil.Sign(strTemp, key);
                dic = (from entry in dic
                       orderby entry.Key ascending
                       select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                dic.Add("sign", sign);
                string postData = Payment.WeiXin.CommonUtil.ArrayToXml(dic);
                string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(postData);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ClientCertificates.Add(cert);
                req.ContentLength = requestBytes.Length;
                System.IO.Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
                string xmlResult = sr.ReadToEnd();
                ToLog(xmlResult);
                sr.Close();
                res.Close();
                var xml = System.Xml.Linq.XDocument.Parse(xmlResult);
                var returnCode = xml.Element("xml").Element("return_code").Value;
                var resultCode = xml.Element("xml").Element("result_code").Value;
                msg = xml.Element("xml").Element("return_msg").Value;

                if (returnCode.ToUpper().Equals("SUCCESS") && (resultCode.ToUpper().Equals("SUCCESS")))
                {
                    weixinRefundId = xml.Element("xml").Element("refund_id").Value;
                    return true;

                }
                else
                {
                    msg = xml.Element("xml").Element("err_code_des").Value;

                }

            }
            catch (Exception ex)
            {
                msg = ex.ToString();

            }
            return false;
        }
        
        /// <summary>
        /// 微信退款 使用余额
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="refundNumber">退款单号</param>
        /// <param name="totalAmount">订单总金额</param>
        /// <param name="refundAmount">退款金额</param>
        /// <param name="appId">appId</param>
        /// <param name="mchId">微信商户号</param>
        /// <param name="key">key</param>
        /// <param name="msg">提示信息</param>
        /// <param name="weixinRefundId">微信退款单号</param>
        /// <returns></returns>
        public bool WeixinRefundYuEr(string orderId, string refundNumber, decimal totalAmount, decimal refundAmount, string appId, string mchId, string key, out string msg, out string weixinRefundId, string websiteOwner = "")
        {
            msg = "false";
            weixinRefundId = "";
            try
            {
                if (string.IsNullOrEmpty(websiteOwner))
                {
                    websiteOwner = WebsiteOwner;
                }
                X509Certificate2 cert = new X509Certificate2(string.Format("D:\\ApiClientCer\\{0}\\apiclient_cert.p12", websiteOwner), mchId);
                string nonStr = Payment.WeiXin.CommonUtil.CreateNoncestr();//随机串
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("appid", appId);
                dic.Add("mch_id", mchId);
                dic.Add("nonce_str", nonStr);
                dic.Add("out_trade_no", orderId);
                dic.Add("out_refund_no", refundNumber);
                dic.Add("total_fee", (totalAmount * 100).ToString("F0"));
                dic.Add("refund_fee", (refundAmount * 100).ToString("F0"));
                dic.Add("op_user_id", mchId);
                dic.Add("refund_account", "REFUND_SOURCE_RECHARGE_FUNDS");
                string strTemp = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(dic, false);
                string sign = Payment.WeiXin.MD5SignUtil.Sign(strTemp, key);
                dic = (from entry in dic
                       orderby entry.Key ascending
                       select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                dic.Add("sign", sign);
                string postData = Payment.WeiXin.CommonUtil.ArrayToXml(dic);
                string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(postData);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ClientCertificates.Add(cert);
                req.ContentLength = requestBytes.Length;
                System.IO.Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
                string xmlResult = sr.ReadToEnd();
                sr.Close();
                res.Close();
                var xml = System.Xml.Linq.XDocument.Parse(xmlResult);
                var returnCode = xml.Element("xml").Element("return_code").Value;
                var resultCode = xml.Element("xml").Element("result_code").Value;
                msg = xml.Element("xml").Element("return_msg").Value;

                if (returnCode.ToUpper().Equals("SUCCESS") && (resultCode.ToUpper().Equals("SUCCESS")))
                {
                    weixinRefundId = xml.Element("xml").Element("refund_id").Value;
                    return true;

                }
                else
                {
                    msg = xml.Element("xml").Element("err_code_des").Value;

                }

            }
            catch (Exception ex)
            {
                msg = ex.ToString();

            }
            return false;
        }

        /// <summary>
        /// 查询微信退款
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="amount">退款金额（单位 元）</param>
        /// <param name="appId">appid</param>
        /// <param name="mchId">商户id</param>
        /// <param name="key">密钥</param>
        /// <param name="openid">用户openid</param>
        /// <param name="ip">用户ip</param>
        /// <param name="desc">说明</param>
        /// <returns></returns>
        public bool QueryWeixinRefund(string refundNumber, string weixinRefundId, string appId, string mchId, string key, out string msg)
        {
            msg = "false";
            try
            {

                string nonStr = Payment.WeiXin.CommonUtil.CreateNoncestr();//随机串
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("appid", appId);
                dic.Add("mch_id", mchId);
                dic.Add("nonce_str", nonStr);
                dic.Add("out_refund_no", refundNumber);
                dic.Add("refund_id", weixinRefundId);
                string strTemp = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(dic, false);
                string sign = Payment.WeiXin.MD5SignUtil.Sign(strTemp, key);
                dic = (from entry in dic
                       orderby entry.Key ascending
                       select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                dic.Add("sign", sign);
                string postData = Payment.WeiXin.CommonUtil.ArrayToXml(dic);
                string url = "https://api.mch.weixin.qq.com/pay/refundquery";
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(postData);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = requestBytes.Length;
                System.IO.Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
                string xmlResult = sr.ReadToEnd();
                sr.Close();
                res.Close();
                var xml = System.Xml.Linq.XDocument.Parse(xmlResult);
                var returnCode = xml.Element("xml").Element("return_code").Value;
                //var return_msg = xml.Element("xml").Element("return_msg").Value;
                msg = xml.Element("xml").Element("refund_status_0").Value;
                if (returnCode.ToUpper().Equals("SUCCESS"))
                {
                    return true;

                }



            }
            catch (Exception ex)
            {
                msg = ex.Message;

            }
            return false;
        }


        /// <summary>
        /// 查询微信付款状态
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="appId">微信公众号AppId</param>
        /// <param name="mchId">商户号</param>
        /// <param name="key">商户key</param>
        /// <returns></returns>
        public bool GetWeixinTransferInfo(string orderId, string appId, string mchId, string key)
        {
            try
            {

                X509Certificate2 cert = new X509Certificate2(string.Format("D:\\ApiClientCer\\{0}\\apiclient_cert.p12", WebsiteOwner), mchId);
                string nonStr = Payment.WeiXin.CommonUtil.CreateNoncestr();//随机串
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("appid", appId);
                dic.Add("mch_id", mchId);
                dic.Add("nonce_str", nonStr);
                dic.Add("partner_trade_no", orderId);
                string strTemp = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(dic, false);
                string sign = Payment.WeiXin.MD5SignUtil.Sign(strTemp, key);
                dic = (from entry in dic
                       orderby entry.Key ascending
                       select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                dic.Add("sign", sign);
                string postData = Payment.WeiXin.CommonUtil.ArrayToXml(dic);
                string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/gettransferinfo";
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(postData);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ClientCertificates.Add(cert);
                req.ContentLength = requestBytes.Length;
                System.IO.Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
                string xmlResult = sr.ReadToEnd();
                sr.Close();
                res.Close();
                var result = System.Xml.Linq.XDocument.Parse(xmlResult);
                var returnCode = result.Element("xml").Element("return_code").Value;
                var resultCode = result.Element("xml").Element("status").Value;
                if (returnCode.ToUpper().Equals("SUCCESS") && (resultCode.ToUpper().Equals("SUCCESS")))
                {

                    return true;

                }





            }
            catch (Exception ex)
            {


            }
            return false;

        }



        /// <summary>
        /// 微信支付验证签名
        /// </summary>
        /// <param name="dicAll">接收到的所有参数</param>
        /// <param name="key">商户 key</param>
        /// <returns></returns>
        public bool VerifySignatureWx(Dictionary<string, string> dicAll, string key)
        {
            //所有参数排序
            dicAll = dicAll.OrderBy(p => p.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            //验签参数
            Dictionary<string, string> dicSign = dicAll.Where(p => !p.Key.Equals("sign")).ToDictionary(pair => pair.Key, pair => pair.Value);//sign 参数不参与签名
            if (Payment.WeiXin.MD5SignUtil.VerifySignature(Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(dicSign, false), dicAll["sign"], key))//验证签名
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 微信支付请求模型
        /// </summary>
        public class WXPayReq
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
            /// 数据包
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
            /// <summary>
            /// 支付微信地址
            /// </summary>
            public string codeUrl { get; set; }
        }

        /// <summary>
        ///创建支付宝手机支付请求表单
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="totalAmount">订单总金额</param>
        /// <param name="sellerAccountName">卖家支付宝账户</param>
        /// <param name="partner">商户号 PID</param>
        /// <param name="key">key</param>
        /// <param name="notifyUrl">异步通知地址（以异步通知为准）</param>
        /// <param name="subject">显示的订单信息(可留空)</param>
        /// <param name="callBackUrl">同步通知地址</param>
        /// <param name="merchantUrl">支付中断跳转地址</param>
        /// <returns></returns>
        public string GetAliPayRequestMobile(string orderId, double totalAmount, string sellerAccountName, string partner, string key, string notifyUrl, string subject = "", string callBackUrl = "", string merchantUrl = "")
        {
            try
            {
                //支付宝网关地址
                string gateWay = "http://wappaygw.alipay.com/service/rest.htm?";
                //返回格式
                string format = "xml";
                //必填，不需要修改
                //返回格式
                string verSion = "2.0";
                //必填，不需要修改
                //请求号
                string reqId = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(0, 99999);
                //必填，须保证每次请求都是唯一
                //请求业务参数详细
                string reqDataToken = "<direct_trade_create_req><notify_url>" + notifyUrl + "</notify_url><call_back_url>" + callBackUrl + "</call_back_url><seller_account_name>" + sellerAccountName + "</seller_account_name><out_trade_no>" + orderId + "</out_trade_no><subject>" + (!string.IsNullOrEmpty(subject) ? subject : string.Format("订单号:{0}", orderId)) + "</subject><total_fee>" + totalAmount.ToString() + "</total_fee><merchant_url>" + merchantUrl + "</merchant_url></direct_trade_create_req>";
                //必填
                //把请求参数打包成数组
                Dictionary<string, string> paraTempToken = new Dictionary<string, string>();
                paraTempToken.Add("partner", partner);
                paraTempToken.Add("_input_charset", "utf-8");
                paraTempToken.Add("sec_id", "MD5");
                paraTempToken.Add("service", "alipay.wap.trade.create.direct");
                paraTempToken.Add("format", format);
                paraTempToken.Add("v", verSion);
                paraTempToken.Add("req_id", reqId);
                paraTempToken.Add("req_data", reqDataToken);
                //建立请求
                string htmlTextToken = Payment.Alipay.Submit.BuildRequestMall(gateWay, paraTempToken, key);
                //URLDECODE返回的信息
                Encoding code = Encoding.GetEncoding("utf-8");
                string htmlTextTokenDecode = System.Web.HttpUtility.UrlDecode(htmlTextToken, code);
                //解析远程模拟提交后返回的信息
                Dictionary<string, string> dicHtmlTextToken = Payment.Alipay.Submit.ParseResponse(htmlTextTokenDecode);
                //获取token
                string requestToken = dicHtmlTextToken["request_token"];
                //业务详细
                string reqData = "<auth_and_execute_req><request_token>" + requestToken + "</request_token></auth_and_execute_req>";
                //必填
                //把请求参数打包成数组
                Dictionary<string, string> paraTemp = new Dictionary<string, string>();
                paraTemp.Add("partner", partner);
                paraTemp.Add("_input_charset", "utf-8");
                paraTemp.Add("sec_id", "MD5");
                paraTemp.Add("service", "alipay.wap.auth.authAndExecute");
                paraTemp.Add("format", format);
                paraTemp.Add("v", verSion);
                paraTemp.Add("req_data", reqData);
                //建立请求表单
                return Payment.Alipay.Submit.BuildRequestMall(gateWay, paraTemp, "get", "确认", key);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetAliPayRequestApp(string orderId, double totalAmount,string appid, string privatekey,string publickey,string signType,string notifyUrl, string subject = "", string body = "")
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", appid, privatekey, "json", "1.0", signType, publickey, "utf-8", false);
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            //SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
            AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
            if (string.IsNullOrWhiteSpace(subject)) subject = "订单号：" + orderId;
            model.Body = string.IsNullOrWhiteSpace(body) ? subject : body;
            model.Subject = subject;
            model.TotalAmount = totalAmount.ToString();
            //model.ProductCode = "QUICK_MSECURITY_PAY";
            model.OutTradeNo = orderId;
            model.TimeoutExpress = "30m";
            request.SetBizModel(model);
            request.SetNotifyUrl(notifyUrl);
            //这里和普通的接口调用不同，使用的是sdkExecute
            AlipayTradeAppPayResponse response = client.SdkExecute(request);
            return response.Body;
            //HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题
            //Response.Write(HttpUtility.HtmlEncode(response.Body));

            ////页面输出的response.Body就是orderString 可以直接给客户端请求，无需再做处理。
            //string total_fee = totalAmount.ToString("N");
            //if (string.IsNullOrWhiteSpace(subject)) subject = "订单：" + orderId;
            //if (string.IsNullOrWhiteSpace(body)) body = subject + "，金额：" + total_fee;

            //Dictionary<string, string> paraTempToken = new Dictionary<string, string>();
            //paraTempToken.Add("service", "mobile.securitypay.pay");
            //paraTempToken.Add("partner", partner);
            //paraTempToken.Add("_input_charset", "UTF-8");
            //paraTempToken.Add("out_trade_no", orderId);
            //paraTempToken.Add("subject", subject);
            //paraTempToken.Add("payment_type", "1");
            //paraTempToken.Add("seller_id", sellerAccountName);
            //paraTempToken.Add("total_fee", total_fee);
            //paraTempToken.Add("body", body);
            //paraTempToken.Add("it_b_pay", "1d");
            //paraTempToken.Add("notify_url", System.Web.HttpUtility.UrlEncode(notifyUrl));

            //string data = Core.CreateLinkString(paraTempToken);
            ////获得签名结果
            //string mysign = Submit.BuildRequestRsaSign(data, privatekey, "RSA", "UTF-8");
            //mysign = System.Web.HttpUtility.UrlEncode(mysign);
            //paraTempToken.Add("sign", mysign);
            //paraTempToken.Add("sign_type", "RSA");
            //string result = Core.CreateLinkString(paraTempToken,"\"");
            //return result;
        }

        /// <summary>
        /// 支付宝退款申请(退款无密接口)
        /// </summary>
        /// <param name="tanNo">支付宝交易号</param>
        /// <param name="batchNo">退款批次号,必须严格按以下格式:
        ///退款日期（ 8 位当天日期） +流水号
        ///（ 3～24 位，流水号可以接受数字或英文字符，建议使用数字，但不可接受“000”）。
        /// </param>
        /// <param name="refundAmount">退款金额</param>
        /// <param name="notifyUrl">通知地址 默认 http://域名/Alipay/NotifyRefund.aspx</param>
        /// <param name="msg">提示消息</param>
        /// <param name="remark">退款备注,选填</param>
        /// <returns>true 已提交受理 false 申请失败</returns>
        public bool AlipayRefund(string tanNo, string batchNo, decimal refundAmount, string notifyUrl, out string msg, string remark = "")
        {
            DateTime dtNow = DateTime.Now;
            msg = "ok";
            if (!batchNo.StartsWith(dtNow.ToString("yyyyMMdd")))
            {
                msg = "退款批次号格式错误,批次号以8位当天日期开头";
                return false;
            }
            if (batchNo.Length < 11 || batchNo.Length > 32)
            {
                msg = "退款批次号长度在11-32之间";
                return false;
            }
            if (refundAmount <= 0)
            {
                msg = "退款金额需大于0";
                return false;
            }
            PayConfig config = GetPayConfig();
            Dictionary<string, string> para = new Dictionary<string, string>();
            para.Add("partner", config.Partner);
            para.Add("_input_charset", "UTF-8");
            para.Add("service", "refund_fastpay_by_platform_nopwd");
            para.Add("notify_url", notifyUrl);
            para.Add("seller_email", config.Seller_Account_Name);
            para.Add("refund_date", dtNow.ToString("yyyy-MM-dd hh:mm:ss"));
            para.Add("batch_no", batchNo);
            para.Add("batch_num", "1");
            para.Add("detail_data", string.Format("{0}^{1}^{2}", tanNo, refundAmount, string.IsNullOrEmpty(remark) ? "退款" : remark));
            string strRequestData = Payment.Alipay.Submit.BuildRequestParaToStringMall(para, System.Text.Encoding.GetEncoding("UTF-8"), config.PartnerKey);
            ZentCloud.Common.HttpInterFace request = new Common.HttpInterFace();
            string result = request.GetWebRequest(strRequestData, "https://mapi.alipay.com/gateway.do", System.Text.Encoding.GetEncoding("UTF-8"));
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            string isSuccess = xmlDoc.SelectSingleNode("/alipay/is_success").InnerText;
            if (isSuccess.ToUpper() == "T")
            {
                return true;
            }
            else
            {
                msg = xmlDoc.SelectSingleNode("/alipay/error").InnerText;
                return false;
            }

        }


        /// <summary>
        /// 支付宝验签
        /// </summary>
        /// <param name="dicAll">接收到的所有参数</param>
        /// <param name="partner">商户号 PID</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public bool VerifySignatureAlipay(Dictionary<string, string> dicAll, string partner, string key)
        {
            return new Payment.Alipay.Notify().VerifyNotifyMall(dicAll, dicAll["sign"], partner, key);
        }

        #region 京东支付

        /// <summary>
        /// 是否配置京东支付
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool IsJDPay(string websiteOwner = "")
        {
            PayConfig payConfig = GetPayConfig(websiteOwner);
            if (payConfig == null)
            {
                return false;
            }
            return IsJDPay(payConfig);
        }
        public bool IsJDPay(PayConfig payConfig)
        {
            if (!string.IsNullOrEmpty(payConfig.JDPayMerchant))
                return true;
            return false;
        }
        public bool IsJDPay(out PayConfig payConfig, string websiteOwner = "")
        {
            payConfig = GetPayConfig(websiteOwner);
            if (payConfig == null)
            {
                return false;
            }
            return IsJDPay(payConfig);
        }

        /// <summary>
        /// 创建京东支付
        /// 返回form表单
        /// </summary>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="orderId">订单号</param>
        /// <param name="amount">订单金额</param>
        /// <param name="userId">下单用户</param>
        /// <param name="orderType">订单类型0-实物，1-虚拟</param>
        /// <param name="callbackUrl">同步通知地址</param>
        /// <param name="notifyUrl">异步通知地址</param>
        /// <returns></returns>
        public string CreateJDPayRequestMobile(out bool isSuccess, string orderId, decimal amount, string userId, string orderType, string callbackUrl, string notifyUrl,string orderMemo = "商城交易订单")
        {
            BLLMall bllMall = new BLLMall();
            string result = string.Empty;
            SortedDictionary<string, string> orderInfoDic = new SortedDictionary<string, string>();
            isSuccess = true;
            PayConfig payConfig;

            if (!IsJDPay(out payConfig))
            {
                isSuccess = false;
                result = "京东支付未配置";
                return result;
            }

            #region 设置参数
            orderInfoDic.Add("version", "V2.0");//版本号 当前固定填写：V2.0
            orderInfoDic.Add("merchant", payConfig.JDPayMerchant);
            orderInfoDic.AddOrReplace("device", WebsiteOwner);
            orderInfoDic.AddOrReplace("tradeNum", orderId);
            orderInfoDic.AddOrReplace("tradeName", orderMemo);//String（50）	商户提供的订单的标题/商品名称/关键字等
            orderInfoDic.AddOrReplace("tradeDesc", "订单号：" + orderId);//String(512)	商户提供的订单的具体描述信息
            orderInfoDic.AddOrReplace("tradeTime", DateTime.Now.ToString("yyyyMMddHHmmss"));
            orderInfoDic.AddOrReplace("amount", (amount * 100).ToString("F0"));
            orderInfoDic.AddOrReplace("currency", "CNY");//String(8)	货币类型，固定填CNY
            orderInfoDic.AddOrReplace("note", "订单号：" + orderId);
            orderInfoDic.AddOrReplace("callbackUrl", callbackUrl);//String(256)	支付成功后跳转的URL
            orderInfoDic.AddOrReplace("notifyUrl", notifyUrl);//String(256)	支付完成后，异步通知商户服务相关支付结果
            orderInfoDic.AddOrReplace("ip", Common.MySpider.GetClientIP());//String(16)	用户IP
            orderInfoDic.AddOrReplace("specCardNo", "");
            orderInfoDic.AddOrReplace("specId", "");
            orderInfoDic.AddOrReplace("specName", "");
            orderInfoDic.AddOrReplace("userType", "");
            orderInfoDic.AddOrReplace("userId", userId);
            orderInfoDic.AddOrReplace("expireTime", "");//Integer	订单的失效时长，单位：秒，失效后则不能再支付，默认失效时间为604800秒(7天)
            orderInfoDic.AddOrReplace("orderType", orderType);//是	String(3)	0-	实物，1-虚拟  ，我们平台根据是否需要物流来判断
            orderInfoDic.AddOrReplace("industryCategoryCode", "");
            #endregion

            #region 签名构造
            //签名构造
            List<String> unSignedKeyList = new List<string>();
            unSignedKeyList.Add("sign");
            String signStr = Payment.JDPay.SignUtil.signRemoveSelectedKeys(orderInfoDic, payConfig.JDPayRSAPrivateKey, unSignedKeyList);
            orderInfoDic.AddOrReplace("sign", signStr);
            #endregion

            #region des3加密
            //des3加密   --  为保证信息安全，表单中的各个字段除了merchant（商户号）、版本号（verion）、签名（sign）以外，其余字段全部采用3DES进行加密
            SortedDictionary<string, string> orderInfoDicTmp = new SortedDictionary<string, string>();
            byte[] key = Convert.FromBase64String(payConfig.JDPayDESKey);
            foreach (var item in orderInfoDic)
            {
                if (item.Key != "version" && item.Key != "merchant" && item.Key != "sign" && !string.IsNullOrWhiteSpace(item.Value))
                {
                    orderInfoDicTmp.Add(item.Key, Payment.JDPay.Des3.Des3EncryptECB(key, item.Value));
                }
                else
                {
                    orderInfoDicTmp.Add(item.Key, item.Value);
                }
            }
            orderInfoDic = orderInfoDicTmp;

            #endregion

            #region 创建表单
            //创建表单
            var payFormId = "jdPayForm" + Guid.NewGuid().ToString();
            StringBuilder payFormHtml = new StringBuilder();
            payFormHtml.AppendFormat("<form id='{0}' name='{0}' action='https://h5pay.jd.com/jdpay/saveOrder' method='post'>", payFormId);

            foreach (var item in orderInfoDic)
            {
                payFormHtml.Append("<input type='hidden' name='" + item.Key + "' value='" + item.Value + "'/>");
            }

            payFormHtml.AppendFormat("<input type='submit' value='立即支付' style='display:none;'></form>");
            payFormHtml.AppendFormat("<script>document.forms['{0}'].submit();</script>", payFormId);
            #endregion

            return payFormHtml.ToString();
        }

        /// <summary>
        /// 京东支付退款
        /// </summary>
        /// <param name="oTradeNum">原交易号</param>
        /// <param name="tradeNum">退款流水号</param>
        /// <param name="amount">退款金额</param>
        /// <param name="notifyUrl">异步通知地址</param>
        /// <param name="note">备注</param>
        /// <param name="msg">提示消息</param>
        /// <returns></returns>
        public bool JDPayRefund(string oTradeNum, string tradeNum, decimal amount, string notifyUrl, string note,out string msg)
        {
            msg="";
            PayConfig payConfig;
            if (!IsJDPay(out payConfig))
            {
                return false;
            }
            SortedDictionary<String, String> refundInfoDic = new SortedDictionary<string, string>();
            refundInfoDic.Add("version", "V2.0");
            refundInfoDic.Add("merchant", payConfig.JDPayMerchant);
            refundInfoDic.Add("tradeNum", tradeNum);
            refundInfoDic.Add("oTradeNum", oTradeNum);
            refundInfoDic.Add("amount", (amount * 100).ToString("F0"));
            refundInfoDic.Add("tradeTime", DateTime.Now.ToString("yyyyMMddHHmmss"));
            refundInfoDic.Add("notifyUrl", notifyUrl);
            refundInfoDic.Add("note", note);
            refundInfoDic.Add("currency", "CNY");
            string priKey = payConfig.JDPayRSAPrivateKey;
            string desKey = payConfig.JDPayDESKey;
            string pubKey = payConfig.JDPayRSAPublicKey;
            String reqXmlStr = Payment.JDPay.XMLUtil.encryptReqXml(priKey, desKey, refundInfoDic);
            string resultXml = Payment.JDPay.HttpUtil.Post("https://paygate.jd.com/service/refund", reqXmlStr);
            var resp= Payment.JDPay.XMLUtil.decryptResXml<Payment.JDPay.Model.RefundResponse>(Payment.JDPay.Config.JDPubKey, desKey, resultXml);
            if (resp.status == "1" || resp.status == "0")
            {
                return true;
            }
            else
            {
                msg = resp.result.desc;
                return false;
            }

            
        }



        #endregion

        /// <summary>
        /// 支付宝支付通知
        /// </summary>
        /// <param name="tradeStatus">状态</param>
        /// <param name="tradeNo">支付宝交易号</param>
        /// <param name="outTradeNo">订单号</param>
        /// <param name="parametersAll">参数</param>
        /// <param name="baseDomain">域名</param>
        /// <param name="msg">错误消息</param>
        /// <returns></returns>
        public bool AliPayMallNotify(string tradeStatus, string tradeNo, string outTradeNo, Dictionary<string, string> parametersAll, string baseDomain, out string msg)
        {
            msg = "";
            if (parametersAll.Count == 0)
            {
                msg = "无通知参数";
                return false;
            }
            if (tradeStatus != "TRADE_FINISHED" && tradeStatus != "TRADE_SUCCESS")
            {
                msg = tradeStatus;
                return false;
            }
            Notify aliNotify = new Notify();
            PayConfig payConfig = GetPayConfig();
            bool verifyResult = aliNotify.VerifyNotifyMall(parametersAll, parametersAll["sign"], payConfig.Partner, payConfig.PartnerKey);
            if(!verifyResult)
            {
                msg = "签名未通过";
                return false;
            }
            return PayMallNotify(tradeNo, outTradeNo, 1, baseDomain, out msg);
        }
        /// <summary>
        /// 支付宝App支付通知
        /// </summary>
        /// <param name="tradeStatus">状态</param>
        /// <param name="tradeNo">支付宝交易号</param>
        /// <param name="outTradeNo">订单号</param>
        /// <param name="parametersAll">参数</param>
        /// <param name="baseDomain">域名</param>
        /// <param name="msg">错误消息</param>
        /// <returns></returns>
        public bool AliPayAppMallNotify(string tradeStatus,string alipayAppId, string tradeNo, string outTradeNo, Dictionary<string, string> parametersAll, string baseDomain, out string msg)
        {
            msg = "";
            if (parametersAll.Count == 0)
            {
                msg = "无通知参数";
                return false;
            }
            if (tradeStatus != "TRADE_FINISHED" && tradeStatus != "TRADE_SUCCESS")
            {
                msg = tradeStatus;
                return false;
            }
            BLLAppManage bllApp = new BLLAppManage();
            AppManage app = bllApp.GetApp(WebsiteOwner, "", alipayAppId);
            bool verifyResult = AlipaySignature.RSACheckV1(parametersAll, app.AlipayPublickey, "utf-8", app.AlipaySignType, false);
            if (!verifyResult)
            {
                msg = "签名未通过";
                return false;
            }
            return PayMallNotify(tradeNo, outTradeNo, 1, baseDomain, out msg);
        }
        /// <summary>
        /// 微信支付通知
        /// </summary>
        /// <param name="tradeNo">微信交易号</param>
        /// <param name="outTradeNo">订单号</param>
        /// <param name="parametersAll">参数</param>
        /// <param name="baseDomain">域名</param>
        /// <param name="msg">错误消息</param>
        /// <returns></returns>
        public bool WeixinPayMallNotify(string tradeNo, string outTradeNo, Dictionary<string, string> parametersAll, string baseDomain, out string msg)
        {
            msg = "";
            if (parametersAll.Count == 0)
            {
                msg = "无通知参数";
                return false;
            }
            if (!(parametersAll["return_code"].Equals("SUCCESS") && parametersAll["result_code"].Equals("SUCCESS")))
            {
                msg = "返回信息有误";
                return false;
            }
            Notify aliNotify = new Notify();
            PayConfig payConfig = GetPayConfig();

            parametersAll = (from entry in parametersAll
                             orderby entry.Key ascending
                             select entry).ToDictionary(pair => pair.Key, pair => pair.Value);//全部参数排序

            if (!VerifySignatureWx(parametersAll, payConfig.WXPartnerKey))//验证签名
            {
                msg = "签名未通过";
                return false;
            }
            return PayMallNotify(tradeNo, outTradeNo, 2, baseDomain, out msg);
        }
        /// <summary>
        /// 支付异步通知
        /// </summary>
        /// <param name="tradeNo">交易号</param>
        /// <param name="outTradeNo">订单号</param>
        /// <param name="paymentType">支付类型</param>
        /// <param name="baseDomain">域名</param>
        /// <param name="msg">错误消息</param>
        /// <returns></returns>
        public bool PayMallNotify(string tradeNo, string outTradeNo, int paymentType, string baseDomain, out string msg)
        {
            msg = "";
            BLLMall bllMall = new BLLMall();
            BLLUser bllUser = new BLLUser();
            BLLCommRelation bllCommRelation = new BLLCommRelation();
            BLLWeixin bllWeiXin = new BLLWeixin();
            BLLEfast bllEfast = new BLLEfast();
            Open.EZRproSDK.Client yikeClient = new Open.EZRproSDK.Client();
            BLLPermission.BLLMenuPermission bllMenuPermission = new BLLPermission.BLLMenuPermission("");
            BllScore bllScore = new BllScore();
            BLLDistribution bllDis = new BLLDistribution();
            BLLCardCoupon bllCard = new BLLCardCoupon();
            var orderInfo = bllMall.GetOrderInfo(outTradeNo);
            if (orderInfo == null)
            {
                msg = "订单未找到";
                return false;
            }
            if (orderInfo.PaymentStatus.Equals(1))
            {
                msg = "订单已支付";
                return false;
            }
            orderInfo.PayTranNo = tradeNo;
            orderInfo.PaymentType = paymentType;

            //更新订单状态
            WXMallProductInfo tProductInfo = new WXMallProductInfo();
            UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, bllUser.WebsiteOwner);//下单用户信息
            string hasOrderIDs = "";
            int maxCount = 1;
            //Tolog("准备检查更新订单状态");
            if (BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType))
            {
                //Tolog("预订类型");
                #region 预约订单修改状态
                orderInfo.PaymentStatus = 1;
                orderInfo.PayTime = DateTime.Now;
                orderInfo.Status = "预约成功";

                #region 检查是否有预约成功的订单
                List<WXMallOrderDetailsInfo> tDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID, null, orderInfo.ArticleCategoryType, null, null);
                List<WXMallOrderDetailsInfo> oDetailList = bllMall.GetOrderDetailsList(null, tDetailList[0].PID, orderInfo.ArticleCategoryType, tDetailList.Min(p => p.StartDate), tDetailList.Max(p => p.EndDate));
                tProductInfo = bllMall.GetByKey<WXMallProductInfo>("PID", tDetailList[0].PID);
                maxCount = tProductInfo.Stock;
                List<string> hasOrderIDList = new List<string>();
                foreach (var item in tDetailList)
                {
                    List<WXMallOrderDetailsInfo> hasOrderDetailList = oDetailList.Where(p => !((item.StartDate >= p.EndDate && item.EndDate > p.EndDate) || (item.StartDate < p.StartDate && item.EndDate <= p.StartDate))).ToList();
                    if (hasOrderDetailList.Count >= maxCount)
                    {
                        hasOrderIDList.AddRange(hasOrderDetailList.Select(p => p.OrderID).Distinct());
                    }
                }
                hasOrderIDList = hasOrderIDList.Where(p => !p.Contains(orderInfo.OrderID)).ToList();
                if (hasOrderIDList.Count > 0)
                {
                    hasOrderIDList = hasOrderIDList.Distinct().ToList();
                    hasOrderIDs = ZentCloud.Common.MyStringHelper.ListToStr(hasOrderIDList, "'", ",");
                }
                #endregion 检查是否有预约成功的订单

                #endregion 预约订单修改状态
            }
            else
            {
                //Tolog("普通类型");
                #region 原订单修改状态
                orderInfo.PaymentStatus = 1;
                orderInfo.Status = "待发货";
                orderInfo.PayTime = DateTime.Now;
                //Tolog("更改状态start");
                //if (bllMall.GetWebsiteInfoModelFromDataBase().IsDistributionMall.Equals(1))
                //{
                orderInfo.GroupBuyStatus = "0";
                orderInfo.DistributionStatus = 1;

                //if (orderInfo.IsMain == 1)
                //{
                //    bllMall.Update(orderInfo, string.Format(" DistributionStatus=1"), string.Format("ParentOrderId='{0}'", orderInfo.OrderID));
                //}
                bllMall.Update(orderInfo, string.Format("PaymentStatus=1,Status='待发货',PayTime=GETDATE(),DistributionStatus=1"), string.Format("ParentOrderId='{0}'", orderInfo.OrderID));

                #region 活动订单
                if (orderInfo.OrderType == 4)
                {
                    ActivityDataInfo data = bllMall.Get<ActivityDataInfo>(string.Format(" OrderId='{0}'", orderInfo.OrderID));
                    if (data != null)
                    {
                        bllMall.Update(data, string.Format(" PaymentStatus=1"), string.Format("  OrderId='{0}'", orderInfo.OrderID));
                    }
                }
                #endregion
                #endregion
            }
            bool result = false;

            if (BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType))
            {
                #region 预约订单修改状态
                if (string.IsNullOrWhiteSpace(hasOrderIDs)) hasOrderIDs = "'0'";
                result = bllMall.Update(new WXMallOrderInfo(),
                    string.Format("PaymentStatus={0},PayTime=GetDate(),Status='{1}'", 1, "预约成功"),
                    string.Format("OrderID='{0}' and WebsiteOwner='{4}' AND (select count(1) from [ZCJ_WXMallOrderInfo] where Status='{3}' and WebsiteOwner='{4}' and  OrderID IN({1}))<{2}",
                        orderInfo.OrderID, hasOrderIDs, maxCount, "预约成功", bllMall.WebsiteOwner)
                    ) > 0;
                if (result)
                {
                    // #region 交易成功加积分
                    //增加积分 （慧聚不需要）
                    //if (orderInfo.TotalAmount > 0)
                    //{
                    //    ScoreConfig scoreConfig = bllScore.GetScoreConfig();
                    //    int addScore = 0;
                    //    if (scoreConfig != null && scoreConfig.OrderAmount > 0 && scoreConfig.OrderScore > 0)
                    //    {
                    //        addScore = (int)(orderInfo.PayableAmount / (scoreConfig.OrderAmount / scoreConfig.OrderScore));
                    //    }
                    //    if (addScore > 0)
                    //    {
                    //        if (bllUser.Update(new UserInfo(), 
                    //            string.Format(" TotalScore+={0},HistoryTotalScore+={0}", addScore),
                    //            string.Format(" UserID='{0}'", orderInfo.OrderUserID)) > 0)
                    //        {
                    //            UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                    //            scoreRecord.AddTime = DateTime.Now;
                    //            scoreRecord.Score = addScore;
                    //            scoreRecord.ScoreType = "OrderSuccess";
                    //            scoreRecord.UserID = orderInfo.OrderUserID;
                    //            scoreRecord.AddNote = "预约-交易成功获得积分";
                    //            bllMall.Add(scoreRecord);
                    //        }
                    //    }
                    //}
                    // #endregion

                    #region 修改其他预约订单为预约失败 返还积分

                    if (BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType) && !string.IsNullOrWhiteSpace(hasOrderIDs))
                    {
                        int tempCount = 0;
                        List<WXMallOrderInfo> tempList = bllMall.GetOrderList(0, 1, "", out tempCount, "预约成功", null, null, null,
                                null, null, null, null, null, null, null, orderInfo.ArticleCategoryType, hasOrderIDs);
                        tempCount = tempCount + 1; //加上当前订单的数量
                        if (tempCount >= maxCount)
                        {
                            tempList = bllMall.GetColOrderListInStatus("'待付款','待审核'", hasOrderIDs, "OrderID,OrderUserID,UseScore", bllMall.WebsiteOwner);
                            if (tempList.Count > 0)
                            {
                                string stopOrderIds = ZentCloud.Common.MyStringHelper.ListToStr(tempList.Select(p => p.OrderID).ToList(), "'", ",");
                                tempList = tempList.Where(p => p.UseScore > 0).ToList();
                                foreach (var item in tempList)
                                {
                                    orderUserInfo.TotalScore += item.UseScore;
                                    if (bllUser.Update(new UserInfo(), string.Format(" TotalScore+={0}", item.UseScore),
                                        string.Format(" UserID='{0}'", item.OrderUserID)) > 0)
                                    {
                                        UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                                        scoreRecord.AddTime = DateTime.Now;
                                        scoreRecord.Score = item.UseScore;
                                        scoreRecord.TotalScore = orderUserInfo.TotalScore;
                                        scoreRecord.ScoreType = "OrderCancel";
                                        scoreRecord.UserID = item.OrderUserID;
                                        scoreRecord.RelationID = item.OrderID;
                                        scoreRecord.AddNote = "预约-订单失败返还积分";
                                        scoreRecord.WebSiteOwner = item.WebsiteOwner;
                                        bllMall.Add(scoreRecord);
                                    }
                                }
                                bllMall.Update(new WXMallOrderInfo(),
                                    string.Format("Status='{0}'", "预约失败"),
                                    string.Format("OrderID In ({0}) and WebsiteOwner='{1}'", stopOrderIds, bllMall.WebsiteOwner));
                            }
                        }
                        //Tolog("更改修改其他预约为预约失败");
                    }
                    #endregion

                }
                #endregion
            }
            else
            {
                result = bllMall.Update(orderInfo);
            }
            if (result)
            {
                #region 拼团订单
                if (orderInfo.OrderType == 2)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(orderInfo.GroupBuyParentOrderId))
                        {
                            var parentOrderInfo = bllMall.GetOrderInfo(orderInfo.GroupBuyParentOrderId);

                            if (parentOrderInfo.Ex10 == "1")
                            {
                                if (bllMall.GetCount<WXMallOrderInfo>(string.Format("PaymentStatus=1 And GroupBuyParentOrderId='{0}' ", parentOrderInfo.OrderID)) >= parentOrderInfo.PeopleCount)
                                {
                                    bllMall.Update(new WXMallOrderInfo(), string.Format("Status='已取消'"), string.Format("  GroupBuyParentOrderId='{0}' And PaymentStatus=0", parentOrderInfo.OrderID));
                                    parentOrderInfo.GroupBuyStatus = "1";
                                    bllMall.Update(parentOrderInfo);

                                }
                            }
                            else
                            {
                                if (bllMall.GetCount<WXMallOrderInfo>(string.Format("PaymentStatus=1 And GroupBuyParentOrderId='{0}' Or OrderId='{0}'", parentOrderInfo.OrderID)) >= parentOrderInfo.PeopleCount)
                                {
                                    bllMall.Update(new WXMallOrderInfo(), string.Format("Status='已取消'"), string.Format("  GroupBuyParentOrderId='{0}' And PaymentStatus=0", parentOrderInfo.OrderID));
                                    parentOrderInfo.GroupBuyStatus = "1";
                                    bllMall.Update(parentOrderInfo);

                                }
                            }

                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                #endregion
                Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(orderInfo.WebsiteOwner);

                //Tolog("更改状态true");

                #region Efast同步
                //判读当前站点是否需要同步到驿氪和efast
                if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncEfast, bllCommRelation.WebsiteOwner, ""))
                {
                    try
                    {
                        //Tolog("开始同步efast");

                        string outOrderId = string.Empty, syncMsg = string.Empty;
                        var syncResult = bllEfast.CreateOrder(orderInfo.OrderID, out outOrderId, out syncMsg);
                        if (syncResult)
                        {
                            orderInfo.OutOrderId = outOrderId;
                            bllMall.Update(orderInfo);
                        }
                        //Tolog(string.Format("efast订单同步结果:{0},订单号：{1}，提示信息：{2}", syncResult, outOrderId, msg));

                    }
                    catch (Exception ex)
                    {
                        //Tolog("efast订单同步异常：" + ex.Message);
                    }
                }
                #endregion

                #region 驿氪同步
                if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                {
                    try
                    {

                        //Tolog("开始同步驿氪");
                        //同步成功订单到驿氪

                        //UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);
                        //if ((!string.IsNullOrEmpty(orderUserInfo.Ex1)) && (!string.IsNullOrEmpty(orderUserInfo.Ex2)) && (!string.IsNullOrEmpty(orderUserInfo.Phone)))
                        //{
                        //    client.BonusUpdate(orderUserInfo.Ex2, -(orderInfo.UseScore), "商城下单使用积分" + orderInfo.UseScore);

                        //}
                        var uploadOrderResult = yikeClient.OrderUpload(orderInfo);

                        //Tolog(string.Format("驿氪订单同步结果：{0}", Common.JSONHelper.ObjectToJson(uploadOrderResult)));
                    }
                    catch (Exception ex)
                    {
                        //Tolog("驿氪订单同步异常：" + ex.Message);

                    }
                }
                #endregion

                #region 付款加积分
                try
                {
                    bllUser.AddUserScoreDetail(orderInfo.OrderUserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.OrderPay), bllMall.WebsiteOwner, null, null);
                }
                catch (Exception)
                { }

                #endregion


                #region 消息通知
                if (!BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType))
                {
                    try
                    {
                        var productName = bllMall.GetOrderDetailsList(orderInfo.OrderID)[0].ProductName;
                        string remark = "";
                        if (!string.IsNullOrEmpty(orderInfo.OrderMemo))
                        {
                            remark = string.Format("客户留言:{0}", orderInfo.OrderMemo);
                        }
                        bllWeiXin.SendTemplateMessageToKefu("有新的订单", string.Format("订单号:{0}\\n订单金额:{1}元\\n收货人:{2}\\n电话:{3}\\n商品:{4}\\n{5}", orderInfo.OrderID, orderInfo.TotalAmount, orderInfo.Consignee, orderInfo.Phone, productName, remark));
                        if (orderInfo.OrderType != 4)//付费的活动不发消息
                        {
                            if (orderInfo.WebsiteOwner != "jikuwifi")
                            {
                                string url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderDetail/{1}#/orderDetail/{1}", baseDomain, orderInfo.OrderID);
                                if (orderInfo.IsMain == 1)
                                {
                                    url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderList#/orderList", baseDomain);
                                }
                                bllWeiXin.SendTemplateMessageNotifyComm(orderUserInfo, "订单已成功支付，我们将尽快发货，请保持手机畅通等待物流送达！", string.Format("订单号:{0}\\n订单金额:{1}元\\n收货人:{2}\\n电话:{3}\\n商品:{4}...\\n查看详情", orderInfo.OrderID, orderInfo.TotalAmount, orderInfo.Consignee, orderInfo.Phone, productName), url);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    try
                    {
                        bllWeiXin.SendTemplateMessageToKefu(orderInfo.Status, string.Format("预约:{2}\\n订单号:{0}\\n订单金额:{1}元\\n预约人:{3}\\n预约人手机:{4}", orderInfo.OrderID, orderInfo.TotalAmount, tProductInfo.PName, orderUserInfo.TrueName, orderUserInfo.Phone));
                        bllWeiXin.SendTemplateMessageNotifyComm(orderUserInfo, orderInfo.Status, string.Format("预约:{2}\\n订单号:{0}\\n订单金额:{1}元", orderInfo.OrderID, orderInfo.TotalAmount, tProductInfo.PName));
                    }
                    catch (Exception)
                    {
                    }
                }
                #endregion
                WebsiteInfo websiteInfo = bllMall.Get<WebsiteInfo>(string.Format(" WebsiteOwner='{0}'", orderInfo.WebsiteOwner));
                #region 分销相关
                try
                {
                    if (bllMenuPermission.CheckUserAndPmsKey(websiteInfo.WebsiteOwner, BLLPermission.Enums.PermissionSysKey.OnlineDistribution, websiteInfo.WebsiteOwner))
                    {
                        if (string.IsNullOrWhiteSpace(orderUserInfo.DistributionOwner))
                        {
                            if (websiteInfo.DistributionRelationBuildMallOrder == 1)
                            {
                                orderUserInfo.DistributionOwner = orderInfo.WebsiteOwner;
                                bllMall.Update(orderUserInfo);
                            }
                        }
                        bllDis.AutoUpdateLevel(orderInfo);
                        bllDis.TransfersEstimate(orderInfo);
                        bllDis.SendMessageToUser(orderInfo);
                        bllDis.UpdateDistributionSaleAmountUp(orderInfo);

                    }
                }
                catch (Exception ex)
                {
                    //Tolog("设置分销员异常：" + ex.Message + " 用户id：" + orderUserInfo.UserID);
                }
                #endregion
                #region 宏巍通知
                try
                {
                    if (websiteInfo.IsUnionHongware == 1)
                    {
                        hongWareClient.OrderNotice(orderUserInfo.WXOpenId, orderInfo.OrderID);
                    }
                }
                catch (Exception)
                {
                }
                #endregion
                bllCard.Give(orderInfo.TotalAmount, orderUserInfo);
                string v1ProductId = Common.ConfigHelper.GetConfigString("YGBV1ProductId");
                string v2ProductId = Common.ConfigHelper.GetConfigString("YGBV2ProductId");
                string v1CouponId = Common.ConfigHelper.GetConfigString("YGBV1CouponId");
                string v2CouponId = Common.ConfigHelper.GetConfigString("YGBV2CouponId");
                //更新销量
                List<WXMallOrderDetailsInfo> orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
                foreach (var item in orderDetailList)
                {
                    item.IsComplete = 1;
                    bllMall.Update(item);
                    #region 购买指定商品发送指定的优惠券
                    if (!string.IsNullOrEmpty(v1ProductId) && !string.IsNullOrEmpty(v1CouponId) && item.PID == v1ProductId)
                    {
                        bllCard.SendCardCouponsByCurrUserInfo(orderUserInfo, v1CouponId);
                    }

                    if (!string.IsNullOrEmpty(v2ProductId) && !string.IsNullOrEmpty(v2CouponId) && item.PID == v2ProductId)
                    {
                        bllCard.SendCardCouponsByCurrUserInfo(orderUserInfo, v2CouponId);
                    }
                    #endregion
                }
                bllMall.UpdateProductSaleCount(orderInfo);
                return true;
            }
            return false;
        }
    }
}
