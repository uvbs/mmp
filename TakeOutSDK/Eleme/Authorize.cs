using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using Newtonsoft.Json;
namespace TakeOutSDK.Eleme
{
    //授权
    public class Authorize
    {
        /// <summary>
        /// 获取Token地址(测试)
        /// </summary>
        private const string sb_toKenUrl = "https://open-api-sandbox.shop.ele.me/token";
        /// <summary>
        /// 获取Token地址(正式)
        /// </summary>
        private const string toKenUrl = "https://open-api.shop.ele.me/token";

        private string key;
        private string secret;
        public Authorize(string _key, string _secret)
        {
            this.key = _key;
            this.secret = _secret;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public Model.AuthorizeResponse GetToken(string url)
        {
            Model.AuthorizeResponse result = new Model.AuthorizeResponse();
            var resp = AuthorizePostRequest(url);
            if (!string.IsNullOrEmpty(resp))
            {
                result = JsonConvert.DeserializeObject<Model.AuthorizeResponse>(resp);
            }
            return result;
        }
        /// <summary>
        /// Post提交数据
        /// <returns>result</returns>
        public string AuthorizePostRequest(string url)
        {
            string result = string.Empty;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ProtocolVersion = HttpVersion.Version10;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded;";
            webRequest.Timeout = 10*1000;
            string keySign = Convert.ToBase64String(Encoding.Default.GetBytes(this.key + ":" + this.secret));
            webRequest.Headers.Add("Authorization","Basic "+keySign);
            string body = "grant_type=client_credentials";
            byte[] data = Encoding.Default.GetBytes(body);
            try
            {
                using (Stream stream = webRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result = sr.ReadToEnd();
                    }
                }
            }
            catch(WebException ex){
                throw ex;
            }
            webRequest.Abort();
            return result;
        } 

    }
}
