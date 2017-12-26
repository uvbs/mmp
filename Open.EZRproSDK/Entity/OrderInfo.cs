using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EZRproSDK.Entity
{
    public class OrderInfo
    {
        /// <summary>
        /// 门店代码
        /// </summary>
        public string ShopCode { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string Code { get; set; }
        ///// <summary>
        ///// 关联单号(退货单关联)
        ///// </summary>
        //public string RefSaleNo { get; set; }
        ///// <summary>
        ///// 订单状态:S=销售、R=退货
        ///// </summary>
        //public string SaleType { get; set; }
        ///// <summary>
        ///// 会员线下卡号
        ///// </summary>
        //public string VipOffCode { get; set; }
        /// <summary>
        /// 销售日期(yyyy-MM-dd HH:mm:ss格式)
        /// </summary>
        public string OrderTime { get; set; }
        /// <summary>
        /// 订单总数量
        /// </summary>
        public int TotalQty { get; set; }
        /// <summary>
        /// 订单总金额(2位小数)
        /// </summary>
        public double TotalMoney { get; set; }
        public bool IsPayed { get; set; }
        public string PayTime { get; set; }
        public double PayAmount { get; set; }
        /// <summary>
        /// 买家账号 或会员线上卡号
        /// </summary>
        public string BuyerCode { get; set; }
        ///// <summary>
        ///// 销售商品品项数
        ///// </summary>
        //public int SaleProdQty { get; set; }
        ///// <summary>
        ///// 平均折扣率(2位小数)(0-100)
        ///// </summary>
        //public double SaleDiscount { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// 订单状态变更时间
        /// </summary>
        public string StatusTime { get; set; }
        /// <summary>
        /// 分销员编号
        /// </summary>
        public long SellerId { get; set; }

        /// <summary>
        /// 优惠金额(2位小数)
        /// </summary>
        public double DiscountMoney { get; set; }

        /// <summary>
        /// 数据来源 1=微商城 2=线上官网 5=淘宝 6=京东
        /// </summary>
        public int DataOrigin { get; set; }

        /// <summary>
        /// 邮费
        /// </summary>
        public double ExpressFee { get; set; }

        ///// <summary>
        ///// 抵现积分
        ///// </summary>
        //public string UseBonus { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        public List<Entity.OrderDetail> Dtls { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string RecvConsignee { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string RecvMobile { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string RecvTel { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string RecvAddress { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string RecvProvince { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string RecvCity { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string RecvCounty { get; set; }


    }
}
