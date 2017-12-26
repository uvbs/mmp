using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 商品品牌档案
    /// </summary>
    public class BrandInfo
    {
        public string brand_id { get; set; }
        public string brand_code { get; set; }
        public string brand_name { get; set; }
        public string brand_logo { get; set; }
        public string brand_desc { get; set; }
        public string site_url { get; set; }
        public string sort_order { get; set; }
        public string is_show { get; set; }
        public string vid { get; set; }
        public string modified { get; set; }
    }
}

