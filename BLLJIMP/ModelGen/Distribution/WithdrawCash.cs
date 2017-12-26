using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 分销提现申请表
    /// </summary>
    public class WithdrawCash : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }

        /// <summary>
        /// 银行开户名
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        public string BankAccount { get; set; }
        /// <summary>
        /// 开户银行名称
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 开户网点
        /// </summary>
        public string AccountBranchName { get; set; }

        /// <summary>
        /// 开户行省份
        /// </summary>
        public string AccountBranchProvince { get; set; }
        /// <summary>
        /// 开户行所在市
        /// </summary>
        public string AccountBranchCity { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 服务费
        /// </summary>
        public decimal ServerFee { get; set; }
        /// <summary>
        /// 实际提现金额 扣除服务费
        /// </summary>
        public decimal RealAmount { get; set; }
        /// <summary>
        /// 对公私标识 1对公 2对私
        /// </summary>
        public int IsPublic { get; set; }
        /// <summary>
        /// 0 代表待审核 1代表已受理 2代表成功 3代表失败
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 申请人用户名
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 申请人姓名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 申请人手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdateDate { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get;set; }
        /// <summary>
        /// 提现类型
        /// DistributionOffLine 线下分销
        /// DistributionOnLine  线上分销
        /// ScoreOnLine  积分提现
        /// </summary>
        public string WithdrawCashType { get; set; }

        /// <summary>
        ///提现方式
        ///0 银行卡
        ///1 微信
        ///2 账户余额
        /// </summary>
        public int TransfersType { get; set; }

        /// <summary>
        /// 提现积分
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// 外部交易Id
        /// </summary>
        public string TranId { get; set; }


    }
}
