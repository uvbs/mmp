using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 快递查询结果
    /// </summary>
    public class ExpressResult : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 自动编号
       /// </summary>
       public int AutoId { get; set; }
       /// <summary>
       /// 快递单号
       /// </summary>
       public string ExpressNumber { get; set; }
       /// <summary>
       /// 快递公司代码
       /// </summary>
       public string ExpressCompanyCode { get; set; }
        /// <summary>
        /// 快递公司名称
        /// </summary>
       public string ExpressCompanyName { get; set; }
        /// <summary>
        /// 快递查询结果
        /// </summary>
       public string ExpressContent { get; set; }
       /// <summary>
       /// 创建时间
       /// </summary>
       public DateTime InsertDate { get; set; }
       /// <summary>
       /// 最后更新时间
       /// </summary>
       public DateTime LastUpdateDate { get; set; }
       /// <summary>
       /// 站点所有者
       /// </summary>
       public string WebsiteOwner { get; set; }
        /// <summary>
       /// /*监控状态:polling:监控中，shutdown:结束，abort:中止，updateall：重新推送。其中当快递单为已签收时status=shutdown，当message为“3天查询无记录”或“60天无变化时”status= abort ，对于stuatus=abort的状度，需要增加额外的处理逻辑，详见本节最后的说明 */
        /// </summary>
       public string Status { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
       public string Message { get; set; }
        /// <summary>
        /// 结果JSON字符串
        /// </summary>
       public string ResultJson { get; set; }

    }
}
