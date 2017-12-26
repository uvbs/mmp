using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
using System.Data;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.BLLJIMP
{
    public class BLLSystemNotice : BLL
    {
        public string Notice;
        public string Site;

        public ReturnValue SendSystemMessage(string title, string ncontent, NoticeType noticeType, SendType sendType,
           string receivers, string redirectUrl)
        {
            return SendSystemMessage(title, ncontent, (int)noticeType, (int)sendType, receivers, redirectUrl, WebsiteOwner);
        }

        public ReturnValue SendSystemMessage(string title, string ncontent, int noticeType, int sendType,
            string receivers,string redirectUrl, string websiteOwner)
        {
            SystemNotice systemNotice = new SystemNotice();
            systemNotice.SerialNum = GetGUID(TransacType.SendSystemNotice);
            systemNotice.Title = title;
            systemNotice.Ncontent = ncontent;
            systemNotice.NoticeType = noticeType;
            systemNotice.RedirectUrl = redirectUrl;
            systemNotice.InsertTime = DateTime.Now;
            systemNotice.WebsiteOwner = websiteOwner;
            systemNotice.SendType = sendType;
            systemNotice.Receivers = receivers;

            ReturnValue  rc;
            rc.Code = 0;
            rc.Msg = string.Empty;
            switch ((SendType)sendType)
            {
                case SendType.All:
                    rc = SendSystemNoticeToAll(systemNotice, websiteOwner);
                    break;
                case SendType.Group:
                    rc = SendSystemNoticeToGroups(systemNotice, receivers, websiteOwner);
                    break;
                case SendType.Personal:
                    rc = SendSystemNoticeToPersonals(systemNotice, receivers);
                    break;
                default:
                    throw new Exception("未定义的发送类型");
            }
            return rc;
        }

        private ReturnValue SendSystemNoticeToAll(SystemNotice systemNotice, string websiteOwner)
        {

            BLLJIMP.BLLWeixin bllWeixin = new BLLWeixin("");
            BLLWeixin.TMTaskNotification notificaiton = new BLLWeixin.TMTaskNotification();
            notificaiton.Url = string.Format("http://{0}/WuBuHui/MyCenter/SystemMessageBox.aspx", System.Web.HttpContext.Current.Request.Url.Host);
            var acctoken = bllWeixin.GetAccessToken();

            List<UserInfo> userList = new List<UserInfo>();
            if (WebsiteOwner.Equals("wubuhui"))
            {
                userList = this.GetList<UserInfo>(string.Format("WebsiteOwner='{0}' and Phone is not null and Phone <>''", websiteOwner));
            }
            else
            {
                userList = this.GetList<UserInfo>(string.Format("WebsiteOwner='{0}'", websiteOwner));
            }
            int successCount = 0;
            foreach (UserInfo uinfo in userList)
            {
                successCount += SendSystemNoticeToUserId(systemNotice, uinfo.UserID) ? 1 : 0;
                try
                {
                    notificaiton.First = "您好，您有新系统消息";
                    notificaiton.Keyword1 = systemNotice.Title;
                    notificaiton.Keyword2 = systemNotice.NoticeTypeString;
                    notificaiton.Remark = "点击查看";
                    bllWeixin.SendTemplateMessage(acctoken, uinfo.WXOpenId, notificaiton);

                }
                catch (Exception)
                {

                    continue;
                }

            }

            return new ReturnValue { Code = 0, Msg = string.Format("成功将消息发送到{0}个人.", successCount) };
        }

        private ReturnValue SendSystemNoticeToGroups(SystemNotice systemNotice, string groups, string websiteOwner)
        {

            BLLJIMP.BLLWeixin bllWeixin = new BLLWeixin("");
            BLLWeixin.TMTaskNotification notificaiton = new BLLWeixin.TMTaskNotification();
            notificaiton.Url = string.Format("http://{0}/WuBuHui/MyCenter/SystemMessageBox.aspx", System.Web.HttpContext.Current.Request.Url.Host);
            var acctoken = bllWeixin.GetAccessToken();

            string[] groupArray = groups.Split(',');
            string strWhere = string.Format("TagName like '%{0}%'", groupArray[0].Trim());
            foreach (string group in groupArray )
            {
                strWhere += string.Format(" or TagName like '%{0}%'", group);
            }
            List<UserInfo> userList = this.GetList<UserInfo>(string.Format("WebsiteOwner='{0}' and ({1})", websiteOwner, strWhere));
            int successCount = 0;
            foreach (UserInfo uinfo in userList)
            {
                successCount += SendSystemNoticeToUserId(systemNotice, uinfo.UserID) ? 1 : 0;

                try
                {
                    notificaiton.First = "您好，您有新系统消息";
                    notificaiton.Keyword1 = systemNotice.Title;
                    notificaiton.Keyword2 = systemNotice.NoticeTypeString;
                    notificaiton.Remark = "点击查看";
                    bllWeixin.SendTemplateMessage(acctoken, uinfo.WXOpenId, notificaiton);

                }
                catch (Exception)
                {

                    continue;
                }

            }
            return new ReturnValue { Code = 0, Msg = string.Format("成功将消息发送到{0}个人.", successCount) };
        }

        private ReturnValue SendSystemNoticeToPersonals(SystemNotice systemNotice, string personals)
        {
            BLLJIMP.BLLUser bllUser=new BLLUser("");
            BLLJIMP.BLLWeixin bllWeixin = new BLLWeixin("");
            BLLWeixin.TMTaskNotification notificaiton = new BLLWeixin.TMTaskNotification();
            notificaiton.Url = string.Format("http://{0}/WuBuHui/MyCenter/SystemMessageBox.aspx", System.Web.HttpContext.Current.Request.Url.Host);
            var accessToken = bllWeixin.GetAccessToken();

            string[] userArray = personals.Split(',');
            int successCount = 0;
            foreach (string userId in userArray)
            {
                successCount += SendSystemNoticeToUserId(systemNotice, userId) ? 1 : 0;


                try
                {
                    notificaiton.First = "您好，您有新系统消息";
                    notificaiton.Keyword1 = systemNotice.Title;
                    notificaiton.Keyword2 = systemNotice.NoticeTypeString;
                    notificaiton.Remark = "点击查看";
                    bllWeixin.SendTemplateMessage(accessToken, bllUser.GetUserInfo(userId).WXOpenId, notificaiton);

                }
                catch (Exception)
                {
                    continue;
                }

            }
            return new ReturnValue { Code = 0, Msg = string.Format("成功将消息发送到{0}个人.", successCount) };
        }

        private bool SendSystemNoticeToUserId(SystemNotice systemNotice, string userId)
        {
            systemNotice.UserId = userId;
            systemNotice.Receivers = userId;
            if (this.GetCount<UserInfo>(string.Format("UserId='{0}'", systemNotice.UserId)) < 1)
            {
                return false;
            }
            return this.Add(systemNotice);
        }

        /// <summary>
        /// 用户所有消息列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="isRead"></param>
        /// <returns></returns>
        public List<SystemNotice> GetMsgList(int pageSize, int pageIndex, string userId, NoticeType? type, string keyword, bool? isRead, string websiteOwner="")
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" Receivers='{0}'", userId);
            if (type.HasValue && type != new NoticeType())
            {
                sbSql.AppendFormat(" and NoticeType={0}", (int)type.Value);
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sbSql.AppendFormat(" and Ncontent like '%{0}%'", keyword);
            }
            if (isRead.HasValue && isRead.Value)
            {
                sbSql.AppendFormat(" and ReadTime is not null");
            }
            else if (isRead.HasValue && !isRead.Value)
            {
                sbSql.AppendFormat(" and ReadTime is null");
            }
            if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                sbSql.AppendFormat(" and WebsiteOwner = '{0}'", websiteOwner);
            }
            return GetLit<SystemNotice>(pageSize, pageIndex, sbSql.ToString(), "AutoID DESC");
        }

        /// <summary>
        /// 用户所以消息数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetMsgCount(string userId, NoticeType? type)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" Receivers='{0}'", userId);
            if (type.HasValue && type != new NoticeType()) sbSql.AppendFormat(" and NoticeType={0}", (int)type.Value);
            return this.GetCount<SystemNotice>(sbSql.ToString());
        }

        /// <summary>
        /// 用户未读消息数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetUnReadMsgCount(string userId, NoticeType? type,string websiteOwner = "")
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" Receivers='{0}' and (ReadTime is null or ReadTime ='')", userId);
            if (type.HasValue && type != new NoticeType()) sbSql.AppendFormat(" and NoticeType={0}", (int)type.Value);
            if (string.IsNullOrWhiteSpace(websiteOwner)) sbSql.AppendFormat(" and WebsiteOwner='{0}'", websiteOwner);
            return this.GetCount<SystemNotice>(sbSql.ToString());
        }

        public DateTime GetUnReadMsgTime(string userId, NoticeType? type)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" Receivers='{0}' and (ReadTime is null or ReadTime ='')", userId);
            if (type.HasValue && type != new NoticeType()) sbSql.AppendFormat(" and NoticeType={0}", (int)type.Value);

            List<SystemNotice> listNotice = this.GetList<SystemNotice>(1, sbSql.ToString(), 
                        string.Format("InsertTime DESC"));
            if (listNotice.Count > 0)
            {
                return listNotice[0].InsertTime;
            }
            else
            {
                return DateTime.Now;
            }
        }

        public List<SystemNotice> GetUnReadMsgList(int pageSize, int pageIndex, string userId, NoticeType? type)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" Receivers='{0}' and (ReadTime is null or ReadTime ='')", userId);
            if (type.HasValue && type != new NoticeType()) sbSql.AppendFormat(" and NoticeType={0}", (int)type.Value);

            return this.GetLit<SystemNotice>(pageSize, pageIndex, sbSql.ToString(), "AutoID Desc"); 
        }

        public void SetReaded(int autoId)
        {
            DateTime dtn = DateTime.Now;
            BLL.ExecuteSql(string.Format(@"update ZCJ_SystemNotice set Readtime = '{0}' where AutoId = {1}", dtn, autoId));
        }
        public void SetReaded(string autoIds)
        {
            DateTime dtn = DateTime.Now;
            BLL.ExecuteSql(string.Format(@"update ZCJ_SystemNotice set Readtime = '{0}' where AutoId In ({1})", dtn, autoIds));
        }

        public void SetAllReaded(string userId, NoticeType? type)
        {
            DateTime dtn = DateTime.Now;
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" Receivers='{0}' and (ReadTime is null or ReadTime ='')", userId);
            if (type.HasValue && type != new NoticeType()) sbSql.AppendFormat(" and NoticeType={0}", (int)type.Value);

            BLL.ExecuteSql(string.Format(@"update ZCJ_SystemNotice set Readtime = '{0}' where {1}", dtn, sbSql.ToString()));
        }

        public void SetAllReaded(string userId)
        {
            DateTime dtn = DateTime.Now;
            BLL.ExecuteSql(string.Format(@"update ZCJ_SystemNotice set Readtime = '{0}' where (Readtime is null or Readtime ='') 
                                         and UserId ='{1}'", dtn, userId));
        }
        /// <summary>
        /// 查看用户是否有未读消息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsHaveUnReadMessage(string userId) {
         return (GetUnReadMsgCount(userId, NoticeType.ReviewReminder) + GetUnReadMsgCount(userId, NoticeType.SystemMessage) + GetUnReadMsgCount(userId, NoticeType.QuestionaryReminder)) > 0 ? true : false;
        }

        public void SendNotice(NoticeType noticeType, UserInfo user, JuActivityInfo juActivity, string receivers, string content)
        { 
            BLLUser bllUser = new BLLUser();
            List<UserInfo> users = bllUser.GetUsers("UserID,WXNickname,TrueName", receivers);
            SendNotice(noticeType, user, juActivity, users, content);
        }

        public void SendNoticeToAllUser()
        {
            BLLUser bllUser = new BLLUser();
            List<UserInfo> userList = bllUser.GetAllUsers("UserID", Site);
            string noticeTypeName = GetNoticeTypeName(NoticeType.SystemMessage);
            string noticeContent = GetContentHtml(NoticeType.SystemMessage, null, null, Notice);
            foreach (var item in userList)
            {
                SystemNotice systemNotice = new SystemNotice();
                systemNotice.SerialNum = GetGUID(TransacType.SendSystemNotice);
                systemNotice.Title = noticeTypeName;
                systemNotice.Ncontent = noticeContent;
                systemNotice.NoticeType = (int)NoticeType.SystemMessage;
                systemNotice.InsertTime = DateTime.Now;
                systemNotice.WebsiteOwner = Site;
                systemNotice.SendType = (int)SendType.Personal;
                SendSystemNoticeToUserId(systemNotice, item.UserID);
            }
        }

        public void SendNotice(NoticeType noticeType, UserInfo user, JuActivityInfo juActivity, List<UserInfo> receivers, string content)
        {
            string noticeTypeName = GetNoticeTypeName(noticeType);
            string noticeContent = GetContentHtml(noticeType, user, juActivity, content);
            foreach (var item in receivers)
            {
                SendSystemMessage(noticeTypeName, noticeContent, noticeType, SendType.Personal, item.UserID, null);
            }
        }

        public string GetContentHtml(NoticeType noticeType, UserInfo user, JuActivityInfo juActivity, string content)
        {
            StringBuilder sbHtml = new StringBuilder();
            BLLUser bllUser = new BLLUser();
            BLLJuActivity bllJuActivity = new BLLJuActivity();

            if (noticeType == NoticeType.FollowUser)
            {
                sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  关注了您",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user));
            }
            else if (noticeType == NoticeType.DisFollowUser)
            {
                sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  取消了对您的关注",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user));
            }
            else if (noticeType == NoticeType.InviteAnswer)
            {
                sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  邀请您回答 ：<a href=\"{2}\">{3}</a>",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user),
                        GetArticleLink(juActivity.JuActivityID, WebsiteOwner, juActivity.ArticleType),
                        juActivity.ActivityName);
            }
            else if (noticeType == NoticeType.FollowArticle)
            {
                sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  关注了您的{2} ：<a href=\"{3}\">《{4}》</a>",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user),
                        GetArticleTypeName(juActivity.ArticleType, WebsiteOwner), 
                        GetArticleLink(juActivity.JuActivityID, WebsiteOwner, juActivity.ArticleType),
                        juActivity.ActivityName);
            }
            else if (noticeType == NoticeType.FollowQuestion)
            {
                sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  关注了您的{2} ：<a href=\"{3}\">{4}</a>",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user),
                        GetArticleTypeName(juActivity.ArticleType, WebsiteOwner), 
                        GetArticleLink(juActivity.JuActivityID, WebsiteOwner, juActivity.ArticleType),
                        juActivity.ActivityName);
            }
            else if (noticeType == NoticeType.QuestionIsAnswered)
            {
                sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  答复了您的{2} ：<a href=\"{3}\">{4}</a>",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user),
                        GetArticleTypeName(juActivity.ArticleType, WebsiteOwner), 
                        GetArticleLink(juActivity.JuActivityID, WebsiteOwner, juActivity.ArticleType),
                        juActivity.ActivityName);
                sbHtml.AppendFormat("<br /><br />回答：{0}" , content);
            }
            else if (noticeType == NoticeType.FollowQuestionIsAnswered)
            {
                sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  答复了您的关注的{2} ：<a href=\"{3}\">{4}</a>",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user),
                        GetArticleTypeName(juActivity.ArticleType, WebsiteOwner), 
                        GetArticleLink(juActivity.JuActivityID, WebsiteOwner, juActivity.ArticleType),
                        juActivity.ActivityName);
                sbHtml.AppendFormat("<br /><br />回答：{0}" , content);
            }
            else if (noticeType == NoticeType.FavoriteArticle)
            {
                if (juActivity.ArticleType == "Statuses")
                {
                    sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  收藏了了您的动态 ：{2}",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user), juActivity.ActivityDescription);
                }
                else
                {
                    string tt = juActivity.ArticleType == "Question" ? "问题" : "文章";
                    sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  收藏了了您的{2} ：<a href=\"{3}\">《{4}》</a>",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user),
                        GetArticleTypeName(juActivity.ArticleType, WebsiteOwner), 
                        GetArticleLink(juActivity.JuActivityID, WebsiteOwner, juActivity.ArticleType),
                        juActivity.ActivityName);
                }
            }
            else if (noticeType == NoticeType.FavoriteReview)
            {
                sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  收藏了您的评论 ：{2}",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user), content);
            }
            else if (noticeType == NoticeType.JuActivityPraise)
            {
                if (juActivity.ArticleType == "Statuses")
                {
                    sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  赞了您的动态 ：{2}",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user), juActivity.ActivityDescription);
                }
                else if (juActivity.ArticleType == "Comment")
                {
                    sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  赞了您的{2} ：<a href=\"{3}\">{4}</a>",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user),
                        GetArticleTypeName(juActivity.ArticleType, WebsiteOwner),
                        GetArticleLink(juActivity.JuActivityID, WebsiteOwner, juActivity.ArticleType),
                        juActivity.Summary);
                }
                else
                {
                    sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  赞了您的{2} ：<a href=\"{3}\">《{4}》</a>",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user),
                        GetArticleTypeName(juActivity.ArticleType, WebsiteOwner), 
                        GetArticleLink(juActivity.JuActivityID, WebsiteOwner, juActivity.ArticleType),
                        juActivity.ActivityName);
                }
            }
            else if (noticeType == NoticeType.DisJuActivityPraise)
            {
                if (juActivity.ArticleType == "Statuses")
                {
                    sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  取消了对您的{2}的点赞 ：{3}",
                            GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user), 
                            GetArticleTypeName(juActivity.ArticleType, WebsiteOwner), juActivity.ActivityDescription);
                }
                else if (juActivity.ArticleType == "Comment")
                {
                    sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  取消了对您的{2}的点赞 ：<a href=\"{3}\">{4}</a>",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user),
                        GetArticleTypeName(juActivity.ArticleType, WebsiteOwner),
                        GetArticleLink(juActivity.JuActivityID, WebsiteOwner, juActivity.ArticleType), 
                        juActivity.Summary);
                }
                else
                {
                    sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  取消了您的{2}的点赞 ：<a href=\"{3}\">《{4}》</a>",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user),
                        GetArticleTypeName(juActivity.ArticleType, WebsiteOwner), 
                        GetArticleLink(juActivity.JuActivityID, WebsiteOwner, juActivity.ArticleType),
                        juActivity.ActivityName);
                }
            }
            else if (noticeType == NoticeType.ReviewPraise)
            {
                sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  赞了您的回复 ：{2}",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user), content);
            }
            else if (noticeType == NoticeType.ReportJuActivityIllegalContent)
            {
                if (juActivity.ArticleType == "Statuses")
                {
                    sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  举报了您的动态 ：{2}",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user), juActivity.ActivityDescription);
                }
                else
                {
                    sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  举报了您的{2} ：<a href=\"{3}\">《{4}》</a>",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user),
                        GetArticleTypeName(juActivity.ArticleType, WebsiteOwner), 
                        GetArticleLink(juActivity.JuActivityID, WebsiteOwner, juActivity.ArticleType),
                        juActivity.ActivityName);
                }
            }
            else if (noticeType == NoticeType.ReportReviewIllegalContent)
            {
                sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  举报了您的评论：{2}", 
                    GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user), content);
            }
            else if (noticeType == NoticeType.ReviewArticle)
            {
                if (juActivity.ArticleType == "Statuses")
                {
                    sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  评论了您的动态 ：{2}",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user),juActivity.ActivityDescription);
                    sbHtml.AppendFormat("<br /><br />评论：{0}", content);
                }
                else
                {
                    sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  评论了您的{2} ：<a href=\"{3}\">《{4}》</a>",
                        GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user), 
                        GetArticleTypeName(juActivity.ArticleType, WebsiteOwner), GetArticleLink(juActivity.JuActivityID,WebsiteOwner,juActivity.ArticleType),
                        juActivity.ActivityName);
                    sbHtml.AppendFormat("<br /><br />评论：{0}", content);
                }
            }
            else if (noticeType == NoticeType.FriendApply)
            {
                sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  申请成为您的好友 <br />", GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user));
                sbHtml.AppendFormat("<span class=\"j-pass\" data-account=\"{0}\"  data-nick=\"{1}\" data-ico=\"{2}\">同意</span>", user.AutoID, bllUser.GetUserDispalyName(user),bllUser.GetUserDispalyAvatar(user));
                sbHtml.AppendFormat("<span class=\"j-reject\" data-account=\"{0}\"  data-nick=\"{1}\" data-ico=\"{2}\">拒绝</span>", user.AutoID, bllUser.GetUserDispalyName(user), bllUser.GetUserDispalyAvatar(user));
            }
            else if (noticeType == NoticeType.PassFriendApply)
            {
                sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  通过了您的好友申请 ", GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user));
            }
            else if (noticeType == NoticeType.RejectFriendApply)
            {
                sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  拒绝了您的好友申请 ", GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user));
            }
            else if (noticeType == NoticeType.DeleteFriend)
            {
                sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  已将您从好友中删除 ", GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user));
            }
            else if (noticeType == NoticeType.SystemMessage)
            {
                sbHtml.AppendFormat("<a href=\"javascript:;\">系统消息</a> ：{0}", content);
            }
            else if (noticeType == NoticeType.Message)
            {
                sbHtml.AppendFormat("<a href=\"{0}\">{1}</a>  给您留言 ：{2}",
                    GetUserLink(user.AutoID, WebsiteOwner), bllUser.GetUserDispalyName(user), content);
            }
            return sbHtml.ToString();
        }
        public string GetArticleTypeName(string type, string websiteOwner)
        {
            if (websiteOwner == "stockplayer")
            {
                if (type == "Bowen")
                {
                    return "博文";
                }
                else if (type == "Stock")
                {
                    return "股权交易";
                }
                else if (type == "Comment")
                {
                    return "时评";
                }
                else if (type == "CompanyPublish")
                {
                    return "公司发布";
                }
            }
            else if (websiteOwner == "ubi")
            {
                if (type == "Statuses")
                {
                    return "动态";
                }
                else if (type == "Question")
                {
                    return "问题";
                }
            }
            return "文章";
        }
        public string GetUserLink(int id, string websiteOwner){
            StringBuilder sbHtml = new StringBuilder();
            if (websiteOwner == "ubi")
            {
                sbHtml.AppendFormat("#/userspace/{0}", id);
            }
            else if (websiteOwner == "stockplayer")
            {
                sbHtml.AppendFormat("/customize/StockPlayer/src/UserCenter/UserCenter.aspx?id={0}", id);
            }
            else
            {
                sbHtml.AppendFormat("javascript:void(0);");
            }
            return sbHtml.ToString();
        }
        public string GetArticleLink(int id, string websiteOwner, string type)
        {
            StringBuilder sbHtml = new StringBuilder();
            if (websiteOwner == "ubi")
            {
                if (type == "Question")
                {
                    sbHtml.AppendFormat("#/ask/{0}", id);
                }
                else
                {
                    sbHtml.AppendFormat("#/askArticleDetail/{0}", id);
                }
            }
            else if (websiteOwner == "stockplayer")
            {
                sbHtml.AppendFormat("/customize/stockplayer/src/detail/detail.aspx?jid={0}", id);
            }
            else
            {
                sbHtml.AppendFormat("javascript:void(0);");
            }
            return sbHtml.ToString();
        }
        public string GetArticleLinkText(JuActivityInfo juActivity)
        {
            if (juActivity.ArticleType == "Statuses")
            {
                return juActivity.ActivityDescription;
            }
            else if (juActivity.ArticleType == "Comment")
            {
                return juActivity.Summary;
            }
            else
            {
                return "《"+juActivity.ActivityName+"》";
            }
        }

        public string GetNoticeTypeName(NoticeType noticeType)
        {
            string result = ""; 
            switch (noticeType)
            {
                case NoticeType.SystemMessage:
                    result = "系统消息";
                    break;
                case NoticeType.ReviewReminder:
                    result = "话题提醒";
                    break;
                case NoticeType.QuestionaryReminder:
                    result = "问卷提醒";
                    break;
                case NoticeType.InviteAnswer:
                    result = "邀请回答";
                    break;
                case NoticeType.FollowUser:
                    result = "用户被关注";
                    break;
                case NoticeType.DisFollowUser:
                    result = "取消关注用户";
                    break;
                case NoticeType.FollowArticle:
                    result = "文章被关注";
                    break;
                case NoticeType.FollowQuestion:
                    result = "问题被关注";
                    break;
                case NoticeType.QuestionIsAnswered:
                    result = "提出的问题被回答";
                    break;
                case NoticeType.FollowQuestionIsAnswered:
                    result = "关注的问题被回答";
                    break;
                case NoticeType.FavoriteArticle:
                    result = "文章被收藏";
                    break;
                case NoticeType.FavoriteReview:
                    result = "评论被收藏";
                    break;
                case NoticeType.JuActivityPraise:
                    result = "文章被点赞";
                    break;
                case NoticeType.ReviewPraise:
                    result = "回复被点赞";
                    break;
                case NoticeType.ReportJuActivityIllegalContent:
                    result = "文章被举报";
                    break;
                case NoticeType.ReportReviewIllegalContent:
                    result = "回复被举报";
                    break;
                case NoticeType.ReviewArticle:
                    result = "文章被评论";
                    break;
                case NoticeType.Reward:
                    result = "打赏";
                    break;
                case NoticeType.GetReward:
                    result = "获得打赏";
                    break;
                default:
                    break;
            }
            return result;
        }

        public enum NoticeType
        {
            SystemMessage = 1,          //系统消息

            ReviewReminder = 11,        //话题提醒

            QuestionaryReminder = 21,   //问卷提醒

            FinancialNotice=31,         //金融通知
            
            AppointmentNotice=32,       //约会通知
            /// <summary>
            /// 邀请回答
            /// </summary>
            InviteAnswer = 101,
            /// <summary>
            /// 关注用户
            /// </summary>
            FollowUser = 102,
            /// <summary>
            /// 取消关注用户
            /// </summary>
            DisFollowUser = 1021,
            /// <summary>
            /// 文章被关注
            /// </summary>
            FollowArticle = 103,
            /// <summary>
            /// 关注问题
            /// </summary>
            FollowQuestion = 104,
            /// <summary>
            /// 提出的问题被回答
            /// </summary>
            QuestionIsAnswered = 105,
            /// <summary>
            /// 关注的问题被回答
            /// </summary>
            FollowQuestionIsAnswered = 106,
            /// <summary>
            /// 文章被收藏
            /// </summary>
            FavoriteArticle = 107,
            /// <summary>
            /// 评论被收藏
            /// </summary>
            FavoriteReview = 108,
            /// <summary>
            /// 文章被点赞
            /// </summary>
            JuActivityPraise = 109,
            /// <summary>
            /// 回复被点赞
            /// </summary>
            ReviewPraise = 110,
            /// <summary>
            /// 文章被举报
            /// </summary>
            ReportJuActivityIllegalContent = 111,
            /// <summary>
            /// 回复被举报
            /// </summary>
            ReportReviewIllegalContent = 112,
            /// <summary>
            /// 文章被评论
            /// </summary>
            ReviewArticle = 113,
            /// <summary>
            /// 申请添加好友
            /// </summary>
            FriendApply = 114,
            /// <summary>
            /// 通过好友申请
            /// </summary>
            PassFriendApply = 115,
            /// <summary>
            /// 拒绝好友申请
            /// </summary>
            RejectFriendApply = 116,
            /// <summary>
            /// 删除好友
            /// </summary>
            DeleteFriend = 117,
            
            /// <summary>
            /// 文章被取消点赞
            /// </summary>
            DisJuActivityPraise = 119,
            
            /// <summary>
            /// 打赏
            /// </summary>
            Reward = 120,
            
            /// <summary>
            /// 获得打赏
            /// </summary>
            GetReward = 121,

            /// <summary>
            /// 留言
            /// </summary>
            Message = 122
        }

        public enum SendType
        {
            All = 0,          //发送给所有人

            Group = 1,        //发送给群组，逗号区分

            Personal = 2,   //发送给个人列表，逗号区分
        }
    }

    
}
