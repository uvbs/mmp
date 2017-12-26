using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 供应商库存 适用于同一商品各个门店不同库存
    /// </summary>
    public partial class ProductSkuSupplier : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// SKU 编号
        /// </summary>
        public int SkuId { get; set; }
        /// <summary>
        ///商品ID
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// sku基础价，如果是0，则读取商品的总基础价
        /// </summary>
        public decimal BasePrice { get; set; }
        /// <summary>
        /// 库存 
        /// </summary>
        public int Stock { get; set; }
        /////<summary>
        /////特征量特征值组合 
        ///// 格式 商品属性id:商品属性值id:商品属性名称:商品属性值名称 多个组合用;分隔 
        ///// 示例 1:1:尺码:S;2:5:颜色:蓝色
        /////</summary>
        //public string Props { get; set; }
        ///// <summary>
        ///// 属性 显示名称 示例 尺码:XS;颜色:红色;
        ///// </summary>
        //public string ShowProps { get; set; }
        /// <summary>
        /// SKU 编码
        /// </summary>
        public string SkuSN { get; set; }
        /// <summary>
        /// 供应商Id
        /// </summary>
        public string SupplierId { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? Modified { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }





    }
}