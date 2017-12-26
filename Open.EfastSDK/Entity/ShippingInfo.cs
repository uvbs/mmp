using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 配送方式档案
    /// </summary>
    public class ShippingInfo
    {
        //"shipping_id": "20",
        //"shipping_code": "ems",
        //"shipping_name": "\u90ae\u653fEMS",
        //"shipping_desc": "EMS\u56fd\u5185\u90ae\u653f\u7279\u5feb\u4e13\u9012\u63cf\u8ff0\u5185\u5bb9",
        //"insure": "0",
        //"support_cod": "1",
        //"enabled": "0",
        //"regular": null,
        //"phone": "11183",
        //"real_shipping_code": "EMS",
        //"modified": "2012-11-15 17:54:00"

        public string shipping_id { get; set; }
        public string shipping_code { get; set; }
        public string shipping_name { get; set; }
        public string shipping_desc { get; set; }
        public string insure { get; set; }
        public string support_cod { get; set; }
        public string enabled { get; set; }
        public string regular { get; set; }
        public string real_shipping_code { get; set; }
        public string phone { get; set; }
        public string modified { get; set; }

    }
}
