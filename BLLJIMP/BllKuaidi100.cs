using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 快递100
    /// </summary>
    public class BllKuaidi100 : BLL
    {
        public BllKuaidi100()
            : base()
        {

        }
        /// <summary>
        /// 快递100 key
        /// </summary>
        private string key = "JsLBKSIs303";
        /// <summary>
        /// 公司编号
        /// </summary>
        private string customer = "177D6A4E1834F8AD2F55F2F2F3B7A391";
        /// <summary>
        /// 订阅请求
        /// </summary>
        /// <returns></returns>
        public bool Poll(string expressCompanyCode,string expressNumber,out string msg) {

            msg = "";
            string data="";
            string url = "http://www.kuaidi100.com/poll";
            ZentCloud.Common.HttpInterFace request = new Common.HttpInterFace();
            var parameters = new
            {
                callbackurl = string.Format("http://{0}/kuaidi100/callback.ashx", HttpContext.Current.Request.Url.Host),

                salt=key,
                resultv2=""
            };
            var body = new
            {
                company = expressCompanyCode,
                from="",
                to="",
                number=expressNumber,
                key=key,
                parameters = parameters

            };
            data = ZentCloud.Common.JSONHelper.ObjectToJson(body);
            var resultStr = request.PostWebRequest(string.Format("param={0}", data), url, Encoding.UTF8);

            ResultModel result = ZentCloud.Common.JSONHelper.JsonToModel<ResultModel>(resultStr);
            msg = result.message;
            if (result.result.ToLower()=="true"&&result.returnCode=="200")
            {
                return true;
            }
            return false;
        
        }

        /// <summary>
        /// 实时查询 返回json
        /// </summary>
        /// <param name="expressCompanyCode">快递公司代码</param>
        /// <param name="expressNumber">快递单号</param>
        /// <returns></returns>
        public string Query(string expressCompanyCode, string expressNumber)
        {
            string url = "http://poll.kuaidi100.com/poll/query.do";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            var paramObj = new { 
            com=expressCompanyCode,
            num=expressNumber,
            from="",
            to=""
            };
            String param = ZentCloud.Common.JSONHelper.ObjectToJson(paramObj);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] inBytes = Encoding.GetEncoding("utf-8").GetBytes(param + key + customer);
            byte[] outBytes = md5.ComputeHash(inBytes);
            string outString = "";
            for (int i = 0; i < outBytes.Length; i++)
            {
                outString += outBytes[i].ToString("x2");
            }
            String sign = outString.ToUpper();
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("param", param);
            parameters.Add("customer", customer);
            parameters.Add("sign", sign);
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值  
            Stream stream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
            JToken jToken = JToken.Parse(sr.ReadToEnd());
            if (jToken["message"].ToString()=="ok")//有物流信息
            {
                return jToken["data"].ToString();
            }
            else
            {
                return "[]";
            }
        
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, Encoding charset)
        {
            HttpWebRequest request = null;
            //HTTPSQ请求  
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            request = WebRequest.Create(url) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            //如果需要POST数据     
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = charset.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 获取快递查询结果
        /// </summary>
        /// <param name="expressNumber">快递单号</param>
        /// <param name="expressCompanyCode">快递公司代码</param>
        /// <returns></returns>
        public ExpressResult GetExpressResult(string expressNumber, string expressCompanyCode)
        {
            return Get<ExpressResult>(string.Format("ExpressNumber='{0}' And ExpressCompanyCode='{1}'",expressNumber,expressCompanyCode));
        
        } 
        /// <summary>
        /// 结果模型
        /// </summary>
        public class ResultModel {
            /// <summary>
            /// 结果 true false
            /// </summary>
            public string result { get; set; }
            /// <summary>
            /// 返回码
            /// </summary>
            public string returnCode { get; set; }
            /// <summary>
            /// 提示信息
            /// </summary>
            public string message { get; set; }
        
        }


        /// <summary>
        /// 快递公司代码映射 efast
        /// </summary>
       public  Dictionary<string, string> expressCompanyMap = new Dictionary<string, string>()
        {
           {"zto","zhongtong"},
           {"sf","shunfeng"},
           {"sto","shentong"},
           {"yto","yuantong"},
           {"zjs","zhaijisong"},
           {"htky","huitongkuaidi"},
           {"ttkdex","tiantian"},
           {"kj","kuaijiesudi"},
           {"db","debangwuliu"},
           {"ys","youshuwuliu"},
           {"lb","longbanwuliu"},
           {"gto","guotongkuaidi"}
           
        };




    }
}
