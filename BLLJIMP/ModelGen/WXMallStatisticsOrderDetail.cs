using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    ///订单统计明细
    /// </summary>
    public class WXMallStatisticsOrderDetail : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        ///任务Id
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// All               全部订单
        /// ShouldCommission  应该分佣订单订单数
        /// RealCommission    实际分佣订单订单数
        /// Refund            订单交易成功且在退款中
        /// WaitProcess       待处理
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }


    }
}
