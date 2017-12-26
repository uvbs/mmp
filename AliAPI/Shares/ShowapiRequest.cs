using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using AliAPI.Shares.Constant;
using AliAPI.Shares.Util;
using System.IO;
namespace AliAPI.Shares
{
    public class ShowapiRequest
    {


        private String appKey = string.Empty;
        private String appSecret = string.Empty;
        private String url = string.Empty;
        private String host = string.Empty;
        private String path = string.Empty;



        //int connectTimeout = 10000;//3秒
        int readTimeout = 15000;//15秒
        //String char_set = "utf-8";
        Dictionary<String, String> ret_headers = new Dictionary<String, String>();
        Dictionary<String, String> querys = new Dictionary<string, string>();

        public ShowapiRequest(String appKey, String appSecret, String url)
        {
            this.appKey = appKey;
            this.appSecret = appSecret;
            this.url = url;
            int ind = url.LastIndexOf("/");
            this.host = url.Substring(0, ind);
            this.path = url.Substring(ind);
        }
        public void setRet_headers(Dictionary<String, String> ret_headers)
        {
            this.ret_headers = ret_headers;
        }

        /**
         * 添加post体的字符串参数
         */
        public ShowapiRequest addTextPara(String key, String value)
        {
            this.querys.Add(key, value);
            return this;
        }

        public String doGet()
        {
            Dictionary<String, String> headers = new Dictionary<string, string>();
            List<String> signHeader = new List<String>();
            //设定Content-Type，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_TEXT);
            //设定Accept，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_TEXT);
            //指定参与签名的header            
            signHeader.Add(SystemHeader.X_CA_TIMESTAMP);


            String ret = "";
            using (HttpWebResponse response = HttpUtil.HttpGet(this.host, this.path, this.appKey, this.appSecret, this.readTimeout, headers, this.querys, signHeader))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                ret = reader.ReadToEnd() + Constants.LF;
            }
            return ret;
        }

        public String doPost()
        {
            Dictionary<String, String> headers = new Dictionary<string, string>();
            List<String> signHeader = new List<String>();
            //设定Content-Type，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_FORM);
            //设定Accept，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);
            //指定参与签名的header            
            signHeader.Add(SystemHeader.X_CA_TIMESTAMP);

            String ret = "";
            using (HttpWebResponse response = HttpUtil.HttpPost(this.host, this.path, this.appKey, this.appSecret, this.readTimeout, headers, null, this.querys, signHeader))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                ret = reader.ReadToEnd() + Constants.LF;

            }
            return ret;
        }

    }
}
