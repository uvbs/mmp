using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Payment.JDPay.Model
{
    [XmlRootAttribute("jdpay", Namespace = "", IsNullable = false)]
    public class AsynNotifyResponse: JdPayBaseResponse
    {

        /**
         * 交易流水  数字或字母
         */
        public String tradeNum { set; get; }
        /**
         * 0:消费,1:退款
         */
        public String tradeType { set; get; }

        /**
         * 交易列表
         */
        public List<PayTradeVo> payList { set; get; }


        /** ================= 退款相关字段  =================**/
        public String oTradeNum { set; get; }
        // 金额，单位分
        public long amount { set; get; }
        public String currency { set; get; }
        public String tradeTime { set; get; }
        public String note { set; get; }
        public String status { set; get; }
        /** ================= 退款相关字段  =================**/
        
    }
}