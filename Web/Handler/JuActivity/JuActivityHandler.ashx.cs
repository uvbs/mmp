using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;
using System.Text;
using ZentCloud.BLLPermission;

namespace ZentCloud.JubitIMP.Web.Handler.JuActivity
{
    /// <summary>
    /// JuActivityHandler 的摘要说明
    /// </summary>
    public class JuActivityHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 基本响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bllJuactivity=new BLLJuActivity();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";

            try
            {
                this.currentUserInfo = bllJuactivity.GetCurrentUserInfo();
                string action = context.Request["Action"];
                switch (action)
                {

                    case "QueryJuActivityForWap":
                        result = QueryJuActivityForWap(context);
                        break;
                    case "QueryJuActivityForWeb":
                        result = QueryJuActivityForWeb(context);
                        break;
                    case "AddJuActivity":
                        result = AddJuActivity(context);//添加聚活动
                        break;
                    case "GetSingelJuActivity":
                        result = GetSingelJuActivity(context);//获取单条聚活动
                        break;
                    case "EditJuActivity":
                        result = EditJuActivity(context);//编辑聚活动
                        break;
                    case "DeleteJuActivity":
                        result = DeleteJuActivity(context);//删除聚活动
                        break;

                    //case "QueryJuMasterFeedBack"://专家库留言 查看给自己的留言
                    //    result = QueryJuMasterFeedBack(context);
                    //    break;

                    //case "AddJuMasterFeedBackDialog"://增加对话框
                    //    result = AddJuMasterFeedBackDialog(context);
                    //    break;
                    //case "QueryJuMasterFeedBackDialogue"://查询对话框详细
                    //    result = QueryJuMasterFeedBackDialogue(context);
                    //    break;

                    //case "QueryJuMasterUserLinkerInfo"://查询联系人
                    //    result = QueryJuMasterUserLinkerInfo(context);
                    //    break;
                    //case "EditMyJuMasterInfo"://编辑 添加我的专家资料
                    //    result = EditMyJuMasterInfo(context);
                    //    break;


                    case "BatchSetJuActivityStatus"://批量设置活动状态
                        result = BatchSetJuActivityStatus(context);
                        break;
                    case "BatchSetJuActivityRecommendCate"://批量活动分类
                        result = BatchSetJuActivityRecommendCate(context);
                        break;


                    case "EditToJubitActivity"://修改申请到取比特活动状态
                        result = EditToJubitActivity(context);
                        break;


                    case "GetSingelActivity"://获取单条报名表信息 ZCJ_ActivityInfo
                        result = GetSingelActivity(context);
                        break;


                    case "GetJuActivityInfoList"://活动文章列表
                        result = GetJuActivityInfoList(context);
                        break;





                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }

            context.Response.Write(result);
        }

        /// <summary>
        ///查询聚活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryJuActivityForWeb(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string userId = context.Request["UserID"];
            string isToJubitActivity = context.Request["IsToJubitActivity"];
            string activityName = context.Request["ActivityName"];
            string isSignUpJubit = context.Request["IsSignUpJubit"];
            string articleType = context.Request["ArticleType"];

            #region 老方法备份-20140104
            //StringBuilder strWhere = new StringBuilder(" 1=1 ");
            //strWhere.Append("AND IsDelete = 0 ");
            //if (!DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_JuActivity_Advanced))
            //{
            //    strWhere.AppendFormat(" And UserID ='{0}'", userInfo.UserID);
            //}
            //if (!string.IsNullOrEmpty(UserID))
            //{
            //    strWhere.AppendFormat(" And  UserID like '{0}'", UserID);
            //}
            //if (!string.IsNullOrEmpty(IsToJubitActivity))
            //{
            //    strWhere.AppendFormat(" And  IsToJubitActivity = '{0}'", IsToJubitActivity);
            //}
            //if (!string.IsNullOrEmpty(ActivityName))
            //{
            //    strWhere.AppendFormat(" And  ActivityName like '%{0}%'", ActivityName);
            //}
            //if (!string.IsNullOrEmpty(IsSignUpJubit))
            //{
            //    strWhere.AppendFormat(" And  IsSignUpJubit ='{0}'", IsSignUpJubit);
            //}
            //if (!string.IsNullOrEmpty(ArticleType))
            //{
            //    strWhere.AppendFormat(" And  ArticleType = '{0}'", ArticleType);
            //}
            //strWhere.AppendFormat(" And  IsDelete !=1");
            //List<JuActivityInfo> dataList = this.juActivityBll.GetLit<JuActivityInfo>(rows, page, strWhere.ToString(), "Sort DESC,ActivityStartDate DESC");
            //int totalCount = this.juActivityBll.GetCount<JuActivityInfo>(strWhere.ToString()); 
            #endregion

