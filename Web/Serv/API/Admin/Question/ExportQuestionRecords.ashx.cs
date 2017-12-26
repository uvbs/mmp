using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Question
{
    /// <summary>
    /// Summary description for ExportQuestionRecords
    /// </summary>
    public class ExportQuestionRecords : BaseHandlerNeedLoginAdminNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            var QuestionnaireID = context.Request["questionnaire_id"];//问卷id

            //检查问卷是否属于当前站点，不属于则返回错误
            var questionnaire = bllUser.Get<BLLJIMP.Model.Questionnaire>(string.Format(" QuestionnaireID = {0} ",QuestionnaireID));
            if (questionnaire.WebsiteOwner != bllUser.WebsiteOwner && currentUserInfo.UserType != 1)
            {
                apiResp.status = false;
                apiResp.msg = "错误的请求";
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            StringBuilder sbExport = new StringBuilder();

            List<int> questionIndexList = new List<int>();

            //构造头部、问题id索引
            List<BLLJIMP.Model.Question> QuestionList = bllUser.GetList<BLLJIMP.Model.Question>(int.MaxValue, string.Format("QuestionnaireID={0}", QuestionnaireID), "Sort Asc");

            //用户id，用户微信昵称，提交时间，IP，IP解析地址
            sbExport.Append("用户ID\t");
            sbExport.Append("微信昵称\t");
            sbExport.Append("提交时间\t");
            sbExport.Append("IP\t");
            sbExport.Append("IP解析地址\t");
            questionIndexList.AddRange(new List<int> { -1,-2,-3,-4,-5});
            
            foreach (var item in QuestionList)
            {
                item.QuestionName = ZentCloud.Common.MyRegex.RemoveHTMLTags(item.QuestionName);                
                questionIndexList.Add(item.QuestionID);//添加id索引
                sbExport.AppendFormat("{0}\t", string.IsNullOrWhiteSpace(item.QuestionName)? "空标题": item.QuestionName);
            }
            
            sbExport.Append("\n");

            //var index = questionIndexList.FindIndex(p => p ==1);
            //根据答题记录构造数据(有些直接读取内容、有些需要根据anwserid去查找选项内容)

            var questionnaireRecordList = bllUser.GetList<BLLJIMP.Model.QuestionnaireRecord>(int.MaxValue,string.Format(" QuestionnaireID = {0} ",QuestionnaireID)," AutoId ASC ");

            List<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail> recordDetailListAll = bllUser.GetList<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format(" QuestionnaireID={0} ", QuestionnaireID));

            foreach (var item in questionnaireRecordList)
            {
                //初始化单行数据
                List<string> record = new List<string>();

                foreach (var questionIndex in questionIndexList)
                {
                    record.Add(" ");
                }

                //用户id，用户微信昵称，提交时间，IP，IP解析地址
                var recordUser = bllUser.GetUserInfo(item.UserId);

                if (recordUser == null)
                {
                    continue;
                }


                record[0] = recordUser.AutoID.ToString();
                record[1] = string.IsNullOrWhiteSpace(recordUser.WXNickname) ? "" : recordUser.WXNickname;

                record[2] = item.InsertDate.ToString();
                record[3] = item.IP;
                string ipLocation = string.Empty;
                try
                {
                    ipLocation = ZentCloud.Common.MySpider.GetIPLocation(item.IP);
                }
                catch { }

                record[4] = string.IsNullOrWhiteSpace(ipLocation) ? "" : ipLocation;
                
                //每一个答题项构造
                foreach (var question in QuestionList)
                {
                    string questionResult = "";
                    
                    var recordDetailList = recordDetailListAll.Where(p => 
                        p.QuestionID == question.QuestionID
                        &&
                        p.UserID == recordUser.UserID
                        &&
                        p.RecordID == item.RecordID
                    ).ToList();

                    if (recordDetailList != null && recordDetailList.Count > 0)
                    {
                        switch (question.QuestionType)
                        {
                            case 0://单选
                                {
                                    var answer = bllUser.Get<ZentCloud.BLLJIMP.Model.Answer>(string.Format("AnswerID={0}", recordDetailList[0].AnswerID));

                                    if (answer != null)
                                    {
                                        questionResult = answer.AnswerName;
                                    }
                                }
                                break;
                            case 1://多选
                                {
                                    for (int i = 0; i < recordDetailList.Count; i++)
                                    {
                                        var answer = bllUser.Get<ZentCloud.BLLJIMP.Model.Answer>(string.Format("AnswerID={0}", recordDetailList[i].AnswerID));
                                        if (answer != null)
                                        {
                                            questionResult += answer.AnswerName + "； ";
                                        }
                                    }
                                }
                                break;
                            case 2://填空
                                questionResult = recordDetailList[0].AnswerContent;
                                break;
                            case 3://分组
                                {
                                    for (int i = 0; i < recordDetailList.Count; i++)
                                    {
                                        var answer = bllUser.Get<ZentCloud.BLLJIMP.Model.Answer>(string.Format("AnswerID={0}", recordDetailList[i].AnswerID));
                                        if (answer != null)
                                        {
                                            questionResult += string.Format("（{1}）{0}；",answer.AnswerName, recordDetailList[i].AnswerContent);
                                        }
                                    }
                                }
                                break;
                            case 4://省市区地址
                                questionResult = recordDetailList[0].AnswerContent;
                                break;
                            case 5://时间
                                questionResult = recordDetailList[0].AnswerContent;
                                break;
                            default:
                                questionResult = recordDetailList[0].AnswerContent;
                                break;
                        }
                    }

                    var index = questionIndexList.FindIndex(p => p ==question.QuestionID);

                    record[index] = questionResult;
                }

                foreach (var recordItem in record)
                {
                    sbExport.Append(recordItem + "\t");
                }

                sbExport.Append("\n");
            }

            context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            context.Response.AppendHeader("Content-Disposition", string.Format("attachment;filename=问卷调查_{1}_{0}.xls", questionnaire.QuestionnaireID, questionnaire.QuestionnaireName));
            context.Response.Write(sbExport);
        }
    }
}