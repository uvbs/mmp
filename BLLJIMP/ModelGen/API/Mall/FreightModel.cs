using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model.API.Mall;

namespace ZentCloud.BLLJIMP.Model.API.Mall
{
    
    /// <summary>
    /// 运费计算模型
    /// </summary>
    public class FreightModel
    {
        /// <summary>
        /// 收货人省份代码
        /// </summary>
        public int receiver_province_code { get; set; }

        /// <summary>
        /// 收货人城市代码
        /// </summary>
        public int receiver_city_code { get; set; }

        /// <summary>
        /// 收货人区域代码
        /// </summary>
        public int receiver_dist_code { get; set; }

        /// <summary>
        /// Sku 列表
        /// </summary>
        public List<SkuModel> skus { get; set; }
    }
}
