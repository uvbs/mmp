using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{
    /// <summary>
    /// 标签
    /// </summary>
    public class Tag : BaseHandlerNeedLoginAdmin
    {
        /// <summary>
        /// 标签BLL
        /// </summary>
        BLLJIMP.BLLTag bllTag = new BLLJIMP.BLLTag();

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {

            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string tagType=context.Request["tag_type"];
            string keyWord=context.Request["keyword"];
            int totalCount = 0;
            string type = "mall";
            if (!string.IsNullOrEmpty(tagType)) type = tagType;
            var sourceData = bllTag.GetTags(bllTag.WebsiteOwner, keyWord, pageIndex, pageSize, out totalCount, type);
            var list = from p in sourceData
                       select new
                       {
                           tag_id = p.AutoId,
                           tag_name = p.TagName

                       };

                     return ZentCloud.Common.JSONHelper.ObjectToJson(new {
                     totalcount=totalCount,
                     list=list
                     
                     });

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {

            string tagName = context.Request["tag_name"];
            BLLJIMP.Model.MemberTag model = new BLLJIMP.Model.MemberTag();
            model.CreateTime = DateTime.Now;
            model.Creator = currentUserInfo.UserID;
            model.TagName = tagName;
            model.TagType = "mall";
            model.WebsiteOwner = bllTag.WebsiteOwner;
            if(bllTag.AddTag(model)){
                resp.errmsg = "ok";
            
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "add fail";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            string tagId = context.Request["tag_id"];
            string tagName = context.Request["tag_name"];
            if (bllTag.UpdateTag(tagId, tagName))
            {
                resp.errmsg = "ok";

            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "update fail";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        ///删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            string tagIds = context.Request["tag_ids"];
            string tagName = context.Request["tag_name"];
            if (bllTag.DelTags(tagIds)==tagIds.Split(',').Length)
            {
                resp.errmsg = "ok";

            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "delete fail";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }




    }
}