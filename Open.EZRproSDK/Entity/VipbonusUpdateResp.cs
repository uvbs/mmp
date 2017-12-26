using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EZRproSDK.Entity
{
    public class VipbonusUpdateResp
    {
        /// <summary>
        /// 电子卡号
        /// </summary>
        public string VipCode { get; set; }
        /// <summary>
        /// 可用积分
        /// </summary>
        public int Bonus { get; set; }
    }
}
