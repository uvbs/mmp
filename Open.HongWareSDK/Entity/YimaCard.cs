using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.HongWareSDK.Entity
{
    /// <summary>
    /// 翼码制作卡券实体
    /// </summary>
    public class YimaCard
    {
        /// <summary>
        /// 交易流水号 创建活动
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 交易流水号制作卡券
        /// 制卡请求流水字段全平台不可重复，不同商户也不可重复
        ///请务必以4位平台号+12位递增顺序号组成16位流水号
        /// </summary>
        public string transaction_id_makecard { get; set; }
        /// <summary>
        /// 活动全称
        /// </summary>
        public string activity_name { get; set; }
        /// <summary>
        /// 活动简称
        /// </summary>
        public string activity_short_name { get; set; }
        /// <summary>
        /// 活动开始时间
        ///示例：20170101000000
        /// </summary>
        public string begin_time { get; set; }
        /// <summary>
        /// 活动结束时间
        /// 示例：20171231235959
        /// </summary>
        public string end_time { get; set; }
        /// <summary>
        /// 1：全场券
        ///2：品类券
        ///3：单品券
        /// </summary>
        public string card_type { get; set; }
        /// <summary>
        /// 优惠编号列表，以英文逗号分隔
        ///品类券为可优惠品类编号列表
        ///单品券为可优惠单品编号列表
        ///全场券可为空
        /// </summary>
        public string codes { get; set; }
        /// <summary>
        /// 可用门店编号以英文逗号分隔
        ///空则为全门店可用，核销时需传入门店编号
        /// </summary>
        public string store_list { get; set; }
        /// <summary>
        /// 1：满足金额
        ///2：满足数量
        /// </summary>
        public string use_type { get; set; }
        /// <summary>
        /// 可使用优惠需要满足的条件
        ///满足金额为金额数，以分为单位
        ///满足数量为个数
        /// </summary>
        public string use_content { get; set; }
        /// <summary>
        /// 0：非正价也可使用优惠
        ///1：仅正价才可使用优惠
        ///非正价可用时使用现价计算满足金额
        /// </summary>
        public string amt_flag { get; set; }
        /// <summary>
        /// 0：可与其他券同时使用
        ///1：不可与其他券同时使用
        /// </summary>
        public string single_flag { get; set; }
        /// <summary>
        /// 1：线上使用
        ///2：线下使用
        ///3：线上线下均可使用
        /// </summary>
        public string channel_type { get; set; }
        /// <summary>
        /// 优惠卡优惠的金额，以分为单位
        /// </summary>
        public string discount_amt { get; set; }
        /// <summary>
        /// 制卡数量 500以内
        /// </summary>
        public string count { get; set; }

    }
}
