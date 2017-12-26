using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EZRproSDK.Entity
{
    public class RespBase<T>
    {
        public string AppId { get; set; }
        /// <summary>
        /// 结果状态，true代表成功，false代表失败
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 返回操作的状态编码，详见statusCode规范
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// 返回操作的消息描述
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 响应时间戳(yyyyMMddHHmmss格式)
        /// </summary>
        public string Timestamp { get; set; }
        /// <summary>
        /// 响应签名，详见签名算法
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 返回的数据实体，Json对象（单个对象或对象数组）
        /// </summary>
        public T Result { get; set; }
    }
}
