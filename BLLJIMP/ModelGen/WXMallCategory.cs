using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微商城分类
    /// </summary>
    public partial class WXMallCategory : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 分类描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 上级分类ID
        /// </summary>
        public int PreID { get; set; }
        /// <summary>
        /// 分类图片
        /// </summary>
        public string CategoryImg { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        private string _type="Mall";
        public string Type
        {
            set { _type = value; }
            get { return _type; }
        }

        public int Sort { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }
        /// <summary>
        /// 是否系统类型
        /// </summary>
        public int IsSys { get; set; }

    }
}
