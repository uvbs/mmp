using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 用户分销会员关系表
    /// </summary>
    [Serializable]
    public class UserDistributionMemberInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        public string MemberId { get; set; }
        /// <summary>
        /// 所属站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertDate { get; set; }


        #region ModelEx

        /// <summary>
        /// 行号 排名
        /// </summary>
        public long RowNumber { get; set; } 

        #endregion


    }
}
