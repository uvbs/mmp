using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ZentCloud.BLLJIMP.Model.API.Mall
{
    /// <summary>
    ///订单详情模型
    /// </summary>
    public class OrderDetailModel : OrderModelBase
    {

        /// <summary>
        /// 买家留言
        /// </summary>
        public string buyer_memo { get; set; }
        ///// <summary>
        ///// 支付类型
        ///// </summary>
        //public string pay_type { get; set; }

        /// <summary>
        /// 物流方式
        /// </summary>
       // public string shipping_type { get; set; }

        /// <summary>
        /// 收货人省份
        /// </summary>
        public string receiver_province { get; set; }
        /// <summary>
        /// 收货人省份代码
        /// </summary>
        public string receiver_province_code { get; set; }
        /// <summary>
        /// 收货人城市
        /// </summary>
        public string receiver_city { get; set; }
        /// <summary>
        /// 收货人城市代码
        /// </summary>
        public string receiver_city_code { get; set; }

        /// <summary>
        /// 收货人区域
        /// </summary>
        public string receiver_dist { get; set; }
        /// <summary>
        /// 收货人区域代码
        /// </summary>
        public string receiver_dist_code { get; set; }
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
        /// 使用积分
        /// </summary>
        public int use_score { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public decimal freight { get; set; }

        /// <summary>
        /// 优惠券名称 
        /// </summary>
        public string cardcoupon { get; set; }
        /// <summary>
        /// 优惠券编号
        /// </summary>
        public string cardcoupon_number { get; set; }
        /// <summary>
        /// 快递公司代码
        /// </summary>
        public string express_company_code { get; set; }
        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string express_company_name { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string express_number { get; set; }

        /// <summary>
        /// 是否可以分享礼品
        /// </summary>
        public bool is_cansendgift { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public string order_time_str { get; set; }
        /// <summary>
        /// 付款时间
        /// </summary>
        public double pay_time { get; set; }
        /// <summary>
        /// 付款时间字符串
        /// </summary>
        public string pay_time_str { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public double delivery_time { get; set; }

        ///// <summary>
        ///// 邮箱
        ///// </summary>
        //public string email { get; set; }

        /// <summary>
        /// 收货人固话
        /// </summary>
        public string receiver_tel { get; set; }

        ///// <summary>
        ///// 配送方式  (现有，不用加) 0 快递   1 上门自提
        ///// </summary>
        //public int delivery_type { get; set; }

        /// <summary>
        /// 扩展1   出发日期
        /// </summary>
        public string ex1 { get; set; }
        /// <summary>
        /// 扩展2   回国日期
        /// </summary>
        public string ex2 { get; set; }
        /// <summary>
        /// 扩展3   自提点ID 配送方式为上门自提时有值
        /// </summary>
        public string ex3 { get; set; }
        /// <summary>
        /// 扩展4   自提点名称 配送方式为上门自提时有值
        /// </summary>
        public string ex4 { get; set; }

        /// <summary>
        /// 扩展5   自提点地址
        /// </summary>
        public string ex5 { get; set; }

        /// <summary>
        /// 扩展6   
        /// </summary>
        public string ex6 { get; set; }

        /// <summary>
        /// 扩展7  
        /// </summary>
        public string ex7 { get; set; }

        /// <summary>
        /// 扩展8 
        /// 是否已申诉
        ///0未申诉
        ///1 已申诉
        /// </summary>
        public string ex8 { get; set; }

        /// <summary>
        /// 扩展9   申诉内容
        /// </summary>
        public string ex9 { get; set; }

        /// <summary>
        /// 扩展10
        ///是否申请退押金
        ///0 未申请
        ///1 已申请
        /// </summary>
        public string ex10 { get; set; }
        /// <summary>
        /// 扩展11
        /// 是否申请退款
        ///0 未申请
        ///1 已申请
        /// </summary>
        public string ex11 { get; set; }
        /// <summary>
        /// 扩展12
        /// 是否可以退款
        ///0 不可以退款
        ///1 可以退款
        /// </summary>
        public string ex12 { get; set; }

        /// <summary>
        /// 扩展13
        /// 是否可以退押金
        ///0 不可以退押金
        ///1 可以退押金
        /// </summary>
        public string ex13 { get; set; }
        /// <summary>
        /// 扩展14
        /// 申请启用退款,退押金原因
        ///Json:[‘原因1’,’原因2’]
        /// </summary>
        public string ex14 { get; set; }

        /// <summary>
        /// 扩展15
        /// 申请启用 退款金额
        /// </summary>
        public string ex15 { get; set; }
        /// <summary>
        /// 扩展16
        /// 扩展字段16
        ///打款交易流水号 验证打款只能一次
        /// </summary>
        public string ex16 { get; set; }
        /// <summary>
        /// 微信退款单号
        /// </summary>
        public string ex17 { get; set; }
        /// <summary>
        /// 是否同意申请退款
        /// </summary>
        public string ex18 { get; set; }
        /// <summary>
        /// 申请退款反馈
        /// </summary>
        public string ex19 { get; set; }
        /// <summary>
        /// 是否同意申请退押金 0不同意1同意
        /// </summary>
        public string ex20 { get; set; }
        /// <summary>
        /// 申请退押金反馈
        /// </summary>
        public string ex21 { get; set; }
        /// <summary>
        ///饿了么： 降级标识 true 已降级 false未降级 
        /// </summary>
        public string ex22 { get; set; }
        /// <summary>
        /// 微信退款状态
        /// SUCCESS 退款成功
        /// Fail    退款失败
        /// PROCESSING—退款处理中
        /// NOTSURE—未确定，需要商户原退款单号重新发起
        /// CHANGE—转入代发，退款到银行发现用户的卡作废或者冻结了，导致原路退款银行卡失败，资金回流到商户的现金帐号，需要商户人工干预，通过线下或者财付通转账的方式进行退款。
        /// </summary>
        public string weixin_refund_status { get; set; }

        /// <summary>
        /// 礼品子订单列表
        /// </summary>
        public List<OrderDetailModel> child_order_list { get; set; }
        /// <summary>
        /// 拼团子订单列表
        /// </summary>
        public List<OrderDetailModel> group_buy_child_order_list { get; set; }

        /// <summary>
        /// 拼团信息
        /// </summary>
        public object group_buy_info { get; set; }
        /// <summary>
        /// 分销订单状态
        /// -1 不是分销订单
        /// 0 未付款
        /// 1 已付款
        /// 2 已收货
        /// 3 已审核
        /// </summary>
        public int distribution_offline_status { get; set; }

        /// <summary>
        /// 使用账户余额
        /// </summary>
        public decimal use_amount { get; set; }

        /// <summary>
        /// 积分抵扣金额
        /// </summary>
        public decimal score_exchang_amount { get; set; }
        /// <summary>
        /// 优惠券抵扣金额
        /// </summary>
        public decimal cardcoupon_exchang_amount { get; set; }

        /// <summary>
        /// 团购父订单
        /// </summary>
        public string groupbuy_parent_order_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal product_fee { get; set; }
        /// <summary>
        /// 评分（0分为未评分）
        /// </summary>
        public double review_score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int is_no_express { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int is_need_name_phone { get; set; }
        /// <summary>
        /// 考试信息
        /// </summary>
        public ExamInfo exam_info { get; set; }
        /// <summary>
        /// 是否主订单
        /// </summary>
        public int is_main { get; set; }
        /// <summary>
        /// 主订单号
        /// </summary>
        public string parent_order_id { get; set; }
        /// <summary>
        /// 商户
        /// </summary>
        public string supplier_name { get; set; }

        /// <summary>
        /// 自定义名称
        /// </summary>
        public string custom_creater_name { get; set; }

        /// <summary>
        /// 自定义手机
        /// </summary>
        public string custom_creater_phone { get; set; }

        /// <summary>
        /// “下单人” 自定义名称
        /// </summary>
        public string custom_rname { get; set; }

        /// <summary>
        /// 下单人信息
        /// </summary>
        public object curr_user_info { get; set; }

        /// <summary>
        /// 代付人信息
        /// </summary>
        public object pay_user_info { get; set; }

        /// <summary>
        /// 余额抵扣金额
        /// </summary>
        public decimal other_use_amount { get; set; }

        /// <summary>
        /// 储值卡抵扣金额
        /// </summary>
        public decimal other_card_coupon_dis_amount { get; set; }

        /// <summary>
        /// 代付金额
        /// </summary>
        public decimal dai_pay_total_amount { get; set; }
        /// <summary>
        /// 外卖类型
        /// </summary>
        public string take_out_type { get; set; }

        /// <summary>
        /// 下单人信息
        /// </summary>
        public object order_user_info { get; set; }
        /// <summary>
        // 卡券类型
        ///0 优惠券
        ///1 储值卡
        /// </summary>
        public int coupon_type { get; set; }
        /// <summary>
        /// 送货时间
        /// </summary>
        public string claim_arrival_time { get; set; }
        /// <summary>
        /// 门店地址
        /// </summary>
        public string store_address { get; set; }

    }





}
