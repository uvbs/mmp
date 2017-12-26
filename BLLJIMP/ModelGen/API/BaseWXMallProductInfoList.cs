﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API
{
    [Serializable]
    public class BaseWXMallProductInfoList
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public List<WXMallProductInfo> List { get; set; }
    }
}
