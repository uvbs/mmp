using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 分销银行卡绑定信息表
    /// </summary>
    public class BindBankCard : ZCBLLEngine.ModelTable
    {
        public BindBankCard()
        { }
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
        /// 申请人用户名
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 银行代码 
        /// </summary>
        public string BankCode { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
    }
}
