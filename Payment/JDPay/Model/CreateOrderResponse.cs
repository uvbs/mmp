using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Payment.JDPay.Model
{
    [XmlRootAttribute("jdpay", Namespace = "", IsNullable = false)]
    public class CreateOrderResponse : JdPayBaseResponse
    {
        public String orderId { set; get; }

        public String merchantName { set; get; }

        public String amount { set; get; }

        public String tradeNum { set; get; }

        public String qrCode { set; get; }

        public String expireTime { set; get; }
    }
}