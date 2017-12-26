using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Score
{
    /// <summary>
    /// Recharge 的摘要说明
    /// </summary>
    public class Recharge : BaseHandlerNeedLoginNoAction
    {
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        public void ProcessRequest(HttpContext context)
        {
            string scoreStr = context.Request["score"];
            decimal score = Convert.ToDecimal(scoreStr);
            if (score == 0)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "积分不能为0";
                bllKeyValueData.ContextResponse(context, apiResp);
                return;
            }
            if (CurrentUserInfo == null)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                apiResp.msg = "您还没有登录";
                bllKeyValueData.ContextResponse(context, apiResp);
                return;
            }
            BllOrder bllOrder = new BllOrder();
            BllPay bllPay = new BllPay();
            PayConfig payConfig = bllPay.GetPayConfig();

            string rechargeValue = bllKeyValueData.GetDataVaule("Recharge", "100", bllKeyValueData.WebsiteOwner);
            decimal rechargeFee = Convert.ToDecimal(rechargeValue);
            decimal totalFee = rechargeFee / 100 * score;

            string websiteOwner = bllKeyValueData.WebsiteOwner;

            OrderPay orderPay = new OrderPay();
            orderPay.OrderId = bllOrder.GetGUID(TransacType.CommAdd);
            orderPay.InsertDate = DateTime.Now;
            orderPay.Status = 0;
            orderPay.Type = "1";
            orderPay.Subject = "积分充值";
            orderPay.Total_Fee = totalFee;
            orderPay.UserId = CurrentUserInfo.UserID;
            orderPay.WebsiteOwner = websiteOwner;
            orderPay.Ex1 = scoreStr;

            var non_str = Payment.WeiXin.CommonUtil.CreateNoncestr();//随机串
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("appid", payConfig.WXAppId);
            dic.Add("body", "订单" + orderPay.OrderId);
            dic.Add("mch_id", payConfig.WXMCH_ID);//商户号
            dic.Add("nonce_str", non_str);
            dic.Add("out_trade_no", orderPay.OrderId);
            dic.Add("spbill_create_ip", context.Request.UserHostAddress);
            dic.Add("total_fee", (totalFee * 100).ToString("F0"));
            dic.Add("notify_url", string.Format("http://{0}/WxPayNotify/DoPayWxNotify.aspx", context.Request.Url.Authority));
            dic.Add("trade_type", "NATIVE");
            dic.Add("product_id", orderPay.OrderId);
            string strtemp = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(dic, false);
            string sign = Payment.WeiXin.MD5SignUtil.Sign(strtemp, payConfig.WXPartnerKey);

            dic = (from entry in dic
                   orderby entry.Key ascending
                   select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
            dic.Add("sign", sign);

            string postData = Payment.WeiXin.CommonUtil.ArrayToXml(dic);
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(postData);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = requestBytes.Length;
            Stream requestStream = req.GetRequestStream();
            requestStream.Write(requestBytes, 0, requestBytes.Length);
            requestStream.Close();

            System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
            string backStr = sr.ReadToEnd();
            sr.Close();
            res.Close();
            var result = System.Xml.Linq.XDocument.Parse(backStr);
            var return_code = result.Element("xml").Element("return_code").Value;

            if (!return_code.ToUpper().Equals("SUCCESS"))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "预支付失败";
                apiResp.result = new
                {
                    return_msg = result.Element("xml").Element("return_msg").Value,
                    post_data = postData
                };
                bllKeyValueData.ContextResponse(context, apiResp);
                return;
            }

            var rusult_code = result.Element("xml").Element("result_code").Value;
            if (return_code.ToUpper().Equals("SUCCESS") && (rusult_code.ToUpper().Equals("SUCCESS")))
            {
                var prepay_id = result.Element("xml").Element("prepay_id").Value;
                var code_url = result.Element("xml").Element("code_url").Value;
                orderPay.Trade_No = prepay_id;
                bllOrder.Add(orderPay);

                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                apiResp.msg = "预支付完成";
                apiResp.status = true;
                apiResp.result = new
                {
                    orderId = orderPay.OrderId,
                    prepay_id = prepay_id,
                    code_url = code_url
                };
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.result = result.Element("xml").Element("err_code_des").Value;
                apiResp.msg = "预支付失败";
            }
            bllKeyValueData.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}