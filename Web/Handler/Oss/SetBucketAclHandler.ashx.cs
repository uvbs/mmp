using AliOss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.Oss
{
    /// <summary>
    /// SetBucketAclHandler 的摘要说明
    /// </summary>
    public class SetBucketAclHandler : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
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
                resp.IsSuccess = false;
                resp.Msg = "您还未登录";
                context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            #endregion

            try
            {
                OssHelper.SetBucketAcl(context.Request["bucketName"], context.Request["aclType"]);
            }
            catch (Exception ex)
            {
                resp.Msg = ex.Message;
                context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            //返回字符串
            resp.IsSuccess = true;
            resp.Msg = "设置Bucket的Acl完成";
            context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
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