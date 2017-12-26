using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public partial class ArticleCategoryType : ModelTable
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string CategoryType { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string CategoryTypeName { get; set; }
        /// <summary>
        /// 是否有图片
        /// </summary>
        public int HaveImg { get; set; }
        /// <summary>
        /// 是否有链接
        /// </summary>
        public int HaveLink { get; set; }
        /// <summary>
        /// 幻灯片宽
        /// </summary>
        public int SlideWidth { get; set; }
        /// <summary>
        /// 幻灯片高
        /// </summary>
        public int SlideHeight { get; set; }

    }
}