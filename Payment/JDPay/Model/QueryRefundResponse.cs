using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Payment.JDPay.Model
{
    [XmlRootAttribute("jdpay", Namespace = "", IsNullable = false)]
    public class QueryRefundResponse: JdPayBaseResponse
    {

        public String tradeNum { set; get; }
        public String tradeType { set; get; }
        public String oTradeNum { set; get; }
        // 金额，单位分
        public long amount { set; get; }
        public String currency { set; get; }
        public String tradeTime { set; get; }
        public String note { set; get; }
        public String status { set; get; }
    }
}