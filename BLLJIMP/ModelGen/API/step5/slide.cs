﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{
    /// <summary>
    /// 首页 幻灯片模型
    /// </summary>
    public class slide
    {
        /// <summary>
        /// 图片url
        /// </summary>
        public string imgurl { get; set; }
        /// <summary>
        /// 跳转链接
        /// </summary>
        public string link { get; set; }


    }
    /// <summary>
    /// 幻灯片api模型
    /// </summary>
    public class slideapi
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 分类集合
        /// </summary>
        public List<slide> list { get; set; }
    }


}
