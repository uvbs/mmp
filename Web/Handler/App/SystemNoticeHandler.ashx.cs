using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using System.Reflection;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// 系统通知
    /// </summary>
    public class SystemNoticeHandler : BaseHandler
    {
        /// <summary>
        /// 通知BLL
        /// </summary>
        BLLSystemNotice bllNotice = new BLLSystemNotice();
        private string GetSystemNoticeList(HttpContext context)
        {
            try
            {
                BLLSystemNotice.NoticeType noticeType = (BLLSystemNotice.NoticeType)int.Parse(context.Request["noticeType"]);
                int pageIndex = int.Parse(context.Request["pageIndex"]);
                int pageSize = int.Parse(context.Request["pageSize"]);
                List<SystemNotice> systemNoticeList = new BLLSystemNotice().GetUnReadMsgList(pageSize, pageIndex, currUserInfo.UserID, noticeType);
                if (systemNoticeList != null && systemNoticeList.Count > 0)
                {
                    resp.Status = 0;
                    resp.ExObj = systemNoticeList;
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "没有数据";
                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 设置消息为已读
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetRead(HttpContext context)
        {
            int id = int.Parse(context.Request["Id"]);
            SystemNotice model = bllNotice.Get<SystemNotice>(string.Format("AutoID={0}", id));
            if (model.Readtime == null)
            {
                model.Readtime = DateTime.Now;
            }
            else
            {
                resp.Status = 1;
            }
            if (bllNotice.Update(model))
            {
                resp.Status = 1;
            }


            return Common.JSONHelper.ObjectToJson(resp);


        }

    }
}