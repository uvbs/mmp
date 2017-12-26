using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Payment.JDPay
{
    public class HttpUtil
    {
        public static string Post(string Url, String dataStr)
        {
            try
            {
               
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.Proxy = null;
                request.Timeout = 10000;
                if (Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
                }
                request.ContentType = "application/xml";
                //如果需要POST数据 
                byte[] data = Encoding.UTF8.GetBytes(dataStr);
                request.ContentLength = data.Length;
                using (Stream myRequestStream = request.GetRequestStream())
                {
                    myRequestStream.Write(data, 0, data.Length);
                    myRequestStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Diagnostics.Debug.WriteLine("响应状态:" + response.StatusCode);
                using (Stream myResponseStream = response.GetResponseStream())
                {
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                    string retString = myStreamReader.ReadToEnd();
                    myStreamReader.Close();
                    myResponseStream.Close();
                    return retString;
                }
            }
            catch (Exception e)
            {
               
                throw e;
            }

        }
        //该方法用于验证服务器证书是否合法，当然可以直接返回true来表示验证永远通过。服务器证书具体内容在参数certificate中。可根据个人需求验证
        //该方法在request.GetResponse()时触发
        private static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            /*
            //获取本地存储的服务器安全证书
            X509Certificate cer = X509Certificate.CreateFromCertFile(RequestUtil.getFilePath() + "jdpay.cer");
            //比较服务器证书的合法
            if (cer.GetSerialNumberString().Equals(certificate.GetSerialNumberString()))
            {
                return true;
            }   
            return false;
            */
            return true;
        }
    }
}
