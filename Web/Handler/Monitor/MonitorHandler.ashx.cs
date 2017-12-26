using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;
using System.Data;
using ZentCloud.Common;


namespace ZentCloud.JubitIMP.Web.Handler.Monitor
{
    /// <summary>
    /// 监测
    /// </summary>
    public class MonitorHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLMonitor bll = new BLLMonitor();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 
        /// </summary>
        BLLDashboard bllDashboard = new BLLDashboard();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        /// <summary>
        /// 当前站点所有者
        /// </summary>
        //ZentCloud.BLLJIMP.Model.UserInfo currWebSiteUserInfo;
        /// <summary>
        /// 
        /// </summary>
        BLLMonitor bllMonitor = new BLLMonitor();
        public void ProcessRequest(HttpContext context)
        {
            string result = "false";
            try
            {
                currentUserInfo = bll.GetCurrentUserInfo();
                context.Response.ContentType = "text/plain";
                context.Response.Expires = 0;
                string action = context.Request["Action"];
                //this.currWebSiteUserInfo = bllUser.GetUserInfo(bll.WebsiteOwner);
                switch (action)
                {
                    //监测任务
                    case "AddPlan":
                        result = AddPlan(context);//添加监测任务
                        break;
                    case "EditPlan":
                        result = EditPlan(context);//编辑监测任务
                        break;
                    case "DeletePlan":
                        result = DeletePlan(context);//删除任务
                        break;
                    case "QueryPlan"://查询监测任务
                        result = QueryPlan(context);
                        break;

                    case "BatChangPlanStatus"://设置任务状态
                        result = BatChangPlanStatus(context);
                        break;

                    //监测任务

                    //链接----
                    case "AddLink":
                        result = AddLink(context);//添加链接
                        break;
                    case "EditLink":
                        result = EditLink(context);//编辑链接
                        break;
                    case "DeleteLink":
                        result = DeleteLink(context);//删除链接
                        break;

                    case "QueryLink"://查询链接
                        result = QueryLink(context);
                        break;
                    case "QueryLink2"://查询链接
                        result = QueryLink2(context);
                        break;

                    case "QueryPlanEventDetail"://查询事件详情
                        result = QueryPlanEventDetail(context);
                        break;
                    case "QueryPlanEventDetailByProduct":
                        result = QueryPlanEventDetailByProduct(context);
                        break;
                    case "QueryPlanEventDetailByPV":
                        result = QueryPlanEventDetailByPV(context);
                        break;
                    case "QueryPlanEventDetailByUV":
                        result = QueryPlanEventDetailByUV(context);
                        break;
                    //链接

                    //微信推广
                    case "AddWeixinSpread":
                        result = AddWeixinSpread(context);//添加微信推广
                        break;
                    case "EditWeixinSpread":
                        result = EditWeixinSpread(context);//编辑微信推广
                        break;
                    case "DeleteWeixinSpread":
                        result = DeleteWeixinSpread(context);//删除微信推广
                        break;
                    case "QueryWeixinSpread"://查询微信推广
                        result = QueryWeixinSpread(context);
                        break;
                    case "BatChangWeixinSpreadState":
                        result = BatChangWeixinSpreadState(context);//修改微信推广状态
                        break;

                    //微信推广






                }

            }
            catch (Exception ex)
            {

                result = ex.Message;
            }
            finally
            {

                context.Response.Write(result);
            }




        }



        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeletePlan(HttpContext context)
        {

            string ids = context.Request["ids"];

            //TODO:删除菜单前，清除相关权限菜单关联

            //由物理删除改为标识删除 -- 2013.11.13

            //int result = bll.Delete(new MonitorPlan(), string.Format(" MonitorPlanID in ({0})", ids));//pmsBll.DeleteUser(idsList);

            int result = bll.Update(new MonitorPlan(), " IsDelete = 1 ", string.Format(" MonitorPlanID in ({0})", ids));

            return result.ToString();
        }



