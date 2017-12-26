using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Log
{
    /// <summary>
    /// AddMemberLog 的摘要说明
    /// </summary>
    public class AddMemberLog : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLLog bllLog = new BLLJIMP.BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            string target_id = context.Request["target_id"];
            string content = context.Request["content"];
            if(string.IsNullOrWhiteSpace(target_id)) {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "未找到会员！";
                bllLog.ContextResponse(context, apiResp);
                return;
            }
            if(string.IsNullOrWhiteSpace(content)) {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "请输入日志内容！";
                bllLog.ContextResponse(context, apiResp);
                return;
            }
            if(bllLog.Add(BLLJIMP.Enums.EnumLogType.ShMember,BLLJIMP.Enums.EnumLogTypeAction.Add,
                currentUserInfo.UserID, content, "", target_id))
            {
                apiResp.status = true;
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                apiResp.msg = "新增日志完成！";
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "新增日志出错！";
            }
            bllLog.ContextResponse(context, apiResp);
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