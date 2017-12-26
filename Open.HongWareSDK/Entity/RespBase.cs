using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.HongWareSDK
{
    /// <summary>
    /// 接口响应基类
    /// </summary>
    public class RespBase
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool isSuccess { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public Map map { get; set; }
    }
    /// <summary>
    /// 基本响应模型
    /// </summary>
    public class Map
    {

        /// <summary>
        /// 错误编码
        /// </summary>
        public string errorCode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errorNumber { get; set; }
        /// <summary>
        /// 错误信息中文
        /// </summary>
        public string errorMsg { get; set; }



    }

}
