using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeOutSDK.Eleme.Model
{
    public class AuthorizeResponse
    {
        /// <summary>
        /// 访问令牌，API调用时需要，请开发者全局保存，不要单机保存
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 令牌类型，固定值，开发者可忽略
        /// </summary>
        public string token_type { get; set; }

        /// <summary>
        /// 令牌的有效时间，单位秒
        /// </summary>

        public double expires_in { get; set; }
    }
}
