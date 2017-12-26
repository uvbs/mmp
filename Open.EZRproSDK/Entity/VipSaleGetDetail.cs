using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EZRproSDK.Entity
{
    public class VipSaleGetDetail
    {
        /// <summary>
        /// 商品条码
        /// </summary>
        public string ProdCode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProdName { get; set; }
        /// <summary>
        /// 售价(两位小数)
        /// </summary>
        public double SalePrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int SaleQty { get; set; }
        /// <summary>
        /// 金额(两位小数)
        /// </summary>
        public double SaleMoney { get; set; }
    }
}
