using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Other
{
    /// <summary>
    /// 微信自动登陆token，存redis
    /// </summary>
    [Serializable]
    public class WxAutoLoginToken
    {
        /// <summary>
        /// UserId
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// 是否高级授权过
        /// </summary>
        public int IsUAuth { get; set; }
        /// <summary>
        /// openid
        /// </summary>
        public string Oid { get; set; }
    }
}
