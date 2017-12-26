using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 退款协商记录表
    /// </summary>
    public class WXMallRefundLog : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 日志编号 
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 退款编号
        /// </summary>
        public string RefundId { get; set; }
        /// <summary>
        /// 订单详情ID
        /// </summary>
        public int OrderDetailId { get; set; }
        /// <summary>
        /// 角色 商家或买家
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string LogContent { get; set; }
        /// <summary>
        ///日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }

    }
}
