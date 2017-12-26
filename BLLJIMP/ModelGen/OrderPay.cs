using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 订单付款信息表
    /// </summary>
    public partial class OrderPay : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Total_Fee { get; set; }
        /// <summary>
        /// 商品
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 订单状态 0未付款 1已经付款
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 订单类型 1充值积分  2购买VIP  3充值信用金 4充值余额 5注册会员 6支付升级 7短信充值 8 美帆开卡
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 下单用户名 
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 交易流水号 由第三方支付 提供
        /// </summary>
        public string Trade_No { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 扩展字段1 
        /// Type=1时 积分数 
        /// Type=2时 是否带发票
        /// Type=4，5时 提交的Json
        /// Type=8 开卡状态
        /// </summary>
        public string Ex1 { get; set; }
        /// <summary>
        /// 扩展字段2 姓名
        /// Type=8 生效日期
        /// </summary>
        public string Ex2 { get; set; }

        /// <summary>
        /// 扩展字段3 
        /// Type=8 会员卡号
        /// </summary>
        public string Ex3 { get; set; }
        /// <summary>
        /// 到期日期
        /// </summary>
        public string Ex4 { get; set; }
        /// <summary>
        /// 购买渠道 1线下 空线上
        /// </summary>
        public string Ex5 { get; set; }
        /// <summary>
        /// 0微信 1支付宝 2京东支付
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 购买短信总条数
        /// </summary>
        public int BuySmsTotalCount { get; set; }

        /// <summary>
        /// 购买短信份数
        /// </summary>
        public int BuySmsNumber { get; set; }
        /// <summary>
        /// 关联ID
        /// </summary>
        public string RelationId { get; set; }

    }
}
