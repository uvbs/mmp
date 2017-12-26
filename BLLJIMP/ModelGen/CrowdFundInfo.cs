using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 众筹信息表
    /// </summary>
    public partial class CrowdFundInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 众筹编号
        /// </summary>
        public int CrowdFundID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        public string CoverImage { get; set; }
        /// <summary>
        /// 筹集金额
        /// </summary>
        public decimal FinancAmount { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime StopTime { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int PV { get; set; }
        /// <summary>
        /// 转发量
        /// </summary>
        public int ShareCount { get; set; }
        /// <summary>
        /// 付款时 姓名是否必填 0选填 1必填
        /// </summary>
        public int NameRequired { get; set; }
        /// <summary>
        /// 付款时 手机是否必填 0选填 1必填
        /// </summary>
        public int PhoneRequired { get; set; }
        /// <summary>
        /// 付款时 公司名称是否必填 0选填 1必填
        /// </summary>
        public int CompanyRequired { get; set; }
        /// <summary>
        /// 付款时 职位是否必填 0选填 1必填
        /// </summary>
        public int PositionRequired { get; set; }
        /// <summary>
        /// 已经筹集金额 显示给用户的文字
        /// </summary>
        public string HaveFinancAmountText { get; set; }
        /// <summary>
        /// 总筹集金额 显示给用户的文字
        /// </summary>
        public string FinancAmountText { get; set; }
        /// <summary>
        /// 参与人员 显示给用户的文字
        /// </summary>
        public string JoinPersonnelText { get; set; }
        /// <summary>
        /// 我要付款 显示给用户的文字
        /// </summary>
        public string PaymentText { get; set; }
        /// <summary>
        /// 我要发起众筹显示给用户的文字
        /// </summary>
        public string AddCrowdFundText { get; set; }
        /// <summary>
        /// 分享显示给用户的文字
        /// </summary>
        public string ShareText { get; set; }
        /// <summary>
        /// 状态 0代表已经停止 1代表正在进行
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }
        /// <summary>
        /// 单位价格
        /// </summary>
        public int UnitPrice { get; set; }
        /// <summary>
        /// 产品类型
        /// 0 产品众筹
        /// 1 股权众筹
        /// 2 公益众筹
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 发起人
        /// </summary>
        public string Originator { get; set; }

    }
}
