using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EZRproSDK.Entity
{
    /// <summary>
    /// 获取会员积分响应实体
    /// </summary>
    public class BonusGetResp
    {
        /// <summary>
        /// 线下会员卡号
        /// </summary>
        public string OldCode { get; set; }
        /// <summary>
        /// 电子卡号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string MobileNo { get; set; }
        /// <summary>
        /// 可用积分
        /// </summary>
        public int Bonus { get; set; }
        /// <summary>
        /// 累计积分
        /// </summary>
        public int BonusTotal { get; set; }
    }
}
