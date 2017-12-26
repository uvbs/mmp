using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP
{
    public class BLLQuestion : BLL
    {
        /// <summary>
        /// 获取题目列表
        /// </summary>
        /// <param name="questionnaires">问卷组，@分隔多张问卷库，|分隔每张问卷抽题数</param>
        /// <param name="returnAll">是否一次全部返回，1全部返回</param>
        /// <param name="isRanSortQuestion">是否随机排序题目，1随机排序</param>
        /// <param name="isRanSortAnswer">是否随机排序选项，1随机排序</param>
        /// <param name="usedId">已使用过的题目ID</param>
        /// <param name="websiteOwner">限制站点</param>
        /// <param name="questions">out 输出题目列表</param>
        /// <param name="msg">out 输出错误信息</param>
        /// <returns></returns>
        public bool GetQuestionList(string questionnaires, string returnAll, string isRanSortQuestion, string isRanSortAnswer, string usedId
            , string websiteOwner, out List<Question> questions, out string msg)
        {
            questions = null;
            questions = new List<Question>();
            msg = "";

            Dictionary<int, int> dicQuestionnaire = new Dictionary<int, int>();
            StringBuilder sbSql = new StringBuilder();

            #region 问卷组格式检查 分割
            try
            {
                foreach (string item in questionnaires.Split('@'))
                {
                    string[] its = item.Split('|');
                    dicQuestionnaire.Add(Convert.ToInt32(its[0]), Convert.ToInt32(its[1]));
                }
            }
            catch (Exception ex)
            {
                msg = "问卷组格式不正确";
                return false;
            }
            if (dicQuestionnaire.Keys.Count == 0) return true;
            #endregion


            #region 过滤问卷
            try
            {
                string questionnaireIds = string.Join(",", dicQuestionnaire.Keys.ToList());
                sbSql.AppendFormat(" SELECT [QuestionnaireID] "); // 使用AddScore代替数量
                sbSql.AppendFormat(" FROM [ZCJ_Questionnaire] WHERE [QuestionnaireID] IN ({0}) ", questionnaireIds);
                if (!string.IsNullOrWhiteSpace(websiteOwner)) sbSql.AppendFormat(" AND [WebsiteOwner] ='{0}' ", websiteOwner);
                List<Questionnaire> questionnaireList = BLL.Query<Questionnaire>(sbSql.ToString());
                sbSql = new StringBuilder(); // 清空StringBuilder
                if (questionnaireList.Count > 0)
                {
                    foreach (KeyValuePair<int, int> item in dicQuestionnaire)
                    {
                        if (questionnaireList.FirstOrDefault(p => p.QuestionnaireID == item.Key) == null)
                        {
                            dicQuestionnaire.Remove(item.Key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "获取题目出错";
                //msg = sbSql.ToString();
                return false;
            }
            if (dicQuestionnaire.Keys.Count == 0) return true;
            #endregion


            #region 排除已使用题的数量
            if (!string.IsNullOrWhiteSpace(usedId))
            {
                List<Questionnaire> usedList = new List<Questionnaire>();
                try
                {
                    sbSql.AppendFormat(" SELECT [QuestionnaireID], COUNT(1) [AddScore] "); // 使用AddScore代替数量
                    sbSql.AppendFormat(" FROM [ZCJ_Question] WHERE [QuestionID] IN ({0}) ", usedId);
                    if (!string.IsNullOrWhiteSpace(websiteOwner)) sbSql.AppendFormat(" AND [WebsiteOwner] ='{0}' ", websiteOwner);
                    sbSql.AppendFormat(" GROUP BY [QuestionnaireID]");
                    usedList = BLL.Query<Questionnaire>(sbSql.ToString());
                    sbSql = new StringBuilder(); // 清空StringBuilder
                }
                catch (Exception ex)
                {
                    msg = "查询已使用题目出错";
                    return false;
                }
                for (int i = 0; i < usedList.Count; i++)
                {
                    int key = usedList[i].QuestionnaireID;
                    int newCount = dicQuestionnaire[key] - usedList[i].AddScore;
                    if (newCount <= 0)
                    {
                        dicQuestionnaire.Remove(key);
                        newCount = 0;
                    }
                    else
                    {
                        dicQuestionnaire[key] = newCount;
                    }
                }
            }
            if (dicQuestionnaire.Keys.Count == 0) return true;
            #endregion


            #region 抽题
            foreach (KeyValuePair<int, int> item in dicQuestionnaire)
            {
                if (item.Value <= 0)
                {
                    dicQuestionnaire.Remove(item.Key);
                }
                else
                {
                    int topNum = returnAll == "1" ? item.Value : 1;
                    List<Question> tempQuestionList = GetQuestionListByQuestionnaire(item.Key, usedId, topNum, isRanSortQuestion);
                    if (tempQuestionList.Count > 0)
                    {
                        questions.AddRange(tempQuestionList);
                        if (returnAll != "1") break;
                    }
                }
            }
            #endregion

            //#region 题目重新排序 好像已经排过序了
            //if (dicQuestionnaire.Keys.Count > 1 && isRanSortQuestion == "1")
            //{
            //    for (int i = 0; i < questions.Count; i++)
            //    {
            //        questions[i].Sort = (new Random()).Next();
            //    }
            //    questions = questions.OrderBy(p => p.Sort).ToList();
            //}
            //#endregion

            #region 取出题目选项
            for (int i = 0; i < questions.Count; i++)
            {
                List<Answer> answerList = GetQuestionListByQuestionnaire(questions[i].QuestionID, isRanSortAnswer);
                //移除正确答案标记
                questions[i].Answers = (from p in answerList
                                          select new Answer
                                          {
                                             AnswerID = p.AnswerID,
                                             AnswerName = p.AnswerName
                                          }).ToList();
            }
            #endregion

            return true;
        }
        public bool CheckQuestionnaireSet(QuestionnaireSet nSet, out string msg)
        {
            msg = "";
            if (nSet == null)
            {
                msg = "未找到答题设置";
                return false;
            }
            if (nSet.StartDate > DateTime.Now)
            {
                msg = "答题还未开始";
                return false;
            }
            if (nSet.EndDate < DateTime.Now)
            {
                msg = "答题已结束";
                return false;
            }
            if (nSet.ScoreNum > 0 && nSet.Score > 0)
            {
                BLLUser bllUser = new BLLUser();
                int UserScoreNum = bllUser.GetUserScoreNum(GetCurrUserID(), "QuestionnaireSet", false, nSet.AutoID.ToString());
                if (UserScoreNum >= nSet.ScoreNum)
                {
                    msg = "您的答题次数已满";
                    return false;
                }
            }
            return true;
        }
        public bool GetQuestionListBySet(int setId, out List<Question> questions, out string msg)
        {
            questions = new List<Question>();
            msg = "";
            QuestionnaireSet nSet = GetByKey<QuestionnaireSet>("AutoID", setId.ToString());
            if (!CheckQuestionnaireSet(nSet, out msg))
            {
                return false;
            }
            return GetQuestionList(nSet.QuestionnaireId.Value + "|" + nSet.QuestionCount.Value, "1", nSet.IsQuestionRandom.ToString(), nSet.IsOptionRandom.ToString(), "", WebsiteOwner, out questions, out msg);
        }

        /// <summary>
        /// 获取题目列表
        /// </summary>
        /// <param name="questionnaireId"></param>
        /// <param name="usedId"></param>
        /// <param name="topNum"></param>
        /// <param name="isRanSortQuestion"></param>
        /// <returns></returns>
        private List<Question> GetQuestionListByQuestionnaire(int questionnaireId, string usedId, int topNum, string isRanSortQuestion)
        {
            string order = isRanSortQuestion == "1" ? "NEWID()" : "Sort";

            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("[QuestionnaireID] ={0} ", questionnaireId);
            if (!string.IsNullOrWhiteSpace(usedId)) sbSql.AppendFormat(" AND [QuestionID] In ({0}) ", usedId);

            return GetList<Question>(topNum, sbSql.ToString(), order);
        }

        /// <summary>
        /// 获取题目选项列表
        /// </summary>
        /// <param name="QuestionnaireID"></param>
        /// <param name="UsedID"></param>
        /// <param name="topNum"></param>
        /// <param name="IsRanSortQuestion"></param>
        /// <returns></returns>
        private List<Answer> GetQuestionListByQuestionnaire(int questionId, string isRanSortAnswer)
        {
            string order = isRanSortAnswer == "1" ? "NEWID()" : "QuestionID";
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("[QuestionID] ={0} ", questionId);
            return GetList<Answer>(int.MaxValue, sbSql.ToString(), order);
        }


        /// <summary>
        /// 提交答题数据
        /// </summary>
        /// <param name="postList">提交的答题数据</param>
        /// <param name="QuestionnaireIDStr">当前问卷ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="websiteOwner">站点ID</param>
        /// <param name="correctCount">正确数</param>
        /// <param name="msg">信息</param>
        /// <returns></returns>
        public bool PostQuestion(List<QuestionPostModel> postList, int questionnaireSetID, string userId, string websiteOwner,string userHostAddress,
            out int correctCount, out string msg, int questionnaireId)
        {
            correctCount = 0;
            msg = "";
            List<QuestionnaireRecordDetail> listRecordDetail = new List<QuestionnaireRecordDetail>();
            QuestionnaireRecord record = new QuestionnaireRecord();

            #region 检查 格式化答案
            if (postList == null || postList.Count == 0)
            {
                msg = "请提交答案";
                return false;
            }

            record.QuestionnaireID = questionnaireId;
            record.UserId = userId;
            record.WebsiteOwner = websiteOwner;
            record.AnswerCount = postList.Count;
            record.InsertDate = DateTime.Now;
            record.IP = userHostAddress;
            record.QuestionnaireSetID = questionnaireSetID;
            try
            {
                foreach (var item in postList)
                {
                    if (string.IsNullOrWhiteSpace(item.Answer)) continue;
                    Question nQuestion = Get<Question>(string.Format("QuestionID={0}", item.QuestionID));
                    if (nQuestion == null) continue;
                    if (record.QuestionnaireID == 0) record.QuestionnaireID = nQuestion.QuestionnaireID;
                    if (nQuestion.QuestionType == 0)
                    {
                        listRecordDetail.Add(new QuestionnaireRecordDetail(userId, questionnaireId, nQuestion.QuestionID, Convert.ToInt32(item.Answer), null, questionnaireSetID));
                        Answer nAnswer = Get<Answer>(string.Format("AnswerID={0}", item.Answer));
                        if (nAnswer == null) continue;
                        if (nAnswer.IsCorrect == 1) correctCount++;
                    }
                    else if (nQuestion.QuestionType == 1)
                    {
                        List<int> pAnswers = item.Answer.Split('|').Select(p => Convert.ToInt32(p)).ToList();
                        foreach (var nitem in pAnswers)
                        {
                            listRecordDetail.Add(new QuestionnaireRecordDetail(userId, questionnaireId, nQuestion.QuestionID, nitem, null, questionnaireSetID));
                        }
                        string pAnswerIDs = Common.MyStringHelper.ListToStr(pAnswers.OrderBy(p => p).ToList(), "", ",");
                        string nAnswerIDs = Common.MyStringHelper.ListToStr(GetList<Answer>(string.Format("QuestionID={0} AND IsCorrect=1 ", item.QuestionID))
                            .Select(p => p.AnswerID).OrderBy(p => p).ToList(), "", ",");

                        if (pAnswerIDs == nAnswerIDs) correctCount++;
                    }
                    else if (nQuestion.QuestionType == 2)
                    {
                        listRecordDetail.Add(new QuestionnaireRecordDetail(userId, questionnaireId, nQuestion.QuestionID, 0, item.Answer, questionnaireSetID));
                    }
                }
                record.CorrectCount = correctCount;
            }
            catch (Exception ex)
            {
                msg = "格式化提交答案出错";
                return false;
            }
            #endregion 

            BLLTransaction tran = new BLLTransaction();//事务
            try
            {
                record.RecordID = Convert.ToInt64(GetRecordGUID());
                if (!Add(record, tran))
                {
                    tran.Rollback();
                    msg = "提交数据库出错";
                    return false;
                }
                foreach (var item in listRecordDetail)
                {
                    item.RecordID = record.RecordID;
                    if (!Add(item, tran))
                    {
                        tran.Rollback();
                        msg = "提交数据库出错";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                msg = "提交数据库出错";
                return false;
            }
            tran.Commit();
            msg = "提交完成";
            return true;
        }


        /// <summary>
        /// 创建答题记录GUID
        /// </summary>
        /// <param name="transacType"></param>
        /// <returns></returns>
        public string GetRecordGUID()
        {
            string strSql = string.Format(
                                @"insert into ZCJ_QuestionnaireRecordGUID (UserID, TransacDate) 
                                    values ('{0}', GETDATE()) select @@IDENTITY",
                                                           this.UserID);
            return GetSingle(strSql).ToString();

        }


        /// <summary>
        /// 获取问卷列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="questionnaireName"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="total"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public List<Questionnaire> GetQuestionnaireList(int rows, int page, string type,string questionnaireName, string websiteOwner, out int total, string sort = "0")
        {
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", websiteOwner));
            if (!string.IsNullOrEmpty(questionnaireName))
            {
                sbWhere.AppendFormat(" And QuestionnaireName like '%{0}%'", questionnaireName);
            }
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" And QuestionnaireType={0}", type);
            }
            total = GetCount<Questionnaire>(sbWhere.ToString());
            string order = "";
            if (string.IsNullOrWhiteSpace(sort) || sort == "0") order = " QuestionnaireID DESC";
            return GetLit<Questionnaire>(rows, page, sbWhere.ToString(), order);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<Questionnaire> GetSelectList(string websiteOwner,string type)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            if (!string.IsNullOrWhiteSpace(type))
            {
                sbWhere.AppendFormat(" AND QuestionnaireType={0}", type);
            }
            sbWhere.AppendFormat(" AND IsDelete=0 ");
            return GetColList<Questionnaire>(int.MaxValue, 1, sbWhere.ToString()," QuestionnaireID DESC", "QuestionnaireID,QuestionnaireName");
        }
        /// <summary>
        /// 查询问题数
        /// </summary>
        /// <param name="questionnaireId"></param>
        /// <returns></returns>
        public int GetQuestionCount(int questionnaireId)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" QuestionnaireID={0} ",questionnaireId);
            return GetCount<Question>(sbWhere.ToString());
        }
        /// <summary>
        /// 获取答题记录
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="questionnaireSetID"></param>
        /// <param name="total"></param>
        /// <param name="returnTotal"></param>
        /// <returns></returns>
        public List<QuestionnaireRecord> GetRecord(int pageSize, int pageIndex, string questionnaireSetID, string userId,out int total , bool returnTotal = false)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("QuestionnaireSetID={0}", questionnaireSetID);
            if(!string.IsNullOrWhiteSpace(userId)) sbWhere.AppendFormat("AND UserId='{0}'", userId);

            total = returnTotal ? GetCount<QuestionnaireRecord>(sbWhere.ToString()) : 0;

            return GetLit<QuestionnaireRecord>(pageSize, pageIndex, sbWhere.ToString(), "AutoId desc");
        }
        /// <summary>
        /// 获取答题记录
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="userId"></param>
        /// <param name="questionnaireSetID"></param>
        /// <param name="questionnaireID"></param>
        /// <returns></returns>
        public List<QuestionnaireRecord> GetRecordList(int pageSize, int pageIndex, string userId, int? questionnaireSetID, int? questionnaireID)
        {
            return GetLit<QuestionnaireRecord>(pageSize, pageIndex, GetRecordSqlParam(userId, questionnaireSetID, questionnaireID), "AutoId desc");
        }
        /// <summary>
        /// 获取答题数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="questionnaireSetID"></param>
        /// <param name="questionnaireID"></param>
        /// <returns></returns>
        public int GetRecordCount(string userId,int? questionnaireSetID,int? questionnaireID)
        {
            return GetCount<QuestionnaireRecord>(GetRecordSqlParam(userId, questionnaireSetID, questionnaireID));
        }

        /// <summary>
        /// 获取用户是否有答题
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="questionnaireSetID"></param>
        /// <param name="questionnaireID"></param>
        /// <returns></returns>
        public bool ExistsRecordCount(string userId, int? questionnaireSetID, int? questionnaireID)
        {
            return Get<QuestionnaireRecord>(GetRecordSqlParam(userId, questionnaireSetID, questionnaireID))==null?false:true;
        }
        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="questionnaireSetID"></param>
        /// <param name="questionnaireID"></param>
        /// <returns></returns>
        public string GetRecordSqlParam(string userId,int? questionnaireSetID,int? questionnaireID)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" 1=1");
            if (!string.IsNullOrWhiteSpace(userId)) sbWhere.AppendFormat("AND UserId='{0}'", userId);
            if (questionnaireSetID.HasValue) sbWhere.AppendFormat(" AND QuestionnaireSetID={0} ", questionnaireSetID);
            if (questionnaireID.HasValue) sbWhere.AppendFormat(" AND QuestionnaireId={0} ", questionnaireID);
            return sbWhere.ToString();
        }
        /// <summary>
        /// 查询答题设置列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="title"></param>
        /// <param name="questionnaireId"></param>
        /// <returns></returns>
        public List<QuestionnaireSet> GetSetList(int pageSize, int pageIndex, string websiteOwner, string title, int? questionnaireId)
        {
            return GetLit<QuestionnaireSet>(pageSize, pageIndex, GetSetSqlParam( websiteOwner,  title, questionnaireId)," AutoID Desc ");
        }
        /// <summary>
        /// 查询答题设置数
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="title"></param>
        /// <param name="questionnaireId"></param>
        /// <returns></returns>
        public int GetSetCount(string websiteOwner, string title, int? questionnaireId)
        {
            return GetCount<QuestionnaireSet>(GetSetSqlParam(websiteOwner, title, questionnaireId));
        }
        /// <summary>
        /// 拼接查询语句
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="title"></param>
        /// <param name="questionnaireId"></param>
        /// <returns></returns>
        public string GetSetSqlParam(string websiteOwner, string title, int? questionnaireId)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" 1=1");
            if (!string.IsNullOrWhiteSpace(title)) sbWhere.AppendFormat(" AND Title Like '%{0}%' ", title);
            if (questionnaireId.HasValue) sbWhere.AppendFormat(" AND QuestionnaireId={0} ", questionnaireId);
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbWhere.AppendFormat("AND WebsiteOwner='{0}'", websiteOwner);
            return sbWhere.ToString();
        }


    }
}
