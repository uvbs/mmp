using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Payment.JDPay.Model
{
    [XmlRootAttribute("jdpay", Namespace = "", IsNullable = false)]
    public class JdPayResponse
    {


        public String version { set; get; }

        public String merchant { set; get; }


        public Result result { set; get; }

        public String encrypt { set; get; }
    }
}