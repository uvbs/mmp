using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.HongWareSDK.Entity
{
    /// <summary>
    /// 翼码卡券查询
    /// </summary>
    public class YimaQueryCard
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
        /// 券号为明文16位数字
        //多张券数据以英文逗号分隔
        /// </summary>
        public string valid_info { get; set; }
        /// <summary>
        /// 所购买的商品列表
        //格式：品类编号,单品编号,数量,现单价(分),原单价(分)#品类编号,单品编号,数量,现单价(分),原单价(分)…
        //同一商品不同字段使用英文逗号分隔，不同商品使用井号分隔
        //必填
        /// </summary>
        public string goods_list { get; set; }

    }
}
