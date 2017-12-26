using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    ///  投票   
    /// </summary>
    [Serializable]
    public class TheVoteInfo : ZCBLLEngine.ModelTable
    {

        public TheVoteInfo() { }
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 投票名称
        /// </summary>
        public string VoteName { get; set; }
        /// <summary>
        ///  封面图片
        /// </summary>
        public string VoteImg { get; set; }
        /// <summary>
        /// 是否公开
        /// </summary>
        public string IsVoteOpen { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string VoteContent { get; set; }

        /// <summary>
        /// 1.页首 2 中间 3页尾
        /// </summary>
        public string VotePosition { get; set; }

        /// <summary>
        /// 1.单选 2.多选  3问答
        /// </summary>
        public string VoteSelect { get; set; }

        /// <summary>
        ///  插入时间
        /// </summary>
        public DateTime InsetDate { get; set; }

        /// <summary>
        ///  插入时间
        /// </summary>
        public string InsetDateStr { get { return InsetDate.ToString("yyyy-MM-dd"); } }
        /// <summary>
        ///  票数
        /// </summary>
        public int VoteNumbers { get; set; }

        /// <summary>
        ///  投票人数
        /// </summary>
        public int PNumber { get; set; }

        /// <summary>
        ///  站点
        /// </summary>
        public string websiteOwner { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime TheVoteOverDate { get; set; }

        /// <summary>
        ///  插入时间
        /// </summary>
        public string TheVoteOverDateStr { get { return TheVoteOverDate.ToString("yyyy-MM-dd HH:mm:ss"); } }


        /// <summary>
        /// 投票GUID
        /// </summary>
        public string TheVoteGUID { get; set; }
        /// <summary>
        /// 投票选项
        /// </summary>
        public List<DictionaryInfo> diInfos { get; set; }

        /// <summary>
        /// 多选最多可以同时选多少个选项
        /// </summary>
        public int MaxSelectItemCount { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int PV { get; set; }
        /// <summary>
        /// ip
        /// </summary>
        public int IP { get; set; }
        /// <summary>
        /// 阅读人数
        /// </summary>
        public int UV { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string ThumbnailsPath { get; set; }
            
    }
}
