using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLPermission;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.MemberTag
{
    /// <summary>
    /// 删除会员标签接口
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLTag bllTag = new BLLTag();
        BLLMenuPermission bllMenupermission = new BLLMenuPermission("");
        public void ProcessRequest(HttpContext context)
        {
            bool isData = bllMenupermission.CheckPerRelationByaccount(currentUserInfo.UserID, -1);
            if (isData)
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "权限不足";
                bllTag.ContextResponse(context, resp);
                return;
            }
            string ids = context.Request["ids"];
            if (string.IsNullOrWhiteSpace(ids))
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "ids不能为空";
                bllTag.ContextResponse(context, resp);
                return;
            }

            if (bllTag.ExistsTag(ids, bllTag.WebsiteOwner))
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "存在其他站点标签无权删除";
                bllTag.ContextResponse(context, resp);
                return;
            }
            List<BLLJIMP.Model.MemberTag> tags = bllTag.GetTagListByIds(ids);

            string usingTag = string.Empty;
            if (bllTag.IsUsingTag(tags, bllTag.WebsiteOwner, out usingTag))
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = string.Format("用户已添加'{0}'标签", usingTag);
                bllTag.ContextResponse(context, resp);
                return;
            }
            int count = bllTag.DeleteMultByKey<BLLJIMP.Model.MemberTag>("AutoID", ids);
            if (count >= 0)
            {
                resp.errcode = (int)APIErrCode.IsSuccess;
                resp.errmsg = string.Format("成功删除了 {0} 条数据", count);
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "删除标签失败";
            }
            bllTag.ContextResponse(context, resp);
        }

    }
}