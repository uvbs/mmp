using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Account
{
    /// <summary>
    /// 更新账户密码
    /// </summary>
    public class UpdatePassword : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int uid = int.Parse(context.Request["AutoID"]);
            UserInfo tUser = bllUser.GetUserInfoByAutoID(uid);
            tUser.Password = context.Request["Password"];
            if (string.IsNullOrWhiteSpace(tUser.Password))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "密码不能为空";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            BLLTransaction tran = new BLLTransaction();
            if (!bllUser.Update(tUser, tran))
            {
                tran.Rollback();
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "修改密码出错";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            tran.Commit();
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "修改完成";
            bllUser.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}