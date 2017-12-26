using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 订单详情查询
    /// </summary>
    public class TradeDetailInfo
    {
        //"order_id": "1009",
        //"shipping_fee": "0.00",
        //"cz_shipping_fee": "0.00",
        //"province": "25",
        //"city": "321",
        public string order_id { get; set; }
        public string shipping_fee { get; set; }
        public string cz_shipping_fee { get; set; }
        public string province { get; set; }
        public string city { get; set; }

        //"district": "2707",
        //"user_id": "751",
        //"order_sn": "411030000074",
        //"deal_code": "201411031403,411030000074",
        //"order_status": "1",
        public string district { get; set; }
        public string user_id { get; set; }
        public string order_sn { get; set; }
        public string deal_code { get; set; }
        public string order_status { get; set; }

        //"shipping_status": "0",
        //"pay_status": "2",
        //"pay_time": "1414994878",
        //"to_buyer": "",
        //"postscript": "",
        public string shipping_status { get; set; }
        public string pay_status { get; set; }
        public string pay_time { get; set; }
        public string postscript { get; set; }
        public string to_buyer { get; set; }

        //"order_amount": "143.00",
        //"user_nick": "lanwest",
        //"consignee": "lanwest",
        //"address": "上海 上海市 浦东新区",
        //"money_paid": "143.00",
        public string order_amount { get; set; }
        public string user_nick { get; set; }
        public string consignee { get; set; }
        public string address { get; set; }
        public string money_paid { get; set; }

        //"inv_payee": "testing12345",
        //"inv_content": "testing123456",
        //"order_note": "testing001",
        //"extension_id": "9",
        public string inv_payee { get; set; }
        public string inv_content { get; set; }
        public string order_note { get; set; }
        public string extension_id { get; set; }

        //"email": "shuofenglancao@live.cn",
        //"sd_id": "25",
        //"ck_id": "6",
        //"user_name": "lanwest",
        public string email { get; set; }
        public string sd_id { get; set; }
        public string ck_id { get; set; }
        public string user_name { get; set; }

        //"ckdm": "LQSD01",
        //"ck_outer_code": "LQSD01",
        //"sddm": "LQSD01",
        //"sd_outer_code": "LQSD01",
        public string ckdm { get; set; }
        public string ck_outer_code { get; set; }
        public string sddm { get; set; }
        public string sd_outer_code { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string invoice_no { get; set; }
        /// <summary>
        /// 快递代码
        /// </summary>
        public string shipping_name { get; set; }

        public List<Entity.TradeDetailOrdersInfo> orders { get; set; }

    }
}
