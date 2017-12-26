using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 调查问卷-问题表
    /// </summary>
    public partial class Question : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 问题编号
        /// </summary>
        public int QuestionID { get; set; }
        /// <summary>
        /// 问题名称
        /// </summary>
        public string QuestionName { get; set; }
        /// <summary>
        /// 问题类型 0代表单选1代表多选2代表填空
        /// </summary>
        public int QuestionType { get; set; }
        /// <summary>
        /// 问卷编号 对应ZCJ_Questionnaire表的QuestionnaireID
        /// </summary>
        public int QuestionnaireID { get; set; }
        /// <summary>
        /// 是否必填 0否1必填
        /// </summary>
        public int IsRequired { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string AnswerGroupName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
