using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微网站-幻灯片
    /// </summary>
    public class CompanyWebsite_Projector : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 幻灯片名称
        /// </summary>
        public string ProjectorName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string ProjectorDescription { get; set; }
        /// <summary>
        /// 播放顺序 从小到大
        /// </summary>
        public int PlayIndex { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ProjectorImage { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public string IsShow { get; set; }
        /// <summary>
        /// 类型 
        /// </summary>
        public string ProjectorType { get; set; }
        /// <summary>
        /// 类型 对应值
        /// </summary>
        public string ProjectorTypeValue { get; set; }
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

    }
}
