using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 五步会-五伴会
    /// </summary>
    public class WBHPartnerInfo : ZCBLLEngine.ModelTable
    {
        public WBHPartnerInfo() { }

        public int AutoId { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string PartnerName { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        public string PartnerImg { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string PartnerType { get; set; }

        /// <summary>
        /// 阅读量
        /// </summary>
        public int PartnerPv { get; set; }

        /// <summary>
        /// 公司简介
        /// </summary>
        public string PartnerContext { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        public string PartnerAddress { get; set; }

        /// <summary>
        /// 点赞
        /// </summary>
        public int ParTnerStep { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? InsertDate { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }

        public List<ArticleCategory> Ctype { get; set; }

    }
}
