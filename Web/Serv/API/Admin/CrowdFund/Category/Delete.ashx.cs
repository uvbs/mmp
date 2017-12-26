using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.CrowdFund.Category
{
    /// <summary>
    /// 删除众筹分类
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 响应实体
        /// </summary>
        DefaultResponse resp = new DefaultResponse();

        BLLJIMP.BLL bll = new BLLJIMP.BLL();
           
        public void ProcessRequest(HttpContext context)
        {
            string categroyIds = context.Request["cotegroy_ids"];
            if (string.IsNullOrEmpty(categroyIds))
            {
                resp.errmsg = "cotegroy_ids 必填,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            if (bll.Delete(new ArticleCategory(), string.Format(" WebsiteOwner='{0}' and AutoID in ({1}) ", bll.WebsiteOwner, categroyIds)) == categroyIds.Split(',').Length)
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