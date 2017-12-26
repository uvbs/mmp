using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 尺码档案
    /// </summary>
    public class SizeInfo
    {
        //"size_id": "2",
        //"size_code": "00001",
        //"size_name": "220x240cm",
        //"size_note": "",
        //"row_position": "2",
        //"col_position": "1",
        //"outer_size_code": null,
        //"is_ext": "0",
        //"modified": "2012-11-15 17:54:00"

        public string size_id { get; set; }
        public string size_code { get; set; }
        public string size_name { get; set; }
        public string size_note { get; set; }
        public string row_position { get; set; }
        public string col_position { get; set; }
        public string outer_size_code { get; set; }
        public string is_ext { get; set; }
        public string modified { get; set; }

    }
}
