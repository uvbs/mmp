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

namespace ZentCloud.JubitIMP.Web.WeiXinOpen.wxe6803c8ef209b51d
{
    /// <summary>
    /// 至云微游戏
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
            string encryptType = context.Request["encrypt_type"];
            string xml = ""; //解密后的xml字符串
            try
            {
                if (bllWeixinOpen.DecryptMsg(signature, timeStamp, nonce, RequestXML(context).InnerXml, out xml))
                {


                    string result = bllWeixin.ActionResultOpen(xml, "hf");
                   // ToLog(result);
                    string resp = bllWeixinOpen.EncryptMsg(result, timeStamp, nonce);
                    ////ToLog(resp);
                    context.Response.Write(resp);
                    context.Response.Flush();
                    context.Response.Close();

                }
                else
                {
                    ToLog("签名失败");
                    context.Response.Write("fail");
                    return;
                }
            }
            catch (Exception ex)
            {

                ToLog(ex.ToString());
            }



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
