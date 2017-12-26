using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 选项卡
    /// </summary>
    [Serializable]
    public partial class CompanyWebsite_Tab : ModelTable
    {
        /// <summary>
        /// AutoID
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 分组
        /// </summary>
        public string TabGroup { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string TabName { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 选中时背景颜色
        /// </summary>
        public string ActBgColor { get; set; }
        /// <summary>
        /// 背景颜色
        /// </summary>
        public string BgColor { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public int IsHide { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}