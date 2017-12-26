using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class WXMallScoreTypeInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 积分类型
        /// </summary>
        public WXMallScoreTypeInfo() { }

        /// <summary>
        /// 编号
        /// </summary>
        public int AutoId { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string TypeImg { get; set; }
        /// <summary>
        /// 当前站点所有者
        /// </summary>
        public string websiteOwner { get; set; }

    }
}
