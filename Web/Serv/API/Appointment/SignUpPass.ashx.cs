using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Appointment
{
    /// <summary>
    /// SignUpPass 的摘要说明
    /// </summary>
    public class SignUpPass : BaseHandlerNeedLoginNoAction
    {

        BLLJuActivity bll = new BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            int activity_id = Convert.ToInt32(context.Request["activity_id"]);
            string signup_pass_ids = context.Request["signup_pass_ids"];
            if (bll.PassSignUp(activity_id, signup_pass_ids, bll.WebsiteOwner, context.Request["notice_signup_pass"]))
            {
                //bll.ReturnGuaranteeCreditAcount(activity_id, bll.WebsiteOwner);//返还信用金
                apiResp.msg = "通过成功";
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "通过失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}