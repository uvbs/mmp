using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class ScoreDefineInfo : ZCBLLEngine.ModelTable
    {
        public ScoreDefineInfo()
        { }
        public int ScoreId { set; get; }
        /// <summary>
        /// 积分类型
        /// </summary>
        public string ScoreType { set; get; }
        /// <summary>
        /// 类型中文
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { set; get; }
        /// <summary>
        /// 分值
        /// </summary>
        public double Score { set; get; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { set; get; }
        /// <summary>
        /// 建立人
        /// </summary>
        public string CreateUserId { set; get; }
        /// <summary>
        /// 建立时间
        /// </summary>
        public DateTime InsertTime { set; get; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public int IsHide { set; get; }
        
        /// <summary>
        /// 每日上限
        /// </summary>
        public double DayLimit { set; get; }
        /// <summary>
        /// 总上限
        /// </summary>
        public double TotalLimit { set; get; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNum { set; get; }
        /// <summary>
        /// 扩展关联ID
        /// </summary>
        public string Ex1 { set; get; }

        /// <summary>
        /// 积分事件
        /// </summary>
        public string ScoreEvent { get; set; }
        /// <summary>
        /// 基础比例值
        /// </summary>
        public decimal BaseRateValue { get; set; }
        /// <summary>
        /// 基础比例值对应积分
        /// </summary>
        public decimal BaseRateScore { get; set; }
    }
}
