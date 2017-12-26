using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.BLLJIMP
{
    public class BLLDashboard : BLL
    {
        /// <summary>
        /// 获取Dashboard有实名用户列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<UserInfo> GetDashboardRegUserList(string websiteOwner, string startDate, string endDate)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" 1=1");
            List<string> memberStandardFieldList = new List<string>();
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            //sbWhere.AppendFormat(" AND (TrueName > '' Or WXNickname>'') ");
            sbWhere.AppendFormat(" AND TrueName > ''");
            sbWhere.AppendFormat(" AND Regtime >= '{0}' ", startDate);
            sbWhere.AppendFormat(" AND Regtime < '{0}' ", DateTime.Parse(endDate).AddDays(1).ToString("yyyy-MM-dd"));
            return Query<UserInfo>("SELECT AutoID,WebsiteOwner,Regtime FROM ZCJ_UserInfo WITH(NOLOCK) WHERE " + sbWhere.ToString());
            //return GetColList<UserInfo>(int.MaxValue, 1, sbWhere.ToString(), "AutoID,WebsiteOwner,Regtime");
        }
        /// <summary>
        /// 获取站点实名用户总数
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public int GetDashboardRegUserTotal(string websiteOwner)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            //sbWhere.AppendFormat(" AND (TrueName > '' Or WXNickname>'') ");
            sbWhere.AppendFormat(" AND TrueName > ''");
            return GetCount<UserInfo>(sbWhere.ToString());
        }
        /// <summary>
        /// 实名用户数
        /// </summary>
        /// <returns></returns>
        public List<TotalInfo> GetAllDashboardRegUserTotal()
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" TrueName > ''");
            List<UserInfo> list = Query<UserInfo>("SELECT WebsiteOwner,UserID FROM ZCJ_UserInfo WITH(NOLOCK) WHERE " + sbWhere.ToString());

            return list.GroupBy(p => new
            {
                p.WebsiteOwner
            }).Select(g => new TotalInfo
            {
                WebsiteOwner = g.Key.WebsiteOwner,
                Total = g.Count()
            }).ToList();
        }

        /// <summary>
        /// 获取Dashboard有效订单数
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public List<WXMallOrderInfo> GetDashboardOrderList(string websiteOwner, string startDate, string endDate, string orderType)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" 1=1");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            if (!string.IsNullOrWhiteSpace(orderType)) sbWhere.AppendFormat(" AND [OrderType] In ({0}) ", orderType);
            sbWhere.AppendFormat(" AND [Status]!='已取消' AND [Status]!='取消'");
            sbWhere.AppendFormat(" AND [InsertDate] >= '{0}' ", startDate);
            sbWhere.AppendFormat(" AND [InsertDate] < '{0}' ", DateTime.Parse(endDate).AddDays(1).ToString("yyyy-MM-dd"));
            return Query<WXMallOrderInfo>("SELECT OrderID,WebsiteOwner,InsertDate FROM ZCJ_WXMallOrderInfo WITH(NOLOCK) WHERE " + sbWhere.ToString());
            //return GetColList<WXMallOrderInfo>(int.MaxValue, 1, sbWhere.ToString(), "OrderID,WebsiteOwner,InsertDate");
        }

        /// <summary>
        /// 获取Dashboard有效订单总数
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public int GetDashboardOrderTotal(string websiteOwner, string orderType)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            if (!string.IsNullOrWhiteSpace(orderType)) sbWhere.AppendFormat(" AND [OrderType] In ({0}) ", orderType);
            sbWhere.AppendFormat(" AND [Status]!='已取消' AND [Status]!='取消'");
            return GetCount<WXMallOrderInfo>(sbWhere.ToString());
        }
        /// <summary>
        /// 获取Dashboard有效订单总数
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public List<TotalInfo> GetAllDashboardOrderTotal(string orderType)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" [OrderType] In ({0}) ", orderType);
            sbWhere.AppendFormat(" AND [Status]!='已取消' AND [Status]!='取消'");
            List<WXMallOrderInfo> list = Query<WXMallOrderInfo>("SELECT OrderID,WebsiteOwner FROM ZCJ_WXMallOrderInfo WITH(NOLOCK) WHERE " + sbWhere.ToString());
            return list.GroupBy(p => new
            {
                p.WebsiteOwner
            }).Select(g => new TotalInfo
            {
                WebsiteOwner = g.Key.WebsiteOwner,
                Total = g.Count()
            }).ToList();
        }
        /// <summary>
        /// 获取访问总数量
        /// </summary>
        /// <param name="websiteOwner">站点</param>
        /// <returns></returns>
        public int GetDashboardMonitorEventDetailsTotal(string websiteOwner)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            return GetCount<MonitorEventDetailsInfo>(sbWhere.ToString());
            //return GetColList<MonitorEventDetailsInfo>(int.MaxValue, 1, sbWhere.ToString(), "DetailID,WebsiteOwner,EventDate,EventUserID");
        }

        /// <summary>
        /// 获取访问总数量
        /// </summary>
        /// <returns></returns>
        public List<TotalInfo> GetAllDashboardMonitorEventDetailsTotal()
        {
            StringBuilder sbWhere = new StringBuilder();
            List<MonitorEventDetailsInfo> list = Query<MonitorEventDetailsInfo>("SELECT DetailID,WebsiteOwner FROM ZCJ_MonitorEventDetailsInfo WITH(NOLOCK)");
            return list.GroupBy(p => new
            {
                p.WebsiteOwner
            }).Select(g => new TotalInfo
            {
                WebsiteOwner = g.Key.WebsiteOwner,
                Total = g.Count()
            }).ToList();
        }
        /// <summary>
        /// 获取新增访问数量
        /// </summary>
        /// <param name="websiteOwner">站点</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<MonitorEventDetailsInfo> GetDashboardMonitorEventDetailsInfoList(string websiteOwner, string startDate, string endDate)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" 1=1");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            sbWhere.AppendFormat(" AND [EventDate] >= '{0}' ", startDate);
            sbWhere.AppendFormat(" AND [EventDate] < '{0}' ", DateTime.Parse(endDate).AddDays(1).ToString("yyyy-MM-dd"));
            return Query<MonitorEventDetailsInfo>("SELECT DetailID,WebsiteOwner,EventDate,EventUserID FROM ZCJ_MonitorEventDetailsInfo WITH(NOLOCK) WHERE " + sbWhere.ToString());
            //return GetColList<MonitorEventDetailsInfo>(int.MaxValue, 1, sbWhere.ToString(), "DetailID,WebsiteOwner,EventDate,EventUserID");
        }
        /// <summary>
        /// 获取访客数据
        /// </summary>
        /// <param name="websiteOwner">站点</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<MonitorEventDetailsInfo> GetDashboardUVList(string websiteOwner, string startDate, string endDate)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" 1=1");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            sbWhere.AppendFormat(" AND [EventDate] >= '{0}' ", startDate);
            sbWhere.AppendFormat(" AND [EventDate] < '{0}' ", DateTime.Parse(endDate).AddDays(1).ToString("yyyy-MM-dd"));
            sbWhere.AppendFormat(" AND EventUserID>''");
            return Query<MonitorEventDetailsInfo>("SELECT DetailID,WebsiteOwner,EventDate,EventUserID,SourceIP,IPLocation,EventBrowserID FROM ZCJ_MonitorEventDetailsInfo WITH(NOLOCK) WHERE " + sbWhere.ToString());
            //return GetColList<MonitorEventDetailsInfo>(int.MaxValue, 1, sbWhere.ToString(), "DetailID,WebsiteOwner,EventDate,EventUserID");
        }

        /// <summary>
        /// 获取访客总数
        /// </summary>
        /// <param name="websiteOwner">站点</param>
        /// <returns></returns>
        public int GetDashboardUVTotal(string websiteOwner)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            sbWhere.AppendFormat(" AND EventUserID>''");
            List<MonitorEventDetailsInfo> list = Query<MonitorEventDetailsInfo>("SELECT Distinct EventUserID FROM ZCJ_MonitorEventDetailsInfo WITH(NOLOCK) WHERE " + sbWhere.ToString());
            return list.Count;
            //return GetColList<MonitorEventDetailsInfo>(int.MaxValue, 1, sbWhere.ToString(), "DetailID,WebsiteOwner,EventDate,EventUserID");
        }
        /// <summary>
        /// 获取访客总数
        /// </summary>
        /// <returns></returns>
        public List<TotalInfo> GetAllDashboardUVTotal()
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" EventUserID>''");
            List<MonitorEventDetailsInfo> list = Query<MonitorEventDetailsInfo>("SELECT Distinct WebsiteOwner,EventUserID FROM ZCJ_MonitorEventDetailsInfo WITH(NOLOCK) WHERE " + sbWhere.ToString());

            return list.GroupBy(p => new
            {
                p.WebsiteOwner
            }).Select(g => new TotalInfo
            {
                WebsiteOwner = g.Key.WebsiteOwner,
                Total = g.Count()
            }).ToList();
        }
        /// <summary>
        /// 获取新增粉丝数量
        /// </summary>
        /// <param name="websiteOwner">站点</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<Log> GetDashboardSubscribeList(string websiteOwner, string startDate, string endDate)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" Module='Weixin' AND Action='Subscribe'");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            sbWhere.AppendFormat(" AND [InsertDate] >= '{0}' ", startDate);
            sbWhere.AppendFormat(" AND [InsertDate] < '{0}' ", DateTime.Parse(endDate).AddDays(1).ToString("yyyy-MM-dd"));
            sbWhere.AppendFormat(" AND UserID>''");
            return Query<Log>("SELECT AutoID,WebsiteOwner,InsertDate,UserID FROM ZCJ_Log WITH(NOLOCK) WHERE " + sbWhere.ToString());
            //return GetColList<Log>(int.MaxValue, 1, sbWhere.ToString(), "AutoID,WebsiteOwner,InsertDate,UserID");
        }

        /// <summary>
        /// 获取粉丝总数
        /// </summary>
        /// <param name="websiteOwner">站点</param>
        /// <returns></returns>
        public int GetDashboardSubscribeTotal(string websiteOwner)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            sbWhere.AppendFormat(" AND IsWeixinFollower = 1");
            return GetCount<UserInfo>(sbWhere.ToString());
        }
        /// <summary>
        /// 获取粉丝总数
        /// </summary>
        /// <param name="websiteOwner">站点</param>
        /// <returns></returns>
        public List<TotalInfo> GetAllDashboardSubscribeTotal()
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" IsWeixinFollower = 1");

            List<UserInfo> list = Query<UserInfo>("SELECT WebsiteOwner,UserID FROM ZCJ_UserInfo WITH(NOLOCK) WHERE " + sbWhere.ToString());

            return list.GroupBy(p => new
            {
                p.WebsiteOwner
            }).Select(g => new TotalInfo
            {
                WebsiteOwner = g.Key.WebsiteOwner,
                Total = g.Count()
            }).ToList();
        }

        /// <summary>
        /// 已有DashboardLog列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<DashboardLog> GetDashboardLogList(string websiteOwner, int startDate, int endDate)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" 1=1");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            sbWhere.AppendFormat(" AND [Date]>={0} AND [Date]<={1} ", startDate, endDate);
            return GetList<DashboardLog>(sbWhere.ToString());
        }
        /// <summary>
        /// 添加展示数据
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="date"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public bool AddDashboardInfo(string websiteOwner, int date, string json)
        {
            return Add(new DashboardInfo() { Date = date, WebsiteOwner = websiteOwner, Json = json });
        }
        /// <summary>
        /// 更新展示数据
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="date"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public bool UpdateDashboardInfo(string websiteOwner, int date, string json)
        {
            DashboardInfo nInfo = GetColByKey<DashboardInfo>("WebsiteOwner", websiteOwner, "WebsiteOwner");
            nInfo.Date = date;
            nInfo.Json = json;
            return Update(nInfo);
        }

        /// <summary>
        /// Dashboard展示数据列表
        /// </summary>
        /// <returns></returns>
        public List<DashboardInfo> GetDashboardInfoList()
        {
            return GetColList<DashboardInfo>(int.MaxValue, 1, "1=1", "WebsiteOwner,Date");
        }

        /// <summary>
        /// 生成Dashboard数据
        /// </summary>
        /// <param name="websiteOwnerList">站点列表</param>
        /// <param name="sDate">开始日期</param>
        /// <param name="eDate">结束日期</param>
        public void BuildDashboardLog(List<string> websiteList, DateTime sDate, DateTime eDate)
        {
            if (websiteList.Count == 0) return;

            List<UserInfo> userList = GetDashboardRegUserList(null, sDate.ToString("yyyy-MM-dd"), eDate.ToString("yyyy-MM-dd"));
            List<WXMallOrderInfo> orderList = GetDashboardOrderList(null, sDate.ToString("yyyy-MM-dd"), eDate.ToString("yyyy-MM-dd"), "0,1,2,3");
            List<MonitorEventDetailsInfo> webAccesslogList = GetDashboardMonitorEventDetailsInfoList(null, sDate.ToString("yyyy-MM-dd"), eDate.ToString("yyyy-MM-dd"));

            int sDateInt = DateTimeHelper.ToDateInt8ByDateTime(sDate);
            int eDateInt = ZentCloud.Common.DateTimeHelper.ToDateInt8ByDateTime(eDate);
            //历史记录
            List<DashboardLog> oDashlogList = GetDashboardLogList(null, sDateInt, eDateInt);

            #region 汇总记录
            List<DashboardLog> userDashboardLogList = userList.GroupBy(p => new
            {
                p.WebsiteOwner,
                Value = DateTimeHelper.ToDateInt8ByDateTime(p.Regtime.Value)
            }).Select(g => new DashboardLog
            {
                WebsiteOwner = g.Key.WebsiteOwner,
                Date = g.Key.Value,
                DashboardType = "Member",
                Num = g.Count()
            }).OrderByDescending(x => x.Date).ThenBy(x => x.WebsiteOwner).ToList();

            List<DashboardLog> orderDashboardLogList = orderList.GroupBy(p => new
            {
                p.WebsiteOwner,
                Value = DateTimeHelper.ToDateInt8ByDateTime(p.InsertDate)
            }).Select(g => new DashboardLog
            {
                WebsiteOwner = g.Key.WebsiteOwner,
                Date = g.Key.Value,
                DashboardType = "WXMallOrder",
                Num = g.Count()
            }).OrderByDescending(x => x.Date).ThenBy(x => x.WebsiteOwner).ToList();

            List<DashboardLog> webAccesslogDashboardLogList = webAccesslogList.GroupBy(p => new
            {
                p.WebsiteOwner,
                Value = DateTimeHelper.ToDateInt8ByDateTime(p.EventDate.Value)
            }).Select(g => new DashboardLog
            {
                WebsiteOwner = g.Key.WebsiteOwner,
                Date = g.Key.Value,
                DashboardType = "WebAccessLog",
                Num = g.Count()
            }).OrderByDescending(x => x.Date).ThenBy(x => x.WebsiteOwner).ToList();

            #endregion 汇总记录

            #region 更新记录
            for (DateTime i = eDate; i >= sDate; i = i.AddDays(-1))
            {
                int iInt = DateTimeHelper.ToDateInt8ByDateTime(i);
                foreach (string web in websiteList)
                {
                    DashboardLog nu = userDashboardLogList.FirstOrDefault(p => p.WebsiteOwner == web && p.Date == iInt);
                    DashboardLog no = orderDashboardLogList.FirstOrDefault(p => p.WebsiteOwner == web && p.Date == iInt);
                    DashboardLog nw = webAccesslogDashboardLogList.FirstOrDefault(p => p.WebsiteOwner == web && p.Date == iInt);

                    DashboardLog om = oDashlogList.FirstOrDefault(p => p.WebsiteOwner == web && p.Date == iInt && p.DashboardType == "Member");
                    DashboardLog oo = oDashlogList.FirstOrDefault(p => p.WebsiteOwner == web && p.Date == iInt && p.DashboardType == "WXMallOrder");
                    DashboardLog ow = oDashlogList.FirstOrDefault(p => p.WebsiteOwner == web && p.Date == iInt && p.DashboardType == "WebAccessLog");

                    if (om == null && nu != null)
                    {
                        Add(nu);
                    }
                    else if (om != null && nu != null)
                    {
                        Update(nu);
                    }
                    else if (om != null && nu == null)
                    {
                        Delete(om);
                    }

                    if (oo == null && no != null)
                    {
                        Add(no);
                    }
                    else if (oo != null && no != null)
                    {
                        Update(no);
                    }
                    else if (oo != null && no == null)
                    {
                        Delete(oo);
                    }

                    if (ow == null && nw != null)
                    {
                        Add(nw);
                    }
                    else if (ow != null && nw != null)
                    {
                        Update(nw);
                    }
                    else if (ow != null && nw == null)
                    {
                        Delete(ow);
                    }
                }
            }
            #endregion 更新记录
        }

        /// <summary>
        /// 生成首页数据
        /// </summary>
        /// <param name="websiteOwnerList">站点列表</param>
        /// <param name="date">日期</param>
        public void BuildDashboardInfo(List<string> websiteList, DateTime nDate)
        {
            if (websiteList.Count == 0) return;

            int nDateInt = DateTimeHelper.ToDateInt8ByDateTime(nDate);
            int lastDate7Int = DateTimeHelper.ToDateInt8ByDateTime(nDate.AddDays(-6));
            int lastDate30Int = DateTimeHelper.ToDateInt8ByDateTime(nDate.AddDays(-29));
            List<DashboardLog> dashLogList = GetDashboardLogList(null, lastDate30Int, nDateInt);
            List<DashboardInfo> dashInfoList = GetDashboardInfoList();

            List<DashboardLog> uvDashLogList = new List<DashboardLog>();
            #region 访客记录统计

            List<MonitorEventDetailsInfo> uvList = GetDashboardUVList(null, nDate.AddDays(-29).ToString("yyyy-MM-dd"), nDate.ToString("yyyy-MM-dd"));
            List<DashboardMonitorInfo> uvDashboardLogList = GetColList<DashboardMonitorInfo>(int.MaxValue, 1, "1=1", "DetailID");
            //ExecuteSql("truncate table ZCJ_DashboardMonitorInfo");//清除原30天UV记录
            if (uvList.Count > 0)
            {
                List<DashboardMonitorInfo> uvGroupList = uvList.GroupBy(p => new
                {
                    p.WebsiteOwner,
                    p.EventUserID
                }).Select(g => new DashboardMonitorInfo
                {
                    WebsiteOwner = g.Key.WebsiteOwner,
                    EventUserID = g.Key.EventUserID,
                    DetailID = g.Max(p => p.DetailID).Value
                }).OrderByDescending(x => x.DetailID).ToList();

                //删除数据
                List<int> delIdList = uvDashboardLogList.Where(p => !uvGroupList.Exists(pi => pi.DetailID == p.DetailID)).Select(pid => pid.DetailID).ToList();
                if (delIdList.Count > 0)
                {
                    DeleteMultByKey<DashboardMonitorInfo>("DetailID", ZentCloud.Common.MyStringHelper.ListToStr(delIdList, "", ","));
                }

                List<int> addIdList = uvGroupList.Where(p => !uvDashboardLogList.Exists(pi => pi.DetailID == p.DetailID)).Select(pid => pid.DetailID).ToList();

                List<DashboardMonitorInfo> uvAddDashboardList = uvList.Where(p => addIdList.Exists(pi => pi == p.DetailID.Value)).Select(g => new DashboardMonitorInfo
                {
                    DetailID = g.DetailID.Value,
                    WebsiteOwner = g.WebsiteOwner,
                    EventUserID = g.EventUserID,
                    EventDate = g.EventDate.Value,
                    SourceIP = g.SourceIP,
                    IPLocation = g.IPLocation,
                    EventBrowserID = g.EventBrowserID
                }).ToList();

                if (uvAddDashboardList.Count > 0)
                {
                    string userIds = MyStringHelper.ListToStr(uvAddDashboardList.Select(p => p.EventUserID).Distinct().ToList(), "'", ",");
                    List<UserInfo> userList = GetColMultListByKey<UserInfo>(int.MaxValue, 1, "UserID", userIds, "AutoID,UserID,TrueName,Phone,WXNickname,WXHeadimgurl");
                    for (int i = 0; i < uvAddDashboardList.Count; i++)
                    {
                        UserInfo nuser = userList.FirstOrDefault(p => p.UserID == uvAddDashboardList[i].EventUserID);
                        if (nuser != null)
                        {
                            uvAddDashboardList[i].EventUserWXNikeName = nuser.WXNickname;
                            uvAddDashboardList[i].EventUserTrueName = nuser.TrueName;
                            uvAddDashboardList[i].EventUserWXImg = nuser.WXHeadimgurl;
                            uvAddDashboardList[i].EventUserPhone = nuser.Phone;
                        }
                        Add(uvAddDashboardList[i]);
                    }

                }

                uvDashLogList = uvList.Where(ni => uvGroupList.Exists(pi => pi.DetailID == ni.DetailID)).GroupBy(p => new
                {
                    p.WebsiteOwner,
                    Value = DateTimeHelper.ToDateInt8ByDateTime(p.EventDate.Value)
                }).Select(g => new DashboardLog
                {
                    WebsiteOwner = g.Key.WebsiteOwner,
                    Date = g.Key.Value,
                    DashboardType = "UV",
                    Num = g.Count()
                }).OrderByDescending(x => x.Date).ThenBy(x => x.WebsiteOwner).ToList();
            }
            #endregion

            List<DashboardLog> fansDashLogList = new List<DashboardLog>();
            #region 粉丝记录统计
            List<Log> fansList = GetDashboardSubscribeList(null, nDate.AddDays(-29).ToString("yyyy-MM-dd"), nDate.ToString("yyyy-MM-dd"));
            if (fansList.Count > 0)
            {
                fansDashLogList = fansList.GroupBy(p => new
                {
                    p.WebsiteOwner,
                    p.UserID
                }).Select(g => new Log
                {
                    WebsiteOwner = g.Key.WebsiteOwner,
                    UserID = g.Key.UserID,
                    InsertDate = g.Max(p => p.InsertDate)
                }).GroupBy(e => new
                {
                    e.WebsiteOwner,
                    Value = DateTimeHelper.ToDateInt8ByDateTime(e.InsertDate)
                }).Select(f => new DashboardLog
                {
                    WebsiteOwner = f.Key.WebsiteOwner,
                    Date = f.Key.Value,
                    DashboardType = "Fans",
                    Num = f.Count()
                }).OrderByDescending(x => x.Date).ThenBy(x => x.WebsiteOwner).ToList();
            }
            #endregion

            List<TotalInfo> memberTotalList = GetAllDashboardRegUserTotal();
            List<TotalInfo> uvTotalList = GetAllDashboardUVTotal();
            List<TotalInfo> fansTotalList = GetAllDashboardSubscribeTotal();
            List<TotalInfo> orderTotalList = GetAllDashboardOrderTotal("0,1,2,3");
            List<TotalInfo> visitTotalList = GetAllDashboardMonitorEventDetailsTotal();

            foreach (string web in websiteList)
            {
                DashboardInfo ndi = dashInfoList.FirstOrDefault(p => p.WebsiteOwner == web);
                //if (ndi!=null && ndi.Date == nDateInt) continue;
                DashboardJson nDashboardJson = new DashboardJson();
                nDashboardJson.visit_num_lastday = dashLogList.Where(p => p.WebsiteOwner == web && p.DashboardType == "WebAccessLog" && p.Date == nDateInt).Sum(p => p.Num);
                nDashboardJson.order_num_lastday = dashLogList.Where(p => p.WebsiteOwner == web && p.DashboardType == "WXMallOrder" && p.Date == nDateInt).Sum(p => p.Num);
                nDashboardJson.member_num_lastday = dashLogList.Where(p => p.WebsiteOwner == web && p.DashboardType == "Member" && p.Date == nDateInt).Sum(p => p.Num);
                nDashboardJson.uv_num_lastday = uvDashLogList.Where(p => p.WebsiteOwner == web && p.Date == nDateInt).Sum(p => p.Num);
                nDashboardJson.fans_num_lastday = fansDashLogList.Where(p => p.WebsiteOwner == web && p.Date == nDateInt).Sum(p => p.Num);

                nDashboardJson.visit_num_lastweek = dashLogList.Where(p => p.WebsiteOwner == web && p.DashboardType == "WebAccessLog" && p.Date >= lastDate7Int && p.Date <= nDateInt).Sum(p => p.Num);
                nDashboardJson.order_num_lastweek = dashLogList.Where(p => p.WebsiteOwner == web && p.DashboardType == "WXMallOrder" && p.Date >= lastDate7Int && p.Date <= nDateInt).Sum(p => p.Num);
                nDashboardJson.member_num_lastweek = dashLogList.Where(p => p.WebsiteOwner == web && p.DashboardType == "Member" && p.Date >= lastDate7Int && p.Date <= nDateInt).Sum(p => p.Num);
                nDashboardJson.uv_num_lastweek = uvDashLogList.Where(p => p.WebsiteOwner == web && p.Date >= lastDate7Int && p.Date <= nDateInt).Sum(p => p.Num);
                nDashboardJson.fans_num_lastweek = fansDashLogList.Where(p => p.WebsiteOwner == web && p.Date >= lastDate7Int && p.Date <= nDateInt).Sum(p => p.Num);

                nDashboardJson.visit_num_lastmonth = dashLogList.Where(p => p.WebsiteOwner == web && p.DashboardType == "WebAccessLog" && p.Date >= lastDate30Int && p.Date <= nDateInt).Sum(p => p.Num);
                nDashboardJson.order_num_lastmonth = dashLogList.Where(p => p.WebsiteOwner == web && p.DashboardType == "WXMallOrder" && p.Date >= lastDate30Int && p.Date <= nDateInt).Sum(p => p.Num);
                nDashboardJson.member_num_lastmonth = dashLogList.Where(p => p.WebsiteOwner == web && p.DashboardType == "Member" && p.Date >= lastDate30Int && p.Date <= nDateInt).Sum(p => p.Num);
                nDashboardJson.uv_num_lastmonth = uvDashLogList.Where(p => p.WebsiteOwner == web && p.Date >= lastDate30Int && p.Date <= nDateInt).Sum(p => p.Num);
                nDashboardJson.fans_num_lastmonth = fansDashLogList.Where(p => p.WebsiteOwner == web && p.Date >= lastDate30Int && p.Date <= nDateInt).Sum(p => p.Num);

                TotalInfo memberTotal = memberTotalList.FirstOrDefault(p => p.WebsiteOwner == web);
                if (memberTotal != null) nDashboardJson.member_total = memberTotal.Total;

                TotalInfo uvTotal = uvTotalList.FirstOrDefault(p => p.WebsiteOwner == web);
                if (uvTotal != null) nDashboardJson.uv_total = uvTotal.Total;

                TotalInfo fansTotal = fansTotalList.FirstOrDefault(p => p.WebsiteOwner == web);
                if (fansTotal != null) nDashboardJson.fans_total = fansTotal.Total;

                TotalInfo orderTotal = orderTotalList.FirstOrDefault(p => p.WebsiteOwner == web);
                if (orderTotal != null) nDashboardJson.order_total = orderTotal.Total;

                TotalInfo visitTotal = visitTotalList.FirstOrDefault(p => p.WebsiteOwner == web);
                if (visitTotal != null) nDashboardJson.visit_total = visitTotal.Total;

                for (DateTime i = nDate; i >= nDate.AddDays(-29); i = i.AddDays(-1))
                {
                    int rDateInt = ZentCloud.Common.DateTimeHelper.ToDateInt8ByDateTime(i);
                    string rDateString = i.ToString("yyyy-MM-dd");
                    nDashboardJson.day_list.Add(rDateString);
                    DashboardLog rVisitLog = dashLogList.FirstOrDefault(p => p.WebsiteOwner == web && p.DashboardType == "WebAccessLog" && p.Date == rDateInt);
                    if (rVisitLog == null)
                    {
                        nDashboardJson.visit_num_list.Add(0);
                    }
                    else
                    {
                        nDashboardJson.visit_num_list.Add(rVisitLog.Num);
                    }
                    DashboardLog rOrderLog = dashLogList.FirstOrDefault(p => p.WebsiteOwner == web && p.DashboardType == "WXMallOrder" && p.Date == rDateInt);
                    if (rOrderLog == null)
                    {
                        nDashboardJson.order_num_list.Add(0);
                    }
                    else
                    {
                        nDashboardJson.order_num_list.Add(rOrderLog.Num);
                    }
                    DashboardLog rMemberLog = dashLogList.FirstOrDefault(p => p.WebsiteOwner == web && p.DashboardType == "Member" && p.Date == rDateInt);
                    if (rMemberLog == null)
                    {
                        nDashboardJson.member_num_list.Add(0);
                    }
                    else
                    {
                        nDashboardJson.member_num_list.Add(rMemberLog.Num);
                    }

                    DashboardLog rUVLog = uvDashLogList.FirstOrDefault(p => p.WebsiteOwner == web && p.Date == rDateInt);
                    if (rUVLog == null)
                    {
                        nDashboardJson.uv_num_list.Add(0);
                    }
                    else
                    {
                        nDashboardJson.uv_num_list.Add(rUVLog.Num);
                    }

                    DashboardLog rFansLog = fansDashLogList.FirstOrDefault(p => p.WebsiteOwner == web && p.Date == rDateInt);
                    if (rFansLog == null)
                    {
                        nDashboardJson.fans_num_list.Add(0);
                    }
                    else
                    {
                        nDashboardJson.fans_num_list.Add(rFansLog.Num);
                    }
                }
                nDashboardJson.timestamp = DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now);

                string nJson = JsonConvert.SerializeObject(nDashboardJson);
                if (ndi == null)
                {
                    Add(new DashboardInfo() { WebsiteOwner = web, Date = nDateInt, Json = nJson });
                }
                else
                {
                    Update(new DashboardInfo() { WebsiteOwner = web, Date = nDateInt, Json = nJson });
                }
            }
        }

        /// <summary>
        /// 获取访客列表
        /// </summary>
        /// <returns></returns>
        public List<DashboardMonitorInfo> GetDashboardMonitorInfoList(int pageSize,int pageIndex,string date,out int totalCount)
        {
            return GetDashboardMonitorInfoList(pageSize, pageIndex, date, out  totalCount, WebsiteOwner);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="date"></param>
        /// <param name="totalCount"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<DashboardMonitorInfo> GetDashboardMonitorInfoList(int pageSize, int pageIndex, string date, out int totalCount, string websiteOwner,string userAutoId="")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            string nowDay = DateTime.Now.ToString("yyyy-MM-dd");
            switch (date)
            {
                case "day":
                    sbWhere.AppendFormat(" AND EventDate>=DateAdd(dd,-1,'{0}') AND EventDate<'{0}'", nowDay);
                    break;
                case "week":
                    sbWhere.AppendFormat(" AND EventDate>=DateAdd(dd,-7,'{0}') AND EventDate<'{0}' ", nowDay);
                    break;
                case "month":
                    sbWhere.AppendFormat(" AND EventDate>=DateAdd(dd,-30,'{0}') AND EventDate<'{0}' ", nowDay);
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(userAutoId))
            {
                sbWhere.AppendFormat(" And EventUserID in(Select UserId from ZCJ_UserInfo where AutoId={0})",userAutoId);
            }
            totalCount = GetCount<DashboardMonitorInfo>(sbWhere.ToString());
            return GetLit<DashboardMonitorInfo>(pageSize, pageIndex, sbWhere.ToString(), " DetailID DESC ");
        }
        [Serializable]
        public class TotalInfo : ZentCloud.ZCBLLEngine.ModelTable
        {
            public string WebsiteOwner { get; set; }
            public int Total { get; set; }
        }
    }
}
