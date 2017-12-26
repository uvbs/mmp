using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Enums
{
    /// <summary>
    /// 评论表类型键
    /// </summary>
    public enum ReviewTypeKey
    {
        /// <summary>
        /// 文章回复，外键为文章id, Expand1为冗余的文章id
        /// </summary>
        ArticleComment,
        /// <summary>
        /// 评论回复,外键为评论id，父id为回复的id Expand1为冗余的文章id
        /// </summary>
        CommentReply,
        /// <summary>
        /// 回答，外键id为 问题id
        /// </summary>
        Answer,
        /// <summary>
        /// 约会评论，外键id为 问题id
        /// </summary>
        AppointmentComment,
        /// <summary>
        /// 订单评论，外键id为订单id , Expand1为商品id
        /// </summary>
        OrderComment
    }
}
