using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 商店档案
    /// </summary>
    public class ShopInfo
    {
        //        {
        //                "Id": "2",
        public string Id { get; set; }
        //                "sddm": "02",
        //                "sdmc": "\u67d2\u724c\u62cd\u62cd",
        //                "sdxz": "0",
        //                "sdlx": "2",
        public string sddm { get; set; }
        public string sdmc { get; set; }
        public string sdxz { get; set; }
        public string sdlx { get; set; }
        //                "presale": "6",
        //                "kl": "0.0000",
        //                "fzr": "",
        //                "bz": "",
        //                "ck_id": "1",
        public string presale { get; set; }
        public string kl { get; set; }
        public string fzr { get; set; }
        public string bz { get; set; }
        public string ck_id { get; set; }
        //                "kh_id": null,
        //                "fp_rate": "100",
        //                "zk": "1.00",
        //                "alipay_no": "",
        public string kh_id { get; set; }
        public string fp_rate { get; set; }
        public string zk { get; set; }
        public string alipay_no { get; set; }
        //                "price_sel": "0",
        //                "discount": "1.00",
        //                "credit_money": "0.00",
        //                "account_money": "0.00",
        //                "freeze_money": "0.00",
        //                "bf_confirm": "0",
        public string price_sel { get; set; }
        public string discount { get; set; }
        public string credit_money { get; set; }
        public string account_money { get; set; }
        public string freeze_money { get; set; }
        public string bf_confirm { get; set; }
        //                "fee_confirm": "0",
        //                "fee_money": "0.00",
        //                "js_sd": "0",
        //                "ww": "",
        //                "outer_code": null,
        public string fee_confirm { get; set; }
        public string fee_money { get; set; }
        public string js_sd { get; set; }
        public string ww { get; set; }
        public string outer_code { get; set; }
        //                "rank": "1",
        //                "goods_sn_lsh": "0",
        //                "province_name": "\u4e0a\u6d77",
        //                "goods_fee_money": "0",
        public string rank { get; set; }
        public string goods_sn_lsh { get; set; }
        public string province_name { get; set; }
        public string goods_fee_money { get; set; }
        //                "city_name": "\u4e0a\u6d77\u5e02",
        //                "goods_fee_confirm": "0",
        //                "rank_points_rate": "0",
        //                "fenxiao_status": "0",
        //                "is_send_msg": "0",
        public string city_name { get; set; }
        public string goods_fee_confirm { get; set; }
        public string rank_points_rate { get; set; }
        public string fenxiao_status { get; set; }
        public string is_send_msg { get; set; }
        //                "is_enable": "1",
        //                "is_rds": "0",
        //                "rds_db_host": "",
        //                "rds_db_name": "",
        //                "rds_db_user": "",
        //                "rds_db_pass": "",
        //                "rds_db_port": "",
        //                "modified": "2012-11-15 17:54:00"
        //            }
        public string is_enable { get; set; }
        public string is_rds { get; set; }
        public string rds_db_host { get; set; }
        public string rds_db_name { get; set; }
        public string rds_db_user { get; set; }
        public string rds_db_pass { get; set; }
        public string rds_db_port { get; set; }
        public string modified { get; set; }

    }
}
