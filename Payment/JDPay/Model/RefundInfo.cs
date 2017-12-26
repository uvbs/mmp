using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Payment.JDPay.Model
{
    [XmlRootAttribute("refund", Namespace = "", IsNullable = false)]
    public class RefundInfo
    {
        public String tradeNum { set; get; }

        public String tradeType { set; get; }

        public String oTradeNum { set; get; }

        public String amount { set; get; }

        public String currency { set; get; }

        public String tradeTime { set; get; }

        public String status { set; get; }

        public String note { set; get; }
    }
}