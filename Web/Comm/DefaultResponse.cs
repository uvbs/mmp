using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web
{
    //public get
    /// <summary>
    /// 默认响应模型
    /// </summary>
    public class DefaultResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool isSuccess { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }

        /// <summary>
        /// 返回值
        /// </summary>
        public string returnValue { get; set; }
        /// <summary>
        /// 返回对象
        /// </summary>
        public dynamic returnObj { get; set; }

    }

}