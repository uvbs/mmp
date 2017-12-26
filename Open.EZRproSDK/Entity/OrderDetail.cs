using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EZRproSDK.Entity
{
    public class OrderDetail
    {
        ///// <summary>
        ///// 商品条码
        ///// </summary>
        //public string ProdCode { get; set; }
        ///// <summary>
        ///// 源系统唯一系统商品编号
        ///// </summary>
        //public string ProdUID { get; set; }
        ///// <summary>
        ///// 零售单价(2位小数)
        ///// </summary>
        //public double RetailPrice { get; set; }
        ///// <summary>
        ///// 实际售价(2位小数)
        ///// </summary>
        //public double SalePrice { get; set; }
        ///// <summary>
        ///// 销售件数,负数代表退货，不可以为0
        ///// </summary>
        //public int SaleQty { get; set; }
        ///// <summary>
        ///// 销售金额(2位小数)
        ///// </summary>
        //public double SaleMoney { get; set; }
        ///// <summary>
        ///// 导购工号
        ///// </summary>
        //public string SalerCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BarCode { get; set; }
        /// <summary>
        /// 原始单价(2位小数)
        /// </summary>
        public double PriceOriginal { get; set; }
        /// <summary>
        /// 实际售价(2位小数)
        /// </summary>
        public double PriceSell { get; set; }
        /// <summary>
        /// 订购数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 订购金额(2位小数)
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// 折扣金额(2位小数)
        /// </summary>
        public double DiscountMoney { get; set; }
        /// <summary>
        /// 是否赠品
        /// </summary>
        public bool IsGift { get; set; }

    }
}
