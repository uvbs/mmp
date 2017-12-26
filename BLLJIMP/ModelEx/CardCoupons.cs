using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class CardCoupons
    {
        BLLCardCoupon bllCardCoupon = new BLLCardCoupon();
        /// <summary>
        /// 已经发放数量
        /// </summary>
        public int SendCount
        {
            get
            {

                return bllCardCoupon.GetCount<MyCardCoupons>(string.Format(" CardId='{0}'", CardId));

            }
        }
        /// <summary>
        /// 已经使用数量
        /// </summary>
        public int UsedCount
        {
            get
            {

                return bllCardCoupon.GetCount<MyCardCoupons>(string.Format(" CardId='{0}' And Status=1", CardId));

            }
        }
        /// <summary>
        /// 未使用数量
        /// </summary>
        public int UnUseCount
        {
            get
            {

                return bllCardCoupon.GetCount<MyCardCoupons>(string.Format(" CardId='{0}' And Status=0", CardId));

            }
        }
        /// <summary>
        /// 剩余量
        /// </summary>
        public int UnSendCount
        {
            get
            {
                if (MaxCount - SendCount < 0)
                {
                    return 0;
                }
                return MaxCount - SendCount;

            }
        }




    }
}
