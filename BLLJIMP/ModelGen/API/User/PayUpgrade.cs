using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.User
{
    [Serializable]
    public class PayUpgrade
    {
        public int level { get; set; }
        public int toLevel { get; set; }
        public decimal amount { get; set; } //补充金额
        public decimal userTotalAmount { get; set; } //原会员级别金额
        public decimal needAmount { get; set; } //会员级别需要金额
        public string vType { get; set; }//优惠券类V1 V2

    }
}
