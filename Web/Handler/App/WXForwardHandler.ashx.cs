using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using System.Reflection;
using System.Text;
using System.Data;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    ///微信转发
    /// </summary>
    public class WXForwardHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 基本响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse(); // 统一回复相应数据
        /// <summary>
        /// 当前登录用户
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo; //当前登陆的用户
        /// <summary>
        /// BLL基类
        /// </summary>
        BLLJIMP.BLL bll = new BLL();
        /// <summary>
        ///站点BLL
        /// </summary>
        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity();  //活动数据
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();  //用户数据
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLActivity bllActivity = new BLLActivity("");
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";

            try
            {
                this.currentUserInfo = bll.GetCurrentUserInfo();
                string action = context.Request["Action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "请联系管理员";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }
            context.Response.Write(result);
        }


        /// <summary>
        /// 导出数据
        /// </summary>
        private void DownLoadForwardData(HttpContext context)
        {
            string mid = context.Request["Mid"];
            //string activityId = context.Request["activityID"];
            //string uId = context.Request["uid"];
           // activityId = bllActivity.Get<BLLJIMP.Model.JuActivityInfo>(" JuActivityID=" + activityId).SignUpActivityID;

            //
            DataTable dt = new DataTable();
            
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT  ");
                strSql.AppendFormat(" userInfo.TrueName as 转发人姓名,");
                strSql.AppendFormat(" userInfo.Phone as 转发人手机号码,");
                strSql.AppendFormat(" ActivityName as 活动名称,");
                strSql.AppendFormat(" ActivitySignUpCount as 报名人数,");
                strSql.AppendFormat(" DistinctOpenCount as IP,");
                strSql.AppendFormat(" OpenCount as PV,");
                strSql.AppendFormat(" UV as 微信阅读人数,");
                strSql.AppendFormat(" RealLink as 链接地址,");
                strSql.AppendFormat(" InsertDate as 创建时间");

                strSql.Append(" FROM ZCJ_MonitorLinkInfo left join ZCJ_UserInfo userInfo on LinkName=userInfo.UserId ");
                strSql.AppendFormat(" WHERE MonitorPlanID={0} ", mid);
                dt = ZentCloud.ZCBLLEngine.BLLBase.Query(strSql.ToString()).Tables[0];

            //
            DataLoadTool.ExportDataTable(dt, string.Format("{0}data.xls", "", DateTime.Now.ToString()));


        }

        /// <summary>
        /// 获取当前活动所有转发信息
        /// </summary>
        /// <returns></returns>
        private string GetForwarInfos(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.MonitorLinkInfo> data;
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string activityId = context.Request["ActivityId"];
            string mid = context.Request["Mid"];
            string sort = context.Request["sort"];
            string order = context.Request["order"];
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" MonitorPlanID='{0}' ", mid));
            string orderBy = "";
            switch (sort)
            {
                case "OpenCount":
                    orderBy = " OpenCount " + order;
                    break;
                case "ActivitySignUpCount":
                    orderBy = " ActivitySignUpCount " + order;
                    break;
                case "ippv":
                    orderBy = " OpenCount " + order;
                    break;
                case "UV":
                    orderBy = "  UV " + order;
                    break;
                case "PowderCount":
                    orderBy = " PowderCount " + order;
                    break;
                case "AnswerCount":
                    orderBy = " AnswerCount " + order;
                    break;
                default:
                    orderBy = "  InsertDate DESC ";
                    break;
            }
            totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.MonitorLinkInfo>(sbWhere.ToString());
            data = this.bllJuactivity.GetLit<BLLJIMP.Model.MonitorLinkInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);

            JuActivityInfo juActivityModel = bll.Get<JuActivityInfo>(string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} ", bll.WebsiteOwner, mid));
            foreach (var item in data)
            {
                item.JuActivityID = juActivityModel != null ? juActivityModel.JuActivityID : 0;
            }
            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = totalCount,
                rows = data
            });
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <returns></returns>
        private string UpdateUser(HttpContext context)
        {
            string name = context.Request["Uname"];
            string phone = context.Request["Uphone"];

            if (string.IsNullOrEmpty(name))
            {
                resp.Status = -1;
                resp.Msg = "请填写用户名";
                goto outF;
            }
            if (string.IsNullOrEmpty(phone))
            {
                resp.Status = -1;
                resp.Msg = "请填写电话";
                goto outF;
            }
            currentUserInfo.TrueName = name;
            currentUserInfo.Phone = phone;
            if (bllUser.Update(currentUserInfo, string.Format(" TrueName='{0}',Phone='{1}'", currentUserInfo.TrueName, currentUserInfo.Phone), string.Format(" AutoID={0}", currentUserInfo.AutoID)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "更新成功";
                goto outF;
            }
            else
            {

                resp.Status = 0;
                resp.Msg = "更新失败";
                goto outF;
            }


        outF:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 检查用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CheckUserInfo(HttpContext context)
        {
            WebsiteInfo webSite = bllWebsite.GetWebsiteInfo(bllUser.WebsiteOwner);
            //判断是否在站点配置需要推荐码
            if (webSite.IsNeedDistributionRecommendCode == 1)
            {
                //需要推荐码
                if (string.IsNullOrEmpty(currentUserInfo.DistributionOwner))
                {
                    resp.Status = -1;
                    resp.Msg = "请输入推荐码";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                if (string.IsNullOrEmpty(currentUserInfo.TrueName) || string.IsNullOrEmpty(currentUserInfo.Phone))
                {
                    resp.Status = -1;
                    resp.Msg = "请填写完整的信息";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                if (bllUser.IsDistributionMember(currentUserInfo))
                {
                    resp.Status = 0;
                }
                else
                {
                    resp.Status = -2;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(currentUserInfo.TrueName) || string.IsNullOrEmpty(currentUserInfo.Phone))
                {
                    resp.Status = -1;
                    resp.Msg = "请填写完整的信息";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }

            ////线下分销会员 财富会员进入微转发免注册
            //if (!string.IsNullOrWhiteSpace(currentUserInfo.DistributionOffLinePreUserId))
            //{
            //    resp.Status = 0;

            //}
            //if (!string.IsNullOrEmpty(currentUserInfo.DistributionOwner))//虽然有上级,但是不符合分销会员标准
            //{
            //    resp.ExStr = "have_pre_user";
            //}
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取活动 所有报名人信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSignUpActivityInfo(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["PageIndex"]);
            int pageSize = Convert.ToInt32(context.Request["PageSize"]);
            string activityId = context.Request["ActivityID"];
            string spreadUserId = currentUserInfo.UserID;
            string monitorPlanId = context.Request["MonitorPlanID"];
            SignUpActivityPInfo model = new SignUpActivityPInfo();
            if (string.IsNullOrEmpty(activityId) && string.IsNullOrEmpty(spreadUserId) && string.IsNullOrEmpty(monitorPlanId))
            {
                resp.Status = -1;
                resp.Msg = "信息不完整";
                goto OutF;
            }

            List<BLLJIMP.Model.ActivityFieldMappingInfo> mapList = bllJuactivity.GetList<BLLJIMP.Model.ActivityFieldMappingInfo>(string.Format(" ActivityId='{0}'", activityId));
            model.MappingNum = mapList.Count;
            model.mppInfo = mapList;

            StringBuilder sbWhere = new StringBuilder(" SpreadUserID='" + currentUserInfo.UserID + "' AND  ActivityID= '" + activityId + "'");
            sbWhere.AppendFormat(" AND MonitorPlanID='{0}' AND WebsiteOwner='{1}' And IsDelete=0 ", monitorPlanId, bll.WebsiteOwner);

            int totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.ActivityDataInfo>(sbWhere.ToString());
            List<BLLJIMP.Model.ActivityDataInfo> activityDataInfo = this.bllJuactivity.GetLit<BLLJIMP.Model.ActivityDataInfo>(pageSize, pageIndex, sbWhere.ToString(), " InsertDate DESC ");
            if (activityDataInfo != null)
            {
                model.AdInfo = activityDataInfo;
                model.totalCount = totalCount;
            }
            if (model != null)
            {
                resp.Status = 1;
                resp.ExObj = model;
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "没有数据！！！";
            }


            int totalPage = bll.GetTotalPage(totalCount, pageSize);
            if ((totalPage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }

        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 后台获取粉丝信息
        /// </summary>
        /// <returns></returns>
        private string GetFansUserInfo(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string articleId = context.Request["article_id"];
            string userId = context.Request["user_id"];
            int totalCount = 0;
            List<UserInfo> userList = bllUser.GetFansInfoLists(pageIndex, pageSize, articleId, null, out totalCount);
            List<UserModel> returnList = new List<UserModel>();
            foreach (UserInfo item in userList)
            {
                UserModel userModel = new UserModel();
                userModel.distribution_owner = item.DistributionOwner;
                userModel.name = item.TrueName;
                userModel.phone = item.Phone;
                userModel.email = item.Email;
                userModel.head_img_url = item.WXHeadimgurl;
                userModel.postion = item.Postion;
                userModel.company = item.Company;
                userModel.wx_nick_name = item.WXNickname;
                returnList.Add(userModel);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                returnList = returnList.Where(p => p.distribution_owner == userId).ToList();

            }
            return Common.JSONHelper.ObjectToJson(new
            {
                total = returnList.Count(),
                rows = returnList
            });
        }
        /// <summary>
        /// 获取答题人信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetAnswerUserInfo(HttpContext context)
        {
            int pageIndex=!string.IsNullOrEmpty(context.Request["page"])?int.Parse(context.Request["page"]):1;
            int pageSize=!string.IsNullOrEmpty(context.Request["rows"])?int.Parse(context.Request["rows"]):50;
            string activityId=context.Request["aid"];
            string spreadUserId=context.Request["spread_userid"];
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" QuestionnaireID={0} ", int.Parse(activityId));
            if (!string.IsNullOrEmpty(spreadUserId))
            {
                sbWhere.AppendFormat(" AND PreUserId='{0}' ",spreadUserId);
            }

            int count = bllActivity.GetCount<QuestionnaireRecord>(sbWhere.ToString());

            List<QuestionnaireRecord> list = bll.GetLit<QuestionnaireRecord>(pageSize,pageIndex,sbWhere.ToString()," InsertDate DESC ");

            List<UserModel> returnList = new List<UserModel>();

            foreach (var item in list)
            {
                UserInfo user = bllUser.GetUserInfo(item.UserId);
                UserModel model=new UserModel();
                model.head_img_url = user.WXHeadimgurl;
                model.name = user.TrueName;
                model.wx_nick_name = user.WXNickname;
                model.phone = user.Phone;
                model.email = user.Email;
                model.company = user.Company;
                model.postion = user.Postion;
                returnList.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(new
            {
                total = count,
                rows = returnList
            });
        }

        //public class UserModel
        //{
        //    public string head_img_url { get; set; }

        //    public string wx_nick_name { get; set; }

        //    public string name { get; set; }

        //    public string phone { get; set; }

        //    public string email { get; set; }

        //    public string company { get; set; }

        //    public string postion { get; set; }
        //}


        /// <summary>
        /// 获取粉丝信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetFansInfo(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["PageIndex"]);
            int pageSize = Convert.ToInt32(context.Request["PageSize"]);
            string articleId = context.Request["article_id"];
            if (string.IsNullOrEmpty(articleId))
            {
                resp.Msg = "文章id为空!";
                resp.Status = -1;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            int totalCount = 0;
            List<UserInfo> userList = bllUser.GetFansInfoList(pageIndex, pageSize, articleId, currentUserInfo.UserID, out totalCount);
            List<dynamic> returnList = new List<dynamic>();
            if (userList.Count == 0)
            {
                resp.Status = -1;
                resp.Msg = "没有数据.";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            foreach (UserInfo item in userList)
            {
                returnList.Add(new
                {
                    name = item.TrueName,
                    phone = item.Phone,
                    email = item.Email,
                    head_img_url = item.WXHeadimgurl,
                    postion = item.Postion,
                    company = item.Company,
                    wxnickname = bllUser.GetUserDispalyName(item)
                });
            }

            int totalPage = bll.GetTotalPage(totalCount, pageSize);
            if ((totalPage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            resp.Status = 1;
            resp.ExObj = returnList;
            resp.ExInt = totalCount;
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 获取答题信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GeAnswerInfo(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["PageIndex"]);
            int pageSize = Convert.ToInt32(context.Request["PageSize"]);
            string articleId = context.Request["article_id"];
            string spreadUserId=context.Request["sid"];
            StringBuilder sbWhere = new StringBuilder();
            if (!string.IsNullOrEmpty(articleId))
            {
                sbWhere.AppendFormat(" QuestionnaireID={0} ", int.Parse(articleId));
            }
            if (!string.IsNullOrEmpty(spreadUserId))
            {
                sbWhere.AppendFormat(" AND PreUserId='{0}' ", spreadUserId);
            }

            int count = bllActivity.GetCount<QuestionnaireRecord>(sbWhere.ToString());

            List<QuestionnaireRecord> list = bll.GetLit<QuestionnaireRecord>(pageSize, pageIndex, sbWhere.ToString(), " InsertDate DESC ");


            List<UserModel> returnList = new List<UserModel>();

            if (list.Count == 0)
            {
                resp.Status = -1;
                resp.Msg = "没有数据.";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            foreach (var item in list)
            {

                UserInfo user = bllUser.GetUserInfo(item.UserId);
                UserModel model = new UserModel();
                model.head_img_url = user.WXHeadimgurl;
                model.name = user.TrueName;
                model.wx_nick_name = user.WXNickname;
                model.phone = user.Phone;
                model.email = user.Email;
                model.company = user.Company;
                model.postion = user.Postion;
                returnList.Add(model);
            }

            int totalPage = bll.GetTotalPage(count, pageSize);
            if ((totalPage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            resp.Status = 1;
            resp.ExObj = returnList;
            resp.ExInt = count;
            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 获取我的转发列表或搜索
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMyForwars(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["PageIndex"]);
            int pageSize = Convert.ToInt32(context.Request["PageSize"]);
            string activityName = context.Request["ActivityName"];
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" LinkName='{0}' And WebsiteOwner='{1}' ", currentUserInfo.UserID, bll.WebsiteOwner);
            if (!string.IsNullOrEmpty(activityName))
            {
                sbWhere.AppendFormat(" AND ActivityName='{0}'", activityName);
            }
            sbWhere.AppendFormat(" And( (Select Count(*) from ZCJ_JuActivityInfo where (ZCJ_JuActivityInfo.SignUpActivityID=ActivityId or ZCJ_JuActivityInfo.JuActivityID=ActivityId) And ZCJ_JuActivityInfo.IsDelete=0 And IsHide=0)>0  or (ForwardType='questionnaire' AND (Select Count(*) from ZCJ_Questionnaire where ZCJ_Questionnaire.QuestionnaireID=ActivityId And ZCJ_Questionnaire.IsDelete=0 And QuestionnaireVisible=1)>0))");
            
            int totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.MonitorLinkInfo>(sbWhere.ToString());
            List<BLLJIMP.Model.MonitorLinkInfo> monitorLink = this.bllJuactivity.GetLit<BLLJIMP.Model.MonitorLinkInfo>(pageSize, pageIndex, sbWhere.ToString(), " ActivityId DESC");
            foreach (var item in monitorLink)
            {
                var type = string.Empty;
                if (item.ForwardType == "fans")
                {
                    item.ForwardType = "文章";
                }
                else if (item.ForwardType == "questionnaire")
                {
                    item.ForwardType = "问卷";
                }
                else
                {
                    item.ForwardType = "活动";
                }

                JuActivityInfo juActivityModel = bllJuactivity.GetJuActivityByActivityID(item.ActivityId.ToString());
                item.PV = juActivityModel != null ? juActivityModel.PV : 0;
            }
            if (monitorLink != null)
            {
                resp.Status = 0;
                resp.ExObj = monitorLink;
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "没有数据.";
            }
            int totalPage = bll.GetTotalPage(totalCount, pageSize);
            if ((totalPage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetForwars(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["PageIndex"]);
            int pageSize = Convert.ToInt32(context.Request["PageSize"]);
            string activityName = context.Request["ActivityName"];

            string strWhere = string.Format("  WebsiteOwner= '{0}' ", bll.WebsiteOwner);
            if (!string.IsNullOrEmpty(activityName))
            {
                strWhere += string.Format(" And ActivityName like '%{0}%'", activityName);
            }
            strWhere += string.Format(" And (Select Count(*) from ZCJ_JuActivityInfo where ZCJ_JuActivityInfo.JuActivityID=ActivityId And ZCJ_JuActivityInfo.IsDelete=0 And IsHide=0)>0 or (ForwardType='questionnaire' And WebsiteOwner='{0}' AND (Select Count(*) from ZCJ_Questionnaire where ZCJ_Questionnaire.QuestionnaireID=ActivityId And ZCJ_Questionnaire.IsDelete=0 And QuestionnaireVisible=1)>0)",bll.WebsiteOwner);
           
            int totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.ActivityForwardInfo>(strWhere.ToString());
            List<BLLJIMP.Model.ActivityForwardInfo> data = this.bllJuactivity.GetLit<BLLJIMP.Model.ActivityForwardInfo>(pageSize, pageIndex, strWhere.ToString(), " InsertDate DESC ");
            if (data == null)
            {
                resp.Status = -1;
                resp.Msg = "列表为空";
            }
            else
            {
                List<BLLJIMP.Model.ActivityForwardInfo> dataList = new List<BLLJIMP.Model.ActivityForwardInfo>();
                foreach (BLLJIMP.Model.ActivityForwardInfo item in data)
                {
                    BLLJIMP.Model.JuActivityInfo juActivityInfo = null;
                    BLLJIMP.Model.Questionnaire question = null;
                    System.Text.StringBuilder sbWhere = new StringBuilder();
                    if (item.ForwardType == "questionnaire")
                    {
                        question = bllActivity.Get<Questionnaire>(string.Format(" QuestionnaireID='{0}'",item.ActivityId));
                        sbWhere.AppendFormat(" MonitorPlanID='{0}' AND LinkName='{1}' ",item.ActivityId,currentUserInfo.UserID);
                    }
                    else
                    {
                        juActivityInfo = bllJuactivity.Get<BLLJIMP.Model.JuActivityInfo>(" JuActivityID=" + item.ActivityId);
                        sbWhere.AppendFormat(" MonitorPlanID='{0}' And LinkName='{1}'", juActivityInfo.MonitorPlanID, currentUserInfo.UserID);
                    }

                    int totalCounts = bllJuactivity.GetCount<BLLJIMP.Model.MonitorLinkInfo>(sbWhere.ToString());

                    string type = string.Empty;
                    if (item.ForwardType == "questionnaire")
                    {
                        type = "问卷";
                    }
                    else if (item.ForwardType == "fans")
                    {
                        type = "文章";
                    }
                    else
                    {
                        type = "活动";
                    }

                    if (juActivityInfo != null || question!=null)
                    {
                        if (juActivityInfo!=null&&juActivityInfo.ActivityStatus == 1)
                        {
                            totalCount--;
                            continue;
                        }
                        if (totalCounts > 0)
                        {
                            if (juActivityInfo!=null)
                            {
                                dataList.Add(new ActivityForwardInfo
                                {
                                    ActivityId = item.ActivityId,
                                    ActivityName = item.ActivityName,
                                    InsertDate = item.InsertDate,
                                    ReadNum = juActivityInfo.UV,
                                    ThumbnailsPath = item.ThumbnailsPath,
                                    UserId = item.UserId,
                                    PV = juActivityInfo.PV,
                                    IsForwar = "已转发",
                                    ForwarNum = juActivityInfo.SignUpTotalCount,
                                    ForwardType = type,
                                    CurrentUserId=bll.GetCurrUserID()
                                });
                            }
                            else
                            {
                                dataList.Add(new ActivityForwardInfo
                                {
                                    ActivityId = item.ActivityId,
                                    ActivityName = item.ActivityName,
                                    InsertDate = item.InsertDate,
                                    ReadNum = question.UV,
                                    ThumbnailsPath = item.ThumbnailsPath,
                                    UserId = item.UserId,
                                    PV = question.PV,
                                    IsForwar = "已转发",
                                    ForwardType = type,
                                    CurrentUserId = bll.GetCurrUserID()
                                });
                            }
                            
                        }
                        else
                        {
                            if (juActivityInfo != null)
                            {
                                dataList.Add(new ActivityForwardInfo
                                {
                                    ActivityId = item.ActivityId,
                                    ActivityName = item.ActivityName,
                                    InsertDate = item.InsertDate,
                                    ReadNum = juActivityInfo.UV,
                                    ThumbnailsPath = item.ThumbnailsPath,
                                    UserId = item.UserId,
                                    IsForwar = "",
                                    PV = juActivityInfo.PV,
                                    ForwarNum = juActivityInfo.SignUpTotalCount,
                                    ForwardType = type,
                                    CurrentUserId = bll.GetCurrUserID()
                                });
                            }
                            else
                            {
                                dataList.Add(new ActivityForwardInfo
                                {
                                    ActivityId = item.ActivityId,
                                    ActivityName = item.ActivityName,
                                    InsertDate = item.InsertDate,
                                    ReadNum = question.UV,
                                    ThumbnailsPath = item.ThumbnailsPath,
                                    UserId = item.UserId,
                                    IsForwar = "",
                                    PV = question.PV,
                                    ForwardType = type,
                                    CurrentUserId = bll.GetCurrUserID()
                                });
                            }
                           
                        }

                    }
                    else
                    {
                        dataList.Add(item);
                    }

                }
                int totalPage = bll.GetTotalPage(totalCount, pageSize);
                if ((totalPage > pageIndex) && (pageIndex.Equals(1)))
                {
                    resp.ExStr = "1";//是否增加下一页按钮
                }
                resp.Status = 0;
                resp.ExObj = dataList;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }




        /// <summary>
        /// 获取微转发或微问卷列表
        /// </summary>
        /// <returns></returns>
        private string GetForwarList(HttpContext context)
        {


              
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string activityName = context.Request["ActivityName"];
            string sort = context.Request["sort"];
            string order = context.Request["order"];

            StringBuilder sbWhere = new StringBuilder(string.Format("websiteOwner= '{0}'", bll.WebsiteOwner));

            sbWhere.AppendFormat(" AND( ForwardType is null or ForwardType='questionnaire' )");
            if (!string.IsNullOrEmpty(activityName))
            {
                sbWhere.AppendFormat(" AND ActivityName like '%{0}%'",activityName);
            }
            string orderBy = "";
            switch (sort)
            {
                case "IPPV":
                    orderBy = " PV " + order;
                    break;
                case "UV":
                    orderBy = " UV " + order;
                    break;
                default:
                    orderBy = " InsertDate DESC ";
                    break;
            }
           
            int totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.ActivityForwardInfo>(sbWhere.ToString());
            List<BLLJIMP.Model.ActivityForwardInfo>  dataList = this.bllJuactivity.GetLit<BLLJIMP.Model.ActivityForwardInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
            List<BLLJIMP.Model.ActivityForwardInfo>  dataLists = new List<BLLJIMP.Model.ActivityForwardInfo>();
            foreach (BLLJIMP.Model.ActivityForwardInfo item in dataList)
            {
                JuActivityInfo juainfo=new JuActivityInfo();
                Questionnaire question=new Questionnaire();
                int forwarNum = 0;
                if (item.ForwardType == "questionnaire")
                {
                    
                    question = bllJuactivity.Get<Questionnaire>(string.Format(" WebsiteOwner='{0}' AND QuestionnaireID={1}", bllJuactivity.WebsiteOwner, item.ActivityId));
                    if (question != null)
                    {
                        forwarNum = bllJuactivity.GetCount<BLLJIMP.Model.MonitorLinkInfo>("  MonitorPlanID=" + question.QuestionnaireID);
                    }
                }
                else
                {
                    juainfo = bllJuactivity.Get<BLLJIMP.Model.JuActivityInfo>(" JuActivityID=" + item.ActivityId);
                    if (juainfo != null)
                    {
                        forwarNum = bllJuactivity.GetCount<BLLJIMP.Model.MonitorLinkInfo>("  MonitorPlanID=" + juainfo.MonitorPlanID);
                    }
                }
                dataLists.Add(new BLLJIMP.Model.ActivityForwardInfo
                {
                    ActivityId = item.ActivityId,
                    ActivityName = item.ActivityName,
                    InsertDate = item.InsertDate,
                    ForwardType = item.ForwardType == "questionnaire" ? "问卷" : "活动",
                    PV = item.PV,
                    IP =item.ForwardType=="questionnaire"?question.IP:juainfo.IP,
                    UV = item.UV,
                    UserId = item.UserId,
                    ForwarNum = forwarNum,
                    Mid = item.ForwardType == "questionnaire" ? int.Parse(item.ActivityId):juainfo.MonitorPlanID 
                });
                
            }

            return Common.JSONHelper.ObjectToJson(
             new
             {
                 total = totalCount,
                 rows = dataLists
             });

        }

        /// <summary>
        /// 微吸粉列表 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetForwarByFansList(HttpContext context)
        {
            string forwardType = context.Request["forward_type"];
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string activityName = context.Request["ActivityName"];
            string sort = context.Request["sort"];
            string order = context.Request["order"];

            StringBuilder sbWhere = new StringBuilder(string.Format("websiteOwner= '{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(activityName))
            {
                sbWhere.AppendFormat(" And ActivityName like '%{0}%'", activityName);
            }
            if (!string.IsNullOrEmpty(forwardType))
            {
                sbWhere.AppendFormat(" AND ForwardType='{0}'", forwardType);
            }
            string orderBy = "";
            switch (sort)
            {
                case "IPPV":
                    orderBy = " PV " + order;
                    break;
                case "UV":
                    orderBy = " UV " + order;
                    break;
                default:
                    orderBy = " InsertDate DESC ";
                    break;
            }

            int totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.ActivityForwardInfo>(sbWhere.ToString());

            List<BLLJIMP.Model.ActivityForwardInfo> dataList = this.bllJuactivity.GetLit<BLLJIMP.Model.ActivityForwardInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);

            List<BLLJIMP.Model.ActivityForwardInfo> dataLists = new List<BLLJIMP.Model.ActivityForwardInfo>();

            foreach (BLLJIMP.Model.ActivityForwardInfo item in dataList)
            {
                BLLJIMP.Model.JuActivityInfo juainfo = bllJuactivity.Get<BLLJIMP.Model.JuActivityInfo>(" JuActivityID=" + item.ActivityId);
                if (juainfo != null)
                {
                    int forwarNum = bllJuactivity.GetCount<BLLJIMP.Model.MonitorLinkInfo>("  MonitorPlanID=" + juainfo.MonitorPlanID);
                    dataLists.Add(new BLLJIMP.Model.ActivityForwardInfo
                    {
                        ActivityId = item.ActivityId,
                        ActivityName = item.ActivityName,
                        InsertDate = item.InsertDate,
                        PV = item.PV,
                        IP = juainfo.IP,
                        UV = item.UV,
                        UserId = item.UserId,
                        ForwarNum = forwarNum,
                        FansCount =bll.GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' AND ArticleId='{1}' AND DistributionOwner!='' ", bll.WebsiteOwner, item.ActivityId)),
                        Mid = juainfo.MonitorPlanID
                    });
                }
                else
                {
                    dataLists.Add(item);
                }
            }

            return Common.JSONHelper.ObjectToJson(
             new
             {
                 total = totalCount,
                 rows = dataLists
             });
        }






        /// <summary>
        /// 删除转发列表
        /// </summary>
        /// <returns></returns>
        private string DeleteForwar(HttpContext context)
        {

            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请选择要删除转发活动";
                goto OutF;

            }
            BLLJIMP.Model.ActivityForwardInfo model = new BLLJIMP.Model.ActivityForwardInfo();
            int count = bllJuactivity.Delete(model, " ActivityId in (" + ids + ") AND WebsiteOwner= '" + bll.WebsiteOwner + "'");
            if (count > 0)
            {
                resp.Status = 0;
                resp.Msg = "删除成功";

            }
            else
            {
                resp.Status = -1;
                resp.Msg = "删除失败";
            }

        OutF:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 是否显示转发排行榜
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateWebsiteForwardRank(HttpContext context)
        {
            string isShowForwardRank = context.Request["IsShowForwardRank"];
            string sortType=context.Request["sort_type"];
            WebsiteInfo webSiteModel = bllWebsite.GetWebsiteInfo(bll.WebsiteOwner);
            if (!string.IsNullOrEmpty(isShowForwardRank))
            {
                webSiteModel.IsShowForwardRank = Convert.ToInt32(isShowForwardRank);
            }

            if (!string.IsNullOrEmpty(sortType))
            {
                webSiteModel.SortType = Convert.ToInt32(sortType);
            }
            
            if (bll.Update(webSiteModel))
            {
                resp.Status = 0;
                resp.Msg = "设置完成";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "设置出错";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }


    /// <summary>
    ///  报名活动信息
    /// </summary>
    public class SignUpActivityPInfo
    {
        /// <summary>
        /// 报名字段个数
        /// </summary>
        public int MappingNum { get; set; }
        /// <summary>
        /// 报名人字段
        /// </summary>
        public List<BLLJIMP.Model.ActivityFieldMappingInfo> mppInfo { get; set; }
        /// <summary>
        /// 报名人详细信息
        /// </summary>
        public List<BLLJIMP.Model.ActivityDataInfo> AdInfo { get; set; }
        /// <summary>
        /// 报名总人数
        /// </summary>
        public int totalCount { get; set; }

    }
    /// <summary>
    /// 用户模型
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// 分销上级
        /// </summary>
        public string distribution_owner { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string head_img_url { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string company { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string wx_nick_name { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string postion { get; set; }
        /// <summary>
        /// email
        /// </summary>
        public string email { get; set; }



    }
}