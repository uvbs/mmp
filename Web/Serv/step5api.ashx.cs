using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using System.Reflection;
using ZentCloud.BLLJIMP.Model.API.step5;
using System.Text;
using ZentCloud.Common;
using ThoughtWorks.QRCode.Codec;
namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// 五步会api
    /// </summary>
    public class step5api : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 基本响应模型
        /// </summary>
        DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// 网站所有者
        /// </summary>
        private string WebSiteOwner;
        /// <summary>
        /// BLL 基类
        /// </summary>
        BLL bll = new BLL();
        /// <summary>
        /// 活动业务逻辑
        /// </summary>
        BLLJIMP.BLLJuActivity bllJuactivity = new BLLJIMP.BLLJuActivity();
        /// <summary>
        /// 真正活动BLL
        /// </summary>
        BLLJIMP.BLLActivity bllActivity = new BLLJIMP.BLLActivity("");
        /// <summary>
        /// 用户业务逻辑
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        /// <summary>
        /// 邮件BLL
        /// </summary>
        BLLEDM bllEmail = new BLLEDM("");
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLWeixin bllWeixin = new BLLWeixin("");
        /// <summary>
        /// 系统通知
        /// </summary>
        BLLSystemNotice bllSystemNotice = new BLLSystemNotice();
        /// <summary>
        /// 基路径 形式如 http://preview.comeoncloud.com.cn
        /// </summary>
        //private string BasePath;
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo CurrentUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                if (bll.IsLogin)
                {
                    CurrentUserInfo = bll.GetCurrentUserInfo();
                }
                WebSiteOwner = bll.WebsiteOwner;
                //BasePath = string.Format("http://{0}", context.Request.Url.Host);
                string action = context.Request["action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法 
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.errcode = -3;
                    resp.errmsg = "action not exist";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {

                resp.errcode = -1;
                resp.errmsg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }

            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                result = string.Format("{0}({1})", context.Request["callback"], result);

            }
            else
            {
                //返回json数据
            }
            context.Response.Write(result);

        }

        #region 活动模块
        /// <summary>
        /// 获取活动分类列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getactivitycategorylist(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            categoryapi apiResult = new categoryapi();
            apiResult.list = new List<category>();
            var sourceData = bll.GetLit<ArticleCategory>(pageSize, pageIndex, string.Format(" CategoryType='activity' And WebsiteOwner='{0}' ", WebSiteOwner));
            apiResult.totalcount = bll.GetCount<ArticleCategory>(string.Format(" CategoryType='activity' And WebsiteOwner='{0}' ", WebSiteOwner));
            foreach (var item in sourceData)
            {
                category model = new category();
                model.categoryid = item.AutoID;
                model.categoryname = item.CategoryName;
                model.contentcount = bll.GetCount<JuActivityInfo>(string.Format("CategoryId={0} And IsDelete=0", item.AutoID));
                apiResult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getactivitylist(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            string sort = context.Request["sort"];//排序
            string cateId = context.Request["cateid"];//分类
            string keyword = context.Request["keyword"];//关键字
            string isMystr = context.Request["ismy"];//是否是我报名的活动
            string myPraise = context.Request["mypraise"];//是否是我赞过的活动
            string status=context.Request["status"];//活动状态
            string isAll = context.Request["isall"];
            bool isMy = false;//我参加的活动
            bool isMyPraise = false;//我赞过的活动
            if ((!string.IsNullOrEmpty(isMystr)) && (isMystr.Equals("true")))
            {
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)errcode.UnLogin;
                    resp.errmsg = "请先登录";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                else
                {
                    isMy = true;
                }

            }
            if ((!string.IsNullOrEmpty(myPraise)) && (myPraise.Equals("true")))
            {
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)errcode.UnLogin;
                    resp.errmsg = "请先登录";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                else
                {
                    isMyPraise = true;
                }

            }
            if (!string.IsNullOrEmpty(status))
            {

                switch (status)
                {
                    case "0":
                        status = "0";//进行中
                        break;
                    case "1":
                        status = "1";//已结束
                        break;
                    case "3":
                        status = "-1";//即将开始
                        break;
                    default:
                        break;
                }
            }

            return QueryActivitylist(pageIndex, pageSize, sort, cateId, keyword, isMy, isMyPraise,status,isAll);
        }

        /// <summary>
        /// 获取已经报名人列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getsignpersonlist(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            int activityId = int.Parse(context.Request["activityid"]);//活动id
            JuActivityInfo juInfo = bllJuactivity.GetJuActivity(activityId);
            signpersonapi apiResult = new signpersonapi();
            apiResult.list = new List<signperson>();
            apiResult.totalcount = bll.GetCount<ActivityDataInfo>(string.Format("ActivityID='{0}' And WebsiteOwner='{1}' AND IsDelete=0", juInfo.SignUpActivityID, WebSiteOwner));
            var sourceData = bll.GetLit<ActivityDataInfo>(pageSize, pageIndex, string.Format("ActivityID='{0}' And WebsiteOwner='{1}' AND IsDelete=0", juInfo.SignUpActivityID, WebSiteOwner), " InsertDate DESC");
            foreach (var item in sourceData)
            {
                signperson model = new signperson();
                model.name = item.Name;
                if (juInfo.ShowPersonnelListType.Equals(1))
                {
                    model.name = model.name.Substring(0, 1) + "**";
                }
                model.time = bll.GetTimeStamp(item.InsertDate);
                model.headimg = string.Format("http://{0}{1}",context.Request.Url.Host,"/img/persion.png");
                if (!string.IsNullOrEmpty(item.WeixinOpenID))
                {
                    UserInfo userInfo = bllUser.GetUserInfoByOpenId(item.WeixinOpenID);
                    if (userInfo != null&&(!string.IsNullOrEmpty(userInfo.WXHeadimgurlLocal)))
                    {
                        model.headimg = bll.GetImgUrl(userInfo.WXHeadimgurlLocal);
                    }

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
        private string getactivitydetail(HttpContext context)
        {

            int activityId = int.Parse(context.Request["activityid"]);
            JuActivityInfo juInfo = bllJuactivity.GetJuActivity(activityId);
            juInfo.PV++;
            bll.Update(juInfo);
            activitydetail apiResult = new activitydetail();
            apiResult.signfield = new List<signfield>();
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
            if (juInfo.IsHide == -1)
            {
                apiResult.status = 3;
            }
            apiResult.activitycontent = juInfo.ActivityDescription;
            if (juInfo.ActivityDescription.Contains("/FileUpload/"))
            {
                apiResult.activitycontent = juInfo.ActivityDescription.Replace("/FileUpload/", string.Format("http://{0}/FileUpload/", context.Request.Url.Host));
            }
            apiResult.score = juInfo.ActivityIntegral;
            if (bll.IsLogin)
            {
                BLLJIMP.Model.ForwardingRecord record = bll.Get<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' AND RUserID='{1}' AND websiteOwner='{2}' AND TypeName = '活动赞'", CurrentUserInfo.UserID, apiResult.activityid, bll.WebsiteOwner));
                if (record != null)
                {
                    apiResult.ispraise = true;
                }

            }
            var fieldList = bllActivity.GetActivityFieldMappingList(juInfo.SignUpActivityID);
            foreach (var item in fieldList)
            {
                signfield model = new signfield();
                model.key = item.MappingName;
                model.value = item.FieldName;
                apiResult.signfield.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 提交报名数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string submitactivitysigndata(HttpContext context)
        {
            int activityId = int.Parse(context.Request["activityid"]);
            JuActivityInfo juInfo = bllJuactivity.GetJuActivity(activityId);
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
                goto outoff;

            }
            if (juInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "活动不存在!";
                goto outoff;

            }
            #region 是否可以报名
            if (juInfo.IsHide.Equals(1))
            {
                resp.errcode = 2;
                resp.errmsg = "活动已结束";
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
                if (CurrentUserInfo.TotalScore < juInfo.ActivityIntegral)
                {
                    resp.errcode = 4;
                    resp.errmsg = "您的积分不足";
                    goto outoff;

                }

            }

            #region 指定标签的用户可以报名

            bool perActivity = false;
            if (!string.IsNullOrEmpty(juInfo.Tags))
            {

                if (!string.IsNullOrEmpty( CurrentUserInfo.TagName))
                {
                    foreach (string item in CurrentUserInfo.TagName.Split(','))
                    {
                        if (juInfo.Tags.Contains(item))
                        {
                            perActivity = true;
                            break;
                        }
                    }
                }

            }
            else
            {
                perActivity = true;
               
            }
            if (!perActivity)
            {
                 resp.errcode = 19;
                 resp.errmsg = "只接受特定用户报名";
                 goto outoff;
            }
            #endregion
            #endregion
            /// <summary>
            /// 当前请求参数键值对
            /// </summary>
            Dictionary<string, string> dicPar = bll.GetRequestParameter();
            //string WeixinOpenID = null;
            string activityID = juInfo.SignUpActivityID;
            //string SpreadUserID = null;
            //DicPar.TryGetValue("SpreadUserID", out SpreadUserID);
            //string StrDistinctKeys = null;//检查重复的字段，多个字段用,分隔， //没有此参数默认用手机检查  
            //DicPar.TryGetValue("DistinctKeys", out StrDistinctKeys);
            //string MonitorPlanID = null;
            //DicPar.TryGetValue("MonitorPlanID", out MonitorPlanID);
            string name = null;
            dicPar.TryGetValue("Name", out name);
            string phone = null;
            dicPar.TryGetValue("Phone", out phone);
            ActivityInfo activity = bll.Get<ActivityInfo>(string.Format("ActivityID='{0}'", activityID));

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
            List<ActivityFieldMappingInfo> fieldMap = bllActivity.GetActivityFieldMappingList(activity.ActivityID);
            foreach (var item in fieldMap)
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
                    System.Text.RegularExpressions.Match m = regUrl.Match(value);
                    if (!m.Success)
                    {
                        resp.errcode = 13;
                        resp.errmsg = string.Format("{0}格式不正确", item.MappingName);
                        goto outoff;

                    }
                }
            }
            #endregion

            //#region 检查是否已经报名
            //if (!string.IsNullOrEmpty(StrDistinctKeys))
            //{

            //    if (!StrDistinctKeys.Equals("none"))//自定义检查重复
            //    {
            //        System.Text.StringBuilder sb = new System.Text.StringBuilder("1=1 ");
            //        string[] DistinctKeys = StrDistinctKeys.Split(',');
            //        foreach (var item in DistinctKeys)
            //        {
            //            sb.AppendFormat("And {0}='{1}' ", item, DicPar.Single(p => p.Key.Equals(item)).Value);
            //        }
            //        sb.Append("  and IsDelete = 0  ");
            //        if (bll.GetCount<ActivityDataInfo>(sb.ToString()) > 0)
            //        {

            //            resp.errcode = 14;
            //            resp.errmsg = "重复的报名!";
            //            goto outoff;


            //        }

            //    }
            //    else//不检查重复
            //    {

            //    }



            //}
            //else//默认检查
            //{

            //}
            if (bll.GetCount<ActivityDataInfo>(string.Format("ActivityID='{0}' And UserId='{1}' and IsDelete = 0 ", activityID, CurrentUserInfo.UserID)) > 0)
            {
                resp.errcode = 15;
                resp.errmsg = "已经报过名了!";
                goto outoff;


            }
            //#endregion

            var newActivityUID = 1001;
            var lastActivityDataInfo = bll.Get<ActivityDataInfo>(string.Format("ActivityID='{0}' order by UID DESC", activityID));
            if (lastActivityDataInfo != null)
            {
                newActivityUID = lastActivityDataInfo.UID + 1;
            }
            ActivityDataInfo model = bll.ConvertRequestToModel<ActivityDataInfo>(new ActivityDataInfo());
            model.UID = newActivityUID;
            //model.WeixinOpenID = WeixinOpenID;
            //model.SpreadUserID = SpreadUserID;
            model.ActivityID = activityID;
            //if (!string.IsNullOrEmpty(MonitorPlanID))
            //{
            //    model.MonitorPlanID = int.Parse(MonitorPlanID);
            //}
            model.WebsiteOwner = bll.WebsiteOwner;
            model.UserId = CurrentUserInfo.UserID;
            model.WeixinOpenID = CurrentUserInfo.WXOpenId;
            if (bll.Add(model))
            {
                resp.errmsg = "ok";
                if (juInfo.ActivityIntegral > 0)//扣积分
                {
                    CurrentUserInfo.TotalScore -= juInfo.ActivityIntegral;
                    if (bll.Update(CurrentUserInfo, string.Format("TotalScore={0}", CurrentUserInfo.TotalScore), string.Format(" AutoID={0}", CurrentUserInfo.AutoID)) <= 0)
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
                        scoreRecord.WebsiteOwner = WebSiteOwner;
                        scoreRecord.UserId = CurrentUserInfo.UserID;
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


        #endregion

        #region 文章模块
        /// <summary>
        /// 获取文章分类列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getarticlecategorylist(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            categoryapi apiResult = new categoryapi();
            apiResult.list = new List<category>();
            var sourceData = bll.GetLit<ArticleCategory>(pageSize, pageIndex, string.Format(" CategoryType='article' And WebsiteOwner='{0}' ", WebSiteOwner));
            apiResult.totalcount = bll.GetCount<ArticleCategory>(string.Format(" CategoryType='article' And WebsiteOwner='{0}' ", WebSiteOwner));
            foreach (var item in sourceData)
            {
                category model = new category();
                model.categoryid = item.AutoID;
                model.categoryname = item.CategoryName;
                model.contentcount = bll.GetCount<JuActivityInfo>(string.Format("CategoryId={0} And IsDelete=0", item.AutoID));
                apiResult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getarticlelist(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            string sort = context.Request["sort"];
            string cateId = context.Request["cateid"];
            string keyword = context.Request["keyword"];
            string myPraise = context.Request["mypraise"];//是否是我赞过的文章
            StringBuilder sbWhere = new StringBuilder(" 1=1");
            if ((!string.IsNullOrEmpty(myPraise)) && (myPraise.Equals("true")))
            {
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)errcode.UnLogin;
                    resp.errmsg = "请先登录";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                else
                {
                    ////兼容以前手机端的
                    sbWhere.AppendFormat(" And JuActivityID in(select RUserID from ZCJ_ForwardingRecord where FUserID='{0}' And  websiteOwner='{1}' AND TypeName = '文章赞')", CurrentUserInfo.UserID, bll.WebsiteOwner);
                    //

                }

            }

            string orderBy = "Sort DESC";//默认排序
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
            sbWhere.AppendFormat(" And ArticleType='article' AND IsDelete=0 And IsHide=0  AND WebsiteOwner='{0}'", WebSiteOwner);
            if (!string.IsNullOrEmpty(cateId))
            {
                sbWhere.AppendFormat(" And CategoryId={0}", cateId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sbWhere.AppendFormat(" And ActivityName like'%{0}%'", keyword);
            }
            articleapi apiResult = new articleapi();
            apiResult.list = new List<article>();
            apiResult.totalcount = bll.GetCount<JuActivityInfo>(sbWhere.ToString());
            var sourceData = bll.GetLit<JuActivityInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
            foreach (var item in sourceData)
            {
                article model = new article();
                model.articleid = item.JuActivityID;
                model.title = item.ActivityName;
                model.digest = item.Summary;
                model.time = bll.GetTimeStamp(item.CreateDate);
                model.pv = item.PV;
                model.categoryname = item.CategoryName;
                model.imgurl = bll.GetImgUrl(item.ThumbnailsPath);
                if (!string.IsNullOrEmpty(item.CategoryId))
                {
                    model.cateid = int.Parse(item.CategoryId);
                }
                apiResult.list.Add(model);
                model.type = "article";
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }


        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getarticledetail(HttpContext context)
        {
            int activityId = int.Parse(context.Request["articleid"]);
            articledetail apiResult = new articledetail();
            JuActivityInfo juInfo = bllJuactivity.GetJuActivity(activityId);
            if (juInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "文章不存在";
                goto outoff;
            }

            juInfo.PV++;
            bll.Update(juInfo);
            apiResult.articleid = juInfo.JuActivityID;
            apiResult.title = juInfo.ActivityName;
            apiResult.digest = juInfo.Summary;
            apiResult.categoryname = juInfo.CategoryName;
            apiResult.pv = juInfo.PV;
            apiResult.time = bll.GetTimeStamp(juInfo.CreateDate);
            apiResult.categoryname = juInfo.CategoryName;
            apiResult.articlecontent = juInfo.ActivityDescription;
            apiResult.imgurl = bll.GetImgUrl(juInfo.ThumbnailsPath);
            if (bll.IsLogin)
            {
                BLLJIMP.Model.ForwardingRecord record = bll.Get<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' AND RUserID='{1}' AND websiteOwner='{2}' AND TypeName = '文章赞'", CurrentUserInfo.UserID, apiResult.articleid, bll.WebsiteOwner));
                if (record != null)
                {
                    apiResult.ispraise = true;
                }

            }
            if (juInfo.ActivityDescription.Contains("/FileUpload/"))
            {
                apiResult.articlecontent = juInfo.ActivityDescription.Replace("/FileUpload/", string.Format("http://{0}/FileUpload/", context.Request.Url.Host));
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(apiResult);

        }




        #region 文章评论
        /// <summary>
        /// 获取文章评论信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getarticlereviewlist(HttpContext context)
        {

            string id = context.Request["articleid"];
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            articlereviewapi apiResult = new articlereviewapi();
            apiResult.list = new List<articlereview>();
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" ReviewID={0}", id);
            apiResult.totalcount = bll.GetCount<ReplyReviewInfo>(sbWhere.ToString());
            List<ReplyReviewInfo> replyList = bll.GetLit<BLLJIMP.Model.ReplyReviewInfo>(pageSize, pageIndex, sbWhere.ToString(), " AutoId desc");
            foreach (var item in replyList)
            {
                articlereview review = new articlereview();
                //目标评论内容
                if (item.PraentId > 0)
                {
                    var targetReply = bll.Get<ReplyReviewInfo>(string.Format("AutoId={0}", item.PraentId)); if (targetReply != null)
                    {
                        review.reply = new articlereplyreview();
                        review.reply.reviewcontent = targetReply.ReplyContent;
                        review.reply.nickname = targetReply.UserName;

                    }
                }

                //目标评论内容
                var userInfo = bllUser.GetUserInfo(item.UserId);
                if (userInfo != null)
                {
                    review.headimg = bll.GetImgUrl(userInfo.WXHeadimgurlLocal);

                }
                review.id = item.AutoId;
                review.nickname = item.UserName;
                review.time = bll.GetTimeStamp(item.InsertDate);
                review.reviewcontent = item.ReplyContent;
                if ((bll.IsLogin) && (item.UserId.Equals(CurrentUserInfo.UserID)))
                {
                    review.deleteflag = true;

                }

                review.type = "articlereview";
                apiResult.list.Add(review);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);


        }

        /// <summary>
        /// 发表评论信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string addarticlereview(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
                goto outoff;
            }
            if (!CheckUserIsReg(CurrentUserInfo))
            {
                resp.errcode = 5;
                resp.errmsg = "请先注册";
                goto outoff;
            }
            int id = int.Parse(context.Request["articleid"]);
            string content = context.Request["content"];
            if (string.IsNullOrEmpty(content))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入评论内容";
                goto outoff;
            }
            if (!CheckArtickeReviewContent(content))
            {
                resp.errcode = 2;
                resp.errmsg = "您的评论含有敏感词,请重新编辑评论内容";
                goto outoff;
            }
            if (bllJuactivity.GetJuActivity(id) == null)
            {
                resp.errcode = 3;
                resp.errmsg = "文章不存在";
                goto outoff;
            }
            bool addResult = bll.Add(new BLLJIMP.Model.ReplyReviewInfo
            {
                ReviewID = id,
                ReplyContent = content,
                UserId = CurrentUserInfo.UserID,
                InsertDate = DateTime.Now,
                WebSiteOwner = bll.WebsiteOwner,
                UserName = CurrentUserInfo.TrueName ?? "匿名用户",
                ReviewType = "文章评论",

            });
            if (addResult)
            {
                resp.errcode = 0;
                resp.errmsg = "评论成功";
                goto outoff;
            }
            else
            {
                resp.errcode = 4;
                resp.errmsg = "评论失败";
            }

        outoff:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除文章评论
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string deletearticlereview(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
                goto outoff;
            }
            string id = context.Request["id"];
            ReplyReviewInfo review = bll.Get<ReplyReviewInfo>(string.Format(" AutoID={0} And UserId='{1}'", id, CurrentUserInfo.UserID));
            if (review == null)
            {
                resp.errcode = 1;
                resp.errmsg = "无权删除";
                goto outoff;

            }
            if (bll.Delete(review) == 0)
            {
                resp.errcode = 2;
                resp.errmsg = "删除失败";
                goto outoff;
            }
            resp.errmsg = "删除成功";
        //bll.Delete(new ReplyReviewInfo(), string.Format(" PraentId={0}", Review.AutoId));
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 发表回复评论信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string replyarticlereview(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
                goto outoff;
            }
            if (!CheckUserIsReg(CurrentUserInfo))
            {
                resp.errcode = 5;
                resp.errmsg = "请先注册";
                goto outoff;
            }
            int articleId = int.Parse(context.Request["articleid"]);
            int id = int.Parse(context.Request["id"]);//评论ID
            string content = context.Request["replycontent"];
            if (string.IsNullOrEmpty(content))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入评论内容";
                goto outoff;
            }
            if (!CheckArtickeReviewContent(content))
            {
                resp.errcode = 2;
                resp.errmsg = "您的评论包含不适合发布的内容,请重新编辑";
                goto outoff;
            }
            bool addResult = bll.Add(new BLLJIMP.Model.ReplyReviewInfo
            {
                ReplyContent = content,
                UserId = CurrentUserInfo.UserID,
                InsertDate = DateTime.Now,
                WebSiteOwner = bll.WebsiteOwner,
                UserName = CurrentUserInfo.TrueName ?? "匿名用户",
                ReviewType = "文章评论",
                PraentId = id,
                ReviewID = articleId

            });
            if (addResult)
            {
                resp.errcode = 0;
                resp.errmsg = "回复成功";
                //if (!context.Request.Url.Equals("preview.comeoncloud.com.cn"))
                //{
                    ///////消息提示
                    ReplyReviewInfo review = bll.Get<ReplyReviewInfo>(string.Format(" AutoID={0}", id));
                    bllSystemNotice.SendSystemMessage("您的评论有新的回复", "您的评论有新的回复,点击查看", 1, 2, review.UserId, string.Format("http://{0}/WuBuHui/News/NewsDetail.aspx?id={1}", context.Request.Url.Host, articleId), WebSiteOwner);
                    /////消息提示
                //}
                goto outoff;
            }
            else
            {
                resp.errcode = 3;
                resp.errmsg = "回复失败";
            }

        outoff:
            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 检查文章评论是否通过
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private bool CheckArtickeReviewContent(string content)
        {
            bool result = true;
            foreach (var item in bll.GetList<FilterWord>(string.Format(" WebsiteOwner='{0}' And FilterType=0", bll.WebsiteOwner)))
            {
                if (content.Trim().ToLower().Contains(item.Word))
                {
                    result = false;
                }
            }

            return result;

        }



        #endregion

        #endregion

        #region 导师模块
        /// <summary>
        /// 获取导师列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getmasterlist(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            string keyWord = context.Request["keyword"];//关键字
            string professionalId = context.Request["professionalid"];//专业id
            string tradeId = context.Request["tradeid"];//行业id
            string sort = context.Request["sort"];//排序
            string orderBy = " TutorName ASC";//默认排序
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" websiteOwner='{0}'", WebSiteOwner);
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And (TutorName like'%{0}%' or Position  like'%{0}%' or City  like'%{0}%' Or Company like '%{0}%')", keyWord);
            }

            //
            //if ((!string.IsNullOrEmpty(ProfessionalId)) && (!string.IsNullOrEmpty(TradeId)))
            //{
            //    SbWhere.AppendFormat(" And (charindex('{0}',ProfessionalStr)>0 Or charindex('{1}',TradeStr)>0)", ProfessionalId, TradeId);
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(ProfessionalId))
            //    {
            //        SbWhere.AppendFormat(" And charindex('{0}',ProfessionalStr)>0", ProfessionalId);
            //    }
            //    if (!string.IsNullOrEmpty(TradeId))
            //    {
            //        SbWhere.AppendFormat(" And charindex('{0}',TradeStr)>0", TradeId);
            //    }
            //}
            ////
            //
            if ((!string.IsNullOrEmpty(tradeId)))
            {


                //
                sbWhere.AppendFormat(" And ( ");
                for (int i = 0; i < tradeId.Split(',').Length; i++)
                {
                    if (i == 0)
                    {
                        sbWhere.AppendFormat(" TradeStr Like '%{0}%' ", tradeId.Split(',')[i]);

                    }
                    else
                    {
                        sbWhere.AppendFormat(" Or TradeStr Like '%{0}%' ", tradeId.Split(',')[i]);

                    }




                }

                sbWhere.AppendFormat(" ) ");

            }

            //
            if ((!string.IsNullOrEmpty(professionalId)))
            {

                //
                sbWhere.AppendFormat(" And ( ");
                for (int i = 0; i < professionalId.Split(',').Length; i++)
                {
                    if (i == 0)
                    {
                        sbWhere.AppendFormat(" ProfessionalStr Like '%{0}%' ", professionalId.Split(',')[i]);

                    }
                    else
                    {
                        sbWhere.AppendFormat(" Or ProfessionalStr Like '%{0}%' ", professionalId.Split(',')[i]);

                    }




                }

                sbWhere.AppendFormat(" ) ");

            }





            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "questioncount":
                        orderBy = " HTNums DESC";
                        break;
                    default:
                        break;
                }
            }

            mastelistrapi apiResult = new mastelistrapi();
            apiResult.list = new List<master>();
            apiResult.totalcount = bll.GetCount<TutorInfo>(sbWhere.ToString());

            var sourceData = bll.GetLit<TutorInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
            foreach (var item in sourceData)
            {
                master model = new master();
                model.headimg = bll.GetImgUrl(item.TutorImg);
                model.userid = item.UserId;
                if (string.IsNullOrEmpty(model.userid))
                {
                    model.userid = item.AutoId.ToString();
                }
                model.truename = item.TutorName;
                model.position = item.Position;
                model.digest = item.Digest;
                model.pv = item.Pv;
                model.praisecount = item.TutorLikes;
                model.company = item.Company;
                model.city = item.City;
                model.askcount = bll.GetCount<ReviewInfo>(string.Format(" UserId='{0}' And WebSiteOwner='{1}' And ReviewType='话题'", item.UserId, WebSiteOwner));
                model.type = "master";
                if (!string.IsNullOrEmpty(item.UserId))
                {
                    model.canaskorattention = true;
                    model.fanscount = bll.GetCount<UserFollowChain>(string.Format(" ToUserId='{0}'", item.UserId));
                }
                model.articlecount = item.WZNums;
                model.tradetags = new List<string>();
                model.professionaltags = new List<string>();
                model.questioncount = item.HTNums;
                if (bll.IsLogin)
                {
                    model.isattention = IsAttention(CurrentUserInfo.UserID, item.UserId);
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
                                model.professionaltags.Add(tag.CategoryName);

                            }

                        }
                    }
                }
                if (!string.IsNullOrEmpty(item.TradeStr))
                {
                    foreach (var tagid in item.TradeStr.Split(','))
                    {
                        if (!string.IsNullOrEmpty(tagid))
                        {
                            var tag = bll.Get<ArticleCategory>(string.Format(" AutoID={0}", tagid));
                            if (tag != null)
                            {
                                model.tradetags.Add(tag.CategoryName);

                            }

                        }
                    }
                }

                apiResult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 获取导师详情
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getmasterdetail(HttpContext context)
        {
            TutorInfo masterInfo;
            string masterUserId = context.Request["userid"];//导师的用户名
            if (string.IsNullOrEmpty(masterUserId))
            {
                resp.errcode = 1;
                resp.errmsg = "用户名不能为空";
                goto outoff;
            }
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" websiteOwner='{0}'", WebSiteOwner);
            sbWhere.AppendFormat(" And UserId='{0}'", masterUserId);
            masterInfo = bll.Get<TutorInfo>(sbWhere.ToString());
            if (masterInfo == null)
            {
                masterInfo = bll.Get<TutorInfo>(string.Format(" websiteOwner='{0}' And AutoId='{1}'", WebSiteOwner, masterUserId));
                if (masterInfo == null)
                {
                    resp.errcode = 2;
                    resp.errmsg = "导师不存在";
                    goto outoff;

                }
            }
            masterdetail model = new masterdetail();
            model.headimg = bll.GetImgUrl(masterInfo.TutorImg);
            model.userid = masterInfo.UserId;
            if (string.IsNullOrEmpty(model.userid))
            {
                model.userid = masterInfo.AutoId.ToString();
            }
            model.truename = masterInfo.TutorName;
            model.position = masterInfo.Position;
            model.digest = masterInfo.Digest;
            model.pv = masterInfo.Pv;
            if (!string.IsNullOrEmpty(masterInfo.UserId))
            {
                model.canaskorattention = true;
            }
            model.praisecount = masterInfo.TutorLikes;
            model.articlecount = masterInfo.WZNums;
            model.professionaltags = new List<string>();
            model.tradetags = new List<string>();
            if (bll.IsLogin)
            {
                model.isattention = IsAttention(CurrentUserInfo.UserID, masterInfo.UserId);
            }
            if (!string.IsNullOrEmpty(masterInfo.TradeStr))
            {
                foreach (var tagid in masterInfo.TradeStr.Split(','))
                {
                    if (!string.IsNullOrEmpty(tagid))
                    {
                        var tag = bll.Get<ArticleCategory>(string.Format(" AutoID={0}", tagid));
                        if (tag != null)
                        {
                            model.tradetags.Add(tag.CategoryName);

                        }

                    }
                }
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
                            model.professionaltags.Add(tag.CategoryName);

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
                model.askcount = bll.GetCount<ReviewInfo>(string.Format(" UserId='{0}' And WebSiteOwner='{1}' And ReviewType='话题'", masterInfo.UserId, WebSiteOwner));

                model.beaskcount = masterInfo.HTNums;

            }
            masterInfo.Pv++;
            bll.Update(masterInfo);
            return Common.JSONHelper.ObjectToJson(model);
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }



        /// <summary>
        /// 添加关注
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string addattention(HttpContext context)
        {
            string toUserId = context.Request["touserid"];//导师的用户名
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
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
                resp.errmsg = "不能关注该导师";
                goto outoff;
            }
            if (bll.GetCount<UserFollowChain>(string.Format(" FromUserId='{0}' And ToUserId='{1}'", CurrentUserInfo.UserID, toUserInfo.UserID)) > 0)
            {
                resp.errcode = 2;
                resp.errmsg = "已经关注过了";
                goto outoff;
            }
            UserFollowChain model = new UserFollowChain();
            model.FromUserId = CurrentUserInfo.UserID;
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
        private string cancelattention(HttpContext context)
        {
            string toUserId = context.Request["touserid"];//取消关注的导师的用户名
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
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
            if (bll.Delete(new UserFollowChain(), string.Format(" FromUserId='{0}' And ToUserId='{1}'", CurrentUserInfo.UserID, toUserInfo.UserID)) > 0)
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
        #endregion

        #region HRLOVE 模块
        /// <summary>
        /// 获取HRLOVE列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string gethrlovelist(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            string keyWord = context.Request["keyword"];
            string orderBy = " AutoID ASC";//默认排序
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'", WebSiteOwner);
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And Name like'%{0}%'", keyWord);
            }
            hrloveapi apiResult = new hrloveapi();
            apiResult.list = new List<hrlove>();
            apiResult.totalcount = bll.GetCount<GameFriendChain>(sbWhere.ToString());
            var sourceData = bll.GetLit<GameFriendChain>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
            foreach (var item in sourceData)
            {
                hrlove model = new hrlove();
                model.id = (int)item.AutoId;
                model.headimg =bll.GetImgUrl(item.PhotoUrl);
                model.truename = item.Name;
                model.donateamount = item.DonateCount;
                model.starsign = item.StarSign;
                model.type = "hrlove";
                apiResult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 获取hrlove详情
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string gethrlovedetail(HttpContext context)
        {
            hrlovedetail apiResult = new hrlovedetail();
            string id = context.Request["id"];
            if (string.IsNullOrEmpty(id))
            {
                resp.errcode = 1;
                resp.errmsg = "id不能为空";
                goto outoff;
            }
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("AutoId={0}", id);
            GameFriendChain model = bll.Get<GameFriendChain>(sbWhere.ToString());
            apiResult.currentuser = new hrlove();
            apiResult.currentuser.id = (int)model.AutoId;
            apiResult.currentuser.headimg = bll.GetImgUrl(model.PhotoUrl);
            apiResult.currentuser.truename = model.Name;
            apiResult.currentuser.donateamount = model.DonateCount;
            apiResult.currentuser.starsign = model.StarSign;
            if (!string.IsNullOrEmpty(model.PreviousUserId))
            {
                GameFriendChain preUser = bll.Get<GameFriendChain>(string.Format("UserId='{0}'", model.PreviousUserId));
                apiResult.preuser = new PreNextUser();
                apiResult.preuser.id = (int)preUser.AutoId;
                apiResult.preuser.headimg = bll.GetImgUrl(preUser.PhotoUrl);
            }
            if (!string.IsNullOrEmpty(model.Next1UserId))
            {
                GameFriendChain nextUser1 = bll.Get<GameFriendChain>(string.Format("UserId='{0}'", model.Next1UserId));
                apiResult.nextuser1 = new PreNextUser();
                apiResult.nextuser1.id = (int)nextUser1.AutoId;
                apiResult.nextuser1.headimg = bll.GetImgUrl(nextUser1.PhotoUrl);
            }
            if (!string.IsNullOrEmpty(model.Next2UserId))
            {
                GameFriendChain nextUser2 = bll.Get<GameFriendChain>(string.Format("UserId='{0}'", model.Next2UserId));
                apiResult.nextuser2 = new PreNextUser();
                apiResult.nextuser2.id = (int)nextUser2.AutoId;
                apiResult.nextuser2.headimg = bll.GetImgUrl(nextUser2.PhotoUrl);
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(apiResult);

        }


        #endregion

        #region 职位招聘 模块
        /// <summary>
        /// 获取职位列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getpositionlist(HttpContext context)
        {
            positionapi apiResult = new positionapi();
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            string keyWord = context.Request["keyword"];
            string isMy = context.Request["ismy"];//是否是我申请的职位
            string professionalId = context.Request["professionalid"];//专业id
            string tradeId = context.Request["tradeid"];//行业id
            string sort = context.Request["sort"];//排序号
            string orderBy = " AutoID ASC";//默认排序
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'", WebSiteOwner);
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And  (Title like'%{0}%' OR Company like'%{0}%' Or City like'%{0}%' )", keyWord);
            }


            //if ((!string.IsNullOrEmpty(ProfessionalId)) && (!string.IsNullOrEmpty(TradeId)))
            //{
            //    SbWhere.AppendFormat(" And (charindex('{0}',ProfessionalIds)>0 Or charindex('{1}',TradeIds)>0)", ProfessionalId, TradeId);
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(ProfessionalId))
            //    {
            //        SbWhere.AppendFormat(" And charindex('{0}',ProfessionalIds)>0", ProfessionalId);
            //    }
            //    if (!string.IsNullOrEmpty(TradeId))
            //    {
            //        SbWhere.AppendFormat(" And charindex('{0}',TradeIds)>0", TradeId);
            //    }
            //}
            if (!string.IsNullOrEmpty(tradeId))
            {
                sbWhere.AppendFormat(" And ( ");
                for (int i = 0; i < tradeId.Split(',').Length; i++)
                {
                    if (i == 0)
                    {
                        sbWhere.AppendFormat(" TradeIds Like '%{0}%' ", tradeId.Split(',')[i]);

                    }
                    else
                    {
                        sbWhere.AppendFormat(" Or TradeIds Like '%{0}%' ", tradeId.Split(',')[i]);

                    }
                }

                sbWhere.AppendFormat(" ) ");
            }
            if (!string.IsNullOrEmpty(professionalId))
            {
                sbWhere.AppendFormat(" And ( ");
                for (int i = 0; i < professionalId.Split(',').Length; i++)
                {
                    if (i == 0)
                    {
                        sbWhere.AppendFormat(" ProfessionalIds Like '%{0}%' ", professionalId.Split(',')[i]);

                    }
                    else
                    {
                        sbWhere.AppendFormat(" Or ProfessionalIds Like '%{0}%' ", professionalId.Split(',')[i]);

                    }
                }

                sbWhere.AppendFormat(" ) ");
            }

            if ((!string.IsNullOrEmpty(isMy)) && (isMy.Equals("true")))
            {
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)errcode.UnLogin;
                    resp.errmsg = "请先登录";
                    goto outff;
                }

                sbWhere.AppendFormat(" And AutoId in(select PositionId from ZCJ_ApplyPositionInfo where UserId='{0}')", CurrentUserInfo.UserID);

            }
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "pv":
                        orderBy = " Pv DESC";//按浏览量降序
                        break;
                    case "time":
                        orderBy = " InsertDate DESC";//按入库时间降序
                        break;
                    default:
                        break;
                }

            }
            apiResult.list = new List<position>();
            apiResult.totalcount = bll.GetCount<PositionInfo>(sbWhere.ToString());
            var sourceData = bll.GetLit<PositionInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
            foreach (var item in sourceData)
            {
                position model = new position();
                model.id = (int)item.AutoId;
                model.imgurl = item.IocnImg;
                model.positionname = item.Title;

                model.time = bll.GetTimeStamp(item.InsertDate);
                model.workyear = item.WorkYear;
                model.company = item.Company;
                model.city = item.City;
                model.education = item.Education;
                model.type = "job";
                model.pv = item.Pv;
                model.tags = new List<string>();
                if (!string.IsNullOrEmpty(item.ProfessionalIds))
                {
                    foreach (var tag in bll.GetList<ArticleCategory>(string.Format("AutoID in({0})", item.ProfessionalIds)))
                    {

                        model.tags.Add(tag.CategoryName);
                    }

                }
                if (!string.IsNullOrEmpty(item.TradeIds))
                {
                    foreach (var tag in bll.GetList<ArticleCategory>(string.Format("AutoID in({0})", item.TradeIds)))
                    {

                        model.tags.Add(tag.CategoryName);
                    }

                }
                apiResult.list.Add(model);
            }
        outff:
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 获取职位详情
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getpositiondetail(HttpContext context)
        {
            positiondetail apiResult = new positiondetail();
            string id = context.Request["id"];
            if (string.IsNullOrEmpty(id))
            {
                resp.errcode = 1;
                resp.errmsg = "id不能为空";
                goto outoff;
            }
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("AutoId={0}", id);
            PositionInfo model = bll.Get<PositionInfo>(sbWhere.ToString());
            model.Pv++;
            apiResult.id = (int)model.AutoId;
            apiResult.imgurl = model.IocnImg;
            apiResult.positionname = model.Title;
            apiResult.time = bll.GetTimeStamp(model.InsertDate);
            apiResult.workyear = model.WorkYear;
            apiResult.company = model.Company;
            apiResult.city = model.City;
            apiResult.education = model.Education;
            apiResult.pv = model.Pv;
            apiResult.tradetags = new List<string>();
            apiResult.professionaltags = new List<string>();
            if (!string.IsNullOrEmpty(model.TradeIds))
            {
                foreach (var tag in bll.GetList<ArticleCategory>(string.Format("AutoID in({0})", model.TradeIds)))
                {

                    apiResult.tradetags.Add(tag.CategoryName);
                }

            }
            if (!string.IsNullOrEmpty(model.ProfessionalIds))
            {
                foreach (var tag in bll.GetList<ArticleCategory>(string.Format("AutoID in({0})", model.ProfessionalIds)))
                {

                    apiResult.professionaltags.Add(tag.CategoryName);
                }

            }
            apiResult.enterprisescale = model.EnterpriseScale;
            apiResult.content = model.Context;
            if (model.Context.Contains("/FileUpload/"))
            {
                apiResult.content = model.Context.Replace("/FileUpload/", string.Format("http://{0}/FileUpload/", context.Request.Url.Host));
            }
            bll.Update(model, "Pv+=1", string.Format(" AutoId={0}", model.AutoId));

        outoff:
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 申请职位
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string applyposition(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
                goto outoff;
            }
            string id = context.Request["id"];
            if (string.IsNullOrEmpty(id))
            {
                resp.errcode = 1;
                resp.errmsg = "id不能为空";
                goto outoff;
            }
            BLLJIMP.Model.PositionInfo positionInfo = bll.Get<BLLJIMP.Model.PositionInfo>(string.Format(" WebsiteOwner='{0}' AND AutoId='{1}'", WebSiteOwner, id));
            if (positionInfo == null)
            {
                resp.errcode = 2;
                resp.errmsg = "职位不存在";
                goto outoff;
            }
            string tradeIds = context.Request["tradeids"];
            string professionalIds = context.Request["professionalids"];
            if (bll.GetCount<BLLJIMP.Model.ApplyPositionInfo>(string.Format("UserId='{0}' And PositionId={1}", CurrentUserInfo.UserID, id)) > 0)
            {
                resp.errcode = 3;
                resp.errmsg = "已经申请过该职位";
                goto outoff;

            }

            BLLJIMP.Model.ApplyPositionInfo applyInfo = new BLLJIMP.Model.ApplyPositionInfo()
            {
                ComeTime = DateTime.Now,
                UserId = CurrentUserInfo.UserID,
                UserName = CurrentUserInfo.TrueName != null ? CurrentUserInfo
.TrueName : CurrentUserInfo.WXNickname,
                WebsiteOwner = WebSiteOwner,
                PositionId = Convert.ToInt32(id),
                Phone = CurrentUserInfo.Phone,
                Trade = tradeIds,
                Professional = professionalIds

            };
            if (bll.Add(applyInfo))
            {

                positionInfo.PersonNum++;
                bll.Update(positionInfo, string.Format("PersonNum={0}", positionInfo.PersonNum), string.Format("AutoId={0}", positionInfo.AutoId));
                resp.errcode = 0;
                resp.errmsg = "申请成功";
                //发送提醒邮件
                bllEmail.Step5ApplyPostionRemind(applyInfo, positionInfo, CurrentUserInfo);
                //发送提醒邮件

            }
            else
            {
                resp.errcode = 4;
                resp.errmsg = "申请失败";
            }

        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }


        #endregion

        #region 话题模块
        /// <summary>
        /// 获取话题列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getasklist(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            asklistapi apiResult = new asklistapi();
            apiResult.list = new List<ask>();
            string reviewTitle = context.Request["keyword"];
            //string Ctype = context.Request["type"];
            string isMyAsk = context.Request["ismyask"];
            string isReply = context.Request["isreply"];
            string sort = context.Request["sort"];
            string masterUserId = context.Request["masteruserid"];
            string myPraise = context.Request["mypraise"];
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");
            StringBuilder sbSort = new StringBuilder();
            sbWhere.AppendFormat(" And ReviewType='话题' AND websiteOwner='{0}'", WebSiteOwner);
            if (!string.IsNullOrEmpty(reviewTitle))
            {
                sbWhere.AppendFormat(" AND ReviewTitle like '%{0}%'", reviewTitle);
            }
            //if (!string.IsNullOrEmpty(Ctype))
            //{
            //    SbWhere.AppendFormat(" AND CategoryType LIKE '%{0}%'", Ctype);
            //}

            if (!string.IsNullOrEmpty(masterUserId))
            {
                sbWhere.AppendFormat(" AND ForeignkeyId = '{0}'", masterUserId);
            }
            if ((!string.IsNullOrEmpty(isMyAsk)) && (isMyAsk.Equals("true")))
            {
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)errcode.UnLogin;
                    resp.errmsg = "请先登录";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                sbWhere.AppendFormat(" AND (ForeignkeyId='{0}' Or UserId='{0}' Or AutoId in(select ReviewID from ZCJ_ReplyReviewInfo where UserId='{0}'))", CurrentUserInfo.UserID);


            }
            else
            {
                sbWhere.AppendFormat(" And ReviewPower=0");
            }
            if ((!string.IsNullOrEmpty(myPraise)) && (myPraise.Equals("true")))
            {
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)errcode.UnLogin;
                    resp.errmsg = "请先登录";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                else
                {
                    ////兼容以前的
                    sbWhere.AppendFormat(" And AutoId in(select RUserID from ZCJ_ForwardingRecord where FUserID='{0}' And  websiteOwner='{1}' AND TypeName = '话题赞')", CurrentUserInfo.UserID, bll.WebsiteOwner);
                    //

                }

            }
            if (!string.IsNullOrEmpty(isReply))
            {
                sbWhere.Append(" AND NumCount>0");

            }
            sbSort.Append(" ReplyDateTiem DESC ");
            if (!string.IsNullOrEmpty(sort))
            {


                if (sort.Equals("replytime"))
                {
                    sbSort.Clear();
                    sbSort.Append(" ReplyDateTiem DESC");
                }
                if (sort.Equals("addtime"))
                {
                    sbSort.Clear();
                    sbSort.Append(" AutoId DESC");
                }
                //if (Sort.Equals("Mosthf"))
                //{
                //    SbSort.Clear();
                //    SbSort.Append(" NumCount DESC");
                //}

                //if (Sort.Equals("Mosthp"))
                //{
                //    SbSort.Clear();
                //    SbSort.Append(" PraiseNum DESC, ReplyDateTiem DESC");
                //}


            }
            apiResult.totalcount = bll.GetCount<ReviewInfo>(sbWhere.ToString());
            List<BLLJIMP.Model.ReviewInfo> SourceData = bll.GetLit<BLLJIMP.Model.ReviewInfo>(pageSize, pageIndex, sbWhere.ToString(), sbSort.ToString());
            foreach (var item in SourceData)
            {
                ask model = new ask();
                #region 头像
                if (IsMaster(item.UserId, WebSiteOwner))
                {
                    TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", item.UserId));
                    model.headimg = bll.GetImgUrl(masterInfo.TutorImg);
                }
                else
                {
                    UserInfo userInfo = bllUser.GetUserInfo(item.UserId);
                    if (userInfo != null)
                    {
                        model.headimg = bll.GetImgUrl(userInfo.WXHeadimgurlLocal);
                    }
                }
                #endregion
                model.id = item.AutoId;
                model.title = item.ReviewTitle;
                model.content = item.ReviewContent;
                if (item.ReplyDateTiem != null)
                {
                    model.time = bll.GetTimeStamp((DateTime)item.ReplyDateTiem);
                }
                else
                {
                    model.time = bll.GetTimeStamp(item.InsertDate);
                }
                model.pv = item.Pv;
                model.sharecount = 0;
                model.praisecount = item.PraiseNum;
                model.type = "ask";
                apiResult.list.Add(model);

            }
            return Common.JSONHelper.ObjectToJson(apiResult);
        }

        /// <summary>
        /// 创建话题接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string addask(HttpContext context)
        {
            string title = context.Request["title"];//标题
            string content = context.Request["content"];//内容
            string toUserId = context.Request["touserid"];//导师用户名
            string isPublic=context.Request["ispublic"];//是否是公开的话题 0代表是公开
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
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
                    resp.errmsg = "不能向该导师咨询";
                    goto outoff;
                }
            }
            else
            {
                resp.errcode = 5;
                resp.errmsg = "导师用户名不能为空";
                goto outoff;
            }
            ReviewInfo model = new ReviewInfo();
            model.ReviewTitle = title;
            model.ReviewContent = content;
            model.UserId = CurrentUserInfo.UserID;
            model.ForeignkeyId = toUserId;
            model.InsertDate = DateTime.Now;
            model.WebsiteOwner = WebSiteOwner;
            model.ReviewType = "话题";
            model.ReplyDateTiem = DateTime.Now;
            if (!string.IsNullOrEmpty(isPublic))
            {
                model.ReviewPower =int.Parse(isPublic);
            }
            else
            {
                model.ReviewPower = 1;
            }
            if (bll.Add(model))
            {
                if (!context.Request.Url.Host.Equals("preview.comeoncloud.com.cn"))//测试域名不处理
                {
                    #region 加积分 提示

                    SystemNotice systemNotice = new SystemNotice();
                    systemNotice.InsertTime = DateTime.Now;
                    string displayName = string.IsNullOrEmpty(this.CurrentUserInfo.TrueName) ? this.CurrentUserInfo.WXNickname : this.CurrentUserInfo.TrueName;
                    systemNotice.Title = string.Format("{0} 向您提问新的话题：\"{1}\"", displayName, model.ReviewTitle);
                    systemNotice.NoticeType = (int)BLLSystemNotice.NoticeType.ReviewReminder;
                    systemNotice.Ncontent = model.ReviewContent;
                    systemNotice.WebsiteOwner = bll.WebsiteOwner;
                    systemNotice.UserId = model.UserId;
                    systemNotice.SendType = (int)BLLSystemNotice.SendType.Personal;
                    systemNotice.Receivers = toUserId;
                    systemNotice.SerialNum = bllSystemNotice.GetGUID(ZentCloud.BLLJIMP.TransacType.SendSystemNotice);
                    systemNotice.RedirectUrl = string.Format("http://{0}/WuBuHui/WordsQuestions/MyWXDiscussList.aspx",
                    System.Web.HttpContext.Current.Request.Url.Host);
                    bllSystemNotice.Add(systemNotice);

                    if (!string.IsNullOrEmpty(toUserId))
                    {

                        UserInfo tutorInfo = bllUser.GetUserInfo(toUserId);
                        if (tutorInfo != null)
                        {
                            //向导师发送模板消息
                            BLLWeixin.TMTaskNotification notificaiton = new BLLWeixin.TMTaskNotification();
                            notificaiton.Url = systemNotice.RedirectUrl;
                            notificaiton.First = "您好，您有新的待办任务";
                            notificaiton.Keyword1 = string.Format("{0} 向您提问新的话题：{1}", displayName, model.ReviewTitle);
                            notificaiton.Keyword2 = "待回复";
                            notificaiton.Remark = "点击查看";
                            bllWeixin.SendTemplateMessage(bllWeixin.GetAccessToken(systemNotice.WebsiteOwner), tutorInfo.WXOpenId, notificaiton);
                        }

                    }

                    //加积分 提示 
                    #endregion
                }

                //导师话题数增加
                TutorInfo tutor = bllUser.Get<TutorInfo>(string.Format(" UserId='{0}'",toUserId));
                tutor.HTNums++;
                bll.Update(tutor);
                resp.errcode = 0;
                resp.errmsg = "操作成功";
                goto outoff;
            }
            else
            {
                resp.errcode = 6;
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
        private string replyask(HttpContext context)
        {
            string id = context.Request["id"];
            string content = context.Request["content"];

            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
                goto outoff;
            }
            if (string.IsNullOrEmpty(id))
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
            ReviewInfo reviewInfo = bll.Get<ReviewInfo>(string.Format("AutoId={0}", id));
            if (reviewInfo == null)
            {
                resp.errcode = 4;
                resp.errmsg = "回复的问题不存在";
                goto outoff;
            }
            BLLJIMP.Model.TutorInfo tutorInfo = bllUser.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}'", this.CurrentUserInfo.UserID));

            if ((tutorInfo == null) && (!reviewInfo.UserId.Equals(CurrentUserInfo.UserID)))
            {
                resp.errcode = 6;
                resp.errmsg = "不能回复他人向导师咨询的问题，建议直接咨询导师";
                goto outoff;
            }

            //
            if (bllUser.GetCount<ReplyReviewInfo>(string.Format(" ReviewID={0} And UserId='{1}'", id, CurrentUserInfo.UserID)) == 0)
            {

                //导师回复非自己提问的问题
                if ((tutorInfo != null) && (!reviewInfo.UserId.Equals(tutorInfo.UserId)))
                {
                    //导师回复自己的问题加20分，回复别人的问题加10分。
                    BLLUserScore bllUserScore = new BLLUserScore(tutorInfo.UserId);
                    BLLUserScore.UserScoreType userScoreType = (reviewInfo.ForeignkeyId == tutorInfo.UserId) ? BLLUserScore.UserScoreType.TutorAnswerQuestionToHim : BLLUserScore.UserScoreType.TutorAnswerQuestionToOthers;
                    bllUserScore.UpdateUserScoreWithWXTMNotify(bllUserScore.GetDefinedUserScore(userScoreType), bllWeixin.GetAccessToken(bll.WebsiteOwner));


                }
                //给提问人加分
                if (!reviewInfo.UserId.Equals(CurrentUserInfo.UserID))
                {
                    BLLUserScore bllUserScore = new BLLUserScore(CurrentUserInfo.UserID);
                    BLLJIMP.Model.TutorInfo tutorInfoNew = bllUser.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}'", CurrentUserInfo.UserID));
                    BLLUserScore.UserScoreType userScoreType = (tutorInfoNew == null) ? BLLUserScore.UserScoreType.UserQuestionIsAnswered : BLLUserScore.UserScoreType.TutorQuestioinIsAnswered;
                    bllUserScore.UpdateUserScoreWithWXTMNotify(bllUserScore.GetDefinedUserScore(userScoreType), bllWeixin.GetAccessToken(bllUser.WebsiteOwner));

                }

            }
            //
            


                SystemNotice systemNotice = new SystemNotice();
                systemNotice.InsertTime = DateTime.Now;
                string displayName = string.IsNullOrEmpty(this.CurrentUserInfo.TrueName) ? this.CurrentUserInfo.WXNickname : this.CurrentUserInfo.TrueName;
                systemNotice.Title = string.Format("有人回复了您的话题\"{0}\"", reviewInfo.ReviewTitle);
                systemNotice.NoticeType = (int)BLLSystemNotice.NoticeType.ReviewReminder;
                systemNotice.Ncontent = reviewInfo.ReviewContent;
                systemNotice.WebsiteOwner = bll.WebsiteOwner;
                systemNotice.UserId = CurrentUserInfo.UserID;
                systemNotice.SendType = (int)BLLSystemNotice.SendType.Personal;

                systemNotice.SerialNum = bllSystemNotice.GetGUID(ZentCloud.BLLJIMP.TransacType.SendSystemNotice);
                systemNotice.RedirectUrl = string.Format("http://{0}/WuBuHui/WordsQuestions/WXDiscussInfo.aspx?AutoId={1}", System.Web.HttpContext.Current.Request.Url.Host, reviewInfo.AutoId);

                BLLWeixin.TMTaskNotification notificaiton = new BLLWeixin.TMTaskNotification();
                notificaiton.Url = systemNotice.RedirectUrl;
                notificaiton.First = "您好，您有新的待办任务";
                notificaiton.Keyword1 = string.Format("{0}回复了您的话题：\"{1}\"", displayName, reviewInfo.ReviewTitle);
                notificaiton.Keyword2 = "待回复";
                notificaiton.Remark = "点击查看";

                //提问者回复
                if ((tutorInfo == null) && (reviewInfo.UserId.Equals(CurrentUserInfo.UserID)))
                {
                    systemNotice.Receivers = reviewInfo.ForeignkeyId;
                }
                else  //导师回复
                {
                    systemNotice.Receivers = reviewInfo.UserId;
                }

                //向导师发系统消息
                bllSystemNotice.Add(systemNotice);
                //向提问者发送模板消息
                bllWeixin.SendTemplateMessage(bllWeixin.GetAccessToken(systemNotice.WebsiteOwner), systemNotice.Receivers, notificaiton);


            

            BLLJIMP.Model.ReplyReviewInfo replyInfo = new BLLJIMP.Model.ReplyReviewInfo()
            {
                ReviewID = Convert.ToInt32(id),
                InsertDate = DateTime.Now,
                ReplyContent = content,
                UserId = CurrentUserInfo.UserID,
                WebSiteOwner = WebSiteOwner,
                ReviewType = "话题"

            };
            if (bll.Add(replyInfo))
            {
                reviewInfo.NumCount++;
                reviewInfo.ReplyDateTiem = DateTime.Now;
                bll.Update(reviewInfo);
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
        /// 获取话题详情
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getaskdetail(HttpContext context)
        {
            string id = context.Request["id"];
            askinfo askinfo = new askinfo();
            if (string.IsNullOrEmpty(id))
            {
                resp.errcode = 1;
                resp.errmsg = "编号不能为空";
                goto outoff;
            }
            ReviewInfo reviewInfo = bll.Get<ReviewInfo>(string.Format("AutoId={0}", id));
            if (reviewInfo == null)
            {
                resp.errcode = 3;
                resp.errmsg = "问题不存在";
                goto outoff;
            }

            askinfo.id = reviewInfo.AutoId;
            askinfo.title = reviewInfo.ReviewTitle;
            askinfo.content = reviewInfo.ReviewContent;
            if (reviewInfo.ReplyDateTiem != null)
            {
                askinfo.time = bll.GetTimeStamp((DateTime)reviewInfo.ReplyDateTiem);
            }
            else
            {
                askinfo.time = bll.GetTimeStamp(reviewInfo.InsertDate);
            }
            askinfo.pv = reviewInfo.Pv;
            askinfo.sharecount = 0;//转发数
            if (IsMaster(reviewInfo.UserId, WebSiteOwner))//提问人是导师
            {
                TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", reviewInfo.UserId));
                if (masterInfo != null)
                {
                    askinfo.headimg = bll.GetImgUrl(masterInfo.TutorImg);
                    askinfo.ismaster = true;
                    askinfo.truename = masterInfo.TutorName;
                    askinfo.userid = masterInfo.UserId;
                }
            }
            else//提问人是普通用户
            {
                UserInfo userInfo = bllUser.GetUserInfo(reviewInfo.UserId);
                if (userInfo != null)
                {
                    askinfo.headimg = userInfo.WXHeadimgurlLocal;
                    askinfo.ismaster = false;
                    askinfo.truename = userInfo.TrueName;
                    if (string.IsNullOrEmpty(askinfo.truename))
                    {
                        askinfo.truename = userInfo.WXNickname;
                    }
                    askinfo.userid = userInfo.UserID;
                }
            }
            if (bll.IsLogin)
            {
                BLLJIMP.Model.ForwardingRecord frecord = bll.Get<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' AND RUserID='{1}' AND websiteOwner='{2}' AND TypeName='话题赞'", CurrentUserInfo.UserID, askinfo.id, bll.WebsiteOwner));
                if (frecord != null)
                {
                    askinfo.ispraise = true;
                }

            }
            askinfo.praisecount = reviewInfo.PraiseNum;
            reviewInfo.Pv++;
            bll.Update(reviewInfo);
        outoff:
            return Common.JSONHelper.ObjectToJson(askinfo);
        }


        /// <summary>
        /// 获取话题回复列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getaskreplylist(HttpContext context)
        {
            string id = context.Request["id"];
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            replylist replyModel = new replylist();
            if (string.IsNullOrEmpty(id))
            {
                resp.errcode = 1;
                resp.errmsg = "编号不能为空";
                goto outoff;
            }
            ReviewInfo reviewInfo = bll.Get<ReviewInfo>(string.Format("AutoId={0}", id));
            if (reviewInfo == null)
            {
                resp.errcode = 3;
                resp.errmsg = "问题不存在";
                goto outoff;
            }
            List<ReplyReviewInfo> sourceData = bll.GetLit<ReplyReviewInfo>(pageSize, pageIndex, string.Format(" ReviewID={0}", id), "AutoId DESC");
            replyModel.totalcount = bll.GetCount<ReplyReviewInfo>(string.Format(" ReviewID={0}", id));
            replyModel.list = new List<reply>();
            foreach (var item in sourceData)
            {
                reply model = new reply();
                model.content = item.ReplyContent;
                model.time = bll.GetTimeStamp(item.InsertDate);
                if (IsMaster(item.UserId, WebSiteOwner))//回复人是导师
                {
                    TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", item.UserId));
                    if (masterInfo != null)
                    {
                        model.headimg = bll.GetImgUrl(masterInfo.TutorImg);
                        model.ismaster = true;
                        model.truename = masterInfo.TutorName;
                        model.userid = masterInfo.UserId;
                    }
                }
                else//回复人是普通用户
                {
                    UserInfo userInfo = bllUser.GetUserInfo(item.UserId);
                    if (userInfo != null)
                    {
                        model.headimg = userInfo.WXHeadimgurl;
                        model.ismaster = false;
                        model.truename = userInfo.TrueName;
                        if (string.IsNullOrEmpty(model.truename))
                        {
                            model.truename = userInfo.WXNickname;
                        }
                        model.userid = userInfo.UserID;
                    }
                }
                replyModel.list.Add(model);

            }
        outoff:
            return Common.JSONHelper.ObjectToJson(replyModel);
        }
        #endregion


        /// <summary>
        /// 获取行业标签列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string gettradetagslist(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            tagapi apiResult = new tagapi();
            apiResult.list = new List<tag>();
            var sourceData = bll.GetLit<ArticleCategory>(pageSize, pageIndex, string.Format(" CategoryType='trade' And WebsiteOwner='{0}' ", WebSiteOwner));
            apiResult.totalcount = bll.GetCount<ArticleCategory>(string.Format(" WebsiteOwner='{0}' AND CategoryType='trade' ", WebSiteOwner));
            foreach (var item in sourceData)
            {
                tag model = new tag();
                model.tagid = item.AutoID;
                model.tagname = item.CategoryName;
                apiResult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 获取专业标签列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getprofessionaltagslist(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            tagapi apiResult = new tagapi();
            apiResult.list = new List<tag>();
            var sourceData = bll.GetLit<ArticleCategory>(pageSize, pageIndex, string.Format(" CategoryType='Professional' And WebsiteOwner='{0}' ", WebSiteOwner));
            apiResult.totalcount = bll.GetCount<ArticleCategory>(string.Format(" WebsiteOwner='{0}' AND CategoryType='Professional' ", WebSiteOwner));
            foreach (var item in sourceData)
            {
                tag model = new tag();
                model.tagid = item.AutoID;
                model.tagname = item.CategoryName;
                apiResult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string login(HttpContext context)
        {

            login apiResult = new login();
            string userId = context.Request["userid"];
            string pwd = context.Request["pwd"];
            string msg;
            UserInfo userInfo = new UserInfo();
            if (bllUser.Login(userId, pwd, out userInfo, out msg))
            {
                context.Session[SessionKey.UserID] = userInfo.UserID;
                apiResult.issuccess = true;
                apiResult.userid = userInfo.UserID;
                apiResult.headimg = bll.GetImgUrl(userInfo.WXHeadimgurlLocal);
            }
            else
            {
                apiResult.message = "用户名或密码错误";
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 检查登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string islogin(HttpContext context)
        {

            checklogin apiResult = new checklogin();
            if (bllUser.IsLogin)
            {
                apiResult.islogin = true;
                apiResult.userid = CurrentUserInfo.UserID;
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }


        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getcurrentuserinfo(HttpContext context)
        {
            userinfo apiResult = new userinfo();
            if (CurrentUserInfo != null)
            {
                apiResult.headimg = CurrentUserInfo.WXHeadimgurl;
                apiResult.truename = CurrentUserInfo.TrueName;
                apiResult.position = CurrentUserInfo.Postion;
                apiResult.company = CurrentUserInfo.Company;
                apiResult.totalscore = CurrentUserInfo.TotalScore;
                apiResult.phone = CurrentUserInfo.Phone;
                apiResult.email = CurrentUserInfo.Email;
                if (string.IsNullOrEmpty(apiResult.truename))
                {
                    apiResult.truename = CurrentUserInfo.WXNickname;
                }
                if (IsMaster(CurrentUserInfo.UserID, WebSiteOwner))
                {
                    TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", CurrentUserInfo.UserID));
                    apiResult.headimg = bll.GetImgUrl(masterInfo.TutorImg);
                    apiResult.truename = masterInfo.TutorName;
                    apiResult.position = masterInfo.Position;
                    apiResult.company = masterInfo.Company;
                    apiResult.totalscore = CurrentUserInfo.TotalScore;
                    apiResult.phone = CurrentUserInfo.Phone;
                    apiResult.email = CurrentUserInfo.Email;

                }
            }

            return Common.JSONHelper.ObjectToJson(apiResult);
        }



        /// <summary>
        /// 根据条件查询活动
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页取几条</param>
        /// <param name="sort">排序</param>
        /// <param name="cateId">分类id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="myActivity">是否是我报名活动false 否true 是</param>
        /// <param name="myActivity">是否查询我喜欢的活动false 否true 是</param>
        /// <param name="status">活动状态</param>
        /// <returns></returns>
        private string QueryActivitylist(int pageIndex, int pageSize, string sort = "", string cateId = "", string keyword = "", bool myActivity = false, bool isMyPraise = false,string status="",string isAll="")
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1 ");
            string orderBy = "IsHide ASC,Sort DESC";//默认排序
            switch (sort)
            {
                case "time":
                    orderBy = " ActivityStartDate DESC";
                    break;
                case "signcount ":
                    orderBy = " SignUpCount DESC";
                    break;
                default:
                    orderBy = " IsHide ASC,Sort DESC";
                    break;
            }

            sbWhere.AppendFormat(" And ArticleType='activity' AND IsDelete=0  AND WebsiteOwner='{0}'", WebSiteOwner);
            if (!string.IsNullOrEmpty(cateId))
            {
                sbWhere.AppendFormat(" And CategoryId={0}", cateId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sbWhere.AppendFormat(" And (ActivityName like'%{0}%' Or ActivityAddress like'%{0}%')", keyword);
            }
            if (myActivity)
            {
                sbWhere.AppendFormat(" And SignUpActivityID in(select ActivityID from ZCJ_ActivityDataInfo where UserId='{0}' And IsDelete=0)",CurrentUserInfo.UserID);
            }
            if (isMyPraise)
            {
                sbWhere.AppendFormat(" And JuActivityID in(select RUserID from ZCJ_ForwardingRecord where FUserID='{0}' And  websiteOwner='{1}' AND TypeName = '活动赞')", CurrentUserInfo.UserID, bll.WebsiteOwner);
            }
            if (!string.IsNullOrEmpty(status))
            {
              sbWhere.AppendFormat(" And IsHide={0}", status);
            }

            activityapi apiResult = new activityapi();
            apiResult.list = new List<activity>();
            apiResult.totalcount = bll.GetCount<JuActivityInfo>(sbWhere.ToString());


            var sourceData = bll.GetLit<JuActivityInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);

            if (!string.IsNullOrEmpty(isAll)&&(isAll.Equals("true")))
            {
                sourceData.Clear();
                sourceData = bll.GetList<JuActivityInfo>(string.Format("ArticleType='activity' AND IsDelete=0  AND WebsiteOwner='{0}' Order by IsHide ASC,ActivityStartDate ASC", WebSiteOwner));
                var listPro = sourceData.Where(p => p.IsHide == -1).ToList();//即将开始的活动
                var listStart = sourceData.Where(p => p.IsHide == 0).ToList();//正在进行中的活动
                var listStop = sourceData.Where(p => p.IsHide == 1).ToList();//已经停止的活动
                listStop = listStop.OrderByDescending(p => p.ActivityStartDate).ToList();
                List<ZentCloud.BLLJIMP.Model.JuActivityInfo> data = new List<BLLJIMP.Model.JuActivityInfo>();
                data.AddRange(listStart);
                data.AddRange(listPro);
                data.AddRange(listStop);
                sourceData.Clear();
                sourceData = data;

            }

            foreach (var item in sourceData)
            {
                activity model = new activity();
                model.activityid = item.JuActivityID;
                model.activityimage = bll.GetImgUrl(item.ThumbnailsPath);
                model.activityname = item.ActivityName;
                model.address = item.ActivityAddress;
                model.categoryname = item.CategoryName;
                model.pv = item.PV;
                model.signcount = item.SignUpTotalCount;
                model.digest = item.Summary;
                if (item.ActivityStartDate != null)
                {
                    model.time = bll.GetTimeStamp((DateTime)item.ActivityStartDate);
                }
                if ((item.MaxSignUpTotalCount > 0) && (item.SignUpTotalCount >= item.MaxSignUpTotalCount))
                {
                    model.status = 2;
                }
                if (item.IsHide == 1)
                {
                    model.status = 1;
                }
                if (item.IsHide == -1)
                {
                    model.status = 3;
                }
                model.score = item.ActivityIntegral;
                model.type = "activity";
                apiResult.list.Add(model);

            }
            if (!string.IsNullOrEmpty(isAll) && (isAll.Equals("true")))
            {

                var listPro = apiResult.list.Where(p => p.status ==3).ToList();//即将开始的活动
                var listStart = apiResult.list.Where(p => p.status == 0).ToList();//正在进行中的活动
                var listStop = apiResult.list.Where(p => p.status == 1).ToList();//已经停止的活动
                var listFull = apiResult.list.Where(p => p.status == 2).ToList();//已经满员的活动

                apiResult.list.Clear();
                apiResult.list.AddRange(listStart);
                apiResult.list.AddRange(listFull);
                apiResult.list.AddRange(listPro);
                apiResult.list.AddRange(listStop);
            
            }


            return Common.JSONHelper.ObjectToJson(apiResult);


        }

        /// <summary>
        /// 获取积分记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getscorerecordlist(HttpContext context)
        {

            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            scorerecordapi apiResult = new scorerecordapi();
            apiResult.list = new List<scorerecord>();
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" UserId='{0}' AND WebsiteOwner='{1}'", CurrentUserInfo.UserID, WebSiteOwner);
            apiResult.totalcount = bll.GetCount<BLLJIMP.Model.WBHScoreRecord>(sbWhere.ToString());
            apiResult.totalscore = (int)CurrentUserInfo.TotalScore;
            List<BLLJIMP.Model.WBHScoreRecord> SourceData = bll.GetLit<BLLJIMP.Model.WBHScoreRecord>(pageSize, pageIndex, sbWhere.ToString(), " AutoId Desc");
            foreach (var item in SourceData)
            {
                scorerecord model = new scorerecord();
                model.title = item.NameStr;
                model.score = item.ScoreNum;
                model.time = bll.GetTimeStamp(item.InsertDate);
                apiResult.list.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 获取我关注的人列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getmyattentionlist(HttpContext context)
        {

            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //我关注的人肯定是导师
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            attentionapi apiResult = new attentionapi();
            apiResult.list = new List<attentionuserinfo>();
            apiResult.totalcount = bll.GetCount<UserFollowChain>(string.Format(" FromUserId='{0}'", CurrentUserInfo.UserID));
            var sourceData = bll.GetLit<UserFollowChain>(pageSize, pageIndex, string.Format(" FromUserId='{0}'", CurrentUserInfo.UserID), " AutoId DESC");
            foreach (var item in sourceData)
            {
                TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", item.ToUserId));
                if (masterInfo != null)
                {
                    attentionuserinfo model = new attentionuserinfo();
                    model.headimg = bll.GetImgUrl(masterInfo.TutorImg);
                    model.userid = masterInfo.UserId;
                    model.truename = masterInfo.TutorName;
                    model.position = masterInfo.Position;
                    model.digest = masterInfo.Digest;
                    model.pv = masterInfo.Pv;
                    model.praisecount = masterInfo.TutorLikes;
                    model.ismaster = true;
                    model.isattention = true;
                    model.professionaltags = new List<string>();
                    foreach (var tagid in masterInfo.ProfessionalStr.Split(','))
                    {
                        if (!string.IsNullOrEmpty(tagid))
                        {
                            var tag = bll.Get<ArticleCategory>(string.Format(" AutoID={0}", tagid));
                            if (tag != null)
                            {
                                model.professionaltags.Add(tag.CategoryName);

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
        private string getmyfanslist(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            attentionapi apiResult = new attentionapi();
            apiResult.list = new List<attentionuserinfo>();
            var sourceData = bll.GetLit<UserFollowChain>(pageSize, pageIndex, string.Format(" ToUserId='{0}'", CurrentUserInfo.UserID), " AutoId DESC");
            apiResult.totalcount = bll.GetCount<UserFollowChain>(string.Format(" ToUserId='{0}'", CurrentUserInfo.UserID));
            foreach (var item in sourceData)
            {
                attentionuserinfo model = new attentionuserinfo();
                if (IsMaster(item.FromUserId, WebSiteOwner))//粉丝是导师
                {
                    TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", item.FromUserId));
                    //if (MasterInfo != null)
                    //{
                    model.headimg = bll.GetImgUrl(masterInfo.TutorImg);
                    model.userid = masterInfo.UserId;
                    model.truename = masterInfo.TutorName;
                    model.position = masterInfo.Position;
                    model.digest = masterInfo.Digest;
                    model.pv = masterInfo.Pv;
                    model.praisecount = masterInfo.TutorLikes;
                    model.ismaster = true;
                    if (bll.GetCount<UserFollowChain>(string.Format(" FromUserId='{0}' And ToUserId='{1}'", CurrentUserInfo.UserID, item.FromUserId)) > 0)
                    {
                        model.isattention = true;
                    }
                    model.professionaltags = new List<string>();
                    foreach (var tagid in masterInfo.ProfessionalStr.Split(','))
                    {
                        if (!string.IsNullOrEmpty(tagid))
                        {
                            var tag = bll.Get<ArticleCategory>(string.Format(" AutoID={0}", tagid));
                            if (tag != null)
                            {
                                model.professionaltags.Add(tag.CategoryName);

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
                        model.position = userInfo.Postion;
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
        private string getmyquestionnairelist(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            questionnaireapi apiResult = new questionnaireapi();
            apiResult.list = new List<questionnaire>();
            var sourceData = bll.GetLit<Questionnaire>(pageSize, pageIndex, string.Format(" QuestionnaireID in(select QuestionnaireID from ZCJ_QuestionnaireRecord where UserId='{0}')", CurrentUserInfo.UserID), " QuestionnaireID DESC");
            apiResult.totalcount = bll.GetCount<Questionnaire>(string.Format(" QuestionnaireID in(select QuestionnaireID from ZCJ_QuestionnaireRecord where UserId='{0}')", CurrentUserInfo.UserID));
            foreach (var item in sourceData)
            {
                questionnaire model = new questionnaire();
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
        private string getscoreranklist(HttpContext context)
        {
            scorerank apiResult = new scorerank();
            apiResult.list = new List<userinfo>();
            int pageSize = int.Parse(context.Request["pagesize"]);
            var sourceData = bll.GetLit<UserInfo>(pageSize, 1, string.Format(" WebsiteOwner='{0}'", WebSiteOwner), " TotalScore DESC,AutoID ASC");
            foreach (var item in sourceData)
            {
                userinfo model = new userinfo();
                if (IsMaster(item.UserID, WebSiteOwner))//是导师
                {
                    TutorInfo masterInfo = bll.Get<TutorInfo>(string.Format(" UserId='{0}'", item.UserID));
                    model.headimg = bll.GetImgUrl(masterInfo.TutorImg);
                    model.truename = masterInfo.TutorName;

                }
                else//普通用户
                {
                    model.headimg = bll.GetImgUrl(item.WXHeadimgurlLocal);
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
        /// 获取系统通知
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getsystemnoticelist(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            systemnoticeapi apiResult = new systemnoticeapi();
            int noticeTypeStr = int.Parse(context.Request["noticetype"]);
            switch (noticeTypeStr)
            {
                case 0://系统消息
                    noticeTypeStr = 1;
                    apiResult.totalcount = bllSystemNotice.GetUnReadMsgCount(CurrentUserInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.SystemMessage);
                    break;
                case 1://问卷消息
                    noticeTypeStr = 21;
                    apiResult.totalcount = bllSystemNotice.GetUnReadMsgCount(CurrentUserInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.QuestionaryReminder);
                    break;
                default:
                    noticeTypeStr = 1;
                    apiResult.totalcount = bllSystemNotice.GetUnReadMsgCount(CurrentUserInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.SystemMessage);
                    break;
            }
            BLLSystemNotice.NoticeType noticeType = (BLLSystemNotice.NoticeType)noticeTypeStr;
            List<SystemNotice> systemNoticeList = bllSystemNotice.GetUnReadMsgList(pageSize, pageIndex, CurrentUserInfo.UserID, noticeType);
            apiResult.list = new List<systemnotice>();
            foreach (var item in systemNoticeList)
            {

                systemnotice model = new systemnotice();
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
        private string setsystemnoticereaded(HttpContext context)
        {
            string id = context.Request["id"];
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
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
        private string getunredsystemnoticecount(HttpContext context)
        {
            int unReadCountSystemMsg = 0;//未读系统消息数量
            int unReadCountSystemQuestionary = 0;//未读问卷数量
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
                goto outoff;
            }
            //if (string.IsNullOrEmpty(noticetype))
            //{
            //    resp.errcode = 3;
            //    resp.errmsg = "通知类型不能为空";
            //    goto outoff;
            //}
            unReadCountSystemMsg = bllSystemNotice.GetUnReadMsgCount(CurrentUserInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.SystemMessage);
            unReadCountSystemQuestionary = bllSystemNotice.GetUnReadMsgCount(CurrentUserInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.QuestionaryReminder);
        outoff:
            return "{\"systemmsg\":" + unReadCountSystemMsg + ",\"questionary\":" + unReadCountSystemQuestionary + "}";

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
        /// 检查是否是导师
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool IsMaster(string userId, string websiteOwner)
        {
            if (bll.GetCount<TutorInfo>(string.Format(" UserId='{0}' And WebsiteOwner='{1}'", userId, websiteOwner)) > 0)
            {
                return true;
            }
            return false;

        }


        /// <summary>
        ///微信分享分享完成
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string weixinsharecomplete(HttpContext context)
        {
            string shareType = context.Request["sharetype"];
            string id = context.Request["id"];
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
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
            shareRecord.UserId = CurrentUserInfo.UserID;

            BLLJIMP.Model.WBHScoreRecord scoreRecord = new BLLJIMP.Model.WBHScoreRecord();
            scoreRecord.Nums = "b55";
            scoreRecord.InsertDate = DateTime.Now;
            scoreRecord.WebsiteOwner = WebSiteOwner;
            scoreRecord.UserId = CurrentUserInfo.UserID;
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
                case "4"://发送导师给朋友
                    scoreRecord.NameStr = "发送导师给朋友";
                    scoreRecord.ScoreNum = "+5";
                    addScore = 5;
                    break;
                case "5"://分享导师到朋友圈
                    scoreRecord.NameStr = "分享导师到朋友圈";
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
            CurrentUserInfo.TotalScore += addScore;
            if ((bllUser.Update(CurrentUserInfo, string.Format(" TotalScore={0}", CurrentUserInfo.TotalScore), string.Format(" AutoId={0}", CurrentUserInfo.AutoID)) > 0) && (bll.Add(shareRecord)) && (bll.Add(scoreRecord)))
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
        /// 注销登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string loginout(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
                goto outoff;
            }
            context.Session.Clear();
            resp.errmsg = "操作成功";
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 获取二维码登录所需要的图片及登录凭据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getqrcodeimg(HttpContext context)
        {
            qrcode respQr = new qrcode();
            QRCodeEncoder QRCodeEncoder = new QRCodeEncoder();
            QRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            QRCodeEncoder.QRCodeScale = 4;
            QRCodeEncoder.QRCodeVersion = 0;
            QRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            string strGuid = Guid.NewGuid().ToString();
            //MD5
            string accessToken = strGuid + new Random().Next(0, 9999999);
            accessToken = accessToken.Replace("-", "").ToUpper();
            //MD5
            String data = string.Format("http://{0}/serv/step5api.ashx?action=qrcodelogin&accesstoken={1}", context.Request.Url.Host, accessToken);
            QrCodeLoginMap model = new QrCodeLoginMap();
            model.AccessToken = accessToken;
            model.InsertDate = DateTime.Now;
            if (bll.Add(model))
            {
                System.Drawing.Bitmap image = QRCodeEncoder.Encode(data);
                string fileName = string.Format("{0}.jpg", strGuid);
                string qrCodeImgUrlRel = string.Format("/FileUpload/QRCode/{0}", fileName);
                image.Save(context.Server.MapPath(qrCodeImgUrlRel));
                respQr.access_token = accessToken;
                respQr.imgurl = string.Format("http://{0}{1}", context.Request.Url.Host, qrCodeImgUrlRel);
                return Common.JSONHelper.ObjectToJson(respQr);
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "生成凭据失败";
                return Common.JSONHelper.ObjectToJson(resp);
            }




        }
        /// <summary>
        /// 微信二维码登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string qrcodelogin(HttpContext context)
        {
            string accessToken = context.Request["accesstoken"];
            if (string.IsNullOrEmpty(accessToken))
            {
                resp.errcode = 1;
                resp.errmsg = "凭据不能为空";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            QrCodeLoginMap model = bll.Get<QrCodeLoginMap>(string.Format(" AccessToken='{0}'", accessToken));
            if (model == null)
            {
                resp.errcode = 2;
                resp.errmsg = "凭据不存在";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if ((DateTime.Now - model.InsertDate).TotalSeconds >= 120)//有效期为2分钟
            {
                if (bll.IsWeiXinBrowser)
                {
                    context.Response.Redirect("/qrloginexpire.html");
                }
                else
  
                {
                resp.errcode = 3;
                resp.errmsg = "凭据已过期,请刷新电脑页面重新扫描";
                return Common.JSONHelper.ObjectToJson(resp);

                }
            }
            if (bll.IsWeiXinBrowser)//用户用微信访问本地址,设置当前用户的用户名到 QrCodeLoginMap
            {
                if (bll.IsLogin)
                {
                    model.UserId = CurrentUserInfo.UserID;
                    bll.Update(model);
                    if (CheckUserIsReg(CurrentUserInfo))
                    {

                        context.Response.Redirect("/WuBuHui/MyCenter/Index.aspx");


                    }
                    else//未注册跳转到登录页
                    {
                        context.Response.Redirect("/WuBuHui/Member/Registration.aspx");
                    }

                }



            }
            else//电脑访问 检查对应的TOKEN 对应的用户名
            {

                if (!string.IsNullOrEmpty(model.UserId))
                {
                    UserInfo userInfo = bllUser.GetUserInfo(model.UserId);
                    if (CheckUserIsReg(userInfo))
                    {
                        context.Session[SessionKey.UserID] = model.UserId;
                        resp.errmsg = "登录成功";
                        return Common.JSONHelper.ObjectToJson(resp);


                    }
                    else
                    {
                        resp.errcode = 5;
                        resp.errmsg = "请先注册";
                        return Common.JSONHelper.ObjectToJson(resp);
                    }



                }


            }
            resp.errcode = 4;
            resp.errmsg = "等待手机扫描";
            return Common.JSONHelper.ObjectToJson(resp);






        }

        /// <summary>
        /// 点赞 或取消点赞
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string adddelpraise(HttpContext context)
        {

            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
                goto outoff;
            }
            string type = context.Request["type"];
            string id = context.Request["id"];
            switch (type)
            {
                case "0"://资讯
                    #region 资讯点赞或取消赞
                    //
                    BLLJIMP.Model.ForwardingRecord forwadRecord = bll.Get<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' AND RUserID='{1}' AND websiteOwner='{2}' AND TypeName = '文章赞'", this.CurrentUserInfo.UserID, id, bll.WebsiteOwner));


                    BLLJIMP.Model.JuActivityInfo juInfo = bll.Get<BLLJIMP.Model.JuActivityInfo>(string.Format(" JuActivityID={0}", id));
                    if (forwadRecord == null)
                    {
                        forwadRecord = new BLLJIMP.Model.ForwardingRecord()
                        {
                            FUserID = this.CurrentUserInfo.UserID,
                            FuserName = this.CurrentUserInfo.LoginName,
                            RUserID = juInfo.JuActivityID.ToString(),
                            RUserName = juInfo.ActivityName,
                            RdateTime = DateTime.Now,
                            WebsiteOwner = bll.WebsiteOwner,
                            TypeName = "文章赞"
                        };

                        bll.Add(forwadRecord);
                        juInfo.UpCount = juInfo.UpCount + 1;
                        if (bll.Update(juInfo))
                        {
                            resp.errcode = 0;
                            resp.errmsg = "操作成功";
                        }
                        else
                        {
                            resp.errcode = 1;
                            resp.errmsg = "操作失败";
                        }
                    }
                    else
                    {
                        int count = bll.Delete(forwadRecord);
                        juInfo.UpCount = juInfo.UpCount - 1;
                        bool Bo = bll.Update(juInfo);
                        if (Bo && count > 0)
                        {
                            resp.errcode = 0;
                            resp.errmsg = "操作成功";

                        }
                        else
                        {
                            resp.errcode = 1;
                            resp.errmsg = "操作失败";
                        }
                    }
                    #endregion
                    break;
                case "1"://话题
                    #region 话题 点赞或取消赞
                    BLLJIMP.Model.ForwardingRecord forwadRecordAsk = bll.Get<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' AND RUserID='{1}' AND websiteOwner='{2}' AND TypeName = '话题赞'", this.CurrentUserInfo.UserID, id, bll.WebsiteOwner));
                    BLLJIMP.Model.ReviewInfo review = bll.Get<BLLJIMP.Model.ReviewInfo>(string.Format(" AutoId={0}", id));
                    if (forwadRecordAsk == null)
                    {
                        forwadRecordAsk = new BLLJIMP.Model.ForwardingRecord()
                        {
                            FUserID = this.CurrentUserInfo.UserID,
                            FuserName = this.CurrentUserInfo.LoginName,
                            RUserID = review.AutoId.ToString(),
                            RUserName = review.ReviewTitle,
                            RdateTime = DateTime.Now,
                            WebsiteOwner = bll.WebsiteOwner,
                            TypeName = "话题赞"
                        };

                        review.PraiseNum = review.PraiseNum + 1;
                        bll.Update(review);
                        if (bll.Add(forwadRecordAsk))
                        {
                            resp.errcode = 0;
                            resp.errmsg = "操作成功";
                        }
                        else
                        {
                            resp.errcode = 1;
                            resp.errmsg = "操作失败";
                        }
                    }
                    else
                    {
                        int count = bll.Delete(forwadRecordAsk);
                        review.PraiseNum = review.PraiseNum - 1;
                        bool isSuccess = bll.Update(review);
                        if (isSuccess && count > 0)
                        {
                            resp.errcode = 0;
                            resp.errmsg = "操作成功";
                        }
                        else
                        {
                            resp.errcode = 1;
                            resp.errmsg = "操作失败";
                        }
                    }
                    #endregion
                    break;
                case "2"://活动
                    #region 活动点赞或取消赞
                    //
                    BLLJIMP.Model.ForwardingRecord forwadRecordAcitvity = bll.Get<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' AND RUserID='{1}' AND websiteOwner='{2}' AND TypeName = '活动赞'", this.CurrentUserInfo.UserID, id, bll.WebsiteOwner));
                    BLLJIMP.Model.JuActivityInfo juInfoActivity = bll.Get<BLLJIMP.Model.JuActivityInfo>(string.Format(" JuActivityID={0}", id));
                    if (forwadRecordAcitvity == null)
                    {
                        forwadRecordAcitvity = new BLLJIMP.Model.ForwardingRecord()
                        {
                            FUserID = this.CurrentUserInfo.UserID,
                            FuserName = this.CurrentUserInfo.LoginName,
                            RUserID = juInfoActivity.JuActivityID.ToString(),
                            RUserName = juInfoActivity.ActivityName,
                            RdateTime = DateTime.Now,
                            WebsiteOwner = bll.WebsiteOwner,
                            TypeName = "活动赞"
                        };

                        bll.Add(forwadRecordAcitvity);
                        juInfoActivity.UpCount = juInfoActivity.UpCount + 1;
                        if (bll.Update(juInfoActivity))
                        {
                            resp.errcode = 0;
                            resp.errmsg = "操作成功";
                        }
                        else
                        {
                            resp.errcode = 1;
                            resp.errmsg = "操作失败";
                        }
                    }
                    else
                    {
                        int count = bll.Delete(forwadRecordAcitvity);
                        juInfoActivity.UpCount = juInfoActivity.UpCount - 1;
                        bool isSuccess = bll.Update(juInfoActivity);
                        if (isSuccess && count > 0)
                        {
                            resp.errcode = 0;
                            resp.errmsg = "操作成功";

                        }
                        else
                        {
                            resp.errcode = 1;
                            resp.errmsg = "操作失败";
                        }
                    }
                    #endregion
                    break;
                default:
                    resp.errcode = 1;
                    resp.errmsg = "未定义的类型";
                    break;
            }

        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 更新当前用户资料
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string updatecurrentuserinfo(HttpContext context)
        {

            if (!bll.IsLogin)
            {
                resp.errcode = (int)errcode.UnLogin;
                resp.errmsg = "请先登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            string trueName = context.Request["turename"];
            string phone = context.Request["phone"];
            string email = context.Request["email"];
            string company = context.Request["company"];
            string position = context.Request["position"];
            CurrentUserInfo.TrueName = trueName;
            CurrentUserInfo.Phone = phone;
            CurrentUserInfo.Email = email;
            CurrentUserInfo.Company = company;
            CurrentUserInfo.Postion = position;
            if (bll.Update(CurrentUserInfo, string.Format(" TrueName='{0}',Phone='{1}',Email='{2}',Company='{3}',Postion='{4}'", CurrentUserInfo.TrueName, CurrentUserInfo.Phone, CurrentUserInfo.Email, CurrentUserInfo.Company, CurrentUserInfo.Postion), string.Format(" AutoID={0}", CurrentUserInfo.AutoID)) > 0)
            {
                resp.errcode = 0;
                resp.errmsg = "操作成功";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "操作失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 检查用户是否已经注册
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private bool CheckUserIsReg(UserInfo userInfo)
        {

            if ((!string.IsNullOrEmpty(userInfo.TrueName)) && (!string.IsNullOrEmpty(userInfo.Phone)) && (!string.IsNullOrEmpty(userInfo.Company)) && (!string.IsNullOrEmpty(userInfo.Email)))
            {


                return true;


            }
            return false;
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
        public class DefaultResponse
        {

            /// <summary>
            /// 错误码
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }

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