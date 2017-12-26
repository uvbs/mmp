using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 类型信息
    /// </summary>
    public class TypeInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 类型ID
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime InsertDate { get; set; }

    }
}
