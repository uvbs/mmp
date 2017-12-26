using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 用户关注信息表
    /// </summary>
    [Serializable]
    public partial class UserFollowChain : ZCBLLEngine.ModelTable
    {
        public UserFollowChain()
        {

        }

        /// <summary>
        /// 自动编号
        /// </summary>
        public int? AutoId { get; set; }

        /// <summary>
        /// 关注者
        /// </summary>
        public string FromUserId { get; set; }

        /// <summary>
        /// 被关注者
        /// </summary>
        public string ToUserId { get; set; }

        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        
    }
}
