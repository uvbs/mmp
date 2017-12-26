using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 条码档案
    /// </summary>
    public class SkuInfo
    {
//          "goods_sn": "0000-01",
//                "barcode_id": "612",
//                "goods_
//id": "1",
//                "cat_id": "1",
//                "color_id": "1",
//                "size_id": "1",
//                "barcode": "DC2001-AF0411",
//                "is_pid": "1",
//                "bswl_flag": "0",
//                "weight": "0.00",
//                "sku_sn": "0000-01000000",
//                "modified": "2012-11-15
//17:54:00"

        public string goods_sn { get; set; }
        public string barcode_id { get; set; }
        public string goods_id { get; set; }
        public string cat_id { get; set; }
        public string color_id { get; set; }
        public string size_id { get; set; }
        public string barcode { get; set; }
        public string is_pid { get; set; }
        public string bswl_flag { get; set; }
        public string weight { get; set; }
        public string sku_sn { get; set; }
        public string modified { get; set; }

    }
}
