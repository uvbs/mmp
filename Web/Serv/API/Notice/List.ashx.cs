using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Notice
{
    /// <summary>
    /// List 的摘要说明   消息列表
    /// </summary>
    public class List : NoticeBase
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

                var pageSize = Convert.ToInt32(context.Request["pageSize"]);
                var pageIndex = Convert.ToInt32(context.Request["pageIndex"]);
                var keyword = context.Request["keyword"];
                var noticeType = context.Request["notice_type"];
                int type = noticeType!=null?Convert.ToInt32(noticeType):0;
                BLLJIMP.BLLSystemNotice.NoticeType notice;
                List<BLLJIMP.Model.SystemNotice> list = new List<BLLJIMP.Model.SystemNotice>();
                int totalCount = 0;

                if (type != 0)
                {
                    notice = (BLLJIMP.BLLSystemNotice.NoticeType)type; 
                    list=bll.GetMsgList(pageSize, pageIndex, bll.GetCurrUserID(), notice, keyword, null);
                    totalCount = bll.GetMsgCount(bll.GetCurrUserID(), notice);
                }
                else
                {
                    list = bll.GetMsgList(pageSize, pageIndex, bll.GetCurrUserID(), null, keyword, null);
                    totalCount = bll.GetMsgCount(bll.GetCurrUserID(), null);
                }

                List<dynamic> returnList = new List<dynamic>();

                resp.isSuccess = true;

                foreach (var item in list)
                {
                    returnList.Add(new
                    {
                        id = item.AutoID,
                        title = item.Title,
                        content = item.Ncontent,
                        sendTime = item.InsertTimeString,
                        readTime = item.ReadtimeString,
                        url = item.RedirectUrl
                    });
                }

                resp.returnObj = new
                {
                    totalCount = totalCount,
                    list = returnList
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