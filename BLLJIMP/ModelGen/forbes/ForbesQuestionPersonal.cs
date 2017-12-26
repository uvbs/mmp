using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Forbes
{
    /// <summary>
    /// 福布斯个人答题记录
    /// </summary>
    [Serializable]
    public partial class ForbesQuestionPersonal : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 第几次答题
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        ///题目Id
        /// </summary>
        public int QuestionId { get; set; }
        /// <summary>
        /// 题目状态 0未答题 1已经答题
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否答对了 0 答错了 1答对了
        /// </summary>
        public int IsCorrect { get; set; }
        
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public int Score { get; set; }
        
        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; }
        
    }
}
