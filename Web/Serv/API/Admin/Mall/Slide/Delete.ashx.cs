using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Slide
{
    /// <summary>
    /// 删除
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string slideIds = context.Request["slide_ids"];
            if (string.IsNullOrEmpty(slideIds))
            {
                resp.errcode = 1;
                resp.errmsg = "slide_ids 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bll.Delete(new ZentCloud.BLLJIMP.Model.Slide(), string.Format(" WebsiteOwner='{0}' AND AutoID in ({1}) ", bll.WebsiteOwner, slideIds)) == slideIds.Split(',').Length)
            {
                resp.errmsg = "ok";
                resp.errcode = 0;
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "删除广告出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
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