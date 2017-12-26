using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 支付方式档案
    /// </summary>
    public class PaymentInfo
    {
        
//"pay_id": "2",
//"pay_code": "balance",
//"pay_name": "\u4f59\u989d\u652f\u4ed8",
//"pay_fee": "0",
//"pay_desc": "\u4
//f7f\u7528\u5e10\u6237\u4f59\u989d\u652f\u4ed8\u3002\u53ea\u6709\u5ba2\u6237\u624d\u80fd\u4f7f\u7528\uff0c
//\u901a\u8fc7\u8bbe\u7f6e\u4fe1\u7528\u989d\u5ea6\uff0c\u53ef\u4ee5\u900f\u652f\u3002",
//"pay_order": "0",
//"pay_config": "a:0:{}",
//"enabled": "1",
//"is_cod": "0",
//"is_online": "1",
//"modified": "2012-11-15 17:54:00"

        public string pay_id { get; set; }
        public string pay_code { get; set; }
        public string pay_name { get; set; }
        public string pay_fee { get; set; }
        public string pay_desc { get; set; }
        public string pay_order { get; set; }
        public string pay_config { get; set; }
        public string enabled { get; set; }
        public string is_cod { get; set; }
        public string is_online { get; set; }
        public string modified { get; set; }

    }
}
