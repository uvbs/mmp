using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 颜色档案
    /// </summary>
    public class ColorInfo
    {
        //"color_id": "2",
        //"color_code": "00001",
        //"color_name": "\u62a4\u690e\u592a\u7a7a\u8bb0\u5fc6\u6795-\u52a0\u957f\u578b",
        //"color_note": "",
        //"img_file": "",
        //"img_id": "0",
        //"outer_color_code": null,
        //"modified": "2012-11-15 17:54:00"

        public string color_id { get; set; }
        public string color_code { get; set; }
        public string color_name { get; set; }
        public string color_note { get; set; }
        public string img_file { get; set; }
        public string img_id { get; set; }
        public string outer_color_code { get; set; }
        public string modified { get; set; }

    }
}
