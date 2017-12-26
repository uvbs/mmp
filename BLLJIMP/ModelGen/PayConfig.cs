using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 支付配置表 全局的
    /// </summary>
    public class PayConfig : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 支付宝商户号
        /// </summary>
        public string Partner { get; set; }
        /// <summary>
        /// 支付宝MD5密钥
        /// </summary>
        public string PartnerKey { get; set; }
        /// <summary>
        /// 支付宝卖家支付宝账号 / 别名
        /// </summary>
        public string Seller_Account_Name { get; set; }
        /// <summary>
        /// 支付宝私钥
        /// </summary>
        public string Private_key { get; set; }
        /// <summary>
        /// 支付宝公钥
        /// </summary>
        public string Public_key { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Input_charset { get; set; }
        /// <summary>
        /// 支付宝 签名类型 MD5或 0001 RSA
        /// </summary>
        public string Sign_type { get; set; }
        /// <summary>
        /// 微信AppId
        /// </summary>
        public string WXAppId { get; set; }
        /// <summary>
        /// 微信商户号
        /// </summary>
        public string WXMCH_ID { get; set; }
        /// <summary>
        /// 微信 支付Api密钥
        /// </summary>
        public string WXPartnerKey { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 京东支付商户号
        /// </summary>
        public string JDPayMerchant { get; set; }
        
        public string JDPayRSAPrivateKey { get; set; }

        public string JDPayRSAPublicKey { get; set; }

        public string JDPayDESKey { get; set; }



    }
}
