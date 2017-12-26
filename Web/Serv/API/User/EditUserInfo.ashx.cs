using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// EditUserInfo 的摘要说明
    /// </summary>
    public class EditUserInfo : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();

        public void ProcessRequest(HttpContext context)
        {
            string company=context.Request["company"];
            string autoid = context.Request["autoid"];
            if (!string.IsNullOrEmpty(autoid))
            {
                apiResp.msg = "autoid为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                bllUser.ContextResponse(context,apiResp);
                return;
            }
            if (!string.IsNullOrEmpty(company))
            {
                apiResp.msg = "请输入公司";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                bllUser.ContextResponse(context,apiResp);
                return;
            }
            UserInfo model = bllUser.GetUserInfoByAutoID(int.Parse(autoid));
            if (model == null)
            {
                apiResp.msg = "公司不存在";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllUser.ContextResponse(context,apiResp);
                return;
            }
            if (bllUser.UpdateUserInfo(model))
            {
                apiResp.msg = "编辑完成";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "编辑出错";
                apiResp.status = false;
            }
            bllUser.ContextResponse(context,apiResp);
        }

        
    }
}