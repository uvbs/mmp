using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{
    /// <summary>
    /// 文章评论目标
    /// </summary>
   public class ArticleReplyReview
    {
        ///// <summary>
        ///// 自动编号
        ///// </summary>
        //public int id { get; set; }
        ///// <summary>
        ///// 头像
        ///// </summary>
        //public string headimg { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string reviewcontent { get; set; }
        ///// <summary>
        ///// 评论时间
        ///// </summary>
        //public double time { get; set; }
        ///// <summary>
        ///// 是否显示删除标记
        ///// </summary>
        //public bool deleteflag { get; set; }
    }
}
