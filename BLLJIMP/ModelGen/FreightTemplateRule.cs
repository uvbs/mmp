using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 运费模板规则表
    /// </summary>
    public class FreightTemplateRule : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary> 
        public int AutoId { get; set; }
        /// <summary>
        /// 模板id 关联 ZCJ_FreightTemplate TemplateId
        /// </summary>
        public int TemplateId { get; set; }
        /// <summary>
        /// 省市区代码 多个代码用逗号分隔
        /// </summary>
        public string AreaCodes { get; set; }
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
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

    }
}
