using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.User.Down
{
    /// <summary>
    /// 下级用户列表接口
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;//页码
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;//页数
            int level = !string.IsNullOrEmpty(context.Request["level"]) ? int.Parse(context.Request["level"]) : 1;//级数
            var childUserList = bll.GetDownUserList(CurrentUserInfo.UserID,level);
            var sourceList = childUserList.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var list = from p in sourceList
                       select new
                       {
                           nick_name =p.WXNickname,//昵称
                           head_img_url = bllUser.GetUserDispalyAvatar(p),//头像
                           level_name = bll.GetUserLevel(p).LevelString,//等级
                           contribution_commission_total_amount = bll.GetContributionCommissionAmount(CurrentUserInfo.UserID, p.UserID, 1) + bll.GetContributionCommissionAmount(CurrentUserInfo.UserID, p.UserID, 2)+bll.GetContributionCommissionAmount(CurrentUserInfo.UserID, p.UserID, 3),//合计贡献佣金
                           contribution_commission_amount = bll.GetContributionCommissionAmount(CurrentUserInfo.UserID, p.UserID, 1),//直接贡献佣金 //一级分销佣金
                           level2_contribution_commission_amount = bll.GetContributionCommissionAmount(CurrentUserInfo.UserID, p.UserID, 2),//二级分销贡献佣金
                           level3_contribution_commission_amount = bll.GetContributionCommissionAmount(CurrentUserInfo.UserID, p.UserID, 3)//三级分销贡献佣金
                       };
            var data = new
            {
                totalcount = childUserList.Count,
                list = list

            };
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = data;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }


    }
}