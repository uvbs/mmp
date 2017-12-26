using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Handler
{
    /// <summary>
    /// TutorApplyHander 的摘要说明
    /// </summary>
    public class TagHandler : IHttpHandler, IRequiresSessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLUser bllUser = new BLLUser();
        UserInfo currentUserInfo;
        BLLTag bllTag = new BLLTag();
        
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();

                if (currentUserInfo == null)
                {
                    resp.Status = (int)APIErrCode.UserIsNotLogin;
                    resp.Msg = "用户未登录";
                    result = Common.JSONHelper.ObjectToJson(resp);
                    return;
                }

                string action = context.Request["Action"];
                switch (action)
                {
                    case "QueryTag":
                        result = QueryTag(context);
                        break;
                    case "AddTag":
                        result = AddTag(context);
                        break;
                    case "UpdateTagName":
                        result = UpdateTagName(context);
                        break;
                    case "DeleteTag":
                        result = DeleteTag(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);

            }
            context.Response.Write(result);
        }

        #region 用户标签管理
        /// <summary>
        /// 设计标签
        /// </summary>
        ///  #region 用户标签管理
        /// <summary>
        /// 查询用户标签管理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryTag(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string tagName = context.Request["TagName"], tagType = context.Request["TagType"];
            int totalCount = 0;
            List<MemberTag> dataList = bllTag.GetTags(bllTag.WebsiteOwner, tagName, pageIndex, pageSize, out totalCount, tagType);
            return Common.JSONHelper.ObjectToJson(
     new
     {
         total = totalCount,
         rows = dataList
     });
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddTag(HttpContext context)
        {
            string tagName = context.Request["TagName"],
                   tagType = context.Request["TagType"];

            if (string.IsNullOrEmpty(tagName))
            {
                resp.Status = 0;
                resp.Msg = "请输入标签名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            MemberTag tag = new MemberTag();
            tag.TagType = string.IsNullOrEmpty(tagType) ? "all" : tagType;
            tag.TagName = tagName;
            tag.CreateTime = DateTime.Now;
            tag.Creator = this.currentUserInfo.UserID;
            tag.WebsiteOwner = bllTag.WebsiteOwner;
            if (bllTag.ExistsTag(tag))
            {
                resp.Status = 0;
                resp.Msg = "标签不能重复添加";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (bllTag.AddTag(tag))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }
        /// <summary>
        /// 修改标签名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateTagName(HttpContext context)
        {
            string autoId = context.Request["AutoID"];
            string tagName = context.Request["TagName"];

            MemberTag tag = bllTag.GetTag(int.Parse(autoId));
            if (bllTag.ExistsTag(new MemberTag() { TagName = tagName, TagType = tag.TagType, WebsiteOwner = tag.TagType }))
            {
                resp.Status = 0;
                resp.Msg = "标签名重复，不能修改成该名称";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if(bllTag.UpdateTag(autoId, tagName))
            {
                resp.Status = 1;
                resp.Msg="修改成功";
            }
            else
            {
                resp.Msg = "修改失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteTag(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (bllTag.DelTags(ids) > 0)
            {
                resp.Status = 1;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}