using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 地理区域档案
    /// </summary>
    public class RegionInfo
    {
    //"region_id": "2",
    //"parent_id": "1",
    //"region_name": "\u5317\u4eac",
    //"region_type": "1"

        public string region_id { get; set; }
        public string parent_id { get; set; }
        public string region_name { get; set; }
        public string region_type { get; set; }

    }
}
