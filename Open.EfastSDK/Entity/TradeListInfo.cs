using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 订单列表查询
    /// </summary>
    public class TradeListInfo
    {
        //"order_id": "133",
        //"order_sn": "407250000154",
        //"deal_code": "7813134123",
        //"order_status": "0",
        //"shipping_status": "0",
        //"pay_status": "0",
        public string order_id { get; set; }
        public string order_sn { get; set; }
        public string deal_code { get; set; }
        public string order_status { get; set; }
        public string shipping_status { get; set; }
        public string pay_status { get; set; }

        //"process_status": "0",
        //"is_send": "0",
        //"is_locked": "0",
        //"is_separate": "0",
        public string process_status { get; set; }
        public string is_send { get; set; }
        public string is_locked { get; set; }
        public string is_separate { get; set; }

        //"shipping_name": "ems",
        //"pay_name": "tenpay",
        //"pay_time": "1333686180",
        //"to_buyer": "商家备注",
        public string shipping_name { get; set; }
        public string pay_name { get; set; }
        public string pay_time { get; set; }
        public string to_buyer { get; set; }

        //"postscript": "",
        //"order_amount": "3013.00",
        //"money_paid": "3013.00",
        //"user_id": "40",
        //"user_nick": "emmahi",
        public string postscript { get; set; }
        public string order_amount { get; set; }
        public string money_paid { get; set; }
        public string user_id { get; set; }
        public string user_nick { get; set; }

        //"shipping_fee": "13.00",
        //"cz_shipping_fee": "0.00",
        //"province": "17",
        //"city": "233",
        //"district": "1960",
        public string shipping_fee { get; set; }
        public string cz_shipping_fee { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }

        //"invoice_no": "",
        //"add_time": "1308067200",
        //"delivery_time": "1406279947",
        //"sd_id": "2",
        //"ck_id": "1",
        public string invoice_no { get; set; }
        public string add_time { get; set; }
        public string delivery_time { get; set; }
        public string sd_id { get; set; }
        public string ck_id { get; set; }

        //"create_time": "0000-00-00 00:00:00",
        //"lylx": "淘宝",
        //"shipping_time": null,
        //"inv_payee": "testing0000001", (发票抬头)
        //"inv_content": "testing0000002",(发票内容)
        public string create_time { get; set; }
        public string lylx { get; set; }
        public string shipping_time { get; set; }
        public string inv_payee { get; set; }
        public string inv_content { get; set; }

        //"order_note": "现代武道",(订单备注)
        //"extension_id": "9",(客服 id)
        //"email": "shuofenglancao@live.cn",(邮箱)
        //"yfje": 0,
        public string order_note { get; set; }
        public string extension_id { get; set; }
        public string email { get; set; }
        public int yfje { get; set; }

        //"ckdm": "IWMS",
        //"ck_outer_code": "testing777",
        //"sddm": "tbfx_mzysp",
        //"sd_outer_code": "",
        //"extension": "dd.zhao"
        public string ckdm { get; set; }
        public string ck_outer_code { get; set; }
        public string sddm { get; set; }
        public string sd_outer_code { get; set; }
        public string extension { get; set; }

    }
}
