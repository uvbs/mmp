using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 评论配置
    /// </summary>
    public class ReviewConFig : ZCBLLEngine.ModelTable
    {
        public ReviewConFig() { }

        public int AutoId { get; set; }

        /// <summary>
        /// 投票
        /// </summary>
        public string VoteId { get; set; }

        /// <summary>
        /// 文章
        /// </summary>
        public string Article { get; set; }

        /// <summary>
        /// 活动
        /// </summary>
        public string Activity { get; set; }


    }
}
