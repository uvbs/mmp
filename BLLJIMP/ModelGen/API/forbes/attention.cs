using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{

    /// <summary>
    /// 关注api模型
    /// </summary>
    public class AttentionApi
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 集合
        /// </summary>
        public List<AttentionUserinfo> list { get; set; }
    }

    /// <summary>
    /// 关注的人的信息
    /// </summary>
    public class AttentionUserinfo:MasterBase {
        /// <summary>
        /// 是否是理财师 true 是 false 否
        /// </summary>
        public bool ismaster;
        /// <summary>
        /// 关注我的人 我是否关注TA
        /// </summary>
        public bool isattention { get; set; }

    }
}
