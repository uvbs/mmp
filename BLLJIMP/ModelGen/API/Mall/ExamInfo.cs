using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.Mall
{
    /// <summary>
    /// 考试信息
    /// </summary>
    public class ExamInfo
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string user_id { get; set; }
        /// <summary>
        /// 考试编号
        /// </summary>
        public int exam_id { get; set; }
        /// <summary>
        /// 0 尚未到考试时间
        /// 1 已经到考试时间,可以正常考试
        /// 2 已经考过了
        /// 3 缺考
        /// 4 未阅卷
        /// 5 已经阅卷 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 状态描述
        /// </summary>
        public string status_text { get; set; }


    }
 




}
