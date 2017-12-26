using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.HaoService
{
    /// <summary>
    /// 品牌
    /// </summary>
    public class Brand
    {
        /// <summary>
        /// 品牌ID
        /// </summary>
        public int I { get; set; }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string N { get; set; }
        /// <summary>
        /// 品牌开头字母
        /// </summary>
        public string L { get; set; }
        /// <summary>
        /// 品牌车系列表
        /// </summary>
        public List<Series> List { get; set; }
    }
}
