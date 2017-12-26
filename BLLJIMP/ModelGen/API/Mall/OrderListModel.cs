using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.Mall
{
    /// <summary>
    /// 订单列表模型
    /// </summary>
    public class OrderListModel:OrderModelBase
    {

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
        ///是否可以继续将礼品发送给别人
        /// </summary>
        public bool is_cansendgift { get; set; }

        /// <summary>
        /// 拼团信息
        /// </summary>
        public object group_buy_info { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string receiver_name { get; set; }
        /// <summary>
        /// 收货人手机
        /// </summary>
        public string receiver_phone { get; set; }
        /// <summary>
        /// 用户AutoId
        /// </summary>
        public string user_aid { get; set; }
        /// <summary>
        /// 评分（0分为未评分）
        /// </summary>
        public double review_score { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string receiver_address { get; set; }
        /// <summary>
        /// 客户留言
        /// </summary>
        public string receiver_remark { get; set; }
        /// <summary>
        /// 考试信息
        /// </summary>
        public ExamInfo exam_info { get; set; }
        /// <summary>
        /// 是否无需物流 1无需物流 0 需要物流
        /// </summary>
        public int is_no_express{get;set;}
        /// <summary>
        /// 是否预购订单
        /// </summary>
        public int is_appointment { get; set; }
        /// <summary>
        /// 是否主订单
        /// </summary>
        public int is_main { get; set; }
        /// <summary>
        ///主订单
        /// </summary>
        public string parent_order_id { get; set; }
        /// <summary>
        /// 商户
        /// </summary>
        public string supplier_name { get; set; }



        /// <summary>
        /// 参团成功人数
        /// </summary>
        public string ex7 { get; set; }

        /// <summary>
        /// 参团总人数
        /// </summary>
        public int people_count { get; set; }
        /// <summary>
        /// 商品id 扩展
        /// </summary>
        public string ex9 { get; set; }
        /// <summary>
        /// 1系统开团
        /// </summary>
        public string ex10 { get; set; }

        /// <summary>
        /// 规则id
        /// </summary>
        public string ex11 { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string ex12 { get; set; }

        /// <summary>
        /// //成团价
        /// </summary>
        public string ex13 { get; set; }

        /// <summary>
        /// 团员折扣
        /// </summary>

        public decimal member_discount { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public double pay_time { get; set; }

        /// <summary>
        /// 外卖类型
        /// </summary>
        public string take_out_type { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>
        public double delivery_time { get; set; }
        /// <summary>
        /// 商家备注
        /// </summary>
        public string ex21 { get; set; }

    }






}
