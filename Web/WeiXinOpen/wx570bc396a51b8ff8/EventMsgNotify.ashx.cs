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

namespace ZentCloud.JubitIMP.Web.WeiXinOpen.wx570bc396a51b8ff8
{
    /// <summary>
    /// 配合微信自动化测试机器人专门写的 微信审核通过后这里的代码就用不到
    /// </summary>
    public class EventMsgNotify : IHttpHandler, IReadOnlySessionState
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

            context.Response.ContentType = "text/plain";
            string signature = context.Request["msg_signature"];//签名串
            string timeStamp = context.Request["timestamp"];//时间戳
            string nonce = context.Request["nonce"];//随机串
            string encryptType = context.Request["encrypt_type"];//加密类型
            string resp = string.Empty;//响应内容
            string xml = ""; //解密后的xml字符串
            if (bllWeixinOpen.DecryptMsg(signature, timeStamp, nonce, RequestXML(context).InnerXml, out xml))
            {
                //ToLog(xml);
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
                context.Response.Clear();
                context.Response.Write(resp);
                context.Response.End();


            }
            else
            {
                ToLog("signfail");
                context.Response.Write("fail");

            }


        }
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="msg"></param>
        private void ToLog(string msg)
        {
            //try
            //{
            //    using (StreamWriter sw = new StreamWriter("D:\\WeixinOpen\\msglog.txt", true, Encoding.GetEncoding("gb2312")))
            //    {
            //        sw.Write(string.Format("\r\n{0}", msg));
            //    }
            //}
            //catch { }
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
