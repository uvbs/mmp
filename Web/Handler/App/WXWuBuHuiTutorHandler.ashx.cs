using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using System.Reflection;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// WXWuBuHuiTutorHandler 的摘要说明
    /// </summary>
    public class WXWuBuHuiTutorHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 默认响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse(); // 统一回复相应数据
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity();  //活动数据
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();  //用户数据
        /// <summary>
        /// 基类 BLL
        /// </summary>
        BLL bll = new BLL();
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLWeixin bllWeixin = new BLLWeixin("");
        /// <summary>
        /// 系统通知
        /// </summary>
        BLLSystemNotice bllSystemNotice = new BLLSystemNotice();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo; //当前登陆的用户
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                string action = context.Request["Action"];
                this.currentUserInfo = bll.GetCurrentUserInfo();
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.Status = 0;
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
        /// 保存文章赞
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveWZPraise(HttpContext context)
        {
            string autoId = context.Request["JId"];

            BLLJIMP.Model.ForwardingRecord forwadRecord = bllJuactivity.Get<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' AND RUserID='{1}' AND websiteOwner='{2}' AND TypeName = '文章赞'", this.currentUserInfo.UserID, autoId, bll.WebsiteOwner));
            BLLJIMP.Model.JuActivityInfo juInfo = bllJuactivity.Get<BLLJIMP.Model.JuActivityInfo>(string.Format(" JuActivityID={0}", autoId));
            if (forwadRecord == null)
            {
                forwadRecord = new BLLJIMP.Model.ForwardingRecord()
                {
                    FUserID = this.currentUserInfo.UserID,
                    FuserName = this.currentUserInfo.LoginName,
                    RUserID = juInfo.JuActivityID.ToString(),
                    RUserName = juInfo.ActivityName,
                    RdateTime = DateTime.Now,
                    WebsiteOwner = bll.WebsiteOwner,
                    TypeName = "文章赞"
                };

                bool isSuccess = bllJuactivity.Add(forwadRecord);
                juInfo.UpCount = juInfo.UpCount + 1;
                isSuccess = bllJuactivity.Update(juInfo);
                if (isSuccess)
                {
                    resp.Status = 0;
                    resp.ExInt = juInfo.UpCount;
                    resp.ExStr = "1";
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "系统错误，请联系管理员";
                }
            }
            else
            {
                int count = bllJuactivity.Delete(forwadRecord);
                juInfo.UpCount = juInfo.UpCount - 1;
                bool isSuccess = bllJuactivity.Update(juInfo);
                if (isSuccess && count > 0)
                {
                    resp.Status = 0;
                    resp.ExInt = juInfo.UpCount;
                    resp.ExStr = "0";

                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "系统错误，请联系管理员";
                }
            }


            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 获取导师文章列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetNewInfos(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            string userId = context.Request["UserId"];
            int totalCount = 0;
            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}' And ArticleType='article' And IsHide=0 And IsDelete=0 And UserId='{1}'", bllUser.WebsiteOwner, userId));
            totalCount = bllUser.GetCount<ZentCloud.BLLJIMP.Model.JuActivityInfo>(sbWhere.ToString());
            List<ZentCloud.BLLJIMP.Model.JuActivityInfo> data = bllUser.GetLit<ZentCloud.BLLJIMP.Model.JuActivityInfo>(pageSize, pageIndex, sbWhere.ToString(), " JuActivityID DESC");
            resp.ExObj = data;
            resp.ExStr = "";
            int totalPage = bllUser.GetTotalPage(totalCount, pageSize);
            if ((totalPage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 保存顶
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SavePraise(HttpContext context)
        {
            string autoId = context.Request["AutoID"];
            if (string.IsNullOrEmpty(autoId))
            {
                resp.Msg = "系统出错，请联系管理员";
                resp.Status = -1;
            }

            BLLJIMP.Model.ForwardingRecord forwadRecord = bllJuactivity.Get<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' AND RUserID='{1}' AND websiteOwner='{2}' AND TypeName = '话题赞'", this.currentUserInfo.UserID, autoId, bll.WebsiteOwner));
            BLLJIMP.Model.ReviewInfo review = bllJuactivity.Get<BLLJIMP.Model.ReviewInfo>(string.Format(" AutoId={0}", autoId));
            if (forwadRecord == null)
            {
                forwadRecord = new BLLJIMP.Model.ForwardingRecord()
                {
                    FUserID = this.currentUserInfo.UserID,
                    FuserName = this.currentUserInfo.LoginName,
                    RUserID = review.AutoId.ToString(),
                    RUserName = review.ReviewTitle,
                    RdateTime = DateTime.Now,
                    WebsiteOwner = bll.WebsiteOwner,
                    TypeName = "话题赞"
                };

                review.PraiseNum = review.PraiseNum + 1;
                bllJuactivity.Update(review);
                if (bllJuactivity.Add(forwadRecord))
                {
                    resp.Status = 0;
                    resp.ExInt = review.PraiseNum;
                    resp.ExStr = "1";
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "系统错误，请联系管理员";
                }
            }
            else
            {
                int count = bllJuactivity.Delete(forwadRecord);
                review.PraiseNum = review.PraiseNum - 1;
                bool isSuccess = bllJuactivity.Update(review);
                if (isSuccess && count > 0)
                {
                    resp.Status = 0;
                    resp.ExInt = review.PraiseNum;
                    resp.ExStr = "0";
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "系统错误，请联系管理员";
                }
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 保存踩
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveStep(HttpContext context)
        {
            string autoId = context.Request["AutoID"];

            BLLJIMP.Model.ReviewInfo review = bllJuactivity.Get<BLLJIMP.Model.ReviewInfo>(string.Format(" AutoId={0}", autoId));
            review.StepNum = review.StepNum + 1;

            if (bllJuactivity.Update(review))
            {
                resp.Status = 0;
                resp.ExInt = review.StepNum;
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "系统错误，请联系管理员";
            }


            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取回复信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetReplyReviewInfo(HttpContext context)
        {
            string autoId = context.Request["AutoID"];
            int pageIndex=int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            List<BLLJIMP.Model.ReplyReviewInfo> dataList = bllJuactivity.GetLit<BLLJIMP.Model.ReplyReviewInfo>(pageSize, pageIndex, string.Format(" ReviewID={0}", autoId), " AutoId DESC");
            //if (dataList != null)
            //{
                foreach (BLLJIMP.Model.ReplyReviewInfo item in dataList)
                {
                    BLLJIMP.Model.UserInfo userInfo = bllUser.GetUserInfo(item.UserId);
                    item.HTNum = bllJuactivity.GetCount<BLLJIMP.Model.ReviewInfo>(string.Format(" ForeignkeyId='{0}' AND websiteOwner='{1}'", item.UserId, bll.WebsiteOwner));
                    if (userInfo != null)
                    {


                        //var tutorInfo = bllUser.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}'", item.UserId));
                        //if (tutorInfo != null)
                        //{
                        //    item.Img = tutorInfo.TutorImg;
                        //    item.NickName = tutorInfo.TutorName;
                        //    item.UserName = tutorInfo.TutorName;
                        //    item.IsTutor = 1;
                        //}
                        //else
                        //{
                        item.Img = bllUser.GetUserDispalyAvatar(userInfo);
                            item.NickName = userInfo.WXNickname;
                            item.UserName = userInfo.TrueName;
                        //}
                    }

                    //item.UserLevel = bllUserScore.GetUserLevelByTotalScore(userInfo.HistoryTotalScore);

                }
                resp.Status = 0;
                resp.ExObj = dataList;
            //}

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 回复话题
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveReplyReviewInfo(HttpContext context)
        {
            int autoId = int.Parse(context.Request["AutoID"]);
            string content = context.Request["Context"];
            //string userId = context.Request["UserId"];
            ReviewInfo reviewInfo = bllJuactivity.Get<ReviewInfo>(string.Format("AutoId={0}", autoId));
            //BLLJIMP.Model.TutorInfo tinfo = bllJuactivity.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}'", this.currentUserInfo.UserID));

            //if ((tinfo == null) && (!reviewInfo.UserId.Equals(currentUserInfo.UserID)))
            //{
            //    resp.Status = -1;
            //    resp.Msg = "不能回复他人向导师咨询的问题，建议直接咨询导师";
            //    return Common.JSONHelper.ObjectToJson(resp);
            //}

            //
            //if (bllUser.GetCount<ReplyReviewInfo>(string.Format(" ReviewID={0} And UserId='{1}'", autoId, currentUserInfo.UserID)) == 0)
            //{

            //    //导师回复非自己提问的问题
            //    if ((tinfo != null) && (!reviewInfo.UserId.Equals(tinfo.UserId)))
            //    {
            //        //导师回复自己的问题加20分，回复别人的问题加10分。
            //        BLLUserScore bllUserScore = new BLLUserScore(tinfo.UserId);
            //        BLLUserScore.UserScoreType UserScoreType = (reviewInfo.ForeignkeyId == tinfo.UserId) ? BLLUserScore.UserScoreType.TutorAnswerQuestionToHim : BLLUserScore.UserScoreType.TutorAnswerQuestionToOthers;
            //        bllUserScore.UpdateUserScoreWithWXTMNotify(bllUserScore.GetDefinedUserScore(UserScoreType), bllWeixin.GetAccessToken(currWebSiteUserInfo.UserID));
            //        //SavaUserToTalScol(this.userInfo.UserID, ReviewInfo.ForeignkeyId == Tinfo.UserId ? 20 : 10);

            //    }

            //    ////给提问人加分
            //    //if (!reviewInfo.UserId.Equals(currentUserInfo.UserID))
            //    //{
            //    //    BLLUserScore bllUserScore = new BLLUserScore(userId);
            //    //    BLLJIMP.Model.TutorInfo TotorInfo = bllJuactivity.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}'", userId));
            //    //    BLLUserScore.UserScoreType UserScoreType = (TotorInfo == null) ? BLLUserScore.UserScoreType.UserQuestionIsAnswered : BLLUserScore.UserScoreType.TutorQuestioinIsAnswered;
            //    //    bllUserScore.UpdateUserScoreWithWXTMNotify(bllUserScore.GetDefinedUserScore(UserScoreType), bllWeixin.GetAccessToken(currWebSiteUserInfo.UserID));
            //    //    //SaveReplyUser(UserId);
            //    //}

            //}
            //
            BLLJIMP.Model.ReplyReviewInfo replyReview = new BLLJIMP.Model.ReplyReviewInfo()
            {
                ReviewID = Convert.ToInt32(autoId),
                InsertDate = DateTime.Now,
                ReplyContent = content,
                UserId = this.currentUserInfo.UserID,
                UserName = this.currentUserInfo.LoginName,
                PraentId = 0,
                WebSiteOwner=bll.WebsiteOwner

            };
            bool isSuccess = bllJuactivity.Add(replyReview);
            if (isSuccess)
            {
                //BLLJIMP.Model.ReviewInfo review = bllJuactivity.Get<BLLJIMP.Model.ReviewInfo>(string.Format(" AutoId={0}", autoId));
                reviewInfo.NumCount++;
                reviewInfo.ReplyDateTiem = DateTime.Now;
                #region Old
                //if (tinfo != null)
                //{
                //    tinfo.ReviewDateTime = DateTime.Now;
                //    bllJuactivity.Update(tinfo);

                //} 
                #endregion
                if (bllJuactivity.Update(reviewInfo))
                {
                    #region OLD
                    ////服务号消息提醒提问者
                    //BLLJIMP.Model.UserInfo askfrom = juActivityBll.Get<BLLJIMP.Model.UserInfo>(string.Format("UserId = '{0}'", rInfo.UserId));
                    //string accesstoken = bllweixin.GetAccessToken(currWebSiteUserInfo.UserID);
                    //if (accesstoken != string.Empty)
                    //{
                    //    string message = string.Format("您的话题获得导师回复啦,赶紧进入五步会主页查看啦！");
                    //    bllweixin.SendKeFuMessageText(accesstoken, askfrom.WXOpenId, message.ToString());
                    //}


                    ////系统消息提示

                    //SystemNotice SystemNotice = new SystemNotice();
                    //SystemNotice.InsertTime = DateTime.Now;
                    //string displayName = string.IsNullOrEmpty(this.currentUserInfo.TrueName) ? this.currentUserInfo.WXNickname : this.currentUserInfo.TrueName;
                    //SystemNotice.Title = string.Format("有人回复了您的话题\"{0}\"", reviewInfo.ReviewTitle);
                    //SystemNotice.NoticeType = (int)BLLSystemNotice.NoticeType.ReviewReminder;
                    //SystemNotice.Ncontent = reviewInfo.ReviewContent;
                    //SystemNotice.WebsiteOwner = bll.WebsiteOwner;
                    //SystemNotice.UserId = currentUserInfo.UserID;
                    //SystemNotice.SendType = (int)BLLSystemNotice.SendType.Personal;

                    //SystemNotice.SerialNum = bllSystemNotice.GetGUID(TransacType.SendSystemNotice);
                    //SystemNotice.RedirectUrl = string.Format("http://{0}/WuBuHui/WordsQuestions/WXDiscussInfo.aspx?AutoId={1}", System.Web.HttpContext.Current.Request.Url.Host, reviewInfo.AutoId);

                    //BLLWeixin.TMTaskNotification notificaiton = new BLLWeixin.TMTaskNotification();
                    //notificaiton.Url = SystemNotice.RedirectUrl;
                    //notificaiton.First = "您好，您有新的待办任务";
                    //notificaiton.Keyword1 = string.Format("{0}回复了您的话题：\"{1}\"", displayName, review.ReviewTitle);
                    //notificaiton.Keyword2 = "待回复";
                    //notificaiton.Remark = "点击查看";

                    ////提问者回复
                    //if ((tinfo == null) && (reviewInfo.UserId.Equals(currentUserInfo.UserID)))
                    //{
                    //    SystemNotice.Receivers = reviewInfo.ForeignkeyId;
                    //}
                    //else  //导师回复
                    //{
                    //    SystemNotice.Receivers = reviewInfo.UserId;
                    //}

                    ////向导师发系统消息
                    //bllSystemNotice.Add(SystemNotice);
                    ////向提问者发送模板消息
                    //bllWeixin.SendTemplateMessage(bllWeixin.GetAccessToken(SystemNotice.WebsiteOwner), SystemNotice.Receivers, notificaiton); 
                    #endregion

                    //给回复者加分
                    int replyCount = bll.GetCount<ReplyReviewInfo>(string.Format("ReviewID={0} And  UserId='{1}'",autoId,currentUserInfo.UserID));
                    if (replyCount<=1)//第一次回答才得分
                    {

                        bllUser.AddUserScoreDetail(bllUser.GetCurrUserID(), CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.AnswerQuestions), this.bllUser.WebsiteOwner, null, null);

                    }
                    resp.Status = 0;
                    resp.Msg = "回复成功";
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "系统出错，请联系管理员";
                }
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "系统出错，请联系管理员";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        ///// <summary>
        ///// 给提问人加分
        ///// </summary>
        ///// <param name="UserId"></param>
        //private void SaveReplyUser(string UserId)
        //{
        //    BLLJIMP.Model.TutorInfo tInfo = juActivityBll.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}'", UserId));

        //    //导师提问被回复加40分，用户加20分
        //    int score = (tInfo ==null) ? 20 : 40;
        //    SavaUserToTalScol(UserId, score);
        //}

        /// <summary>
        /// 获取话题列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetReviewInfoList(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            string reviewTitle = context.Request["Title"];
            string type = context.Request["type"];
            string my = context.Request["UserId"];
            string haveReply = context.Request["HaveReply"];
            string sort = context.Request["Sort"];
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbOrderBy = new StringBuilder();
            sbWhere.AppendFormat(" ReviewType='话题' AND websiteOwner='{0}'", bllUser.WebsiteOwner);
            if (!string.IsNullOrEmpty(reviewTitle))
            {
                sbWhere.AppendFormat(" AND ReviewTitle like '%{0}%'", reviewTitle);
            }
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" AND CategoryType LIKE '%{0}%'", type);
            }
            if (my.Equals("my"))
            {
                if (bllJuactivity.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}'", this.currentUserInfo.UserID)) != null)
                {
                    sbWhere.AppendFormat(" AND (ForeignkeyId='{0}' Or UserId='{0}' Or AutoId in(select ReviewID from ZCJ_ReplyReviewInfo where UserId='{0}'))", this.currentUserInfo.UserID);
                }
                else
                {
                    sbWhere.AppendFormat(" AND UserId='{0}'", this.currentUserInfo.UserID);
                }
                //sb.AppendFormat(" AND (ForeignkeyId='{0}' Or UserId='{0}')", this.userInfo.UserID);

            }
            else
            {
                sbWhere.Append(" AND ReviewPower=0");
                //sbWhere.AppendFormat(" AND NumCount>0");
            }

            if (!string.IsNullOrEmpty(context.Request["MyAttention"]))//我关注的话题
            {
               // pageSize = 100;
                List<ZentCloud.BLLJIMP.Model.UserFollowChain> followChainList = bllUser.GetList<ZentCloud.BLLJIMP.Model.UserFollowChain>(string.Format("FromUserId='{0}'", currentUserInfo.UserID));
                string userIds = "";
                if (followChainList.Count > 0)
                {

                    foreach (var item in followChainList)
                    {
                        userIds += string.Format("'{0}',", item.ToUserId);
                    }
                    userIds = userIds.TrimEnd(',');
                    sbWhere.AppendFormat(" AND (ForeignkeyId in({0}) Or UserId in({0}) Or AutoId in(select ReviewID from ZCJ_ReplyReviewInfo where UserId in({0}) ) )", userIds);

                }
                else
                {
                    sbWhere.AppendFormat(" And 1=0 ");
                }
            }
            if (!string.IsNullOrEmpty(haveReply))
            {
                sbWhere.Append(" AND NumCount>0");

            }
            sbOrderBy.Append(" AutoId DESC ");

            if (sort.Equals("Newhf"))
            {
                sbOrderBy.Clear();
                sbOrderBy.Append(" ReplyDateTiem DESC");
            }
            if (sort.Equals("Mosthf"))
            {
                sbOrderBy.Clear();
                sbOrderBy.Append(" NumCount DESC");
            }

            if (sort.Equals("Mosthp"))
            {
                sbOrderBy.Clear();
                sbOrderBy.Append(" PraiseNum DESC, ReplyDateTiem DESC");
            }


            List<BLLJIMP.Model.ReviewInfo> data = bllJuactivity.GetLit<BLLJIMP.Model.ReviewInfo>(pageSize, pageIndex, sbWhere.ToString(), sbOrderBy.ToString());

            if (data != null && data.Count > 0)
            {
                //foreach (BLLJIMP.Model.ReviewInfo item in rInfos)
                //{
                //    if (!string.IsNullOrEmpty(item.CategoryType))
                //    {
                //        item.actegory = juActivityBll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format(" CategoryType='word' AND AutoID IN ({0}) AND WebsiteOwner='{1}'", item.CategoryType.Substring(0, item.CategoryType.Length - 1), userBll.WebsiteOwner));
                //    }
                //    if (string.IsNullOrEmpty(my))
                //    {
                //        List<BLLJIMP.Model.ReplyReviewInfo> list = juActivityBll.GetList<BLLJIMP.Model.ReplyReviewInfo>(string.Format(" ReviewID={0}", item.AutoId));
                //        if (list == null)
                //        {
                //            rInfos.Remove(item);
                //        }
                //    }
                //}
                resp.Status = 0;
                resp.ExObj = data;
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "没有数据";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取导师话题
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetReviewInfos(HttpContext context)
        {
            string pageIndex = context.Request["PageIndex"];
            string pageSize = context.Request["PageSize"];
            string userId = context.Request["UserId"];
            if (string.IsNullOrEmpty(userId))
            {
                resp.Status = -1;
                resp.Msg = "系统错误，请联系管理员";
                goto OutF;
            }
            List<BLLJIMP.Model.ReviewInfo> data = bllJuactivity.GetLit<BLLJIMP.Model.ReviewInfo>(Convert.ToInt32(pageSize), Convert.ToInt32(pageIndex), string.Format(" ForeignkeyId='{0}' AND websiteOwner='{1}'", userId, bll.WebsiteOwner), " InsertDate DESC");
            if (data != null && data.Count > 0)
            {
                foreach (BLLJIMP.Model.ReviewInfo item in data)
                {
                    if (!string.IsNullOrEmpty(item.CategoryType))
                    {
                        item.actegory = bllJuactivity.GetList<BLLJIMP.Model.ArticleCategory>(string.Format(" AutoId in ({0})", item.CategoryType.Substring(0, item.CategoryType.Length - 1)));
                    }

                }
                resp.Status = 0;
                resp.ExObj = data;
            }
            else
            {
                resp.Status = -1;
            }

        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取话题分类标签
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetACategoryWords(HttpContext context)
        {

            BLLJIMP.Model.ArticleCategory data = bllJuactivity.Get<BLLJIMP.Model.ArticleCategory>(string.Format("  WebsiteOwner='{0}' AND CategoryType='word'", bll.WebsiteOwner));
            if (data != null)
            {
                resp.Status = 0;
                resp.ExObj = data;
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "没有数据";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 保存话题信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveReviewInfo(HttpContext context)
        {
            string userId = context.Request["UserId"];
            string title = context.Request["Title"];
            string reviewContent = context.Request["ReviewContent"];
            string power = context.Request["Power"];
            string categoryType = context.Request["CategoryType"];
            if (userId.Equals(this.currentUserInfo.UserID))
            {
                resp.Status = -1;
                resp.Msg = "导师不能提问自己";
                goto Outf;
            }
            if (string.IsNullOrEmpty(title))
            {
                resp.Status = -1;
                resp.Msg = "话题标题不能为空";
                goto Outf;

            }
            if (string.IsNullOrEmpty(reviewContent))
            {
                resp.Status = -1;
                resp.Msg = "话题内容不能为空";
                goto Outf;
            }





            BLLJIMP.Model.TutorInfo tutorInfo = bllJuactivity.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}'", userId));
            if (bllUser.GetCount<ReviewInfo>(string.Format("ForeignkeyId='{0}' And UserId='{1}' And ReviewTitle='{2}'", tutorInfo.UserId, currentUserInfo.UserID, title)) > 0)
            {
                resp.Status = -1;
                resp.Msg = "重复提交";
                goto Outf;

            }
            DateTime dtNow = DateTime.Now;
            BLLJIMP.Model.ReviewInfo review = new BLLJIMP.Model.ReviewInfo
            {
                ForeignkeyId = tutorInfo.UserId,
                ForeignkeyName = tutorInfo.TutorName,
                UserId = this.currentUserInfo.UserID,
                UserName = this.currentUserInfo.TrueName,
                ReviewPower = Convert.ToInt32(power),
                InsertDate = dtNow,
                ReviewTitle = title,
                ReviewContent = reviewContent,
                WebsiteOwner = bll.WebsiteOwner,
                PraiseNum = 0,
                StepNum = 0,
                ReviewType = "话题",
                CategoryType = categoryType,
                ReplyDateTiem = dtNow
            };
            if (bllJuactivity.Add(review))
            {
                //SavaCurrUser();
                //SaveTutorUser(UserId);
                SaveHTNums(userId);
                resp.Status = 0;
                resp.Msg = "发表成功";

                //向导师发送提示信息
                WXQiyeConfig config = bllWeixin.Get<BLLJIMP.Model.WXQiyeConfig>(string.Format("WebsiteOwner='{0}'", bll.WebsiteOwner));
                if ((config != null) && (!string.IsNullOrEmpty(tutorInfo.WxQiyeUserId)) && (!string.IsNullOrEmpty(tutorInfo.UserId)))
                {
                    if ((!string.IsNullOrEmpty(config.AppId)) && (!string.IsNullOrEmpty(config.CorpID)) && (!string.IsNullOrEmpty(config.Secret)))
                    {
                        if (reviewContent.Length > 31)
                        {
                            reviewContent = reviewContent.Substring(0, 30) + "...";
                        }
                        string url = string.Format("http://{0}/WuBuHui/WordsQuestions/WXDiscussInfo.aspx?AutoId={1}", context.Request.Url.Host, bllUser.Get<BLLJIMP.Model.ReviewInfo>(string.Format(" ForeignkeyId='{0}' And websiteOwner='{1}' Order By AutoId DESC", tutorInfo.UserId, bll.WebsiteOwner)).AutoId);
                        string msg = string.Format("您有新的话题:\r\n{0}\r\n{1}\r\n{2}", title, reviewContent, url);
                        bllWeixin.SendMessageTextQiye(config.CorpID, config.Secret, config.AppId, tutorInfo.WxQiyeUserId, msg);
                    }
                }

                //系统消息提示
                SystemNotice systemNotice = new SystemNotice();
                systemNotice.InsertTime = dtNow;
                string displayName = string.IsNullOrEmpty(this.currentUserInfo.TrueName) ? this.currentUserInfo.WXNickname : this.currentUserInfo.TrueName;
                systemNotice.Title = string.Format("{0} 向您提问新的话题：\"{1}\"", displayName, review.ReviewTitle);
                systemNotice.NoticeType = (int)BLLSystemNotice.NoticeType.ReviewReminder;
                systemNotice.Ncontent = review.ReviewContent;
                systemNotice.WebsiteOwner = bll.WebsiteOwner;
                systemNotice.UserId = review.UserId;
                systemNotice.SendType = (int)BLLSystemNotice.SendType.Personal;
                systemNotice.Receivers = tutorInfo.UserId;
                systemNotice.SerialNum = bllSystemNotice.GetGUID(TransacType.SendSystemNotice);
                systemNotice.RedirectUrl = string.Format("http://{0}/WuBuHui/WordsQuestions/MyWXDiscussList.aspx",
                System.Web.HttpContext.Current.Request.Url.Host);
                bllSystemNotice.Add(systemNotice);

                if (!string.IsNullOrEmpty(tutorInfo.UserId))
                {

                    UserInfo tuserInfo = bllUser.GetUserInfo(tutorInfo.UserId);
                    if (tuserInfo != null)
                    {
                        //向导师发送模板消息
                        BLLWeixin.TMTaskNotification notificaiton = new BLLWeixin.TMTaskNotification();
                        notificaiton.Url = systemNotice.RedirectUrl;
                        notificaiton.First = "您好，您有新的待办任务";
                        notificaiton.Keyword1 = string.Format("{0} 向您提问新的话题：{1}", displayName, review.ReviewTitle);
                        notificaiton.Keyword2 = "待回复";
                        notificaiton.Remark = "点击查看";
                        var result = bllWeixin.SendTemplateMessage(bllWeixin.GetAccessToken(), tuserInfo.WXOpenId, notificaiton);
                    }

                }

            }
            else
            {
                resp.Status = -1;
                resp.Msg = "发表失败";
            }

        Outf:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 话题数
        /// </summary>
        /// <param name="UserId"></param>
        private void SaveHTNums(string UserId)
        {
            BLLJIMP.Model.TutorInfo model = bllJuactivity.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}'", UserId));
            model.HTNums = model.HTNums + 1;
            bllJuactivity.Update(model);
        }

        ///// <summary>
        ///// 导师保存积分
        ///// </summary>
        ///// <param name="UserId"></param>
        //private void SaveTutorUser(string UserId)
        //{
        //    SavaUserToTalScol(UserId, 20);
        //    BLLJIMP.Model.WBHScoreRecord srInfo = new BLLJIMP.Model.WBHScoreRecord()
        //    {
        //        NameStr = "导师获得新问题加分",
        //        Nums = "34",
        //        InsertDate = DateTime.Now,
        //        ScoreNum = "+20",
        //        WebsiteOwner = this.websiteOwner,
        //        UserId = this.userInfo.UserID,
        //        RecordType = "2"
        //    };
        //    juActivityBll.Add(srInfo);
        //}

        ///// <summary>
        ///// 保存当前用户信息
        ///// </summary>
        //private void SavaCurrUser()
        //{
        //    SavaUserToTalScol(this.userInfo.UserID, 10);
        //    BLLJIMP.Model.WBHScoreRecord srInfo = new BLLJIMP.Model.WBHScoreRecord()
        //    {
        //        NameStr = "提问加分",
        //        Nums = "34",
        //        InsertDate = DateTime.Now,
        //        ScoreNum = "+10",
        //        WebsiteOwner = this.websiteOwner,
        //        UserId = this.userInfo.UserID,
        //        RecordType = "2"

        //    };
        //    juActivityBll.Add(srInfo);
        //}


        /// <summary>
        /// 保存积分
        /// </summary>
        /// <param name="p"></param>
        //private bool SavaUserToTalScol(string UserID, int s)
        //{
        //    try
        //    {
        //        BLLJIMP.Model.UserInfo uinfo = juActivityBll.Get<BLLJIMP.Model.UserInfo>(string.Format(" UserId='{0}'", UserID));
        //        uinfo.TotalScore = uinfo.TotalScore + s;

        //        ZentCloud.ZCBLLEngine.BLLTransaction bllTransaction = new ZentCloud.ZCBLLEngine.BLLTransaction();
        //        if (!juActivityBll.Update(uinfo)) //更改用户积分
        //        {
        //            bllTransaction.Rollback();
        //            return false;
        //        }

        //        if (!SaveScoreRecord(UserID, s))
        //        {
        //            bllTransaction.Rollback();
        //            return false;
        //        }
        //        bllTransaction.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //private bool SaveScoreRecord(string userID, int s)
        //{
        //    BLLJIMP.Model.WBHScoreRecord srInfo = new BLLJIMP.Model.WBHScoreRecord()
        //    {
        //        NameStr = "回答问题加分",
        //        Nums = "34",
        //        InsertDate = DateTime.Now,
        //        ScoreNum = string.Format("+{0}", s),
        //        WebsiteOwner = this.websiteOwner,
        //        UserId = userID,
        //        RecordType = "2"

        //    };
        //    return juActivityBll.Add(srInfo);
        //}

        /// <summary>
        /// 获得行业分类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetTradeInfos(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.ArticleCategory> data;
            string categoryName = context.Request["CategoryName"];
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" Activityid='{0}'", ActivityId));
            StringBuilder sbWhere = new StringBuilder(string.Format(" websiteOwner='{0}' AND CategoryType in ('Trade','Professional','word','Partner')", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(categoryName))
            {
                sbWhere.AppendFormat(" AND CategoryName lIKE '%{0}%'", categoryName);
            }
            totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.ArticleCategory>(sbWhere.ToString());
            data = this.bllJuactivity.GetLit<BLLJIMP.Model.ArticleCategory>(pageSize, pageIndex, sbWhere.ToString(), " CategoryType ASC");

            return Common.JSONHelper.ObjectToJson(
                new
                {
                    total = totalCount,
                    rows = data
                });
        }

        /// <summary>
        /// 添加修改行业分类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UTradeInfo(HttpContext context)
        {
            string autoId = context.Request["AutoId"];
            string categoryName = context.Request["cName"];
            string categoryType = context.Request["CategoryType"];
            BLLJIMP.Model.ArticleCategory articleCategory = bllJuactivity.Get<BLLJIMP.Model.ArticleCategory>(string.Format(" Autoid={0} AND WebsiteOwner='{1}'", autoId, bll.WebsiteOwner));
            if (articleCategory == null)
            {
                articleCategory = new BLLJIMP.Model.ArticleCategory()
                {
                    CategoryName = categoryName,
                    CategoryType = categoryType,
                    WebsiteOwner = bll.WebsiteOwner
                };
                bool isSuccess = bllJuactivity.Add(articleCategory);
                if (isSuccess)
                {
                    resp.Status = 0;
                    resp.Msg = "添加成功";
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "添加失败";
                }

            }
            else
            {
                articleCategory.CategoryName = categoryName;
                articleCategory.CategoryType = categoryType;
                if (bllJuactivity.Update(articleCategory))
                {
                    resp.Status = 0;
                    resp.Msg = "修改成功";
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "修改失败";
                }
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        private string DelTradeInfo(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请至少选择一条！！！";
                goto Outf;
            }
            if (bllJuactivity.Delete(new BLLJIMP.Model.ArticleCategory(), string.Format(" AutoId in ({0})", ids)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "删除成功。";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "删除失败。";
            }

        Outf:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取导师列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetTutorInfos(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.TutorInfo> data;
            string name = context.Request["Name"];
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" Activityid='{0}'", ActivityId));
            StringBuilder sbWhere = new StringBuilder(string.Format(" websiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(name))
            {
                sbWhere.AppendFormat(" AND TutorName lIKE '%{0}%'", name);
            }
            totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.TutorInfo>(sbWhere.ToString());
            data = this.bllJuactivity.GetLit<BLLJIMP.Model.TutorInfo>(pageSize, pageIndex, sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = data
    });
        }

        /// <summary>
        /// 删除导师列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelTutorInfos(HttpContext context)
        {
            string ids = context.Request["ids"];

            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请至少选择一条！！！";
                goto Outf;
            }
            if (bllJuactivity.Delete(new BLLJIMP.Model.TutorInfo(), string.Format(" AutoId in ({0})", ids)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "删除成功。";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "删除失败。";
            }

        Outf:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 添加修改导师信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddTutorInfo(HttpContext context)
        {
            string autoId = context.Request["AutoId"];
            string tutorName = context.Request["TutorName"];
            string tutorImg = context.Request["TutorImg"];
            string tutorExplain = context.Request["TutorExplain"];
            string tradeStr = context.Request["TradeStr"];
            string professionalStr = context.Request["ProfessionalStr"];
            string userId = context.Request["UserId"];
            string company = context.Request["Company"];
            string position = context.Request["Position"];
            string signature = context.Request["Signature"];
            string wxQiyeUserId = context.Request["WxQiyeUserId"];
            string city = context.Request["City"];

            if (string.IsNullOrEmpty(autoId))
            {
                autoId = "0";
            }

            BLLJIMP.Model.TutorInfo model = bllJuactivity.Get<BLLJIMP.Model.TutorInfo>(string.Format(" AutoId={0}", autoId));
            if (model != null)
            {
                model.TutorName = tutorName;
                model.TutorImg = tutorImg;
                model.TutorExplain = tutorExplain;
                model.TradeStr = tradeStr;
                model.ProfessionalStr = professionalStr;
                model.RDataTime = DateTime.Now;
                model.UserId = userId;
                model.Company = company;
                model.Position = position;
                model.Signature = signature;
                model.WxQiyeUserId = wxQiyeUserId;
                model.City = city;
                if (bllJuactivity.Update(model))
                {
                    resp.Status = 1;
                    resp.Msg = "保存成功";
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "修改失败";
                }
            }
            else
            {
                model = new BLLJIMP.Model.TutorInfo()
                {
                    TutorName = tutorName,
                    TutorImg = tutorImg,
                    TutorExplain = tutorExplain,
                    TradeStr = tradeStr,
                    ProfessionalStr = professionalStr,
                    RDataTime = DateTime.Now,
                    UserId = userId,
                    websiteOwner = bll.WebsiteOwner,
                    Company = company,
                    Position = position,
                    Signature = signature,
                    WxQiyeUserId = wxQiyeUserId,
                    City = city

                };
                if (bllJuactivity.Add(model))
                {
                    resp.Status = 1;
                    resp.Msg = "添加成功";
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "添加失败";
                }

            }



            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 福布斯 添加修改理财师信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddEditMasterInfo(HttpContext context)
        {
            string autoId = context.Request["AutoId"];
            string tutorName = context.Request["TutorName"];
            string tutorImg = context.Request["TutorImg"];
            string tutorExplain = context.Request["TutorExplain"];
            string professionalStr = context.Request["ProfessionalStr"];//标签
            string userId = context.Request["UserId"];
            string company = context.Request["Company"];
            string position = context.Request["Position"];
            string digest = context.Request["Digest"];
            string city = context.Request["City"];
            int number = int.Parse(context.Request["Number"]);
            string gender = context.Request["Gender"];
            int age = int.Parse(context.Request["Age"]);
            string bank = context.Request["Bank"];
            string companyType = context.Request["CompanyType"];
            int workYear = int.Parse(context.Request["WorkYear"]);
            int masterWorkYear = int.Parse(context.Request["MasterWorkYear"]);
            string education = context.Request["Education"];
            string email = context.Request["Email"];
            string area = context.Request["Area"];
            int rank = int.Parse(context.Request["Rank"]);


            if (string.IsNullOrEmpty(autoId))
            {
                autoId = "0";
            }
            if (!string.IsNullOrEmpty(userId))
            {
                //if (userBll.GetUserInfo(UserId) == null)
                //{
                //resp.Msg = "用户名不存在,请检查";
                ///goto outoff;
                // }
            }
            BLLJIMP.Model.TutorInfo model = bllJuactivity.Get<BLLJIMP.Model.TutorInfo>(string.Format(" AutoId={0}", autoId));
            if (model != null)
            {
                model.TutorName = tutorName;
                model.TutorImg = tutorImg;
                model.TutorExplain = tutorExplain;
                model.ProfessionalStr = professionalStr;
                model.RDataTime = DateTime.Now;
                model.UserId = userId;
                model.Company = company;
                model.Position = position;
                model.Digest = digest;
                model.City = city;
                model.Number = number;

                model.Gender = gender;
                model.Age = age;
                model.Bank = bank;
                model.CompanyType = companyType;
                model.WorkYear = workYear;
                model.MasterWorkYear = masterWorkYear;
                model.Education = education;
                model.Email = email;
                model.Area = area;
                model.Rank = rank;
                bool isSuccess = bllJuactivity.Update(model);
                if (isSuccess)
                {
                    resp.Status = 1;
                    resp.Msg = "保存成功";
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "修改失败";
                }
            }
            else
            {
                model = new BLLJIMP.Model.TutorInfo()
                {
                    TutorName = tutorName,
                    TutorImg = tutorImg,
                    TutorExplain = tutorExplain,
                    ProfessionalStr = professionalStr,
                    RDataTime = DateTime.Now,
                    UserId = userId,
                    websiteOwner = bll.WebsiteOwner,
                    Company = company,
                    Position = position,
                    Digest = digest,
                    City = city,
                    Number = number,
                    Gender = gender,
                    Age = age,
                    Bank = bank,
                    CompanyType = companyType,
                    WorkYear = workYear,
                    MasterWorkYear = masterWorkYear,
                    Education = education,
                    Email = email,
                    Area = area,
                    Rank = rank

                };
                if (bllJuactivity.Add(model))
                {
                    resp.Status = 1;
                    resp.Msg = "添加成功";
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "添加失败";
                }

            }


            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取导师
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetTutorInfo(HttpContext context)
        {
            string autoId = context.Request["AutoId"];
            BLLJIMP.Model.TutorInfo model = bllJuactivity.Get<BLLJIMP.Model.TutorInfo>(string.Format(" AutoId={0}", autoId));
            if (model != null)
            {
                resp.Status = 0;
                resp.ExObj = model;
            }

            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 保存到记录表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SavaLike(HttpContext context)
        {
            string userId = context.Request["UserID"];
            if (string.IsNullOrEmpty(userId))
            {
                resp.Status = -1;
                resp.Msg = "系统错误，请联系管理员";
                goto OutF;
            }
            BLLJIMP.Model.ForwardingRecord forWardRecord = bllJuactivity.Get<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' AND RUserID='{1}' AND websiteOwner='{2}' AND  TypeName = '赞导师' ", this.currentUserInfo.UserID, userId, bll.WebsiteOwner));
            BLLJIMP.Model.TutorInfo tutorInfo = bllJuactivity.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}' AND websiteOwner='{1}'", userId, bll.WebsiteOwner));
            if (forWardRecord == null)
            {
                forWardRecord = new BLLJIMP.Model.ForwardingRecord()
                {
                    FUserID = this.currentUserInfo.UserID,
                    FuserName = this.currentUserInfo.LoginName,
                    RUserID = tutorInfo.UserId,
                    RUserName = tutorInfo.TutorName,
                    RdateTime = DateTime.Now,
                    WebsiteOwner = bll.WebsiteOwner,
                    TypeName = "赞导师"
                };
                bool isSuccess = bllJuactivity.Add(forWardRecord);
                tutorInfo.TutorLikes = tutorInfo.TutorLikes + 1;
                isSuccess = bllJuactivity.Update(tutorInfo);
                if (isSuccess)
                {
                    resp.Msg = "点赞";
                    resp.Status = 0;
                    resp.ExInt = tutorInfo.TutorLikes;
                    resp.ExStr = "1";
                }
                else
                {
                    resp.Msg = "系统出错,请联系管理员";
                    resp.Status = -1;
                }
            }
            else
            {
                int count = bllJuactivity.Delete(forWardRecord);
                tutorInfo.TutorLikes = tutorInfo.TutorLikes - 1;
                bool isSuccess = bllJuactivity.Update(tutorInfo);
                if (isSuccess && count > 0)
                {
                    resp.Status = 1;
                    resp.ExInt = tutorInfo.TutorLikes;
                    resp.Msg = "取消赞";
                    resp.ExStr = "0";
                }
                else
                {
                    resp.Msg = "系统出错,请联系管理员";
                    resp.Status = -1;
                }
            }

        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 添加关注
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddFollowChain(HttpContext context)
        {
            string toUserId = context.Request["toUserId"];
            int isFollow = int.Parse(context.Request["isFollow"]);
            string fromUserId = currentUserInfo.UserID;
            bool isSuccess = false;
            if (isFollow == 1)//关注
            {
                isSuccess = bllUser.Follow(fromUserId, toUserId);
            }
            else//取消关注
            {
                isSuccess = bllUser.CancelFollow(fromUserId, toUserId);
            }

            resp.Msg = isSuccess ? "操作成功！" : "操作失败";
            resp.Status = isSuccess ? 0 : -1;
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetFanAttentionList(HttpContext context)
        {
            string userId = context.Request["UserId"];
            string fromOrTo = context.Request["FromOrTo"];
            int pageIndex = Convert.ToInt32(context.Request["PageIndex"]);
            int pageSize = Convert.ToInt32(context.Request["PageSize"]);
            StringBuilder sbWhere = new StringBuilder("");
            if (fromOrTo.ToLower().Equals("from"))
            {
                sbWhere.AppendFormat("FromUserId='{0}'", userId);
            }
            else if (fromOrTo.ToLower().Equals("to"))
            {
                sbWhere.AppendFormat("ToUserId='{0}'", userId);
            }
            List<ZentCloud.BLLJIMP.Model.UserFollowChain> data = bllUser.GetLit<ZentCloud.BLLJIMP.Model.UserFollowChain>(pageSize, pageIndex, sbWhere.ToString(), "AutoId DESC");
            if (data.Count > 0)
            {
                resp.Status = 1;
            }
            resp.ExObj = data;
            return Common.JSONHelper.ObjectToJson(resp);




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
