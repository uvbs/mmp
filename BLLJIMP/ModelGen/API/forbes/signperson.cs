using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{
    /// <summary>
    /// 已经报名人模型
    /// </summary>
    public class SignPerson
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string headimg { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 报名时间
        /// </summary>
        public double time { get; set; }

        public string userId { get; set; }
        public string signupTime { get; set; }

        public string openId { get; set; }

    }
    /// <summary>
    /// 已经报名人 api模型
    /// </summary>
    public class SignPersonApi {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 报名人集合
        /// </summary>
        public List<SignPerson> list { get; set; }
    }



}
