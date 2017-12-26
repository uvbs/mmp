using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.Shop
{
    public partial class Index : System.Web.UI.Page
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 当前站点信息
        /// </summary>
        public ZentCloud.BLLJIMP.Model.WebsiteInfo webSiteInfo = new BLLJIMP.Model.WebsiteInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            webSiteInfo = bllMall.GetWebsiteInfoModelFromDataBase();



            //try
            //{
            //    string postData = "";
            //    string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            //    System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            //    byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(postData);
            //    req.Method = "POST";
            //    req.ContentType = "application/x-www-form-urlencoded";
            //    req.ContentLength = requestBytes.Length;
            //    System.IO.Stream requestStream = req.GetRequestStream();
            //    requestStream.Write(requestBytes, 0, requestBytes.Length);
            //    requestStream.Close();
            //    System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
            //    System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
            //    string backStr = sr.ReadToEnd();
            //    sr.Close();
            //    res.Close();
            //}
            //catch { }

        }
    }
}