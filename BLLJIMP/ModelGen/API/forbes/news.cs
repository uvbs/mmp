using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{
    /// <summary>
    /// api 资讯模型
    /// </summary>
    public class News
    {
        /// <summary>
        /// 资讯编号
        /// </summary>
        public int newsid { get; set; }
        /// <summary>
        /// 资讯标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 资讯描述
        /// </summary>
        public string digest { get; set; }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public double time { get; set; }
        /// <summary>
        /// 阅读量
        /// </summary>
        public int pv { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string categoryname { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        public int cateid { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string imgurl { get; set; }

    }

    /// <summary>
    ///资讯返回结果模型
    /// </summary>
    public class NewsApi
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 集合
        /// </summary>
        public List<News> list { get; set; }
    }

}
