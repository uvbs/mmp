using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.HongWareSDK.Entity
{
    /// <summary>
    /// 翼码领券实体
    /// </summary>
    public class YimaGetCard
    {
        /// <summary>
        /// 同一个商户必须唯一，且大于12位
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 制卡申请时的请求流水号
        /// </summary>
        public string makecard_transid { get; set; }
        /// <summary>
        /// 起始卡券数
        //如从本批次第一张卡券开始获取，则传入“1”
        /// </summary>
        public string start_number { get; set; }
        /// <summary>
        //需要获取的数量
        //单次不超过500条
        /// </summary>
        public string count { get; set; }
        /// <summary>
        /// 活动号，活动创建接口返回的值
        /// </summary>
        public string activity_id { get; set; }

    }
}
