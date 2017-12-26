using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.PayConfig
{
    /// <summary>
    /// 获取支付配置接口
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 支付BLL
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        public void ProcessRequest(HttpContext context)
        {

            ZentCloud.BLLJIMP.Model.PayConfig payconfig = bllPay.GetPayConfig();

            if (payconfig == null)
            {
                resp.errcode = 1;
                resp.errmsg = "你还没有配置支付,请先检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            RequestModel requestModel = new RequestModel();
            requestModel.payconfig_wxappid = payconfig.WXAppId;
            requestModel.payconfig_wxmch_id = payconfig.WXMCH_ID;
            requestModel.payconfig_wxpartnerkey = payconfig.WXPartnerKey;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(requestModel));
        }
        /// <summary>
        /// 返回实体
        /// </summary>
        public class RequestModel
        {
            ///// <summary>
            ///// 支付宝商户号
            ///// </summary>
            //public string payconfig_partner { get; set; }

            ///// <summary>
            ///// 支付宝密钥
            ///// </summary>
            //public string payconfig_partnerKey { get; set; }
            ///// <summary>
            ///// 支付宝卖家支付宝账号
            ///// </summary>
            //public string payconfig_seller_account_name { get; set; }
            ///// <summary>
            ///// 支付宝私钥
            ///// </summary>
            //public string payconfig_private_key { get; set; }
            ///// <summary>
            ///// 支付宝公钥
            ///// </summary>
            //public string payconfig_public_key { get; set; }
            ///// <summary>
            ///// 编码
            ///// </summary>
            //public string payconfig_input_charset { get; set; }
            ///// <summary>
            ///// 支付宝 签名类型 MD5或 0001 RSA
            ///// </summary>
            //public string payconfig_sign_type { get; set; }


            /// <summary>
            /// 微信AppId
            /// </summary>
            public string payconfig_wxappid { get; set; }
            /// <summary>
            /// 微信MCH_ID
            /// </summary>
            public string payconfig_wxmch_id { get; set; }
            
            /// <summary>
            /// 微信 Key
            /// </summary>
            public string payconfig_wxpartnerkey { get; set; }
        }


    }
}