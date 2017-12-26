using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{
    /// <summary>
    /// 理财师基本模型
    /// </summary>
    public class MasterBase
    {
        /// <summary>
        /// 理财师头像
        /// </summary>
        public string headimg { get; set; }
        /// <summary>
        /// 理财师用户名
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 理财师姓名
        /// </summary>
        public string truename { get; set; }
        /// <summary>
        /// 理财师职位
        /// </summary>
        public string postion { get; set; }
        /// <summary>
        /// 理财师简要介绍
        /// </summary>
        public string digest { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int pv { get; set; }
        /// <summary>
        /// 赞数量
        /// </summary>
        public int praisecount { get; set; }
        /// <summary>
        /// 标签列表
        /// </summary>
        public List<string> tags { get; set; }

    }


}
