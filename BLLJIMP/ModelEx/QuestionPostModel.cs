using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class QuestionPostModel
    {
        /// <summary>
        /// 问题ID
        /// </summary>
        public int QuestionID { get; set; }
        /// <summary>
        /// 选项ID，多选时用|分隔
        /// 填空题内容
        /// </summary>
        public string Answer { get; set; }
    }
}
