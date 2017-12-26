using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Activity
{
    /// <summary>
    /// SetCategory 的摘要说明
    /// </summary>
    public class SetCategory : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL  业务逻辑层
        /// </summary>
        BLLJIMP.BLLJuActivity blljuActivity = new BLLJIMP.BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            string activityIds = context.Request["activity_ids"];
            string categoryId = context.Request["category_id"];
            if (string.IsNullOrEmpty(activityIds))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "activity_ids 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(categoryId))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "category_id 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (blljuActivity.Update(new JuActivityInfo(), string.Format(" CategoryId={0}", int.Parse(categoryId)), string.Format(" WebsiteOwner='{0}' AND ArticleType='activity' AND JuActivityID in ({1})", blljuActivity.WebsiteOwner, activityIds)) == activityIds.Split(',').Length)
            {
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "批量设置分类出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
} 