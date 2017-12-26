using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.SignIn.Address
{
    /// <summary>
    /// EditTime 的摘要说明 编辑签到时间
    /// </summary>
    public class EditTime : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLSignIn bllSignIn = new BLLJIMP.BLLSignIn();
        public void ProcessRequest(HttpContext context)
        {
            string addressId=context.Request["address_id"];
            string times = context.Request["times"];
            SignInAddress model = bllSignIn.GetSignInAddress(bllSignIn.WebsiteOwner,addressId);
            if (model == null)
            {
                apiResp.msg = "签到地址不存在";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            model.SignInTime = times;
            if (bllSignIn.Update(model))
            {
                apiResp.msg = "编辑完成";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "编辑出错";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }
    }
}