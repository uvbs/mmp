using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    ///限时特卖活动
    /// </summary>
    public class PromotionActivity : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 活动Id
       /// </summary>
       public int ActivityId { get; set; }
       /// <summary>
       /// 限时特卖活动名称
       /// </summary>
       public string ActivityName { get; set; }
       /// <summary>
       ///描述
       /// </summary>
       public string ActivitySummary { get; set; }
       /// <summary>
       /// 活动图片
       /// </summary>
       public string ActivityImage { get; set; }
       /// <summary>
       /// 开始时间
       /// </summary>
       public double StartTime { get; set; }
       /// <summary> 
       /// 结束时间
       /// </summary>
       public double StopTime { get; set; }
       /// <summary>
       /// 插入时间
       /// </summary>
       public DateTime InsertDate { get; set; }
       /// <summary>
       /// 站点所有者
       /// </summary>
       public string WebSiteOwner { get; set; }
        /// <summary>
        /// 排序号,越小越靠前显示
        /// </summary>
       public int Sort { get; set; }
        /// <summary>
        /// 限制购买商品数量
        /// </summary>
       public int LimitBuyProductCount { get; set; }

        /// <summary>
        /// 特卖类型
        /// </summary>
       public string PromotionActivityType { get; set; }

        /// <summary>
        /// 特卖配置
        /// </summary>
       public string PromotionActivityRule { get; set; }


    }
}
