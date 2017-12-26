using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 商品价格配置表
    /// </summary>
    public class ProductPriceConfig : ZCBLLEngine.ModelTable
    {

        public int AutoId { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// sku ID
        /// </summary>
        public string SkuId { get; set; }
        /// <summary>
        /// 日期 格式 2016/05/30
        /// </summary>
        public string Date{get;set;}
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }



    }
}
