using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 首页图片
    /// </summary>
    public class AdInfo : ZCBLLEngine.ModelTable
    {
        public int AutoId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 类型 1手机首页滚动图
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 图片链接
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 网站链接
        /// </summary>
        public string SiteUrl { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
    }
}
