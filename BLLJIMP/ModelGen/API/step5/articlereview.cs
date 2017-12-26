using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{

    /// <summary>
    /// 文章评论
    /// </summary>
   public class ArticleReview
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string headimg { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string reviewcontent { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public double time { get; set; }
        /// <summary>
        /// 是否显示删除标记
        /// </summary>
        public bool deleteflag { get; set; }

        /// <summary>
        /// 评论目标
        /// </summary>
        public ArticleReplyReview reply { get; set; }
        ///// <summary>
        ///// 回复数量
        ///// </summary>
        //public int replycount { get; set; }
        ///// <summary>
        ///// 回复列表
        ///// </summary>
        //public List<articlereplyreview> replylist { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }


    }

   /// <summary>
   ///活动api返回结果模型
   /// </summary>
   public class ArticleReviewApi
   {
       /// <summary>
       /// 总数
       /// </summary>
       public int totalcount { get; set; }
       /// <summary>
       /// 集合
       /// </summary>
       public List<ArticleReview> list { get; set; }
   }
}
