using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Level
{
    /// <summary>
    /// Delete 的摘要说明
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string userlevelIds = context.Request["level_ids"];
            if (string.IsNullOrEmpty(userlevelIds))
            {
                resp.errmsg = "level_ids为必填项,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bll.Delete(new UserLevelConfig(), string.Format(" WebsiteOwner='{0}' AND AutoID in ({1}) ", bll.WebsiteOwner, userlevelIds)) == userlevelIds.Split(',').Length)
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errmsg = "删除出错";
                resp.errcode = -1;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }


    }
}