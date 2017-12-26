using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP;
using System.Xml;
using ZentCloud.BLLJIMP.Model;
using System.IO;
using System.Web.SessionState;

namespace ZentCloud.BLLModule
{
    /// <summary>
    /// 微信开放平台消息被动回复
    /// </summary>
    public class WXOpenReplyMsgHandler : IHttpHandler, IReadOnlySessionState
    {


        /// <summary>
        /// 微信开放平台BLL
        /// </summary>
        BLLWeixinOpen bllWeixinOpen = new BLLWeixinOpen();
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLWeixin bllWeixin = new BLLWeixin();
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string filePath = context.Request.FilePath;
                if (filePath.EndsWith("EventMsgNotify.wxopenmsg"))//拦截处理消息
                {
                    context.Response.ContentType = "text/plain";
                    string signature = context.Request["msg_signature"];//签名串
                    string timeStamp = context.Request["timestamp"];//时间戳
                    string nonce = context.Request["nonce"];//随机串
                    string encryptType = context.Request["encrypt_type"];
                    string xml = ""; //解密后的xml字符串
                    if (bllWeixinOpen.DecryptMsg(signature, timeStamp, nonce, RequestXML(context).InnerXml, out xml))
                    {
                        string appId = filePath.Split('/')[2];

                        #region 微信的测试机器人
                        if (appId == "wx570bc396a51b8ff8")
                        {

                            var dic = bllWeixinOpen.XmlToDictionary(xml);
                            switch (dic["MsgType"].ToString().ToLower())
                            {

                                #region 事件消息
                                case "event":
                                    var acctoken = GetToken();
                                    bllWeixin.SendKeFuMessageText(GetToken(), dic["FromUserName"], dic["Event"].ToString() + "from_callback");

                                    break;
                                #endregion

                                #region 文本消息
                                case "text":
                                    if (dic["Content"].ToString().StartsWith("QUERY_AUTH_CODE"))
                                    {

                                        var code = dic["Content"].Replace("QUERY_AUTH_CODE:", "");
                                        //客服消息
                                        var authInfo = bllWeixinOpen.GetQueryAuth(code);
                                        var accessToken = authInfo.authorization_info.authorizer_access_token;
                                        bllWeixin.SendKeFuMessageText(accessToken, dic["FromUserName"], code + "_from_api");
                                        SaveToken(accessToken);


                                    }
                                    else
                                    {
                                        bllWeixin.SendKeFuMessageText(GetToken(), dic["FromUserName"], "TESTCOMPONENT_MSG_TYPE_TEXT_callback");

                                    }
                                    break;
                                #endregion
                                default:
                                    break;
                            }
                            context.Response.Write(string.Empty);
                            //context.Response.Flush();
                            //context.Response.Close();
                            return;

                        }
                        #endregion

                        #region 一般公众号
                        WebsiteInfo websiteInfo = bllWeixin.Get<WebsiteInfo>(string.Format(" AuthorizerAppId='{0}'", appId));
                        if (websiteInfo == null)
                        {
                            return;
                        }

                        bllWeixin.SetWebSiteOwner(websiteInfo.WebsiteOwner);

                        string result = bllWeixin.ActionResultOpen(xml, websiteInfo.WebsiteOwner);

                        if (result.Contains("<Content><![CDATA[]]></Content>") && result.ToLower().Contains("<msgtype>text</msgtype>"))
                        {
                            result = result.Replace("<Content><![CDATA[]]></Content>", "").Replace("<MsgType>Text</MsgType>", "<MsgType><![CDATA[transfer_customer_service]]></MsgType>");
                            //context.Response.Write(result);
                            //return;
                        }


                        string resp = bllWeixinOpen.EncryptMsg(result, timeStamp, nonce);
                        context.Response.Write(resp);
                        return;
                        #endregion

                    }
                    else
                    {
                        context.Response.Write("fail");
                        return;
                    }


                }
                return;


            }
            catch (Exception ex)
            {
                context.Response.Write(ex.ToString());
                return;
            }





        }

        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// 获取收到的加密XML
        /// </summary>
        /// <param name="context"></param>
        private XmlDocument RequestXML(HttpContext context)
        {
           
            XmlDocument xmlDocEn = new XmlDocument();
            xmlDocEn.Load(context.Request.InputStream);//加密的xml
            ToLog(xmlDocEn.ToString());
            return xmlDocEn;

        }
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="msg"></param>
        private void ToLog(string msg)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("D:\\WeixinOpen\\msglog.txt", true, Encoding.GetEncoding("gb2312")))
                {
                    sw.Write(string.Format("{0}", msg));
                }
            }
            catch { }
        }
        /// <summary>
        /// 保存token
        /// </summary>
        /// <param name="token"></param>
        private void SaveToken(string token)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("D:\\WeixinOpen\\Token.txt", false, Encoding.GetEncoding("gb2312")))
                {
                    sw.Write(token);
                }
            }
            catch { }
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        private string GetToken()
        {

            StreamReader sr = new StreamReader("D:\\WeixinOpen\\Token.txt", Encoding.Default);
            return sr.ReadToEnd();

        }
    }
}
