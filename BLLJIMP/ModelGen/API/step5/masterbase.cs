using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{
    /// <summary>
    /// 导师基本模型
    /// </summary>
    public class MasterBase
    {
        /// <summary>
        /// 导师头像
        /// </summary>
        public string headimg { get; set; }
        /// <summary>
        /// 导师用户名
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 导师姓名
        /// </summary>
        public string truename { get; set; }
        /// <summary>
        /// 导师职位
        /// </summary>
        public string position { get; set; }
        /// <summary>
        /// 导师简要介绍
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
        /// 行业标签列表
        /// </summary>
        public List<string> tradetags { get; set; }
        /// <summary>
        /// 专业标签列表
        /// </summary>
        public List<string> professionaltags { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 文章数量
        /// </summary>
        public int articlecount { get; set; }
    }


}
