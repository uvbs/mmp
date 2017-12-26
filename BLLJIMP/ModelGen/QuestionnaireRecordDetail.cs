using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 问卷提交记录表-详细
    /// </summary>
    public partial class QuestionnaireRecordDetail : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }

        /// <summary>
        /// 记录ID
        /// </summary>
        public long RecordID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 问卷编号对应ZCJ_Questionnaire表的QuestionnaireID
        /// </summary>
        public int QuestionnaireID { get; set; }
        /// <summary>
        /// 问题编号 对应ZCJ_Question 表QuestionID
        /// </summary>
        public int QuestionID { get; set; }
        /// <summary>
        /// 问题选项编号 对应ZCJ_Answer表AnswerID
        /// </summary>
        public int? AnswerID { get; set; }
        /// <summary>
        /// 答案(用于记录)
        /// </summary>
        public string AnswerContent { get; set; }
        /// <summary>
        /// 答题设置id
        /// </summary>
        public int QuestionnaireSetID { get; set; }

    }
}
