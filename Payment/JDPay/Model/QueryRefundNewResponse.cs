using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Payment.JDPay.Model
{
    [XmlRootAttribute("jdpay", Namespace = "", IsNullable = false)]
    public class QueryRefundNewResponse: JdPayBaseResponse 
    {
        public List<RefundInfo> refList { set; get; }
    }
}