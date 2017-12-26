using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.User
{
    /// <summary>
    /// 会员列表
    /// </summary>
    public class List : BaseHanderOpen
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            string keyWord = context.Request["keyword"];
            string regTimeFrom = context.Request["reg_time_from"];
            string regTimeTo = context.Request["reg_time_to"];
            int totalCount = 0;
            var userList = bllUser.GetUserList(pageIndex, pageSize, keyWord, "", "", "", "", out totalCount, regTimeFrom, regTimeTo);
            var data = from p in userList
                       select new
                       {
                           id = p.AutoID,
                           head_img = p.WXHeadimgurl,
                           nick_name = p.WXNickname,
                           true_name = p.TrueName,
                           phone = p.Phone,
                           company = p.Company,
                           position = p.Postion,
                           email = p.Email,
                           tag = p.TagName,
                           total_score = p.TotalScore,
                           is_subscribe = p.IsWeixinFollower,
                           subscribe_time = p.SubscribeTime,
                           un_subscribe_time = p.UnSubscribeTime,
                           reg_time = p.Regtime != null ? ((DateTime)p.Regtime).ToString("yyyy-MM-dd HH:mm:ss") : ""
                       };

            resp.msg = "ok";
            resp.status = true;
            resp.result = new
            {
                totalcount = totalCount,
                list = data

            };
            bllUser.ContextResponse(context, resp);


        }


    }
}