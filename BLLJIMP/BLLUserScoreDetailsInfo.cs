using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP
{
    public class BLLUserScoreDetailsInfo : BLL
    {
        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="scoreType"></param>
        /// <param name="relationID"></param>
        /// <param name="userID"></param>
        /// <param name="scoreEvents"></param>
        /// <param name="month">yyyy-MM</param>
        /// <returns></returns>
        public string GetWhereString(string websiteOwner, string scoreType, string relationID,
            string userIDs = "", string scoreEvents = "", string month = "", string startTime = "",
            string endTime = "", string scoreWinStatus = "", string relationUserID = "", string isPrint = "", string scoreNotEvents = "",string moreQuery = "")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbWhere.AppendFormat(" And WebSiteOwner='{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(scoreType)) {
                sbWhere.AppendFormat(" And ScoreType='{0}' ", scoreType);
            }
            else
            {
                sbWhere.AppendFormat(" And ScoreType!='AccountAmount' And ScoreType!='TotalAmount' "); 
            }
            if (!string.IsNullOrWhiteSpace(relationID)) sbWhere.AppendFormat(" And RelationID='{0}' ", relationID);
            if (!string.IsNullOrWhiteSpace(relationUserID)) sbWhere.AppendFormat(" And RelationUserID='{0}' ", relationUserID);
            if (!string.IsNullOrWhiteSpace(isPrint)) sbWhere.AppendFormat(" And IsPrint='{0}' ", isPrint);
            if (!string.IsNullOrWhiteSpace(userIDs))
            {
                userIDs = "'" + userIDs.Replace(",", "','") + "'";
                sbWhere.AppendFormat(" And  UserID In ({0}) ", userIDs); 
            }
            if (!string.IsNullOrWhiteSpace(scoreEvents))
            {
                scoreEvents = "'" + scoreEvents.Replace(",", "','") + "'";
                sbWhere.AppendFormat(" And  ScoreEvent In ({0}) ", scoreEvents);
            }
            if (!string.IsNullOrWhiteSpace(scoreNotEvents))
            {
                scoreEvents = "'" + scoreNotEvents.Replace(",", "','") + "'";
                sbWhere.AppendFormat(" And  ScoreEvent Not In ({0}) ", scoreEvents);
            }
            if (!string.IsNullOrWhiteSpace(month))
            {
                sbWhere.AppendFormat(" And  AddTime>='{0}' And AddTime<'{1}' ", 
                    Convert.ToDateTime(month + "-01").ToString("yyyy-MM-dd"), 
                    Convert.ToDateTime(month + "-01").AddMonths(1).ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                sbWhere.AppendFormat(" And AddTime>='{0}' ", startTime);
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                sbWhere.AppendFormat(" And AddTime<='{0}' ",endTime);
            }
            if (!string.IsNullOrWhiteSpace(scoreWinStatus))
            {
                if (scoreWinStatus == "1")
                {
                    sbWhere.AppendFormat(" And Score>0 ");
                } 
                else if (scoreWinStatus == "2")
                {
                    sbWhere.AppendFormat(" And Score<0 ");
                }
            }

            if (!string.IsNullOrWhiteSpace(moreQuery))
            {
                sbWhere.AppendFormat(" {0} ",moreQuery);
            }

            return sbWhere.ToString();
        }
        /// <summary>
        /// 查询分类总积分
        /// </summary>
        /// <param name="scoreType"></param>
        /// <param name="relationID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public double GetSumScore(string websiteOwner, string scoreType = "", string relationID = "", string userIDs = "",
            BLLTransaction trans = null, string scoreEvents = "", string month = "", string sumColName = "", string startTime = "",
            string endTime = "", string scoreWinStatus = "", string relationUserID = "", string isPrint = "", string scoreNotEvents = "", string moreQuery = "")
        {
            double result = 0;
            if (string.IsNullOrWhiteSpace(sumColName)) sumColName = "Score";
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" Select Sum({0}) From  ZCJ_UserScoreDetailsInfo ", sumColName);
            sbSql.AppendFormat(" Where {0} ", GetWhereString(websiteOwner, scoreType, relationID, userIDs, scoreEvents, month,
                startTime, endTime, scoreWinStatus, relationUserID, isPrint, scoreNotEvents, moreQuery));
            object resultObj = GetSingle(sbSql.ToString(), trans, "UserScoreDetailsInfo");
            if (resultObj != null)
            {
                result = Convert.ToDouble(resultObj);
            }
            return result;
        }
        /// <summary>
        /// 查询分类积分列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="scoreType"></param>
        /// <param name="relationID"></param>
        /// <param name="userID"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public List<UserScoreDetailsInfo> GetScoreList(int rows, int page, string websiteOwner, string scoreType = "", string relationID = "", string userIDs = "", string colName = "", string scoreEvents = "",
            string month = "", string startTime = "", string endTime = "", string scoreWinStatus = "", string relationUserID = "", string isPrint = "", string scoreNotEvents = "", string moreQuery = "")
        {
            if (!string.IsNullOrWhiteSpace(colName))
            {
                return GetColList<UserScoreDetailsInfo>(rows, page, GetWhereString(websiteOwner, scoreType, relationID, userIDs, scoreEvents, month, startTime, endTime, scoreWinStatus, relationUserID, isPrint, scoreNotEvents, moreQuery), " AddTime Desc,AutoID desc", colName);
            }
            return GetLit<UserScoreDetailsInfo>(rows, page, GetWhereString(websiteOwner, scoreType, relationID, userIDs, scoreEvents, month, startTime, endTime, scoreWinStatus, relationUserID, isPrint, scoreNotEvents, moreQuery), "AddTime Desc,AutoID desc");
        }
        /// <summary>
        /// 删除积分
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="scoreType"></param>
        /// <param name="relationID"></param>
        /// <param name="userIDs"></param>
        /// <param name="colName"></param>
        /// <param name="scoreEvents"></param>
        /// <param name="month"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="scoreWinStatus"></param>
        /// <param name="relationUserID"></param>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        public bool DeleteScore(string websiteOwner, string scoreType = "", string relationID = "", string userIDs = "", string scoreEvents = "",
            string month = "", string startTime = "", string endTime = "", string scoreWinStatus = "", string relationUserID = "", string isPrint = "")
        {
            return Delete(new UserScoreDetailsInfo(),
                GetWhereString(websiteOwner, scoreType, relationID, userIDs, scoreEvents, month, startTime, endTime, scoreWinStatus, relationUserID, isPrint)) > 0;
        }
        /// <summary>
        /// 获取最新的一条
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="scoreType"></param>
        /// <param name="relationID"></param>
        /// <param name="userIDs"></param>
        /// <param name="colName"></param>
        /// <param name="scoreEvents"></param>
        /// <param name="month"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="scoreWinStatus"></param>
        /// <returns></returns>
        public UserScoreDetailsInfo GetNewScore(string websiteOwner, string scoreType = "", string relationID = "", string userIDs = "", string colName = "", string scoreEvents = "",
            string month = "", string startTime = "", string endTime = "", string scoreWinStatus = "", string isPrint = "")
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(GetWhereString(websiteOwner, scoreType, relationID, userIDs,
                scoreEvents, month, startTime, endTime, scoreWinStatus));
            sbSql.AppendFormat(" Order By AddTime Desc,AutoID desc ");
            return Get<UserScoreDetailsInfo>(sbSql.ToString());
        }

        /// <summary>
        /// 查询分类积分记录数
        /// </summary>
        /// <param name="scoreType"></param>
        /// <param name="relationID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int GetScoreRowCount(string websiteOwner, string scoreType = "", string relationID = "", string userIDs = "", string scoreEvents = "",
            string month = "", string startTime = "", string endTime = "", string scoreWinStatus = "", string relationUserID="", string isPrint="", string scoreNotEvents = "", string moreQuery = "")
        {
            return GetCount<UserScoreDetailsInfo>(GetWhereString(websiteOwner, scoreType, relationID,
                userIDs, scoreEvents, month, startTime, endTime, scoreWinStatus, relationUserID, isPrint, scoreNotEvents, moreQuery));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="curUser"></param>
        /// <param name="author"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        public bool RewardJuActivity(UserInfo curUser, UserInfo author, JuActivityInfo activity, double score, string websiteOwner, out string msg,
            string scoreName = "积分", string actionName = "赠送")
        {
            msg = "";
            msg = string.Format("{0}{1}完成", actionName, scoreName);

            if (author == null && activity == null)
            {
                msg = string.Format("{0}目标不能为空", actionName);
                return false;
            }
            DateTime curTime = DateTime.Now;
            BLLSystemNotice bllSystemNotice = new BLLSystemNotice();
            BLLUser bllUser = new BLLUser();

            UserScoreDetailsInfo scoreModel = new UserScoreDetailsInfo();
            #region 打赏明细
            scoreModel.Score = 0 - score;
            scoreModel.ScoreType = "Reward";
            scoreModel.UserID = curUser.UserID;
            scoreModel.WebSiteOwner = websiteOwner;
            scoreModel.AddTime = curTime;
            scoreModel.AddNote = string.Format("{0}消耗{1}", actionName, scoreName);
            if (activity != null) scoreModel.RelationID = activity.JuActivityID.ToString();
            if (author != null) scoreModel.Ex1 = author.AutoID.ToString();
            #endregion

            SystemNotice systemNotice = new SystemNotice();
            #region 打赏通知
            systemNotice.Title = scoreModel.AddNote;
            if (activity != null)
            {
                systemNotice.Ncontent = string.Format("您{0}了<a href=\"{6}\">{7}</a>{1}{2}{3} ： <a href=\"{4}\">{5}</a>", actionName,
                    bllSystemNotice.GetArticleTypeName(activity.ArticleType, websiteOwner),
                    score,
                    scoreName,
                    bllSystemNotice.GetArticleLink(activity.JuActivityID, websiteOwner, activity.ArticleType),
                    bllSystemNotice.GetArticleLinkText(activity),
                    bllSystemNotice.GetUserLink(author.AutoID, websiteOwner),
                    bllUser.GetUserDispalyName(author)
                    );
            }
            else
            {
                systemNotice.Ncontent = string.Format("您{0}了<a href=\"{4}\">{1}</a>{2}{3}",
                    actionName,
                    bllUser.GetUserDispalyName(author),
                    score,
                    scoreName,
                    bllSystemNotice.GetUserLink(author.AutoID, websiteOwner));
            }
            systemNotice.NoticeType = (int)BLLSystemNotice.NoticeType.Reward;
            systemNotice.InsertTime = curTime;
            systemNotice.WebsiteOwner = websiteOwner;
            systemNotice.SendType = 2;
            systemNotice.UserId = curUser.UserID;
            systemNotice.Receivers = curUser.UserID;
            #endregion 打赏通知

            UserScoreDetailsInfo scoreGetModel = new UserScoreDetailsInfo();
            SystemNotice systemNoticeGet = new SystemNotice();
            #region 获赏明细
            if (author != null)
            {
                scoreGetModel.Score = score;
                scoreGetModel.ScoreType = "GetReward";
                scoreGetModel.UserID = author.UserID;
                scoreGetModel.WebSiteOwner = websiteOwner;
                scoreGetModel.AddTime = curTime;
                scoreGetModel.AddNote = string.Format("获得{0}{1}", actionName, scoreName);
                if (activity != null) scoreGetModel.RelationID = activity.JuActivityID.ToString();
                scoreGetModel.Ex1 = curUser.AutoID.ToString();

                #region 获赏通知
                systemNoticeGet.Title = scoreGetModel.AddNote;
                if (activity != null)
                {
                    systemNoticeGet.Ncontent = string.Format("您的{0}获得了<a href=\"{1}\">{2}</a>{3}的{4}{5} ： <a href=\"{6}\">{7}</a>",
                        bllSystemNotice.GetArticleTypeName(activity.ArticleType, websiteOwner),
                        bllSystemNotice.GetUserLink(curUser.AutoID, websiteOwner),
                        bllUser.GetUserDispalyName(curUser),
                        actionName,
                        score,
                        scoreName,
                        bllSystemNotice.GetArticleLink(activity.JuActivityID, websiteOwner, activity.ArticleType),
                        bllSystemNotice.GetArticleLinkText(activity));
                }
                else
                {
                    systemNoticeGet.Ncontent = string.Format("您获得了<a href=\"{0}\">{1}</a>{2}的{3}{4}",
                        bllSystemNotice.GetUserLink(curUser.AutoID, websiteOwner),
                        bllUser.GetUserDispalyName(curUser),
                        actionName,
                        score,
                        scoreName);
                }
                systemNoticeGet.NoticeType = (int)BLLSystemNotice.NoticeType.GetReward;
                systemNoticeGet.InsertTime = curTime;
                systemNoticeGet.WebsiteOwner = websiteOwner;
                systemNoticeGet.SendType = 2;
                systemNoticeGet.UserId = author.UserID;
                systemNoticeGet.Receivers = author.UserID;

                #endregion 打赏通知
            }
            #endregion

            ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            #region 打赏用户积分处理
            if (Update(curUser,
                string.Format(" TotalScore = TotalScore - {0}", score),
                string.Format(" AutoID = {0} And  TotalScore>= {1}", curUser.AutoID, score),
                tran) <= 0)
            {
                tran.Rollback();
                msg = string.Format("{0}失败，可能是{1}不足", actionName, scoreName);
                return false;
            }
            curUser = GetByKey<UserInfo>("AutoID", curUser.AutoID.ToString(), false, tran: tran);
            scoreModel.TotalScore = curUser.TotalScore;
            if (!Add(scoreModel, tran))
            {
                tran.Rollback();
                msg = string.Format("{0}记录{1}明细出错", actionName, scoreName);
                return false;
            }

            systemNotice.SerialNum = GetGUID(TransacType.SendSystemNotice);
            if (!Add(systemNotice, tran))
            {
                tran.Rollback();
                msg = string.Format("{0}{1}通知出错", actionName, scoreName);
                return false;
            }

            #endregion

            #region 打赏目标用户积分处理
            if (author != null)
            {
                if (Update(author,
                    string.Format(" TotalScore = TotalScore + {0}", score),
                    string.Format(" AutoID = {0} ", author.AutoID),
                    tran) <= 0)
                {
                    tran.Rollback();
                    msg = string.Format("{0}目标用户加{1}出错", actionName, scoreName);
                    return false;
                }
                author = GetByKey<UserInfo>("AutoID", author.AutoID.ToString(), false, tran: tran);
                scoreGetModel.TotalScore = author.TotalScore;
                if (!Add(scoreGetModel, tran))
                {
                    tran.Rollback();
                    msg = string.Format("{0}目标用户记录{1}明细出错", actionName, scoreName);
                    return false;
                }
                systemNoticeGet.SerialNum = GetGUID(TransacType.SendSystemNotice);
                if (!Add(systemNoticeGet, tran))
                {
                    tran.Rollback();
                    msg = string.Format("获得{0}{1}通知出错", actionName, scoreName);
                    return false;
                }
            }
            #endregion
            #region 打赏目标积分处理
            if (activity != null)
            {
                double sumScore = GetSumScore(websiteOwner, "GetReward", activity.JuActivityID.ToString(), "", tran);

                if (Update(activity,
                    string.Format(" RewardTotal={0}", sumScore),
                    string.Format(" JuActivityID = {0} ", activity.JuActivityID),
                    tran) <= 0)
                {
                    tran.Rollback();
                    msg = string.Format("{0}目标加{1}出错", actionName, scoreName);
                    return false;
                }
            }
            #endregion

            tran.Commit();
            return true;
        }


        /// <summary>
        /// 获取积分详细信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserScoreDetailsInfo> GetTotalScore(string userId, Enums.UserScoreBalanceType balanceType, string scoreType,string relationId="")
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ",WebsiteOwner);
            if (!string.IsNullOrEmpty(userId))
            {
                sbSql.AppendFormat(" AND UserID='{0}'", userId);
            }
            if (!string.IsNullOrEmpty(relationId))
            {
                sbSql.AppendFormat(" AND RelationID='{0}' ", relationId);
            }

            if (balanceType == Enums.UserScoreBalanceType.Income) {
                sbSql.AppendFormat(" AND Score > 0 ");
            }
            else if (balanceType == Enums.UserScoreBalanceType.Pay) 
            {
                sbSql.AppendFormat(" AND Score < 0 ");
            }

            if (!string.IsNullOrEmpty(scoreType))
            {
                sbSql.AppendFormat(" AND ScoreType ='{0}' ", scoreType);
            }
 
            sbSql.AppendFormat(" And ScoreType!='AccountAmount' And ScoreType!='TotalAmount' ");

            return GetList<UserScoreDetailsInfo>(sbSql.ToString()) ;
        }
        
    }
}
