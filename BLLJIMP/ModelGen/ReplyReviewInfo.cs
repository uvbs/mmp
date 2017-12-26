using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 评论
    /// </summary>
    public class ReplyReviewInfo : ZCBLLEngine.ModelTable
    {
        public ReplyReviewInfo() { }

        public int AutoId { get; set; }
        /// <summary>
        /// 评论编号
        /// </summary>
        public int ReviewID { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string ReplyContent { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// 父类编号
        /// </summary>
        public int PraentId { get; set; }

        //头像
        public string Img { get; set; }

        //微信名称
        public string NickName { get; set; }

        public int HTNum { get; set; }

        /// <summary>
        /// 用户等级
        /// </summary>
        public int UserLevel { get; set; }
        /// <summary>
        /// 是否是导师 1导师
        /// </summary>
        public int IsTutor { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string ReviewType { get; set; }

        /// <summary>
        /// 0 待审核
        /// 1 审核通过
        /// 2 审核不通过
        /// </summary>
        public int Status { get; set; }
    }
}
