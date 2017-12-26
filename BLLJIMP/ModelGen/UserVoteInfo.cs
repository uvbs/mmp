using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    ///  用户投票记录
    /// </summary>
    public class UserVoteInfo : ZCBLLEngine.ModelTable
    {
        public UserVoteInfo() { }

        /// <summary>
        /// 编号
        /// </summary>
        public int AutoId { get; set; }

        /// <summary>
        /// 投票人编号
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 投票活动编号
        /// </summary>
        public string VoteId { get; set; }

        /// <summary>
        /// 投票内容
        /// </summary>
        public string DiInfoId { get; set; }
    }
}
