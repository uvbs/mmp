using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Enums
{
    /// <summary>
    /// 订单类型
    /// </summary>
    public enum OrderType
    {

        /// <summary>
        /// 普通订单
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 礼品订单
        /// </summary>
        Gift = 1,
        /// <summary>
        /// 拼团订单
        /// </summary>
        GroupBuy = 2,
        /// <summary>
        /// 预约订单
        /// </summary>
        Booking = 3,
        /// <summary>
        /// 付费活动订单
        /// </summary>
        Activity = 4,
        /// <summary>
        /// 医生预约
        /// </summary>
        Master = 5,
        /// <summary>
        /// 医生预约(推荐)
        /// </summary>
        MasterRecommend = 6,
        /// <summary>
        ///课程订单
        /// </summary>
        Course=7
     
    }
}
