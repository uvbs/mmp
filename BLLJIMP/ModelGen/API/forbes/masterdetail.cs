using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{
    /// <summary>
    /// 理财师详细信息模型
    /// </summary>
    public class MasterDetail:MasterBase
    {
        /// <summary>
        /// 是否已经关注 true已关注 false 未关注
        /// </summary>
        public bool isattention { get; set; }
        /// <summary>
        /// 关注数量
        /// </summary>
        public int attentioncount { get; set; }
        /// <summary>
        /// 粉丝数量
        /// </summary>
        public int fanscount { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string company { get; set; }
        /// <summary>
        /// 理财师详细介绍
        /// </summary>
        public string introduction { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 咨询数量
        /// </summary>
        public int askcount { get; set; }
        /// <summary>
        /// 被咨询数量
        /// </summary>
        public int beaskcount { get; set; }
        /// <summary>
        /// 是否可以咨询该理财师 或关注该理财师
        /// </summary>
        public bool canaskorattention { get; set; }

    }


}
