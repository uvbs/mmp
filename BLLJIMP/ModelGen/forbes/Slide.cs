using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Forbes
{
    /// <summary>
    /// 福布斯幻灯片
    /// </summary>
   public class Slide: ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
       /// <summary>
       /// 图片路径
       /// </summary>
        public string ImageUrl { get; set; }
       /// <summary>
       /// 跳转链接
       /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// 播放顺序 从小到大
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }


    }
}
