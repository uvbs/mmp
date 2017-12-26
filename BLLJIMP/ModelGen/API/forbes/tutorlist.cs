using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model.API.forbes;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{
    /// <summary>
    /// 专家模型
    /// </summary>
    public class Tutor : MasterModel
    {
        /// <summary>
        /// 最新动态ID
        /// </summary>
        public int newactivityid { get; set; }
        /// <summary>
        /// 最新动态标题
        /// </summary>
        public string newactivityname { get; set; }
    }
    /// <summary>
    /// 专家列表模型
    /// </summary>
    public class TutorListrApi
    {
        public TutorListrApi()
        {
            list = new List<Tutor>();
        }
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 分类集合
        /// </summary>
        public List<Tutor> list { get; set; }
    }
}
