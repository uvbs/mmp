using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 自提点
    /// </summary>
    public  class GetAddress : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自提点id
        /// </summary>
        public string GetAddressId { get; set; }
        /// <summary>
        /// 自提点名称
        /// </summary>
        public string GetAddressName { get; set; }
        /// <summary>
        /// 自提点地址
        /// </summary>
        public string GetAddressLocation { get; set; }
        /// <summary>
        /// 是否禁用
        /// 0 启用
        /// 1 禁用
        /// </summary>
        public int IsDisable { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgUrl { get; set; }

    }
}
