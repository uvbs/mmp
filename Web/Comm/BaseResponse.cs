using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.JubitIMP.Web
{
    /// <summary>
    /// 前端系统级API返回
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool status { get; set; }
        /// <summary>
        /// 状态码，对应枚举 APIErrCode
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 系统信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 业务层结果
        /// </summary>
        public dynamic result { get; set; }
        
    }

    public class EasyUIResponse : BaseResponse
    {
        public int total { get; set; }

        public dynamic rows { get; set; }
    }
}
