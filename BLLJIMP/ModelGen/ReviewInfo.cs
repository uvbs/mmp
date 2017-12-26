using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 评论，话题表
    /// </summary>
    public class ReviewInfo : ZCBLLEngine.ModelTable
    {
        #region ModelBase

        public ReviewInfo() { }

        /// <summary>
        ///编号   
        /// </summary>
        public int AutoId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 评论内容    
        /// </summary>
        public string ReviewContent { get; set; }

        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        public string InsertDateStr { get { return InsertDate.ToString("yyyy-MM-dd HH:mm"); } }

        /// <summary>
        /// 关联编号
        /// </summary>
        public string ForeignkeyId { get; set; }

        /// <summary>
        /// 回复次数
        /// </summary>
        public int NumCount { get; set; }

        /// <summary>
        /// 顶
        /// </summary>
        public int PraiseNum { get; set; }

        /// <summary>
        /// 踩
        /// </summary>
        public int StepNum { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string ForeignkeyName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string ReviewType { get; set; }

        /// <summary>
        /// 话题名称
        /// </summary>
        public string ReviewTitle { get; set; }

        /// <summary>
        /// 权限 0表示公开 1表示不公开
        /// </summary>
        public int ReviewPower { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string CategoryType { get; set; }

        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 最新回复 时间
        /// </summary>
        public DateTime? ReplyDateTiem
        {
            get;
            set;
        }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int Pv { get; set; }

        /// <summary>
        /// 信息是否已阅读，1阅读，0或NULL未阅读
        /// </summary>
        public int IsRead { get; set; }

        /// <summary>
        /// 信息的父信息ID，为0或为空表示一个新的信息（话题，评论，。。。)
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 信息是否通过审核，1为通过，0或空为未审核 2不通过
        /// </summary>
        public int AuditStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 扩展1
        /// </summary>
        public string Expand1 { get; set; }
        /// <summary>
        /// 扩展2
        /// </summary>
        public string Ex2 { get; set; }

        public string AuditStatusString
        {
            get
            {
                switch (AuditStatus)
                {
                    case 0:
                        return "未审核";
                    case 1:
                        return "审核通过";
                    case 2:
                        return "审核拒绝";
                    default:
                        return "未审核";
                }
            }
        }

        public List<BLLJIMP.Model.ReplyReviewInfo> rrInfos { get; set; }

        public List<BLLJIMP.Model.ArticleCategory> actegory { get; set; }

        /// <summary>
        /// 评论主id
        /// </summary>
        public int ReviewMainId { get; set; }

        /// <summary>
        /// 是否匿名评论
        /// </summary>
        public int IsHideUserName { get; set; }
        /// <summary>
        /// 评价分
        /// </summary>
        public double ReviewScore { get; set; }

        /// <summary>
        /// 评论图片
        /// </summary>
        public string CommentImg { get; set; }
        #endregion

        #region ModelEx

        public bool CurrUserIsPraise { get; set; }

        public bool CurrUserIsFavorite { get; set; }

        /// <summary>
        /// 回复数
        /// </summary>
        public int ReplyCount { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int PraiseCount { get; set; }
        /// <summary>
        /// 收藏数
        /// </summary>
        public int FavoriteCount { get; set; }
        /// <summary>
        /// 发布用户
        /// </summary>
        public UserInfo PubUser { get; set; }
        /// <summary>
        /// 回复的用户
        /// </summary>
        public UserInfo ReplayToUser { get; set; } 
        #endregion
    }
}
