using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.IO;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 支付
    /// </summary>
    public class Payment : BaseHandlerNeedLogin
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLMall bllMall = new BLLMall();
        /// <summary>
        /// 支付BLL
        /// </summary>
        BllPay bllPay = new BllPay();
        BLLAppManage bllApp = new BLLAppManage();
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
                resp.errmsg = "请在微信中打开";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            string orderId = context.Request["order_id"];
            decimal totalAmount = 0;//订单金额
            string appId = "";//微信AppId
            string mchId = "";//商户号
            string key = "";//api密钥
            string openId = "";//openid
            string ip = "";//ip
            string notifyUrl = baseUrl + "/WxPayNotify/NotifyV2.aspx";//支付通知地址
            string body = "";//订单内容
            if (string.IsNullOrEmpty(orderId))
            {
                resp.errcode = 1;
                resp.errmsg = "order_id 必传";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            var orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo != null)
            {
                totalAmount = orderInfo.TotalAmount;
                //if (orderInfo.OrderUserID != CurrentUserInfo.UserID)
                //{
                //    resp.errcode = 1;
                //    resp.errmsg = "订单号无效";
                //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                //}
                if (orderInfo.PaymentStatus == 1)
                {
                    resp.errcode = 1;
                    resp.errmsg = "该订单已经支付,不需重复支付";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                //更改支付方式
                bllMall.Update(orderInfo, " PaymentType=2 ", string.Format(" (OrderID = '{0}' Or ParentOrderId='{0}') ", orderInfo.OrderID));

                //resp.errcode = 1;
                //resp.errmsg = "订单号不存在";
                //return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                #region 众筹
                var orderInfoCrowd = bllMall.Get<CrowdFundRecord>(string.Format(" RecordID={0}", orderId));
                if (orderInfoCrowd == null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "订单号不存在";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (orderInfoCrowd.UserID != currentUserInfo.UserID)
                {
                    resp.errcode = 1;
                    resp.errmsg = "订单号无效";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (orderInfoCrowd.Status == 1)
                {
                    resp.errcode = 1;
                    resp.errmsg = "订单已经支付,不需重复支付";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                totalAmount = orderInfoCrowd.Amount;
                notifyUrl = baseUrl + "/WxPayNotify/NotifyCrowdFundV2.aspx";
                #endregion
            }

            PayConfig payConfig = bllPay.GetPayConfig();
            if (payConfig == null)
            {
                resp.errcode = 1;
                resp.errmsg = "该商户微信支付还没有配置";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if ((string.IsNullOrEmpty(payConfig.WXAppId)) || (string.IsNullOrEmpty(payConfig.WXMCH_ID)) || (string.IsNullOrEmpty(payConfig.WXPartnerKey)))
            {
                resp.errcode = 1;
                resp.errmsg = "该商户微信支付还没有配置";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            appId = payConfig.WXAppId;
            mchId = payConfig.WXMCH_ID;
            key = payConfig.WXPartnerKey;
            openId = currentUserInfo.WXOpenId;
            ip = context.Request.UserHostAddress;
            string payReqStr = bllPay.GetBrandWcPayRequest(orderId, totalAmount, appId, mchId, key, openId, ip, notifyUrl, body);
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
            resp.errmsg = "获取配置失败";
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
                resp.errmsg = "请不要在微信中打开";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (!bllPay.IsMobile)
            {
                resp.errcode = 1;
                resp.errmsg = "请用手机浏览器访问";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            if (string.IsNullOrEmpty(orderId))
            {
                resp.errcode = 1;
                resp.errmsg = "order_id 必传";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            var orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "订单号不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            //if (orderInfo.OrderUserID != CurrentUserInfo.UserID)
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "订单号无效";
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //}
            if (orderInfo.PaymentStatus == 1)
            {
                resp.errcode = 1;
                resp.errmsg = "订单已经支付,不需重复支付";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            
            //更改支付方式
            bllMall.Update(orderInfo, " PaymentType=1 ", string.Format(" (OrderID = '{0}' Or ParentOrderId='{0}') ", orderInfo.OrderID));

            PayConfig payConfig = bllPay.GetPayConfig();
            if (payConfig == null)
            {
                resp.errcode = 1;
                resp.errmsg = "请先填写支付配置信息";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if ((string.IsNullOrEmpty(payConfig.Seller_Account_Name)) || (string.IsNullOrEmpty(payConfig.Partner)) || (string.IsNullOrEmpty(payConfig.PartnerKey)))
            {
                resp.errcode = 1;
                resp.errmsg = "请先填写支付配置信息";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            string notifyUrl = baseUrl + "/Alipay/MallNotifyUrlV2.aspx";
            var payForm = bllPay.GetAliPayRequestMobile(orderInfo.OrderID, (double)orderInfo.TotalAmount, payConfig.Seller_Account_Name, payConfig.Partner, payConfig.PartnerKey, notifyUrl);


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
        /// 支付宝App支付
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string BuildAlipayAppRequest(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            string appId = context.Request["app_id"];
            string websiteOwner =bllPay.WebsiteOwner;
            if (string.IsNullOrEmpty(orderId))
            {
                resp.errcode = 1;
                resp.errmsg = "order_id 必传";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            var orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "订单号不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (orderInfo.OrderUserID != currentUserInfo.UserID)
            {
                resp.errcode = 1;
                resp.errmsg = "订单号无效";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (orderInfo.PaymentStatus == 1)
            {
                resp.errcode = 1;
                resp.errmsg = "订单已经支付,不需重复支付";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            
            BLLJIMP.Model.AppManage app = bllApp.GetApp(websiteOwner, appId);
            if (!bllApp.IsAppAlipay(app))
            {
                resp.errcode = 1;
                resp.errmsg = "未配置App支付宝支付";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            //更改支付方式
            //bllMall.Update(orderInfo, " PaymentType=1 ", string.Format(" (OrderID = '{0}' Or ParentOrderId='{0}') ", orderInfo.OrderID));

            string notifyUrl = baseUrl + "/Alipay/MallAppNotifyUrlV2.aspx";
            var payForm = bllPay.GetAliPayRequestApp(orderInfo.OrderID, (double)orderInfo.TotalAmount, app.AlipayAppId,
                app.AlipayPrivatekey, app.AlipayPublickey, app.AlipaySignType, notifyUrl);

            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                errcode = 0,
                pay_req = payForm
            });
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

        private void TologWxPay(string msg)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"D:\paylog.txt", true, System.Text.Encoding.GetEncoding("GB2312")))
                {
                    sw.WriteLine(string.Format(" {0}  {1} ", DateTime.Now.ToString(), msg));
                }
            }
            catch { }
        }


    }
}