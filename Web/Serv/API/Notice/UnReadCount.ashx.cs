using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Notice
{
    /// <summary>
    /// UnReadCount 的摘要说明
    /// </summary>
    public class UnReadCount : NoticeBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            resp.isSuccess = false;

            try
            {
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                    bll.ContextResponse(context, resp);
                    return;
                }
                
                var data = bll.GetUnReadMsgCount(bll.GetCurrUserID(), BLLJIMP.BLLSystemNotice.NoticeType.SystemMessage);

                resp.isSuccess = true;
                
                resp.returnObj = new
                {
                    totalCount = data
                };

            }
            catch (Exception ex)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }

            bll.ContextResponse(context, resp);
        }

    }
}