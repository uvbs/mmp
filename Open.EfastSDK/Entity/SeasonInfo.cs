using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 商品季节档案
    /// </summary>
    public class SeasonInfo
    {
        public string season_id { get; set; }
        public string season_code { get; set; }
        public string season_name { get; set; }
        public string default_item { get; set; }
        public string vid { get; set; }
        public string modified { get; set; }
    }
}
