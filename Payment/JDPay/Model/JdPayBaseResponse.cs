using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Payment.JDPay.Model
{
    [XmlRootAttribute("jdpay", Namespace = "", IsNullable = false)]
    public class JdPayBaseResponse
    {
        public String version { set; get; }
        public String merchant { set; get; }
        public String device { set; get; }
        public String sign { set; get; }
        public Result result { set; get; }
    }
}