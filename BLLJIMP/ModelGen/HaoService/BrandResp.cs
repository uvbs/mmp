using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.HaoService
{

    /// <summary>
    /// 品牌车系查询响应
    /// </summary>
    public class BrandResp:RespBase
    {
        /// <summary>
        /// 品牌结果
        /// </summary>
        public List<Brand> result { get; set; }
    }
}
