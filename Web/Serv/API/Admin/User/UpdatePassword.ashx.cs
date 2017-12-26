using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User
{
    /// <summary>
    /// UpdatePassword 的摘要说明  修改用户密码
    /// </summary>
    public class UpdatePassword : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL 用户
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string autoId = context.Request["autoid"];
            string passWord=context.Request["pwd"];
            if (string.IsNullOrEmpty(autoId))
            {
                apiResp.msg = "autoid为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
            }
            if (string.IsNullOrEmpty(passWord))
            {
                apiResp.msg = "密码为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
            }
            UserInfo model = bllUser.GetUserInfoByAutoID(int.Parse(autoId));
            if (model == null)
            {
                apiResp.msg = "用户不存在";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
            }
            model.Password = passWord;
            if (bllUser.UpdatePassword(model))
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