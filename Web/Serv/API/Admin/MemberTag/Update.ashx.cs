using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.MemberTag
{
    /// <summary>
    /// 更新会员标签接口
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLTag bllTag = new BLLTag();
        public void ProcessRequest(HttpContext context)
        {
            string autoId = context.Request["id"],
                tagName = context.Request["name"],
                tagType = context.Request["type"],
                accessLevel = context.Request["level"];

            if (string.IsNullOrEmpty(tagName))
            {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "请输入标签名称";
                bllTag.ContextResponse(context, resp);
                return;
            }

            BLLJIMP.Model.MemberTag tag = bllTag.GetByKey<BLLJIMP.Model.MemberTag>("AutoId", autoId);
            if (tag == null)
            {
                resp.errcode = (int)APIErrCode.IsNotFound;
                resp.errmsg = "未找到需要更新的标签";
                bllTag.ContextResponse(context, resp);
                return;
            }

            tag.TagName = tagName;
            tag.AccessLevel = string.IsNullOrWhiteSpace(accessLevel) ? 0 : Convert.ToInt32(accessLevel);
            if (bllTag.ExistsTag(tag))
            {
                resp.errcode = (int)APIErrCode.IsRepeat;
                resp.errmsg = "标签不能重复添加";
                bllTag.ContextResponse(context, resp);
                return;
            }
            if (bllTag.Update(tag))
            {
                resp.errcode = (int)APIErrCode.IsSuccess;
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "更新失败";
            }

            bllTag.ContextResponse(context, resp);
        }

    }
}