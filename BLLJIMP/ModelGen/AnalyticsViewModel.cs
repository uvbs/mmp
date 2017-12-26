using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 数据分析视图数据实体
    /// </summary>
    public class AnalyticsViewModel
    {
        /// <summary>
        /// 新用户今日
        /// </summary>
        public int NewUserToday { get; set; }
        public int NewUserYesterday { get; set; }
        public int NewUserThisWeek { get; set; }
        public int NewUserThisMonth { get; set; }
        public int NewUserAll { get; set; }

        /// <summary>
        /// pc首页访问IP
        /// </summary>
        public int PCIndexViewIPToday { get; set; }
        public int PCIndexViewIPYesterday { get; set; }
        public int PCIndexViewIPThisWeek { get; set; }
        public int PCIndexViewIPThisMonth { get; set; }
        public int PCIndexViewIPAll { get; set; }

        /// <summary>
        /// pc首页访问IP
        /// </summary>
        public int PCIndexViewUVToday { get; set; }
        public int PCIndexViewUVYesterday { get; set; }
        public int PCIndexViewUVThisWeek { get; set; }
        public int PCIndexViewUVThisMonth { get; set; }
        public int PCIndexViewUVAll { get; set; }


        /// <summary>
        /// PC首页访问UV
        /// </summary>
        public int PCIndexViewPVToday { get; set; }
        public int PCIndexViewPVYesterday { get; set; }
        public int PCIndexViewPVThisWeek { get; set; }
        public int PCIndexViewPVThisMonth { get; set; }
        public int PCIndexViewPVAll { get; set; }

        public int MobileIndexViewIPToday { get; set; }
        public int MobileIndexViewIPYesterday { get; set; }
        public int MobileIndexViewIPThisWeek { get; set; }
        public int MobileIndexViewIPThisMonth { get; set; }
        public int MobileIndexViewIPAll { get; set; }

        public int MobileIndexViewUVToday { get; set; }
        public int MobileIndexViewUVYesterday { get; set; }
        public int MobileIndexViewUVThisWeek { get; set; }
        public int MobileIndexViewUVThisMonth { get; set; }
        public int MobileIndexViewUVAll { get; set; }

        public int MobileIndexViewPVToday { get; set; }
        public int MobileIndexViewPVYesterday { get; set; }
        public int MobileIndexViewPVThisWeek { get; set; }
        public int MobileIndexViewPVThisMonth { get; set; }
        public int MobileIndexViewPVAll { get; set; }

        public int ArticleViewIPToday { get; set; }
        public int ArticleViewIPYesterday { get; set; }
        public int ArticleViewIPThisWeek { get; set; }
        public int ArticleViewIPThisMonth { get; set; }
        public int ArticleViewIPAll { get; set; }

        public int ArticleViewUVToday { get; set; }
        public int ArticleViewUVYesterday { get; set; }
        public int ArticleViewUVThisWeek { get; set; }
        public int ArticleViewUVThisMonth { get; set; }
        public int ArticleViewUVAll { get; set; }

        public int ArticleViewPVToday { get; set; }
        public int ArticleViewPVYesterday { get; set; }
        public int ArticleViewPVThisWeek { get; set; }
        public int ArticleViewPVThisMonth { get; set; }
        public int ArticleViewPVAll { get; set; }

        public int ArticlePubToday { get; set; }
        public int ArticlePubYesterday { get; set; }
        public int ArticlePubThisWeek { get; set; }
        public int ArticlePubThisMonth { get; set; }
        public int ArticlePubAll { get; set; }

        public int ActivityPubToday { get; set; }
        public int ActivityPubYesterday { get; set; }
        public int ActivityPubThisWeek { get; set; }
        public int ActivityPubThisMonth { get; set; }
        public int ActivityPubAll { get; set; }

        public int ShareToday { get; set; }
        public int ShareYesterday { get; set; }
        public int ShareThisWeek { get; set; }
        public int ShareThisMonth { get; set; }
        public int ShareAll { get; set; }

        public int SignUpToday { get; set; }
        public int SignUpYesterday { get; set; }
        public int SignUpThisWeek { get; set; }
        public int SignUpThisMonth { get; set; }
        public int SignUpAll { get; set; }

    }
}
