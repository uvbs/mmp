using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// ZCJ_UserCreditAcountDetails
    /// </summary>
    [Serializable]
    public partial class UserCreditAcountDetails : ModelTable
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 信用金 或增加的金额
        /// </summary>
        public decimal CreditAcount { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string AddNote { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 用户剩余信用
        /// </summary>
        public decimal UserCreditAcount { get; set; }

        /// <summary>
        /// 系统类型
        ///空或 CreditAcount 信用金
        ///AccountAmount 账户 余额
        /// </summary>
        public string SysType { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }

    }
}
