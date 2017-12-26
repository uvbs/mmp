using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ZentCloud.BLLJIMP.Model.API
{
    /// <summary>
    /// 响应
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonProperty("status")]
        public bool Status { get; set; }
        /// <summary>
        /// 状态码，对应枚举 APIErrCode
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }
        /// <summary>
        /// 系统信息
        /// </summary>
        [JsonProperty("msg")]
        public string Msg { get; set; }
        /// <summary>
        /// 业务层结果
        /// </summary>
        [JsonProperty("result")]
        public dynamic Result { get; set; }
        
        #region 兼容老字段
        public int errcode
        {
            get
            {
                return Code;
            }
        }

        public string errmsg
        {
            get
            {
                return Msg;
            }
        }
        #endregion
        
        
    }
}
