using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User
{
    /// <summary>
    /// EditDistribution 的摘要说明   设置分销员
    /// </summary>
    public class EditDistribution : BaseHandlerNeedLoginAdminNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            string autoid=context.Request["autoid"];//会员autoid
            string memberLevel=context.Request["member_level"];//分销员等级
            if (string.IsNullOrEmpty(autoid))
            {
                apiResp.msg = "autoid 为必填项";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(memberLevel))
            {
                apiResp.msg = "member_level 为必填项";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }

            if (bllUser.Update(new UserInfo(), string.Format(" DistributionOwner='{0}',MemberLevel={1} ", bllUser.WebsiteOwner, int.Parse(memberLevel)), string.Format(" WebsiteOwner='{0}' AND AutoID={1} ", bllUser.WebsiteOwner,int.Parse(autoid))) > 0)
            {
                apiResp.msg = "添加分销员成功";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "添加分销员出错";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }

      
    }
}