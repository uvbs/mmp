using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EZRproSDK.Entity
{
    /// <summary>
    /// 商品上下架请求模型
    /// </summary>
    public class ProductSale
    {
        /// <summary>
        /// 商品货号代码
        /// </summary>
        public string ItemNo { get; set; }
        /// <summary>
        /// 是否上架
        /// 1 上架
        /// 0 下架
        /// </summary>
        public string IsOnSale { get; set; }
    }
}
