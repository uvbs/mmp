using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Navigation
{
    /// <summary>
    /// 导航删除接口
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public  void ProcessRequest(HttpContext context)
        {
            string navigationIds = context.Request["navigation_ids"];
            if (string.IsNullOrEmpty(navigationIds))
            {
                resp.errcode = 1;
                resp.errmsg = "navigation_id 为空,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bll.Delete(new ZentCloud.BLLJIMP.Model.Navigation(), string.Format(" WebsiteOwner='{0}' AND AutoID in ({1})", bll.WebsiteOwner, navigationIds)) == navigationIds.Split(',').Length)
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = -1;
                resp.errmsg = "删除导航出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}