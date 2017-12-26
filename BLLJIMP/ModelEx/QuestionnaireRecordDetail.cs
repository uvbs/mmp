using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class QuestionnaireRecordDetail
    {
        public QuestionnaireRecordDetail(){}

        public QuestionnaireRecordDetail(string userID, int questionnaireID, int questionID, int answerID, string answerContent, int questionnaireSetID)
        {
            UserID = userID;
            QuestionnaireID = questionnaireID;
            QuestionID = questionID;
            AnswerID = answerID;
            QuestionnaireSetID = questionnaireSetID;
            if (!string.IsNullOrWhiteSpace(answerContent)) AnswerContent = answerContent;
        }
    }
}