        /// <summary>
        /// 设置任务状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string BatChangPlanStatus(HttpContext context)
        {

            string ids = context.Request["ids"];
            string status = context.Request["status"];
            int count = bll.Update(new MonitorPlan(), string.Format("PlanStatus={0}", status), string.Format(" MonitorPlanID in ({0})", ids));
            if (ids.Split(',').Length == count)
            {
                return "true";
            }
            else
            {
                return "false";
            }


        }


        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddPlan(HttpContext context)
        {


            string jsonData = context.Request["JsonData"];
            MonitorPlan model = ZentCloud.Common.JSONHelper.JsonToModel<MonitorPlan>(jsonData);
            model.MonitorPlanID = int.Parse(bll.GetGUID(ZentCloud.BLLJIMP.TransacType.MonitorPlanID));
            model.UserID = bll.WebsiteOwner;
            model.InsertDate = DateTime.Now;
            bool result = bll.Add(model);
            return result.ToString().ToLower();
        }

        /// <summary>
        /// 编辑任务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditPlan(HttpContext context)
        {

            string jsonData = context.Request["JsonData"];
            MonitorPlan model = ZentCloud.Common.JSONHelper.JsonToModel<MonitorPlan>(jsonData);
            int count = bll.Update(model, string.Format("PlanName='{0}',PlanStatus='{1}',Remark='{2}'", model.PlanName, model.PlanStatus, model.Remark), string.Format("MonitorPlanID='{0}'", model.MonitorPlanID));
            return count > 0 ? "true" : "false";
        }


