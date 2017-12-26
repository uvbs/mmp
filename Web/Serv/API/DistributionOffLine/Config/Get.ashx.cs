using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.Config
{
    /// <summary>
    /// 分销配置信息接口
    /// </summary>
    public class Get : BaseHandlerNeedLoginNoAction
    {

        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        BLLJIMP.BLLWebSite bllWebSite = new BLLJIMP.BLLWebSite();
        public void ProcessRequest(HttpContext context)
        {
            WebsiteInfo currentWebSiteInfo = bll.GetWebsiteInfoModelFromDataBase();
            CompanyWebsite_Config websiteConfig = bllWebSite.GetCompanyWebsiteConfig();
            apiResp.result = new
            {
                website_name = currentWebSiteInfo.WebsiteName,//站点名称
                distribution_level = bll.GetDistributionLevel(),//后台配置的几级
                distribution_show_level = bll.GetDistributionShowLevel(),//会员显示分销级别
                commission_show_name = string.IsNullOrWhiteSpace(currentWebSiteInfo.CommissionShowName) ? "积分" : currentWebSiteInfo.CommissionShowName,//佣金显示名称
                distribution_show_name = string.IsNullOrWhiteSpace(currentWebSiteInfo.DistributionShowName) ? "会员" : currentWebSiteInfo.DistributionShowName,//分销显示名称
                is_show_distribution_rate = currentWebSiteInfo.IsShowDistributionOffLineRate,//是否显示分销比例
                share_title = websiteConfig.WebsiteTitle,//分享标题
                share_desc = websiteConfig.WebsiteDescription,//分享描述
                share_img_url = websiteConfig.WebsiteImage,//分享图片
                project_field_list = bll.GetProjectFieldMapListF(),//项目自定义字段
                slide_type = currentWebSiteInfo.DistributionOffLineSlideType,//广告类型
                is_show_member_score = currentWebSiteInfo.DistributionOffLineIsShowMemberScore,//是否显示会员积分
                system_show_name=currentWebSiteInfo.DistributionOffLineSystemShowName//分销系统显示名称
            };

            apiResp.status = true;
            apiResp.msg = "ok";
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }




    }
}