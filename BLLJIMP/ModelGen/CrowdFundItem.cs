using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 众筹选项表
    /// </summary>
    public class CrowdFundItem : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 选项Id
        /// </summary>
        public int ItemId { get; set; }
        /// <summary>
        /// 众筹项目ID 关联ZCJ_CrowdFundInfo 表AutoID 关联主表ID
        /// </summary>
        public int CrowdFundID { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 回报方式,商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 选项类型
        /// 空 众筹
        /// Activity 活动报名
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriginalPrice { get; set; }


    }
}
