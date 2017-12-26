using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 商城导航
    /// </summary>
    public class Navigation : ZCBLLEngine.ModelTable
    {
      
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 上级ID
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 导航图片
        /// </summary>
        public string NavigationImage { get; set; }
        /// <summary>
        /// 导航名称
        /// </summary>
        public string NavigationName { get; set; }
        /// <summary>
        /// 导航名称
        /// </summary>
        public string NavigationLink { get; set; }
        /// <summary>
        /// 导航类型  top顶部菜单 bottom 底部导航 left 左侧导航 其它自定义
        /// </summary>
        public string NavigationLinkType { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 当前站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }



    }
}
