using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.MemberTag
{
    /// <summary>
    /// 会员标签列表接口
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLTag bllTag = new BLLTag();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string tagName = context.Request["name"], tagType = context.Request["type"];
            int totalCount = 0;
            List<BLLJIMP.Model.MemberTag> data = bllTag.GetTags(bllTag.WebsiteOwner, tagName, pageIndex, pageSize, out totalCount, tagType);


            resp.isSuccess = true;
            resp.returnObj = new
            {
                totalcount = totalCount,
                list = (from p in data
                        select new { 
                           id = p.AutoId,
                           name = p.TagName,
                           type = p.TagType,
                           level = p.AccessLevel
                        })
            };
            bllTag.ContextResponse(context, resp);
        }
    }
}