            int totalCount = 0;
            List<JuActivityInfo> dataList = this.bllJuactivity.QueryJuActivityData(
                null, out totalCount, null, null, null, null, activityName, pageIndex, pageSize, 
                currentUserInfo.UserType.Equals(1) ? "" : currentUserInfo.UserID, 
                null, articleType);

            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = totalCount,
                rows = dataList
            });
        }

        /// <summary>
        /// 批量设置聚活动状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string BatchSetJuActivityStatus(HttpContext context)
        {
            string juActivityIds = context.Request["JuActivityIds"];
            string status = context.Request["Status"];
            //if (!DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_JuActivity_Advanced))
            //{
            //    JuActivityIds=


            //} 
            int count = bllJuactivity.Update(new JuActivityInfo(), string.Format("IsToJubitActivity='{0}'", status), string.Format("JuActivityID in ({0})", juActivityIds));
            if (count == juActivityIds.Split(',').Length)
            {
                resp.Status = 1;
                resp.Msg = "修改成功!";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "修改失败";

            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 批量设置聚活动分类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string BatchSetJuActivityRecommendCate(HttpContext context)
        {
            string juActivityIds = context.Request["JuActivityIds"];
            string recommendCate = context.Request["RecommendCate"];
            int count = bllJuactivity.Update(new JuActivityInfo(), string.Format("RecommendCate='{0}'", recommendCate), string.Format("JuActivityID in ({0})", juActivityIds));
            if (count == juActivityIds.Split(',').Length)
            {
                resp.Status = 1;
                resp.Msg = "设置分类成功!";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "设置分类失败";

            }
            return Common.JSONHelper.ObjectToJson(resp);

        }


        //private string QueryJuActivityForWap(HttpContext context)
        //{
        //    throw new NotImplementedException();
        //}

        private string DeleteJuActivity(HttpContext context)
        {
            //删除活动由物理删除变为更新IsDelete标识为1，并且只有自己能删除自己的数据 -- 2013.11.13

            string ids = context.Request["ids"];
            //int result = this.juActivityBll.Delete(new JuActivityInfo(), string.Format(" JuActivityID in ({0})", ids));

            int result = this.bllJuactivity.Update(new JuActivityInfo(), " IsDelete = 1 ", string.Format(" JuActivityID in ({0})  {1}", ids, this.currentUserInfo.UserType != 1 ? string.Format("AND UserID = '{0}'", this.currentUserInfo.UserID) : ""));

            resp.Status = 1;
            resp.Msg = string.Format("成功删除了{0}个活动", result);
            return Common.JSONHelper.ObjectToJson(resp);
        }

        private string EditJuActivity(HttpContext context)
        {
            int juActivityID = Convert.ToInt32(context.Request["JuActivityID"]);
            BLLJIMP.Model.JuActivityInfo model = this.bllJuactivity.Get<BLLJIMP.Model.JuActivityInfo>("JuActivityID = " + juActivityID.ToString());

            if (this.currentUserInfo.UserType != 1)
            {
                model = this.bllJuactivity.Get<BLLJIMP.Model.JuActivityInfo>(string.Format("JuActivityID = {0}  AND UserID = '{1}' ", juActivityID.ToString(), this.currentUserInfo.UserID));
            }

            if (model == null)
            {
                resp.Status = 0;
                resp.Msg = "活动不存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            model.ActivityName = context.Request["ActivityName"];
            string activityStartDate = context.Request["ActivityStartDate"];
            if (!string.IsNullOrEmpty(activityStartDate))
            {
                model.ActivityStartDate = DateTime.Parse(activityStartDate);
            }
            string activityEndDate = context.Request["ActivityEndDate"];
            if (!string.IsNullOrEmpty(activityEndDate))
            {
                model.ActivityEndDate = DateTime.Parse(activityEndDate);
            }


            model.ActivityAddress = context.Request["ActivityAddress"];
            model.ActivityWebsite = context.Request["ActivityWebsite"];
            model.ActivityDescription = context.Request["ActivityDescription"];
            model.ThumbnailsPath = context.Request["ThumbnailsPath"];

            model.RecommendCate = context.Request["RecommendCate"];
            model.IsHide = Convert.ToInt32(context.Request["IsHide"]);
            model.Sort = Convert.ToInt32(context.Request["Sort"]);
            model.IsFee = Convert.ToInt32(context.Request["IsFee"]);
            model.ArticleTemplate = Convert.ToInt32(context.Request["ArticleTemplate"]);

            model.IsByWebsiteContent = Convert.ToInt32(context.Request["IsByWebsiteContent"]);

            model.TopImgPath = context.Request["TopImgPath"];

            //model.SignUpActivityID = context.Request["SignUpActivityID"];活动ID默认创建，不给编辑了
            model.IsSpread = Convert.ToInt32(context.Request["IsSpread"]);

            bool isAddSignUpplan = false;
            model.ArticleType = context.Request["ArticleType"];
            //如果ArticleType类型是article，则IsSignUpJubit都为0;
            if (model.ArticleType == "article")
                model.IsSignUpJubit = 0;
            else
            {
                //如果由其他状态编辑更改为自动报名状态，则重新自动创建任务
                int isSignUpJubit = Convert.ToInt32(context.Request["IsSignUpJubit"]);
                if (model.IsSignUpJubit != 1 && isSignUpJubit == 1)
                {
                    isAddSignUpplan = true;
                }

                model.IsSignUpJubit = isSignUpJubit;
            }

            resp.Status = 0;
            resp.Msg = "更新失败!";

            //model.IsSignUpJubit = int.Parse(context.Request["IsSignUpJubit"]);
            if (isAddSignUpplan)
            {
                ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

                try
                {
                    ActivityInfo signUpActivityModel = this.bllJuactivity.CreateSignUpActivityModelByJuActivity(model, this.currentUserInfo.UserID);
                    model.SignUpActivityID = signUpActivityModel.ActivityID;


                    //添加默认字段
                    List<ActivityFieldMappingInfo> fieldData = new List<ActivityFieldMappingInfo>() {
                         new ActivityFieldMappingInfo()
                        { 
                            ActivityID = model.SignUpActivityID, 
                            ExFieldIndex = 1, 
                            FieldIsDefauld = 0,
                            FieldType = 0,
                            FormatValiFunc = "email",
                            MappingName = "邮箱"
                        },
                        new ActivityFieldMappingInfo()
                        { 
                            ActivityID = model.SignUpActivityID, 
                            ExFieldIndex = 2, 
                            FieldIsDefauld = 0,
                            FieldType = 0,
                            MappingName = "公司"
                        },
                        new ActivityFieldMappingInfo()
                        { 
                            ActivityID = model.SignUpActivityID, 
                            ExFieldIndex = 3, 
                            FieldIsDefauld = 0,
                            FieldType = 0,
                            MappingName = "职位"
                        }
                        //,
                        //new ActivityFieldMappingInfo()
                        //{ 
                        //    ActivityID = model.SignUpActivityID, 
                        //    ExFieldIndex =4, 
                        //    FieldIsDefauld = 0,
                        //    FieldType =1,
                        //    MappingName = "推广人"
                        //}
                    };
                    if (!bllJuactivity.AddList(fieldData))
                    {
                        tran.Rollback();
                    }
                    else
                    {

                        if (this.bllJuactivity.Update(model, tran) && this.bllJuactivity.Add(signUpActivityModel, tran))
                        {
                            tran.Commit();
                            resp.Status = 1;
                            resp.Msg = "更新成功!";
                        }
                        else
                            tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    resp.Msg = ex.Message;
                }

            }
            else
            {

                if (this.bllJuactivity.Update(model))
                {
                    resp.Status = 1;
                    resp.Msg = "更新成功!";
                }
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        private string GetSingelJuActivity(HttpContext context)
        {
            int juActivityID = Convert.ToInt32(context.Request["JuActivityID"]);
            BLLJIMP.Model.JuActivityInfo model = this.bllJuactivity.Get<BLLJIMP.Model.JuActivityInfo>("JuActivityID = " + juActivityID.ToString());

            if (this.currentUserInfo.UserType != 1)
            {
                model = this.bllJuactivity.Get<BLLJIMP.Model.JuActivityInfo>(string.Format("JuActivityID = {0}  AND UserID = '{1}' ", juActivityID.ToString(), this.currentUserInfo.UserID));
            }

            if (model == null)
            {
                resp.Status = 0;
                resp.Msg = "活动不存在";
            }
            else
            {
                resp.Status = 1;
                resp.ExObj = model;
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        private string AddJuActivity(HttpContext context)
        {

            //判断是否已经补足个人资料
            string isChcekUserInfo = context.Request["alluser"];

            if (isChcekUserInfo == "1")
            {
                if (!new BLLUser("").IsAllUserBaseInfo(this.currentUserInfo.UserID))
                {
                    resp.Status = 0;
                    resp.Msg = "请补足个人资料再进行操作!";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }

            string isReplaceN = context.Request["isReplaceN"];

            BLLJIMP.Model.JuActivityInfo model = new JuActivityInfo();
            model.UserID = this.currentUserInfo.UserID;
            model.JuActivityID = int.Parse(this.bllJuactivity.GetGUID(BLLJIMP.TransacType.ActivityAdd));
            model.ActivityName = context.Request["ActivityName"];
            string activityStartDate = context.Request["ActivityStartDate"];
            if (!string.IsNullOrEmpty(activityStartDate))
            {
                model.ActivityStartDate = DateTime.Parse(activityStartDate);
            }
            string activityEndDate = context.Request["ActivityEndDate"];
            if (!string.IsNullOrEmpty(activityEndDate))
            {
                model.ActivityEndDate = DateTime.Parse(activityEndDate);
            }
            model.ActivityAddress = context.Request["ActivityAddress"];
            model.ActivityWebsite = context.Request["ActivityWebsite"];
            model.ActivityDescription = context.Request["ActivityDescription"];
            model.ThumbnailsPath = context.Request["ThumbnailsPath"];
            model.IsSignUpJubit = int.Parse(context.Request["IsSignUpJubit"]);
            model.SignUpActivityID = context.Request["SignUpActivityID"];
            model.RecommendCate = context.Request["RecommendCate"];
            model.IsHide = Convert.ToInt32(context.Request["IsHide"]);
            model.Sort = Convert.ToInt32(context.Request["Sort"]);
            model.IsFee = Convert.ToInt32(context.Request["IsFee"]);
            model.ArticleTemplate = Convert.ToInt32(context.Request["ArticleTemplate"]);
            model.TopImgPath = context.Request["TopImgPath"];
            model.ActivityLecturer = context.Request["ActivityLecturer"];
            model.IsByWebsiteContent = Convert.ToInt32(context.Request["IsByWebsiteContent"]);
            model.WebsiteOwner = bllJuactivity.WebsiteOwner;
            if(isReplaceN == "1")
                model.ActivityDescription = model.ActivityDescription.Replace("\n", "<br />");

            model.CreateDate = DateTime.Now;

            model.IsSpread = Convert.ToInt32(context.Request["IsSpread"]);
            model.ArticleType = context.Request["ArticleType"];
            //如果ArticleType类型是article，则IsSignUpJubit都为0;
            if (model.ArticleType == "article")
                model.IsSignUpJubit = 0;

            ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

            try
            {
                if (model.IsSignUpJubit == 1)
                {
                    //自动创建报名活动
                    ActivityInfo signUpActivityModel = new ActivityInfo();
                    signUpActivityModel.ActivityID = this.bllJuactivity.GetGUID(TransacType.ActivityAdd);
                    signUpActivityModel.UserID = this.currentUserInfo.UserID;
                    signUpActivityModel.ActivityName = model.ActivityName;
                    signUpActivityModel.ActivityDate = model.ActivityStartDate;
                    signUpActivityModel.ActivityAddress = model.ActivityAddress;
                    signUpActivityModel.ActivityWebsite = model.ActivityWebsite;
                    signUpActivityModel.ActivityStatus = 1;
                    signUpActivityModel.LimitCount = 100;
                    signUpActivityModel.ActivityDescription = string.Format("该任务为活动{0}自动创建", model.JuActivityID);

                    //设置自动生成的ID
                    model.SignUpActivityID = signUpActivityModel.ActivityID;

                    if (!this.bllJuactivity.Add(signUpActivityModel, tran))
                    {
                        tran.Rollback();
                        resp.Status = 0;
                        resp.Msg = "添加失败!";
                        return Common.JSONHelper.ObjectToJson(resp);
                    }

                    //添加默认字段
                    List<ActivityFieldMappingInfo> fieldData = new List<ActivityFieldMappingInfo>() {
                         new ActivityFieldMappingInfo()
                        { 
                            ActivityID = model.SignUpActivityID, 
                            ExFieldIndex = 1, 
                            FieldIsDefauld = 0,
                            FieldType = 0,
                            FormatValiFunc = "email",
                            MappingName = "邮箱"
                        },
                        new ActivityFieldMappingInfo()
                        { 
                            ActivityID = model.SignUpActivityID, 
                            ExFieldIndex = 2, 
                            FieldIsDefauld = 0,
                            FieldType = 0,
                            MappingName = "公司"
                        },
                        new ActivityFieldMappingInfo()
                        { 
                            ActivityID = model.SignUpActivityID, 
                            ExFieldIndex = 3, 
                            FieldIsDefauld = 0,
                            FieldType = 0,
                            MappingName = "职位"
                        }
                        //,
                        //new ActivityFieldMappingInfo()
                        //{ 
                        //    ActivityID = model.SignUpActivityID, 
                        //    ExFieldIndex =4, 
                        //    FieldIsDefauld = 0,
                        //    FieldType =1,
                        //    MappingName = "推广人"
                        //}
                    };
                    if (!bllJuactivity.AddList(fieldData))
                    {
                        tran.Rollback();
                        resp.Status = 0;
                        resp.Msg = "添加默认字段失败!";
                        return Common.JSONHelper.ObjectToJson(resp);

                    };


                }

                //自动创建推广活动
                MonitorPlan monitorPlanModel = new MonitorPlan();
                monitorPlanModel.MonitorPlanID = int.Parse(this.bllJuactivity.GetGUID(TransacType.MonitorPlanID));
                monitorPlanModel.PlanName = model.ActivityName;
                monitorPlanModel.PlanStatus = "1";
                monitorPlanModel.UserID = this.currentUserInfo.UserID;
                monitorPlanModel.InsertDate = DateTime.Now;
                monitorPlanModel.Remark = "自动创建的监测任务";

                model.MonitorPlanID = monitorPlanModel.MonitorPlanID;

                if (this.bllJuactivity.Add(monitorPlanModel, tran) && this.bllJuactivity.Add(model, tran))
                {
                    tran.Commit();
                    resp.Status = 1;
                    resp.ExObj = model;
                    resp.ExStr = model.JuActivityIDHex;//将16进制ID传回去
                    resp.Msg = "添加成功!";
                }
                else
                {
                    tran.Rollback();
                    resp.Status = 0;
                    resp.Msg = "添加失败!";
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.Status = 0;
                resp.Msg = ex.Message;
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// wap 活动查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryJuActivityForWap(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string queryType = context.Request["queryType"];
            string activityAddress = context.Request["ActivityAddress"];
            string activityStartDate = context.Request["ActivityStartDate"];//传入的是最近天数
            string recommendCate = context.Request["RecommendCate"];
            string isFee = context.Request["IsFee"];
            string keyWord = context.Request["KeyWord"];
            string fromSearchHtml = context.Request["fromSearchHtml"];
            List<JuActivityInfo> dataList = new List<JuActivityInfo>();
            int count = 0;
            dataList = bllJuactivity.QueryJuActivityData(queryType, out count, activityAddress, activityStartDate, recommendCate, isFee, keyWord, pageIndex, pageSize);
            int totalcount = this.bllJuactivity.GetTotalPage(count, pageSize);
            if (string.IsNullOrEmpty(activityAddress) && string.IsNullOrEmpty(activityStartDate) && string.IsNullOrEmpty(recommendCate) && string.IsNullOrEmpty(isFee) && string.IsNullOrEmpty(keyWord) && (pageIndex == 1) && (fromSearchHtml == "0"))
            {

                return ConverHtmlFormateTop(dataList, ((totalcount > pageIndex) && pageIndex == 1));

            }
            if (recommendCate.Equals("聚比特活动"))
            {
                return ConverHtmlFormartJubit(dataList, keyWord, ((totalcount > pageIndex) && pageIndex == 1));

            }

            return ConverHtmlFormate(dataList, keyWord, ((totalcount > pageIndex) && pageIndex == 1));

        }


        ///// <summary>
        ///// 查询专家自己的留言列表
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QueryJuMasterFeedBack(HttpContext context)
        //{

        //    int page = Convert.ToInt32(context.Request["page"]);
        //    int rows = Convert.ToInt32(context.Request["rows"]);
        //    // string MasterID = context.Request["MasterID"];
        //    string FeedBackStatus = context.Request["FeedBackStatus"];
        //    StringBuilder sbWhere = new StringBuilder(string.Format("MasterID='{0}'", userInfo.UserID));

        //    // sbWhere.AppendFormat("MasterID='{0}'", MasterID);
        //    if (!string.IsNullOrEmpty(FeedBackStatus))
        //    {
        //        if (FeedBackStatus.Equals("0"))
        //        {
        //            FeedBackStatus = "未处理";
        //        }
        //        if (FeedBackStatus.Equals("1"))
        //        {
        //            FeedBackStatus = "已回复";
        //        }

        //        sbWhere.AppendFormat(" And ProcessStatus='{0}'", FeedBackStatus);


        //    }
        //    int count = juActivityBll.GetCount<JuMasterFeedBack>(sbWhere.ToString());
        //    int totalcount = this.juActivityBll.GetTotalPage(count, rows);

        //    List<JuMasterFeedBack> dataList = new List<JuMasterFeedBack>();
        //    dataList = juActivityBll.GetLit<JuMasterFeedBack>(rows, page, sbWhere.ToString(), "FeedBackID DESC");

        //    return ConverHtmlFormateMasterFeedBack(dataList, rows, ((totalcount > page) && page == 1));




        //}

        ///// <summary>
        ///// 查询专家留言对话框详细
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QueryJuMasterFeedBackDialogue(HttpContext context)
        //{
        //    string feedbackid = context.Request["FeedBackID"];
        //    StringBuilder sbWhere = new StringBuilder(string.Format("FeedBackID='{0}'", feedbackid));
        //    sbWhere.Append(" Order by FeedBackID DESC");
        //    // sbWhere.AppendFormat("MasterID='{0}'", MasterID);
        //    List<JuMasterFeedBackDialogue> dataList = new List<JuMasterFeedBackDialogue>();
        //    dataList = juActivityBll.GetList<JuMasterFeedBackDialogue>(sbWhere.ToString());
        //    StringBuilder sb = new StringBuilder();
        //    JuMasterFeedBack feedbackInfo = juActivityBll.Get<JuMasterFeedBack>(string.Format("FeedBackID='{0}'", feedbackid));
        //    sb.AppendLine("<div style=\"border: 1px solid #CCC;margin-top:10px;\">");
        //    sb.AppendLine("<div style=\"font-family: Helvetica,Arial,sans-serif;text-align: left;font-weight: bold;background-color: #E7E7E7;padding: 5px;font-size: 16px;color: #930;\">");
        //    sb.AppendFormat("{0} : {1}", feedbackInfo.UserNickName, feedbackInfo.SubmitDate.ToString("yyyy-MM-dd HH:mm:ss"));
        //    sb.AppendLine("</div>");
        //    sb.AppendLine("<div style=\"font-family: Helvetica,Arial,sans-serif;margin-left:5px;margin-top:10px;font-size: 16px;color: #666666;line-height: 18px;\">");
        //    sb.Append(feedbackInfo.FeedBackContent);
        //    sb.AppendLine("</div>");
        //    sb.AppendLine("</div>");

        //    return sb + ConverHtmlFormateMasterFeedBackDialog(dataList);

        //}


        ///// <summary>
        ///// 添加对话框
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string AddJuMasterFeedBackDialog(HttpContext context)
        //{
        //    BLLJIMP.Model.JuMasterFeedBackDialogue model = new JuMasterFeedBackDialogue();
        //    model.DialogueID = int.Parse(this.juActivityBll.GetGUID(BLLJIMP.TransacType.AddJuMasterFeedBackDialogue));

        //    model.UserID = this.userInfo.UserID;
        //    model.FeedBackID = int.Parse(context.Request["FeedBackID"]);
        //    model.DialogueContent = context.Request["DialogueContent"];
        //    model.SubmitDate = DateTime.Now;
        //    model.SubmitIP = Common.MySpider.GetClientIP();
        //    ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

        //    try
        //    {
        //        if (this.juActivityBll.Add(model, tran))
        //        {
        //            if (juActivityBll.Update(new JuMasterFeedBack(), " ProcessStatus='已回复'", string.Format("FeedBackID={0}", model.FeedBackID), tran) > 0)
        //            {
        //                tran.Commit();
        //                resp.Status = 1;
        //                resp.Msg = "添加成功!";

        //            }
        //            else
        //            {
        //                tran.Rollback();
        //                resp.Status = 0;
        //                resp.Msg = "添加失败!";

        //            }
        //        }
        //        else
        //        {
        //            tran.Rollback();
        //            resp.Status = 0;
        //            resp.Msg = "添加失败!";
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        resp.Status = 0;
        //        resp.Msg = ex.Message;


        //    }

        //    return Common.JSONHelper.ObjectToJson(resp);
        //}
        /// <summary>
        /// 获取单条活动信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSingelActivity(HttpContext context)
        {
            int activityId = Convert.ToInt32(context.Request["ActivityID"]);
            BLLJIMP.Model.ActivityInfo model = this.bllJuactivity.Get<BLLJIMP.Model.ActivityInfo>("ActivityID = " + activityId.ToString());
            if (this.currentUserInfo.UserType != 1)
            {
                model = this.bllJuactivity.Get<BLLJIMP.Model.ActivityInfo>(string.Format("ActivityID = {0}  AND UserID = '{1}' ", activityId.ToString(), this.currentUserInfo.UserID));
            }

            if (model == null)
            {
                resp.Status = 0;
                resp.Msg = "活动不存在";
            }
            else
            {
                resp.Status = 1;
                resp.ExObj = model;
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        ///// <summary>
        ///// 查询专家库留言列表
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QueryJuMasterUserLinkerInfo(HttpContext context)
        //{

        //    int page = Convert.ToInt32(context.Request["page"]);
        //    int rows = Convert.ToInt32(context.Request["rows"]);
        //    // string MasterID = context.Request["MasterID"];
        //    // string MasterID = context.Request["MasterID"];
        //    StringBuilder sbWhere = new StringBuilder(string.Format("MasterID='{0}'", userInfo.UserID));

        //    // sbWhere.AppendFormat("MasterID='{0}'", MasterID);
        //    //if (!string.IsNullOrEmpty(FeedBackStatus))
        //    //{
        //    //    if (FeedBackStatus.Equals("0"))
        //    //    {
        //    //        FeedBackStatus = "未处理";
        //    //    }
        //    //    if (FeedBackStatus.Equals("1"))
        //    //    {
        //    //        FeedBackStatus = "已回复";
        //    //    }

        //    //    sbWhere.AppendFormat(" And ProcessStatus='{0}'", FeedBackStatus);


        //    //}
        //    int count = juActivityBll.GetCount<JuMasterUserLinkerInfo>(sbWhere.ToString());
        //    int totalcount = this.juActivityBll.GetTotalPage(count, rows);

        //    List<JuMasterUserLinkerInfo> dataList = new List<JuMasterUserLinkerInfo>();
        //    dataList = juActivityBll.GetLit<JuMasterUserLinkerInfo>(rows, page, sbWhere.ToString(), "LinkerID DESC");

        //    return ConverHtmlFormateMasterUserLinkerInfo(dataList, rows, ((totalcount > page) && page == 1));




        //}

        ///// <summary>
        ///// 修改我的专家资料
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string EditMyJuMasterInfo(HttpContext context)
        //{
        //    JuMasterInfo model = new JuMasterInfo();
        //    model.MasterName = context.Request["MasterName"];
        //    model.Gender = context.Request["Gender"];
        //    model.Company = context.Request["Company"];
        //    model.Title = context.Request["Title"];
        //    model.Summary = context.Request["Summary"];
        //    model.IntroductionContent = context.Request["IntroductionContent"];
        //    model.HeadImg = context.Request["HeadImg"];

        //    JuMasterInfo masterInfo = juActivityBll.Get<JuMasterInfo>(string.Format("MasterID='{0}'", userInfo.UserID));
        //    if (masterInfo != null)//编辑
        //    {
        //        masterInfo.MasterName = model.MasterName;
        //        masterInfo.Gender = model.Gender;
        //        masterInfo.Company = model.Company;
        //        masterInfo.Title = model.Title;
        //        masterInfo.Summary = model.Summary;
        //        masterInfo.IntroductionContent = model.IntroductionContent;
        //        masterInfo.HeadImg = model.HeadImg;
        //        if (juActivityBll.Update(masterInfo))
        //        {

        //            resp.Status = 1;
        //            resp.Msg = "保存成功！";


        //        }
        //        else
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "保存失败";

        //        }




        //    }
        //    else//添加
        //    {
        //        model.MasterID = userInfo.UserID;
        //        model.AddUserID = userInfo.UserID;
        //        if (juActivityBll.Add(model))
        //        {
        //            resp.Status = 1;
        //            resp.Msg = "保存成功！";

        //        }
        //        else
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "保存失败";

        //        }

        //    }

        //    return Common.JSONHelper.ObjectToJson(resp);


        //}

        /// <summary>
        /// 修改申请到取比特活动状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditToJubitActivity(HttpContext context)
        {

            string juActivityIds = context.Request["JuActivityIDs"];
            string stautus = context.Request["Status"];
            if (bllJuactivity.Update(new JuActivityInfo(), string.Format("IsToJubitActivity='{0}'", stautus), string.Format("JuActivityID in ({0})", juActivityIds)) == juActivityIds.Split(',').Length)
            {

                resp.Status = 1;
                resp.Msg = "修改成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "修改失败";

            }
            return Common.JSONHelper.ObjectToJson(resp);


        }

        private string ConverHtmlFormate(List<JuActivityInfo> dataList, string highLight = "", bool isShowBtnNext = false)
        {

            StringBuilder sbWhere = new StringBuilder();
            if (dataList.Count.Equals(0))
            {
                return "";
            }
            sbWhere.AppendLine("<table  style=\"margin-top :5;margin-left:5;padding:10;width:100%;\">");
            sbWhere.AppendLine("<tbody>");
            foreach (var item in dataList)
            {
                if (!string.IsNullOrWhiteSpace(highLight))
                    item.ActivityName = item.ActivityName.Replace(highLight, "<span style=\"color:red;\">" + highLight + "</span>");

                sbWhere.AppendFormat(" <tr rel=\"{0}\" onclick=\"goto(this)\" ><td >", item.ActivityWebsite);
                sbWhere.AppendFormat("<span style=\"font-size:100%;line-height:100%;color:Black\" >{0}</span>", item.ActivityName);
                sbWhere.AppendLine("<br />");
                sbWhere.AppendFormat("<div  style=\"font-size: 90%;line-height: 100%;margin-top:5px;\" >{0}</div>", string.Format("{0:f}", item.ActivityStartDate));
                sbWhere.AppendLine("</td><td valign=\"top\" align=\"right\"  >");
                sbWhere.AppendFormat("<img src=\"{0}\" style=\"width:50px;height:50px;margin-right:10px;\" /> ", item.ThumbnailsPath);
                sbWhere.AppendLine("</td></tr>");
                sbWhere.AppendFormat("<tr><td colspan=\"2\"><hr /></td></tr>");



            }
            sbWhere.AppendLine("</tbody>");
            sbWhere.AppendLine("</table>");
            if (isShowBtnNext)
            {
                //sb.AppendFormat(" <tr><td colspan=\"2\"><input   data-theme=\"c\"  type=\"button\" id=\"btnNext\" value=\"加载更多...\"/></td></tr>");
                sbWhere.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" ></div>");
                //sb.AppendFormat("<a href=\"javascript:;\" data-role=\"button\" inline=\"true\" data-theme=\"c\" id=\"btnNext\" data-corners=\"true\" isshow=\"yes\" data-shadow=\"true\" data-iconshadow=\"true\" data-wrapperels=\"span\" class=\"ui-btn ui-shadow ui-btn-corner-all ui-btn-hover-c ui-btn-up-c\" ><span class=\"ui-btn-inner ui-btn-corner-all\"><span class=\"ui-btn-text\"> 加载更多...</span></span></a>");
                sbWhere.AppendFormat("<span id=\"btnNext\" isshow=\"yes\" style=\"display:none;\"> 加载更多...</span>");
                // sb.AppendFormat("<span id=\"btnNext\" class=\"ui-btn-text\"> 向下拖动加载更多...</span>");

            }
            return sbWhere.ToString();
        }

        private string ConverHtmlFormateTop(List<JuActivityInfo> dataList, bool isShowBtnNext = false)
        {

            StringBuilder sbWhere = new StringBuilder();
            if (dataList.Count.Equals(0))
            {
                return "";
            }
            sbWhere.AppendLine("<table  style=\"margin-top :5;margin-left:5;padding:10;width:100%;\">");
            sbWhere.AppendLine("<tbody>");
            for (int i = 0; i < dataList.Count; i++)
            {
                if (i == 0)
                {
                    //sb.AppendFormat(" <tr rel=\"{0}\" onclick=\"goto(this)\" style=\"background:url(/img/hb/hb1.jpg );width:100%;height:100px; repeat-x;\")\" >", list[i].ActivityWebsite);
                    sbWhere.AppendFormat(" <tr rel=\"{0}\" onclick=\"goto(this)\">", dataList[i].ActivityWebsite);
                    sbWhere.AppendFormat("<td colspan=\"2\" valign=\"bottom\" >");
                    sbWhere.AppendFormat("<div style=\"position:relative;\">");

                    //sb.AppendFormat("<img src=\"{0}\" class=\"topimg\" width:100% style=\"height:80px;margin-right:10px;\" />", list[0].TopImgPath);

                    sbWhere.AppendFormat("<img src=\"{0}\" class=\"topimg\" style=\"width:100%;height:auto;margin-right:10px;\" />", dataList[0].TopImgPath);



                    sbWhere.AppendFormat("<div style=\"width:99.9%;filter:alpha(Opacity=100);-moz-opacity:0.8;opacity: 0.8; background-color:#3C3C3C;position:absolute; bottom: 0px;\">");
                    sbWhere.AppendFormat("<span style=\"color:#FFFFFF;margin-left:5;text-shadow:none;\">");

                    sbWhere.AppendFormat(dataList[0].ActivityName);
                    sbWhere.AppendFormat("</span>");
                    sbWhere.AppendFormat("</div>");
                    sbWhere.AppendFormat("</div>");
                    sbWhere.AppendFormat("</td>");
                    sbWhere.AppendFormat(" </tr>");

                    sbWhere.AppendFormat("<tr><td colspan=\"2\"><hr /></td></tr>");


                }

                else
                {


                    sbWhere.AppendFormat(" <tr rel=\"{0}\" onclick=\"goto(this)\" ><td >", dataList[i].ActivityWebsite);
                    sbWhere.AppendFormat("<span style=\"font-size:100%;line-height:100%;color:Black\" >{0}</span>", dataList[i].ActivityName);
                    sbWhere.AppendLine("<br />");
                    sbWhere.AppendFormat("<div  style=\"font-size: 90%;line-height: 100%;margin-top:5px;\" >{0}</div>", string.Format("{0:f}", dataList[i].ActivityStartDate));
                    sbWhere.AppendLine("</td><td valign=\"top\" align=\"right\"  >");
                    sbWhere.AppendFormat("<img src=\"{0}\" style=\"width:50px;height:50px;margin-right:10px;\" /> ", dataList[i].ThumbnailsPath);
                    sbWhere.AppendLine("</td></tr>");
                    sbWhere.AppendFormat("<tr><td colspan=\"2\"><hr /></td></tr>");
                }


            }
            //            for (int i=0;i<dataList.Count;i++)
            //            {
            //                if (i==0)
            //                {
            //                  sb.AppendFormat(" <tr rel=\"{0}\" onclick=\"goto(this)\" >", dataList[i].ActivityWebsite);
            //    sb.AppendFormat("<td colspan=\"2\">") ;
            //    sb.AppendFormat("<div style=\"width:100%;\" >");
            //     sb.AppendFormat("<img src=\"{0}\" style=\"width:100%;height:80px;margin-right:10px;\" />",dataList[0].TopImgPath) ;
            //sb.AppendFormat("</div>");

            //    sb.AppendFormat("<div style=\"filter:alpha(Opacity=100);-moz-opacity:0.8;opacity: 0.8; background-color:#3C3C3C;overflow:hidden; \">");
            //    sb.AppendFormat("<span style=\"color:#FFFFFF;margin-left:5;\">");

            //     sb.AppendFormat(dataList[0].ActivityName);
            //    sb.AppendFormat("</span>");
            //     sb.AppendFormat("</div>");
            //      sb.AppendFormat("</td>");
            //    sb.AppendFormat(" </tr>");

            //  sb.AppendFormat("<tr><td colspan=\"2\"><hr /></td></tr>");


            //                }

            //                else
            //    {


            //                sb.AppendFormat(" <tr rel=\"{0}\" onclick=\"goto(this)\" ><td >", dataList[i].ActivityWebsite);
            //                sb.AppendFormat("<span style=\"font-size:100%;line-height:100%;color:Black\" >{0}</span>", dataList[i].ActivityName);
            //                sb.AppendLine("<br />");
            //                sb.AppendFormat("<div  style=\"font-size: 90%;line-height: 100%;\" >{0}</div>", string.Format("{0:f}", dataList[i].ActivityStartDate));
            //                sb.AppendLine("</td><td valign=\"top\" align=\"right\"  >");
            //                sb.AppendFormat("<img src=\"{0}\" style=\"width:50px;height:50px;margin-right:10px;\" /> ", dataList[i].ThumbnailsPath);
            //                sb.AppendLine("</td></tr>");
            //                sb.AppendFormat("<tr><td colspan=\"2\"><hr /></td></tr>");
            //            }


            //            }
            sbWhere.AppendLine("</tbody>");
            sbWhere.AppendLine("</table>");
            if (isShowBtnNext)
            {
                //sb.AppendFormat(" <tr><td colspan=\"2\"><input   data-theme=\"c\"  type=\"button\" id=\"btnNext\" value=\"加载更多...\"/></td></tr>");
                sbWhere.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" ></div>");
                //sb.AppendFormat("<a href=\"javascript:;\" data-role=\"button\" inline=\"true\" data-theme=\"c\" id=\"btnNext\" data-corners=\"true\" isshow=\"yes\" data-shadow=\"true\" data-iconshadow=\"true\" data-wrapperels=\"span\" class=\"ui-btn ui-shadow ui-btn-corner-all ui-btn-hover-c ui-btn-up-c\" ><span class=\"ui-btn-inner ui-btn-corner-all\"><span class=\"ui-btn-text\"> 加载更多...</span></span></a>");
                sbWhere.AppendFormat("<span id=\"btnNext\" isshow=\"yes\" style=\"display:none;\"> 加载更多...</span>");
                // sb.AppendFormat("<span id=\"btnNext\" class=\"ui-btn-text\"> 向下拖动加载更多...</span>");

            }
            return sbWhere.ToString();
        }

        private string ConverHtmlFormartJubit(List<JuActivityInfo> dataList, string highLight = "", bool isShowBtnNext = false)
        {

            StringBuilder sbWhere = new StringBuilder();
            if (dataList.Count.Equals(0))
            {
                return "";
            }
            sbWhere.AppendLine("<table  style=\"margin-top :5;margin-left:5;padding:10;width:100%;\">");
            sbWhere.AppendLine("<tbody>");
            foreach (var item in dataList)
            {
                if (!string.IsNullOrWhiteSpace(highLight))
                    item.ActivityName = item.ActivityName.Replace(highLight, "<span style=\"color:red;\">" + highLight + "</span>");

                sbWhere.AppendFormat(" <tr rel=\"/JuActivity/Wap/JuActivityDetail.aspx?id={0}\" onclick=\"goto(this)\" ><td >", item.JuActivityID);
                sbWhere.AppendFormat("<span style=\"font-size:100%;line-height:100%;color:Black\" >{0}</span>", item.ActivityName);
                sbWhere.AppendLine("<br />");
                sbWhere.AppendFormat("<div  style=\"font-size: 90%;line-height: 100%;margin-top:5px;\" >{0}</div>", string.Format("{0:f}", item.ActivityStartDate));
                sbWhere.AppendLine("</td><td valign=\"top\" align=\"right\"  >");
                sbWhere.AppendFormat("<img src=\"{0}\" style=\"width:50px;height:50px;margin-right:10px;\" /> ", item.ThumbnailsPath);
                sbWhere.AppendLine("</td></tr>");
                sbWhere.AppendFormat("<tr><td colspan=\"2\"><hr /></td></tr>");



            }
            sbWhere.AppendLine("</tbody>");
            sbWhere.AppendLine("</table>");
            if (isShowBtnNext)
            {
                //sb.AppendFormat(" <tr><td colspan=\"2\"><input   data-theme=\"c\"  type=\"button\" id=\"btnNext\" value=\"加载更多...\"/></td></tr>");
                sbWhere.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" ></div>");
                //sb.AppendFormat("<a href=\"javascript:;\" data-role=\"button\" inline=\"true\" data-theme=\"c\" id=\"btnNext\" data-corners=\"true\" isshow=\"yes\" data-shadow=\"true\" data-iconshadow=\"true\" data-wrapperels=\"span\" class=\"ui-btn ui-shadow ui-btn-corner-all ui-btn-hover-c ui-btn-up-c\" ><span class=\"ui-btn-inner ui-btn-corner-all\"><span class=\"ui-btn-text\"> 加载更多...</span></span></a>");
                sbWhere.AppendFormat("<span id=\"btnNext\" isshow=\"yes\" style=\"display:none;\"> 加载更多...</span>");
                // sb.AppendFormat("<span id=\"btnNext\" class=\"ui-btn-text\"> 向下拖动加载更多...</span>");

            }
            return sbWhere.ToString();
        }


        ///// <summary>
        ///// 专家库留言查询
        ///// </summary>
        ///// <param name="dataList"></param>
        ///// <returns></returns>
        //private string ConverHtmlFormateMasterFeedBack(List<JuMasterFeedBack> dataList, int rows, bool isShowBtnNext = false)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (dataList.Count == 0)
        //    {
        //        return "";
        //    }
        //    foreach (var item in dataList)
        //    {
        //        sb.AppendFormat("<div style=\"border: 1px solid #CCC;margin-top:10px;\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\" rel=\"/JuActivity/Wap/JuMasterFeedBackDialog.aspx?feedbackid={0}\" onclick=\"GotoRel(this)\" >", item.FeedBackID);
        //        sb.AppendLine("  <div style=\"font-family: Helvetica,Arial,sans-serif;text-align: left;font-weight: bold;background-color: #E7E7E7;padding: 5px;font-size: 16px;color: #930;\">");

        //        sb.AppendFormat("{0} 发表于 {1}", item.UserNickName, item.SubmitDate.ToString("yyyy-MM-dd HH:mm:ss"));

        //        sb.AppendLine("</div>");

        //        sb.AppendLine("<div style=\"font-family: Helvetica,Arial,sans-serif;margin-left:5px;margin-top:10px;font-size: 16px;color: #666666;line-height: 18px;\">");

        //        sb.Append(item.FeedBackContent);
        //        // 请问因学习需要可以自带笔记本电脑在图书馆无线上网查资料吗?（总馆和分馆都行吗）
        //        sb.AppendLine("</div>");



        //        var FeedBackDialogue = juActivityBll.Get<JuMasterFeedBackDialogue>(string.Format("FeedBackID={0}", item.FeedBackID));
        //        if (FeedBackDialogue != null)
        //        {
        //            sb.AppendLine("  <div style=\"font-family: Helvetica,Arial,sans-serif;margin-left:5px;margin-top:10px;text-align: left;font-weight: bold;font-size: 16px;color:#930;\"> ");
        //            sb.AppendFormat("专家回复 {0}", FeedBackDialogue.SubmitDate.ToString("yyyy-MM-dd HH:mm:ss"));
        //            //管理员回复 2013-10-21 13:12:25
        //            sb.AppendLine("</div>");
        //            sb.AppendLine("<div style=\"margin-left:5px;margin-top:10px;font-size: 16px;color: #666666;line-height: 18px;\">");
        //            sb.Append(FeedBackDialogue.DialogueContent);
        //            //可以的。

        //            sb.AppendLine("</div>");



        //        }


        //        sb.AppendLine("</div>");


        //    }
        //    if (isShowBtnNext)
        //    {
        //        sb.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" style=\"display:none;\"></div>");
        //        sb.AppendFormat("<div id=\"btnNext\" isshow=\"yes\" style=\"display:block;text-align:center;font-family: Helvetica,Arial,sans-serif;font-size: 12px;color: #666666;line-height: 18px;\"> 向上拉动显示下{0}条</div>", rows);

        //    }


        //    return sb.ToString();


        //}

        ///// <summary>
        ///// 专家库留言详细对话框查询
        ///// </summary>
        ///// <param name="dataList"></param>
        ///// <returns></returns>
        //private string ConverHtmlFormateMasterFeedBackDialog(List<JuMasterFeedBackDialogue> dataList)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (dataList.Count == 0)
        //    {
        //        return "";
        //    }
        //    foreach (var item in dataList)
        //    {
        //        sb.AppendFormat("<div style=\"border: 1px solid #CCC;margin-top:10px;\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\" >", item.FeedBackID);
        //        sb.AppendLine("  <div style=\"font-family: Helvetica,Arial,sans-serif;text-align: left;font-weight: bold;background-color: #E7E7E7;padding: 5px;font-size: 16px;color: #930;\">");

        //        sb.AppendFormat("{0} : {1}", item.UserID, item.SubmitDate.ToString("yyyy-MM-dd HH:mm:ss"));

        //        sb.AppendLine("</div>");

        //        sb.AppendLine("<div style=\"font-family: Helvetica,Arial,sans-serif;margin-left:5px;margin-top:10px;font-size: 16px;color: #666666;line-height: 18px;\">");

        //        sb.Append(item.DialogueContent);

        //        sb.AppendLine("</div>");




        //        sb.AppendLine("</div>");


        //    }



        //    return sb.ToString();


        //}



        ///// <summary>
        ///// 联系人信息
        ///// </summary>
        ///// <param name="dataList"></param>
        ///// <returns></returns>
        //private string ConverHtmlFormateMasterUserLinkerInfo(List<JuMasterUserLinkerInfo> dataList, int rows, bool isShowBtnNext = false)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (dataList.Count == 0)
        //    {
        //        return "";
        //    }
        //    foreach (var item in dataList)
        //    {
        //        sb.AppendLine("<div class=\"msg-item-wrappertop\">");
        //        sb.AppendLine("<div class=\"msg-item multi-msg\" >");

        //        sb.AppendLine("<div style=\"padding:10px;\">");

        //        sb.AppendLine("<div style=\"font-weight:bold;\">");//SubmitDate
        //        sb.AppendLine(string.Format("{0:f}", item.SubmitDate));
        //        sb.AppendLine("</div>");

        //        sb.AppendLine("<div style=\"margin-top:5px;\">");//Name
        //        sb.AppendFormat("<label style=\"font-weight:bold;\" >姓名:</label>{0}", item.Name);
        //        sb.AppendLine("</div>");


        //        sb.AppendLine("<div style=\"margin-top:5px;\">");//Company
        //        sb.AppendFormat("<label style=\"font-weight:bold;\" >公司:</label>{0}", item.Company);
        //        sb.AppendLine("</div>");

        //        sb.AppendLine("<div style=\"margin-top:5px;\">");//Title
        //        sb.AppendFormat("<label style=\"font-weight:bold;\" >职位:</label>{0}", item.Title);
        //        sb.AppendLine("</div>");

        //        sb.AppendLine("<div style=\"margin-top:5px;\">");//Mobile
        //        sb.AppendFormat("<label style=\"font-weight:bold;\" >手机号码:</label>{0}", item.Mobile);
        //        sb.AppendLine("</div>");

        //        sb.AppendLine("<div style=\"margin-top:5px;\">");//Email
        //        sb.AppendFormat("<label style=\"font-weight:bold;\" >Email:</label>{0}", item.Email);
        //        sb.AppendLine("</div>");

        //        sb.AppendLine("<div style=\"margin-top:5px;\">");//OtherDescription
        //        sb.AppendFormat("<label style=\"font-weight:bold;\" >其它说明:</label>{0}", item.OtherDescription);
        //        sb.AppendLine("</div>");

        //        sb.AppendLine("</div>");

        //        sb.AppendLine("</div>");
        //        sb.AppendLine("</div>");





        //    }
        //    if (isShowBtnNext)
        //    {
        //        sb.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" style=\"display:none;\"></div>");
        //        sb.AppendFormat("<div id=\"btnNext\" isshow=\"yes\" style=\"display:block;text-align:center;font-family: Helvetica,Arial,sans-serif;font-size: 12px;color: #666666;line-height: 18px;\"> 向上拉动显示下{0}条</div>", rows);

        //    }


        //    return sb.ToString();


        //}

        private string GetJuActivityInfoList(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            var articletype = context.Request["ArticleType"];
            List<JuActivityInfo> dataList = new List<JuActivityInfo>();
            int totalCount = 0;

            dataList = this.bllJuactivity.QueryJuActivityData(
                null,out totalCount, null, null, null, null, null, pageIndex, pageSize,
                currentUserInfo.UserType.Equals(1) ? "" : currentUserInfo.UserID,
                null,
                articletype);

            int totalPage = this.bllJuactivity.GetTotalPage(totalCount, pageSize);

            if (articletype.Equals("activity"))
            {
                return ConverHtmlFormateActivityInfo(dataList, pageSize, ((totalPage > pageIndex) && pageIndex == 1), pageIndex);//
            }
            else if (articletype.Equals("greetingcard"))
            {
                return ConverHtmlFormateGreetingCard(dataList, pageSize, ((totalPage > pageIndex) && pageIndex == 1), pageIndex);//
            }else
            {
                return ConverHtmlFormateArticle(dataList, pageSize, ((totalPage > pageIndex) && pageIndex == 1), pageIndex);//
            }

        }

        private string GetJuActivityInfoListAll(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            var articleType = context.Request["ArticleType"];

            List<JuActivityInfo> dataList = new List<JuActivityInfo>();
            int totalCount = 0;


            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(string.Format("IsHide=0 and IsDelete = 0 and ArticleType='{1}'", articleType));


            dataList = this.bllJuactivity.GetLit<JuActivityInfo>(pageSize, pageIndex, sbWhere.ToString());

            totalCount = this.bllJuactivity.GetCount<JuActivityInfo>(sbWhere.ToString());

            int totalpage = this.bllJuactivity.GetTotalPage(totalCount, pageSize);

            if (articleType.Equals("activity"))
            {
                return ConverHtmlFormateActivityInfo(dataList, pageSize, ((totalpage > pageIndex) && pageIndex == 1), pageIndex);//


            }
            else
            {

                return ConverHtmlFormateArticle(dataList, pageSize, ((totalpage > pageIndex) && pageIndex == 1), pageIndex);//
            }
        }

        /// <summary>
        /// 格式化活动列表
        /// </summary>
        /// <param name="memberIDHex"></param>
        /// <param name="dataList"></param>
        /// <param name="rows"></param>
        /// <param name="isShowBtnNext"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        private string ConverHtmlFormateActivityInfo(List<JuActivityInfo> dataList, int rows, bool isShowBtnNext = false, int pageIndex = 1)
        {

            StringBuilder sbWhere = new StringBuilder();
            if (dataList.Count.Equals(0))
            {
                return "";
            }

            foreach (var item in dataList)
            {

                //sb.AppendFormat("<li data-corners=\"false\" data-shadow=\"false\" data-iconshadow=\"true\" data-wrapperels=\"div\" data-icon=\"false\" data-iconpos=\"right\" data-theme=\"b\" class=\"ui-btn ui-btn-icon-right ui-li ui-li-has-alt ui-li-has-thumb ui-last-child ui-btn-hover-b\"><div class=\"ui-btn-inner ui-li ui-li-has-alt\"><div class=\"ui-btn-text\"><a href=\"#\" onclick=\"window.location.href=\'/{0}/share.chtml\'\" class=\"ui-link-inherit\">", item.JuActivityIDHex);
                //sb.AppendFormat("<img src=\"{0}\" class=\"ui-li-thumb\">", item.ThumbnailsPath);

                //sb.AppendLine("<h2 class=\"ui-li-heading\">");
                //sb.AppendLine(item.ActivityName);
                //sb.AppendLine("</h2>");
                //sb.AppendLine("<p class=\"ui-li-desc\">");
                ////sb.AppendLine(string.Format("{0:f}", item.ActivityStartDate));
                //sb.AppendFormat("<font color='red'>{0}</font>IP/<font color='red'>{1}</font>PV", item.IP, item.PV);
                //sb.AppendFormat("&nbsp;<font color='red'>{0}</font>次分享", item.ShareTotalCount);
                //sb.AppendFormat("&nbsp;<font color='red'>{0}</font>人报名", item.SignUpTotalCount);

                //sb.AppendLine("</p>");

                //sb.AppendFormat("</a> </div></div><a href=\"{0}\" title=\"查看报名用户\" class=\"ui-li-link-alt ui-btn ui-btn-up-b ui-btn-icon-notext\" data-corners=\"false\" data-shadow=\"false\" data-iconshadow=\"true\" data-wrapperels=\"span\" data-icon=\"false\" data-iconpos=\"notext\" data-theme=\"b\"><span class=\"ui-btn-inner\"><span class=\"ui-btn-text\"></span><span data-corners=\"true\" data-shadow=\"true\" data-iconshadow=\"true\" data-wrapperels=\"span\" data-icon=\"bars\" data-iconpos=\"notext\" data-theme=\"d\" title=\"\" class=\"ui-btn ui-btn-up-d ui-shadow ui-btn-corner-all ui-btn-icon-notext\"><span class=\"ui-btn-inner\"><span class=\"ui-btn-text\"></span><span class=\"ui-icon ui-icon-bars ui-icon-shadow\">&nbsp;</span></span></span></span></a></li>",
                //        "/Fshare/Wap/ActivityStatistics.aspx?jid=" + item.JuActivityIDHex
                //    );

                sbWhere.AppendFormat(@"	<li data-role=""list-divider"" role=""heading"" class=""ui-li ui-li-divider ui-bar-b ui-li-has-count ui-first-child"">{0}</li>",item.ActivityName);

                sbWhere.AppendFormat(@"<li data-corners=""false"" data-shadow=""false"" data-iconshadow=""true"" data-wrapperels=""div"" data-icon=""arrow-r"" data-iconpos=""right"" data-theme=""c"" class=""ui-btn ui-btn-icon-right ui-li-has-arrow ui-li ui-btn-up-c""><div class=""ui-btn-inner ui-li""><div class=""ui-btn-text""><a href=""javascript:;"" onclick=""window.location.href=\'/{3}/share.chtml\'"" class=""ui-link-inherit"">
						<h2 class=""ui-li-heading""><font color='red'>{0}</font>IP/<font color='red'>{1}</font>PV&nbsp;<font color='red'>{2}</font>次分享</h2>
						<p class=""ui-li-desc""><strong>点击进入页面转发</strong></p>
					</a></div><span class=""ui-icon ui-icon-arrow-r ui-icon-shadow"">&nbsp;</span></div></li>",item.IP,item.PV,item.ShareTotalCount,item.JuActivityIDHex);

                sbWhere.AppendFormat(@"<li data-corners=""false"" data-shadow=""false"" data-iconshadow=""true"" data-wrapperels=""div"" data-icon=""arrow-r"" data-iconpos=""right"" data-theme=""c"" class=""ui-btn ui-btn-icon-right ui-li-has-arrow ui-li ui-btn-up-c""><div class=""ui-btn-inner ui-li""><div class=""ui-btn-text""><a href=""{1}"" class=""ui-link-inherit"">
						<h2 class=""ui-li-heading"">已有<font color='red'>{0}</font>人报名</h2>
						<p class=""ui-li-desc""><strong>点击进入报名管理</strong></p>
					</a></div><span class=""ui-icon ui-icon-arrow-r ui-icon-shadow"">&nbsp;</span></div></li>",
                        item.SignUpTotalCount,
                        "/Fshare/Wap/ActivityStatistics.aspx?jid=" + item.JuActivityIDHex
                        );

            }
            if (isShowBtnNext)
            {
                sbWhere.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" style=\"display:none;\" ></div>");
                sbWhere.AppendFormat("<div id=\"btnNext\" isshow=\"yes\" style=\"display:block;text-align:center;font-family: Helvetica,Arial,sans-serif;font-size: 12px;color: #666666;line-height: 18px;\"> 向上拉动显示下{0}条</div>", rows);


            }
            return sbWhere.ToString();
        }


        /// <summary>
        /// 格式化文章列表
        /// </summary>
        /// <param name="memberIDHex"></param>
        /// <param name="dataList"></param>
        /// <param name="rows"></param>
        /// <param name="isShowBtnNext"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        private string ConverHtmlFormateArticle(List<JuActivityInfo> dataList, int rows, bool isShowBtnNext = false, int pageIndex = 1)
        {

            StringBuilder sbWhere = new StringBuilder();
            if (dataList.Count.Equals(0))
            {
                return "";
            }
            foreach (var item in dataList)
            {

                sbWhere.AppendFormat("<li data-corners=\"false\" data-shadow=\"false\" data-iconshadow=\"true\" data-wrapperels=\"div\" data-icon=\"false\" data-iconpos=\"right\" data-theme=\"b\" class=\"ui-btn ui-btn-icon-right ui-li ui-li-has-alt ui-li-has-thumb ui-last-child ui-btn-hover-b\"><div class=\"ui-btn-inner ui-li ui-li-has-alt\"><div class=\"ui-btn-text\"><a href=\"#\" class=\"ui-link-inherit\">");
                sbWhere.AppendLine(string.Format("<div onclick=\"window.location.href=\'/{0}/share.chtml\'\">", item.JuActivityIDHex));
                sbWhere.AppendFormat("<img src=\"{0}\" class=\"ui-li-thumb\">", item.ThumbnailsPath);
                sbWhere.AppendLine("<h2 class=\"ui-li-heading\">");
                sbWhere.AppendLine(item.ActivityName);
                sbWhere.AppendLine("</h2>");
                sbWhere.AppendLine("<p class=\"ui-li-desc\">");
                //sb.AppendLine(string.Format("{0:f}", item.ActivityStartDate));
                //sb.AppendFormat("&nbsp;&nbsp;已有<font color='red'>8</font>人阅读");
                sbWhere.AppendFormat("<font color='red'>{0}</font>IP/<font color='red'>{1}</font>PV", item.IP, item.PV);
                sbWhere.AppendFormat("&nbsp;<font color='red'>{0}</font>次分享", item.ShareTotalCount);

                sbWhere.AppendLine("</p>");
                sbWhere.AppendLine("</div>");
                sbWhere.AppendFormat("</a> </div></div></li>");
                //sb.AppendFormat("</a> </div></div><a href=\"#\" title=\"\" class=\"ui-li-link-alt ui-btn ui-btn-up-b ui-btn-icon-notext\" data-corners=\"false\" data-shadow=\"false\" data-iconshadow=\"true\" data-wrapperels=\"span\" data-icon=\"false\" data-iconpos=\"notext\" data-theme=\"b\"><span class=\"ui-btn-inner\"><span class=\"ui-btn-text\"></span><span data-corners=\"true\" data-shadow=\"true\" data-iconshadow=\"true\" data-wrapperels=\"span\" data-icon=\"gear\" data-iconpos=\"notext\" data-theme=\"d\" title=\"\" class=\"ui-btn ui-btn-up-d ui-shadow ui-btn-corner-all ui-btn-icon-notext\"><span class=\"ui-btn-inner\"><span class=\"ui-btn-text\"></span><span class=\"ui-icon ui-icon-gear ui-icon-shadow\">&nbsp;</span></span></span></span></a></li>");

            }
            if (isShowBtnNext)
            {
                sbWhere.AppendFormat("<div class=\"progressBar\" id=\"progressBarArticle\" style=\"display:none;\" ></div>");
                sbWhere.AppendFormat("<div id=\"btnNextArticle\" isshow=\"yes\" style=\"display:block;text-align:center;font-family: Helvetica,Arial,sans-serif;font-size: 12px;color: #666666;line-height: 18px;\"> 向上拉动显示下{0}条</div>", rows);


            }
            return sbWhere.ToString();
        }

        /// <summary>
        /// 格式化贺卡列表
        /// </summary>
        /// <param name="memberIDHex"></param>
        /// <param name="dataList"></param>
        /// <param name="rows"></param>
        /// <param name="isShowBtnNext"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        private string ConverHtmlFormateGreetingCard(List<JuActivityInfo> dataList, int rows, bool isShowBtnNext = false, int pageIndex = 1)
        {

            StringBuilder sb = new StringBuilder();
            if (dataList.Count.Equals(0))
            {
                if (pageIndex.Equals(1))
                {
                    return "暂时没有贺卡";
                }
                return "";
            }
            foreach (var item in dataList)
            {
                sb.AppendFormat("<li data-corners=\"false\" data-shadow=\"false\" data-iconshadow=\"true\" data-wrapperels=\"div\" data-icon=\"false\" data-iconpos=\"right\" data-theme=\"b\" class=\"ui-btn ui-btn-icon-right ui-li ui-li-has-alt ui-li-has-thumb ui-last-child ui-btn-hover-b\"><div class=\"ui-btn-inner ui-li ui-li-has-alt\"><div class=\"ui-btn-text\"><a href=\"#\" onclick=\"window.location.href=\'/{0}/share.chtml\'\" class=\"ui-link-inherit\">", item.JuActivityIDHex);
                sb.AppendFormat("<img src=\"{0}\" class=\"ui-li-thumb\">", item.ThumbnailsPath);

                sb.AppendLine("<h2 class=\"ui-li-heading\">");
                sb.AppendLine(item.ActivityName);
                sb.AppendLine("</h2>");
                sb.AppendLine("<p class=\"ui-li-desc\">");
                sb.AppendFormat("&nbsp;<font color='red'>{0}</font>人查看", item.IP);
                sb.AppendFormat("&nbsp;<font color='red'>{0}</font>人回复", item.SignUpTotalCount);
                sb.AppendLine("</p>");
                sb.AppendFormat("</a> </div></div><a href=\"{0}\" title=\"查看回复列表\" class=\"ui-li-link-alt ui-btn ui-btn-up-b ui-btn-icon-notext\" data-corners=\"false\" data-shadow=\"false\" data-iconshadow=\"true\" data-wrapperels=\"span\" data-icon=\"false\" data-iconpos=\"notext\" data-theme=\"b\"><span class=\"ui-btn-inner\"><span class=\"ui-btn-text\"></span><span data-corners=\"true\" data-shadow=\"true\" data-iconshadow=\"true\" data-wrapperels=\"span\" data-icon=\"bars\" data-iconpos=\"notext\" data-theme=\"d\" title=\"\" class=\"ui-btn ui-btn-up-d ui-shadow ui-btn-corner-all ui-btn-icon-notext\"><span class=\"ui-btn-inner\"><span class=\"ui-btn-text\"></span><span class=\"ui-icon ui-icon-bars ui-icon-shadow\">&nbsp;</span></span></span></span></a></li>",
                        "/App/Cation/Wap/GreetingCardStatistics.aspx?jid=" + item.JuActivityIDHex
                    );
            }
            if (isShowBtnNext)
            {
                sb.AppendFormat("<div class=\"progressBar\" id=\"progressBarGreetingCard\" style=\"display:none;\" ></div>");
                sb.AppendFormat("<div id=\"btnNextGreetingCard\" isshow=\"yes\" style=\"display:block;text-align:center;font-family: Helvetica,Arial,sans-serif;font-size: 12px;color: #666666;line-height: 18px;\"> 向上拉动显示下{0}条</div>", rows);
            }
            return sb.ToString();
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