using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Activity
{
    /// <summary>
    /// Delete 的摘要说明   批量删除文章
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLLActivity bllActivity = new BLLJIMP.BLLActivity("");
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string activityIds = context.Request["activity_ids"];
            if (string.IsNullOrEmpty(activityIds))
            {
                resp.errcode = -1;
                resp.errmsg = "activity_ids 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bllActivity.Update(new JuActivityInfo(), string.Format(" isDelete=1"), string.Format(" JuActivityID in ({0})  And WebsiteOwner='{1}' ", activityIds, bllActivity.WebsiteOwner)) == activityIds.Split(',').Length)
            {
                resp.errmsg = "ok";
                resp.errcode = 0;
                resp.isSuccess = true;
            }
            else
            {
                
                resp.errcode = 1;
                resp.errmsg = "删除活动出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}