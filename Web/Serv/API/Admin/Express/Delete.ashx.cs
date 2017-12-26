using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.ModelGen;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Express
{
    /// <summary>
    /// 删除快递公司
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        { 
            string expressIds = context.Request["express_company_ids"];
            resp.errcode = 1;
            resp.errmsg = "暂不支持删除";
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            return;
            if (string.IsNullOrEmpty(expressIds))
            {
                resp.errcode = 1;
                resp.errmsg = "express_company_ids 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            if (bll.Delete(new ExpressInfo(), string.Format(" WebsiteOwner='{0}' and AutoID in ({1}) ", bll.WebsiteOwner, expressIds)) == expressIds.Split(',').Length)
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = -1;
                resp.errmsg = "删除出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            return;
        }
    }
}