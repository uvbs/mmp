using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.App.Distribution.m
{
    public partial class index : System.Web.UI.Page
    {
        /// <summary>
        /// 基本响应
        /// </summary>
        BaseResponse apiResp = new BaseResponse();
        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 网站BLL
        /// </summary>
        BLLJIMP.BLLWebSite bllWebSite = new BLLJIMP.BLLWebSite();
        /// <summary>
        /// KeyValue BLL
        /// </summary>
        BLLJIMP.BLLKeyValueData bllKeyValue = new BLLJIMP.BLLKeyValueData();
        protected void Page_Load(object sender, EventArgs e)
        {

            //if (!Request.UserAgent.ToLower().Contains("micromessenger"))
            //{
            //    // ToLog("非微信浏览器进入不执行授权");
            //    Response.Write("请用微信浏览器打开");
            //    Response.End();
            //    return;//非微信浏览器进入不执行授权
            //}
            if (!bllUser.IsLogin)
            {
                Response.Write("请用微信打开");
                Response.End();
                return;
            }
            var website = bll.GetWebsiteInfoModelFromDataBase();
            CompanyWebsite_Config companyWebsiteConfig = bllWebSite.GetCompanyWebsiteConfig(); ;
            string indexStr = File.ReadAllText(this.Server.MapPath(@"\App\Distribution\m\app.html"));
            UserInfo CurrentUserInfo = bllUser.GetCurrentUserInfo();
            if (indexStr.Contains("'$$CURRENTUSERINFO$$'"))
            {

                var websiteLogo = companyWebsiteConfig.WebsiteImage;
                if (string.IsNullOrEmpty(websiteLogo))
                {
                    websiteLogo = ConfigHelper.GetConfigString("WebsiteLogo");

                }
                else
                {
                    websiteLogo = bll.GetImgUrl(websiteLogo);
                }

                string nextLevelName = string.Empty;
                double distanceNextLevelScore = 0;

                UserLevelConfig currUserLevel = bll.GetUserLevel(CurrentUserInfo, out nextLevelName, out distanceNextLevelScore);


                apiResp.result = new
                {
                    user_show_name = bllUser.GetUserDispalyName(CurrentUserInfo),
                    website_logo = websiteLogo,
                    website_name = website.WebsiteName,
                    is_distribution_member = IsDistributionMember(CurrentUserInfo, website),//是否是分销会员
                    recommend_id = CurrentUserInfo.AutoID,//我的推荐码
                    recommend_count = bll.GetUserCommendCount(CurrentUserInfo.UserID),//推荐人数
                    nick_name = CurrentUserInfo.WXNickname,//昵称
                    true_name = CurrentUserInfo.TrueName,//真实姓名
                    head_img_url = bllUser.GetUserDispalyAvatar(CurrentUserInfo),//头像
                    history_commission_total_amount = CurrentUserInfo.HistoryDistributionOffLineTotalAmount,//累计佣金
                    can_use_amount = bll.GetUserCanUseAmount(CurrentUserInfo),//可提现金额
                    level_name = currUserLevel.LevelString,
                    direct_rate = bll.GetUserLevel(CurrentUserInfo).DistributionRateLevel0,//直销佣金比例
                    direct_sale_amount = bll.GetDirectSaleAmount(CurrentUserInfo.UserID),//累计直接销售
                    down_user_total_count = bll.GetDownUserTotalCount(CurrentUserInfo.UserID, bll.GetDistributionLevel()),//下级用户总和
                    down_user_level1_count = bll.GetDownUserCount(CurrentUserInfo.UserID, 1),//一级分销用户数
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
                indexStr = indexStr.Replace("'$$CURRENTUSERINFO$$'", ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
            }

            if (indexStr.Contains("'$$fx-websiteConfig$$'"))
            {
                
                dynamic websiteConfigResult = new
                {
                    website_name = website.WebsiteName,//站点名称
                    distribution_level = bll.GetDistributionLevel(),//后台配置的几级
                    distribution_show_level = bll.GetDistributionShowLevel(),//会员显示分销级别
                    commission_show_name = string.IsNullOrWhiteSpace(website.CommissionShowName) ? "积分" : website.CommissionShowName,//佣金显示名称
                    distribution_show_name = string.IsNullOrWhiteSpace(website.DistributionShowName) ? "会员" : website.DistributionShowName,//分销显示名称
                    is_show_distribution_rate = website.IsShowDistributionOffLineRate,//是否显示分销比例
                    share_title = companyWebsiteConfig.WebsiteTitle,//分享标题
                    share_desc = companyWebsiteConfig.WebsiteDescription,//分享描述
                    share_img_url = companyWebsiteConfig.WebsiteImage,//分享图片
                    project_field_list = bll.GetProjectFieldMapListF(),//项目自定义字段
                    slide_type = website.DistributionOffLineSlideType,
                    is_show_member_score = website.DistributionOffLineIsShowMemberScore
                };

                indexStr = indexStr.Replace("'$$fx-websiteConfig$$'", ZentCloud.Common.JSONHelper.ObjectToJson(websiteConfigResult));

            }

            if (indexStr.Contains("$$fx-wrapApplyHeader$$"))
            {
                indexStr = indexStr.Replace("$$fx-wrapApplyHeader$$", website.DistributionOffLineDescription);
            }

            if (indexStr.Contains("$$fx-wrapApplyWaitInfo$$"))
            {
                indexStr = indexStr.Replace("$$fx-wrapApplyWaitInfo$$", website.DistributionOffLineApplyWaitInfo);
            }

            //直接返回广告数组
            if (indexStr.Contains("'$$fx-slides$$'") && !string.IsNullOrWhiteSpace(website.DistributionOffLineSlideType))
            {

                string slideType = website.DistributionOffLineSlideType;
                System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebsiteOwner='{0}'", bll.WebsiteOwner));
                if (!string.IsNullOrEmpty(slideType))
                {
                    sbWhere.AppendFormat(" And Type='{0}'", slideType);
                }
                sbWhere.Append(" order by Sort DESC");
                var sourceData = bll.GetList<BLLJIMP.Model.Slide>(sbWhere.ToString());
                var list = from p in sourceData
                           select new
                           {
                               img_url = bll.GetImgUrl(p.ImageUrl),
                               link = p.Link,
                               slide_type = p.Type,
                               link_text = p.LinkText
                           };
                
                var data = new
                {
                    totalcount = sourceData.Count,
                    proportion = bllKeyValue.GetSlideProportion(slideType),
                    list = list,//列表

                };

                indexStr = indexStr.Replace("'$$fx-slides$$'", ZentCloud.Common.JSONHelper.ObjectToJson(data));
            }

            this.Response.Write(indexStr);
        }

        /// <summary>
        /// 是否是分销员
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="websiteInfo"></param>
        /// <returns></returns>
        public bool IsDistributionMember(UserInfo userInfo, WebsiteInfo websiteInfo)
        {

            if (websiteInfo.IsNeedDistributionRecommendCode == 0)//不需要分销推荐码
            {
                if (string.IsNullOrEmpty(userInfo.DistributionOwner) && (userInfo.UserID != bll.WebsiteOwner))
                {
                    //不需要分销推荐码，用系统默认的
                    bll.Update(userInfo, string.Format(" DistributionOwner='{0}'", bll.WebsiteOwner), string.Format(" AutoId={0}", userInfo.AutoID));

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