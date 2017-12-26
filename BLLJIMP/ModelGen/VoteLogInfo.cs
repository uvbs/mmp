using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 投票记录表
    /// </summary>
    public partial class VoteLogInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 投票标识 关联 VoteInfo AutoID
        /// </summary>
        public int VoteID { get; set; }
        /// <summary>
        /// 投票对象ID 关联 VoteObjectInfo AutoID
        /// </summary>
        public int VoteObjectID { get; set; }
        /// <summary>
        /// 投票数
        /// </summary>
        public int VoteCount { get; set; }
        /// <summary>
        /// 参加投票的用户的登录名
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 投票时间
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// 创建者用户名
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// IP所在地
        /// </summary>
        public string IPLocation { get; set; }

    }
}
