using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 文章活动分类
    /// </summary>
    public partial class ArticleCategory : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID{get;set;}
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 分类类型 article 文章 activity活动
        /// </summary>
        public string CategoryType { get; set; }

        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 上级 分类ID
        /// </summary>
        public int PreID { get; set; }
        /// <summary>
        /// 排序号 从小到大
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        public int SysType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgSrc { get; set; }
        /// <summary>
        /// 分类介绍
        /// </summary>
        public string Summary { get; set; }

    }
}
