using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.SystemNotice
{
    /// <summary>
    /// UnReadCount 的摘要说明
    /// </summary>
    public class UnReadCount : BaseHandlerNeedLoginNoAction
    {
        BLLSystemNotice bll = new BLLSystemNotice();
        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request["type"];
            BLLSystemNotice.NoticeType nType = new BLLSystemNotice.NoticeType();
            if (!string.IsNullOrWhiteSpace(type))
            {
                if (!Enum.TryParse(type, out nType))
                {
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    apiResp.msg = "类型格式不能识别";
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }
            string userId = CurrentUserInfo.UserID;
            int total = bll.GetUnReadMsgCount(userId, nType,bll.WebsiteOwner);
            apiResp.status = true;
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.msg = "查询完成";
            apiResp.result = total;
            bll.ContextResponse(context, apiResp);
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