using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Article
{
    /// <summary>
    /// SetAccessLevel 的摘要说明
    /// </summary>
    public class SetAccessLevel : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL  业务逻辑层
        /// </summary>
        BLLJIMP.BLLJuActivity blljuActivity = new BLLJIMP.BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            string activityIds = context.Request["activity_ids"];
            string accessLevel = context.Request["access_level"];
            if (string.IsNullOrEmpty(activityIds))
            {
                resp.errmsg = "activity_ids 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(accessLevel))
            {
                resp.errmsg = "access_level 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (blljuActivity.Update(new JuActivityInfo(), string.Format(" AccessLevel={0}", int.Parse(accessLevel)), string.Format(" WebsiteOwner='{0}' AND ArticleType='article' AND JuActivityID in ({1})", blljuActivity.WebsiteOwner, activityIds)) == activityIds.Split(',').Length)
            {
                resp.isSuccess = true;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "批量设置访问等级出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}