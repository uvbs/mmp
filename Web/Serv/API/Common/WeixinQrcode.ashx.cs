using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// 微信二维码
    /// </summary>
    public class WeixinQrcode : BaseHandlerNoAction
    {
        /// <summary>
        /// 微信
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request["type"];
            string code = context.Request["code"];
            string id = context.Request["id"];
            string qrCodeUrl = "";
            if (string.IsNullOrEmpty(type))
            {

                apiResp.msg = "type 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(id))
            {

                apiResp.msg = "id 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(code))
            {

                apiResp.msg = "code 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }

            switch (type.ToLower())
            {
                case "activitysignin"://活动签到

                    var wxQrcode = bllWeixin.Get<WXQrCode>(string.Format(" WebsiteOwner='{0}'   And Id={1} And QrCodeType='ActivitySignIn'", bllWeixin.WebsiteOwner, id));
                    if (wxQrcode != null)
                    {
                        qrCodeUrl = wxQrcode.QrCodeUrl;
                        //qrCodeUrl = bllWeixin.CompoundImageLogo(qrCodeUrl);
                        qrCodeUrl = bllWeixin.CompoundImageLogoToOss(qrCodeUrl, bllWeixin.WebsiteOwner);
                    }
                    else
                    {

                        qrCodeUrl = bllWeixin.GetWxQrcodeLimit(code);
                        WXQrCode qrCodeModel = new WXQrCode();
                        qrCodeModel.WebsiteOwner = bllWeixin.WebsiteOwner;
                        qrCodeModel.Id = id;
                        qrCodeModel.QrCodeType = "ActivitySignIn";
                        qrCodeModel.QrCodeUrl = qrCodeUrl;
                        bllWeixin.Add(qrCodeModel);


                    }
                    break;
                default:
                    break;
            }


            apiResp.status = true;
            apiResp.msg = "ok";
            if (string.IsNullOrEmpty(qrCodeUrl))
            {
                apiResp.status = false;
                apiResp.msg = "生成二维码失败";
            }
            apiResp.result = new
            {
                qrcode_url = qrCodeUrl
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }


    }
}