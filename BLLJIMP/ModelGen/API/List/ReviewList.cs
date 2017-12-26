using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.List
{
    [Serializable]
    public class ReviewList
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public List<ReviewInfo> List { get; set; }
    }
}
