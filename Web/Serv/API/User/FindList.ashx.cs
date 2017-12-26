using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// FindList 的摘要说明
    /// </summary>
    public class FindList : BaseHandlerNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string phone = context.Request["phone"];
            string nickname = context.Request["nickname"];
            string ids = context.Request["ids"];
            int total = 0;
            List<UserInfo> list = bllUser.FindList(out total, rows, page, phone, nickname, bllUser.WebsiteOwner,"2",
                "AutoID,UserID,TrueName,WXNickname,WXHeadimgurl,UserType,Phone,Avatar,ViewType,TotalScore,OnlineTimes", ids);
            var result = from p in list
                         select new
                         {
                             id = p.AutoID,
                             username = p.UserID,
                             nickname = bllUser.GetUserDispalyName(p),
                             avatar = bllUser.GetUserDispalyAvatar(p),
                             phone = (p.ViewType == 0 ? p.Phone : ""),
                             score = p.TotalScore,
                             times = p.OnlineTimes,
                             isFriend = p.UserID == bllUser.GetCurrUserID() ? "1" : "0"
                         };
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.result = new
            {
                totalcount = total,
                list = result
            };
            apiResp.msg = "查询完成";
            bllUser.ContextResponse(context, apiResp);
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