using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.Mall
{
    /// <summary>
    /// SKU 模型
    /// </summary>
    public class SkuModel
    {
        /// <summary>
        /// SKU  编号
        /// </summary>
        public int sku_id { get; set; }
        /// <summary>
        /// sku_sn
        /// </summary>
        public string sku_sn { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? price { get; set; }
       


    }
    /// <summary>
    /// SKU 列表
    /// </summary>
    public class SkuList
    {

        public List<SkuModel> skus { get; set; }


    }




}
