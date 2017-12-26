using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Payment.JDPay.Model
{
    [XmlRootAttribute("jdpay", Namespace = "", IsNullable = false)]
    public class PaymentTradeResponse: JdPayBaseResponse
    {
        /**
     * 交易流水  数字或字母
     */
        public String tradeNum { set; get; }
        /**
         * 交易类型
         */
        public int tradeType { set; get; }
        /** 交易备注
        */
        public String note { set; get; }
        /**
         * 支付总金额
         */
        public long amount { set; get; }
        /**
         * 交易返回状态  成功：2，失败，3
         */
        public String status { set; get; }
        /**
         * 交易列表
         */
        public List<PayTradeVo> payList { set; get; }
    }
}