using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EZRproSDK.Entity
{
    public class RefundInfo
    {
        /// <summary>
        /// 商品条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 退货数量
        /// </summary>
        public int ReturnQuantity { get; set; }
        /// <summary>
        /// 退货单价(2位小数)
        /// </summary>
        public double ReturnPrice { get; set; }
        /// <summary>
        /// 退货金额 
        /// </summary>
        public decimal ReturnMoney { get; set; }

    }
}
