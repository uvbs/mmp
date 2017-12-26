using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.MonitorEventDetail
{
    /// <summary>
    /// 添加事件记录
    /// </summary>
    public class Add : BaseHandlerNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        BLLJIMP.BllLottery bllLottery = new BLLJIMP.BllLottery();
        public void ProcessRequest(HttpContext context)
        {
            string lotteryId = context.Request["lottery_id"];
            WXLotteryV1 model = bllLottery.GetLottery(int.Parse(lotteryId));
            if (model == null)
            {
                apiResp.msg = "摇一摇不存在";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            MonitorEventDetailsInfo detailInfo = new MonitorEventDetailsInfo();
            detailInfo.MonitorPlanID = int.Parse(lotteryId);
            detailInfo.EventType = 0;
            detailInfo.EventBrowser = HttpContext.Current.Request.Browser == null ? "" : HttpContext.Current.Request.Browser.ToString();
            detailInfo.EventBrowserID = HttpContext.Current.Request.Browser.Id; ;
            if (HttpContext.Current.Request.Browser.Beta)
                detailInfo.EventBrowserIsBata = "测试版";
            else
                detailInfo.EventBrowserIsBata = "正式版";

            detailInfo.EventBrowserVersion = HttpContext.Current.Request.Browser.Version;
            detailInfo.EventDate = DateTime.Now;
            if (HttpContext.Current.Request.Browser.Win16)
                detailInfo.EventSysByte = "16位系统";
            else
                if (HttpContext.Current.Request.Browser.Win32)
                    detailInfo.EventSysByte = "32位系统";
                else
                    detailInfo.EventSysByte = "64位系统";
            detailInfo.EventSysPlatform = HttpContext.Current.Request.Browser.Platform;
            detailInfo.SourceIP = ZentCloud.Common.MySpider.GetClientIP();
            detailInfo.IPLocation = ZentCloud.Common.MySpider.GetIPLocation(detailInfo.SourceIP);
            detailInfo.SourceUrl = HttpContext.Current.Request.Url.ToString();
            detailInfo.WebsiteOwner = bll.WebsiteOwner;
            if (bll.IsLogin)
            {
                detailInfo.EventUserID = bll.GetCurrUserID();
            }
            bool eventDetail = bll.Add(detailInfo);

            int ipCount = bll.GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} ", bll.WebsiteOwner, int.Parse(lotteryId)));
            int uvCount = bll.GetCount<MonitorEventDetailsInfo>(" EventUserID ", string.Format(" EventUserID is not null AND WebsiteOwner='{0}' AND MonitorPlanID={1} ", bll.WebsiteOwner, int.Parse(lotteryId)));
            int pvCount = bll.GetCount<MonitorEventDetailsInfo>(string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} ", bll.WebsiteOwner, int.Parse(lotteryId)));

            bll.Update(new WXLotteryV1(), string.Format(" IP={0},PV={1},UV={2} ", ipCount, pvCount, uvCount), string.Format(" LotteryID={0} ", int.Parse(lotteryId)));
            apiResp.msg = "操作完成";
            apiResp.status = true;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
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