using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User
{
    /// <summary>
    /// 设置用户文章活动访问等级
    /// </summary>
    public class SetLevel : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int autoid = !string.IsNullOrEmpty(context.Request["autoid"]) ? int.Parse(context.Request["autoid"]) : 0;
            string accessLevel = context.Request["user_access_level"];
            double userLevel = 0;
            if (autoid <= 0)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "autoid 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (!string.IsNullOrEmpty(accessLevel))
            {
                if (!double.TryParse(accessLevel, out userLevel))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    resp.errmsg = "用户等级不正确";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
            }
            UserInfo userInfo = bllUser.GetUserInfoByAutoID(autoid, bllUser.WebsiteOwner);
            if (userInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "没有找到该条会员信息";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bllUser.Update(new UserInfo(), string.Format(" AccessLevel={0}", userLevel), string.Format(" WebsiteOwner='{0}' AND AutoID={1}",bllUser.WebsiteOwner,autoid)) > 0)
            {
                resp.isSuccess = true;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "设置访问等级出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}