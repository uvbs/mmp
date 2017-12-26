using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 积分配置
    /// </summary>
    public class ScoreConfig : ZCBLLEngine.ModelTable
    {   
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 积分规则
        /// </summary>
        public string ScoreRule { get; set; }
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 指定订单日期(用于计算指定日期获得的积分) 旧
        /// </summary>
        public DateTime? OrderDate { get; set; }
        /// <summary>
        /// 订单金额 旧
        /// </summary>
        public decimal? OrderDateTotalAmount { get; set; }
        /// <summary>
        /// 订单金额 获得积分
        /// </summary>
        public int? OrderScore { get; set; }
        /// <summary>
        /// 兑换积分
        /// </summary>
        public int ExchangeScore { get; set; }
        /// <summary>
        /// 兑换积分可以兑换的金额
        /// </summary>
        public decimal ExchangeAmount { get; set; } 
        
        /// <summary>
        /// 订单金额 订单消费多少元获得多少积分 新
        /// </summary>
        public int OrderAmount { get; set; }


    }
}
