using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.MQ
{
    /// <summary>
    /// 商品访问统计
    /// </summary>
    public class ShopDetailOpenStatistics : WebStatisticsBase
    {
        /// <summary>
        /// 商品id，对应赋值给 MonitorPlanID
        /// </summary>
        public int ProductId { get; set; }

    }
}
