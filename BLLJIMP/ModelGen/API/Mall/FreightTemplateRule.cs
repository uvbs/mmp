using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.Mall
{
    /// <summary>
    /// 运费规则 用于下单时运费计算
    /// </summary>
   public class FreightTemplateRuleModel
    {
       /// <summary>
       /// 规则ID
       /// </summary>
       public int RuleId { get; set; }

        /// <summary>
        /// 模板id 关联 FreightTemplate TemplateId
        /// </summary>
        public int TemplateId { get; set; }

        /// <summary>
        /// 省市区代码 集合
        /// </summary>
        public List<string> AreaCodeList { get; set; }

        /// <summary>
        /// 首件 
        /// 首重
        /// </summary>
        public decimal InitialProductCount { get; set; }
        /// <summary>
        /// 首费
        /// </summary>
        public decimal InitialAmount { get; set; }
        /// <summary>
        /// 续件
        /// 续重
        /// </summary>
        public decimal AddProductCount { get; set; }
        /// <summary>
        /// 续费
        /// </summary>
        public decimal AddAmount { get; set; }

    }
}
