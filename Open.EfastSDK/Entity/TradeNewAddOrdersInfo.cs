using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 创建订单-商品明细
    /// </summary>
    public class TradeNewAddOrdersInfo
    {
        //商品明细 outer_sku：匹配条码 goods_name：商品名称 goods_number：数量 goods_price：价格 payment_ft: 商
        //品分摊价 is_gift：是否礼品
        //0=>array('outer_sku'=>'zzm00105000','goods_name'=>'zzm','goods_number'=>1,'goods_price'=>60,
        //'payment_ft'=>60,’is_gift’=>1)

        /// <summary>
        /// 匹配条码
        /// </summary>
        public string outer_sku { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string goods_name { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int goods_number { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public double goods_price { get; set; }
        /// <summary>
        /// 商品分摊价
        /// </summary>
        public double payment_ft { get; set; }
        /// <summary>
        /// 是否礼品
        /// </summary>
        public int is_gift { get; set; }


    }
}
