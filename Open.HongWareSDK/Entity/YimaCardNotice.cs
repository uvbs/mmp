using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.HongWareSDK.Entity
{
    /// <summary>
    /// 翼码卡券通知实体
    /// </summary>
   public class YimaCardNotice
    {
        /// <summary>
        /// 终端号（海澜定制可不填写）
        /// </summary>
        public string pos_id { get; set; }
        /// <summary>
        /// 门店号
        //需填写门店号才可以校验优惠卡券的门店使用规则
        /// </summary>
        public string store_id { get; set; }
        /// <summary>
        /// 同一个商户必须唯一，且大于12位
        /// </summary>
        public string pos_seq { get; set; }
        /// <summary>
//        /// 0：失败
//1：成功
//       通知订单失败后翼码将原验证卡券进行撤销，订单成功则向业务平台推送核销成功通知

        /// </summary>
        public string order_result { get; set; }
        /// <summary>
       ///所通知的验证交易终端流水号
        /// </summary>
        public string org_pos_seq { get; set; }

    }
}
