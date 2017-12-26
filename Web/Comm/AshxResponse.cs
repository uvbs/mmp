using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web
{
    /// <summary>
    /// ashx统一回复响应 
    /// </summary>
    public class AshxResponse
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }
        public string msg
        {
            get
            {
                return Msg;
            }
        }
        /// <summary>
        /// 扩展对象
        /// </summary>
        public object ExObj { get; set; }
        /// <summary>
        /// 扩展字符串
        /// </summary>
        public string ExStr { get; set; }
        /// <summary>
        /// 扩展数值
        /// </summary>
        public int ExInt { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public object Result { get; set; }

    }
}