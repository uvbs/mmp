using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Payment.JDPay.Model
{
    [XmlRootAttribute("jdpay", Namespace = "", IsNullable = false)]
    public class RefundResponse : JdPayBaseResponse
    {


        public String tradeNum { set; get; }

        public String oTradeNum { set; get; }

        public String amount { set; get; }

        public String currency { set; get; }

        public String tradeTime { set; get; }
        /// <summary>
        /// 0-处理中
        ///1-成功
        ///2-失败，最终状态，不用重试
        ///3-失败，需要原单号发起重
        /// </summary>
        public String status { set; get; }

        public String note { set; get; }
    }
}