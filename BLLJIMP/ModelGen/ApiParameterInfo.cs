using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// api参数表
    /// </summary>
    public class ApiParameterInfo:ZentCloud.ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 参数id
        /// </summary>
        public int AutoID { get; set; }

        /// <summary>
        /// 对应api主表的autoid
        /// </summary>
        public int ApiId { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 是否必传  0否   1是
        /// </summary>
        public int IsNull { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
