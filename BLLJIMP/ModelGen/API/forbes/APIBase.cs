using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.ModelGen.API.forbes
{
    /// <summary>
    /// API返回基类
    /// </summary>
    public class APIBase
    {
        /// <summary>
        /// 错误码，对应于枚举类：ZentCloud.BLLJIMP.Enums.APIErrCode
        /// </summary>
        public int errcode { get; set; }
    }
}
