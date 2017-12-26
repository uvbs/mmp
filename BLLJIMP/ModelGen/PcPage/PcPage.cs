using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.ModelGen.PcPage
{
   public class PcPage
    {
       /// <summary>
       /// 页面Id
       /// </summary>
       public int PageId { get; set; }
        /// <summary>
        /// 页面名称
        /// </summary>
        public string PageName { get; set; }
        /// <summary>
        /// 最顶部内容
        /// </summary>
        public string TopContent { get; set; }
        /// <summary>
        /// Logo
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// 顶部菜单 存储的是导航的Id
        /// </summary>
        public string TopMenu { get; set; }
        /// <summary>
        ///中部内容
        ///存储幻灯片 编辑器内容等
        /// </summary>
        public List<MiddModel> MiddList { get; set; }
        /// <summary>
        /// 底部内容 存储版权等
        /// </summary>
        public string BottomContent { get; set; }
        /// <summary>
        /// 站点所有者 
        /// </summary>
        public string WebsiteOwner { get; set; }

    }
    /// <summary>
    /// 中部内容模型
    /// </summary>
   public class MiddModel  {
       /// <summary>
       /// 类型
       /// slide 幻灯片
       /// html  编辑器内容
       /// </summary>
       public string Type { get; set; }
       /// <summary>
       /// 编辑器内容
       /// </summary>
       public string Content { get; set; }
       /// <summary>
       /// 幻灯片名称
       /// </summary>
       public string SlideName { get; set; }
       /// <summary>
       /// 幻灯片效果
       /// </summary>
       public string SlideType { get; set; }
   
   }
}
