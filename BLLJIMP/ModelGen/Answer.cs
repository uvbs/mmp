using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 调查问卷-问题选项表
    /// </summary>
    public partial class Answer : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 选项编号
        /// </summary>
        public int AnswerID { get; set; }
        /// <summary>
        /// 选项名称
        /// </summary>
        public string AnswerName { get; set; }
        /// <summary>
        /// 问题编号 对应ZCJ_Question 表QuestionID
        /// </summary>
        public int QuestionID { get; set; }
        /// <summary>
        /// 问卷编号对应 ZCJ_Questionnaire表的QuestionnaireID
        /// </summary>
        public int QuestionnaireID { get; set; }
        /// <summary>
        /// 是否是正确答案
        /// </summary>
        public int IsCorrect { get; set; }
    }
}
