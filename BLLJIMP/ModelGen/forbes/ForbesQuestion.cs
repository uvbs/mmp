using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Forbes
{
    /// <summary>
    /// 福布斯题目
    /// </summary>
    [Serializable]
    public partial class ForbesQuestion : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        ///类别
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 一级知识点
        /// </summary>
        public string KnowledgeLv1 { get; set; }
        /// <summary>
        /// 二级知识点
        /// </summary>
        public string KnowledgeLv2 { get; set; }
        /// <summary>
        /// 问题
        /// </summary>
        public string Question { get; set; }
        /// <summary>
        /// 答案A
        /// </summary>
        public string AnswerA { get; set; }
        /// <summary>
        /// 答案B
        /// </summary>
        public string AnswerB { get; set; }
        /// <summary>
        /// 答案C
        /// </summary>
        public string AnswerC { get; set; }
        /// <summary>
        /// 答案D
        /// </summary>
        public string AnswerD { get; set; }
        /// <summary>
        /// 答案E
        /// </summary>
        public string AnswerE { get; set; }
        /// <summary>
        /// 答案F
        /// </summary>
        public string AnswerF { get; set; }
        /// <summary>
        /// 答案G
        /// </summary>
        public string AnswerG { get; set; }

        /// <summary>
        /// 正确答案
        /// </summary>
       public string  CorrectAnswer{get;set;}

       /// <summary>
       /// 正确答案 A B C D E F G
       /// </summary>
       public string CorrectAnswerCode { get; set; }

       /// <summary>
       /// 站点
       /// </summary>
       public string WebsiteOwner { get; set; }

        /// <summary>
        /// 分值
        /// </summary>
       public int Score { get; set; }

    }
}
