using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 支付方式类型表
    /// </summary>
    public class WXMallPaymentType : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 支付方式编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 排序 从小到大排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        ///支付类型 0代表线下支付 1代表支付宝 2代表微信支付
        /// </summary>
        public int PaymentType { get; set; }
        /// <summary>
        ///支付方式名称
        /// </summary>
        public string PaymentTypeName { get; set; }

       /// <summary>
       /// 支付宝商户号
       /// </summary>
        public string AlipayPartner { get; set; }

       /// <summary>
       /// 支付宝密钥
       /// </summary>
        public string AlipayPartnerKey { get; set; }
       /// <summary>
       /// 支付宝账号
       /// </summary>
        public string AlipaySeller_Account_Name { get; set; }
        /// <summary>
        /// 微信AppId
        /// </summary>
        public string WXAppId { get; set; }
        /// <summary>
        /// 微信AppKey
        /// </summary>
        public string WXAppKey { get; set; }
        /// <summary>
        /// 微信Partner
        /// </summary>
        public string WXPartner { get; set; }
        /// <summary>
        /// 微信PartnerKey
        /// </summary>
        public string WXPartnerKey { get; set; }


        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 是否停用 0启用 1信用
        /// </summary>
        public int IsDisable { get; set; }
    }
}
