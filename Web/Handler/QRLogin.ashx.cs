using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using System.Web.SessionState;
using System.Text;


namespace ZentCloud.JubitIMP.Web.Handler
{
    /// <summary>
    /// QRLogin 的摘要说明
    /// </summary>
    public class QRLogin : IHttpHandler, IRequiresSessionState
    {

        AshxResponse resp = new AshxResponse();
        BLLUser userBll;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
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
            StringBuilder strLog = new StringBuilder();

            try
            {
                /* 
                   * 手机访问登录端
                   * 检查有登录并授权过后才进行登录成功处理
                   * 修改相关登录凭据
                   * 跳转到用户中心页面
                   * 
                   */
                string tiketKey = context.Request["tiket"];

                if (string.IsNullOrWhiteSpace(tiketKey))
                {
                    return "登录凭据不能为空，请刷新网页登录页面重试";
                }

                strLog.AppendFormat("tiketKeyEncode:{0}<br />\n", tiketKey);

                tiketKey = this.userBll.TransmitStringDeCode(tiketKey);

                strLog.AppendFormat("tiketKeyDecode:{0}<br />\n", tiketKey);

                if (Common.DataCache.GetCache(tiketKey) == null)
                {
                    return "登录凭据不存在，请刷新网页登录页面重试";
                }

                strLog.AppendFormat("tiketKeyGetCache:{0}<br />\n", Common.DataCache.GetCache(tiketKey));

                Common.DataCache.SetCache(tiketKey, tiketKey + "-login");

                strLog.AppendFormat("tiketKey-login:{0}<br />\n", Common.DataCache.GetCache(tiketKey));

                Common.DataCache.SetCache(tiketKey + "-user", context.Session[ZentCloud.Common.SessionKey.UserID].ToString());

                strLog.AppendFormat("tiketKey-user:{0}<br />\n", Common.DataCache.GetCache(tiketKey + "-user"));

                context.Response.Redirect("/FShare/Wap/UserHub.aspx");
            }
            catch (Exception ex)
            {

                strLog.AppendFormat("Ex:{0}<br />\n", ex.Message);

            }

            return strLog.ToString();
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