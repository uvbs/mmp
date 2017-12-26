using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 积分冻结表        
    /// </summary>
    public class ScoreLockInfo : ZCBLLEngine.ModelTable
    {
        // 冻结积分表(AutoId，冻结类型：如下单获得积分冻结，关联Id：跟着类型走如订单的冻结为订单id，状态，用户Id，站点Id，冻结时间，解冻时间，取消时间，取消原因）

        /// <summary>
        /// 
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 冻结类型：1 下单获得积分冻结 2分佣获得佣金冻结
        /// </summary>
        public int LockType { get; set; }
        /// <summary>
        /// 关联Id：跟着类型走如订单的冻结为订单id
        /// </summary>
        public string ForeignkeyId { get; set; }
        /// <summary>
        /// 关联Id2：分佣明细ID
        /// </summary>
        public string ForeignkeyId2 { get; set; }
        /// <summary>
        /// 状态:0冻结、1解冻、2取消、3解冻撤销
        /// </summary>
        public int LockStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FromUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LockTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UnLockTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CancelTime { get; set; }
        /// <summary>
        /// 备注，有需要备注的内容追加处理
        /// </summary>
        public string Memo { get; set; }


        #region ModelEx
        /// <summary>
        /// 
        /// </summary>
        public string LockTimeStr
        {
            get
            {
                return LockTime.ToString("yyyy-MM-dd hh:mm");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UnLockTimeStr
        {
            get
            {
                if (UnLockTime == null)
                {
                    return "";
                }

                return UnLockTime.Value.ToString("yyyy-MM-dd hh:mm");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CancelTimeStr
        {
            get
            {
                if (CancelTime == null)
                {
                    return "";
                }

                return CancelTime.Value.ToString("yyyy-MM-dd hh:mm");
            }
        }

        #endregion

    }
}
