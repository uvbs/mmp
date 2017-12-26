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
    /// 公众号授权成功回调
    /// </summary>
    public class OAuthCallBack : IHttpHandler, IReadOnlySessionState
    {

        /// <summary>
        /// 微信开放平台BLL
        /// </summary>
        BLLWeixinOpen bll = new BLLWeixinOpen();
        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";
            try
            {
                #region 公众号授权成功处理
                if (!string.IsNullOrEmpty(context.Request["auth_code"]))//授权成功
                {
                    var isSuccess = bll.AuthSuccess(context.Request["auth_code"]);
                    if (isSuccess)
                    {
                        context.Response.Redirect("OAuthSuccess.html");

                    }
                    else
                    {
                        context.Response.Write("授权失败");
                    }


                }
                #endregion
            }
            catch (Exception ex)
            {
                
               context.Response.Write(ex.ToString());
            }




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