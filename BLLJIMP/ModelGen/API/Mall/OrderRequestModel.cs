using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.Mall
{
    /// <summary>
    /// 提交订单请求模型
    /// </summary>
    public class OrderRequestModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string order_id { get; set; }
        /// <summary>
        /// 订单类型
        /// 0 普通订单
        /// 1 礼品订单
        /// 2 拼团订单
        /// 3 预约订单
        /// 4 付费活动订单
        /// 5 医生预约
        /// 6 医生预约(推荐)
        /// </summary>
        public int order_type { get; set; }
        /// <summary>
        /// 买家留言
        /// </summary>
        public string buyer_memo { get; set; }
        /// <summary>
        /// 支付类型 WEIXIN 或 ALIPAY
        /// </summary>
        public string pay_type { get; set; }
        /// <summary>
        /// 物流方式0 快递 1 上门自提 2 送货上门
        /// </summary>
        public string delivery_type { get; set; }
        /// <summary>
        /// 收货人省份
        /// </summary>
        public string receiver_province { get; set; }
        /// <summary>
        /// 收货人省份代码
        /// </summary>
        public int receiver_province_code { get; set; }
        /// <summary>
        /// 收货人城市名称
        /// </summary>
        public string receiver_city { get; set; }
        /// <summary>
        /// 收货人城市代码
        /// </summary>
        public int receiver_city_code { get; set; }

        /// <summary>
        /// 收货人区域名称
        /// </summary>
        public string receiver_dist { get; set; }
        /// <summary>
        /// 收货人区域代码
        /// </summary>
        public int receiver_dist_code { get; set; }
        /// <summary>
        /// 街道地址
        /// </summary>
        public string receiver_address { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string receiver_name { get; set; }
        /// <summary>
        /// 收货人邮编
        /// </summary>
        public string receiver_zip { get; set; }
        /// <summary>
        /// 收货人电话
        /// </summary>
        public string receiver_phone { get; set; }
        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string express_company { get; set; }
        /// <summary>
        /// 商品SKU列表
        /// </summary>
        public List<SkuModel> skus { get; set; }
        /// <summary>
        /// 优惠券编号 我的优惠券编号
        /// </summary>
        public int cardcoupon_id { get; set; }
        /// <summary>
        /// 使用积分
        /// </summary>
        public int use_score { get; set; }

        /// <summary>
        /// 导购用户ID
        /// </summary>
        public string sale_id { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public decimal freight { get; set; }
        /// <summary>
        /// 使用账户金额
        /// </summary>
        public decimal use_amount { get; set; }
        /// <summary>
        /// 团购类型：下团长单leader、下团员单member
        /// </summary>
        public string groupbuy_type { get; set; }
        /// <summary>
        /// 团购规则ID
        /// </summary>
        public string rule_id { get; set; }
        /// <summary>
        /// 团购父订单
        /// </summary>
        public string groupbuy_parent_orderid { get; set; }
        /// <summary>
        /// 要求送达时间
        /// </summary>
        public string claim_arrival_time { get; set; }

        /// <summary>
        /// 自定义名称
        /// </summary>
        public string custom_creater_name { get; set; }

        /// <summary>
        /// 自定义手机
        /// </summary>
        public string custom_creater_phone { get; set; }
        /// <summary>
        /// 卡券类型
        /// 0 优惠券
        /// 1 储值卡
        /// </summary>
        public string coupon_type { get; set; }

        /// <summary>
        /// 运费模板备注  eg:购买商品件数满5件包邮
        /// </summary>
        public string freight_remark { get; set; }
        /// <summary>
        /// 是否无需物流1 无需物流 0需要物流
        /// </summary>
        public string is_no_express { get; set; }
        /// <summary>
        /// 供应商Id
        /// </summary>
        public string supplier_id { get; set; }
        /// <summary>
        /// 折扣类型
        /// 0商品折扣
        /// 1 会员折扣
        /// 2 生日折扣
        /// </summary>
        public string discount_type { get; set; }

        ///// <summary>
        ///// 经度
        ///// </summary>
        //public string longitude { get; set; }
        ///// <summary>
        ///// 纬度
        ///// </summary>
        //public string latitude { get; set; }

    }
}
