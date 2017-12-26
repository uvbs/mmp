using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{
    public class ScoreRank
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 集合
        /// </summary>
        public List<UserInfoModel> list { get; set; }
    }
}
