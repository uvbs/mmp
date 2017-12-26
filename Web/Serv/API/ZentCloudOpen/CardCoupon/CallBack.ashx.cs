using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Xml;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.CardCoupon
{
    /// <summary>
    /// 卡券回传
    /// </summary>
    public class CallBack : BaseHanderOpen
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();

        public void ProcessRequest(HttpContext context)
        {
            try
            {


                string data = context.Request["data"];
                if (string.IsNullOrEmpty(data))
                {
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "data 参数必传";
                    bllMall.ContextResponse(context, resp);
                    return;
                }
                Tolog(data);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("D:\\result.xml");
                xmlDoc.LoadXml(data);
                 
                string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(xmlDoc);
                
                resp.status = true;
                resp.msg = "ok";

            }
            catch (Exception ex)
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = ex.Message;
                resp.result = ex.ToString();
                Tolog("制券通知异常"+ex.ToString());
            }
            bllMall.ContextResponse(context, resp);

        }
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="msg"></param>
        private void Tolog(string msg)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"D:\log.txt", true, Encoding.GetEncoding("GB2312")))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "  " + msg);
                }
            }
            catch { }
        }


    }
}