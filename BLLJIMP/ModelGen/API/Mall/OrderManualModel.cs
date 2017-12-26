using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.Mall
{
    /// <summary>
    /// 提交订单请求模型
    /// </summary>
    public class OrderManualModel
    {
        /// <summary>
        /// 订单来源
        /// </summary>
        public string order_source { get; set; }

        /// <summary>
        /// 外部订单号
        /// </summary>
        public string out_order_id { get; set; }

        /// <summary>
        /// 下单日期
        /// </summary>
        public string insert_date { get; set; }

        /// <summary>
        /// 支付类型 微信 支付宝
        /// </summary>
        public int pay_type { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string order_status { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public int pay_status { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public string pay_time { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal total_amount { get; set; }
        /// <summary>
        /// 使用积分
        /// </summary>
        public int use_score { get; set; }
        /// <summary>
        /// 使用余额
        /// </summary>
        public decimal use_amount { get; set; }
        /// <summary>
        /// 卡券名称
        /// </summary>
        public string card_coupon_name { get; set; }
        /// <summary>
        /// 配送方式 
        /// 快递 无需物流
        /// </summary>
        public string develive_type { get; set; }

        /// <summary>
        /// 街道地址
        /// </summary>
        public string receiver_address { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string receiver_name { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        public string receiver_phone { get; set; }

          /// <summary>
        /// 收货人姓名
        /// </summary>
        public string buy_name { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        public string buy_phone { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public decimal freight { get; set; }

        /// <summary>
        /// 买家留言
        /// </summary>
        public string buyer_memo { get; set; }
        /// <summary>
        /// 商品列表
        /// </summary>
        public List<OrderProductManualModel> product_list { get; set; }

    }

    /// <summary>
    /// 商品
    /// </summary>
    public class OrderProductManualModel {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string product_name { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal product_price { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int product_count { get; set; }
    
    }
}
