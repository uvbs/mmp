using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    public class GoodsColorResp:RespDataBase
    {
        /// <summary>
        /// 颜色列表
        /// </summary>
        public List<GoodsColorInfo> list { get; set; }
    }
}
