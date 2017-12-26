using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User
{
    /// <summary>
    /// 设置核销码
    /// </summary>
    public class SetHexiaoCode : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string autoId = context.Request["AutoId"];
            string hexiaoCode = context.Request["HexiaoCode"];
            if (string.IsNullOrEmpty(autoId))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "autoid 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(hexiaoCode))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "核销码 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }

            if (bllUser.Update(new UserInfo(), string.Format(" HexiaoCode='{0}'", hexiaoCode), string.Format(" AutoID in ({0}) And (Select Count(*) from ZCJ_UserInfo Where WebsiteOwner='{1}' And AutoId !={0} And HexiaoCode='{2}')=0 And (DistributionOwner!='')", autoId, bllUser.WebsiteOwner, hexiaoCode)) > 0)
            {

                apiResp.status = true;
                apiResp.msg = "ok";
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "设置失败";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }

    }
}