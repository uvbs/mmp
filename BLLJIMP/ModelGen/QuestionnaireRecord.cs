using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 问卷调查记录表(只记录用户名，问卷编号,日期)
    /// </summary>
    public class QuestionnaireRecord : ZCBLLEngine.ModelTable
    {
        public int AutoId { get; set; }
        /// <summary>
        /// 记录ID
        /// </summary>
        public long RecordID { get; set; }
       /// <summary>
       /// 用户名
       /// </summary>
       public string UserId { get; set; }
       /// <summary>
       /// 问卷编号对应ZCJ_Questionnaire表的QuestionnaireID
       /// </summary>
       public int QuestionnaireID { get; set; }
        /// <summary>
        /// IP
        /// </summary>
       public string IP { get; set; }
       /// <summary>
       /// 提交时间
       /// </summary>
       public DateTime InsertDate { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
       public string WebsiteOwner { get; set; }
       /// <summary>
       /// 正确数
       /// </summary>
       public int CorrectCount { get; set; }
       /// <summary>
       /// 答题数
       /// </summary>
       public int AnswerCount { get; set; }
        /// <summary>
        /// 答题设置ID
        /// </summary>
        public int QuestionnaireSetID { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string WXNickname { get; set; }
        /// <summary>
        /// 微信头像
        /// </summary>
        public string WXHeadimgurl { get; set; }

        /// <summary>
        /// /推广人id
        /// </summary>
        public string PreUserId { get; set; }

        /// <summary>
        /// 考试结果
        /// </summary>
        public string Result { get; set; }
        
    }
}
