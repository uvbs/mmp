using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User
{
    /// <summary>
    /// SetDistributionLevel 的摘要说明  设置分销员等级
    /// </summary>
    public class SetDistributionLevel : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 用户 BLL
        /// </summary>

        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string autoIds = context.Request["autoid"];
            string memberLevel = context.Request["member_level"];

            if (string.IsNullOrEmpty(autoIds))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "autoids 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            double userLevel = 0;
            if (!string.IsNullOrEmpty(memberLevel))
            {
                if (!double.TryParse(memberLevel, out userLevel))
                {
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    apiResp.msg = "分销员等级错误";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "member_level 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }




            if (bllUser.Update(new UserInfo(), string.Format(" MemberLevel={0},Ex15=''", userLevel), string.Format(" WebsiteOwner='{0}' AND AutoID in ({1})", bllUser.WebsiteOwner, autoIds)) > 0)
            {
                apiResp.status = true;
                apiResp.msg = "ok";
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "设置分销员等级出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }

        
    }
}