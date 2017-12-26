using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Notice
{
    /// <summary>
    /// AddSystemNotice 的摘要说明
    /// </summary>
    public class AddSystemNotice : NoticeBase
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
                var title = context.Request["title"];
                var content = context.Request["content"];
                var url = context.Request["url"];
                var noticeType = context.Request["notice_type"];
                var userId = context.Request["userid"];
                ReturnValue value;
                switch (noticeType)
                {
                    case "1":
                        value = bll.SendSystemMessage(title, content, BLLJIMP.BLLSystemNotice.NoticeType.SystemMessage, BLLJIMP.BLLSystemNotice.SendType.Personal, userId, url);
                        break;
                    case "31":
                        value = bll.SendSystemMessage(title, content, BLLJIMP.BLLSystemNotice.NoticeType.FinancialNotice, BLLJIMP.BLLSystemNotice.SendType.Personal, userId, url);
                        break;
                    case "32":
                        value = bll.SendSystemMessage(title, content, BLLJIMP.BLLSystemNotice.NoticeType.AppointmentNotice, BLLJIMP.BLLSystemNotice.SendType.Personal, userId, url);
                        break;
                    default:
                        value = bll.SendSystemMessage(title, content, BLLJIMP.BLLSystemNotice.NoticeType.SystemMessage, BLLJIMP.BLLSystemNotice.SendType.Personal, userId, url);
                        break;
                }
                resp.isSuccess = value.Code == 0;
                resp.errmsg = value.Msg;
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