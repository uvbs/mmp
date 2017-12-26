using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.BigData.PV
{
    /// <summary>
    /// List 的摘要说明 访问统计
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL 监测详细
        /// </summary>
        BLLJIMP.BLLMonitor bllMonitor = new BLLJIMP.BLLMonitor();
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string mType=context.Request["module_type"];//访问类型
            string dTime=context.Request["time"];//时间类型
            var monitorList = bllMonitor.GetMonitorStatisticsList(50, dTime, mType);
            List<dynamic> returnList = new List<dynamic>();
            
            foreach (var item in monitorList)
            {
                string title = string.Empty;
                switch (mType)
                {
                    case "activity":
                    case "article":
                    case "greetingcard":
                        JuActivityInfo jModel = bllMonitor.Get<JuActivityInfo>(string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} ",bllMonitor.WebsiteOwner,item.MonitorPlanID));
                        if (jModel == null) continue;
                        title = jModel.ActivityName;
                        returnList.Add(new
                        {
                            id = jModel.JuActivityID,
                            title = title,
                            count = item.tCount
                        });
                        break;
                    case "product":
                        WXMallProductInfo pModel = bllMall.GetProduct(item.MonitorPlanID.ToString());
                        if (pModel == null) continue;
                        title = pModel.PName;
                        returnList.Add(new
                        {
                            id = pModel.PID,
                            title = title,
                            count = item.tCount
                        });
                        break;
                    case "question":
                        Questionnaire qModel = bllMall.Get<Questionnaire>(string.Format(" WebsiteOwner='{0}' AND QuestionnaireID={1}",bllMall.WebsiteOwner,item.MonitorPlanID));
                        if (qModel == null) continue;
                        title = qModel.QuestionnaireName;
                        returnList.Add(new
                        {
                            id = qModel.QuestionnaireID,
                            title = title,
                            count = item.tCount
                        });
                        break;
                    case "questionnaireset":
                        BLLJIMP.Model.QuestionnaireSet sModel = bllMall.Get<BLLJIMP.Model.QuestionnaireSet>(string.Format(" WebsiteOwner='{0}' AND AutoId={1}",bllMall.WebsiteOwner,item.MonitorPlanID));
                        if (sModel == null) continue;
                        title = sModel.Title;
                        returnList.Add(new
                        {
                            id = sModel.AutoID,
                            title = title,
                            count = item.tCount
                        });
                        break;
                    case "thevote":
                         TheVoteInfo tModel = bllMall.Get<TheVoteInfo>(string.Format(" WebsiteOwner='{0}' AND AutoID={1}", bllMall.WebsiteOwner, item.MonitorPlanID));
                         if (tModel == null) continue;
                         title = tModel.VoteName;
                         returnList.Add(new
                         {
                             id = tModel.AutoId,
                             title = title,
                             count = item.tCount
                         });
                         break;
                    case "scratch":
                    case "shake":
                        WXLotteryV1 wModel = bllMall.Get<WXLotteryV1>(string.Format(" WebsiteOwner='{0}' AND LotteryID={1} ", bllMall.WebsiteOwner, item.MonitorPlanID));
                        if (wModel == null) continue;
                        title = wModel.LotteryName;
                        returnList.Add(new
                        {
                            id = wModel.LotteryID,
                            title = title,
                            count = item.tCount
                        });
                        break;
                    case "wshow":
                        WXShowInfo xModel = bllMall.Get<WXShowInfo>(string.Format(" WebsiteOwner='{0}' AND AutoId={1}", bllMall.WebsiteOwner, item.MonitorPlanID));
                        if (xModel == null) continue;
                        title = xModel.ShowName;
                        returnList.Add(new
                        {
                            id = xModel.AutoId,
                            title = title,
                            count = item.tCount
                        });
                        
                        break;
                    default:
                        break;
                }
            }
         
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(returnList));


        }

       
    }
}