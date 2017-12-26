using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.MQ
{
    /// <summary>
    /// 新分销会员通知
    /// </summary>
    [Serializable]
    public class DistNewMemberNoticeInfo
    {
        /// <summary>
        /// 分销员autoid
        /// </summary>
        public string DistributionOwnerAutoId { get; set; }
        /// <summary>
        /// 新会员autoid
        /// </summary>
        public string MemberAutoId { get; set; }
    }
}
