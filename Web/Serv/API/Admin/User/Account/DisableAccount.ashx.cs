using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Account
{
    /// <summary>
    /// DisableAccount 的摘要说明
    /// </summary>
    public class DisableAccount : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            string autoIds = context.Request["autoids"];
            if (string.IsNullOrEmpty(autoIds))
            {
                resp.errmsg = "autoid 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bll.Update(new UserInfo(), string.Format(" IsDisable=1 "), string.Format(" WebsiteOwner='{0}' And IsSubAccount='1' And AutoID in ({1})", bll.WebsiteOwner, autoIds)) == autoIds.Split(',').Length)
            {
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errmsg = "禁用子账户出错";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}