using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 订单详情查询-商品明细
    /// </summary>
    public class TradeDetailOrdersInfo
    {
        //"goods_sn": "ERP301",
        //"color_id": "85",
        //"size_id": "92",
        //"market_price": "55.00",
        //"color_code": "1001",
        //"size_code": "1001"

        public string goods_sn { get; set; }
        public string color_id { get; set; }
        public string size_id { get; set; }
        public string market_price { get; set; }
        public string color_code { get; set; }
        public string size_code { get; set; }

    }
}
