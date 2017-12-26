using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 购物车
    /// </summary>
    public  class ShoppingCart : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 购物车ID
        /// </summary>
        public int CartId { get; set; } 
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// sku 编号
        /// </summary>
        public int SkuId { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 商品标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// SKU的值，即：商品的规格。如：颜色:黑色;尺码:M
        /// </summary>
        public string SkuPropertiesName { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 吊牌价
        /// </summary>
        public decimal QuotePrice { get; set; }
        /// <summary>
        ///单价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 交易明细内的优惠金额。精确到2位小数，单位：元
        /// </summary>
        public decimal DiscountFee { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public decimal TotalFee { get; set; }
        /// <summary>
        /// 运费方式，seller（卖家承担即包邮），buyer(买家承担）
        /// </summary>
        public string FreightTerms { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }

        /// <summary>
        /// 商品标签
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// 供应商Id
        /// </summary>
        public string SupplierId { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 门店地址
        /// </summary>
        public string StoreAddress { get; set; }

    }
}
