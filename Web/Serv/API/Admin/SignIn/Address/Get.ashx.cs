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
    /// 获取签到地址
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLSignIn bllSignIn = new BLLSignIn();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            SignInAddress signInAddress = bllSignIn.GetByKey<SignInAddress>("AutoID", id);
   
            if (signInAddress == null)
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "地址未找到";
                bllSignIn.ContextResponse(context, apiResp);
                return;
            }
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "查询完成";
            apiResp.result = new
            {
                id = signInAddress.AutoID,
                address = signInAddress.Address,
                longitude = signInAddress.Longitude,
                latitude = signInAddress.Latitude,
                range = signInAddress.Range,
                isdelete = signInAddress.IsDelete
            };
            bllSignIn.ContextResponse(context, apiResp);
        }
    }
}