using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 美帆活动 (活动,比赛,培训共用)
    /// </summary>
    public class MeifanActivity : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 活动ID
        /// </summary>
        public string ActivityId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string ActivityName { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string ActivityImg { get; set; }
        /// <summary>
        /// 概要
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        ///类型
        ///activity 活动
        ///match 比赛
        ///train 培训
        /// </summary>
        public string ActivityType { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 是否需要支付
        /// 0 需要
        /// 1 不需要
        /// </summary>
        public int IsNeedPay { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string Websiteowner { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDelete { get; set; }
        /// <summary>
        /// 是否已经发布
        /// </summary>
        public int IsPublish { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 最多报名人数
        /// </summary>
        public int MaxSignUpCount { get; set; }
        /// <summary>
        /// 要点
        /// </summary>
        public string MainPoints { get; set; }

    }
}
