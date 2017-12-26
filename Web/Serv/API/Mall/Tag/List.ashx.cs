using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall.Tag
{
    /// <summary>
    /// List 商品标签列表
    /// </summary>
    public class List : BaseHandlerNoAction
    {
        /// <summary>
        /// 标签逻辑层
        /// </summary>
        BLLJIMP.BLLTag bllBag = new BLLJIMP.BLLTag();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string tagName = context.Request["tag_name"];
            string tagType = context.Request["tag_type"];
            if (string.IsNullOrEmpty(tagType))
            {
                apiResp.msg = "标签类型为空,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllBag.ContextResponse(context, apiResp);
                return;
            }
            int totalCount=0;
            List<ZentCloud.BLLJIMP.Model.MemberTag> tagList = bllBag.GetTags(bllBag.WebsiteOwner,tagName,pageIndex,pageSize,out totalCount,tagType);
            List<dynamic> returnList = new List<dynamic>();
            foreach (var item in tagList)
            {
                returnList.Add(new 
                {
                    id=item.AutoId,
                    tag_name=item.TagName,
                    access_level=item.AccessLevel
                });
            }
            apiResp.result= new 
            {
                list=returnList,
                totalcount=totalCount
            };
            apiResp.status = true;
            apiResp.msg = "查询完成";
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }

        
    }
}