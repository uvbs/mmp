using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.TakeOutNotify.Model
{
    /// <summary>
    /// 订单实体
    /// </summary>
    public class OOrder
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 订单id
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 送餐地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public string createdAt { get; set; }
        /// <summary>
        /// 订单生效时间
        /// </summary>
        public string activeAt { get; set; }
        /// <summary>
        /// 配送费
        /// </summary>
        public double? deliverFee { get; set; }
        /// <summary>
        /// 预计送达时间
        /// </summary>
        public string deliverTime { get; set; }
        /// <summary>
        /// 订单备注
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 订单详细类目的列表
        /// </summary>
        public List<OGoodsGroup> groups { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string invoice { get; set; }
        /// <summary>
        /// 是否预订单
        /// </summary>
        public bool book { get; set; }
        /// <summary>
        /// 是否在线支付
        /// </summary>
        public bool onlinePaid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string railwayAddress { get; set; }
        /// <summary>
        /// 顾客联系电话
        /// </summary>
        public List<string> phoneList { get; set; }
        /// <summary>
        /// 店铺id
        /// </summary>
        public long shopId { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string shopName { get; set; }
        /// <summary>
        /// 店铺当日订单流水号
        /// </summary>
        public int daySn { get; set; }
        /// <summary>
        /// 订单状态  pending未生效订单unprocessed未处理订单refunding退单处理中valid已处理的有效订单invalid无效订单settled已完成订单
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 退款状态noRefund未申请退单applied用户申请退单rejected店铺拒绝退单arbitrating客服仲裁中failed退单失败successful退单成功
        /// </summary>
        public string refundStatus { get; set; }
        /// <summary>
        /// 下单用户id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 订单总价，用户实际支付的金额，单位：元
        /// </summary>
        public double? totalPrice { get; set; }
        /// <summary>
        /// 订单原始价格
        /// </summary>
        public double? originalPrice { get; set; }
        /// <summary>
        /// 单收货人姓名
        /// </summary>
        public string consignee { get; set; }
        /// <summary>
        /// 订单收货地址经纬度
        /// </summary>
        public string deliveryGeo { get; set; }
        /// <summary>
        /// 送餐地址
        /// </summary>
        public string deliveryPoiAddress { get; set; }
        /// <summary>
        /// 顾客是否需要发票
        /// </summary>
        public bool invoiced { get; set; }
        /// <summary>
        /// 店铺实收
        /// </summary>
        public double? income { get; set; }
        /// <summary>
        /// 饿了么服务费率
        /// </summary>
        public double? serviceRate { get; set; }
        /// <summary>
        /// 饿了么服务费
        /// </summary>
        public double? serviceFee { get; set; }
        /// <summary>
        /// 订单中的红包金额
        /// </summary>
        public double? hongbao { get; set; }
        /// <summary>
        /// 餐盒费
        /// </summary>
        public double? packageFee { get; set; }
        /// <summary>
        /// 订单活动总额
        /// </summary>
        public double? activityTotal { get; set; }
        /// <summary>
        /// 店铺承担活动费用
        /// </summary>
        public double? shopPart { get; set; }
        /// <summary>
        /// 饿了么承担活动费用
        /// </summary>
        public double? elemePart { get; set; }
        /// <summary>
        /// 降级标识
        /// </summary>
        public bool downgraded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double? vipDeliveryFeeDiscount { get; set; }
        /// <summary>
        /// 店铺绑定的外部ID
        /// </summary>
        public string openId { get; set; }
    }
}