using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ZentCloud.Common;
using Newtonsoft.Json;
using NeteaseIMSDK.Model;

namespace NeteaseIMSDK
{
    public class NeteaseIMSDK
    {
        private string AppKey = string.Empty;
        private string AppSecret = string.Empty;
        public NeteaseIMSDK(string appKey, string appSecret)
        {
            AppKey = appKey;
            AppSecret = appSecret;
        }

        /// <summary>
        /// Post提交数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private string PostWebRequest(string data, string url)
        {
            string result = string.Empty;
            string postData = data;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData); // 转化

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8;";
            webRequest.ContentLength = byteArray.Length;

            var checkSum = Model.CheckSum.GetCheckSum(AppKey, AppSecret);
            webRequest.Headers.Add("AppKey", checkSum.AppKey);
            webRequest.Headers.Add("Nonce", checkSum.Nonce);
            webRequest.Headers.Add("CurTime", checkSum.CurTime);
            webRequest.Headers.Add("CheckSum", checkSum.CheckSumResult);

            Stream newStream = webRequest.GetRequestStream();
            // Send the data.
            newStream.Write(byteArray, 0, byteArray.Length);    //写入参数
            newStream.Close();

            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                result = sr.ReadToEnd();
            }

            return result;

        }
        //刷新创建账号
        public Model.CreateUserResp CreateUser(string accid, string name, string props, string icon)
        {
            Model.CreateUserResp result = new Model.CreateUserResp();

            string url = "https://api.netease.im/nimserver/user/create.action",
                reqData = string.Format("accid={0}&name={1}&props={2}&icon={3}", accid, name, props, icon);

            var resp = PostWebRequest(reqData, url);

            if (!string.IsNullOrWhiteSpace(resp))
            {
                result = JsonConvert.DeserializeObject<Model.CreateUserResp>(resp);
            }

            return result;
        }
        //刷新token
        public Model.CreateUserResp RefreshToken(string accid)
        {
            Model.CreateUserResp result = new Model.CreateUserResp();

            string url = "https://api.netease.im/nimserver/user/refreshToken.action",
                reqData = string.Format("accid={0}", accid);

            var resp = PostWebRequest(reqData, url);

            if (!string.IsNullOrWhiteSpace(resp))
            {
                result = JsonConvert.DeserializeObject<Model.CreateUserResp>(resp);
            }
            return result;
        }
    }
}
