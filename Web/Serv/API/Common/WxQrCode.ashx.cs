using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// 生成微信二维码
    /// </summary>
    public class WxQrCode : BaseHandlerNoAction
    {
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        public void ProcessRequest(HttpContext context)
        {
            string code=context.Request["code"];
            if (string.IsNullOrEmpty(code))
            {
                apiResp.msg = "code 参数必传";
                 bllWeixin.ContextResponse(context, apiResp);
                 return;
            }
            var qrCodeUrl = bllWeixin.GetWxQrcodeLimit(code);
            if (!string.IsNullOrEmpty(qrCodeUrl))
            {
                apiResp.status = true;
                apiResp.result = new
                {
                    qrcode_url=qrCodeUrl
                };
            }
            else
            {
                apiResp.status = false;
                apiResp.msg = "生成二维码失败";
            }
            bllWeixin.ContextResponse(context, apiResp);

           
        }

        
    }
}