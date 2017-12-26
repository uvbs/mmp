using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.Mall
{
    /// <summary>
    /// 订单列表模型
    /// </summary>
    public class OrderModelBase
    {

        /// <summary>
        ///订单编号
        /// </summary>
        public string order_id { get; set; }
        /// <summary>
        /// 外部订单号
        /// </summary>
        public string out_order_id { get; set; }
        /// <summary>
        /// 下单日期
        /// </summary>
        public double order_time { get; set; }
        /// <summary>
        /// 下单日期
        /// </summary>
        public string order_time_str { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int product_count { get; set; }
        /// <summary>
        /// 实付总金额
        /// </summary>
        public decimal total_amount { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string order_status { get; set; }

        /// <summary>
        /// 是否已经付款 0未支付 1已经支付
        /// </summary>
        public int is_pay { get; set; }

        /// <summary>
        /// 支付类型
        /// WEIXIN 微信支付
        /// ALIPAY 支付宝支付
        /// </summary>
        public string pay_type { get; set; }
        /// <summary>
        /// 订单类型
        /// 0 普通订单
        /// 1 礼品订单
        /// </summary>
        public int order_type { get; set; }
        /// <summary>
        /// 礼品订单类型
        /// 0 我收到的
        /// 1 我送给别人的
        /// </summary>
        public string gift_order_type { get; set; }
        /// <summary>
        /// 我的商品列表
        /// </summary>
        public List<OrderProductModel> product_list { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 配送方式 0 快递   1 上门自提 2送货上门
        /// </summary>
        public int delivery_type { get; set; }
        /// <summary>
        /// 是否是退款中的订单
        /// </summary>
        public string is_refund { get; set; }
        ///// <summary>
        ///// 扩展1   出国时间
        ///// </summary>
        //public string ex1 { get; set; }
        ///// <summary>
        ///// 扩展4   回国时间
        ///// </summary>
        //public string ex2 { get; set; }
        ///// <summary>
        ///// 扩展4   自提点id
        ///// </summary>
        //public string ex3 { get; set; }
        ///// <summary>
        ///// 扩展4   自提点名称
        ///// </summary>
        //public string ex4 { get; set; }

        ///// <summary>
        ///// 扩展5   押金
        ///// </summary>
        //public string ex5 { get; set; }

        ///// <summary>
        ///// 扩展6   租金
        ///// </summary>
        //public string ex6 { get; set; }

        ///// <summary>
        ///// 扩展7   自提时间
        ///// </summary>
        //public string ex7 { get; set; }

    }






}
