using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.User
{
    /// <summary>
    /// 当前用户信息
    /// </summary>
    public class CurrentUserInfo : BaseHandlerNeedLoginNoAction
    {

        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {

            var websiteInfo = bll.GetWebsiteInfoModelFromDataBase();
            var websiteLogo = websiteInfo.WebsiteLogo;
            if (string.IsNullOrEmpty(websiteInfo.WebsiteLogo))
            {
                websiteLogo = ConfigHelper.GetConfigString("WebsiteLogo");
            }

            string nextLevelName = string.Empty;//下一等级名称
            double distanceNextLevelScore = 0;//距离下一等级差值

            UserLevelConfig currUserLevel = bll.GetUserLevel(CurrentUserInfo,out nextLevelName,out distanceNextLevelScore);

            apiResp.result = new
            {
                user_show_name = bllUser.GetUserDispalyName(CurrentUserInfo),
                website_logo = websiteLogo,
                website_name = websiteInfo.WebsiteName,
                is_distribution_member = IsDistributionMember(CurrentUserInfo, websiteInfo),//是否是分销会员
                recommend_id = CurrentUserInfo.AutoID,//我的推荐码
                recommend_count = bll.GetUserCommendCount(CurrentUserInfo.UserID),//推荐人数
                nick_name=CurrentUserInfo.WXNickname,//昵称
                true_name=CurrentUserInfo.TrueName,//真实姓名
                head_img_url = bllUser.GetUserDispalyAvatar(CurrentUserInfo),//头像
                history_commission_total_amount = CurrentUserInfo.HistoryDistributionOffLineTotalAmount,//累计佣金
                can_use_amount=bll.GetUserCanUseAmount(CurrentUserInfo),//可提现金额
                level_name = currUserLevel.LevelString,
                direct_rate = bll.GetUserLevel(CurrentUserInfo).DistributionRateLevel0,//直销佣金比例
                direct_sale_amount=bll.GetDirectSaleAmount(CurrentUserInfo.UserID),//累计直接销售
                down_user_total_count = bll.GetDownUserTotalCount(CurrentUserInfo.UserID,bll.GetDistributionLevel()),//下级用户总和
                down_user_level1_count=bll.GetDownUserCount(CurrentUserInfo.UserID,1),//一级分销用户数
                down_user_level2_count = bll.GetDownUserCount(CurrentUserInfo.UserID, 2),//二级分销用户数
                down_user_level3_count = bll.GetDownUserCount(CurrentUserInfo.UserID, 3),//三级分销用户数
                distribution_level1_rate = bll.GetUserLevel(CurrentUserInfo).DistributionRateLevel1,//一级分销佣金比例
                distribution_level2_rate = bll.GetUserLevel(CurrentUserInfo).DistributionRateLevel2,//二级分销佣金比例
                distribution_level3_rate = bll.GetUserLevel(CurrentUserInfo).DistributionRateLevel3,//三级分销佣金比例
                next_level_name = nextLevelName,//下个等级名称
                distance_next_level_score = distanceNextLevelScore //距离下个等级的积分
                
            };
            apiResp.status = true;
            apiResp.msg = "ok";
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }

        /// <summary>
        /// 是否是分销员
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="websiteInfo"></param>
        /// <returns></returns>
        public bool IsDistributionMember(UserInfo userInfo,WebsiteInfo websiteInfo) {

            if (websiteInfo.IsNeedDistributionRecommendCode==0)//不需要分销推荐码
            {
                if (string.IsNullOrEmpty(userInfo.DistributionOwner)&&(userInfo.UserID!=bll.WebsiteOwner))
                {
                 //不需要分销推荐码，用系统默认的
                    bll.Update(userInfo, string.Format(" DistributionOwner='{0}'",bll.WebsiteOwner), string.Format(" AutoId={0}", userInfo.AutoID));
   
                }
            }
            else//需要分销推荐码
            {

                if (string.IsNullOrEmpty(userInfo.DistributionOwner) && (userInfo.UserID != bll.WebsiteOwner))
                {
                    return false;//需要分销推荐码

                }

            }
            return true;
        
        }



    }
}