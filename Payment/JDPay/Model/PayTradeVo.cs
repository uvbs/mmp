using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Payment.JDPay.Model
{
    [XmlRootAttribute("pay", Namespace = "", IsNullable = false)]
    public class PayTradeVo
    {
        /**
     * 支付方式
     */
        public String payType { set; get; }
        /**
         * 交易金额
         */
        public long amount { set; get; }
        /**
         * 交易币种
         */
        public String currency { set; get; }
        /**
         * 交易时间 yyyyMMddHHmmss
         */
        public String tradeTime { set; get; }
        /**
         * 支付明细，不同支付方式的明细信息也不同
         */
        public PayTradeDetail detail { set; get; }
    }
}