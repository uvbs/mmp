using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{
    /// <summary>
    /// 用户是否登录模型
    /// </summary>
    public class CheckLogin
    {
        /// <summary>
        /// 是否已经登录
        /// </summary>
        public bool islogin { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string userid { get; set; }


    }
}
