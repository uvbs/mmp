using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.CardCoupon
{
    /// <summary>
    /// 我的卡券模型
    /// </summary>
    public class MyCardModel
    {
        /// <summary>
        /// 卡券编号
        /// </summary>
        public int cardcoupon_id { get; set; }

        /// <summary>
        /// 主卡编号
        /// </summary>
        public int main_cardcoupon_id { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string img_url { get; set; }
        /// <summary>
        ///卡券名称
        /// </summary>
        public string cardcoupon_name { get; set; }
        /// <summary>
        /// 卡券类型
        /// </summary>
        public int cardcoupon_type { get; set; }
        /// <summary>
        /// 是否是储值卡
        /// </summary>
        public int is_store_card { get; set; }
        /// <summary>
        /// 有效期开始
        /// </summary>
        public string valid_from { get; set; }
        /// <summary>
        ///有效期结束
        /// </summary>
        public string valid_to { get; set; }
        /// <summary>
        /// 状态 0 未使用 1已经使用
        /// </summary>
        public int cardcoupon_status { get; set; }

        /// <summary>
        /// 获取日期
        /// </summary>
        public double cardcoupon_gettime { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public string product_id { get; set; }

        /// <summary>
        /// 有效期开始时间戳
        /// </summary>
        public double valid_from_timestamp { get; set; }
        /// <summary>
        ///有效期结束时间戳
        /// </summary>
        public double valid_to_timestamp { get; set; }
        /// <summary>
        /// 折扣（1-10）(折扣券)cardcoupon_type 等于0时必填
        /// </summary>
        public string discount { get; set; }
        /// <summary>
        /// 抵扣金额(抵扣券)cardcoupon_type 等于1时必填
        /// </summary>
        public string deductible_amount { get; set; }
        /// <summary>
        /// 满多少元包邮(免邮券)cardcoupon_type 等于2时必填
        /// </summary>
        public string freefreight_amount { get; set; }
        /// <summary>
        /// 满多少金额(满扣券)cardcoupon_type 等于3时必填
        /// </summary>
        public string buckle_amount { get; set; }
        /// <summary>
        /// 可减去多少金额（满扣券）cardcoupon_type 等于3时必填
        /// </summary>
        public string buckle_sub_amount { get; set; }
        /// <summary>
        /// 限制类型
        /// 空表示不限制
        /// 0 表示商品ID
        /// 1 表示商品标签
        /// </summary>
        public string limit_type { get; set; }
        /// <summary>
        /// 商品标签
        /// </summary>
        public string product_tags { get; set; }
        /// <summary>
        /// 是否可以赠送给他人
        /// </summary>
        public bool is_can_give { get; set; }
        /// <summary>
        /// 赠送人信息
        /// </summary>
        public UserInfoModel from_user_info { get; set; }
        /// <summary>
        /// 接收人信息
        /// </summary>
        public UserInfoModel to_user_info { get; set; }
        /// <summary>
        /// 核销渠道
        /// </summary>
        public string hexiao_channel { get; set; }

        /// <summary>
        /// 是否可用在普通下单购买
        /// </summary>
        public int is_can_use_shop { get; set; }

        /// <summary>
        /// 算法可用在团购下单购买
        /// </summary>
        public int is_can_use_groupbuy { get; set; }

        /// <summary>
        /// 微信卡券id
        /// </summary>
        public string weixin_card_id { get; set; }

    }
    /// <summary>
    /// 用户信息模型
    /// </summary>
    public class UserInfoModel
    {

        /// <summary>
        /// 头像
        /// </summary>
        public string head_img_url { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nick_name { get; set; }

    }
    /// <summary>
    /// 主卡券模型
    /// </summary>
    public class MainCardModel
    {
        /// <summary>
        /// 卡券编号
        /// </summary>
        public int cardcoupon_id { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string img_url { get; set; }
        /// <summary>
        ///卡券名称
        /// </summary>
        public string cardcoupon_name { get; set; }
        /// <summary>
        /// 有效期开始
        /// </summary>
        public string valid_from { get; set; }
        /// <summary>
        ///有效期结束
        /// </summary>
        public string valid_to { get; set; }
        /// <summary>
        /// 有效期结束
        /// </summary>
        public double valid_to_timestamp { get; set; }
        /// <summary>
        /// 卡券类型
        ///0折扣券：凭折扣券对指定商品（全场）打折
        ///1抵扣券：支付时可以抵扣现金
        ///2免邮券：满一定金额包邮
        ///3满扣券：消费满一定金额减去一定金额
        /// </summary>
        public double cardcoupon_type { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public double product_id { get; set; }
        /// <summary>
        /// 折扣（1-10）(折扣券)cardcoupon_type 等于0时必填
        /// </summary>
        public double discount { get; set; }
        /// <summary>
        /// 抵扣金额(抵扣券)cardcoupon_type 等于1时必填
        /// </summary>
        public double deductible_amount { get; set; }
        /// <summary>
        /// 满多少元包邮(免邮券)cardcoupon_type 等于2时必填
        /// </summary>
        public double freefreight_amount { get; set; }
        /// <summary>
        /// 满多少金额(满扣券)cardcoupon_type 等于3时必填
        /// </summary>
        public double buckle_amount { get; set; }
        /// <summary>
        /// 可减去多少金额（满扣券）cardcoupon_type 等于3时必填
        /// </summary>
        public double buckle_sub_amount { get; set; }
        /// <summary>
        /// 是否已经领取 0未领 1已领
        /// </summary>
        public int is_recivece { get; set; }

        /// <summary>
        /// 发放总量
        /// </summary>
        public int max_count { get; set; }
        /// <summary>
        /// 已经发放数量
        /// </summary>
        public int send_count { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public int un_send_count { get; set; }
        /// <summary>
        /// 限制类型
        /// 空表示不限制
        /// 0 表示商品ID
        /// 1 表示商品标签
        /// </summary>
        public string limit_type { get; set; }
        /// <summary>
        /// 商品标签
        /// </summary>
        public string product_tags { get; set; }
        /// <summary>
        /// 赠送人信息
        /// </summary>
        public UserInfoModel from_user_info { get; set; }
        /// <summary>
        /// 是否已经转赠
        /// </summary>
        public bool is_donation { get; set; }
        /// <summary>
        /// GetLimitType 获取限制类型，1为只能分销会员领取
        /// </summary>
        public string user_get_limit_type { get; set; }

        /// <summary>
        /// 是否可用在普通下单购买
        /// </summary>
        public int is_can_use_shop { get; set; }

        /// <summary>
        /// 算法可用在团购下单购买
        /// </summary>
        public int is_can_use_groupbuy { get; set; }
        /// <summary>
        /// 过期类型
        /// </summary>
        public string expire_time_type { get; set; }
        /// <summary>
        /// 领取后几天过期
        /// </summary>
        public string expire_day { get; set; }
        /// <summary>
        /// 微信卡券id
        /// </summary>
        public string weixin_card_id { get; set; }

    }

    

}
