using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.User
{
    /// <summary>
    /// 当前用户信息
    /// </summary>
   public class UserInfo
    {
       /// <summary>
       /// 当前用户名
       /// </summary>
       public string userid { get; set; }
       /// <summary>
       /// 当前手机号
       /// </summary>
       public string phone { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public double totalscore { get; set; }
    }
}
