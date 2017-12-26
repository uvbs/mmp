using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.SystemNotice
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {
        BLLSystemNotice bll = new BLLSystemNotice();
        int auto_read = 0;
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string type = context.Request["type"];
            string keyword = context.Request["keyword"];
            auto_read = Convert.ToInt32(context.Request["auto_read"]);

            BLLSystemNotice.NoticeType nType =  new BLLSystemNotice.NoticeType();
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
            List<BLLJIMP.Model.SystemNotice>  list = bll.GetMsgList(rows, page, CurrentUserInfo.UserID, nType, keyword, null,bll.WebsiteOwner);
            int total = bll.GetMsgCount(userId, nType);
            if (auto_read == 1 && list.Count>0)
            {
                string nids = ZentCloud.Common.MyStringHelper.ListToStr(list.Select(p => p.AutoID).ToList(),"",",");
                bll.SetReaded(nids);
            }

            apiResp.status = true;
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.msg = "查询完成";
            apiResp.result = new
            {
                list = from p in list
                       select new
                       {
                           id = p.AutoID,
                           title = p.Title,
                           type = p.NoticeType,
                           content = p.Ncontent,
                           time = p.InsertTime.ToString("yyyy/MM/dd HH:mm:ss"),
                           read = p.Readtime.HasValue
                       },
                total = total
            };
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