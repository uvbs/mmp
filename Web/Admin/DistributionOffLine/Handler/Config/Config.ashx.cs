using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.Config
{
    /// <summary>
    /// Config 的摘要说明
    /// </summary>
    public class Config : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        /// <summary>
        /// 日志模块BLL
        /// </summary>
        BLLJIMP.BLLLog bllLog = new BLLJIMP.BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            string distributionOffLineLevel = context.Request["DistributionOffLineLevel"];
            string distributionOffLineShowLevel = context.Request["DistributionOffLineShowLevel"];
            string commissionShowName = context.Request["CommissionShowName"];
            string distributionShowName = context.Request["DistributionShowName"];
            string isShowDistributionOffLineRate = context.Request["IsShowDistributionOffLineRate"];
            string distributionOffLineDescription = context.Request["DistributionOffLineDescription"];
            string distributionOffLineSlideType = context.Request["DistributionOffLineSlideType"];
            string distributionOffLineIsShowMemberScore = context.Request["DistributionOffLineIsShowMemberScore"];
            string distributionOffLineApplyWaitInfo = context.Request["DistributionOffLineApplyWaitInfo"];
            string systemShowName=context.Request["SystemShowName"];


            WebsiteInfo currentWebsiteInfo = bll.GetWebsiteInfoModelFromDataBase();
            currentWebsiteInfo.DistributionOffLineLevel = int.Parse(distributionOffLineLevel);
            currentWebsiteInfo.DistributionOffLineShowLevel = int.Parse(distributionOffLineShowLevel);
            currentWebsiteInfo.CommissionShowName = commissionShowName;
            currentWebsiteInfo.DistributionShowName = distributionShowName;
            currentWebsiteInfo.IsShowDistributionOffLineRate = int.Parse(isShowDistributionOffLineRate);
            currentWebsiteInfo.DistributionOffLineDescription = distributionOffLineDescription;
            currentWebsiteInfo.DistributionOffLineIsShowMemberScore = Convert.ToInt32(distributionOffLineIsShowMemberScore);
            currentWebsiteInfo.DistributionOffLineSlideType = distributionOffLineSlideType;
            currentWebsiteInfo.DistributionOffLineApplyWaitInfo = distributionOffLineApplyWaitInfo;
            currentWebsiteInfo.DistributionOffLineSystemShowName = systemShowName;
            if (bll.Update(currentWebsiteInfo))
            {
                bllLog.Add(BLLJIMP.Enums.EnumLogType.DistributionOffLine,BLLJIMP.Enums.EnumLogTypeAction.Config,bllLog.GetCurrUserID(),"业务分销配置["+bllLog.GetCurrUserID()+"]");
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "保存失败";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));


        }
    }
}