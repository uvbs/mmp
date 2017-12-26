using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微网站-首页中部图标导航
    /// </summary>
    public partial class CompanyWebsite_Navigate : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 导航名称
        /// </summary>
        public string NavigateName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string NavigateDescription { get; set; }
        /// <summary>
        /// 播放顺序 从小到大
        /// </summary>
        public int PlayIndex { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string NavigateImage { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public string IsShow { get; set; }
        /// <summary>
        /// 类型 
        /// </summary>
        public string NavigateType { get; set; }
        /// <summary>
        /// 类型 对应值
        /// </summary>
        private string _navigate_type_value;
        public string NavigateTypeValue
        {
            get { return ZentCloud.Common.StringHelper.ReplaceLinkRD(_navigate_type_value); }
            set { _navigate_type_value = value; }
        }
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

    }
}
