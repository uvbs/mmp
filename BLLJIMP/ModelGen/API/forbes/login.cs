using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{
    /// <summary>
    /// 用户登录模型
    /// </summary>
    public class Login : ZentCloud.BLLJIMP.ModelGen.API.forbes.APIBase
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
        /// 头像（兼容老版本保留）
        /// </summary>
        public string headimg { get; set; }
        /// <summary>
        /// userName
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// autoid
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 云信token
        /// </summary>
        public string im_token { get; set; }
        /// <summary>
        /// TotalScore 积分
        /// </summary>
        public double score { get; set; }

    }
}
