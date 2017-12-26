using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 库存档案（走库存分配策略）
    /// </summary>
    public class SkuStockInfo
    {
        //"sl": 19,
        //"goods_id": 1645,
        //"color_id": 91,
        //"size_id": 1,
        //"msg": "
        //\u8ba1\u7b97\u65b9\u6cd5\uff1a(\u57fa\u784047-\u5b89\u51680-\u7f3a\u8d2715)=32*60%=19-\u672a\u8f6c\u53550
        //=19"

        /// <summary>
        /// sku 库存
        /// </summary>
        public int sl { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public int goods_id { get; set; }
        /// <summary>
        /// 颜色Id
        /// </summary>
        public int color_id { get; set; }
        /// <summary>
        /// 尺码ID
        /// </summary>
        public int size_id { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string msg { get; set; }

    }
}
