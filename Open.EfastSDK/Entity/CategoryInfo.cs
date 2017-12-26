using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 商品分类档案
    /// </summary>
    public class CategoryInfo
    {

//         "cat_id": "1",
//                "cat_name": "Jacky\u5206\u7c7b",
//                "cat_code": "0000-01",
//                "keywords": "",
//                "cat_desc": "",
//                "parent_id": "0",
//                "sort_order": "0",
//                "template_file": "",
//                "measure_unit": "",
//                "show_in_nav": "0",
//                "style": "",
//                "is_show": "0",
//                "grade": "0",
//                "filter_attr": "0",
//                "goods_type": "0",
//                "cid": "",
//                "modified": "2012-11-15
//17:54:00"

        public string cat_id { get; set; }
        public string cat_name { get; set; }
        public string cat_code { get; set; }
        public string keywords { get; set; }
        public string cat_desc { get; set; }
        public string parent_id { get; set; }
        public string sort_order { get; set; }
        public string template_file { get; set; }
        public string measure_unit { get; set; }
        public string show_in_nav { get; set; }
        public string style { get; set; }
        public string is_show { get; set; }
        public string grade { get; set; }
        public string filter_attr { get; set; }
        public string goods_type { get; set; }
        public string cid { get; set; }
        public string modified { get; set; }

    }
}
