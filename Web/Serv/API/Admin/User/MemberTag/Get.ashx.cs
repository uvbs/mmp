using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.MemberTag
{
    /// <summary>
    /// Get 的摘要说明   会员表情详情接口
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLTag bllTag = new BLLJIMP.BLLTag();
        public void ProcessRequest(HttpContext context)
        {
            string tagId = context.Request["tag_id"];
            if (string.IsNullOrEmpty(tagId))
            {
                resp.errmsg = "tag_id 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            BLLJIMP.Model.MemberTag model = bllTag.GetTag(int.Parse(tagId));
            if (model == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "不存在该条会员标签";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            resp.isSuccess = true;
            resp.returnObj=new
            {
                tag_name = model.TagName,
                access_level = model.AccessLevel
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

    }
}