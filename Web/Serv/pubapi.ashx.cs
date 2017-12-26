using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using ZentCloud.JubitIMP.Web.Handler;
using ZentCloud.BLLJIMP.Model.API.forbes;
using ZentCloud.BLLJIMP.Model;
using System.Text;
using ZentCloud.BLLJIMP;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using CommonPlatform.Helper;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// 对外api
    /// </summary>
    public class pubapi : IHttpHandler, IRequiresSessionState
    {
        #region 模块对象
        DefaultResponse resp = new DefaultResponse();
        ///// <summary>
        ///// 网站所有者
        ///// </summary>
        //private string bll.WebsiteOwner;
        /// <summary>
        /// 活动业务逻辑
        /// </summary>
        BLLJIMP.BLLJuActivity bll = new BLLJIMP.BLLJuActivity();
        /// <summary>
        /// 用户业务逻辑
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        /// <summary>
        /// 专家业务逻辑
        /// </summary>
        BLLTutor bllTutor = new BLLTutor();

        /// <summary>
        /// 字典业务逻辑（省市区）
        /// </summary>
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        /// <summary>
        /// 用户扩展模块
        /// </summary>
        BLLUserExpand bllUserExpand = new BLLUserExpand();
        /// <summary>
        /// 消息中心模块
        /// </summary>
        BLLSystemNotice bllSystemNotice = new BLLSystemNotice();

        /// <summary>
        /// 评论回复模块
        /// </summary>
        BLLReview bllReview = new BLLReview();
        /// <summary>
        /// 短信业务逻辑
        /// </summary>
        BLLSMS bllSms = new BLLSMS("");
        /// <summary>
        /// 通用关系业务
        /// </summary>
        BLLCommRelation bLLCommRelation = new BLLCommRelation();
        /// <summary>
        /// 标签业务逻辑
        /// </summary>
        BLLTag bllTag = new BLLTag();
        /// <summary>
        /// 文章分类业务逻辑
        /// </summary>
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        /// <summary>
        /// 基路径 形式如 http://dev.comeoncloud.net
        /// </summary>
        //private string basePath;
        /// <summary>
        /// 当前请求参数键值对
        /// </summary>
        Dictionary<string, string> dicPar;
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        /// <summary>
        /// 真实活动
        /// </summary>
        BLLJIMP.BLLActivity bllTrueActivity = new BLLJIMP.BLLActivity("");
        /// <summary>
        /// 系统通知
        /// </summary>
        BLLSystemNotice bllNotice = new BLLSystemNotice();
        /// <summary>
        /// 车型
        /// </summary>
        BLLCarLibrary bllCar = new BLLCarLibrary();
        #endregion

        #region 入口
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                if (bll.IsLogin)
                {
                    currentUserInfo = bll.GetCurrentUserInfo();
                }
                //bll.WebsiteOwner = bll.WebsiteOwner;
                //basePath = string.Format("http://{0}{1}", context.Request.Url.Host, context.Request.Url.Port != 80 ? ":" + context.Request.Url.Port.ToString() : "");
                string action = context.Request["action"];
                TologTemp(action);
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    if (method == null)
                    {
                        resp.errmsg = "action not exist";
                        result = Common.JSONHelper.ObjectToJson(resp);
                        context.Response.Write(result);
                        return;
                    }
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.errmsg = "action not exist";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {

                resp.errcode = -1;
                resp.errmsg = ex.ToString();
                result = Common.JSONHelper.ObjectToJson(resp);
            }
            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                context.Response.Write(string.Format("{0}({1})", context.Request["callback"], result));
            }
            else
            {
                //返回json数据
                context.Response.Write(result);
            }

        }
        #endregion


        private void TologTemp(string msg)
        {
            //if (File.Exists(@"D:\hzhtest.txt"))
            //{
            //    using (StreamWriter sw = new StreamWriter(@"D:\hzhtest.txt", true, Encoding.GetEncoding("gb2312")))
            //    {
            //        sw.WriteLine(DateTime.Now.ToString());
            //        sw.WriteLine(msg);
            //    }
            //}

        }

        /// <summary>
        /// 提交分享
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ShareSubmit(HttpContext context)
        {
            TologTemp("进入ShareSubmit");
            string
                    result = "",
                    url = context.Request["url"],
                    shareId = context.Request["shareId"],
                    preId = context.Request["preId"],
                    userId = context.Request["userId"],
                    userWxOpenId = context.Request["userWxOpenId"],
                    wxMsgType = context.Request["wxMsgType"];

            try
            {
                BLLShareMonitor bllShareMonitor = new BLLShareMonitor();
                
                //判断shareid是否已有，已有则不新增
                if (bllShareMonitor.ExistsShareInfo(shareId))
                {
                    return result;
                }

                ShareMonitorInfo monitorInfo = bllShareMonitor.GetMonitorByUrl(url);

                int monitorId = 0;

                if (monitorInfo != null)
                {
                    monitorId = monitorInfo.MonitorId;
                }

                if (currentUserInfo != null)
                {
                    if (string.IsNullOrWhiteSpace(userId))
                    {
                        userId = currentUserInfo.UserID;
                    }
                    if (string.IsNullOrWhiteSpace(userWxOpenId))
                    {
                        userWxOpenId = currentUserInfo.WXOpenId;
                    }
                }

                //到到监测任务取出任务id记录

                ShareInfo shareModel = new ShareInfo()
                {
                    MonitorId = monitorId,
                    PreId = preId,
                    ShareId = shareId,
                    ShareTime = DateTime.Now,
                    Url = url,
                    UserId = userId,
                    UserWxOpenId = userWxOpenId,
                    WxMsgType = wxMsgType
                };

                this.bll.Add(shareModel);

            }
            catch (Exception ex)
            {
                TologTemp(ex.Message);
            }
            return result;
        }


        #region 活动模块
        /// <summary>
        /// 获取活动分类列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetActivityCategoryList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            CategoryApi apiResult = new CategoryApi();
            apiResult.list = new List<Category>();

            var sourceData = bll.GetLit<ArticleCategory>(pageSize, pageIndex, string.Format(" CategoryType='activity' And WebsiteOwner='{0}' ", bll.WebsiteOwner));
            apiResult.totalcount = bll.GetCount<ArticleCategory>(string.Format(" CategoryType='activity' And WebsiteOwner='{0}' ", bll.WebsiteOwner));
            foreach (var item in sourceData)
            {
                Category model = new Category();
                model.categoryid = item.AutoID;
                model.categoryname = item.CategoryName;
                apiResult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetActivityList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);

            int year = 0, month = 0;
            if (int.TryParse(context.Request["year"], out year))
            {

            }
            if (int.TryParse(context.Request["month"], out month))
            {

            }


            string sort = context.Request["sort"];
            string cateid = context.Request["cateid"];
            string keyword = context.Request["keyword"];
            return QueryActivityList(pageIndex, pageSize, sort, cateid, keyword, false, year, month);

        }

        /// <summary>
        /// 获取报名人列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSignPersonList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            int activityId = int.Parse(context.Request["activityid"]);
            JuActivityInfo juInfo = bll.GetJuActivity(activityId, true);
            SignPersonApi apiResult = new SignPersonApi();
            apiResult.list = new List<SignPerson>();
            apiResult.totalcount = bll.GetCount<ActivityDataInfo>(string.Format("ActivityID='{0}' And WebsiteOwner='{1}' AND IsDelete=0", juInfo.SignUpActivityID, bll.WebsiteOwner));
            var SourceData = bll.GetLit<ActivityDataInfo>(pageSize, pageIndex, string.Format("ActivityID='{0}' And WebsiteOwner='{1}' AND IsDelete=0", juInfo.SignUpActivityID, bll.WebsiteOwner), " InsertDate DESC");
            foreach (var item in SourceData)
            {
                var userInfo = this.bllUser.GetUserInfo(item.UserId);
                if (userInfo == null)
                {
                    userInfo = this.bllUser.GetUserInfoByOpenId(item.WeixinOpenID);
                }
                SignPerson model = new SignPerson();

                model.name = item.Name;
                model.time = bll.GetTimeStamp(item.InsertDate);
                model.signupTime = item.InsertDate.ToString();

                model.headimg = bll.GetImgUrl("/img/europejobsites.png");

                if (userInfo != null)
                {
                    model.userId = userInfo.UserID;
                    model.openId = userInfo.WXOpenId;
                    model.headimg = this.bllUser.GetUserDispalyAvatar(userInfo);
                }

                if (juInfo.ShowPersonnelListType.Equals(1))
                {
                    model.name = model.name.Substring(0, 1) + "**";
                }
                apiResult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 获取活动详情
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetActivityDetail(HttpContext context)
        {

            int activityId = int.Parse(context.Request["activityid"]);
            JuActivityInfo juInfo = bll.GetJuActivity(activityId, true);
            juInfo.PV++;
            bll.Update(juInfo);
            #region 记录 点击IP
            MonitorPlan planInfo = bll.Get<MonitorPlan>(string.Format("MonitorPlanID={0}", juInfo.MonitorPlanID));
            if (planInfo != null && planInfo.PlanStatus != "0")
            {
                MonitorEventDetailsInfo detailInfo = new MonitorEventDetailsInfo();
                detailInfo.MonitorPlanID = juInfo.MonitorPlanID;
                detailInfo.EventType = 0;
                if (HttpContext.Current.Request.Browser != null)
                {
                    detailInfo.EventBrowser = HttpContext.Current.Request.Browser.ToString();
                    detailInfo.EventBrowserID = HttpContext.Current.Request.Browser.Id;
                    if (HttpContext.Current.Request.Browser.Beta)
                    {
                        detailInfo.EventBrowserIsBata = "测试版";
                    }
                    else
                    {
                        detailInfo.EventBrowserIsBata = "正式版";
                    }
                    detailInfo.EventBrowserVersion = HttpContext.Current.Request.Browser.Version;

                    if (HttpContext.Current.Request.Browser.Win16)
                        detailInfo.EventSysByte = "16位系统";
                    else
                        if (HttpContext.Current.Request.Browser.Win32)
                            detailInfo.EventSysByte = "32位系统";
                        else
                            detailInfo.EventSysByte = "64位系统";

                    detailInfo.EventSysPlatform = HttpContext.Current.Request.Browser.Platform;
                }

                detailInfo.EventDate = DateTime.Now;

                detailInfo.SourceIP = Common.MySpider.GetClientIP();
                detailInfo.IPLocation = Common.MySpider.GetIPLocation(detailInfo.SourceIP);
                detailInfo.SourceUrl = HttpContext.Current.Request.Url.ToString();
                detailInfo.WebsiteOwner = bll.WebsiteOwner;
                if (bll.Add(detailInfo))
                {
                    bll.UpdateIPCount(activityId);
                }
            }
            #endregion
            ActivityDetail apiResult = new ActivityDetail();
            apiResult.signfield = new List<SignField>();
            apiResult.activityid = juInfo.JuActivityID;
            apiResult.activityimage = bll.GetImgUrl(juInfo.ThumbnailsPath);
            apiResult.activityname = juInfo.ActivityName;
            apiResult.address = juInfo.ActivityAddress;
            apiResult.categoryname = juInfo.CategoryName;
            apiResult.pv = juInfo.PV;
            apiResult.signcount = juInfo.SignUpTotalCount;
            if (juInfo.ActivityStartDate != null)
            {
                apiResult.time = bll.GetTimeStamp((DateTime)juInfo.ActivityStartDate);
            }
            if (juInfo.IsHide == 1)
            {
                apiResult.status = 1;
            }
            if ((juInfo.MaxSignUpTotalCount > 0) && (juInfo.SignUpTotalCount >= juInfo.MaxSignUpTotalCount))
            {
                apiResult.status = 2;
            }
            apiResult.activitycontent = juInfo.ActivityDescription;
            if (juInfo.ActivityDescription.Contains("/FileUpload/"))
            {
                apiResult.activitycontent = juInfo.ActivityDescription.Replace("/FileUpload/", string.Format("http://{0}/FileUpload/", context.Request.Url.Host));
            }
            apiResult.score = juInfo.ActivityIntegral;

            apiResult.commentcount = bllReview.GetReviewCount(BLLJIMP.Enums.ReviewTypeKey.ArticleComment, juInfo.JuActivityID.ToString(), null);
            //result.CommentCount = new BLLReview().GetReviewCount(Enums.ReviewTypeKey.ArticleComment, item.JuActivityID.ToString());

            //BLLJIMP.BLLActivity bllTrueActivity = new BLLJIMP.BLLActivity("");
            var fieldList = bllTrueActivity.GetActivityFieldMappingList(juInfo.SignUpActivityID);
            foreach (var item in fieldList)
            {
                SignField model = new SignField();
                model.key = item.MappingName;
                model.value = item.FieldName;
                //TODO:增加是否可为空字段返回
                model.isnull = item.FieldIsNull;
                apiResult.signfield.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 提交报名数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SubmitActivitySignData(HttpContext context)
        {
            int activityId = int.Parse(context.Request["activityid"]);
            JuActivityInfo juInfo = bll.GetJuActivity(activityId, true);
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                goto outoff;

            }
            if (juInfo == null)
            {
                resp.errcode = 4;
                resp.errmsg = "活动不存在!";
                goto outoff;

            }
            #region 是否可以报名
            if (juInfo.ActivityStatus.Equals(1))
            {
                resp.errcode = 2;
                resp.errmsg = "活动已停止";
                goto outoff;

            }
            if (juInfo.MaxSignUpTotalCount > 0)//检查报名人数
            {
                if (juInfo.SignUpTotalCount > (juInfo.MaxSignUpTotalCount - 1))
                {
                    resp.errcode = 3;
                    resp.errmsg = "报名人数已满";
                    goto outoff;

                }

            }
            if (juInfo.ActivityIntegral > 0)
            {
                if (currentUserInfo.TotalScore < juInfo.ActivityIntegral)
                {
                    resp.errcode = 4;
                    resp.errmsg = "您的积分不足";
                    goto outoff;

                }

            }
            #endregion
            dicPar = bll.GetRequestParameter();
            string weixinOpenID = null;
            string signUpActivityID = juInfo.SignUpActivityID;
            string spreadUserID = null;
            dicPar.TryGetValue("SpreadUserID", out spreadUserID);
            string strDistinctKeys = null;//检查重复的字段，多个字段用,分隔， //没有此参数默认用手机检查  
            dicPar.TryGetValue("DistinctKeys", out strDistinctKeys);
            string monitorPlanID = null;
            dicPar.TryGetValue("MonitorPlanID", out monitorPlanID);
            string name = null;
            dicPar.TryGetValue("Name", out name);
            string phone = null;
            dicPar.TryGetValue("Phone", out phone);
            ActivityInfo activity = bll.Get<ActivityInfo>(string.Format("ActivityID='{0}'", signUpActivityID));

            #region IP限制
            //获取用户IP;
            string userHostAddress = context.Request.UserHostAddress;
            var count = DataCache.GetCache(userHostAddress);
            if (count != null)
            {
                int newCount = int.Parse(count.ToString()) + 1;
                DataCache.SetCache(userHostAddress, newCount);
                int limitCount = 1000;
                if (activity != null)
                {

                    limitCount = activity.LimitCount;

                }
                if (newCount >= limitCount)
                {

                    resp.errcode = 5;
                    resp.errmsg = "您的提交过于频繁，请稍后再试";
                    goto outoff;



                }
            }
            else
            {
                DataCache.SetCache(userHostAddress, 1, DateTime.MaxValue, new TimeSpan(4, 0, 0));
            }


            #endregion

            #region 活动权限验证
            if (activity == null)
            {
                resp.errcode = 6;
                resp.errmsg = "活动不存在!";
                goto outoff;
            }
            if (activity.ActivityStatus.Equals(0))
            {
                resp.errcode = 7;
                resp.errmsg = "活动已关闭!";
                goto outoff;
            }

            if (activity.IsDelete.Equals(1))
            {
                resp.errcode = 8;
                resp.errmsg = "活动已删除!";
                goto outoff;
            }
            #endregion

            #region 判断必填项
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone))
            {
                resp.errcode = 9;
                resp.errmsg = "姓名和手机不能为空!";
                goto outoff;

            }

            if ((!phone.StartsWith("1")) || (!phone.Length.Equals(11)))
            {
                resp.errcode = 10;
                resp.errmsg = "手机号码无效!";
                goto outoff;
            }

            #endregion

            #region 检查自定义必填项
            List<ActivityFieldMappingInfo> listRequiredField = bll.GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}' And FieldIsNull=1", activity.ActivityID));
            if (listRequiredField.Count > 0)
            {
                foreach (var requiredField in listRequiredField)
                {
                    if (string.IsNullOrEmpty(dicPar.SingleOrDefault(p => p.Key.Equals(string.Format("K{0}", requiredField.ExFieldIndex))).Value))
                    {
                        resp.errcode = 11;
                        resp.errmsg = string.Format(" {0} 必填", requiredField.MappingName);
                        goto outoff;

                    }
                }
            }
            #endregion

            #region 检查数据格式
            //检查数据格式
            List<ActivityFieldMappingInfo> activityFieldMapping = bll.GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}'", activity.ActivityID));
            foreach (var item in activityFieldMapping)
            {

                string value = dicPar.SingleOrDefault(p => p.Key.Equals(string.Format("K{0}", item.ExFieldIndex))).Value;

                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                //检查数据格式
                if (item.FormatValiFunc == "email")//email检查
                {
                    if (!Common.ValidatorHelper.EmailLogicJudge(value))
                    {
                        resp.errcode = 12;
                        resp.errmsg = string.Format("{0}格式不正确", item.MappingName);
                        goto outoff;

                    }
                }
                if (item.FormatValiFunc == "url")//url检查
                {
                    System.Text.RegularExpressions.Regex regUrl = new System.Text.RegularExpressions.Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");//网址
                    System.Text.RegularExpressions.Match match = regUrl.Match(value);
                    if (!match.Success)
                    {
                        resp.errcode = 13;
                        resp.errmsg = string.Format("{0}格式不正确", item.MappingName);
                        goto outoff;

                    }
                }
            }
            #endregion

            #region 检查是否已经报名
            if (!string.IsNullOrEmpty(strDistinctKeys))
            {

                if (!strDistinctKeys.Equals("none"))//自定义检查重复
                {
                    System.Text.StringBuilder sbWhere = new System.Text.StringBuilder("1=1 ");
                    string[] distinctKeys = strDistinctKeys.Split(',');
                    foreach (var item in distinctKeys)
                    {
                        sbWhere.AppendFormat("And {0}='{1}' ", item, dicPar.Single(p => p.Key.Equals(item)).Value);
                    }
                    sbWhere.Append("  and IsDelete = 0  ");
                    if (bll.GetCount<ActivityDataInfo>(sbWhere.ToString()) > 0)
                    {

                        resp.errcode = 14;
                        resp.errmsg = "重复的报名!";
                        goto outoff;


                    }

                }
                else//不检查重复
                {

                }



            }
            else//默认检查
            {
                if (bll.GetCount<ActivityDataInfo>(string.Format("ActivityID='{0}' And Phone='{1}' and IsDelete = 0 ", signUpActivityID, phone)) > 0)
                {
                    resp.errcode = 15;
                    resp.errmsg = "已经报过名了!";
                    goto outoff;


                }
            }



            #endregion
            var newActivityUID = 1001;
            var lastActivityDataInfo = bll.Get<ActivityDataInfo>(string.Format("ActivityID='{0}' order by UID DESC", signUpActivityID));
            if (lastActivityDataInfo != null)
            {
                newActivityUID = lastActivityDataInfo.UID + 1;
            }
            ActivityDataInfo model = bll.ConvertRequestToModel<ActivityDataInfo>(new ActivityDataInfo());
            model.UID = newActivityUID;
            model.WeixinOpenID = weixinOpenID;
            model.SpreadUserID = spreadUserID;
            model.ActivityID = signUpActivityID;
            if (!string.IsNullOrEmpty(monitorPlanID))
            {
                model.MonitorPlanID = int.Parse(monitorPlanID);
            }
            model.WebsiteOwner = bll.WebsiteOwner;
            if (bll.IsLogin)
            {
                model.UserId = bll.GetCurrUserID();
            }
            model.ArticleType = juInfo.ArticleType;
            model.CategoryId = juInfo.CategoryId;
            if (bll.Add(model))
            {
                resp.errmsg = "ok";
                resp.isSuccess = true;
                if (juInfo.ActivityIntegral > 0)//扣积分
                {
                    currentUserInfo.TotalScore -= juInfo.ActivityIntegral;
                    if (bll.Update(currentUserInfo, string.Format("TotalScore={0}", currentUserInfo.TotalScore), string.Format(" AutoID={0}", currentUserInfo.AutoID)) <= 0)
                    {
                        resp.errcode = 16;
                        resp.errmsg = "扣除用户积分失败";
                    }
                    else
                    {
                        //
                        BLLJIMP.Model.WBHScoreRecord scoreRecord = new BLLJIMP.Model.WBHScoreRecord();
                        scoreRecord.Nums = "b55";
                        scoreRecord.InsertDate = DateTime.Now;
                        scoreRecord.WebsiteOwner = bll.WebsiteOwner;
                        scoreRecord.UserId = currentUserInfo.UserID;
                        scoreRecord.RecordType = "2";
                        scoreRecord.NameStr = "参加活动:" + juInfo.ActivityName;
                        scoreRecord.ScoreNum = string.Format("-{0}", juInfo.ActivityIntegral);
                        if (!bll.Add(scoreRecord))
                        {
                            resp.errcode = 17;
                            resp.errmsg = "插入积分记录失败";
                        }


                    }
                }

            }
            else
            {
                resp.errcode = 18;
                resp.errmsg = "报名失败，请重试或联系管理员!";
                goto outoff;

            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        private string SubmitActivityDataNoLogin(HttpContext context)
        {
            int activityId = int.Parse(context.Request["activityid"]);
            JuActivityInfo juInfo = bll.GetJuActivity(activityId, true);

            if (juInfo == null)
            {
                resp.errcode = 4;
                resp.errmsg = "活动不存在!";
                goto outoff;

            }
            #region 是否可以报名
            if (juInfo.ActivityStatus.Equals(1))
            {
                resp.errcode = 2;
                resp.errmsg = "活动已停止";
                goto outoff;

            }
            if (juInfo.MaxSignUpTotalCount > 0)//检查报名人数
            {
                if (juInfo.SignUpTotalCount > (juInfo.MaxSignUpTotalCount - 1))
                {
                    resp.errcode = 3;
                    resp.errmsg = "报名人数已满";
                    goto outoff;

                }

            }
            if (juInfo.ActivityIntegral > 0)
            {
                if (currentUserInfo.TotalScore < juInfo.ActivityIntegral)
                {
                    resp.errcode = 4;
                    resp.errmsg = "您的积分不足";
                    goto outoff;

                }

            }
            #endregion
            dicPar = bll.GetRequestParameter();
            string weixinOpenID = null;
            string signUpActivityID = juInfo.SignUpActivityID;
            string spreadUserID = null;
            dicPar.TryGetValue("SpreadUserID", out spreadUserID);
            string strDistinctKeys = null;//检查重复的字段，多个字段用,分隔， //没有此参数默认用手机检查  
            dicPar.TryGetValue("DistinctKeys", out strDistinctKeys);
            string monitorPlanID = null;
            dicPar.TryGetValue("MonitorPlanID", out monitorPlanID);
            string name = null;
            dicPar.TryGetValue("Name", out name);
            string phone = null;
            dicPar.TryGetValue("Phone", out phone);
            ActivityInfo activity = bll.Get<ActivityInfo>(string.Format("ActivityID='{0}'", signUpActivityID));

            #region IP限制
            //获取用户IP;
            string userHostAddress = context.Request.UserHostAddress;
            var count = DataCache.GetCache(userHostAddress);
            if (count != null)
            {
                int newCount = int.Parse(count.ToString()) + 1;
                DataCache.SetCache(userHostAddress, newCount);
                int limitCount = 1000;
                if (activity != null)
                {

                    limitCount = activity.LimitCount;

                }
                if (newCount >= limitCount)
                {

                    resp.errcode = 5;
                    resp.errmsg = "您的提交过于频繁，请稍后再试";
                    goto outoff;



                }
            }
            else
            {
                DataCache.SetCache(userHostAddress, 1, DateTime.MaxValue, new TimeSpan(4, 0, 0));
            }


            #endregion

            #region 活动权限验证
            if (activity == null)
            {
                resp.errcode = 6;
                resp.errmsg = "活动不存在!";
                goto outoff;
            }
            if (activity.ActivityStatus.Equals(0))
            {
                resp.errcode = 7;
                resp.errmsg = "活动已关闭!";
                goto outoff;
            }

            if (activity.IsDelete.Equals(1))
            {
                resp.errcode = 8;
                resp.errmsg = "活动已删除!";
                goto outoff;
            }
            #endregion

            #region 判断必填项
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone))
            {
                resp.errcode = 9;
                resp.errmsg = "姓名和手机不能为空!";
                goto outoff;

            }

            if ((!phone.StartsWith("1")) || (!phone.Length.Equals(11)))
            {
                resp.errcode = 10;
                resp.errmsg = "手机号码无效!";
                goto outoff;
            }

            #endregion

            #region 检查自定义必填项
            List<ActivityFieldMappingInfo> listRequiredField = bll.GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}' And FieldIsNull=1", activity.ActivityID));
            if (listRequiredField.Count > 0)
            {
                foreach (var requiredField in listRequiredField)
                {
                    if (string.IsNullOrEmpty(dicPar.SingleOrDefault(p => p.Key.Equals(string.Format("K{0}", requiredField.ExFieldIndex))).Value))
                    {
                        resp.errcode = 11;
                        resp.errmsg = string.Format(" {0} 必填", requiredField.MappingName);
                        goto outoff;

                    }
                }
            }
            #endregion

            #region 检查数据格式
            //检查数据格式
            List<ActivityFieldMappingInfo> activityFieldMapping = bll.GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}'", activity.ActivityID));
            foreach (var item in activityFieldMapping)
            {

                string value = dicPar.SingleOrDefault(p => p.Key.Equals(string.Format("K{0}", item.ExFieldIndex))).Value;

                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                //检查数据格式
                if (item.FormatValiFunc == "email")//email检查
                {
                    if (!Common.ValidatorHelper.EmailLogicJudge(value))
                    {
                        resp.errcode = 12;
                        resp.errmsg = string.Format("{0}格式不正确", item.MappingName);
                        goto outoff;

                    }
                }
                if (item.FormatValiFunc == "url")//url检查
                {
                    System.Text.RegularExpressions.Regex regUrl = new System.Text.RegularExpressions.Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");//网址
                    System.Text.RegularExpressions.Match match = regUrl.Match(value);
                    if (!match.Success)
                    {
                        resp.errcode = 13;
                        resp.errmsg = string.Format("{0}格式不正确", item.MappingName);
                        goto outoff;

                    }
                }
            }
            #endregion

            #region 检查是否已经报名
            if (!string.IsNullOrEmpty(strDistinctKeys))
            {

                if (!strDistinctKeys.Equals("none"))//自定义检查重复
                {
                    System.Text.StringBuilder sbWhere = new System.Text.StringBuilder("1=1 ");
                    string[] distinctKeys = strDistinctKeys.Split(',');
                    foreach (var item in distinctKeys)
                    {
                        sbWhere.AppendFormat("And {0}='{1}' ", item, dicPar.Single(p => p.Key.Equals(item)).Value);
                    }
                    sbWhere.Append("  and IsDelete = 0  ");
                    if (bll.GetCount<ActivityDataInfo>(sbWhere.ToString()) > 0)
                    {

                        resp.errcode = 14;
                        resp.errmsg = "重复的报名!";
                        goto outoff;


                    }

                }
                else//不检查重复
                {

                }



            }
            else//默认检查
            {
                if (bll.GetCount<ActivityDataInfo>(string.Format("ActivityID='{0}' And Phone='{1}' and IsDelete = 0 ", signUpActivityID, phone)) > 0)
                {
                    resp.errcode = 15;
                    resp.errmsg = "已经报过名了!";
                    goto outoff;


                }
            }



            #endregion
            var newActivityUID = 1001;
            var lastActivityDataInfo = bll.Get<ActivityDataInfo>(string.Format("ActivityID='{0}' order by UID DESC", signUpActivityID));
            if (lastActivityDataInfo != null)
            {
                newActivityUID = lastActivityDataInfo.UID + 1;
            }
            ActivityDataInfo model = bll.ConvertRequestToModel<ActivityDataInfo>(new ActivityDataInfo());
            model.UID = newActivityUID;
            model.WeixinOpenID = weixinOpenID;
            model.SpreadUserID = spreadUserID;
            model.ActivityID = signUpActivityID;
            if (!string.IsNullOrEmpty(monitorPlanID))
            {
                model.MonitorPlanID = int.Parse(monitorPlanID);
            }
            model.WebsiteOwner = bll.WebsiteOwner;
            if (bll.IsLogin)
            {
                model.UserId = bll.GetCurrUserID();
            }
            if (bll.Add(model))
            {
                resp.errmsg = "ok";
                if (juInfo.ActivityIntegral > 0)//扣积分
                {
                    currentUserInfo.TotalScore -= juInfo.ActivityIntegral;
                    if (bll.Update(currentUserInfo, string.Format("TotalScore={0}", currentUserInfo.TotalScore), string.Format(" AutoID={0}", currentUserInfo.AutoID)) <= 0)
                    {
                        resp.errcode = 16;
                        resp.errmsg = "扣除用户积分失败";
                    }
                    else
                    {
                        //
                        BLLJIMP.Model.WBHScoreRecord scoreRecord = new BLLJIMP.Model.WBHScoreRecord();
                        scoreRecord.Nums = "b55";
                        scoreRecord.InsertDate = DateTime.Now;
                        scoreRecord.WebsiteOwner = bll.WebsiteOwner;
                        scoreRecord.UserId = currentUserInfo.UserID;
                        scoreRecord.RecordType = "2";
                        scoreRecord.NameStr = "参加活动:" + juInfo.ActivityName;
                        scoreRecord.ScoreNum = string.Format("-{0}", juInfo.ActivityIntegral);
                        if (!bll.Add(scoreRecord))
                        {
                            resp.errcode = 17;
                            resp.errmsg = "插入积分记录失败";
                        }


                    }
                }

            }
            else
            {
                resp.errcode = 18;
                resp.errmsg = "报名失败，请重试或联系管理员!";
                goto outoff;

            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 获取用户报名数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetUserSingUp(HttpContext context)
        {
            resp.isSuccess = false;

            var aid = Convert.ToInt32(context.Request["aid"]);
            var userId = context.Request["userId"];
            var data = bll.GetUserSingup(aid, userId);

            resp.isSuccess = data != null;

            if (resp.isSuccess)
            {
                resp.returnObj = new
                {
                    userId = data.UserId,
                    openId = data.WeixinOpenID,
                    k1 = data.K1,
                    k2 = data.K2,
                    k3 = data.K3
                };
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }
        #endregion

        #region 资讯模块
        /// <summary>
        /// 获取资讯分类列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetNewsCategoryList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            CategoryApi apiRusult = new CategoryApi();
            apiRusult.list = new List<Category>();
            var sourceData = bll.GetLit<ArticleCategory>(pageSize, pageIndex, string.Format(" CategoryType='article' And WebsiteOwner='{0}' ", bll.WebsiteOwner));
            apiRusult.totalcount = bll.GetCount<ArticleCategory>(string.Format(" CategoryType='article' And WebsiteOwner='{0}' ", bll.WebsiteOwner));
            foreach (var item in sourceData)
            {
                Category model = new Category();
                model.categoryid = item.AutoID;
                model.categoryname = item.CategoryName;
                apiRusult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiRusult);

        }

        /// <summary>
        /// 获取资讯列表（获取文章列表）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetNewsList(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["pageindex"]),
                pageSize = Convert.ToInt32(context.Request["pagesize"]);

            string sort = context.Request["sort"],
                   cateid = context.Request["cateid"],
                   keyword = context.Request["keyword"],
                   orderBy = "";//默认排序

            #region 构造查询条件
            switch (sort)
            {
                case "time":
                    orderBy = " CreateDate DESC";
                    break;
                case "pv ":
                    orderBy = " PV DESC";
                    break;
                default:
                    orderBy = " Sort DESC,CreateDate DESC";
                    break;
            }

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" ArticleType='article' AND IsDelete=0 AND WebsiteOwner='{0}'", bll.WebsiteOwner);
            if (!string.IsNullOrEmpty(cateid))
            {
                sbWhere.AppendFormat(" And CategoryId={0}", cateid);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sbWhere.AppendFormat(" And ActivityName like'%{0}%'", keyword);
            }
            #endregion

            List<dynamic> dataList = new List<dynamic>();

            var sourceData = bll.GetLit<JuActivityInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);//获取数据
            var totalCount = bll.GetCount<JuActivityInfo>(sbWhere.ToString());//获取总数

            //构造返回对象
            foreach (var item in sourceData)
            {
                dataList.Add(new
                {
                    newsid = item.JuActivityID,
                    title = item.ActivityName,
                    digest = item.Summary,
                    time = bll.GetTimeStamp(item.CreateDate),
                    pv = item.PV,
                    categoryname = item.CategoryName,
                    imgurl = bll.GetImgUrl(item.ThumbnailsPath),
                    cateid = string.IsNullOrEmpty(item.CategoryId) ? 0 : int.Parse(item.CategoryId),
                    pubtime = item.CreateDate.ToString()

                });
            }

            dynamic result = new
            {
                totalcount = totalCount,
                list = dataList
            };

            return Common.JSONHelper.ObjectToJson(result);

        }


        /// <summary>
        /// 获取资讯详情
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetNewsDetail(HttpContext context)
        {

            int activityId = int.Parse(context.Request["newsid"]);
            JuActivityInfo item = bll.GetJuActivity(activityId, false);
            item.PV++;
            bll.Update(item);

            if (item.ActivityDescription.Contains("/FileUpload/"))
            {
                item.ActivityDescription = item.ActivityDescription.Replace("/FileUpload/", string.Format("http://{0}/FileUpload/", context.Request.Url.Host));
            }

            dynamic result = new
            {
                newsid = item.JuActivityID,
                title = item.ActivityName,
                digest = item.Summary,
                time = bll.GetTimeStamp(item.CreateDate),
                pv = item.PV,
                categoryname = item.CategoryName,
                imgurl = bll.GetImgUrl(item.ThumbnailsPath),
                cateid = string.IsNullOrEmpty(item.CategoryId) ? 0 : int.Parse(item.CategoryId),
                newscontent = item.ActivityDescription,
                pubtime = item.CreateDate.ToString()
            };

            return Common.JSONHelper.ObjectToJson(result);

        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetArticleList(HttpContext context)
        {
            //DateTime start = DateTime.Now;
            int pageIndex = Convert.ToInt32(context.Request["pageIndex"]),
               pageSize = Convert.ToInt32(context.Request["pageSize"]),
               isGetNoCommentData = Convert.ToInt32(context.Request["isGetNoCommentData"]),
               isHasCommentAndReplayCount = Convert.ToInt32(context.Request["isHasCommentAndReplayCount"]);
            string cateId = context.Request["cateId"],
                   keyword = context.Request["keyword"],
                   tags = context.Request["tags"],
                   cityCode = context.Request["city"],
                   provinceCode = context.Request["province"],
                   orderby = context.Request["orderby"],
                   type = context.Request["type"],
                   author = context.Request["author"],
                   keyType = context.Request["keyType"],
                   hasStatisticsStr = context.Request["hasStatistics"],
                   hasAuthorStr = context.Request["hasAuthor"],
                   column = context.Request["column"],
                   data_type = context.Request["data_type"],
                   create_start = context.Request["create_start"],
                   create_end = context.Request["create_end"],
                   keyword_author = context.Request["keyword_author"],
                   order_all = context.Request["order_all"],
                   chk_friend = context.Request["chk_friend"];

            if (orderby == "comment")
            {
                orderby = "CommentAndReplayCount desc";
            }
            bool hasStatistics = true;
            bool hasAuthor = true;
            bool chkFriend = false;
            bool isHide = false;
            if (hasStatisticsStr == "0") hasStatistics = false;
            if (hasAuthorStr == "0") hasAuthor = false;
            if (chk_friend == "1") chkFriend = true;
            //取微吸粉数据
            bool hasActivityForward = false;
            if (data_type == "1") hasActivityForward = true;

            if (!string.IsNullOrWhiteSpace(author)) author = bllUser.GetUserInfoByAutoID(int.Parse(author)).UserID;

            var totalCount = 0;
            var sourceData = this.bll.GetJuActivityList(
                    type,
                    "",
                    out totalCount,
                    pageIndex,
                    pageSize,
                    author,
                    this.currentUserInfo == null ? "" : this.currentUserInfo.UserID,
                    cateId,
                    this.bll.WebsiteOwner,
                    keyword,
                    tags,
                    provinceCode,
                    cityCode,
                    null,
                    isGetNoCommentData > 0,
                    orderby,
                    isHasCommentAndReplayCount > 0,
                    false,
                    null,
                    false,
                    column,
                    hasStatistics,
                    hasAuthor,
                    hasActivityForward,
                    create_start,
                    create_end,
                    keyword_author == "1",
                    order_all
                );
            //DateTime dataend = DateTime.Now;

            List<dynamic> returnList = new List<dynamic>();

            foreach (var item in sourceData)
            {
                returnList.Add(StructureArticle(item, false,currentUserInfo, chkFriend));
            }
            //DateTime dataStructure = DateTime.Now;

            dynamic result = new
            {
                totalcount = totalCount,
                list = returnList
                //,
                //start = start.ToString("yyyy-MM-dd hh:mm:ss.fff"),
                //dataend = dataend.ToString("yyyy-MM-dd hh:mm:ss.fff"),
                //dataStructure = dataStructure.ToString("yyyy-MM-dd hh:mm:ss.fff")
            };

            return Common.JSONHelper.ObjectToJson(result);
        }


        public dynamic GetArticleList(
            int pageIndex,
            int pageSize,
            int isGetNoCommentData,
            int isHasCommentAndReplayCount,
            string cateId,
            string keyword,
            string tags,
            string cityCode,
            string provinceCode,
            string orderby,
            string type,
            string author,
            string keyType,
            string column = "",
            bool hasStatistics = true,
            bool hasAuthor = true,
            bool isForward = false
            )
        {
            if (orderby == "comment")
            {
                orderby = "CommentAndReplayCount desc";
            }

            if (!string.IsNullOrWhiteSpace(author) && hasAuthor) author = bllUser.GetUserInfoByAutoID(int.Parse(author)).UserID;

            var totalCount = 0;
            var sourceData = this.bll.GetJuActivityList(
                    type,
                    "",
                    out totalCount,
                    pageIndex,
                    pageSize,
                    author,
                    this.currentUserInfo == null ? "" : this.currentUserInfo.UserID,
                    cateId,
                    this.bll.WebsiteOwner,
                    keyword,
                    tags,
                    provinceCode,
                    cityCode,
                    null,
                    isGetNoCommentData > 0,
                    orderby,
                    isHasCommentAndReplayCount > 0,
                    false,
                    null,
                    false,
                    column,
                    hasStatistics,
                    hasAuthor,
                    isForward
                );


            List<dynamic> returnList = new List<dynamic>();

            foreach (var item in sourceData)
            {
                returnList.Add(StructureArticle(item, false, currentUserInfo));
            }


            dynamic result = new
            {
                totalcount = totalCount,
                list = returnList
            };

            return result;
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetArticleNewList(HttpContext context)
        {
            return GetArticleList(context);
        }
        /// <summary>
        /// 公开课公告
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetOpenClassNotice(HttpContext context)
        {
            return Common.JSONHelper.ObjectToJson(new
            {
                notice = bllKeyValueData.GetDataDefVaule("OpenClassNotice", "0")
            });
        }
        /// <summary>
        /// 获取公开课详情
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetOpenClassDetail(HttpContext context)
        {
            //返回文章基本信息
            int articleId = Convert.ToInt32(context.Request["articleId"]);

            var article = this.bll.GetJuActivity(articleId, true);

            string summary = article.Summary;
            if (string.IsNullOrWhiteSpace(summary))
            {
                summary = MySpider.MyRegex.RemoveHTMLTags(article.ActivityDescription);
                if (summary.Length > 160) summary = summary.Substring(0, 160) + "...";
            }

            int favoriteCount = bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.JuActivityFavorite, article.JuActivityID.ToString(), null);//收藏数

            bool currUserIsLogin = this.currentUserInfo == null ? false : true;
            bool currUserIsFavorite = this.currentUserInfo == null ? false : bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.JuActivityFavorite, article.JuActivityID.ToString(), this.currentUserInfo.UserID) > 0;

            bool currUserIsVip = false;
            if (this.currentUserInfo != null)
            {
                string VipStr = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserIsVip, this.currentUserInfo.UserID);
                DateTime vipEndTime = new DateTime();
                DateTime.TryParse(VipStr, out vipEndTime);
                currUserIsVip = vipEndTime > DateTime.Now ? true : false;
            }

            bool isFee = article.IsFee == 1 ? true : false;
            bool currUserIsPurchased = this.currentUserInfo == null ? false : bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.ViewOpenClass, article.JuActivityID.ToString(), this.currentUserInfo.UserID) > 0;

            bool isCanView = false;
            if (currUserIsVip || isFee || currUserIsPurchased) isCanView = true;
            string webUrl = string.Empty;
            List<dynamic> files = new List<dynamic>();
            if (isCanView)
            {
                string website = article.ActivityWebsite;
                string pwd = article.ActivityAddress;
                string nickname = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserOpenCreate, article.UserID);
                if (!string.IsNullOrWhiteSpace(website))
                {
                    webUrl = website + "?nickname=" + HttpUtility.UrlEncode(nickname) + "&sec=md5&token=" + Payment.WeiXin.MD5Util.getMd5HexStr(pwd, "UTF-8") + "&uid=";
                    string userStr = currUserIsLogin ? (1000000000 + this.currentUserInfo.AutoID).ToString() : DateTime.Now.Ticks.ToString();
                    webUrl += userStr;
                    string k = MySpider.Base64Change.EncodeBase64("ubi" + userStr);
                    webUrl += "&k=" + k;
                    Common.DataCache.SetCache(k, "1", DateTime.Now.AddMinutes(5), TimeSpan.Zero);
                }

                List<JuActivityFiles> juActivityFiles = bll.GetFiles(articleId);
                foreach (JuActivityFiles item in juActivityFiles)
                {
                    files.Add(new
                    {
                        fileName = item.FileName,
                        filePath = item.FilePath
                    });
                }
            }

            bll.PlusNumericalCol("PV", article.JuActivityID);

            dynamic result = new
            {
                id = article.JuActivityID,
                title = article.ActivityName,
                content = article.ActivityDescription,
                createDate = article.CreateDate.ToString(),
                favoriteCount = favoriteCount,//收藏数
                currUserIsFavorite = currUserIsFavorite,
                needScore = Math.Abs(article.ActivityIntegral),
                isCanView = isCanView,
                webUrl = webUrl,
                files = files,
                pv = article.PV,
                tags = article.Tags
            };
            return Common.JSONHelper.ObjectToJson(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetOpenClassWebUrl(HttpContext context)
        {
            int articleId = Convert.ToInt32(context.Request["articleId"]);
            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            var article = bll.GetJuActivity(articleId);

            bool currUserIsVip = false;
            if (this.currentUserInfo != null)
            {
                string VipStr = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserIsVip, this.currentUserInfo.UserID);
                DateTime vipEndTime = new DateTime();
                DateTime.TryParse(VipStr, out vipEndTime);
                currUserIsVip = vipEndTime > DateTime.Now ? true : false;
            }

            bool isFee = article.IsFee == 1 ? true : false;
            bool isCanView = false;
            if (currUserIsVip || isFee) isCanView = true;

            if (!isCanView && !bLLCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.ViewOpenClass, articleId.ToString(), this.currentUserInfo.UserID))
            {
                if (this.currentUserInfo.TotalScore + article.ActivityIntegral < 0)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IntegralProblem;
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                if (!bLLCommRelation.AddCommRelation(BLLJIMP.Enums.CommRelationType.ViewOpenClass, articleId.ToString(), this.currentUserInfo.UserID))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.AddCommRelationError;
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                bllUser.AddUserScoreDetail(this.currentUserInfo.UserID, EnumStringHelper.ToString(ScoreDefineType.OpenClass), this.bll.WebsiteOwner, article.ActivityIntegral);
            }

            string webUrl = string.Empty;

            string website = article.ActivityWebsite;
            string pwd = article.ActivityAddress;
            string nickname = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserOpenCreate, article.UserID);
            if (!string.IsNullOrWhiteSpace(website))
            {

                webUrl = website + "?nickname=" + HttpUtility.UrlEncode(nickname) + "&sec=md5&token=" + Payment.WeiXin.MD5Util.getMd5HexStr(pwd, "UTF-8") + "&uid=";
                string userStr = (1000000000 + this.currentUserInfo.AutoID).ToString();
                webUrl += userStr;
                string k = MySpider.Base64Change.EncodeBase64("ubi" + userStr);
                webUrl += "&k=" + k;
                Common.DataCache.SetCache(k, "1", DateTime.Now.AddMinutes(5), TimeSpan.Zero);
            }

            List<dynamic> files = new List<dynamic>();
            List<JuActivityFiles> juActivityFiles = bll.GetFiles(articleId);
            foreach (JuActivityFiles item in juActivityFiles)
            {
                files.Add(new
                {
                    fileName = item.FileName,
                    filePath = item.FilePath
                });
            }


            resp.isSuccess = true;
            resp.returnObj = new
            {
                webUrl = webUrl,
                files = files
            };
            return Common.JSONHelper.ObjectToJson(resp);
        }
        private string CheckOpenClassK(HttpContext context)
        {
            string k = context.Request["k"];
            if (Common.DataCache.GetCache(k) == null)
            {
                return "fail";
            }
            else
            {
                Common.DataCache.ClearCache(k);
                return "pass";
            }
        }


        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetArticleDetail(HttpContext context)
        {
            //返回文章基本信息
            int articleId = Convert.ToInt32(context.Request["articleid"]);

            var article = this.bll.GetJuActivity(articleId, true);

            article = this.bll.FilterJuActivityExInfo(article, this.currentUserInfo == null ? "" : this.currentUserInfo.UserID);

            bll.PlusNumericalCol("PV", article.JuActivityID);

            return Common.JSONHelper.ObjectToJson(StructureArticle(article, true, currentUserInfo));
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddArticle(HttpContext context)
        {
            /*
             * 添加文章
             * 
             * 必填：
             * 类型 type
             * 内容 content
             * 所属分类 cateId
             * 
             */
            string type = context.Request["type"]
                , title = context.Request["title"]
                , content = context.Request["content"]
                , cateId = context.Request["cateId"]
                , province = context.Request["province"]
                , city = context.Request["city"]
                , tag = context.Request["tag"]
                , receivers = context.Request["receivers"]
                , summary = context.Request["summary"]
                , thumbnailspath = context.Request["thumbnails"]
                ,rootid=context.Request["rootid"]
                , k1 = context.Request["k1"]//淘股金
                , k2 = context.Request["k2"]//昵称
                , k3 = context.Request["k3"]//股权数
                , k4 = context.Request["k4"]
                , k5 = context.Request["k5"]
                , k6 = context.Request["k6"]
                , k7 = context.Request["k7"]
                , k8 = context.Request["k8"]
                , k9 = context.Request["k9"]
                , k10 = context.Request["k10"];

            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            BLLJIMP.Enums.ContentType nType = new BLLJIMP.Enums.ContentType();
            if (!Enum.TryParse(type, out nType))
            {
                resp.errcode = 1;
                resp.errmsg = "类型格式不能识别";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (nType == BLLJIMP.Enums.ContentType.Statuses && !bLLCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.FollowArticleCategory, cateId, this.currentUserInfo.UserID))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.NoFollow;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            //敏感词检查
            BLLFilterWord bllFilterWord = new BLLFilterWord();
            string errmsg = "";
            if (!bllFilterWord.CheckFilterWord(content, this.bll.WebsiteOwner, out errmsg, "0"))
            {
                resp.errmsg = errmsg;
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            var jid = this.bll.GetGUID(TransacType.CommAdd);

            if (string.IsNullOrWhiteSpace(summary))
            {
                summary = MySpider.MyRegex.RemoveHTMLTags(content);
                if (summary.Length > 200) summary = summary.Substring(0, 200) + "...";
            }
            if (string.IsNullOrWhiteSpace(title))
            {
                title = type;
            }

            string img = string.Empty;
            if (!string.IsNullOrEmpty(thumbnailspath))
            {
                img = thumbnailspath;
            }
            else if (string.IsNullOrEmpty(MySpider.MyRegex.GetPadSrcUrl(content)))
            {
                img = MySpider.MyRegex.GetPadSrcUrl(content);
            }
            else
            {
                img = null;
            }

            if (type.ToLower() == "comment")
            {
                int nHour = DateTime.Now.Hour;
                int tCount = 0;
                List<ArticleCategory> listCate = bllArticleCategory.GetCateList(out tCount, "Comment", 0, "");
                foreach (var nCate in listCate)
                {
                    if (!string.IsNullOrWhiteSpace(nCate.Summary) && nCate.Summary.Split('-').Count() == 2)
                    {
                        string[] sl = nCate.Summary.Split('-');
                        int s = 0;
                        int e = 0;
                        int.TryParse(sl[0], out s);
                        int.TryParse(sl[1], out e);
                        if (nHour >= s && nHour < e)
                        {
                            cateId = nCate.AutoID.ToString();
                            break;
                        }
                    }
                }
            }
            if (type.ToLower() == "pupildebate")
            {
                JuActivityInfo isRepeat = bll.Get<JuActivityInfo>(string.Format(" WebsiteOwner='{0}' AND UserId='{1}' AND RootCateId='{2}' AND CategoryId='{3}' AND CreateDate>='{4}' ", bll.WebsiteOwner, this.currentUserInfo.UserID,rootid,cateId,k4));
                if (isRepeat != null)
                {
                    resp.errmsg = "你已经参与过一次";
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }

            string CaseCategoryId = System.Configuration.ConfigurationManager.AppSettings["CaseCategoryId"];

            JuActivityInfo juActivityInfo = new JuActivityInfo()
            {
                JuActivityID = int.Parse(jid),
                ActivityName = title,
                ActivityDescription = content,
                CategoryId = cateId,
                ArticleType = type,
                ProvinceCode = province,
                CityCode = city,
                Tags = tag,
                CreateDate = DateTime.Now,
                CreateIP = MySpider.MySpider.GetClientIP(),
                UserID = this.currentUserInfo.UserID,
                WebsiteOwner = this.bll.WebsiteOwner,
                Summary = summary,
                TStatus = cateId == CaseCategoryId ? 5 : 0,
                ThumbnailsPath = img,
                K1 = k1,
                K2 = k2,
                K3 = k3,
                K4 = k4,
                K5 = k5,
                K6 = k6,
                K7 = k7,
                K8 = k8,
                K9 = k9,
                K10 = k10,
                Sort = 1,
                RootCateId=rootid
            };

            var result = this.bll.Add(juActivityInfo);

            resp.isSuccess = result;
            resp.returnValue = jid;
            if (result)
            {
                List<string> tagList = null;

                if (!string.IsNullOrWhiteSpace(tag))
                {
                    tagList = tag.Split(',').ToList();
                }
                this.bll.SetJuActivityContentTags(int.Parse(jid), tagList);

                if (nType == BLLJIMP.Enums.ContentType.Question)
                {
                    bllUser.AddUserScoreDetail(this.currentUserInfo.UserID, EnumStringHelper.ToString(ScoreDefineType.AddQuestions), this.bll.WebsiteOwner, null, null);
                }
                else if (nType == BLLJIMP.Enums.ContentType.Article)
                {
                    if (juActivityInfo.CategoryId == CaseCategoryId)
                    {
                        //bllUser.AddUserScoreDetail(this.currentUserInfo.UserID, BLLJIMP.Enums.ScoreDefineType.AddCase, this.bll.WebsiteOwner, null, null);
                    }
                    else
                    {
                        bllUser.AddUserScoreDetail(this.currentUserInfo.UserID, EnumStringHelper.ToString(ScoreDefineType.AddArticle), this.bll.WebsiteOwner, null, null);
                    }
                }

                if (nType == BLLJIMP.Enums.ContentType.Question && !string.IsNullOrWhiteSpace(receivers))
                {
                    string[] receiverList = receivers.Split(',');
                    for (int i = 0; i < receiverList.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(receiverList[i]))
                        {
                            bLLCommRelation.AddCommRelation(BLLJIMP.Enums.CommRelationType.InviteAnswer, jid, receiverList[i]);
                            bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.InviteAnswer, this.currentUserInfo, juActivityInfo, receiverList[i], null);
                        }
                    }
                }

                if (bllUser.IsTutor(this.currentUserInfo.UserID))
                {
                    bllTutor.UpdateWZNums(this.currentUserInfo.UserID);
                }


            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 邀请回答
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string InviteAnswer(HttpContext context)
        {
            string id = context.Request["id"], userId = context.Request["userId"];
            if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(userId))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (bLLCommRelation.AddCommRelation(BLLJIMP.Enums.CommRelationType.InviteAnswer, id, userId))
            {
                JuActivityInfo juActivityInfo = bll.GetJuActivityByActivityID(id);
                bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.InviteAnswer, this.currentUserInfo, juActivityInfo, userId, null);

                resp.isSuccess = true;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                return Common.JSONHelper.ObjectToJson(resp);
            }
        }

        /// <summary>
        /// 获取今日推荐文章
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetTodyRecommendArticle(HttpContext context)
        {
            //TODO:推荐文章 算法实现，目前临时代替
            var article = this.bll.GetList<JuActivityInfo>(1, " ArticleType = 'article' ", " CreateDate DESC ")[0];
            return Common.JSONHelper.ObjectToJson(StructureArticle(article, false, currentUserInfo));
        }
        /// <summary>
        /// 获取热门文章列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetHotArticleList(HttpContext context)
        {
            /*
             * 有评论，按评论数 按评论数排列
             * 
             * 
             * 
             */

            int pageIndex = Convert.ToInt32(context.Request["pageindex"]),
                pageSize = Convert.ToInt32(context.Request["pagesize"]);

            string cateid = context.Request["cateid"],
                   keyword = context.Request["keyword"],
                   tags = context.Request["tags"],
                   cityCode = context.Request["city"],
                   type = context.Request["type"];

            if (pageIndex == 0)
            {
                pageIndex = 1;
                pageSize = 10;
            }

            var totalCount = 0;
            var sourceData = this.bll.GetJuActivityList(
                    type,
                    "",
                    out totalCount,
                    pageIndex,
                    pageSize,
                    "",
                    this.currentUserInfo == null ? "" : this.currentUserInfo.UserID,
                    cateid,
                    this.bll.WebsiteOwner,
                    keyword,
                    tags,
                    null,
                    cityCode,
                    null,
                    false,
                    "CommentAndReplayCount desc"
                );

            List<dynamic> returnList = new List<dynamic>();

            foreach (var item in sourceData)
            {
                returnList.Add(StructureArticle(item, false, currentUserInfo));
            }

            dynamic result = new
            {
                totalcount = totalCount,
                list = returnList
            };

            return Common.JSONHelper.ObjectToJson(result);
        }

        /// <summary>
        /// 获取热门标签列表（按使用数排序）无使用的不会显示
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetHotTags(HttpContext context)
        {
            var num = context.Request["num"];//标签分类id

            int? numInt;
            if (!string.IsNullOrWhiteSpace(num)) numInt = int.Parse(num);
            List<CommRelationInfo> tags = new List<CommRelationInfo>();
            if (!string.IsNullOrWhiteSpace(num))
            {
                tags = bllTag.GetTagList(int.Parse(num));
            }
            else
            {
                tags = bllTag.GetTagList(null);
            }

            var result = from p in tags
                         select new
                         {
                             tag = p.RelationId,
                             useCount = p.AutoId
                         };
            return Common.JSONHelper.ObjectToJson(result);
        }



        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetTags(HttpContext context)
        {
            int tCount = 0;
            List<MemberTag> tags = bllTag.GetTags(this.bll.WebsiteOwner, null, 1, int.MaxValue, out tCount, null);

            var result = from p in tags
                         select new
                         {
                             id = p.AutoId,
                             tag = p.TagName
                         };
            return Common.JSONHelper.ObjectToJson(result);
        }


        /// <summary>
        /// 每日案例
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetDailyCase(HttpContext context)
        {
            string cateId = context.Request["cateId"];
            string articleId = string.Empty;

            var relation = bLLCommRelation.GetRelationInfo(BLLJIMP.Enums.CommRelationType.DailyCase, null, null);
            JuActivityInfo article = null;
            if (relation != null)
            {
                article = bll.GetJuActivity(int.Parse(relation.MainId), true);
            }
            if (article == null)
            {
                article = bll.GetNewActivityByCateId(cateId, false);
            }

            if (article == null)
            {
                resp.isSuccess = false;
                resp.errcode = 1;
                resp.errmsg = "没有该类文章";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            string thumImg = article.ThumbnailsPath;
            if (string.IsNullOrWhiteSpace(thumImg)) thumImg = "/img/hb/hb2.jpg";
            return Common.JSONHelper.ObjectToJson(new
            {
                id = article.JuActivityID,
                title = article.ActivityName,
                summary = article.Summary,
                thumImg = thumImg
            });
        }

        /// <summary>
        /// 获取文章评论列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetCommentList(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["pageindex"]),
                pageSize = Convert.ToInt32(context.Request["pagesize"]);
            var articleId = context.Request["articleid"];
            var userAutoId = context.Request["userAutoId"];
            var reviewType = context.Request["reviewType"];

            var totalCount = 0;
            BLLJIMP.Enums.ReviewTypeKey nType = BLLJIMP.Enums.ReviewTypeKey.ArticleComment;
            if (string.IsNullOrWhiteSpace(reviewType) && !string.IsNullOrWhiteSpace(articleId))
            {
                int artId = Convert.ToInt32(articleId);
                JuActivityInfo juArtcle = bll.GetJuActivity(artId);
                if (juArtcle.ArticleType.ToLower() == "question")
                {
                    nType = BLLJIMP.Enums.ReviewTypeKey.Answer;
                }
            }
            else
            {
                Enum.TryParse(reviewType, out nType);
            }
            string userId = "";
            if (!string.IsNullOrWhiteSpace(userAutoId))
            {
                UserInfo user = bllUser.GetUserInfoByAutoID(int.Parse(userAutoId));
                if (user != null)
                {
                    userId = user.UserID;
                }
                else
                {
                    userId = "-1";
                }

            }
            //仅显示审核通过的
            var sourceData = this.bllReview.GetReviewList(nType, out totalCount, pageIndex, pageSize, articleId, this.bll.WebsiteOwner, this.currentUserInfo == null ? "" : this.currentUserInfo.UserID, "", userId, "1");

            List<dynamic> returnList = new List<dynamic>();

            foreach (var item in sourceData)
            {
                int actId = 0;
                int.TryParse(item.Expand1, out actId);
                JuActivityInfo actInfo = bll.GetJuActivity(actId);
                returnList.Add(new
                {
                    id = item.ReviewMainId,
                    content = item.ReviewContent,
                    createDate = item.InsertDate.ToString(),
                    replyCount = item.ReplyCount,//回复数
                    praiseCount = item.PraiseCount,//点赞数
                    pv = item.Pv,//浏览数
                    currUserIsPraise = item.CurrUserIsPraise,
                    articleId = actInfo != null ? actInfo.JuActivityID : 0,
                    articleName = actInfo != null ? actInfo.ActivityName : "",
                    pubUser = new
                    {
                        id = item.PubUser == null ? 0 : item.PubUser.AutoID,
                        userId = item.PubUser == null ? "" : item.PubUser.UserID,
                        userName = item.PubUser == null ? "" : bllUser.GetUserDispalyName(item.PubUser),
                        avatar = item.PubUser == null ? "" : bllUser.GetUserDispalyAvatar(item.PubUser),
                        isTutor = item.PubUser == null ? false : bllUser.IsTutor(item.PubUser)
                    },
                    replayToUser = new
                    {
                        id = item.ReplayToUser == null ? 0 : item.ReplayToUser.AutoID,
                        userId = item.ReplayToUser == null ? "" : item.ReplayToUser.UserID,
                        userName = item.ReplayToUser == null ? "" : bllUser.GetUserDispalyName(item.ReplayToUser),
                        avatar = item.ReplayToUser == null ? "" : bllUser.GetUserDispalyAvatar(item.ReplayToUser),
                        isTutor = item.ReplayToUser == null ? false : bllUser.IsTutor(item.ReplayToUser)
                    }
                });
            }

            dynamic result = new
            {
                totalcount = totalCount,
                list = returnList
            };

            return Common.JSONHelper.ObjectToJson(result);
        }

        private string GetCommentReplyList(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["pageindex"]),
                pageSize = Convert.ToInt32(context.Request["pagesize"]);
            var commentId = context.Request["commentid"];

            var totalCount = 0;
            //仅显示审核通过的
            var sourceData = this.bllReview.GetReviewList(BLLJIMP.Enums.ReviewTypeKey.CommentReply, out totalCount, pageIndex, pageSize, commentId, this.bll.WebsiteOwner, this.currentUserInfo == null ? "" : this.currentUserInfo.UserID, null, null, "1");

            List<dynamic> returnList = new List<dynamic>();

            foreach (var item in sourceData)
            {
                returnList.Add(new
                {
                    id = item.ReviewMainId,
                    content = item.ReviewContent,
                    createDate = item.InsertDate.ToString(),
                    replyCount = item.ReplyCount,//回复数
                    praiseCount = item.PraiseCount,//点赞数
                    currUserIsPraise = item.CurrUserIsPraise,
                    pubUser = new
                    {
                        id = item.PubUser == null ? 0 : item.PubUser.AutoID,
                        userId = item.PubUser == null ? "" : item.PubUser.UserID,
                        userName = item.PubUser == null ? "" : bllUser.GetUserDispalyName(item.PubUser),
                        avatar = item.PubUser == null ? "" : bllUser.GetUserDispalyAvatar(item.PubUser),
                        isTutor = item.PubUser == null ? false : bllUser.IsTutor(item.PubUser)
                    },
                    replayToUser = new
                    {
                        id = item.ReplayToUser == null ? 0 : item.ReplayToUser.AutoID,
                        userId = item.ReplayToUser == null ? "" : item.ReplayToUser.UserID,
                        userName = item.ReplayToUser == null ? "" : bllUser.GetUserDispalyName(item.ReplayToUser),
                        avatar = item.ReplayToUser == null ? "" : bllUser.GetUserDispalyAvatar(item.ReplayToUser),
                        isTutor = item.ReplayToUser == null ? false : bllUser.IsTutor(item.ReplayToUser)
                    }
                });
            }

            dynamic result = new
            {
                totalcount = totalCount,
                list = returnList
            };

            return Common.JSONHelper.ObjectToJson(result);
        }

        /// <summary>
        /// 获取单条评论或回复详情
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetCommentDetail(HttpContext context)
        {
            var id = Convert.ToInt32(context.Request["id"]);

            var item = this.bllReview.FilterReviewInfo(id, this.currentUserInfo == null ? "" : this.currentUserInfo.UserID);

            return Common.JSONHelper.ObjectToJson(new
            {
                id = item.ReviewMainId,
                content = item.ReviewContent,
                createDate = item.InsertDate.ToString(),
                replyCount = item.ReplyCount,//回复数
                praiseCount = item.PraiseCount,//点赞数
                currUserIsPraise = item.CurrUserIsPraise,
                pubUser = new
                {
                    id = item.PubUser == null ? 0 : item.PubUser.AutoID,
                    userId = item.PubUser == null ? "" : item.PubUser.UserID,
                    userName = item.PubUser == null ? "" : bllUser.GetUserDispalyName(item.PubUser),
                    avatar = item.PubUser == null ? "" : bllUser.GetUserDispalyAvatar(item.PubUser),
                    isTutor = item.PubUser == null ? false : bllUser.IsTutor(item.PubUser)
                },
                replayToUser = new
                {
                    id = item.ReplayToUser == null ? 0 : item.ReplayToUser.AutoID,
                    userId = item.ReplayToUser == null ? "" : item.ReplayToUser.UserID,
                    userName = item.ReplayToUser == null ? "" : bllUser.GetUserDispalyName(item.ReplayToUser),
                    avatar = item.ReplayToUser == null ? "" : bllUser.GetUserDispalyAvatar(item.ReplayToUser),
                    isTutor = item.ReplayToUser == null ? false : bllUser.IsTutor(item.ReplayToUser)
                }
            });
        }

        /// <summary>
        /// 文章评论
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CommentArticle(HttpContext context)
        {
            var articleId = context.Request["articleid"];
            var content = context.Request["content"];
            var replyId = Convert.ToInt32(context.Request["replyid"]);//评论了文章里的哪个评论

            int isHideUserName = Convert.ToInt32(context.Request["isHideUserName"]);

            resp.isSuccess = false;

            if (string.IsNullOrWhiteSpace(articleId) || string.IsNullOrWhiteSpace(content))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (bll.GetCount<JuActivityInfo>(string.Format(" JuActivityID = {0} ", articleId)) == 0)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //敏感词检查
            BLLFilterWord bllFilterWord = new BLLFilterWord();
            string errmsg = "";
            if (!bllFilterWord.CheckFilterWord(content, this.bll.WebsiteOwner, out errmsg, "0"))
            {
                resp.errmsg = errmsg;
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            //添加评论
            int reviewId = 0;
            JuActivityInfo article = bll.GetJuActivity(int.Parse(articleId), true);
            BLLJIMP.Enums.ReviewTypeKey reviewType = BLLJIMP.Enums.ReviewTypeKey.ArticleComment;
            if (article.ArticleType == "Question") reviewType = BLLJIMP.Enums.ReviewTypeKey.Answer;
            var addResult = bllReview.AddReview(reviewType, articleId, replyId, this.currentUserInfo.UserID, "评论", content, this.bll.WebsiteOwner, out reviewId, isHideUserName);

            if (addResult)
            {
                if (reviewType == BLLJIMP.Enums.ReviewTypeKey.Answer)
                {
                    bllUser.AddUserScoreDetail(this.currentUserInfo.UserID, EnumStringHelper.ToString(ScoreDefineType.AnswerQuestions), this.bll.WebsiteOwner, null, null);
                }
                resp.isSuccess = true;
                resp.returnValue = reviewId.ToString();

                if (article.ArticleType == "Question")
                {
                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.QuestionIsAnswered, this.currentUserInfo, article, article.UserID, content);

                    List<UserInfo> users = bllUser.GetRelationUserList(BLLJIMP.Enums.CommRelationType.JuActivityFollow, articleId);
                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.FollowQuestionIsAnswered, this.currentUserInfo, article, users, content);
                }
            }
            else
            {
                resp.isSuccess = false;
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 评论回复
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ReplyComment(HttpContext context)
        {
            var commentId = context.Request["commentid"];
            var replyId = Convert.ToInt32(context.Request["replyid"]);//回复了评论里的哪个回复
            var content = context.Request["content"];
            int isHideUserName = Convert.ToInt32(context.Request["isHideUserName"]);
            resp.isSuccess = false;

            if (string.IsNullOrWhiteSpace(commentId) || string.IsNullOrWhiteSpace(content))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (bll.GetCount<ReviewInfo>(string.Format(" ReviewMainId = {0} ", commentId)) == 0)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            //敏感词检查
            BLLFilterWord bllFilterWord = new BLLFilterWord();
            string errmsg = "";
            if (!bllFilterWord.CheckFilterWord(content, this.bll.WebsiteOwner, out errmsg, "0"))
            {
                resp.errmsg = errmsg;
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            //添加回复

            int reviewId = 0;

            var addResult = bllReview.AddReview(BLLJIMP.Enums.ReviewTypeKey.CommentReply, commentId, replyId, this.currentUserInfo.UserID, "评论", content, this.bll.WebsiteOwner, out reviewId, isHideUserName);

            if (addResult)
            {
                resp.isSuccess = true;
                resp.returnValue = reviewId.ToString();

                if (bllUser.IsTutor(this.currentUserInfo.UserID))
                {
                    bllTutor.UpdateAnswers(this.currentUserInfo.UserID);
                }
            }
            else
            {
                resp.isSuccess = false;
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 举报内容
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ReportIllegalContent(HttpContext context)
        {
            return AddCurrUserRelation(BLLJIMP.Enums.CommRelationType.ReportJuActivityIllegalContent, context.Request["id"], context.Request["reason"]);
        }

        /// <summary>
        /// 文章收藏
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string FavoriteArticle(HttpContext context)
        {
            return AddCurrUserRelation(BLLJIMP.Enums.CommRelationType.JuActivityFavorite, context.Request["id"]);
        }

        /// <summary>
        /// 文章关注
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string FollowArticle(HttpContext context)
        {
            return AddCurrUserRelation(BLLJIMP.Enums.CommRelationType.JuActivityFollow, context.Request["id"]);
        }

        /// <summary>
        /// 取消关注文章
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DisFollowArticle(HttpContext context)
        {
            return DisCurrUserRelation(BLLJIMP.Enums.CommRelationType.JuActivityFollow, context.Request["id"]);
        }

        /// <summary>
        /// 内容点赞（文章、评论、回复）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string PraiseContent(HttpContext context)
        {
            return AddCurrUserRelation(BLLJIMP.Enums.CommRelationType.JuActivityPraise, context.Request["id"]);
        }

        /// <summary>
        /// 取消内容点赞（文章、评论、回复）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DisPraiseContent(HttpContext context)
        {
            return DisCurrUserRelation(BLLJIMP.Enums.CommRelationType.JuActivityPraise, context.Request["id"]);
        }

        /// <summary>
        /// 关注文章分类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string FollowArticleCategory(HttpContext context)
        {
            return AddCurrUserRelation(BLLJIMP.Enums.CommRelationType.FollowArticleCategory, context.Request["id"]);
        }

        /// <summary>
        /// 取消关注文章分类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DisFollowArticleCategory(HttpContext context)
        {
            return DisCurrUserRelation(BLLJIMP.Enums.CommRelationType.FollowArticleCategory, context.Request["id"]);
        }

        /// <summary>
        /// 关注文章
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string FollowContent(HttpContext context)
        {
            return AddCurrUserRelation(BLLJIMP.Enums.CommRelationType.JuActivityFollow, context.Request["id"]);
        }

        /// <summary>
        /// 取消关注文章
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DisFollowContent(HttpContext context)
        {
            return DisCurrUserRelation(BLLJIMP.Enums.CommRelationType.JuActivityFollow, context.Request["id"]);
        }

        /// <summary>
        /// 取消收藏文章
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DisFavoriteArticle(HttpContext context)
        {
            return DisCurrUserRelation(BLLJIMP.Enums.CommRelationType.JuActivityFavorite, context.Request["id"]);
        }


        /// <summary>
        /// 内容点赞（评论、回复）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string PraiseReview(HttpContext context)
        {

            return AddCurrUserRelation(BLLJIMP.Enums.CommRelationType.ReviewPraise, context.Request["id"]);
        }

        /// <summary>
        /// 取消内容点赞（评论、回复）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DisReviewContent(HttpContext context)
        {
            return DisCurrUserRelation(BLLJIMP.Enums.CommRelationType.ReviewPraise, context.Request["id"]);
        }

        /// <summary>
        /// 内容收藏（评论、回复）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string FavoriteReview(HttpContext context)
        {
            return AddCurrUserRelation(BLLJIMP.Enums.CommRelationType.ReviewFavorite, context.Request["id"]);
        }

        /// <summary>
        /// 取消收藏（评论、回复）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DisFavoriteReview(HttpContext context)
        {
            return DisCurrUserRelation(BLLJIMP.Enums.CommRelationType.ReviewFavorite, context.Request["id"]);
        }

        /// <summary>
        /// 举报评论回复
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ReportIllegalReview(HttpContext context)
        {
            return AddCurrUserRelation(BLLJIMP.Enums.CommRelationType.ReportReviewIllegalContent, context.Request["id"], context.Request["reason"]);
        }


        #region 内部方法
        /// <summary>
        /// 解除用户和JuActivity内容的关系
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <returns></returns>
        protected string DisCurrUserRelation(BLLJIMP.Enums.CommRelationType rtype, string mainId)
        {
            resp.isSuccess = false;

            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (!this.bLLCommRelation.ExistRelation(rtype, mainId, this.currentUserInfo.UserID))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.bLLCommRelation.DelCommRelation(rtype, mainId, this.currentUserInfo.UserID)){
                resp.isSuccess = true;

                if (rtype == BLLJIMP.Enums.CommRelationType.JuActivityFollow) //取消关注
                {
                    //关注数直接修改到主表
                    int followCount = bLLCommRelation.GetRelationCount(rtype, mainId, null);
                    bll.Update(new JuActivityInfo(), string.Format("FollowCount={0}", followCount), string.Format("JuActivityID={0}", mainId));
                }
                else if (rtype == BLLJIMP.Enums.CommRelationType.JuActivityFavorite) //取消收藏
                {
                    //收藏数直接修改到主表
                    int favoriteCount = bLLCommRelation.GetRelationCount(rtype, mainId, null);
                    bll.Update(new JuActivityInfo(), string.Format("FavoriteCount={0}", favoriteCount), string.Format("JuActivityID={0}", mainId));
                }
            }
            else
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 添加用户和JuActivity内容的关系
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <returns></returns>
        protected string AddCurrUserRelation(BLLJIMP.Enums.CommRelationType rtype, string mainId, string ExpandId = "")
        {
            if (mainId == "0" || string.IsNullOrWhiteSpace(mainId))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            //if (bll.GetCount<JuActivityInfo>(string.Format(" JuActivityID = {0} ", mainId)) == 0)
            //{
            //    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
            //    return Common.JSONHelper.ObjectToJson(resp);
            //}

            if (this.bLLCommRelation.ExistRelation(rtype, mainId, this.currentUserInfo.UserID))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.bLLCommRelation.AddCommRelation(rtype, mainId, this.currentUserInfo.UserID, ExpandId))
            {
                resp.isSuccess = true;
                if (rtype == BLLJIMP.Enums.CommRelationType.JuActivityFollow)
                {
                    JuActivityInfo article = bll.GetJuActivity(int.Parse(mainId), false);

                    //关注数直接修改到主表
                    int followCount = bLLCommRelation.GetRelationCount(rtype, mainId, null);
                    bll.Update(article, string.Format("FollowCount={0}", followCount), string.Format("JuActivityID={0}", article.JuActivityID));

                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.FollowArticle, this.currentUserInfo, article, article.UserID, null);
                }
                else if (rtype == BLLJIMP.Enums.CommRelationType.JuActivityFavorite)
                {
                    JuActivityInfo article = bll.GetJuActivity(int.Parse(mainId), false);

                    //收藏数直接修改到主表
                    int favoriteCount = bLLCommRelation.GetRelationCount(rtype, mainId, null);
                    bll.Update(article, string.Format("FavoriteCount={0}", favoriteCount), string.Format("JuActivityID={0}", article.JuActivityID));

                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.FavoriteArticle, this.currentUserInfo, article, article.UserID, null);
                }
                else if (rtype == BLLJIMP.Enums.CommRelationType.ReportJuActivityIllegalContent)
                {
                    JuActivityInfo article = bll.GetJuActivity(int.Parse(mainId), false);
                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.ReportJuActivityIllegalContent, this.currentUserInfo, article, article.UserID, null);
                }
                else if (rtype == BLLJIMP.Enums.CommRelationType.JuActivityPraise)
                {
                    JuActivityInfo article = bll.GetJuActivity(int.Parse(mainId), false);

                    //点赞数直接修改到主表
                    int praiseCount = bLLCommRelation.GetRelationCount(rtype, mainId, null);
                    bll.Update(article, string.Format("PraiseCount={0}", praiseCount), string.Format("JuActivityID={0}", article.JuActivityID));

                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.JuActivityPraise, this.currentUserInfo, article, article.UserID, null);
                }
                else if (rtype == BLLJIMP.Enums.CommRelationType.ReviewPraise)
                {
                    ReviewInfo review = bllReview.GetReviewInfo(int.Parse(mainId));
                    if (review.ReviewType == "Answer")
                    {
                        bllUser.AddUserScoreDetail(review.UserId, EnumStringHelper.ToString(ScoreDefineType.BePraise), this.bll.WebsiteOwner, null, null);
                    }
                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.ReviewPraise, this.currentUserInfo, null, review.UserId, review.ReviewContent);
                }
                else if (rtype == BLLJIMP.Enums.CommRelationType.ReportReviewIllegalContent)
                {
                    ReviewInfo review = bllReview.GetReviewInfo(int.Parse(mainId));
                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.ReportReviewIllegalContent, this.currentUserInfo, null, review.UserId, review.ReviewContent);
                }

            }
            else
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 结构化文章
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isHasContent"></param>
        /// <returns></returns>
        public dynamic StructureArticle(JuActivityInfo item, bool isHasContent, UserInfo curUser, bool chkFriend = false)
        {

            string summary = item.Summary;
            if (string.IsNullOrWhiteSpace(summary) && !string.IsNullOrWhiteSpace(item.ActivityDescription))
            {
                summary = MySpider.MyRegex.RemoveHTMLTags(item.ActivityDescription);
                if (summary.Length > 160) summary = summary.Substring(0, 160) + "...";
            }

            bool isFriend = false;

            if (chkFriend && curUser != null)
            {
                if (item.PubUser == null || item.PubUser.UserType == 6 || curUser.UserType==6 || item.PubUser.AutoID == curUser.AutoID)
                {
                    isFriend = true;
                }
                else
                {
                    isFriend = bLLCommRelation.ExistRelation(CommRelationType.Friend, item.PubUser.AutoID.ToString(), curUser.AutoID.ToString());
                }
            }

            if (item.PubUser != null && item.PubUser.UserType == 6)
            {
                isFriend = true;
            }

            return new
            {
                id = item.JuActivityID,
                title = item.ActivityName,
                summary = summary,
                type = item.ArticleType,
                content = isHasContent ? item.ActivityDescription : "",
                createDate = item.CreateDate.ToString(),
                commentCount = item.CommentCount,//评论数
                praiseCount = item.PraiseCount,//点赞数
                favoriteCount = item.FavoriteCount,//收藏数
                followCount = item.FollowCount,
                rewardTotal = item.RewardTotal,//打赏总数额
                currUserIsPraise = item.CurrUserIsPraise,
                currUserIsFavorite = item.CurrUserIsFavorite,
                currUserIsFollow = item.CurrUserIsFollow,
                pubUser = item.PubUser == null ? null : new
                {
                    id = item.PubUser == null ? 0 : item.PubUser.AutoID,
                    userId = item.PubUser == null ? "" : item.PubUser.UserID,
                    userName = item.PubUser == null ? "" : bllUser.GetUserDispalyName(item.PubUser),
                    avatar = item.PubUser == null ? "" : bllUser.GetUserDispalyAvatar(item.PubUser),
                    isTutor = item.PubUser == null ? false : bllUser.IsTutor(item.PubUser),
                    score = item.PubUser == null ? 0 : item.PubUser.TotalScore,
                    times = item.PubUser == null ? 0 : item.PubUser.OnlineTimes,
                    describe = item.PubUser == null ? "" : item.PubUser.Description,
                    isFriend = isFriend
                },
                pv = item.PV,
                categoryName = item.CategoryName,
                imgSrc = string.IsNullOrWhiteSpace(item.ThumbnailsPath) ? "" : bll.GetImgUrl(item.ThumbnailsPath),
                categoryId = item.CategoryId,
                tags = item.Tags,
                k1 = item.K1,
                k2 = item.K2,
                k3 = item.K3,
                k4 = item.K4,
                k5 = item.K5,
                k6 = item.K6,
                k7 = item.K7,
                k8 = item.K8,
                k9 = item.K9,
                k10 = item.K10,
                k11 = item.K11,
                k12 = item.K12,
                k13 = item.K13,
                k14 = item.K14,
                k15 = item.K15,
                province = item.Province,
                redirect_url = item.RedirectUrl,
                ishide=item.IsHide
            };
        }

        #endregion

        #endregion


        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetArticleCateList(HttpContext context)
        {
            /*
             * 必填：
             * 传入分类类型 type
             * 
             * 选填：
             * 传入父 preId
             * 传入页码 pageIndex
             * 传入页大小 pageSize
             * 
             */

            string type = context.Request["type"];

            int preId = Convert.ToInt32(context.Request["preId"]),
                pageIndex = Convert.ToInt32(context.Request["pageIndex"]),
                pageSize = Convert.ToInt32(context.Request["pageSize"]),
                totalCount = 0;

            if (pageIndex == 0) pageIndex = 1;

            if (pageSize == 0) pageSize = int.MaxValue;

            var dataList = this.bllArticleCategory.GetCateList(out totalCount, type, preId, this.bll.WebsiteOwner, pageSize, pageIndex);

            List<dynamic> result = new List<dynamic>();

            foreach (var item in dataList)
            {
                result.Add(new
                {
                    id = item.AutoID,
                    name = item.CategoryName,
                    createTime = item.CreateTime.ToString(),
                    img = item.ImgSrc,
                    summary = item.Summary,
                    followUserCount = bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowArticleCategory, item.AutoID.ToString(), null),
                    userIsFollow = this.currentUserInfo == null ? false : bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowArticleCategory, item.AutoID.ToString(), this.currentUserInfo.UserID) > 0,
                    articleCount = bll.QueryJuActivityCountByCategoryId(item.AutoID.ToString(), "", false)
                });
            }

            return Common.JSONHelper.ObjectToJson(new
            {
                totalCount = totalCount,
                list = result
            });
        }


        /// <summary>
        /// 成员列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetFollowArticleCategoryUser(HttpContext context)
        {
            string categoryId = context.Request["cateId"];
            string keyword = context.Request["keyword"];
            int pageIndex = int.Parse(context.Request["pageIndex"]);
            int pageSize = int.Parse(context.Request["pageSize"]);

            List<CommRelationInfo> rels = new List<CommRelationInfo>();
            int TCount = 0;
            if (string.IsNullOrWhiteSpace(keyword))
            {
                rels = bLLCommRelation.GetRelationListDesc(BLLJIMP.Enums.CommRelationType.FollowArticleCategory, categoryId, null, pageIndex, pageSize, out TCount);
            }
            else
            {
                rels = bLLCommRelation.GetRelationListDescByUserName(BLLJIMP.Enums.CommRelationType.FollowArticleCategory, categoryId, null, keyword, pageIndex, pageSize, out TCount);
            }
            List<dynamic> result = new List<dynamic>();

            foreach (var item in rels)
            {
                UserInfo user = bllUser.GetUserInfo(item.RelationId);
                if (user == null) continue;

                result.Add(new
                {
                    id = user.AutoID,
                    userId = user.UserID,
                    userName = bllUser.GetUserDispalyName(user),
                    avatar = bllUser.GetUserDispalyAvatar(user),
                    isTutor = bllUser.IsTutor(user)
                });
            }


            return Common.JSONHelper.ObjectToJson(new
            {
                totalcount = TCount,
                list = result
            });
        }


        #region 省市区接口
        /// <summary>
        /// 省份城市区域
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetGetKeyVauleDatas(HttpContext context)
        {
            string type = context.Request["type"];//类型
            string prekey = context.Request["prekey"];//上级

            List<KeyVauleDataInfo> keyVaules = new List<KeyVauleDataInfo>();

            int totalCount = 0;
            if (type == "province")// 省份
            {
                keyVaules = bllKeyValueData.GetProvinces(out totalCount);
            }
            else if (type == "city")// 城市
            {
                keyVaules = bllKeyValueData.GetCitys(prekey, out totalCount);
            }
            else if (type == "district")// 区域
            {
                keyVaules = bllKeyValueData.GetDistricts(prekey, out totalCount);
            }
            else
            {
                keyVaules = bllKeyValueData.GetKeyVauleDataInfoList(type, prekey, this.bll.WebsiteOwner);
            }

            var result = from p in keyVaules
                         select new
                         {
                             id = p.DataKey,
                             name = p.DataValue
                         };

            return Common.JSONHelper.ObjectToJson(new
            {
                totalcount = totalCount,
                list = result
            });
        }
        #endregion

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Login(HttpContext context)
        {

            Login apiResult = new Login();
            string userId = context.Request["userid"];
            string pwd = context.Request["pwd"];
            string msg;

            string hasCheckCode = context.Request["hascheckcode"];
            string checkCode = context.Request["checkcode"];
            var serverCheckCode = context.Session["CheckCode"];

            if (!string.IsNullOrWhiteSpace(hasCheckCode) && hasCheckCode == "1" && serverCheckCode != null)
            {
                if (
                    string.IsNullOrWhiteSpace(checkCode)
                        ||
                    !checkCode.Equals(serverCheckCode.ToString(), StringComparison.OrdinalIgnoreCase)
                    )
                {
                    apiResult.message = "验证码错误";
                    apiResult.errcode = (int)BLLJIMP.Enums.APIErrCode.CheckCodeErr;
                    return Common.JSONHelper.ObjectToJson(apiResult);
                }
            }

            UserInfo userInfo = new UserInfo();
            if (bllUser.Login(userId, pwd, out userInfo, out msg, this.bll.WebsiteOwner))
            {
                bll.ToLog("login log: 登陆成功后继续");

                if (DateTime.Now.ToString("yyyy-MM-dd") != userInfo.LastLoginDate.ToString("yyyy-MM-dd"))
                {
                    bll.ToLog("login log: 登录加积分");
                    var addLoginScoreResult = bllUser.AddUserScoreDetail(userInfo.UserID, EnumStringHelper.ToString(ScoreDefineType.DayLogin), this.bll.WebsiteOwner, null, null);
                    bll.ToLog("login log: 登录加积分结束" + addLoginScoreResult);
                }
                
                bllUser.UpdateLoginInfo(userInfo);

                bll.ToLog("login log: 修改登录日志成功");

                context.Session[SessionKey.UserID] = userInfo.UserID;
                apiResult.issuccess = true;
                apiResult.userid = userInfo.UserID;
                apiResult.headimg = this.bllUser.GetUserDispalyAvatar(userInfo);
                apiResult.userName = this.bllUser.GetUserDispalyName(userInfo);
                apiResult.avatar = this.bllUser.GetUserDispalyAvatar(userInfo);
                apiResult.phone = userInfo.Phone;

                bll.ToLog("login log: 全部处理完毕");

            }
            else
            {
                apiResult.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResult.message = "用户名或密码错误";
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Logout(HttpContext context)
        {
            context.Session.Clear();
            resp.isSuccess = true;
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 检查登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string IsLogin(HttpContext context)
        {

            CheckLogin apiResult = new CheckLogin();
            if (bllUser.IsLogin)
            {
                return Common.JSONHelper.ObjectToJson(new
                {
                    islogin = true,
                    id = currentUserInfo.AutoID,
                    userid = currentUserInfo.UserID,
                    userName = this.bllUser.GetUserDispalyName(this.currentUserInfo),
                    phone = this.currentUserInfo.Phone,
                    avatar = this.bllUser.GetUserDispalyAvatar(this.currentUserInfo),
                    userUnReadNoticeCount = bllSystemNotice.GetUnReadMsgCount(currentUserInfo.UserID, null),
                    userType=currentUserInfo.UserType
                });
            }
            return Common.JSONHelper.ObjectToJson(apiResult);
        }
        /// <summary>
        /// 获取首页幻灯片
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSlide(HttpContext context)
        {

            SlideApi apiResult = new SlideApi();
            apiResult.list = new List<SlideModel>();
            var SourceData = bll.GetList<Slide>(string.Format("WebsiteOwner='{0}' Order by Sort ASC", bll.WebsiteOwner));
            apiResult.totalcount = bll.GetCount<Slide>(string.Format("WebsiteOwner='{0}'", bll.WebsiteOwner));
            foreach (var item in SourceData)
            {
                SlideModel model = new SlideModel();
                model.imgurl = bll.GetImgUrl(item.ImageUrl);
                model.link = item.Link;
                apiResult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetCurrentUserInfo(HttpContext context)
        {
            UserInfoModel apiResult = new UserInfoModel();
            //UserInfo TagetUserInfo = new UserInfo();
            // if (!string.IsNullOrEmpty(context.Request["userid"]))
            // {
            //TagetUserInfo = bllUser.GetUserInfo(context.Request["userid"], WebSiteOwner);
            //}
            //else
            //{
            //TagetUserInfo = CurrentUserInfo;

            //}
            if (currentUserInfo != null)
            {
                apiResult.headimg = currentUserInfo.WXHeadimgurl;
                apiResult.truename = currentUserInfo.TrueName;
                apiResult.postion = currentUserInfo.Postion;
                apiResult.company = currentUserInfo.Company;
                apiResult.totalscore = currentUserInfo.TotalScore;
                if (string.IsNullOrEmpty(apiResult.truename))
                {
                    apiResult.truename = currentUserInfo.WXNickname;
                }
                if (IsMaster(currentUserInfo.UserID, bll.WebsiteOwner))
                {
                    TutorInfo MasterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", currentUserInfo.UserID));
                    apiResult.headimg = bll.GetImgUrl(MasterInfo.TutorImg);
                    apiResult.truename = MasterInfo.TutorName;
                    apiResult.postion = MasterInfo.Position;
                    apiResult.company = MasterInfo.Company;
                    apiResult.totalscore = currentUserInfo.TotalScore;
                }
            }
            return Common.JSONHelper.ObjectToJson(apiResult);
        }

        /// <summary>
        ///更新当前用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateCurrentUserInfo(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                goto outoff;
            }
            UserInfoModel requestUserinfo = bll.ConvertRequestToModel<UserInfoModel>(new UserInfoModel());
            //UserInfo CurrentUserInfo = bll.GetCurrentUserInfo();
            if (currentUserInfo != null)
            {
                currentUserInfo.TrueName = requestUserinfo.truename;
                currentUserInfo.Postion = requestUserinfo.postion;
                currentUserInfo.Company = requestUserinfo.company;
                if (bll.Update(currentUserInfo, string.Format(" TrueName='{0}',Postion='{1}',Company='{2}'", currentUserInfo.TrueName, currentUserInfo.Postion, currentUserInfo.Company), string.Format(" AutoID={0}", currentUserInfo.AutoID)) > 0)
                {
                    resp.errcode = 0;
                    resp.errmsg = "更新成功";
                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "更新失败";
                }
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMyActivityList(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            string sort = context.Request["sort"];
            string cateId = context.Request["cateid"];
            string keyword = context.Request["keyword"];
            return QueryActivityList(pageIndex, pageSize, sort, cateId, keyword, true);
        }

        /// <summary>
        /// 根据条件查询活动
        /// </summary>
        /// <param name="pageindex">第几页</param>
        /// <param name="pagesize">每页取几条</param>
        /// <param name="sort">排序</param>
        /// <param name="cateid">分类id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="myactivity">是否是我的活动false 否true 是</param>
        /// <returns></returns>
        private string QueryActivityList(int pageindex, int pagesize, string sort = "", string cateid = "", string keyword = "", bool myactivity = false, int year = 0, int month = 0)
        {

            string orderBy = "";//默认排序
            switch (sort)
            {
                case "time":
                    orderBy = " ActivityStartDate DESC";
                    break;
                case "signcount ":
                    orderBy = " SignUpCount DESC";
                    break;
                default:
                    orderBy = " Sort DESC";
                    break;
            }
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" ArticleType='activity' AND IsDelete=0 AND WebsiteOwner='{0}'", bll.WebsiteOwner);

            if (year > 0 && month > 0)
            {
                sbWhere.AppendFormat(" And DATEDIFF(M,ActivityStartDate,'{0}-{1}-1') = 0 ", year, month);
            }

            if (!string.IsNullOrEmpty(cateid))
            {
                sbWhere.AppendFormat(" And CategoryId={0}", cateid);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sbWhere.AppendFormat(" And (ActivityName like'%{0}%' Or ActivityAddress like'%{0}%')", keyword);
            }
            if (myactivity)
            {
                sbWhere.AppendFormat(" And SignUpActivityID in(select ActivityID from ZCJ_ActivityDataInfo where UserId='{0}')", bll.GetCurrUserID());
            }

            sbWhere.Append(" AND IsSys = 0 ");

            ActivityApi apiResult = new ActivityApi();
            apiResult.list = new List<Activity>();
            apiResult.totalcount = bll.GetCount<JuActivityInfo>(sbWhere.ToString());
            var sourceData = bll.GetLit<JuActivityInfo>(pagesize, pageindex, sbWhere.ToString(), orderBy);
            foreach (var item in sourceData)
            {
                Activity model = new Activity();
                model.activityid = item.JuActivityID;
                model.activityimage = bll.GetImgUrl(item.ThumbnailsPath);
                model.activityname = item.ActivityName;
                model.address = item.ActivityAddress;
                model.categoryname = item.CategoryName;
                model.pv = item.PV;
                model.signcount = item.SignUpTotalCount;
                if (item.ActivityStartDate != null)
                {
                    model.time = bll.GetTimeStamp((DateTime)item.ActivityStartDate);
                }
                if (item.IsHide == 1)
                {
                    model.status = 1;
                }
                if ((item.MaxSignUpTotalCount > 0) && (item.SignUpTotalCount >= item.MaxSignUpTotalCount))
                {
                    model.status = 2;
                }
                model.score = item.ActivityIntegral;
                model.starttimestr = item.ActivityStartDate == null ? "" : item.ActivityStartDate.Value.ToString("yyyy-MM-dd HH:mm");

                if (!string.IsNullOrWhiteSpace(item.Tags))
                {
                    model.tags = item.Tags.Split(',').Take(5).ToList();
                }

                apiResult.list.Add(model);

            }
            return Common.JSONHelper.ObjectToJson(apiResult);


        }

        /// <summary>
        /// 获取积分记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetScoreRecordList(HttpContext context)
        {

            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            ScoreRecordApi apiResult = new ScoreRecordApi();
            apiResult.list = new List<ScoreRecord>();
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" UserId='{0}' AND WebsiteOwner='{1}'", currentUserInfo.UserID, bll.WebsiteOwner);
            apiResult.totalcount = bll.GetCount<BLLJIMP.Model.WBHScoreRecord>(sbWhere.ToString());
            apiResult.totalscore = (int)currentUserInfo.TotalScore;
            List<BLLJIMP.Model.WBHScoreRecord> SourceData = bll.GetLit<BLLJIMP.Model.WBHScoreRecord>(pageSize, pageIndex, sbWhere.ToString(), " AutoId Desc");
            foreach (var item in SourceData)
            {
                ScoreRecord model = new ScoreRecord();
                model.title = item.NameStr;
                model.score = item.ScoreNum;
                model.time = bll.GetTimeStamp(item.InsertDate);
                apiResult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 添加关注
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddAttention(HttpContext context)
        {
            string toUserId = context.Request["touserid"];
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                goto outoff;
            }
            if (string.IsNullOrEmpty(toUserId))
            {
                resp.errcode = 3;
                resp.errmsg = "关注用户名不能为空";
                goto outoff;
            }
            UserInfo toUserInfo = bllUser.GetUserInfo(toUserId);
            if (toUserInfo == null)
            {
                resp.errcode = 4;
                resp.errmsg = "不能关注该理财师";
                goto outoff;
            }
            if (bll.GetCount<UserFollowChain>(string.Format(" FromUserId='{0}' And ToUserId='{1}'", currentUserInfo.UserID, toUserInfo.UserID)) > 0)
            {
                resp.errcode = 2;
                resp.errmsg = "已经关注过了";
                goto outoff;
            }
            UserFollowChain model = new UserFollowChain();
            model.FromUserId = currentUserInfo.UserID;
            model.ToUserId = toUserInfo.UserID;
            if (bll.Add(model))
            {
                resp.errcode = 0;
                resp.errmsg = "关注成功";
                goto outoff;
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "关注失败";
                goto outoff;
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CancelAttention(HttpContext context)
        {
            string toUserId = context.Request["touserid"];
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                goto outoff;
            }
            if (string.IsNullOrEmpty(toUserId))
            {
                resp.errcode = 3;
                resp.errmsg = "用户名不能为空";
                goto outoff;
            }
            UserInfo toUserInfo = bllUser.GetUserInfo(toUserId);
            if (toUserInfo == null)
            {
                resp.errcode = 4;
                resp.errmsg = "关注用户不存在";
                goto outoff;
            }
            if (bll.Delete(new UserFollowChain(), string.Format(" FromUserId='{0}' And ToUserId='{1}'", currentUserInfo.UserID, toUserInfo.UserID)) > 0)
            {
                resp.errcode = 0;
                resp.errmsg = "取消关注成功";
                goto outoff;
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "取消关注失败";
                goto outoff;
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取我关注的人列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMyAttentionList(HttpContext context)
        {

            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //我关注的人肯定是理财师
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            AttentionApi apiResult = new AttentionApi();
            apiResult.list = new List<AttentionUserinfo>();
            apiResult.totalcount = bll.GetCount<UserFollowChain>(string.Format(" FromUserId='{0}'", currentUserInfo.UserID));
            var sourceData = bll.GetLit<UserFollowChain>(pageSize, pageIndex, string.Format(" FromUserId='{0}'", currentUserInfo.UserID), " AutoId DESC");
            foreach (var item in sourceData)
            {
                TutorInfo MasterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", item.ToUserId));
                if (MasterInfo != null)
                {
                    AttentionUserinfo model = new AttentionUserinfo();
                    model.headimg = bll.GetImgUrl(MasterInfo.TutorImg);
                    model.userid = MasterInfo.UserId;
                    model.truename = MasterInfo.TutorName;
                    model.postion = MasterInfo.Position;
                    model.digest = MasterInfo.Digest;
                    model.pv = MasterInfo.Pv;
                    model.praisecount = MasterInfo.TutorLikes;
                    model.ismaster = true;
                    model.isattention = true;
                    model.tags = new List<string>();
                    foreach (var tagid in MasterInfo.ProfessionalStr.Split(','))
                    {
                        if (!string.IsNullOrEmpty(tagid))
                        {
                            var tag = bll.Get<ArticleCategory>(string.Format(" AutoID={0}", tagid));
                            if (tag != null)
                            {
                                model.tags.Add(tag.CategoryName);

                            }

                        }
                    }
                    apiResult.list.Add(model);

                }

            }

            return Common.JSONHelper.ObjectToJson(apiResult);

        }
        /// <summary>
        /// 获取我的粉丝列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMyFansList(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            AttentionApi apiResult = new AttentionApi();
            apiResult.list = new List<AttentionUserinfo>();
            var sourceData = bll.GetLit<UserFollowChain>(pageSize, pageIndex, string.Format(" ToUserId='{0}'", currentUserInfo.UserID), " AutoId DESC");
            apiResult.totalcount = bll.GetCount<UserFollowChain>(string.Format(" ToUserId='{0}'", currentUserInfo.UserID));
            foreach (var item in sourceData)
            {
                AttentionUserinfo model = new AttentionUserinfo();
                if (IsMaster(item.FromUserId, bll.WebsiteOwner))//粉丝是理财师
                {
                    TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", item.FromUserId));
                    //if (MasterInfo != null)
                    //{
                    model.headimg = bll.GetImgUrl(masterInfo.TutorImg);
                    model.userid = masterInfo.UserId;
                    model.truename = masterInfo.TutorName;
                    model.postion = masterInfo.Position;
                    model.digest = masterInfo.Digest;
                    model.pv = masterInfo.Pv;
                    model.praisecount = masterInfo.TutorLikes;
                    model.ismaster = true;
                    if (bll.GetCount<UserFollowChain>(string.Format(" FromUserId='{0}' And ToUserId='{1}'", currentUserInfo.UserID, item.FromUserId)) > 0)
                    {
                        model.isattention = true;
                    }
                    model.tags = new List<string>();
                    foreach (var tagid in masterInfo.ProfessionalStr.Split(','))
                    {
                        if (!string.IsNullOrEmpty(tagid))
                        {
                            var tag = bll.Get<ArticleCategory>(string.Format(" AutoID={0}", tagid));
                            if (tag != null)
                            {
                                model.tags.Add(tag.CategoryName);

                            }

                        }
                    }
                    //}

                }
                else
                {
                    UserInfo userInfo = bllUser.GetUserInfo(item.FromUserId);
                    if (userInfo != null)
                    {
                        model.headimg = userInfo.WXHeadimgurl;
                        model.userid = userInfo.UserID;
                        model.truename = userInfo.TrueName;
                        model.postion = userInfo.Postion;
                        if (string.IsNullOrEmpty(model.truename))
                        {
                            model.truename = userInfo.WXNickname;
                        }

                    }
                }
                apiResult.list.Add(model);

            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 获取我提交过的问卷列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMyQuestionnaireList(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            QuestionnaireApi apiResult = new QuestionnaireApi();
            apiResult.list = new List<QuestionnaireModel>();
            var sourceData = bll.GetLit<Questionnaire>(pageSize, pageIndex, string.Format(" QuestionnaireID in(select QuestionnaireID from ZCJ_QuestionnaireRecord where UserId='{0}')", currentUserInfo.UserID), " QuestionnaireID DESC");
            apiResult.totalcount = bll.GetCount<Questionnaire>(string.Format(" QuestionnaireID in(select QuestionnaireID from ZCJ_QuestionnaireRecord where UserId='{0}')", currentUserInfo.UserID));
            foreach (var item in sourceData)
            {
                QuestionnaireModel model = new QuestionnaireModel();
                model.id = item.QuestionnaireID;
                model.title = item.QuestionnaireName;
                model.content = item.QuestionnaireSummary;
                model.imgurl = bll.GetImgUrl(item.QuestionnaireImage);
                model.link = string.Format("http://{0}/app/questionnaire/wap/questionnaire.aspx?id={1}", context.Request.Url.Host, item.QuestionnaireID);
                apiResult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 获取积分排行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetScoreRankList(HttpContext context)
        {
            ScoreRank apiResult = new ScoreRank();
            apiResult.list = new List<UserInfoModel>();
            int pageSize = int.Parse(context.Request["pagesize"]);
            var sourceData = bll.GetLit<UserInfo>(pageSize, 1, string.Format(" WebsiteOwner='{0}'", bll.WebsiteOwner), " TotalScore DESC,AutoID ASC");
            foreach (var item in sourceData)
            {
                UserInfoModel model = new UserInfoModel();
                if (IsMaster(item.UserID, bll.WebsiteOwner))//是理财师
                {
                    TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", item.UserID));
                    model.headimg = bll.GetImgUrl(masterInfo.TutorImg);
                    model.truename = masterInfo.TutorName;

                }
                else//普通用户
                {
                    model.headimg = item.WXHeadimgurl;
                    model.truename = item.TrueName;
                    if (string.IsNullOrEmpty(item.TrueName))
                    {
                        model.truename = item.WXNickname;
                    }
                }
                model.totalscore = item.TotalScore;
                apiResult.list.Add(model);
            }
            apiResult.totalcount = sourceData.Count;
            return Common.JSONHelper.ObjectToJson(apiResult);
        }

        /// <summary>
        /// 消息中心
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetNoticelistt(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            string keyword = context.Request["keyword"];

            List<SystemNotice> msgs = bllSystemNotice.GetMsgList(pageSize, pageIndex, currentUserInfo.UserID, null, keyword, null);
            int tCount = bllSystemNotice.GetMsgCount(currentUserInfo.UserID, null);
            List<dynamic> result = new List<dynamic>();
            foreach (SystemNotice msg in msgs)
            {
                UserInfo nUser = bllUser.GetUserInfo(msg.UserId);
                bllSystemNotice.SetReaded(msg.AutoID);
                result.Add(new
                {
                    content = msg.Ncontent.Replace("/FileUpload/", string.Format("http://{0}/FileUpload/", context.Request.Url.Host)),
                    date = msg.InsertTime.ToString(),
                    avatar = bllUser.GetUserDispalyAvatar(nUser)
                });
            }
            return Common.JSONHelper.ObjectToJson(new
            {
                totalcount = tCount,
                list = result
            });
        }

        /// <summary>
        /// 获取系统通知
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSystemNoticeList(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            SystemNoticeApi apiResult = new SystemNoticeApi();
            int noticetype = int.Parse(context.Request["noticetype"]);
            switch (noticetype)
            {
                case 0://系统消息
                    noticetype = 1;
                    apiResult.totalcount = bllNotice.GetUnReadMsgCount(currentUserInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.SystemMessage);
                    break;
                case 1://问卷消息
                    noticetype = 21;
                    apiResult.totalcount = bllNotice.GetUnReadMsgCount(currentUserInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.QuestionaryReminder);
                    break;
                default:
                    noticetype = 1;
                    apiResult.totalcount = bllNotice.GetUnReadMsgCount(currentUserInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.SystemMessage);
                    break;
            }
            BLLSystemNotice.NoticeType noticeType = (BLLSystemNotice.NoticeType)noticetype;
            List<SystemNotice> systemNoticeList = bllNotice.GetUnReadMsgList(pageSize, pageIndex, currentUserInfo.UserID, noticeType);
            apiResult.list = new List<SystemNoticeModel>();
            foreach (var item in systemNoticeList)
            {

                SystemNoticeModel model = new SystemNoticeModel();
                model.id = item.AutoID;
                model.title = item.Title;
                model.content = item.Ncontent;
                if (model.content.Contains("/FileUpload/"))
                {
                    model.content = model.content.Replace("/FileUpload/", string.Format("http://{0}/FileUpload/", context.Request.Url.Host));
                }
                model.link = item.RedirectUrl;
                model.time = bll.GetTimeStamp(item.InsertTime);
                apiResult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);
        }
        /// <summary>
        /// 设置系统消息为已读
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetSystemNoticeReaded(HttpContext context)
        {
            string id = context.Request["id"];
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                goto outoff;
            }
            if (string.IsNullOrEmpty(id))
            {
                resp.errcode = 3;
                resp.errmsg = "通知编号不能为空";
                goto outoff;
            }
            SystemNotice model = bll.Get<SystemNotice>(string.Format("AutoID={0}", id));
            if (model == null)
            {
                resp.errcode = 2;
                resp.errmsg = "通知不存在";
                goto outoff;
            }
            if (model.Readtime == null)
            {
                model.Readtime = DateTime.Now;
            }
            if (bll.Update(model))
            {
                resp.errcode = 0;
                resp.errmsg = "操作成功";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "操作失败";
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetUnReadSystemNoticeCount(HttpContext context)
        {
            //string noticetype = context.Request["noticetype"];
            int unReadSysMsgCount = 0;//未读系统消息数量
            int unReadSysQuestionaryCount = 0;//未读问卷数量
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                goto outoff;
            }
            //if (string.IsNullOrEmpty(noticetype))
            //{
            //    resp.errcode = 3;
            //    resp.errmsg = "通知类型不能为空";
            //    goto outoff;
            //}
            unReadSysMsgCount = bllNotice.GetUnReadMsgCount(currentUserInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.SystemMessage);
            unReadSysQuestionaryCount = bllNotice.GetUnReadMsgCount(currentUserInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.QuestionaryReminder);
        outoff:
            return "{\"systemmsg\":" + unReadSysMsgCount + ",\"questionary\":" + unReadSysQuestionaryCount + "}";

        }

        /// <summary>
        /// 获取理财师列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMasterList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            string keyword = context.Request["keyword"];
            string number = context.Request["number"];//第几届理财师
            string tagIds = context.Request["cateid"];//标签id

            string orderBy = " Number DESC,TutorName ASC";//默认排序
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" websiteOwner='{0}'", bll.WebsiteOwner);

            if (!string.IsNullOrEmpty(keyword))
            {
                sbWhere.AppendFormat(" And (TutorName like'%{0}%' or Position  like'%{0}%')", keyword);
            }
            if (!string.IsNullOrEmpty(number))
            {
                sbWhere.AppendFormat(" And Number={0}", number);
            }
            if (!string.IsNullOrEmpty(tagIds))
            {
                sbWhere.AppendFormat(" And charindex('{0}',ProfessionalStr)>0", tagIds);
            }

            MasteListApi apiResult = new MasteListApi();
            apiResult.list = new List<MasterModel>();
            apiResult.totalcount = bll.GetCount<TutorInfo>(sbWhere.ToString());

            var sourceData = bll.GetLit<TutorInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
            foreach (var item in sourceData)
            {
                MasterModel model = new MasterModel();
                model.headimg = bll.GetImgUrl(item.TutorImg);
                model.userid = item.UserId;
                if (string.IsNullOrEmpty(model.userid))
                {
                    model.userid = item.AutoId.ToString();
                }
                model.truename = item.TutorName;
                model.postion = item.Position;
                model.digest = item.Digest;
                model.pv = item.Pv;
                model.praisecount = item.TutorLikes;
                model.company = item.Company;
                if (!string.IsNullOrEmpty(item.UserId))
                {
                    model.canaskorattention = true;
                }
                model.tags = new List<string>();
                if (bll.IsLogin)
                {
                    model.isattention = IsAttention(currentUserInfo.UserID, item.UserId);
                }
                if (!string.IsNullOrEmpty(item.ProfessionalStr))
                {
                    foreach (var tagid in item.ProfessionalStr.Split(','))
                    {
                        if (!string.IsNullOrEmpty(tagid))
                        {
                            var tag = bll.Get<ArticleCategory>(string.Format(" AutoID={0}", tagid));
                            if (tag != null)
                            {
                                model.tags.Add(tag.CategoryName);

                            }

                        }
                    }
                }

                apiResult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 获取理财师详情
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMasterDetail(HttpContext context)
        {
            string userId = context.Request["userid"];
            if (string.IsNullOrEmpty(userId))
            {
                resp.errcode = 1;
                resp.errmsg = "用户名不能为空";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" websiteOwner='{0}'", bll.WebsiteOwner);
            sbWhere.AppendFormat(" And UserId='{0}'", userId);
            TutorInfo masterInfo = bll.Get<TutorInfo>(sbWhere.ToString());
            if (masterInfo == null)
            {
                masterInfo = bll.Get<TutorInfo>(string.Format(" websiteOwner='{0}' And AutoId='{1}'", bll.WebsiteOwner, userId));
                if (masterInfo == null)
                {
                    resp.errcode = 2;
                    resp.errmsg = "理财师不存在";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }
            MasterDetail model = new MasterDetail();
            model.headimg = bll.GetImgUrl(masterInfo.TutorImg);
            model.userid = masterInfo.UserId;
            model.truename = masterInfo.TutorName;
            model.postion = masterInfo.Position;
            model.digest = masterInfo.Digest;
            model.pv = masterInfo.Pv;
            if (!string.IsNullOrEmpty(masterInfo.UserId))
            {
                model.canaskorattention = true;
            }
            model.praisecount = masterInfo.TutorLikes;
            model.tags = new List<string>();
            if (bll.IsLogin)
            {
                model.isattention = IsAttention(currentUserInfo.UserID, masterInfo.UserId);
            }
            if (!string.IsNullOrEmpty(masterInfo.ProfessionalStr))
            {
                foreach (var tagid in masterInfo.ProfessionalStr.Split(','))
                {
                    if (!string.IsNullOrEmpty(tagid))
                    {
                        var tag = bll.Get<ArticleCategory>(string.Format(" AutoID={0}", tagid));
                        if (tag != null)
                        {
                            model.tags.Add(tag.CategoryName);

                        }

                    }
                }
            }
            //
            model.company = masterInfo.Company;
            model.introduction = masterInfo.TutorExplain;
            model.city = masterInfo.City;
            if (!string.IsNullOrEmpty(masterInfo.UserId))
            {
                model.attentioncount = bll.GetCount<UserFollowChain>(string.Format(" FromUserId='{0}'", masterInfo.UserId));
                model.fanscount = bll.GetCount<UserFollowChain>(string.Format(" ToUserId='{0}'", masterInfo.UserId));
                model.askcount = bll.GetCount<ReviewInfo>(string.Format(" UserId='{0}' And WebSiteOwner='{1}' And ReviewType='话题'", masterInfo.UserId, bll.WebsiteOwner));
                model.beaskcount = bll.GetCount<ReplyReviewInfo>(string.Format(" UserId='{0}' And WebSiteOwner='{1}' And ReviewType='话题'", masterInfo.UserId, bll.WebsiteOwner));

            }
            masterInfo.Pv++;
            bll.Update(masterInfo);
            return Common.JSONHelper.ObjectToJson(model);
        }


        /// <summary>
        /// 获取理财师标签列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMasterTagList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            MasterTagApi apiResult = new MasterTagApi();
            apiResult.list = new List<MasterTag>();
            var sourceData = bll.GetLit<ArticleCategory>(pageSize, pageIndex, string.Format(" CategoryType='Professional' And WebsiteOwner='{0}' ", bll.WebsiteOwner));
            apiResult.totalcount = bll.GetCount<ArticleCategory>(string.Format(" WebsiteOwner='{0}' AND CategoryType='Professional' ", bll.WebsiteOwner));
            foreach (var item in sourceData)
            {
                MasterTag model = new MasterTag();
                model.tagid = item.AutoID;
                model.tagname = item.CategoryName;
                apiResult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 检查是否已经关注
        /// </summary>
        /// <param name="fromuserid"></param>
        /// <param name="touserid"></param>
        /// <returns></returns>
        private bool IsAttention(string fromuserid, string touserid)
        {
            if (bll.GetCount<UserFollowChain>(string.Format(" FromUserId='{0}' And ToUserId='{1}'", fromuserid, touserid)) > 0)
            {
                return true;
            }
            return false;

        }

        /// <summary>
        /// 检查是否是理财师
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool IsMaster(string userId, string websiteowner)
        {
            if (bll.GetCount<TutorInfo>(string.Format(" UserId='{0}' And WebsiteOwner='{1}'", userId, websiteowner)) > 0)
            {
                return true;
            }
            return false;

        }


        /// <summary>
        /// 获取问答列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetAskList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            AskListApi apiResult = new AskListApi();
            apiResult.list = new List<Ask>();
            string reviewTitle = context.Request["keyword"];
            string ctype = context.Request["type"];
            string ismyask = context.Request["ismyask"];
            string haveReply = context.Request["havereply"];
            string sort = context.Request["sort"];
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbSort = new StringBuilder();

            sbWhere.AppendFormat(" ReviewType='话题' AND websiteOwner='{0}'", bll.WebsiteOwner);
            if (!string.IsNullOrEmpty(reviewTitle))
            {
                sbWhere.AppendFormat(" AND ReviewTitle like '%{0}%'", reviewTitle);
            }
            if (!string.IsNullOrEmpty(ctype))
            {
                sbWhere.AppendFormat(" AND CategoryType LIKE '%{0}%'", ctype);
            }
            if ((!string.IsNullOrEmpty(ismyask)) && (ismyask.Equals("true")))
            {
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)errcode.UnLogin;
                    resp.errmsg = "尚未登录";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                //if (juActivityBll.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}'", this.userInfo.UserID)) != null)
                //{
                sbWhere.AppendFormat(" AND (ForeignkeyId='{0}' Or UserId='{0}' Or AutoId in(select ReviewID from ZCJ_ReplyReviewInfo where UserId='{0}'))", currentUserInfo.UserID);
                //}
                //else
                //{
                //    sb.AppendFormat(" AND UserId='{0}'", this.userInfo.UserID);
                //}
                //sb.AppendFormat(" AND (ForeignkeyId='{0}' Or UserId='{0}')", this.userInfo.UserID);

            }
            else
            {
                //sbWhere.Append(" AND ReviewPower=0");
                // sbWhere.AppendFormat(" AND NumCount>0");
            }
            if (!string.IsNullOrEmpty(haveReply))
            {
                sbWhere.Append(" AND NumCount>0");

            }
            sbSort.Append(" ReplyDateTiem DESC ");
            if (!string.IsNullOrEmpty(sort))
            {


                if (sort.Equals("Newhf"))
                {
                    sbSort.Clear();
                    sbSort.Append(" ReplyDateTiem DESC");
                }
                if (sort.Equals("Mosthf"))
                {
                    sbSort.Clear();
                    sbSort.Append(" NumCount DESC");
                }

                if (sort.Equals("Mosthp"))
                {
                    sbSort.Clear();
                    sbSort.Append(" PraiseNum DESC, ReplyDateTiem DESC");
                }
            }
            apiResult.totalcount = bll.GetCount<ReviewInfo>(sbWhere.ToString());
            List<BLLJIMP.Model.ReviewInfo> SourceData = bll.GetLit<BLLJIMP.Model.ReviewInfo>(pageSize, pageIndex, sbWhere.ToString(), sbSort.ToString());
            foreach (var item in SourceData)
            {
                Ask model = new Ask();
                #region 头像
                if (IsMaster(item.UserId, bll.WebsiteOwner))
                {
                    TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", item.UserId));
                    model.headimg = bll.GetImgUrl(masterInfo.TutorImg);
                }
                else
                {
                    UserInfo userInfo = bllUser.GetUserInfo(item.UserId);
                    if (userInfo != null)
                    {
                        model.headimg = userInfo.WXHeadimgurl;
                    }
                }
                #endregion
                model.id = item.AutoId;
                model.title = item.ReviewTitle;
                model.content = item.ReviewContent;
                model.time = bll.GetTimeStamp(item.InsertDate);
                model.pv = item.Pv;
                model.sharecount = item.PraiseNum;
                apiResult.list.Add(model);

            }
            return Common.JSONHelper.ObjectToJson(apiResult);
        }

        /// <summary>
        /// 创建问答接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddAsk(HttpContext context)
        {
            string title = context.Request["title"];
            string content = context.Request["content"];
            string toUserId = context.Request["touserid"];
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                goto outoff;
            }
            if (string.IsNullOrEmpty(title))
            {
                resp.errcode = 2;
                resp.errmsg = "标题不能为空";
                goto outoff;
            }
            if (string.IsNullOrEmpty(content))
            {
                resp.errcode = 3;
                resp.errmsg = "内容不能为空";
                goto outoff;
            }
            if (!string.IsNullOrEmpty(toUserId))
            {
                if (bllUser.GetUserInfo(toUserId) == null)
                {
                    resp.errcode = 4;
                    resp.errmsg = "不能向该理财师咨询";
                    goto outoff;
                }
            }
            else
            {
                toUserId = currentUserInfo.UserID;
            }
            ReviewInfo model = new ReviewInfo();
            model.ReviewTitle = title;
            model.ReviewContent = content;
            model.UserId = currentUserInfo.UserID;
            model.ForeignkeyId = toUserId;
            model.InsertDate = DateTime.Now;
            model.WebsiteOwner = bll.WebsiteOwner;
            model.ReviewType = "话题";
            model.ReplyDateTiem = DateTime.Now;
            if (bll.Add(model))
            {
                BLLRedis.ClearReviewList(bll.WebsiteOwner);
                resp.errcode = 0;
                resp.errmsg = "操作成功";
                goto outoff;
            }
            else
            {
                resp.errcode = 5;
                resp.errmsg = "操作失败";
                goto outoff;
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 回复问题
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ReplyAsk(HttpContext context)
        {
            string Id = context.Request["id"];
            string content = context.Request["content"];
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                goto outoff;
            }
            if (string.IsNullOrEmpty(Id))
            {
                resp.errcode = 2;
                resp.errmsg = "回复编号不能为空";
                goto outoff;
            }
            if (string.IsNullOrEmpty(content))
            {
                resp.errcode = 3;
                resp.errmsg = "回复内容不能为空";
                goto outoff;
            }
            ReviewInfo reviewInfo = bll.Get<ReviewInfo>(string.Format("AutoId={0}", Id));
            if (reviewInfo == null)
            {
                resp.errcode = 4;
                resp.errmsg = "回复的问题不存在";
                goto outoff;
            }
            BLLJIMP.Model.ReplyReviewInfo replyInfo = new BLLJIMP.Model.ReplyReviewInfo()
            {
                ReviewID = Convert.ToInt32(Id),
                InsertDate = DateTime.Now,
                ReplyContent = content,
                UserId = currentUserInfo.UserID,
                WebSiteOwner = bll.WebsiteOwner,
                ReviewType = "话题"

            };
            if (bll.Add(replyInfo))
            {
                reviewInfo.NumCount++;
                reviewInfo.ReplyDateTiem = DateTime.Now;
                bll.Update(reviewInfo);
                BLLRedis.ClearReviewList(bll.WebsiteOwner);
                resp.errmsg = "回复成功";
                goto outoff;

            }
            else
            {
                resp.errcode = 5;
                resp.errmsg = "操作失败";
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取问答详情
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetAskDetail(HttpContext context)
        {
            AskDetailApi apiResult = new AskDetailApi();
            string id = context.Request["id"];
            //int PageIndex=int.Parse(context.Request["pageindex"]);
            //int PageSize=int.Parse(context.Request["pagesize"]); 
            //if (!bll.IsLogin)
            //{
            //    resp.errcode = -1;
            //    resp.errmsg = "尚未登录";
            //    goto outoff;
            //}
            if (string.IsNullOrEmpty(id))
            {
                resp.errcode = 1;
                resp.errmsg = "回复编号不能为空";
                goto outoff;
            }
            ReviewInfo ReviewInfo = bll.Get<ReviewInfo>(string.Format("AutoId={0}", id));
            if (ReviewInfo == null)
            {
                resp.errcode = 3;
                resp.errmsg = "回复的问题不存在";
                goto outoff;
            }
            List<ReplyReviewInfo> sourceData = bll.GetList<ReplyReviewInfo>(string.Format(" ReviewID={0} Order by AutoId DESC ", id));
            AskInfo askInfo = new AskInfo();
            askInfo.id = ReviewInfo.AutoId;
            askInfo.title = ReviewInfo.ReviewTitle;
            askInfo.content = ReviewInfo.ReviewContent;
            askInfo.time = bll.GetTimeStamp(ReviewInfo.InsertDate);
            askInfo.pv = ReviewInfo.Pv;
            askInfo.sharecount = 0;//转发数
            if (IsMaster(ReviewInfo.UserId, bll.WebsiteOwner))//提问人是理财师
            {
                TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", ReviewInfo.UserId));
                if (masterInfo != null)
                {
                    askInfo.headimg = bll.GetImgUrl(masterInfo.TutorImg);
                    askInfo.ismaster = true;
                    askInfo.truename = masterInfo.TutorName;
                }
            }
            else//提问人是普通用户
            {
                UserInfo userInfo = bllUser.GetUserInfo(ReviewInfo.UserId);
                if (userInfo != null)
                {
                    askInfo.headimg = userInfo.WXHeadimgurl;
                    askInfo.ismaster = false;
                    askInfo.truename = userInfo.TrueName;
                    if (string.IsNullOrEmpty(askInfo.truename))
                    {
                        askInfo.truename = userInfo.WXNickname;
                    }
                }
            }
            ReplyList replyModel = new ReplyList();
            replyModel.totalcount = bll.GetCount<ReplyReviewInfo>(string.Format(" ReviewID={0}", id));
            replyModel.list = new List<Reply>();
            foreach (var item in sourceData)
            {
                Reply model = new Reply();
                model.content = item.ReplyContent;
                model.time = bll.GetTimeStamp(item.InsertDate);
                if (IsMaster(item.UserId, bll.WebsiteOwner))//回复人是理财师
                {
                    TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", item.UserId));
                    if (masterInfo != null)
                    {
                        model.headimg = bll.GetImgUrl(masterInfo.TutorImg);
                        model.ismaster = true;
                        model.truename = masterInfo.TutorName;
                    }
                }
                else//回复人是普通用户
                {
                    UserInfo UserInfo = bllUser.GetUserInfo(item.UserId);
                    if (UserInfo != null)
                    {
                        model.headimg = UserInfo.WXHeadimgurl;
                        model.ismaster = false;
                        model.truename = UserInfo.TrueName;
                        if (string.IsNullOrEmpty(model.truename))
                        {
                            model.truename = UserInfo.WXNickname;
                        }
                    }
                }
                replyModel.list.Add(model);

            }
            apiResult.ask = askInfo;
            apiResult.reply = replyModel;
            ReviewInfo.Pv++;
            bll.Update(ReviewInfo);
        outoff:
            return Common.JSONHelper.ObjectToJson(apiResult);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Reg(HttpContext context)
        {
            string phone = context.Request["phone"];
            string pwd = context.Request["pwd"];
            string verCode = context.Request["vercode"];
            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入手机号";
                goto outoff;
            }
            if ((!phone.StartsWith("1")) || (!phone.Length.Equals(11)))
            {
                resp.errcode = 2;
                resp.errmsg = "手机号格式不正确";
                goto outoff;
            }
            if (string.IsNullOrEmpty(pwd))
            {
                resp.errcode = 3;
                resp.errmsg = "请输入密码";
                goto outoff;
            }
            if (string.IsNullOrEmpty(verCode))
            {
                resp.errcode = 4;
                resp.errmsg = "请输入验证码";
                goto outoff;
            }
            if (bllUser.GetUserInfo(phone, bll.WebsiteOwner) != null)
            {
                resp.errcode = 5;
                resp.errmsg = "此手机号已经被注册";
                goto outoff;
            }
            ////验证码检查
            var lastSmsVerificationCode = bllSms.GetLastSmsVerificationCode(phone);
            if (lastSmsVerificationCode == null)
            {
                resp.errcode = 6;
                resp.errmsg = "请先获取手机验证码";
                goto outoff;
            }
            if (!lastSmsVerificationCode.VerificationCode.Equals(verCode))
            {
                resp.errcode = 7;
                resp.errmsg = "验证码不正确";
                goto outoff;
            }
            ////
            UserInfo regUser = new UserInfo();
            regUser.WXHeadimgurl = bll.GetImgUrl("/img/persion.png");
            regUser.UserID = phone;
            regUser.Password = pwd;
            regUser.WebsiteOwner = bll.WebsiteOwner;
            regUser.UserType = 2;
            regUser.Regtime = DateTime.Now;
            regUser.LastLoginDate = DateTime.Now;
            if (bllUser.Add(regUser))
            {
                resp.errcode = 0;
                resp.errmsg = "注册成功";
                context.Session[SessionKey.UserID] = regUser.UserID;
                goto outoff;
            }
            else
            {
                resp.errcode = 6;
                resp.errmsg = "注册失败";
                goto outoff;
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        ///获取短信验证码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSmsVerCode(HttpContext context)
        {
            string phone = context.Request["phone"];
            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入手机号";
                goto outoff;
            }
            if ((!phone.StartsWith("1")) || (!phone.Length.Equals(11)))
            {
                resp.errcode = 2;
                resp.errmsg = "手机号格式不正确";
                goto outoff;
            }
            var lastSmsVerificationCode = bllSms.GetLastSmsVerificationCode(phone);
            if (lastSmsVerificationCode != null)
            {

                if ((DateTime.Now - lastSmsVerificationCode.InsertDate).TotalSeconds < 60)
                {
                    resp.errcode = 3;
                    resp.errmsg = "验证码限制每60秒发送一次";
                    goto outoff;

                }

            }
            bool isSuccess = false;

            string msg = "";

            string verCode = new Random().Next(111111, 999999).ToString();
            bllSms.SendSmsVerificationCode(phone, string.Format("您的验证码{0}", verCode), "福布斯", verCode, out isSuccess, out msg);

            if (isSuccess)
            {
                resp.errcode = 0;
                //resp.errmsg = VerCode;
            }
            else
            {
                resp.errcode = 4;
                resp.errmsg = "发送验证码失败";
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        ///微信分享分享完成
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string WeixinShareComplete(HttpContext context)
        {
            string shareType = context.Request["sharetype"];
            string id = context.Request["id"];
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                goto outoff;
            }
            if (string.IsNullOrEmpty(shareType))
            {
                resp.errcode = 2;
                resp.errmsg = "分享类型必填";
                goto outoff;
            }
            if (string.IsNullOrEmpty(id))
            {
                resp.errcode = 3;
                resp.errmsg = "id必填";
                goto outoff;
            }

            if (bll.GetCount<WBHShareRecord>(string.Format(" ShareId={0} And Type={1}", id, shareType)) > 0)//已经分享过了
            {
                resp.errcode = 4;
                resp.errmsg = "已经分享过了";
                goto outoff;
            }

            WBHShareRecord shareRecord = new WBHShareRecord();
            shareRecord.InsertDate = DateTime.Now;
            shareRecord.ShareId = int.Parse(id);
            shareRecord.Type = shareType;
            shareRecord.UserId = currentUserInfo.UserID;

            BLLJIMP.Model.WBHScoreRecord scoreRecord = new BLLJIMP.Model.WBHScoreRecord();
            scoreRecord.Nums = "b55";
            scoreRecord.InsertDate = DateTime.Now;
            scoreRecord.WebsiteOwner = bll.WebsiteOwner;
            scoreRecord.UserId = currentUserInfo.UserID;
            scoreRecord.RecordType = "2";
            int addScore = 0;
            switch (shareType)
            {
                case "0"://发送文章给朋友
                    scoreRecord.NameStr = "发送资讯给朋友";
                    scoreRecord.ScoreNum = "+5";
                    addScore = 5;
                    break;
                case "1"://分享文章到朋友圈
                    scoreRecord.NameStr = "分享资讯到朋友圈";
                    scoreRecord.ScoreNum = "+5";
                    addScore = 5;
                    break;
                case "2"://发送活动给朋友
                    scoreRecord.NameStr = "发送活动给朋友";
                    scoreRecord.ScoreNum = "+5";
                    addScore = 5;
                    break;
                case "3"://分享活动到朋友圈
                    scoreRecord.NameStr = "分享活动到朋友圈";
                    scoreRecord.ScoreNum = "+5";
                    addScore = 5;
                    break;
                case "4"://发送理财师给朋友
                    scoreRecord.NameStr = "发送理财师给朋友";
                    scoreRecord.ScoreNum = "+5";
                    addScore = 5;
                    break;
                case "5"://分享理财师到朋友圈
                    scoreRecord.NameStr = "分享理财师到朋友圈";
                    scoreRecord.ScoreNum = "+5";
                    addScore = 5;
                    break;
                case "6"://发送话题给朋友
                    scoreRecord.NameStr = "发送话题给朋友";
                    scoreRecord.ScoreNum = "+5";
                    addScore = 5;
                    break;
                case "7"://分享话题到朋友圈
                    scoreRecord.NameStr = "分享话题到朋友圈";
                    scoreRecord.ScoreNum = "+5";
                    addScore = 5;
                    //话题转发数增加
                    ReviewInfo reviewInfo = bll.Get<ReviewInfo>(string.Format(" AutoId={0}", id));
                    if (reviewInfo != null)
                    {
                        reviewInfo.PraiseNum++;
                        bll.Update(reviewInfo);
                    }
                    //
                    break;
                default:
                    break;
            }
            currentUserInfo.TotalScore += addScore;
            if ((bllUser.Update(currentUserInfo, string.Format(" TotalScore={0}", currentUserInfo.TotalScore), string.Format(" AutoId={0}", currentUserInfo.AutoID)) > 0) && (bll.Add(shareRecord)) && (bll.Add(scoreRecord)))
            {
                resp.errcode = 0;
                resp.errmsg = "操作成功";

            }
            else
            {
                resp.errcode = 5;
                resp.errmsg = "操作失败";

            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 获取首页信息列表接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetIndexMsgList(HttpContext context)
        {

            IndexMsgApi apiResult = new IndexMsgApi();
            apiResult.list = new List<IndexMsg>();
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            List<IndexMsg> list = new List<IndexMsg>();
            //文章查询条件
            StringBuilder sbWhereArticle = new StringBuilder(string.Format(" WebsiteOwner='{0}' And IsDelete=0 And  IsHide=0 And ArticleType='article' ", bll.WebsiteOwner));
            //活动查询条件
            StringBuilder sbWhereActivity = new StringBuilder(string.Format(" WebsiteOwner='{0}' And IsDelete=0 And  IsHide=0 And ArticleType='activity' ", bll.WebsiteOwner));
            //创建话题回复条件
            StringBuilder sbWhereReview = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bll.WebsiteOwner));
            //回复话题回复条件
            StringBuilder sbWhereReplyReview = new StringBuilder(string.Format(" WebSiteOwner='{0}' And ReviewType='话题'", bll.WebsiteOwner));
            #region 添加我关注的人查询条件
            if (bll.IsLogin && bll.GetCount<UserFollowChain>(string.Format(" FromUserId='{0}'", currentUserInfo.UserID)) > 0)//获取我关注的人的所有信息
            {
                string strAttentionUserId = "";
                List<UserFollowChain> attentionList = bll.GetList<UserFollowChain>(string.Format(" FromUserId='{0}'", currentUserInfo.UserID));
                foreach (var item in attentionList)
                {
                    strAttentionUserId += string.Format("'{0}',", item.ToUserId);
                }
                strAttentionUserId = strAttentionUserId.TrimEnd(',');

                sbWhereArticle.AppendFormat(" And UserID in({0})", strAttentionUserId);

                sbWhereActivity.AppendFormat(" And UserID in({0})", strAttentionUserId);

                sbWhereReview.AppendFormat(" And UserID in({0})", strAttentionUserId);

                sbWhereReplyReview.AppendFormat(" And UserID in({0})", strAttentionUserId);

            }
            #endregion

            #region 发表文章
            foreach (var item in bll.GetList<JuActivityInfo>(200, sbWhereArticle.ToString(), " JuActivityID DESC"))
            {
                TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", item.UserID));
                IndexMsg model = new IndexMsg();
                model.type = 0;
                model.id = item.JuActivityID;
                model.imgurl = bll.GetImgUrl(item.ThumbnailsPath);
                if (masterInfo != null)
                {
                    model.imgurl = bll.GetImgUrl(masterInfo.TutorImg);

                }
                model.newstitle = item.ActivityName;
                model.digest = item.Summary;
                model.time = bll.GetTimeStamp(item.CreateDate);
                list.Add(model);
            }
            #endregion

            #region 发表活动
            foreach (var item in bll.GetList<JuActivityInfo>(200, sbWhereActivity.ToString(), " JuActivityID DESC"))
            {
                TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", item.UserID));
                IndexMsg model = new IndexMsg();
                model.type = 1;
                model.id = item.JuActivityID;
                model.imgurl = bll.GetImgUrl(item.ThumbnailsPath);
                if (masterInfo != null)
                {
                    model.imgurl = bll.GetImgUrl(masterInfo.TutorImg);

                }
                model.activityname = item.ActivityName;
                model.digest = item.Summary;
                if (item.ActivityStartDate != null)
                {
                    model.activitytime = bll.GetTimeStamp((DateTime)item.ActivityStartDate);

                }
                model.activityaddress = item.ActivityAddress;
                model.time = bll.GetTimeStamp(item.CreateDate);
                list.Add(model);
            }
            #endregion

            #region 发表话题
            foreach (var item in bll.GetList<ReviewInfo>(200, sbWhereReview.ToString(), " AutoID DESC"))
            {
                TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", item.UserId));
                IndexMsg model = new IndexMsg();
                model.type = 2;
                model.id = item.AutoId;
                if (masterInfo != null)
                {
                    model.imgurl = bll.GetImgUrl(masterInfo.TutorImg);

                }
                else
                {
                    UserInfo userInfo = bllUser.GetUserInfo(item.UserId);
                    if (userInfo != null)
                    {
                        model.imgurl = userInfo.WXHeadimgurl;
                    }
                }
                model.asktitle = item.ReviewTitle;
                model.time = bll.GetTimeStamp(item.InsertDate);
                list.Add(model);
            }
            #endregion

            #region 回复话题
            foreach (var item in bll.GetList<ReplyReviewInfo>(200, sbWhereReplyReview.ToString(), " AutoId DESC"))
            {
                ReviewInfo reviewInfo = bll.Get<ReviewInfo>(string.Format(" AutoId={0}", item.ReviewID));
                if (reviewInfo != null)
                {
                    TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", item.UserId));
                    IndexMsg model = new IndexMsg();
                    model.type = 3;
                    model.id = item.ReviewID;
                    if (masterInfo != null)
                    {
                        model.imgurl = bll.GetImgUrl(masterInfo.TutorImg);

                    }
                    else
                    {
                        UserInfo userInfo = bllUser.GetUserInfo(item.UserId);
                        if (userInfo != null)
                        {
                            model.imgurl = userInfo.WXHeadimgurl;
                        }
                    }
                    model.asktitle = reviewInfo.ReviewTitle;
                    model.askcontent = item.ReplyContent;
                    model.time = bll.GetTimeStamp(item.InsertDate);
                    list.Add(model);
                }

            }
            #endregion

            apiResult.totalcount = list.Count;
            list = list.OrderByDescending(p => p.time).ToList();//按时间排序
            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();//分页
            apiResult.list = list;
            return Common.JSONHelper.ObjectToJson(apiResult);

        }
        /// <summary>
        /// 注销登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string LoginOut(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "尚未登录";
                goto outoff;
            }
            context.Session.Clear();
            resp.errmsg = "操作成功";
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 获取理财师届数列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMasterNumberList(HttpContext context)
        {
            MasterNumberApi apiResult = new MasterNumberApi();
            apiResult.list = new List<string>();
            System.Data.DataTable data = ZentCloud.ZCBLLEngine.BLLBase.Query(string.Format("SELECT distinct(number) FROM [CommonPlatform].[dbo].[ZCJ_TutorInfo] where Number is not null and Number>0 and WebsiteOwner='{0}'", bll.WebsiteOwner), "TutorInfo").Tables[0];
            foreach (System.Data.DataRow item in data.Rows)
            {
                apiResult.list.Add(item[0].ToString());
            }
            apiResult.totalcount = apiResult.list.Count;
            return Common.JSONHelper.ObjectToJson(apiResult);


        }

        #region 注册模块
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UserRegister(HttpContext context)
        {
            string pId = context.Request["pId"];
            string name = context.Request["name"];
            string email = context.Request["email"];
            string pwd = context.Request["pwd"];
            string vercode = context.Request["vercode"];
            var serverCheckCode = context.Session["CheckCode"];
            if (!CheckRegister(name, email, pwd, vercode, serverCheckCode))
            {
                return Common.JSONHelper.ObjectToJson(resp);
            }
            UserInfo regUser = new UserInfo();
            regUser.UserID = string.Format("WebPC{0}{1}", ZentCloud.Common.StringHelper.GetDateTimeNum(), ZentCloud.Common.Rand.Str(5));//WXUser+时间字符串+随机5位数字
            regUser.TrueName = name;
            regUser.Email = email;
            regUser.Password = pwd;
            regUser.WebsiteOwner = bllUser.WebsiteOwner;
            regUser.UserType = 2;
            regUser.Regtime = DateTime.Now;
            string ip = ZentCloud.Common.MySpider.GetClientIP();
            regUser.RegIP = ip;
            regUser.LastLoginIP = ip;
            regUser.LastLoginDate = DateTime.Now;
            regUser.LoginTotalCount = 1;

            if (bllUser.Add(regUser))
            {
                context.Session[SessionKey.UserID] = regUser.UserID;

                bllUser.AddUserScoreDetail(regUser.UserID, EnumStringHelper.ToString(ScoreDefineType.Register), this.bll.WebsiteOwner, null, null);
                if (!string.IsNullOrWhiteSpace(pId))
                {
                    string pUserId = MySpider.Base64Change.DecodeBase64(pId);
                    bllUser.AddUserScoreDetail(regUser.UserID, EnumStringHelper.ToString(ScoreDefineType.RegisterFromShare), this.bll.WebsiteOwner, null, null);
                    bllUser.AddUserScoreDetail(pUserId, EnumStringHelper.ToString(ScoreDefineType.ShareRegister), this.bll.WebsiteOwner, null, null);
                }

                return Common.JSONHelper.ObjectToJson(new
                {
                    isSuccess = true,
                    user = new
                    {
                        userid = regUser.UserID,
                        userName = this.bllUser.GetUserDispalyName(regUser),
                        avatar = this.bllUser.GetUserDispalyAvatar(regUser)
                    }
                });
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.RegisterFailure;
                resp.errmsg = "注册失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 律师注册
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string LawyerRegister(HttpContext context)
        {
            string pId = context.Request["pId"];
            string name = context.Request["name"];
            string email = context.Request["email"];
            string pwd = context.Request["pwd"];
            string vercode = context.Request["vercode"];
            object serverCheckCode = context.Session["CheckCode"];
            if (!CheckRegister(name, email, pwd, vercode, serverCheckCode))
            {
                return Common.JSONHelper.ObjectToJson(resp);
            }
            string company = context.Request["company"];
            string postion = context.Request["postion"];
            string idCardNo = context.Request["idCardNo"];//身份证
            string phone = context.Request["phone"];
            string tel = context.Request["tel"];
            string province = context.Request["province"];
            string city = context.Request["city"];
            string licensePhoto = context.Request["licensePhoto"];
            string companyaddress = context.Request["companyaddress"];
            if (!CheckLawyerRegister(company, idCardNo, postion, phone, tel, province, city, licensePhoto, companyaddress))
            {
                return Common.JSONHelper.ObjectToJson(resp);
            }
            UserInfo regUser = new UserInfo();
            regUser.UserID = string.Format("WebPC{0}{1}", ZentCloud.Common.StringHelper.GetDateTimeNum(), ZentCloud.Common.Rand.Str(5));//WXUser+时间字符串+随机5位数字
            regUser.TrueName = name;
            regUser.Email = email;
            regUser.Password = pwd;
            regUser.Company = company;
            regUser.Postion = postion;
            regUser.Phone = phone;
            regUser.ProvinceCode = province;
            if (!string.IsNullOrWhiteSpace(province)) regUser.Province = bllKeyValueData.GetDataDefVaule("Province", province);
            regUser.CityCode = city;
            if (!string.IsNullOrWhiteSpace(city)) regUser.City = bllKeyValueData.GetDataDefVaule("City", city);

            regUser.WebsiteOwner = bllUser.WebsiteOwner;
            regUser.UserType = 4;
            regUser.Regtime = DateTime.Now;
            string ip = ZentCloud.Common.MySpider.GetClientIP();
            regUser.RegIP = ip;
            regUser.LastLoginIP = ip;
            regUser.LastLoginDate = DateTime.Now;
            regUser.LoginTotalCount = 1;

            if (bllUser.Add(regUser))
            {
                bllUser.AddUserScoreDetail(regUser.UserID, EnumStringHelper.ToString(ScoreDefineType.Register), this.bll.WebsiteOwner, null, null);
                if (!string.IsNullOrWhiteSpace(pId))
                {
                    string pUserId = MySpider.Base64Change.DecodeBase64(pId);
                    bllUser.AddUserScoreDetail(regUser.UserID, EnumStringHelper.ToString(ScoreDefineType.RegisterFromShare), this.bll.WebsiteOwner, null, null);
                    bllUser.AddUserScoreDetail(pUserId, EnumStringHelper.ToString(ScoreDefineType.ShareRegister), this.bll.WebsiteOwner, null, null);
                }
                bllUserExpand.AddUserExpand(UserExpandType.IdCardNo, regUser.UserID, idCardNo);
                bllUserExpand.AddUserExpand(UserExpandType.UserTel, regUser.UserID, tel);
                bllUserExpand.AddUserExpand(UserExpandType.UserCompanyAddress, regUser.UserID, companyaddress);
                bllUserExpand.AddUserExpand(UserExpandType.UserCompanyAddress, regUser.UserID, companyaddress);
                bllUserExpand.AddUserExpand(UserExpandType.LawyerLicensePhoto, regUser.UserID, licensePhoto);

                context.Session[SessionKey.UserID] = regUser.UserID;
                return Common.JSONHelper.ObjectToJson(new
                {
                    isSuccess = true,
                    user = new
                    {
                        userid = regUser.UserID,
                        userName = this.bllUser.GetUserDispalyName(regUser),
                        avatar = this.bllUser.GetUserDispalyAvatar(regUser)
                    }
                });
            }
            else
            {
                resp.errcode = 999;
                resp.errmsg = "注册失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 编辑资料
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateUserInfo(HttpContext context)
        {
            string avatar = context.Request["avatar"];
            string name = context.Request["name"];
            string sex = context.Request["sex"];
            string phone = context.Request["phone"];
            string tel = context.Request["tel"];
            string province = context.Request["province"];
            string city = context.Request["city"];
            string introduction = context.Request["introduction"];
            string company = context.Request["company"];
            string postion = context.Request["postion"];
            string companyaddress = context.Request["companyaddress"];
            string receiveaddress = context.Request["receiveaddress"];
            string IsSHowInfo = context.Request["IsSHowInfo"];
            //string LawyerLicenseNo = context.Request["LawyerLicenseNo"];


            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            UserInfo user = bllUser.GetUserInfo(this.currentUserInfo.UserID);
            if (user == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "找不到用户";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            bool check = CheckUpdateUserInfo(name, phone);
            if (!check) return Common.JSONHelper.ObjectToJson(resp);

            bool isTutor = bllUser.IsTutor(user);
            if (isTutor)
            {
                check = CheckUpdateLawyerInfo(company, postion, phone, tel, province, city, companyaddress);
                if (!check) return Common.JSONHelper.ObjectToJson(resp);
            }
            isTutor = bllUser.IsTutor(user);
            user.WXHeadimgurl = avatar;
            user.TrueName = name;
            if (!string.IsNullOrWhiteSpace(sex))
            {
                int sexInt = 0;
                Int32.TryParse(sex, out sexInt);
                user.WXSex = sexInt;
                user.Gender = sex;
            }
            user.Phone = phone;

            user.ProvinceCode = province;
            if (!string.IsNullOrWhiteSpace(province)) user.Province = bllKeyValueData.GetDataDefVaule("Province", province);
            user.CityCode = city;
            if (!string.IsNullOrWhiteSpace(city)) user.City = bllKeyValueData.GetDataDefVaule("City", city);

            user.Company = company;
            user.Postion = postion;
            if (bllUser.UpdateUserInfo(user))
            {
                bllUserExpand.UpdateUserExpand(BLLJIMP.Enums.UserExpandType.UserTel, this.currentUserInfo.UserID, tel);
                bllUserExpand.UpdateUserExpand(BLLJIMP.Enums.UserExpandType.UserIntroduction, this.currentUserInfo.UserID, introduction);
                bllUserExpand.UpdateUserExpand(BLLJIMP.Enums.UserExpandType.UserCompanyAddress, this.currentUserInfo.UserID, companyaddress);
                bllUserExpand.UpdateUserExpand(BLLJIMP.Enums.UserExpandType.UserReceiveAddress, this.currentUserInfo.UserID, receiveaddress);
                //bllUserExpand.UpdateUserExpand(BLLJIMP.Enums.UserExpandType.LawyerLicenseNo, this.currentUserInfo.UserID, LawyerLicenseNo);
                bllUserExpand.UpdateUserExpand(BLLJIMP.Enums.UserExpandType.IsSHowInfo, this.currentUserInfo.UserID, IsSHowInfo);

                if (isTutor)
                {
                    bllTutor.UpdateTutorInfoByUserInfo(user);
                }
                resp.isSuccess = true;
                resp.errmsg = "保存成功";
            }
            else
            {
                resp.errcode = 999;
                resp.errmsg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdatePassword(HttpContext context)
        {
            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            UserInfo user = bllUser.GetUserInfo(this.currentUserInfo.UserID);
            if (user == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "找不到用户";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            string pwd = context.Request["pwd"];
            user.Password = pwd;
            if (bllUser.UpdatePassword(user))
            {
                resp.isSuccess = true;
                resp.errmsg = "保存成功";
            }
            else
            {
                resp.errcode = 999;
                resp.errmsg = "保存失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 注册检查
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="email">邮箱</param>
        /// <param name="pwd">密码</param>
        /// <param name="vercode">验证码</param>
        /// <param name="serverCheckCode">session验证码</param>
        /// <returns></returns>
        private bool CheckRegister(string name, string email, string pwd, string vercode, object serverCheckCode)
        {
            if (string.IsNullOrEmpty(name))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入姓名";
                return false;
            }

            if (string.IsNullOrEmpty(email))
            {
                resp.errcode = 2;
                resp.errmsg = "请输入邮箱";
                return false;
            }
            if (string.IsNullOrEmpty(pwd))
            {
                resp.errcode = 3;
                resp.errmsg = "请输入密码";
                return false;
            }
            if (string.IsNullOrEmpty(vercode))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.CheckCodeErr;
                resp.errmsg = "请输入验证码";
                return false;
            }

            if (!MySpider.MyRegex.EmailLogicJudge(email))
            {
                resp.errcode = 5;
                resp.errmsg = "邮箱格式不正确";
                return false;
            }

            if (serverCheckCode == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.CheckCodeErr;
                resp.errmsg = "验证码超时";
                return false;
            }

            if (!vercode.Equals(serverCheckCode.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.CheckCodeErr;
                resp.errmsg = "验证码错误";
                return false;
            }

            if (bllUser.GetUserInfoByEmail(email) != null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.EmailIsHave;
                resp.errmsg = "此邮箱已经被注册";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 律师注册参数检查
        /// </summary>
        /// <param name="company">公司</param>
        /// <param name="postion">职位</param>
        /// <param name="phone">手机</param>
        /// <param name="tel">座机</param>
        /// <param name="province">省份</param>
        /// <param name="city">城市</param>
        /// <param name="companyaddress">公司地址</param>
        /// <returns></returns>
        private bool CheckLawyerRegister(string company, string idCardNo, string postion, string phone, string tel, string province, string city, string licensePhoto, string companyaddress)
        {
            if (string.IsNullOrEmpty(company))
            {
                resp.errcode = 8;
                resp.errmsg = "请输入公司";
                return false;
            }
            if (string.IsNullOrEmpty(idCardNo))
            {
                resp.errcode = 18;
                resp.errmsg = "请输入身份证";
                return false;
            }
            //if (string.IsNullOrEmpty(postion))
            //{
            //    resp.errcode = 9;
            //    resp.errmsg = "请输入职位";
            //    return false;
            //}

            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 10;
                resp.errmsg = "请输入手机";
                return false;
            }
            if (string.IsNullOrEmpty(tel))
            {
                resp.errcode = 11;
                resp.errmsg = "请输入座机";
                return false;
            }
            //if (string.IsNullOrEmpty(province))
            //{
            //    resp.errcode = 12;
            //    resp.errmsg = "请选择省份";
            //    return false;
            //}

            //if (string.IsNullOrEmpty(city))
            //{
            //    resp.errcode = 13;
            //    resp.errmsg = "请选择城市";
            //    return false;
            //}
            if (string.IsNullOrEmpty(licensePhoto))
            {
                resp.errcode = 17;
                resp.errmsg = "请上传律师执业证";
                return false;
            }
            if (string.IsNullOrEmpty(companyaddress))
            {
                resp.errcode = 14;
                resp.errmsg = "请输入公司地址";
                return false;
            }

            if (!ZentCloud.Common.PageValidate.IsMobile(phone))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PhoneFormatError;
                resp.errmsg = "手机格式错误";
                return false;
            }

            //if (!ZentCloud.Common.PageValidate.IsPhone(tel))
            //{
            //    resp.errcode = 16;
            //    resp.errmsg = "座机格式错误";
            //    return false;
            //}

            if (bllUser.GetUserInfoByPhone(phone) != null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PhoneIsHave;
                resp.errmsg = "此手机已经被注册";
                return false;
            }
            return true;
        }

        private bool CheckUpdateUserInfo(string name, string phone)
        {
            if (string.IsNullOrEmpty(name))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入姓名";
                return false;
            }
            return true;
        }

        private bool CheckUpdateLawyerInfo(string company, string postion, string phone, string tel, string province, string city, string companyaddress)
        {
            if (string.IsNullOrEmpty(company))
            {
                resp.errcode = 8;
                resp.errmsg = "请输入公司";
                return false;
            }

            //if (string.IsNullOrEmpty(postion))
            //{
            //    resp.errcode = 9;
            //    resp.errmsg = "请输入职位";
            //    return false;
            //}
            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 10;
                resp.errmsg = "请输入手机";
                return false;
            }
            if (string.IsNullOrEmpty(tel))
            {
                resp.errcode = 11;
                resp.errmsg = "请输入座机";
                return false;
            }
            //if (string.IsNullOrEmpty(province))
            //{
            //    resp.errcode = 12;
            //    resp.errmsg = "请选择省份";
            //    return false;
            //}

            //if (string.IsNullOrEmpty(city))
            //{
            //    resp.errcode = 13;
            //    resp.errmsg = "请选择城市";
            //    return false;
            //}
            if (string.IsNullOrEmpty(companyaddress))
            {
                resp.errcode = 14;
                resp.errmsg = "请输入公司地址";
                return false;
            }
            if (!MySpider.MyRegex.PhoneNumLogicJudge(phone))
            {
                resp.errcode = 15;
                resp.errmsg = "手机格式错误";
                return false;
            }
            //if (!ZentCloud.Common.PageValidate.IsPhone(tel))
            //{
            //    resp.errcode = 16;
            //    resp.errmsg = "座机格式错误";
            //    return false;
            //}

            return true;
        }

        #endregion

        #region 专家模块

        /// <summary>
        /// 专家列表查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetTutors(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["pageIndex"]);
            int pageSize = int.Parse(context.Request["pageSize"]);
            string province = context.Request["province"];
            string city = context.Request["city"];
            string keyword = context.Request["keyword"];
            string sort = context.Request["sort"];
            int totalCount = 0;
            List<TutorInfo> data = bllTutor.GetTutorsList(pageIndex, pageSize, province, city, keyword, sort, out totalCount);
            int totalAllCount = bllTutor.GetTutorsCount("", "", "");

            List<dynamic> list = new List<dynamic>();
            foreach (TutorInfo item in data)
            {
                UserInfo userInfo = bllUser.GetUserInfo(item.UserId);
                list.Add(new
                {
                    id = userInfo.AutoID,
                    avatar = bllUser.GetUserDispalyAvatar(userInfo),
                    userName = bllUser.GetUserDispalyName(userInfo),
                    userId = item.UserId,
                    digest = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserIntroduction, item.UserId),
                    answerCount = item.TutorAnswers,
                    articleCount = item.WZNums,
                    newActivity = GetNewActivity(item.UserId),
                    followUserCount = bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowUser, item.UserId, null),
                    userIsFollow = this.currentUserInfo == null ? false : bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowUser, item.UserId, this.currentUserInfo.UserID) > 0
                });

            }
            dynamic result = new
            {
                totalallCount = totalAllCount,
                totalcount = totalCount,
                list = list

            };
            return Common.JSONHelper.ObjectToJson(result);
        }
        /// <summary>
        /// 最新动态
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        private dynamic GetNewActivity(string UserId)
        {
            JuActivityInfo act = bll.GetNewActivityByUserId(UserId, false);
            if (act == null) return null;
            return new
            {
                activtyId = act.JuActivityID,
                activityName = act.ActivityName
            };
        }

        /// <summary>
        /// 关注用户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string FollowUser(HttpContext context)
        {
            resp.isSuccess = false;
            string mainId = context.Request["userId"];

            if (string.IsNullOrWhiteSpace(mainId))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.bLLCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.FollowUser, mainId, this.currentUserInfo.UserID))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.bLLCommRelation.AddCommRelation(BLLJIMP.Enums.CommRelationType.FollowUser, mainId, this.currentUserInfo.UserID))
            {
                resp.isSuccess = true;
                bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.FollowUser, this.currentUserInfo, null, mainId, null);
            }
            else
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 取消关注用户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DisFollowUser(HttpContext context)
        {
            resp.isSuccess = false;
            string mainId = context.Request["userId"];

            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (!this.bLLCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.FollowUser, mainId, this.currentUserInfo.UserID))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.bLLCommRelation.DelCommRelation(BLLJIMP.Enums.CommRelationType.FollowUser, mainId, this.currentUserInfo.UserID))
            {
                resp.isSuccess = true;
                bllSystemNotice.SendNotice(BLLSystemNotice.NoticeType.DisFollowUser, this.currentUserInfo, null, mainId, null);
            }
            else
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 申请成为专家
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ApplyToTutor(HttpContext context)
        {
            return AddCurrUserRelation(BLLJIMP.Enums.CommRelationType.ApplyToTutor, "-1");
        }

        /// <summary>
        /// 取消专家申请
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        //private string disApplyToTutor(HttpContext context)
        //{
        //    return DisCurrUserRelation(BLLJIMP.Enums.CommRelationType.FollowArticleCategory, context.Request["id"]);
        //}

        #endregion

        #region 用户信息
        /// <summary>
        /// 获取用户简单信息（点击用户名时弹出用户信息进行关注）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetUserSingleInfo(HttpContext context)
        {
            string userId = context.Request["id"];
            UserInfo userInfo = bllUser.GetUserInfo(userId);
            if (userInfo == null) return "";

            dynamic rUser = new
            {
                id = userInfo.AutoID,
                userId = userInfo.UserID,
                userName = bllUser.GetUserDispalyName(userInfo),
                avatar = bllUser.GetUserDispalyAvatar(userInfo),
                isTutor = bllUser.IsTutor(userInfo),
                userType = userInfo.UserType,
                userFollowCount = bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowUser, null, userInfo.UserID),
                followUserCount = bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowUser, userInfo.UserID, null),
                userIsFollow = this.currentUserInfo == null ? false : bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowArticleCategory, userInfo.UserID, this.currentUserInfo.UserID) > 0
            };
            return Common.JSONHelper.ObjectToJson(rUser);
        }


        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetUserInfo(HttpContext context)
        {
            string id = context.Request["id"];

            UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(id));
            if (userInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                return Common.JSONHelper.ObjectToJson(resp);
            };


            bool isCurrUser = (this.currentUserInfo != null && this.currentUserInfo.UserID == userInfo.UserID);
            string isSHowInfo = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.IsSHowInfo, userInfo.UserID);

            bool isShow = (isCurrUser || (isSHowInfo != null && isSHowInfo.ToLower() == "true"));
            bool isLawyer = userInfo.UserType == 3 ? true : false;

            dynamic rUser = new
            {
                id = userInfo.AutoID,
                userId = userInfo.UserID,
                pId = MySpider.Base64Change.EncodeBase64(userInfo.UserID),
                userName = bllUser.GetUserDispalyName(userInfo),
                avatar = bllUser.GetUserDispalyAvatar(userInfo),
                userProvince = userInfo.Province,
                openId = userInfo.WXOpenId,
                userCity = userInfo.City,
                userProvinceCode = userInfo.ProvinceCode,
                userCityCode = userInfo.CityCode,
                userPostion = isShow ? userInfo.Postion : "",
                userCompany = (isLawyer || isShow) ? userInfo.Company : "",
                LawyerLicenseNo = isLawyer ? bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.LawyerLicenseNo, userInfo.UserID) : "",
                userEmail = isShow ? userInfo.Email : "",
                userPhone = isShow ? userInfo.Phone : "",
                userSexInt = bllUser.GetSexInt(userInfo),
                userTel = isShow ? bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserTel, userInfo.UserID) : "",
                userIntroduction = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserIntroduction, userInfo.UserID),
                userCompanyAddress = isShow ? bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserCompanyAddress, userInfo.UserID) : "",
                userReceiveAddress = isShow ? bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserReceiveAddress, userInfo.UserID) : "",
                isCurrUser = isCurrUser,
                userTotalScore = isCurrUser ? userInfo.TotalScore : 0,
                userUnReadNoticeCount = isCurrUser ? bllSystemNotice.GetUnReadMsgCount(userInfo.UserID, null) : 0,
                userType = userInfo.UserType,
                isTutor = bllUser.IsTutor(userInfo),
                userPraiseCount = bllUser.GetPraiseCount(userInfo.UserID),
                viewCount = userInfo.ViewCount,
                userFollowCount = bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowUser, null, userInfo.UserID),
                followUserCount = bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowUser, userInfo.UserID, null),
                userIsFollow = this.currentUserInfo == null ? false : bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowUser, userInfo.UserID, this.currentUserInfo.UserID) > 0
            };
            //浏览次数加1
            if (!isCurrUser) bllUser.PlusNumericalCol("ViewCount", userInfo.UserID);

            return Common.JSONHelper.ObjectToJson(rUser);
        }
        /// <summary>
        /// 获取编辑时的用户详细信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetEditUserInfo(HttpContext context)
        {
            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            UserInfo userInfo = this.currentUserInfo;

            string IsSHowInfo = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.IsSHowInfo, userInfo.UserID);

            dynamic rUser = new
            {
                id = userInfo.AutoID,
                userId = userInfo.UserID,
                userName = bllUser.GetUserDispalyName(userInfo),
                avatar = bllUser.GetUserDispalyAvatar(userInfo),
                userProvince = userInfo.Province,
                userCity = userInfo.City,
                userProvinceCode = userInfo.ProvinceCode,
                userCityCode = userInfo.CityCode,
                userSexInt = bllUser.GetSexInt(userInfo),
                userPhone = userInfo.Phone,
                userCompany = userInfo.Company,
                userPostion = userInfo.Postion,
                userTel = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserTel, userInfo.UserID),
                userIntroduction = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserIntroduction, userInfo.UserID),
                userCompanyAddress = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserCompanyAddress, userInfo.UserID),
                userReceiveAddress = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserReceiveAddress, userInfo.UserID),
                //lawyerLicenseNo = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.LawyerLicenseNo, user.UserID),
                userType = userInfo.UserType,
                isShowInfo = string.IsNullOrWhiteSpace(IsSHowInfo) ? "false" : IsSHowInfo
            };
            return Common.JSONHelper.ObjectToJson(rUser);
        }

        /// <summary>
        /// 获取申请律师的用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetUserInfoToApplyLawyer(HttpContext context)
        {
            string id = context.Request["id"];
            UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(id));
            if (userInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            DataTable dt = bllUserExpand.GetUserExpands("IdCardNo,LawyerLicensePhoto,UserTel,UserCompanyAddress", userInfo.UserID);
            CommonPlatform.Helper.DataTableHelper dtHelper = new CommonPlatform.Helper.DataTableHelper();
            object IdCardNo = dtHelper.GetValueByDataTableTop(dt, "IdCardNo");
            object LawyerLicensePhoto = dtHelper.GetValueByDataTableTop(dt, "LawyerLicensePhoto");

            dynamic rUser = new
            {
                isOk = CheckInfoToApplyLawyer(userInfo, dt),
                idCardNo = IdCardNo == null ? "" : IdCardNo.ToString(),
                lawyerLicensePhoto = LawyerLicensePhoto == null ? "" : LawyerLicensePhoto.ToString()
            };
            return Common.JSONHelper.ObjectToJson(rUser);
        }

        /// <summary>
        /// 用户信息检查
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool CheckInfoToApplyLawyer(UserInfo user, DataTable dt)
        {
            if (string.IsNullOrWhiteSpace(user.Phone))
            {
                return false;
            }
            else if (string.IsNullOrWhiteSpace(user.Company))
            {
                return false;
            }
            CommonPlatform.Helper.DataTableHelper dtHelper = new CommonPlatform.Helper.DataTableHelper();
            object UserTel = dtHelper.GetValueByDataTableTop(dt, "UserTel");
            object UserCompanyAddress = dtHelper.GetValueByDataTableTop(dt, "UserCompanyAddress");
            if (UserTel == null || UserTel.ToString().Trim() == "")
            {
                return false;
            }
            if (UserCompanyAddress == null || UserCompanyAddress.ToString().Trim() == "")
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 申请律师
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ApplyLawyer(HttpContext context)
        {
            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            string idCardNo = context.Request["idCardNo"];
            string lawyerLicensePhoto = context.Request["lawyerLicensePhoto"];

            if (bllUser.UpdateUserType(this.currentUserInfo.UserID, 4))
            {
                bllUserExpand.UpdateUserExpand(BLLJIMP.Enums.UserExpandType.IdCardNo, this.currentUserInfo.UserID, idCardNo);
                bllUserExpand.UpdateUserExpand(BLLJIMP.Enums.UserExpandType.LawyerLicensePhoto, this.currentUserInfo.UserID, lawyerLicensePhoto);
                resp.isSuccess = true;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取邀请码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetInvitCode(HttpContext context)
        {
            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            resp.returnValue = MySpider.Base64Change.EncodeBase64(this.currentUserInfo.UserID);
            resp.isSuccess = true;
            return Common.JSONHelper.ObjectToJson(resp);
        }
        #endregion

        #region 常用数量查询
        /// <summary>
        /// 获取回复数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetReviewCount(HttpContext context)
        {
            string type = context.Request["type"];
            string foreignKeyId = context.Request["foreignkeyId"];
            string userId = context.Request["userId"];
            BLLJIMP.Enums.ReviewTypeKey nType = new BLLJIMP.Enums.ReviewTypeKey();
            if (!Enum.TryParse(type, out nType))
            {
                resp.errcode = 1;
                resp.errmsg = "类型格式不能识别";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            return Common.JSONHelper.ObjectToJson(new
            {
                totalcount = bllReview.GetReviewCount(nType, foreignKeyId, userId)
            });
        }

        /// <summary>
        /// 获取文章数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetArticleCount(HttpContext context)
        {
            string type = context.Request["type"];
            string cateId = context.Request["cateId"];
            string userId = context.Request["userId"];

            return Common.JSONHelper.ObjectToJson(new
            {
                totalcount = bll.GetJuActivityCount(type, cateId, userId, false)
            });
        }

        /// <summary>
        /// 获取关系数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetCommRelationCount(HttpContext context)
        {
            string type = context.Request["type"];
            string mainId = context.Request["mainId"];
            string relationId = context.Request["relationId"];

            BLLJIMP.Enums.CommRelationType nType = new BLLJIMP.Enums.CommRelationType();
            if (!Enum.TryParse(type, out nType))
            {
                resp.errcode = 1;
                resp.errmsg = "类型格式不能识别";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            return Common.JSONHelper.ObjectToJson(new
            {
                totalcount = bLLCommRelation.GetRelationCount(nType, mainId, relationId)
            });
        }
        #endregion

        /// <summary>
        /// 积分获取规则
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetScoreDefineList(HttpContext context)
        {
            BLLScoreDefine bllScoreDefine = new BLLScoreDefine();
            List<ScoreDefineInfo> scoreDefineInfoList = bllScoreDefine.GetScoreDefineList(this.bll.WebsiteOwner);

            var result = from p in scoreDefineInfoList
                         select new
                         {
                             name = p.Name,
                             score = p.Score >= 0 ? "+" + p.Score : p.Score.ToString(),
                             summary = p.Description
                         };

            return Common.JSONHelper.ObjectToJson(result);
        }
        /// <summary>
        /// 用户积分明细
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetScoreDetailsList(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            int balanceType = Convert.ToInt32(context.Request["balanceType"]);
            string keyword = context.Request["keyword"];

            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                return Common.JSONHelper.ObjectToJson(resp);
            }
            int tCount = 0;
            List<UserScoreDetailsInfo> scorecoreDetailsList = bllUser.GetScoreDetailsList(pageSize, pageIndex, this.currentUserInfo.UserID, keyword, out tCount, balanceType);

            var result = from p in scorecoreDetailsList
                         select new
                         {
                             date = p.AddTime.ToString(),
                             score = p.Score >= 0 ? "+" + p.Score : p.Score.ToString(),
                             type = p.ScoreType,
                             summary = p.AddNote,
                             totalscore = p.TotalScore
                         };

            return Common.JSONHelper.ObjectToJson(new
            {
                totalcount = tCount,
                list = result
            });
        }
        /// <summary>
        /// 用户收藏
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetUserFavoriteList(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["pageIndex"]);
            int pageSize = int.Parse(context.Request["pageSize"]);
            int userAutoId = int.Parse(context.Request["userAutoId"]);

            UserInfo userInfo = bllUser.GetUserInfoByAutoID(userAutoId);
            System.Data.DataSet ds = bllUser.GetUserJuActivityFavoriteList(pageSize, pageIndex, userInfo.UserID, "", false);

            List<dynamic> list = new List<dynamic>();
            foreach (System.Data.DataRow item in ds.Tables[0].Rows)
            {
                list.Add(new
                {
                    id = item["JuActivityID"],
                    title = item["ActivityName"],
                    summary = item["Summary"],
                    userName = bllUser.GetUserDispalyName(userInfo),
                    type = item["ArticleType"],
                    tags = item["Tags"],
                    time = item["RelationTime"].ToString(),
                    commentCount = item["CommentCount"],
                    pv = item["PV"]
                });
            }


            return Common.JSONHelper.ObjectToJson(new
            {
                totalcount = ds.Tables[1].Rows[0][0],
                list = list
            });
        }
        /// <summary>
        /// 新入会员列表
        /// </summary>
        /// <returns></returns>
        private string GetNewUsers(HttpContext context)
        {
            int topNum = int.Parse(context.Request["topNum"]);
            List<UserInfo> users = bllUser.GetNewUserList(topNum, 1, this.bll.WebsiteOwner);
            var result = from p in users
                         select new
                         {
                             id = p.AutoID,
                             userId = p.UserID,
                             userName = bllUser.GetUserDispalyName(p),
                             avatar = bllUser.GetUserDispalyAvatar(p),
                             isTutor = bllUser.IsTutor(p),
                             createDate = p.Regtime.HasValue ? p.Regtime.Value.ToString() : ""
                         };

            return Common.JSONHelper.ObjectToJson(result);
        }
        /// <summary>
        /// 可能感兴趣的人
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetInterestedUser(HttpContext context)
        {
            int topNum = int.Parse(context.Request["topNum"]);
            string userId = this.currentUserInfo != null ? this.currentUserInfo.UserID : "";
            List<UserInfo> users = bllUser.GetNoRelationUserList(BLLJIMP.Enums.CommRelationType.FollowUser, topNum, userId, this.bll.WebsiteOwner);

            var list = from p in users
                       select new
                       {
                           id = p.AutoID,
                           avatar = bllUser.GetUserDispalyAvatar(p),
                           userName = bllUser.GetUserDispalyName(p),
                           userId = p.UserID,
                           isTutor = bllUser.IsTutor(p),
                           userIsFollow = this.currentUserInfo == null ? false : bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowUser, p.UserID, this.currentUserInfo.UserID) > 0
                       };
            return Common.JSONHelper.ObjectToJson(list);
        }
        /// <summary>
        /// 获取可邀请回答用户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetCanInvitUsers(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["pageIndex"]);
            int pageSize = int.Parse(context.Request["pageSize"]);
            string keyword = context.Request["keyword"];


            int TCount = 0;
            List<UserInfo> users = bllUser.GetCanInvitUsers(pageSize, pageIndex, BLLJIMP.Enums.CommRelationType.FollowUser, this.currentUserInfo.UserID, keyword, out TCount);

            var list = from p in users
                       select new
                       {
                           avatar = bllUser.GetUserDispalyAvatar(p),
                           userName = bllUser.GetUserDispalyName(p),
                           userId = p.UserID
                       };
            return Common.JSONHelper.ObjectToJson(new
            {
                totalcount = TCount,
                list = list
            });
        }

        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetFansUsers(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["pageIndex"]);
            int pageSize = int.Parse(context.Request["pageSize"]);
            string userAutoId = context.Request["userAutoId"];
            string keyword = context.Request["keyword"];

            UserInfo user = bllUser.GetUserInfoByAutoID(int.Parse(userAutoId));


            int totalCount = 0;
            List<UserInfo> users = bllUser.GetRelationUserList(pageSize, pageIndex, BLLJIMP.Enums.CommRelationType.FollowUser, user.UserID, null, keyword, out totalCount);

            var list = from p in users
                       select new
                       {
                           id = p.AutoID,
                           avatar = bllUser.GetUserDispalyAvatar(p),
                           userName = bllUser.GetUserDispalyName(p),
                           userId = p.UserID,
                           isTutor = bllUser.IsTutor(p),
                           info = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserIntroduction, p.UserID),
                           userIsFollow = this.currentUserInfo == null ? false : bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowUser, p.UserID, this.currentUserInfo.UserID) > 0
                       };
            return Common.JSONHelper.ObjectToJson(new
            {
                totalcount = totalCount,
                list = list
            });
        }


        /// <summary>
        /// 获取关注列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetFollowUsers(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["pageIndex"]);
            int pageSize = int.Parse(context.Request["pageSize"]);
            string userAutoId = context.Request["userAutoId"];
            string keyword = context.Request["keyword"];

            UserInfo user = bllUser.GetUserInfoByAutoID(int.Parse(userAutoId));

            int TCount = 0;
            List<UserInfo> users = bllUser.GetRelationUserList(pageSize, pageIndex, BLLJIMP.Enums.CommRelationType.FollowUser, null, user.UserID, keyword, out TCount);

            var list = from p in users
                       select new
                       {
                           id = p.AutoID,
                           avatar = bllUser.GetUserDispalyAvatar(p),
                           userName = bllUser.GetUserDispalyName(p),
                           userId = p.UserID,
                           isTutor = bllUser.IsTutor(p),
                           info = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserIntroduction, p.UserID),
                           userIsFollow = this.currentUserInfo == null ? false : bLLCommRelation.GetRelationCount(BLLJIMP.Enums.CommRelationType.FollowUser, p.UserID, this.currentUserInfo.UserID) > 0
                       };
            return Common.JSONHelper.ObjectToJson(new
            {
                totalcount = TCount,
                list = list
            });
        }

        /// <summary>
        /// 获取通用用户关系列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetCommUserRelationList(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["pageIndex"]);
            int pageSize = int.Parse(context.Request["pageSize"]);
            string userAutoId = context.Request["userAutoId"];
            string keyword = context.Request["keyword"]; //昵称搜索
            string type = context.Request["type"]; //昵称搜索
            string exchange = context.Request["exchange"]; //1时mainId，relationId互换
            string isid = context.Request["isid"]; //AutoID建立的关系
            BLLJIMP.Enums.CommRelationType nType = BLLJIMP.Enums.CommRelationType.FollowUser;
            if (!string.IsNullOrWhiteSpace(type))
            {
                if (!Enum.TryParse(type, out nType))
                {
                    resp.errcode = 1;
                    resp.errmsg = "类型格式不能识别";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }
            string mainId = "";
            string relationId = "";
            if (isid == "1"){
                relationId = userAutoId;
            }
            else
            {
                UserInfo user = bllUser.GetUserInfoByAutoID(int.Parse(userAutoId));
                relationId = user.UserID;
            }
            if (exchange == "1")
            {
                mainId = relationId;
                relationId = "";
            }

            int TCount = 0;
            List<UserInfo> users = bllUser.GetRelationUserList(pageSize, pageIndex, nType, mainId, relationId, keyword, out TCount, isid == "1");

            var list = from p in users
                       select new
                       {
                           id = p.AutoID,
                           avatar = bllUser.GetUserDispalyAvatar(p),
                           userName = bllUser.GetUserDispalyName(p),
                           userHasRelation = this.currentUserInfo == null ? false : (isid == "1" ? bLLCommRelation.ExistRelation(nType, p.AutoID.ToString(), this.currentUserInfo.AutoID.ToString()) : bLLCommRelation.ExistRelation(nType, p.UserID, this.currentUserInfo.UserID)),
                           relationTime = p.LastLoginDate.ToString("yyyy/MM/dd hh:mm:ss")
                       };

            return Common.JSONHelper.ObjectToJson(new
            {
                totalcount = TCount,
                list = list
            });
        }
        /// <summary>
        /// 分享加积分
        /// </summary>
        /// <returns></returns>
        private string ShareAddScore(HttpContext context)
        {
            string type = context.Request["type"];
            string shareType = "";//分类类型字符串
            if (this.currentUserInfo != null)
            {
                //ScoreDefineType scoreDefineType = ScoreDefineType.ShareArticle;
                //if (type == "Question")
                //{
                //    scoreDefineType = ScoreDefineType.ShareQuestions;
                //}
                //else if (type == "Case")
                //{
                //    scoreDefineType = ScoreDefineType.ShareCase;
                //}
                switch (type)
                {
                    case "Question":
                        shareType = "ShareQuestions";
                        break;
                    case "Case":
                        shareType = "ShareCase";
                        break;
                    default:
                        shareType = type;
                        break;
                }
                if (bllUser.AddUserScoreDetail(this.currentUserInfo.UserID, shareType
                    , this.bll.WebsiteOwner, null, null))
                {
                    resp.isSuccess = true;
                }
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取100积分需要多少元
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetRechargeConfig(HttpContext context)
        {
            resp.isSuccess = true;
            resp.returnValue = bllKeyValueData.GetDataVaule("Recharge", "100", this.bll.WebsiteOwner);
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取微信预订单信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetWeixinPreOrder(HttpContext context)
        {
            string scoreStr = context.Request["score"];
            decimal score = Convert.ToDecimal(scoreStr);
            if (score == 0)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "积分不能为0";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                resp.errmsg = "您还没有登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            BllOrder bllOrder = new BllOrder();
            BllPay bllPay = new BllPay();
            PayConfig payConfig = bllPay.GetPayConfig();

            string rechargeValue = bllKeyValueData.GetDataVaule("Recharge", "100", bllKeyValueData.WebsiteOwner);
            decimal rechargeFee = Convert.ToDecimal(rechargeValue);
            decimal totalFee = rechargeFee / 100 * score;

            OrderPay orderPay = new OrderPay();
            orderPay.OrderId = bllOrder.GetGUID(TransacType.CommAdd);
            orderPay.InsertDate = DateTime.Now;
            orderPay.Status = 0;
            orderPay.Type = "1";
            orderPay.Subject = "积分充值";
            orderPay.Total_Fee = totalFee;
            orderPay.UserId = currentUserInfo.UserID;
            orderPay.WebsiteOwner = this.bll.WebsiteOwner;
            orderPay.Ex1 = scoreStr;

            var non_str = Payment.WeiXin.CommonUtil.CreateNoncestr();//随机串
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("appid", payConfig.WXAppId);
            dic.Add("body", "订单" + orderPay.OrderId);
            dic.Add("mch_id", payConfig.WXMCH_ID);//商户号
            dic.Add("nonce_str", non_str);
            dic.Add("out_trade_no", orderPay.OrderId);
            dic.Add("spbill_create_ip", context.Request.UserHostAddress);
            dic.Add("total_fee", (totalFee * 100).ToString("F0"));
            dic.Add("notify_url", string.Format("http://{0}/Admin/DoPay/DoPayWxNotify.aspx", context.Request.Url.Host));
            dic.Add("trade_type", "NATIVE");
            dic.Add("product_id", orderPay.OrderId);
            string strtemp = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(dic, false);
            string sign = Payment.WeiXin.MD5SignUtil.Sign(strtemp, payConfig.WXPartnerKey);

            dic = (from entry in dic
                   orderby entry.Key ascending
                   select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
            dic.Add("sign", sign);

            string postData = Payment.WeiXin.CommonUtil.ArrayToXml(dic);
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(postData);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = requestBytes.Length;
            Stream requestStream = req.GetRequestStream();
            requestStream.Write(requestBytes, 0, requestBytes.Length);
            requestStream.Close();

            System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
            string backStr = sr.ReadToEnd();
            sr.Close();
            res.Close();
            var result = System.Xml.Linq.XDocument.Parse(backStr);
            var return_code = result.Element("xml").Element("return_code").Value;

            if (!return_code.ToUpper().Equals("SUCCESS"))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "预支付失败";
                resp.returnValue = result.Element("xml").Element("return_msg").Value;
                resp.returnObj = postData;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            var rusult_code = result.Element("xml").Element("result_code").Value;
            if (return_code.ToUpper().Equals("SUCCESS") && (rusult_code.ToUpper().Equals("SUCCESS")))
            {
                var prepay_id = result.Element("xml").Element("prepay_id").Value;
                var code_url = result.Element("xml").Element("code_url").Value;
                orderPay.Trade_No = prepay_id;
                bllOrder.Add(orderPay);

                resp.isSuccess = true;
                resp.returnObj = new
                {
                    orderId = orderPay.OrderId,
                    prepay_id = prepay_id,
                    code_url = code_url
                };
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.returnValue = result.Element("xml").Element("err_code_des").Value;
                resp.errmsg = "预支付失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 微信内获取预订单信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetWeixinJSAPIPreOrder(HttpContext context)
        {
            string scoreStr = context.Request["score"];
            decimal score = Convert.ToDecimal(scoreStr);
            if (score == 0)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "积分不能为0";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                resp.errmsg = "您还没有登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            BllOrder bllOrder = new BllOrder();
            BllPay bllPay = new BllPay();
            PayConfig payConfig = bllPay.GetPayConfig();

            string rechargeValue = bllKeyValueData.GetDataVaule("Recharge", "100", bllKeyValueData.WebsiteOwner);
            decimal rechargeFee = Convert.ToDecimal(rechargeValue);
            decimal totalFee = rechargeFee / 100 * score;

            //写1分钱
            //totalFee = (decimal)0.01;

            OrderPay orderPay = new OrderPay();
            orderPay.OrderId = bllOrder.GetGUID(TransacType.CommAdd);
            orderPay.InsertDate = DateTime.Now;
            orderPay.Status = 0;
            orderPay.Type = "1";
            orderPay.Subject = "积分充值";
            orderPay.Total_Fee = totalFee;
            orderPay.UserId = currentUserInfo.UserID;
            orderPay.WebsiteOwner = this.bll.WebsiteOwner;
            orderPay.Ex1 = scoreStr;

            if (bllOrder.Add(orderPay))
            {
                resp.isSuccess = true;
                resp.returnValue = orderPay.OrderId;
            }
            //
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 检查订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CheckIsRecharge(HttpContext context)
        {
            string orderId = context.Request["id"];
            BllOrder bllOrder = new BllOrder();
            OrderPay orderPay = bllOrder.GetOrderPay(orderId);
            if (orderPay != null && orderPay.Status == 1)
            {
                resp.isSuccess = true;
                resp.returnValue = "ok";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 获取手机首页图片
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetAdList(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["pageIndex"]);
            int pageSize = int.Parse(context.Request["pageSize"]);
            int AdType = int.Parse(context.Request["Type"]);
            BLLAdInfo bllAdInfo = new BLLAdInfo();
            int totalCount = 0;
            List<ZentCloud.BLLJIMP.Model.AdInfo> dataList = bllAdInfo.GetAdInfoList(pageSize, pageIndex, AdType, this.bll.WebsiteOwner, out totalCount);
            var data = from p in dataList
                       select new
                       {
                           title = p.Title,
                           img = p.ImgUrl,
                           url = p.SiteUrl
                       };

            return Common.JSONHelper.ObjectToJson(new
            {
                totalCount = totalCount,
                list = data
            });
        }


        private string FindPassword(HttpContext context)
        {
            string checkCode = context.Request["checkcode"];
            object serverCheckCode = context.Session["CheckCode"];
            if (serverCheckCode == null || string.IsNullOrWhiteSpace(checkCode) || !checkCode.Equals(serverCheckCode.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                resp.errmsg = "验证码错误";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.CheckCodeErr;
                return Common.JSONHelper.ObjectToJson(resp);
            }


            string email = context.Request["email"];
            if (MySpider.MyRegex.EmailLogicJudge(email))
            {
                UserInfo user = bllUser.GetUserInfoByEmail(email);

                MySpider.MyEmail bllEmail = new MySpider.MyEmail();
                string Step5SmtpHost = System.Configuration.ConfigurationManager.AppSettings["Step5SmtpHost"];
                string Step5EmaiSendUserName = System.Configuration.ConfigurationManager.AppSettings["Step5EmaiSendUserName"];
                string Step5EmaiSendUserPwd = System.Configuration.ConfigurationManager.AppSettings["Step5EmaiSendUserPwd"];
                string Step5SmtpPort = System.Configuration.ConfigurationManager.AppSettings["Step5SmtpPort"];
                string errorStr = "";


                bllEmail.SendSMTPEMail(Step5SmtpHost, Step5EmaiSendUserName, Step5EmaiSendUserPwd, email, Step5EmaiSendUserName
                    , "易劳邮箱密码找回", "您的密码是：" + user.Password, new List<string>(), "易劳平台", false, Encoding.Default, out errorStr, int.Parse(Step5SmtpPort));

                if (string.IsNullOrWhiteSpace(errorStr))
                {
                    resp.isSuccess = true;
                }
                else
                {
                    resp.errmsg = errorStr;
                }
            }
            else
            {
                resp.errmsg = "邮箱格式不能识别！";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }


        #region 车型库相关

        /// <summary>
        /// 查询品牌数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryBrand(HttpContext context)
        {
            string firstLetter = context.Request["firstLetter"];
            var isMustInWebsite = Convert.ToInt32(context.Request["isMustInWebsite"]) == 1;

            int totalCount = 0,
                pageSize = Convert.ToInt32(context.Request["pageSize"]),
                pageIndex = Convert.ToInt32(context.Request["pageIndex"]);

            var dataList = bllCar.QueryBrand(out totalCount, this.bll.WebsiteOwner, pageSize, pageIndex, firstLetter, isMustInWebsite);

            return MySpider.JSONHelper.ObjectToJson(new
            {
                totalCount = totalCount,
                dataList = dataList
            });
        }

        private string GetSeriesCateList(HttpContext context)
        {
            var brandId = Convert.ToInt32(context.Request["brandId"]);

            var list = bllCar.GetSeriesCateList(brandId);

            return MySpider.JSONHelper.ObjectToJson(list);
        }

        private string GetSeriesList(HttpContext context)
        {
            var cateId = Convert.ToInt32(context.Request["cateId"]);
            var brandId = Convert.ToInt32(context.Request["brandId"]);
            var list = bllCar.GetSeriesList(cateId, brandId);

            return MySpider.JSONHelper.ObjectToJson(list);
        }

        private string GetModelList(HttpContext context)
        {
            var pageSize = 1000000;
            var pageIndex = 1;
            var modelCateId = 1;
            var seriesId = Convert.ToInt32(context.Request["seriesId"]);
            int totalCount = 0;

            var list = bllCar.GetModelList(out totalCount, pageSize, pageIndex, modelCateId, seriesId);

            return MySpider.JSONHelper.ObjectToJson(new
            {
                totalCount = totalCount,
                list = list
            });
        }

        #endregion

        /// <summary>
        /// 驿氪会员数据回传
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EZRproCallBackMember(HttpContext context)
        {
            string result = string.Empty;

            if (context.Request["token"] != "74F2EBA1C83264DEC0ACD7E8D2CAB50CBF767535")
            {
                return JsonConvert.SerializeObject(new
                {
                    Status = false,
                    Msg = "错误:token",
                    StatusCode = 0,
                    Result = 0
                });
            }

            Open.EZRproSDK.Entity.MemberCallBackReq info = new Open.EZRproSDK.Entity.MemberCallBackReq();
            info = bll.ConvertRequestToModel<Open.EZRproSDK.Entity.MemberCallBackReq>(info);
            var args = context.Request["Args"];

            TologTemp("MemberInfo ConvertRequestToModel:" + JsonConvert.SerializeObject(info));
            TologTemp("args:" + args);

            TologTemp("AppId:" + context.Request["AppId"]);
            TologTemp("Sign:" + context.Request["Sign"]);

            info.Args = JsonConvert.DeserializeObject<Open.EZRproSDK.Entity.MemberInfo>(args);

            result = new Open.EZRproSDK.Client().CallBackMemberInfo(info);

            return result;
        }

        /// <summary>
        /// 查询商城订单列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EZRproQueryMallOrderList(HttpContext context)
        {
            string result = string.Empty;
            if (context.Request["token"] != "74F2EBA1C83264DEC0ACD7E8D2CAB50CBF767535")
            {
                return JsonConvert.SerializeObject(new
                {
                    Status = false,
                    Msg = "错误:token",
                    StatusCode = 0,
                    Result = 0
                });
            }

            int pageIndex = int.Parse(context.Request["pageIndex"]);
            int pageSize = int.Parse(context.Request["pageSize"]);
            string status = context.Request["status"];
            string userId = context.Request["userId"];
            int totalCount = 0;

            BLLMall bllMall = new BLLMall();
            Open.EZRproSDK.Client ykClient = new Open.EZRproSDK.Client();
            //List<ZentCloud.BLLJIMP.Model.WXMallOrderInfo> dataList = bllMall.GetOrderList(out totalCount, pageSize, pageIndex, userId, status);
            List<ZentCloud.BLLJIMP.Model.WXMallOrderInfo> dataList = bllMall.GetOrderList(pageSize, pageIndex, "", out totalCount, status, userId, "", "", "", "");
            List<Open.EZRproSDK.Entity.OrderInfo> resultList = new List<Open.EZRproSDK.Entity.OrderInfo>();

            foreach (var item in dataList)
            {
                var order = ykClient.GeiMappingOrderInfo(item);
                resultList.Add(order);
            }

            //构造返回

            return result;
        }

        /// <summary>
        /// 获取线下订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetOffLineOrder(HttpContext context)
        {
            Open.EZRproSDK.Client ykClient = new Open.EZRproSDK.Client();
            var resp = ykClient.VipSaleGetResp(currentUserInfo.Ex2);
            return JsonConvert.SerializeObject(resp);
        }

        private string VistWxpay(HttpContext context)
        {
            try
            {
                string postData = "";
                string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(postData);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = requestBytes.Length;
                System.IO.Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
                string backStr = sr.ReadToEnd();
                sr.Close();
                res.Close();
            }
            catch { }

            resp.isSuccess = true;

            return JsonConvert.SerializeObject(resp);
        }
        /// <summary>
        /// 添加通用用户关系
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <returns></returns>
        protected string AddCommUserRelation(HttpContext context)
        {
            string rtype = context.Request["rtype"],
                mainId = context.Request["mainId"],
                expandId = context.Request["expandId"],
                ex1 = context.Request["ex1"],
                exchange = context.Request["exchange"];//1时mainId，relationId互换

            BLLJIMP.Enums.CommRelationType nType = new BLLJIMP.Enums.CommRelationType();
            if (!Enum.TryParse(rtype, out nType))
            {
                resp.errcode = 1;
                resp.errmsg = "类型格式不能识别";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (mainId == "0" || string.IsNullOrWhiteSpace(mainId))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                resp.errmsg = "关联主Id错误";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                resp.errmsg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            string relationId = this.currentUserInfo.AutoID.ToString();
            if (mainId == relationId)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "不能跟自己建立关系";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (exchange == "1")
            {
                relationId = mainId;
                mainId = this.currentUserInfo.AutoID.ToString();
            }
            if (nType == CommRelationType.FriendApply)
            {
                if (this.bLLCommRelation.ExistRelation(CommRelationType.Friend, mainId, relationId))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.HasFriend;
                    resp.errmsg = "已经是好友";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }
            if (this.bLLCommRelation.ExistRelation(nType, mainId, relationId))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                resp.errmsg = "已存在关系";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (this.bLLCommRelation.AddCommRelation(nType, mainId, relationId, expandId, ex1))
            {

                if (nType == CommRelationType.FriendApply)
                {
                    UserInfo toUser = bllUser.GetUserInfoByAutoID(int.Parse(mainId));
                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.FriendApply, this.currentUserInfo, null, new List<UserInfo>() { toUser }, null);
                }
                else if (nType == CommRelationType.Friend)
                {
                    UserInfo toUser = bllUser.GetUserInfoByAutoID(int.Parse(mainId));
                    //添加相关好友关系
                    bLLCommRelation.AddCommRelation(CommRelationType.Friend, relationId, mainId);
                    //通过好友申请删除申请关系
                    bLLCommRelation.DelCommRelation(CommRelationType.FriendApply, relationId, mainId);

                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.PassFriendApply, this.currentUserInfo, null, new List<UserInfo>() { toUser }, null);
                }

                resp.isSuccess = true;
                resp.errmsg = "添加完成";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "添加失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除通用用户关系
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <returns></returns>
        protected string DelCommUserRelation(HttpContext context)
        {
            string rtype = context.Request["rtype"],
                mainId = context.Request["mainId"],
                exchange = context.Request["exchange"]; //1时mainId，relationId互换

            BLLJIMP.Enums.CommRelationType nType = new BLLJIMP.Enums.CommRelationType();
            if (!Enum.TryParse(rtype, out nType))
            {
                resp.errcode = 1;
                resp.errmsg = "类型格式不能识别";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (mainId == "0" || string.IsNullOrWhiteSpace(mainId))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                resp.errmsg = "关联主Id错误";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (this.currentUserInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                resp.errmsg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            string relationId = this.currentUserInfo.AutoID.ToString();
            if (mainId == relationId)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "不能跟自己建立关系";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (exchange == "1")
            {
                relationId = mainId;
                mainId = this.currentUserInfo.AutoID.ToString();
            }

            if (!this.bLLCommRelation.ExistRelation(nType, mainId, relationId))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                resp.errmsg = "关系不存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (this.bLLCommRelation.DelCommRelation(nType, mainId, relationId))
            {
                if (nType == CommRelationType.FriendApply)
                {
                    UserInfo toUser = bllUser.GetUserInfoByAutoID(int.Parse(mainId));
                    //通过好友申请删除申请关系
                    bLLCommRelation.DelCommRelation(CommRelationType.FriendApply, relationId, mainId);
                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.RejectFriendApply, this.currentUserInfo, null, new List<UserInfo>() { toUser }, null);
                }
                else if (nType == CommRelationType.Friend)
                {
                    UserInfo toUser = bllUser.GetUserInfoByAutoID(int.Parse(mainId));
                    //删除好友关系
                    bLLCommRelation.DelCommRelation(CommRelationType.Friend, mainId, relationId);
                    bLLCommRelation.DelCommRelation(CommRelationType.Friend, relationId, mainId);

                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.DeleteFriend, this.currentUserInfo, null, new List<UserInfo>() { toUser }, null);
                }

                resp.isSuccess = true;
                resp.errmsg = "删除完成";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "删除失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 错误码
        /// </summary>
        private enum errcode
        {
            /// <summary>
            /// 未登录
            /// </summary>
            UnLogin = -2

        }
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}