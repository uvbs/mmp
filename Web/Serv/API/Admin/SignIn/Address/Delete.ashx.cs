using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.SignIn.Address
{
    /// <summary>
    /// 删除签到地址
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLSignIn bllSignIn = new BLLSignIn();

        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["ids"];
            if (bllSignIn.DeleteMultByKey<SignInAddress>("AutoID", id) > 0)
            {
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "删除完成";
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "删除失败";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
           
        }
    }
}