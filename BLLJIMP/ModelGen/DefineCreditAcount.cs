using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class DefineCreditAcount : ModelTable
    {
        public int AutoID { set; get; }
        /// <summary>
        /// 积分类型
        /// </summary>
        public string Type { set; get; }
        /// <summary>
        /// 类型中文
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { set; get; }
        /// <summary>
        /// 信用金
        /// </summary>
        public decimal CreditAcount { set; get; }
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
        public decimal DayLimit { set; get; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNum { set; get; }
    }
}
