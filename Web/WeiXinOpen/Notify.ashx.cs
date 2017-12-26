using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Xml;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using System.IO;
using System.Text;

namespace ZentCloud.JubitIMP.Web.WeiXinOpen
{
    /// <summary>
    /// 授权事件接收URL：用于接收取消授权通知、授权成功通知、授权更新通知，也用于接收ticket
    /// </summary>
    public class Notify : IHttpHandler, IReadOnlySessionState
    {
        /// <summary>
        /// 微信开放平台BLL
        /// </summary>
        BLLWeixinOpen bll = new BLLWeixinOpen();
        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";
            string signature = context.Request["msg_signature"];//签名串
            string timeStamp = context.Request["timestamp"];//时间戳
            string nonce = context.Request["nonce"];//随机串
            string xml = ""; //解密后的xml字符串
            if (bll.DecryptMsg(signature, timeStamp, nonce, RequestXML(context).InnerXml, out xml))
            {
                Dictionary<string, string> parameters = bll.XmlToDictionary(xml);
                switch (parameters["InfoType"])//信息类型
                {
                    case "component_verify_ticket"://组件tiket
                        SystemSet systemSet = bll.GetSysSet();
                        systemSet.ComponentVerifyTicket = parameters["ComponentVerifyTicket"];
                        if (bll.Update(systemSet))
                        {
                            ToLog("接收component_verify_ticket成功");
                           
                        }
                        break;
                    case "authorized"://授权成功通知
                        break;
                    case "unauthorized"://取消授权通知
                        var websiteInfoList = bll.GetList<WebsiteInfo>(string.Format(" AuthorizerAppId='{0}'", parameters["AuthorizerAppid"]));
                        if (websiteInfoList!=null)
                        {
                            foreach (var website in websiteInfoList)
                            {
                                website.AuthorizerAppId = "";
                                website.AuthorizerAccessToken = "";
                                website.AuthorizerRefreshToken = "";
                                website.AuthorizerNickName = "";
                                website.AuthorizerServiceType = "";
                                website.AuthorizerVerifyType = "";
                                website.AuthorizerUserName = "";
                                bll.Update(website);
                            }

                        }

                        break;
                    case "updateauthorized"://授权更新通知
                        break;
                    default:
                        break;
                }

            }
            else
            {
                context.Response.Write("fail");
                return;
            }
            context.Response.Write("success");

        }
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="msg"></param>
        private void ToLog(string msg)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("D:\\WeixinOpen\\log.txt", true, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), msg));
                }
            }
            catch { }
        }
        /// <summary>
        /// 获取收到的加密XML
        /// </summary>
        /// <param name="context"></param>
        private XmlDocument RequestXML(HttpContext context)
        {

            XmlDocument xmlDocEn = new XmlDocument();
            xmlDocEn.Load(context.Request.InputStream);//加密的xml
            return xmlDocEn;

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