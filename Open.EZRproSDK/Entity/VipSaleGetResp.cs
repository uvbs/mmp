using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EZRproSDK.Entity
{
    public class VipSaleGetResp
    {
        /// <summary>
        /// 订单系统号
        /// </summary>
        public long SaleId { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string SaleNo { get; set; }
        /// <summary>
        /// 店铺代码
        /// </summary>
        public string ShopCode { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 关联单号(退货)
        /// </summary>
        public string RefSaleNo { get; set; }
        /// <summary>
        /// 会员线上卡号
        /// </summary>
        public string VipCode { get; set; }
        /// <summary>
        /// 会员线下卡号
        /// </summary>
        public string VipOffCode { get; set; }
        /// <summary>
        /// 订单交易日期
        /// </summary>
        public string SaleDate { get; set; }
        /// <summary>
        /// 订单交易数量
        /// </summary>
        public int SaleQty { get; set; }
        /// <summary>
        /// 订单交易金额
        /// </summary>
        public double SaleMoney { get; set; }
        /// <summary>
        /// 订单支付金额(两位小数)
        /// </summary>
        public double SalePayMoney { get; set; }
        /// <summary>
        /// 数据来源
        /// </summary>
        public short DataOrigin { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string Timestamp { get; set; }

        public List<VipSaleGetDetail> Dtls { get; set; }
    }
}
