using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{
    /// <summary>
    ///问答模型
    /// </summary>
    public class Ask
    {

        /// <summary>
        /// 编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string headimg { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public double time { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int pv { get; set; }
        /// <summary>
        /// 转发数量
        /// </summary>
        public int sharecount { get; set; }


    }
    /// <summary>
    /// 问答列表api模型
    /// </summary>
    public class AskListApi
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 集合
        /// </summary>
        public List<Ask> list { get; set; }
    }


}
