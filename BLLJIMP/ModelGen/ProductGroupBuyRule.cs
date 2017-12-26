using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 拼团规则表
    /// </summary>
    public class ProductGroupBuyRule : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 规则ID
        /// </summary>
        public int RuleId { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName{get;set;}
        /// <summary>
        /// 团长折扣0-10
        /// </summary>
        public decimal HeadDiscount { get; set; }
        /// <summary>
        /// 会员折扣0-10
        /// </summary>
        public decimal MemberDiscount { get; set; }
        /// <summary>
        /// 拼团人数
        /// </summary>
        public int PeopleCount { get; set; }
        /// <summary>
        /// 过期天数 
        /// </summary>
        public int ExpireDay { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime ModifyDate { get; set; }
        /// <summary>
        ///站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        ///操作人
        /// </summary>
        public string Operator { get; set; }
    }
}
