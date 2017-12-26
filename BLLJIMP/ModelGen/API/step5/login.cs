using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{
    /// <summary>
    /// 用户登录模型
    /// </summary>
    public class Login
    {
        /// <summary>
        /// 是否已经登录
        /// </summary>
        public bool issuccess { get; set; }
        /// <summary>
        /// 信息提示
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string headimg { get; set; }


    }
}
