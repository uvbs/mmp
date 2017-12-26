using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 幻灯片
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
        private string _link;
        public string Link {
            get { return ZentCloud.Common.StringHelper.ReplaceLinkRD(_link); }
            set { _link = value; } 
        }
        /// <summary>
        /// 播放顺序 从小到大
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
       /// <summary>
       /// 类型
       /// </summary>
        public string Type { get; set; }
       /// <summary>
       /// 链接文字
       /// </summary>
        public string LinkText { get; set; }
       /// <summary>
       /// 图片宽
       /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 图片高
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 链接选择类型
        /// </summary>
        public string Stype { get; set; }
        /// <summary>
        /// 链接选择值
        /// </summary>
        public string Svalue { get; set; }
        /// <summary>
        /// 链接选择名称
        /// </summary>
        public string Stext { get; set; }
       /// <summary>
       /// 是否Pc
       /// </summary>
        public int IsPc { get; set; }

    }
}
