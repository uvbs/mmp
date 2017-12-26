using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// ZCJ_TeamPerformanceDetails
    /// </summary>
    [Serializable]
    public partial class TeamPerformanceDetails : ModelTable
    {
        /// <summary>
        /// AutoId
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public int YearMonth { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户手机
        /// </summary>
        public string UserPhone { get; set; }
        /// <summary>
        /// 推荐人
        /// </summary>
        public string DistributionOwner { get; set; }
        /// <summary>
        /// 个人消费业绩
        /// </summary>
        public decimal Performance { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string AddNote { get; set; }
        /// <summary>
        /// 注册
        /// 升级
        /// 撤单
        /// 变更
        /// </summary>
        public string AddType { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
    }
}