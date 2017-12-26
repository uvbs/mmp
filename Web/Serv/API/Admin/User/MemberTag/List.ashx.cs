using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.MemberTag
{
    /// <summary>
    /// List 的摘要说明   会员标签列表接口
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLLTag bllTag = new BLLJIMP.BLLTag();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            int totalCount = 0;
            var tagList = bllTag.GetTags(bllTag.WebsiteOwner,keyWord,pageIndex,pageSize,out totalCount,"Member");
            resp.isSuccess = true;
            List<dynamic> returnList = new List<dynamic>();
            foreach (var item in tagList)
            {
                returnList.Add(new 
                {
                    tag_id=item.AutoId,
                    tag_name=item.TagName,
                    access_level=item.AccessLevel
                }); 
            }
            resp.returnObj = new
            {
                totalcount=totalCount,
                list=returnList
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}