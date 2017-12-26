using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{
    /// <summary>
    /// 积分模型
    /// </summary>
    public class ScoreRecord
    {
        /// <summary>
        /// 积分记录标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 获得或减掉的积分
        /// </summary>
        public string score { get; set; }
        /// <summary>
        /// 记录时间戳
        /// </summary>
        public double time { get; set; }
        /// <summary>
        /// 积分记录类型 0代表回答话题加分1代表申请理财师加分
        /// </summary>
        public int type { get; set; }



    }
    /// <summary>
    /// 积分记录api模型
    /// </summary>
    public class ScoreRecordApi
    {

        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 总积分
        /// </summary>
        public int totalscore { get; set; }
        /// <summary>
        /// 分类集合
        /// </summary>
        public List<ScoreRecord> list { get; set; }
    }


}
