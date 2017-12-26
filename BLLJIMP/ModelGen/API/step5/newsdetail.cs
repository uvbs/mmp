using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{

    /// <summary>
    /// api 资讯详情模型
    /// </summary>
    public class ArticleDetail
    {
        /// <summary>
        /// 文章编号
        /// </summary>
        public int articleid { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string digest { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string articlecontent { get; set; }
        /// <summary>
        /// 时间
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
        /// <summary>
        /// 是否已经赞过了
        /// </summary>
        public bool ispraise { get; set; }
    }


}
