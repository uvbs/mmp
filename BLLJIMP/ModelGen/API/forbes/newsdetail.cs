using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{

    /// <summary>
    /// api 资讯详情模型
    /// </summary>
    public class NewsDetail
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
        /// 资讯内容
        /// </summary>
        public string newscontent { get; set; }
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
        /// 图片
        /// </summary>
        public string imgurl { get; set; }
    }


}
