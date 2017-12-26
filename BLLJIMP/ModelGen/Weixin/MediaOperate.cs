using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    /// <summary>
    /// 微信多媒体操作文件类
    /// </summary>
    public static class MediaOperate
    {

        /// <summary>
        /// 向微信接口提交上传文件数据
        /// </summary>
        /// <param name="list">表单参数集合</param>
        /// <param name="uri">提交地址</param>
        /// <returns>响应字符串</returns>
        public static string PostFormData(List<FormItem> list, string uri)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            //请求 
            WebRequest req = WebRequest.Create(uri);
            req.Method = "POST";
            req.ContentType = "multipart/form-data; boundary=" + boundary;
            //组织表单数据 
            StringBuilder sb = new StringBuilder();
            foreach (FormItem item in list)
            {
                switch (item.ParamType)
                {
                    case ParamType.Text:
                        sb.Append("--" + boundary);
                        sb.Append("\r\n");
                        sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"",item.Name);
                      
                        sb.Append("\r\n\r\n");
                        sb.Append(item.Value);
                        sb.Append("\r\n");
                        break;
                    case ParamType.File:
                        sb.Append("--" + boundary);
                        sb.Append("\r\n");
                    
                        sb.AppendFormat("Content-Disposition: form-data; name=\"media\"; filename=\"{0}\"",item.Value);
                        sb.Append("\r\n");
                        sb.Append("Content-Type: application/octet-stream");
                        sb.Append("\r\n\r\n");
                        break;
                }
            }
            string head = sb.ToString();
            //post字节总长度
            long length = 0;
            byte[] form_data = Encoding.UTF8.GetBytes(head);
            //结尾 
            byte[] foot_data = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            List<FormItem> fileList = list.Where(f => f.ParamType == ParamType.File).ToList();
            length = form_data.Length + foot_data.Length;
            foreach (FormItem fi in fileList)
            {
                FileStream fileStream = new FileStream(fi.Value, FileMode.Open, FileAccess.Read);
                length += fileStream.Length;
                fileStream.Close();
            }
            req.ContentLength = length;           

            Stream requestStream = req.GetRequestStream();
            //发送表单参数 
            requestStream.Write(form_data, 0, form_data.Length);
            foreach (FormItem fd in fileList)
            {
                FileStream fileStream = new FileStream(fd.Value, FileMode.Open, FileAccess.Read);
                //文件内容 
                byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    requestStream.Write(buffer, 0, bytesRead);
                //结尾 
                requestStream.Write(foot_data, 0, foot_data.Length);
            }
            requestStream.Close();

            //响应 
            WebResponse pos = req.GetResponse();
            StreamReader sr = new StreamReader(pos.GetResponseStream(), Encoding.UTF8);
            string html = sr.ReadToEnd().Trim();
            sr.Close();
            if (pos != null)
            {
                pos.Close();
                pos = null;
            }
            if (req != null)
            {
                req = null;
            }
            return html;
        }


        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="ACCESS_TOKEN"></param>
        /// <param name="MEDIA_ID"></param>
        /// <returns></returns>
        public static string DownloadImage(string ACCESS_TOKEN, string MEDIA_ID)
        {
            string file = string.Empty;
            string content = string.Empty;
            string strpath = string.Empty;
            string savepath = string.Empty;
            string stUrl = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + ACCESS_TOKEN + "&media_id=" + MEDIA_ID;
            string relatePath;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(stUrl);

            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {

                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                strpath = myResponse.ResponseUri.ToString();
                WebClient mywebclient = new WebClient();
                relatePath= string.Format("/FileUpload/Weixin/FileReceive/{0}.jpg", DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next().ToString().Substring(0, 4));
                savepath = System.Web.HttpContext.Current.Server.MapPath(relatePath);
                try
                {
                    mywebclient.DownloadFile(strpath, savepath);
                    file = savepath;
                }
                catch (Exception ex)
                {
                    savepath = ex.ToString();

                }

            }
            return relatePath;
        }

        /// <summary>
        /// 参数类型
        /// </summary>
        public enum ParamType
        {
            ///
            /// 文本类型
            ///
            Text,
            ///
            /// 文件路径，需要全路径（例：C:\A.JPG)
            ///
            File
        }
    
    }

    /// <summary>
    /// 表单参数项
    /// </summary>
    public class FormItem
    {

        public string Name { get; set; }
        public ZentCloud.BLLJIMP.Model.Weixin.MediaOperate.ParamType ParamType { get; set; }
        public string Value { get; set; }


    }



}
