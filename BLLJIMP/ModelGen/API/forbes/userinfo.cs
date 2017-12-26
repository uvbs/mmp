using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{
    /// <summary>
    /// api用户资料模型
    /// </summary>
    public class UserInfoModel
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string headimg { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string truename { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string postion { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string company { get; set; }
        /// <summary>
        /// 剩余积分
        /// </summary>
        public double totalscore { get; set; }

    }
}
