using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Slide
{
    /// <summary>
    /// 类型列表
    /// </summary>
    public class ListType : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebSiteOwner='{0}'", bll.WebsiteOwner));
            var slideData = bll.GetColList<BLLJIMP.Model.Slide>(int.MaxValue, 1, sbWhere.ToString(), "AutoID,Type");
            var result = slideData.Select(p => p.Type).Distinct().OrderBy(p => p);
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.result = result;
            bll.ContextResponse(context, apiResp);
        }
    }
}