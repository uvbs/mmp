using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public class GoodsInfo
    {
        public string goods_id { get; set; }
        /// <summary>
        /// 分类 id
        /// </summary>
        public string cat_id { get; set; }
        public string size_lib_id { get; set; }
        /// <summary>
        /// 商品货号
        /// </summary>
        public string goods_sn { get; set; }
        public string goods_name { get; set; }
        public string goods_name_style { get; set; }
        public string click_count { get; set; }
        /// <summary>
        /// 品牌 id
        /// </summary>
        public string brand_id { get; set; }
        public string goods_number { get; set; }
        /// <summary>
        /// 商品重量
        /// </summary>
        public string goods_weight { get; set; }
        /// <summary>
        /// 市场价
        /// </summary>
        public string market_price { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public string shop_price { get; set; }
        /// <summary>
        /// 促销价
        /// </summary>
        public string promote_price { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string keywords { get; set; }
        /// <summary>
        /// 商品简称
        /// </summary>
        public string goods_brief { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string goods_desc { get; set; }
        public string goods_thumb { get; set; }
        public string goods_img { get; set; }
        public string original_img { get; set; }
        public string is_real { get; set; }
        /// <summary>
        /// 季节id
        /// </summary>
        public string season_id { get; set; }
        /// <summary>
        /// 系列id
        /// </summary>
        public string series_id { get; set; }
        /// <summary>
        /// 是否在售
        /// </summary>
        public string is_on_sale { get; set; }
        /// <summary>
        /// 是否新品
        /// </summary>
        public string is_new { get; set; }
        /// <summary>
        /// 是否热销
        /// </summary>
        public string is_hot { get; set; }
        /// <summary>
        /// 是否礼品
        /// </summary>
        public string is_gift { get; set; }
        public string goods_unit { get; set; }

        //"modified": "2012-11-15 17:54:00",
        public DateTime? modified { get; set; }

        public List<GoodsColorInfo> color_list { get; set; }
        public List<GoodsSizeInfo> size_list { get; set; }
        
    }
}
