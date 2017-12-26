using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model.Forbes;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 答题
    /// </summary>
    public class BLLForbesQuestion : BLL
    {
        /// <summary>
        /// 随机生成题目
        /// </summary>
        /// <param name="category">类型</param>
        /// <param name="limitId">排除ID</param>
        /// <param name="websiteOwner">站点</param>
        /// <returns></returns>
        public ForbesQuestion GetRandomQuestion(string category, string limitId, string websiteOwner)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(category)) sbSql.AppendFormat(" AND Category ='{0}' ", category);
            if (!string.IsNullOrWhiteSpace(limitId)) sbSql.AppendFormat(" AND AutoID NOT IN ({0}) ", limitId);
            sbSql.AppendFormat(" ORDER BY NEWID() ", websiteOwner);
            return Get<ForbesQuestion>(sbSql.ToString());
        }

        /// <summary>
        /// 获取结果列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="websiteOwner">站点</param>
        /// <param name="activity">活动</param>
        /// <returns></returns>
        public List<ForbesQuestionResult> GetQuestionResultList(string userId, string websiteOwner, string activity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            sbSql.AppendFormat(" AND UserId='{0}' ", userId);
            sbSql.AppendFormat(" AND Activity='{0}' ", activity);
            return GetList<ForbesQuestionResult>(sbSql.ToString());
        }

        /// <summary>
        /// 获取结果数
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="websiteOwner">站点</param>
        /// <param name="activity">活动</param>
        /// <returns></returns>
        public int GetQuestionResultCount(string userId, string websiteOwner, string activity)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            sbSql.AppendFormat(" AND UserId='{0}' ", userId);
            sbSql.AppendFormat(" AND Activity='{0}' ", activity);
            return GetCount<ForbesQuestionResult>(sbSql.ToString());
        }

        /// <summary>
        /// 获取答题次序号
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        private int GetResultNum(string userId, string websiteOwner)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            sbSql.AppendFormat(" AND UserId='{0}' ", userId);
            sbSql.AppendFormat(" ORDER BY Num Desc ", websiteOwner);
            ForbesQuestionResult questionResult = Get<ForbesQuestionResult>(sbSql.ToString());
            if (questionResult == null)
            {
                return 1;
            }
            else
            {
                return questionResult.Num + 1;
            }
        }
        /// <summary>
        /// 根据用户，回答次序，站点查结果
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="resultNum"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public ForbesQuestionResult GetResultByUser(string userId, int resultNum, string websiteOwner)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            sbSql.AppendFormat(" AND UserId='{0}' ", userId);
            sbSql.AppendFormat(" AND Num={0} ", resultNum);
            return Get<ForbesQuestionResult>(sbSql.ToString());
        }

        /// <summary>
        /// 修改兑换编码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="resultNum"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="GiftCode"></param>
        /// <returns></returns>
        public bool UpdateResultGiftCode(string userId, int resultNum, string websiteOwner, string GiftCode)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            sbSql.AppendFormat(" AND UserId='{0}' ", userId);
            sbSql.AppendFormat(" AND Num={0} ", resultNum);
            return Update(new ForbesQuestionResult(),string.Format(" GiftCode='{0}'", GiftCode), sbSql.ToString())>0;
        }

        /// <summary>
        /// 提交答案
        /// </summary>
        /// <param name="questionPersonalList"></param>
        /// <param name="needCount"></param>
        /// <param name="UserId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public bool PostQuestionResult(List<ForbesQuestionPersonal> questionPersonalList, int needCount, string UserId, string websiteOwner
            , string activity, out string errmsg, out int totalScore, out int resultNum)
        {
            if (needCount > 0 && questionPersonalList.Count < needCount)
            {
                errmsg = "请完成答题";
                totalScore = 0;
                resultNum = 0;
                return false;
            }
            ForbesQuestionResult questionResult = new ForbesQuestionResult();
            questionResult.Num = GetResultNum(UserId, websiteOwner);
            resultNum = questionResult.Num;
            questionResult.TotalScore = questionPersonalList.Sum(p => p.Score);
            totalScore = questionResult.TotalScore;
            questionResult.UserId = UserId;
            questionResult.WebsiteOwner = websiteOwner;
            questionResult.CreateDate = DateTime.Now;
            questionResult.Status = 1;
            questionResult.Activity = activity;

            if (Add(questionResult))
            {
                foreach (ForbesQuestionPersonal QuestionPersonal in questionPersonalList)
                {
                    QuestionPersonal.Count = questionResult.Num;
                    QuestionPersonal.Status = 1;
                    Add(QuestionPersonal);
                }
                errmsg = "提交完成";
                return true;
            }
            else
            {
                errmsg = "提交失败";
                return false;
            }
        }

    }
}
