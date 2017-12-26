using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Web;
namespace TakeOutSDK.Eleme
{

    /// <summary>
    /// 操作订单
    /// </summary>
    public class OrderHelper
    {
        
        /// <summary>
        /// API接口服务访问地址(测试)
        /// </summary>
        private const string sb_apiUrl="https://open-api-sandbox.shop.ele.me/api/v1/";
        /// <summary>
        /// API接口服务访问地址(正式)
        /// </summary>
        private const string apiUrl="https://open-api.shop.ele.me/api/v1/";


        public string AccessToken = "";

        public string AppKey = "";

        public string AppSecret = "";
        public OrderHelper(string toKen,string appKey, string appSecret)
        {
            this.AccessToken = toKen;
            this.AppKey = appKey;
            this.AppSecret = appSecret;
        }

        public string GetOrder(string orderId)
        {
            string result = string.Empty;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(sb_apiUrl);
            webRequest.ProtocolVersion = HttpVersion.Version10;
            webRequest.Method = "POST";
            webRequest.ContentType = "Content-type: application/json; charset=utf-8";
            var postData = GetParamsData(orderId);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData); // 转化
            using (Stream newStream = webRequest.GetRequestStream())
            {
                newStream.Write(byteArray, 0, byteArray.Length);
            }
            using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                }
            }
            webRequest.Abort();
            return result;
        }

        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <returns></returns>
        public string GetParamsData(string orderId)
        {
            long timestamp = ZentCloud.Common.DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now) /1000;
            string id = Guid.NewGuid().ToString();
            string signo = "eleme.order.getOrder" + this.AccessToken + "app_key=\"" + this.AppKey
                + "\"orderId=\"" + orderId + "\"timestamp=" + timestamp + this.AppSecret;
            string sign = ZentCloud.Common.DEncrypt.GetMD5(signo);
            //string metas = JsonConvert.SerializeObject(new{app_key=appKey,timestamp=timestamp});
            //string param = JsonConvert.SerializeObject(new{orderId=orderId});
            //string option = "token=" + toKen + "&nop=1.0.0&metas=" + HttpUtility.UrlEncode(metas)
            //    + "&params=" + HttpUtility.UrlEncode(param)
            //    + "&action=eleme.order.getOrder&id=" + id + "&signature=" + sign.ToUpper();

            var data = new
            {
                token = this.AccessToken,
                nop = "1.0.0",
                metas = new
                {
                    app_key = this.AppKey,
                    timestamp = timestamp
                },
                param = new
                {
                    orderId = orderId
                },
                action = "eleme.order.getOrder",
                id = id,
                signature = sign.ToUpper()
            };
            string option = JsonConvert.SerializeObject(data);
            option=option.Replace("param", "params");
            return option;
        }
    }
}
