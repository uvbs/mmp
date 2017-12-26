using AliOss;
using Aliyun.OpenServices.OpenStorageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.Oss
{
    /// <summary>
    /// SignatureHandler 的摘要说明
    /// </summary>
    public class SignatureHandler : IHttpHandler, IReadOnlySessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLWebsiteDomainInfo bllWebsiteDomainInfo = new BLLWebsiteDomainInfo();
        UserInfo currentUserInfo;
        string websiteOwner;

        public void ProcessRequest(HttpContext context)
        {
            #region 检查是否登录
            currentUserInfo = bllWebsiteDomainInfo.GetCurrentUserInfo();
            if (currentUserInfo == null)
            {
                resp.Msg = "您还未登录";
                context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            websiteOwner = bllWebsiteDomainInfo.WebsiteOwner;
            #endregion

            OssSign ossSign = new OssSign();
            try
            {
                ossSign = OssHelper.BuildSign(OssHelper.GetBucket(websiteOwner),OssHelper.GetBaseDir(websiteOwner),currentUserInfo.UserID, context.Request["fd"],null);
            }
            catch (Exception ex)
            {
                resp.Msg = ex.Message;
                context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            resp.IsSuccess = true;
            resp.Result = ossSign;
            context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
            return;
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