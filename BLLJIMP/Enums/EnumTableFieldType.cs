using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Enums
{
    /// <summary>
    /// 表字段映射 字段类型
    /// </summary>
    public enum EnumTableFieldType
    {
       /// <summary>
        /// 默认类型
        /// </summary>
        Default = 0,
        /// <summary>
        /// 卡券类型-门票
        /// </summary>
        CardCoupon_EntranceTicket = 1,
        Int = 2,
        String = 3,
        Email = 4,
        DateTime = 5,
        Date = 6
    }
}
