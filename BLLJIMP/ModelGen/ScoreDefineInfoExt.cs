using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 积分规则扩展
    /// </summary>
    public class ScoreDefineInfoExt : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
       /// <summary>
       /// 积分规则Id
       /// </summary>
        public int ScoreId { set; get; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 比例值
        /// </summary>
        public decimal RateValue { get; set; }
        /// <summary>
        /// 比例值对应积分
        /// </summary>
        public decimal RateScore { get; set; }


        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTimeStr { get { return BeginTime.ToString("yyyy-MM-dd HH:mm:ss"); } }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTimeStr { get { return EndTime.ToString("yyyy-MM-dd HH:mm:ss"); } }
    }
}
