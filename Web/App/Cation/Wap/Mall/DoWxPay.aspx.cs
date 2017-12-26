using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using Payment.WeiXin;
using System.Net;
using System.IO;
using System.Xml.Linq;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class DoWxPay : System.Web.UI.Page
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        public WXMallOrderInfo model = new WXMallOrderInfo();
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 微信支付请求字符串
        /// </summary>
        public string WxPayReq = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                #region 检查订单是否可以支付
                if (Request["oid"] == null)
                {
                    Response.Write("订单无效");
                    Response.End();
                }
                int OrderId;
                if (!int.TryParse(Request["oid"], out OrderId))
                {
                    Response.Write("订单无效");
                    Response.End();
                }
                model = bllMall.GetOrderInfo(OrderId.ToString());
                if (model == null)
                {
                    Response.Write("订单无效");
                    Response.End();

                }
                if (!model.PaymentStatus.Equals(0))
                {
                    Response.Write("订单不是未付款状态");
                    Response.End();
                }
                if (!model.PaymentType.Equals(2))
                {
                    Response.Write("订单不属于微信支付");
                    Response.End();
                }
                if (string.IsNullOrEmpty(model.PaymentTypeAutoId))
                {
                    Response.Write("无效支付方式");
                    Response.End();
                }
                WXMallPaymentType paymentType = bllMall.GetPaymentType(int.Parse(model.PaymentTypeAutoId));
                if (paymentType == null)
                {
                    Response.Write("无效支付方式");
                    Response.End();
                } 
                #endregion

                
                var non_str=Payment.WeiXin.CommonUtil.CreateNoncestr();//随机串

                #region 获取微信支付预支付ID 
                if (string.IsNullOrEmpty(model.WXPrepay_Id))//还没有预支付ID
                {
                    //
                    Dictionary<string, string> Dic = new Dictionary<string, string>();
                    Dic.Add("appid", paymentType.WXAppId);

                    Dic.Add("body", "订单号" + model.OrderID);

                    Dic.Add("mch_id", paymentType.WXPartner);

                    Dic.Add("nonce_str", non_str);

                    Dic.Add("out_trade_no", model.OrderID);

                    Dic.Add("openid", bllMall.GetCurrentUserInfo().WXOpenId);

                    Dic.Add("spbill_create_ip", Request.UserHostAddress);

                    Dic.Add("total_fee", (model.TotalAmount * 100).ToString("F0"));

                    Dic.Add("notify_url", string.Format("http://{0}/WxPayNotify/Notify.aspx", Request.Url.Host));

                    Dic.Add("trade_type", "JSAPI");
                    string strtemp = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(Dic, false);
                    string sign = MD5SignUtil.Sign(strtemp, paymentType.WXPartnerKey);

                    Dic = (from entry in Dic
                           orderby entry.Key ascending
                           select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                    Dic.Add("sign", sign);

                    string postdata = Payment.WeiXin.CommonUtil.ArrayToXml(Dic);
                    string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(postdata);
                    req.Method = "POST";
                    req.ContentType = "application/x-www-form-urlencoded";
                    req.ContentLength = requestBytes.Length;
                    Stream requestStream = req.GetRequestStream();
                    requestStream.Write(requestBytes, 0, requestBytes.Length);
                    requestStream.Close();

                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
                    string backstr = sr.ReadToEnd();
                    sr.Close();
                    res.Close();
                    var result = XDocument.Parse(backstr);
                    var return_code = result.Element("xml").Element("return_code").Value;
                    //if (!return_code.ToUpper().Equals("SUCCESS"))
                    //{
                    //    Response.Write(backstr);
                    //    return;
                    //}
                    var rusult_code = result.Element("xml").Element("result_code").Value;
                    if (return_code.ToUpper().Equals("SUCCESS") && (rusult_code.ToUpper().Equals("SUCCESS")))
                    {
                        var prepay_id = result.Element("xml").Element("prepay_id").Value;
                        model.WXPrepay_Id = prepay_id;
                        bllMall.Update(model);//更新订单 微信预支付ID

                    }

                    //
                } 
                #endregion

                #region 生成微信支付请求
                WXPayReq reqwx = new WXPayReq();
                string timeStamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
                reqwx.appId = paymentType.WXAppId;
                reqwx.nonceStr = non_str;
                reqwx.package = "prepay_id=" + model.WXPrepay_Id;
                reqwx.signType = "MD5";
                reqwx.timeStamp = timeStamp;

                Dictionary<string, string> DicNew = new Dictionary<string, string>();
                DicNew.Add("appId", reqwx.appId);
                DicNew.Add("timeStamp", reqwx.timeStamp);
                DicNew.Add("nonceStr", reqwx.nonceStr);
                DicNew.Add("package", reqwx.package);
                DicNew.Add("signType", "MD5");
                string strtemp1 = Payment.WeiXin.CommonUtil.FormatQueryParaMap(DicNew);
                string PaySign = MD5SignUtil.Sign(strtemp1, paymentType.WXPartnerKey);
                reqwx.paySign = PaySign;
                WxPayReq = ZentCloud.Common.JSONHelper.ObjectToJson(reqwx); 
                #endregion
                


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
        private class WXPayReq {
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
        
        }
    }
}