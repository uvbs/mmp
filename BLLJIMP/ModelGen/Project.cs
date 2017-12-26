using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 项目
    /// </summary>
    public partial class Project : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 用户名  
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 项目类型
        /// DistributionOffLine(线下分销)
        /// </summary>
        public string ProjectType { get; set; }
        /// <summary>
        /// 是否已经完成 已经完成的订单不可再次打款
        /// </summary>
        public int IsComplete { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        public string WeiXin { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 备注 原因
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string Ex1 { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string Ex2 { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string Ex3 { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string Ex4 { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string Ex5 { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string Ex6{ get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string Ex7 { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string Ex8 { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string Ex9 { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string Ex10 { get; set; }
        public string Ex11 { get; set; }
        public string Ex12 { get; set; }
        public string Ex13 { get; set; }
        public string Ex14 { get; set; }
        public string Ex15 { get; set; }
        public string Ex16 { get; set; }
        public string Ex17 { get; set; }
        public string Ex18 { get; set; }
        public string Ex19 { get; set; }
        public string Ex20 { get; set; }

    }
}
