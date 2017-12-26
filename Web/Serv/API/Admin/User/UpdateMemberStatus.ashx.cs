using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User
{
    /// <summary>
    /// UpdateMemberStatus 的摘要说明  修改用户审核状态
    /// </summary>
    public class UpdateMemberStatus : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 用户 BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string autoIds=context.Request["ids"];
            string applyStatus=context.Request["status"];
            if (string.IsNullOrEmpty(autoIds))
            {
                apiResp.msg = "autoid为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
            }
            if (string.IsNullOrEmpty(applyStatus))
            {
                apiResp.msg = "status为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
            }
            string[] ids = autoIds.Split(',');
            bool result = false;
            foreach (var item in ids)
            {
                UserInfo model = bllUser.GetUserInfoByAutoID(int.Parse(item));
                if (model == null) continue;

                if (bllUser.UpdateUserMemberApplyStatus(int.Parse(item), int.Parse(applyStatus)))
                    result = true;
                else
                    result = false;
               
            }
            if (result)
            {
                apiResp.msg = "操作成功";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "操作失败";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }
    }
}