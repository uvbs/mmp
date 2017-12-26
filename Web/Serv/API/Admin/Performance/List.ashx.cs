using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Performance
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            int yearMonth = Convert.ToInt32(context.Request["yearmonth"]);
            string up_member = context.Request["up_member"];
            string member = context.Request["member"];
            string sum = context.Request["sum"];
            string websiteOwner = bll.WebsiteOwner;
            string userIds = "";
            #region 查出userIds
            //List<string> mList = new List<string>();
            if (!string.IsNullOrWhiteSpace(member))
            {
                userIds = bllUser.GetSpreadUserIds(member, websiteOwner);
                //string members = bllUser.GetSpreadUserIds(member, websiteOwner);
                //mList = members.Split(',').ToList();
            }
            string parentIds = "";
            if (!string.IsNullOrWhiteSpace(up_member))
            {
                parentIds = bllUser.GetSpreadUserIds(up_member, websiteOwner);
            }
            //List<string> uList = new List<string>();
            //if (!string.IsNullOrWhiteSpace(up_member))
            //{
            //    string parentIds = bllUser.GetSpreadUserIds(up_member, websiteOwner);
            //    uList = parentIds.Split(',').ToList();
            //    //bool hasChild = true;
            //    //while (hasChild)
            //    //{
            //    //    List<UserInfo> nextList = bll.GetChildrenIdList(parentIds, websiteOwner, true, "AutoID,UserID");
            //    //    if (nextList.Count == 0) break;
            //    //    List<string> nList = nextList.Select(p => p.UserID).ToList();
            //    //    parentIds = ZentCloud.Common.MyStringHelper.ListToStr(nList, "", ",");
            //    //    uList.AddRange(nList);
            //    //}
            //}
            //if (!string.IsNullOrWhiteSpace(member) && !string.IsNullOrWhiteSpace(up_member))
            //{
            //    List<string> tList = uList.Intersect(mList).ToList();
            //    if (tList.Count > 0) {
            //        userIds = ZentCloud.Common.MyStringHelper.ListToStr(tList, "", ",");
            //    }
            //    else
            //    {
            //        userIds = "-1";
            //    }
            //}
            //else if (!string.IsNullOrWhiteSpace(up_member))
            //{
            //    if (uList.Count > 0)
            //    {
            //        userIds = ZentCloud.Common.MyStringHelper.ListToStr(uList, "", ",");
            //    }
            //    else
            //    {
            //        userIds = "-1";
            //    }
            //}
            //else if (!string.IsNullOrWhiteSpace(member))
            //{
            //    userIds = ZentCloud.Common.MyStringHelper.ListToStr(mList, "", ",");
            //}
            #endregion 查出userIds

            int total = bll.GetChildPerformanceCount(parentIds, websiteOwner, yearMonth, userIds);
            List<TeamPerformance> performanceList = new List<TeamPerformance>();
            if (total > 0) performanceList = bll.GetChildPerformanceList(rows, page, parentIds,
                websiteOwner, yearMonth, userIds, "AutoID,UserId,UserName,UserPhone,YearMonth,Performance,Reward,Status");
            decimal sumPerformance = 0;
            if (sum == "1") sumPerformance = bll.GetPerformanceSum(parentIds, websiteOwner, yearMonth, userIds);
            List<dynamic> rList = new List<dynamic>();
            if (performanceList.Count > 0)
            {
                string pUserIds = ZentCloud.Common.MyStringHelper.ListToStr( performanceList.Select(p=>p.UserId).Distinct().ToList(),"'",",");
                List<UserInfo> uList = bllUser.GetColMultListByKey<UserInfo>(int.MaxValue,1,"UserID",pUserIds,"AutoID,WXNickname,TrueName,Phone,UserID,IntelligenceCertificateBusiness",websiteOwner:websiteOwner);
                foreach (var item in performanceList)
	            {
                    UserInfo u = uList.FirstOrDefault(p=>p.UserID == item.UserId);
                    rList.Add(new{
                        id = item.AutoID,
                        name = u == null? item.UserName: bllUser.GetUserDispalyName(u),
                        phone = u == null? item.UserPhone: u.Phone,
                        business = u == null? " ": u.IntelligenceCertificateBusiness,
                        yearmonth = item.YearMonth,
                        performance = item.Performance,
                        reward = item.Reward,
                        status = item.Status
                    });
	            }
            }
            apiResp.msg = "获取业绩列表";
            apiResp.result = new
            {
                totalcount = total,
                sum = Convert.ToDouble(sumPerformance),
                list = rList 
            };
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
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