        /// <summary>
        /// 查询监测任务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryPlan(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string planName = context.Request["PlanName"];
            string planId = context.Request["PlanID"];
            //string feedbackType = context.Request["FeedbackType"];
            //string feedbackPlatformCategory = context.Request["FeedbackPlatformCategory"];
            //string targetUser = context.Request["TargetUser"];
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" IsDelete = 0 ");

            if (currentUserInfo.UserType != 1)
            {
                sbWhere.AppendFormat(string.Format(" And UserID='{0}'", bll.WebsiteOwner));
            }
            if (!string.IsNullOrEmpty(planName))
            {
                sbWhere.AppendFormat(" And PlanName like '%{0}%'", planName);
            }
            if (!string.IsNullOrEmpty(planId))
            {
                sbWhere.AppendFormat(" And MonitorPlanID ={0}", planId);
            }

            //if (!string.IsNullOrEmpty(feedBackStatus))
            //{
            //    sbCondtion.AppendFormat(" And FeedBackStatus ='{0}'", feedBackStatus);
            //}
            //if (!string.IsNullOrEmpty(feedbackType))
            //{
            //    sbCondtion.AppendFormat(" And FeedBackType ='{0}'", feedbackType);
            //}
            //if (!string.IsNullOrEmpty(feedbackPlatformCategory))
            //{
            //    sbCondtion.AppendFormat(" And PlatformCategory ='{0}'", feedbackPlatformCategory);
            //}
            //if (!string.IsNullOrEmpty(targetUser))
            //{
            //    if (targetUser.Equals("0"))
            //    {
            //        sbCondtion.AppendFormat(" And UserID ='{0}'", useinfo.UserID);
            //    }
            //    else if (targetUser.Equals("1"))
            //    {
            //        sbCondtion.AppendFormat(" And AssignationUserID ='{0}'", useinfo.UserID);
            //    }

            //}


            //sbCondtion.AppendFormat(" And (UserID='{0}' or AssignationUserID='{0}')", useinfo.UserID);


            List<ZentCloud.BLLJIMP.Model.MonitorPlan> list = bll.GetLit<ZentCloud.BLLJIMP.Model.MonitorPlan>(pageSize, pageIndex, sbWhere.ToString(), "MonitorPlanID DESC");
            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.MonitorPlan>(sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = list
    });
        }





        /// <summary>
        /// 查询 任务下的链接
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryLink(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string planId = context.Request["PlanId"];
            string linkName = context.Request["LinkName"];

            //string title = context.Request["SearchTitle"];
            //string feedBackStatus = context.Request["FeedBackStatus"];
            //string feedbackType = context.Request["FeedbackType"];
            //string feedbackPlatformCategory = context.Request["FeedbackPlatformCategory"];
            //string targetUser = context.Request["TargetUser"];
            StringBuilder sbWhere = new StringBuilder(string.Format(" MonitorPlanID='{0}'", planId));

            if (!string.IsNullOrEmpty(linkName))
            {
                sbWhere.AppendFormat(" And LinkName like '%{0}%'", linkName);
            }
            //if (!string.IsNullOrEmpty(feedBackStatus))
            //{
            //    sbCondtion.AppendFormat(" And FeedBackStatus ='{0}'", feedBackStatus);
            //}
            //if (!string.IsNullOrEmpty(feedbackType))
            //{
            //    sbCondtion.AppendFormat(" And FeedBackType ='{0}'", feedbackType);
            //}
            //if (!string.IsNullOrEmpty(feedbackPlatformCategory))
            //{
            //    sbCondtion.AppendFormat(" And PlatformCategory ='{0}'", feedbackPlatformCategory);
            //}
            //if (!string.IsNullOrEmpty(targetUser))
            //{
            //    if (targetUser.Equals("0"))
            //    {
            //        sbCondtion.AppendFormat(" And UserID ='{0}'", useinfo.UserID);
            //    }
            //    else if (targetUser.Equals("1"))
            //    {
            //        sbCondtion.AppendFormat(" And AssignationUserID ='{0}'", useinfo.UserID);
            //    }

            //}


            //sbCondtion.AppendFormat(" And (UserID='{0}' or AssignationUserID='{0}')", useinfo.UserID);

            List<ZentCloud.BLLJIMP.Model.MonitorLinkInfo> list = bll.GetLit<ZentCloud.BLLJIMP.Model.MonitorLinkInfo>(pageSize, pageIndex, sbWhere.ToString(), "LinkID DESC");

            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.MonitorLinkInfo>(sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
     new
     {
         total = totalCount,
         rows = list
     });
        }

        /// <summary>
        /// 查询 任务下的链接
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryLink2(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string planId = context.Request["PlanId"];
            string linkName = context.Request["LinkName"];

            StringBuilder sbWhere = new StringBuilder(string.Format(" MonitorPlanID='{0}'", planId));

            if (!string.IsNullOrEmpty(linkName))
            {
                sbWhere.AppendFormat(" And LinkName like '%{0}%'", linkName);
            }

            //List<ZentCloud.BLLJIMP.Model.MonitorLinkInfo> list = bll.GetLit<ZentCloud.BLLJIMP.Model.MonitorLinkInfo>(rows, page, sbCondtion.ToString(), "LinkID DESC");
            int totalCount = 0;
            DataTable dt = this.bll.QueryMonitorPlanSpreadRank(int.Parse(planId), "ip", pageIndex, pageSize, out totalCount);
            string jsonResult = ZentCloud.Common.JSONHelper.DataTableToEasyUIJson(totalCount, dt);
            return jsonResult;
        }


        /// <summary>
        /// 删除链接
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteLink(HttpContext context)
        {

            string ids = context.Request["ids"];

            //TODO:删除菜单前，清除相关权限菜单关联

            int result = bll.Delete(new MonitorLinkInfo(), string.Format(" LinkID in ({0})", ids));//pmsBll.DeleteUser(idsList);
            return result.ToString();
        }

        /// <summary>
        /// 添加链接
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddLink(HttpContext context)
        {


            string jsonData = context.Request["JsonData"];
            MonitorLinkInfo model = ZentCloud.Common.JSONHelper.JsonToModel<MonitorLinkInfo>(jsonData);
            //相关检查


            if (!string.IsNullOrEmpty(model.RealLink))
            {
                if (!Common.PageValidate.IsUrl(model.RealLink))
                {
                    return "请输入正确的链接地址,格式如 http://www.baidu.com";
                }

            }
            //相关检查
            model.LinkID = int.Parse(bll.GetGUID(ZentCloud.BLLJIMP.TransacType.MonitorLinkID));
            model.InsertDate = DateTime.Now;
            model.EncryptParameter = Common.Base64Change.EncodeBase64ByUTF8(string.Format("{0}{1}{2}", model.MonitorPlanID, model.LinkID, model.RealLink));
            return bll.Add(model).ToString().ToLower();



        }

        /// <summary>
        /// 编辑链接
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditLink(HttpContext context)
        {

            string jsonData = context.Request["JsonData"];
            MonitorLinkInfo model = ZentCloud.Common.JSONHelper.JsonToModel<MonitorLinkInfo>(jsonData);
            //相关检查


            if (!string.IsNullOrEmpty(model.RealLink))
            {
                if (!Common.PageValidate.IsUrl(model.RealLink))
                {
                    return "请输入正确的链接地址,格式如 http://www.baidu.com";
                }

            }
            //相关检查

            int count = bll.Update(model, string.Format("RealLink='{0}',LinkName='{1}'", model.RealLink, model.LinkName), string.Format("LinkID='{0}'", model.LinkID));
            return count > 0 ? "true" : "false";
        }

        /// <summary>
        /// 查询 任务下的事件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryPlanEventDetail(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            if (pageSize==0)
            {
                pageSize = 10000;
            }
            if (pageIndex==0)
            {
                pageIndex = 1;
            }
            string planId = context.Request["planId"];
            string linkId = context.Request["linkId"];
            string uv = context.Request["uv"];
            string spreadUserId = context.Request["spreadUserID"];
            string share = context.Request["share"];
            //string title = context.Request["SearchTitle"];
            //string feedBackStatus = context.Request["FeedBackStatus"];
            //string feedbackType = context.Request["FeedbackType"];
            //string feedbackPlatformCategory = context.Request["FeedbackPlatformCategory"];
            //string targetUser = context.Request["TargetUser"];


            StringBuilder sbWhere = new StringBuilder(string.Format(" MonitorPlanID='{0}'", planId));
            sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", bll.WebsiteOwner);
            #region 据jw说，现在不考虑EventType，暂时注释掉，后面再考证
            //if (bll.GetCount<MonitorEventDetailsInfo>(string.Format("LinkID={0} and EventType=0", linkId)) > 0)
            //{
            //    sbCondtion.AppendFormat(" And EventType =0", linkId);
            //}
            //else
            //{
            //    sbCondtion.AppendFormat(" And EventType =1", linkId);
            //} 
            #endregion

            if (!string.IsNullOrEmpty(linkId))
            {
                sbWhere.AppendFormat(" And LinkID = '{0}'", linkId);
            }
            if (!string.IsNullOrEmpty(spreadUserId))
            {
                sbWhere.AppendFormat(" And spreadUserID = '{0}'", spreadUserId);
            }
            if (!string.IsNullOrEmpty(share))
            {
                sbWhere.AppendFormat(" And ShareTimestamp = '{0}'", share);
            }
            List<ZentCloud.BLLJIMP.Model.MonitorEventDetailsInfo> list = new List<ZentCloud.BLLJIMP.Model.MonitorEventDetailsInfo>();
            int totalCount = 0;
            List<dynamic> returnList = new List<dynamic>();
            if (!string.IsNullOrEmpty(uv))
            {
                StringBuilder sqlWhere = new StringBuilder();
                if (!string.IsNullOrEmpty(spreadUserId))
                {
                    sqlWhere.AppendFormat(@"   SELECT top 10000 max(b.[WXHeadimgurl]) EventUserWXImg, max(b.[WXNickname]) EventUserWXNikeName , max(b.[TrueName]) EventUserTrueName , max(b.[Phone]) EventUserPhone, max(a.SourceIP) SourceIP,max(a.IPLocation) IPLocation,max(a.EventDate) EventDate,max(a.EventBrowserID) EventBrowserID,max(a.EventSysPlatform) EventSysPlatform,max(a.EventUserID) EventUserID,MAX(a.SpreadUserID) spreadUserID
                    FROM ZCJ_MonitorEventDetailsInfo a LEFT JOIN ZCJ_UserInfo b ON a.EventUserID = b.UserID where a.EventUserID is not null and a.MonitorPlanID = {0} AND a.spreadUserId='{1}' group by a.EventUserID ORDER BY max(a.EventDate) desc", planId, spreadUserId);
                }
                else
                {
                    if (!string.IsNullOrEmpty(share))
                    {
                        sqlWhere.AppendFormat(@"   SELECT top 10000 max(b.[WXHeadimgurl]) EventUserWXImg, max(b.[WXNickname]) EventUserWXNikeName , max(b.[TrueName]) EventUserTrueName , max(b.[Phone]) EventUserPhone, max(a.SourceIP) SourceIP,max(a.IPLocation) IPLocation,max(a.EventDate) EventDate,max(a.EventBrowserID) EventBrowserID,max(a.EventSysPlatform) EventSysPlatform,max(a.EventUserID) EventUserID,MAX(a.SpreadUserID) spreadUserID
                    FROM ZCJ_MonitorEventDetailsInfo a LEFT JOIN ZCJ_UserInfo b ON a.EventUserID = b.UserID where a.EventUserID is not null and a.MonitorPlanID = {0} AND ShareTimestamp='{1}' group by a.EventUserID ORDER BY max(a.EventDate) desc", planId,share);
                    }
                    else
                    {
                        sqlWhere.AppendFormat(@"   SELECT top 10000 max(b.[WXHeadimgurl]) EventUserWXImg, max(b.[WXNickname]) EventUserWXNikeName , max(b.[TrueName]) EventUserTrueName , max(b.[Phone]) EventUserPhone, max(a.SourceIP) SourceIP,max(a.IPLocation) IPLocation,max(a.EventDate) EventDate,max(a.EventBrowserID) EventBrowserID,max(a.EventSysPlatform) EventSysPlatform,max(a.EventUserID) EventUserID,MAX(a.SpreadUserID) spreadUserID
                    FROM ZCJ_MonitorEventDetailsInfo a LEFT JOIN ZCJ_UserInfo b ON a.EventUserID = b.UserID where a.EventUserID is not null and a.MonitorPlanID = {0} group by a.EventUserID ORDER BY max(a.EventDate) desc", planId);
                    }
                    
                }
                List<MonitorEventDetailsInfo> eventDetails= ZentCloud.ZCBLLEngine.BLLBase.Query<MonitorEventDetailsInfo>(sqlWhere.ToString());
                return Common.JSONHelper.ObjectToJson(
                new
                {
                    total = eventDetails.Count,
                    rows = eventDetails
                });
            }
            else
            {

                list = bll.GetLit<ZentCloud.BLLJIMP.Model.MonitorEventDetailsInfo>(pageSize, pageIndex, sbWhere.ToString(), "DetailID DESC");
                totalCount = bll.GetCount<MonitorEventDetailsInfo>(sbWhere.ToString());


            }
            foreach (var item in list)
            {
                UserInfo userModel = bllUser.GetUserInfo(item.EventUserID);
                if (userModel != null)
                {
                    item.EventUserWXNikeName = userModel.WXNickname;
                    item.EventUserWXImg = userModel.WXHeadimgurl;
                    item.EventUserTrueName = userModel.TrueName;
                    item.EventUserPhone = userModel.Phone;
                }
            }
            return Common.JSONHelper.ObjectToJson(
                  new
                  {
                      total = totalCount,
                      rows = list
                  });

        }
        /// <summary>
        /// 商品监测明细
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryPlanEventDetailByProduct(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string planId = context.Request["planId"];
            string spreadUserId = context.Request["spreadUserID"];
            string pv = context.Request["pv"];
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("  WebsiteOwner='{0}' ", bll.WebsiteOwner);
            if (!string.IsNullOrEmpty(planId))
            {
                sbWhere.AppendFormat(" AND MonitorPlanID={0}", int.Parse(planId));
            }

            if (!string.IsNullOrEmpty(spreadUserId))
            {
                sbWhere.AppendFormat(" AND speardUserId='{0}'", spreadUserId);
            }
            int totalCount = 0;
            List<MonitorEventDetailsInfo> list = new List<MonitorEventDetailsInfo>();
            if (!string.IsNullOrEmpty(pv))
            {
                list = bll.GetLit<ZentCloud.BLLJIMP.Model.MonitorEventDetailsInfo>(pageSize, pageIndex, sbWhere.ToString(), "DetailID DESC");
                totalCount = bll.GetCount<MonitorEventDetailsInfo>(sbWhere.ToString());
            }
            else
            {
                StringBuilder sqlWhere = new StringBuilder();
                sqlWhere.AppendFormat(@"   SELECT top 10000 max(b.[WXHeadimgurl]) EventUserWXImg, max(b.[WXNickname]) EventUserWXNikeName , max(b.[TrueName]) EventUserTrueName , max(b.[Phone]) EventUserPhone, max(a.SourceIP) SourceIP,max(a.IPLocation) IPLocation,max(a.EventDate) EventDate,max(a.EventBrowserID) EventBrowserID,max(a.EventSysPlatform) EventSysPlatform,max(a.EventUserID) EventUserID,MAX(a.SpreadUserID) spreadUserID
                    FROM ZCJ_MonitorEventDetailsInfo a LEFT JOIN ZCJ_UserInfo b ON a.EventUserID = b.UserID where a.EventUserID is not null and a.MonitorPlanID = {0} group by a.EventUserID ORDER BY max(a.EventDate) desc", planId);
                var eventList = ZentCloud.ZCBLLEngine.BLLBase.Query<MonitorEventDetailsInfo>(sqlWhere.ToString());
                List<MonitorEventDetailsInfo> returnList = new List<MonitorEventDetailsInfo>();
                if (eventList.Count > 0)
                {
                    returnList = eventList.Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
                }
                return Common.JSONHelper.ObjectToJson(new
                {
                    total = eventList.Count,
                    rows = returnList
                });

            }
            foreach (var item in list)
            {
                UserInfo userModel = bllUser.GetUserInfo(item.EventUserID);
                if (userModel != null)
                {
                    item.EventUserWXNikeName = userModel.WXNickname;
                    item.EventUserWXImg = userModel.WXHeadimgurl;
                    item.EventUserTrueName = userModel.TrueName;
                    item.EventUserPhone = userModel.Phone;
                }
            }
            return Common.JSONHelper.ObjectToJson(new
            {
                total = totalCount,
                rows = list
            });
        }
        /// <summary>
        /// 浏览量监测明细
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryPlanEventDetailByPV(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 50;

            string time=context.Request["time"];
            string userAutoId = context.Request["user_auto_id"];
            string userId = context.Request["userid"];
            int totalCount = 0;
            List<MonitorEventDetailsInfo> list = bllMonitor.GetMonitorEventDetailsByTime(pageSize, pageIndex, time, out totalCount, false, userAutoId, userId);
            foreach (var item in list)
            {
                UserInfo userModel = bllUser.GetUserInfo(item.EventUserID);
                if (userModel != null)
                {
                    item.EventUserWXNikeName = userModel.WXNickname;
                    item.EventUserWXImg = userModel.WXHeadimgurl;
                    item.EventUserTrueName = userModel.TrueName;
                    item.EventUserPhone = userModel.Phone;
                }
                item.SourceUrl = item.SourceUrl.TrimEnd();
            }
            return Common.JSONHelper.ObjectToJson(new
            {
                total = totalCount,
                rows=list
            });
        }
        /// <summary>
        /// UV全局统计
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryPlanEventDetailByUV(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 50;

            string date=context.Request["date"];

            string userAutoId=context.Request["user_auto_id"];
            int totalCount=0;

            List<DashboardMonitorInfo> list = bllDashboard.GetDashboardMonitorInfoList(pageSize, pageIndex, date, out totalCount,bll.WebsiteOwner,userAutoId);

            return Common.JSONHelper.ObjectToJson(new
            {
                total = totalCount,
                rows = list
            });

        }

        /// <summary>
        /// 删除微信推广
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteWeixinSpread(HttpContext context)
        {

            string ids = context.Request["ids"];
            int result = bll.Delete(new WeixinSpread(), string.Format(" WeixinSpreadID in ({0})", ids));
            return result.ToString();
        }

        /// <summary>
        /// 添加微信推广
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddWeixinSpread(HttpContext context)
        {


            string jsonData = context.Request["JsonData"];
            WeixinSpread model = ZentCloud.Common.JSONHelper.JsonToModel<WeixinSpread>(jsonData);
            //相关检查


            //if (!string.IsNullOrEmpty(model.SpreadUrl))
            //{
            //    if (!Common.PageValidate.IsUrl(model.SpreadUrl) || ((!model.SpreadUrl.EndsWith(".html")) && (!model.SpreadUrl.EndsWith(".htm")) && (!model.SpreadUrl.EndsWith(".php")) && (!model.SpreadUrl.EndsWith(".jsp")) && (!model.SpreadUrl.EndsWith(".aspx") && (!model.SpreadUrl.EndsWith(".asp")))))
            //    {
            //        return "请输入正确的链接地址,格式如 http://www.xxxx.com/index.html";
            //    }
            //}
            ////相关检查



            model.WeixinSpreadID = int.Parse(bll.GetGUID(ZentCloud.BLLJIMP.TransacType.WeixinSpreadID));
            model.UserID = currentUserInfo.UserID;
            model.InsertDate = DateTime.Now;

            MonitorPlan plan = new MonitorPlan();
            plan.MonitorPlanID = int.Parse(bll.GetGUID(ZentCloud.BLLJIMP.TransacType.MonitorPlanID));
            plan.PlanName = model.SpreadName;
            plan.PlanStatus = model.Status;
            plan.UserID = currentUserInfo.UserID;
            plan.InsertDate = DateTime.Now;
            if (bll.Add(plan))
            {
                model.PlanID = plan.MonitorPlanID.ToString();
                bool result = bll.Add(model);
                return result.ToString().ToLower();

            }
            else
            {
                return "false";
            }



        }

        /// <summary>
        /// 编辑微信推广 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWeixinSpread(HttpContext context)
        {

            string jsonData = context.Request["JsonData"];
            WeixinSpread model = ZentCloud.Common.JSONHelper.JsonToModel<WeixinSpread>(jsonData);
            //相关检查


            //if (!string.IsNullOrEmpty(model.SpreadUrl))
            //{
            //    if (!Common.PageValidate.IsUrl(model.SpreadUrl) ||((!model.SpreadUrl.EndsWith(".html")) && (!model.SpreadUrl.EndsWith(".htm")) && (!model.SpreadUrl.EndsWith(".php")) && (!model.SpreadUrl.EndsWith(".jsp")) && (!model.SpreadUrl.EndsWith(".aspx")&& (!model.SpreadUrl.EndsWith(".asp")))))
            //    {
            //        return "请输入正确的链接地址,格式如 http://www.xxxx.com/index.html";
            //    }
            //}
            //相关检查

            int count = bll.Update(model, string.Format("SpreadName='{0}',SpreadUrl='{1}',ActivityID='{2}',Status='{3}'", model.SpreadName, model.SpreadUrl, model.ActivityID, model.Status), string.Format("WeixinSpreadID='{0}'", model.WeixinSpreadID));

            if (count > 0)
            {

                return bll.Update(new MonitorPlan(), string.Format("PlanName='{0}',PlanStatus='{1}'", model.SpreadName, model.Status), string.Format("MonitorPlanID='{0}'", model.PlanID)) > 0 ? "true" : "false";

            }
            else
            {
                return "false";

            }


        }

        /// <summary>
        /// 修改微信推广状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string BatChangWeixinSpreadState(HttpContext context)
        {
            string[] ids = context.Request["ids"].Split(',');
            string status = context.Request["status"];

            int count = 0;
            foreach (string weixinSpreadID in ids)
            {
                WeixinSpread spreadinfo = bll.Get<WeixinSpread>(string.Format("WeixinSpreadID={0}", weixinSpreadID));
                if (spreadinfo != null)
                {
                    if (bll.Update(spreadinfo, string.Format("Status='{0}'", status), string.Format("WeixinSpreadID={0}", weixinSpreadID)) > 0)
                    {
                        if (bll.Update(new MonitorPlan(), string.Format("PlanStatus='{0}'", status), string.Format("MonitorPlanID={0}", spreadinfo.PlanID)) > 0)
                        {
                            count++;
                        }


                    }
                }



            }
            if (count == ids.Length)
            {
                return "true";
            }
            else
            {
                return "false";
            }







        }



        /// <summary>
        /// 查询微信推广
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryWeixinSpread(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string spreadName = context.Request["SpreadName"];
            //string feedBackStatus = context.Request["FeedBackStatus"];
            //string feedbackType = context.Request["FeedbackType"];
            //string feedbackPlatformCategory = context.Request["FeedbackPlatformCategory"];
            //string targetUser = context.Request["TargetUser"];
            StringBuilder sbCondtion = new StringBuilder("1=1");
            if (currentUserInfo.UserType != 1)
            {
                sbCondtion.AppendFormat(string.Format(" And UserID='{0}'", currentUserInfo.UserID));
            }

            if (!string.IsNullOrEmpty(spreadName))
            {
                sbCondtion.AppendFormat(" And SpreadName like '%{0}%'", spreadName);
            }

            //if (!string.IsNullOrEmpty(feedBackStatus))
            //{
            //    sbCondtion.AppendFormat(" And FeedBackStatus ='{0}'", feedBackStatus);
            //}
            //if (!string.IsNullOrEmpty(feedbackType))
            //{
            //    sbCondtion.AppendFormat(" And FeedBackType ='{0}'", feedbackType);
            //}
            //if (!string.IsNullOrEmpty(feedbackPlatformCategory))
            //{
            //    sbCondtion.AppendFormat(" And PlatformCategory ='{0}'", feedbackPlatformCategory);
            //}
            //if (!string.IsNullOrEmpty(targetUser))
            //{
            //    if (targetUser.Equals("0"))
            //    {
            //        sbCondtion.AppendFormat(" And UserID ='{0}'", useinfo.UserID);
            //    }
            //    else if (targetUser.Equals("1"))
            //    {
            //        sbCondtion.AppendFormat(" And AssignationUserID ='{0}'", useinfo.UserID);
            //    }

            //}


            //sbCondtion.AppendFormat(" And (UserID='{0}' or AssignationUserID='{0}')", useinfo.UserID);


            List<ZentCloud.BLLJIMP.Model.WeixinSpread> list = bll.GetLit<ZentCloud.BLLJIMP.Model.WeixinSpread>(pageSize, pageIndex, sbCondtion.ToString(), "WeixinSpreadID DESC");
            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.WeixinSpread>(sbCondtion.ToString());
            //string jsonResult = ZentCloud.Common.JSONHelper.ListToEasyUIJson(totalCount, list);
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                total = totalCount,
                rows = list
            });
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