using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using System.Text;

namespace ZentCloud.JubitIMP.Web.Handler
{
    /// <summary>
    /// QRLogin 的摘要说明
    /// </summary>
    public class QLoginV1 : IHttpHandler, IRequiresSessionState
    {

        AshxResponse resp = new AshxResponse();
        BLLUser userBll;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            context.Response.Expires = 0;
            string result = "false";
            try
            {

                this.userBll = new BLLUser("");

                result = ToQRcodeLogin(context);

            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }

            context.Response.ClearContent();
            context.Response.Write(result);
        }


        private string ToQRcodeLogin(HttpContext context)
        {
            //StringBuilder strLog = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<meta content=\"width=device-width,initial-scale=1,user-scalable=no\" name=\"viewport\" />");
            sb.Append("</head>");
            sb.Append("<body>");
            //try
            //{
                /* 
                   * 手机访问登录端
                   * 检查有登录并授权过后才进行登录成功处理
                   * 修改相关登录凭据
                   * 
                   * 
                   */
                string tiketKey = context.Request["tiket"];

                if (string.IsNullOrWhiteSpace(tiketKey))
                {
                    sb.Append("<h1>登录凭据不能为空，请刷新网页登录页面重试</h1>");
                    sb.Append("</body>");
                    sb.Append("</html>");
                    return sb.ToString();
                }

                //strLog.AppendFormat("tiketKeyEncode:{0}<br />\n", tiketKey);
                tiketKey = this.userBll.TransmitStringDeCode(tiketKey);
               // strLog.AppendFormat("tiketKeyDecode:{0}<br />\n", tiketKey);
                if (Common.DataCache.GetCache(tiketKey) == null)
                {

                        sb.Append("<h1>登录凭据不存在，请刷新网页登录页面重试</h1>");
                        sb.Append("</body>");
                        sb.Append("</html>");
                        return sb.ToString();

                    
                }
               // strLog.AppendFormat("tiketKeyGetCache:{0}<br />\n", Common.DataCache.GetCache(tiketKey));

                Common.DataCache.SetCache(tiketKey, tiketKey + "-login", DateTime.MaxValue, new TimeSpan(0, 0, 7200));

               // strLog.AppendFormat("tiketKey-login:{0}<br />\n", Common.DataCache.GetCache(tiketKey));

                Common.DataCache.SetCache(tiketKey + "-user", context.Session[ZentCloud.Common.SessionKey.UserID].ToString(), DateTime.MaxValue, new TimeSpan(0, 0, 7200));


                //strLog.AppendFormat("tiketKey-user:{0}<br />\n", Common.DataCache.GetCache(tiketKey + "-user"));

                 //context.Response.Redirect("/App/Cation/Wap/UserHub.aspx");
                sb.Append("<h1 style='text-align:center;color:#40bd5b;padding-top:50px;'>登录成功!</h1><p style='font-size:16px;color:#999;text-align:center;'>请在网页上执行操作<p>");

                sb.Append("</body>");
                sb.Append("</html>");
                return sb.ToString();

            //}
            //catch (Exception ex)
            //{

            //    strLog.AppendFormat("Ex:{0}<br />\n", ex.Message);

            //}

            //return strLog.ToString();
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