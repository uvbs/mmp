using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{
    /// <summary>
    ///hrlove详情
    /// </summary>
    public class HrLoveDetail
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        public HrLove currentuser { get; set; }
        /// <summary>
        /// 上一级用户
        /// </summary>
        public PreNextUser preuser { get; set; }
        /// <summary>
        /// 下一级用户1
        /// </summary>
        public PreNextUser nextuser1 { get; set; }
        /// <summary>
        /// 下一级用户2
        /// </summary>
        public PreNextUser nextuser2 { get; set; }

    }
    /// <summary>
    /// 上一级用户或下级用户模型
    /// </summary>
    public class PreNextUser {
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string headimg { get; set; }
    
    }



}
