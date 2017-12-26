using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 用户等级配置
    /// </summary>
    public class UserLevelConfig : ZCBLLEngine.ModelTable
    {
        public int? AutoId { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int LevelNumber { get; set; }
        /// <summary>
        /// 等级名称
        /// </summary>
        public string LevelString { get; set; }
        /// <summary>
        /// 等级图标
        /// </summary>
        public string LevelIcon { get; set; }
        /// <summary>
        /// 历史积分下限 
        /// 累计佣金金额开始
        /// 购买会员时的价格
        /// </summary>
        public double FromHistoryScore { get; set; }
        /// <summary>
        /// 历史积分上限 累计佣金金额结束
        /// </summary>
        public double ToHistoryScore { get; set; }
        /// <summary>
        /// 商城折扣
        /// </summary>
        public double Discount { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }
        /// <summary>
        /// 直销提成比例 首单
        /// </summary>
        public string DistributionRateLevel0First { get; set; }
        /// <summary>
        /// 直销提成比例 续单
        /// </summary>
        public string DistributionRateLevel0 { get; set; }
        /// <summary>
        /// 一级分销提成比例 首单
        /// </summary>
        public string DistributionRateLevel1First { get; set; }
        /// <summary>
        /// 一级分销提成比例 续单
        /// </summary>
        public string DistributionRateLevel1 { get; set; }
        /// <summary>
        /// 二级分销提成比例 无用
        /// </summary>
        public string DistributionRateLevel2 { get; set; }
        /// <summary>
        /// 三级分销提成比例 无用
        /// </summary>
        public string DistributionRateLevel3 { get; set; }
        /// <summary>
        /// 等级类型
        /// DistributionOffLine 业务分销
        /// DistributionOnLine 商城分销
        /// CommonScore 能用积分
        /// </summary>
        public string LevelType { get; set; }
        /// <summary>
        /// 渠道分销提成比例
        /// </summary>
        public string ChannelRate { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 直销获得返积分比例，默认0
        /// </summary>
        public string RebateScoreRate { get; set; }
        /// <summary>
        /// 一级分销比例 另一部分（颂和 购房补助）
        /// </summary>
        public string DistributionRateLevel1Ex1 { get; set; }
        /// <summary>
        /// 公积金比例（颂和 公积金比例）
        /// </summary>
        public string AccumulationFundRateLevel1 { get; set; }
        /// <summary>
        /// 会员分佣比例
        /// </summary>
        public string RebateMemberRate { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public int IsDisable { get; set; }
        /// <summary>
        /// 供应商提成比例
        /// </summary>
        public string SupplierRate { get; set; }
        /// <summary>
        /// 奖励金额
        /// </summary>
        public decimal AwardAmount { get; set; }
        /// <summary>
        /// 优惠券id
        /// </summary>
        public string CouponId { get; set; }
    }
}
