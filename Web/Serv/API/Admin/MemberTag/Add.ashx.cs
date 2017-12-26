using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.MemberTag
{
    /// <summary>
    /// 添加会员标签接口
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLTag bllTag = new BLLTag();
        public void ProcessRequest(HttpContext context)
        {
            string tagName = context.Request["name"],
                   tagType = context.Request["type"],
                   accessLevel = context.Request["level"];

            if (string.IsNullOrEmpty(tagName))
            {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "请输入标签名称";
                bllTag.ContextResponse(context, resp);
                return;
            }
            ZentCloud.BLLJIMP.Model.MemberTag tag = new ZentCloud.BLLJIMP.Model.MemberTag();
            tag.TagType = string.IsNullOrWhiteSpace(tagType) ? "all" : tagType;
            tag.TagName = tagName;
            tag.AccessLevel = string.IsNullOrWhiteSpace(accessLevel) ? 0 : Convert.ToInt32(accessLevel);
            tag.CreateTime = DateTime.Now;
            tag.Creator = currentUserInfo.UserID;
            tag.WebsiteOwner = bllTag.WebsiteOwner;
            if (bllTag.ExistsTag(tag))
            {
                resp.errcode = (int)APIErrCode.IsRepeat;
                resp.errmsg = "标签不能重复添加";
                bllTag.ContextResponse(context, resp);
                return;
            }

            if (bllTag.AddTag(tag))
            {
                resp.errcode = (int)APIErrCode.IsSuccess;
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "添加失败";
            }

            bllTag.ContextResponse(context, resp);
        }

    }
}