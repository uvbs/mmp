using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// PayMethod 的摘要说明
    /// </summary>
    public class PayMethod : BaseHandlerNoAction
    {
        BllPay bllPay = new BllPay();
        public void ProcessRequest(HttpContext context)
        {
            PayConfig payConfig = bllPay.GetPayConfig();
            apiResp.result = new
            {
                is_wx_pay = bllPay.IsWeixinPay(payConfig),
                is_ali_pay = bllPay.IsAliPay(payConfig),
                is_jd_pay = bllPay.IsJDPay(payConfig)
            };
            apiResp.status = true;
            bllPay.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}