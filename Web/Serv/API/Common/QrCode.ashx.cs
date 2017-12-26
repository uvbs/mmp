using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ThoughtWorks.QRCode.Codec;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// 二维码生成接口
    /// </summary>
    public class QrCode:BaseHandlerNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 4;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            String code = context.Request["code"];
            if (string.IsNullOrEmpty(code))
            {
                apiResp.msg = "code 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            System.Drawing.Bitmap image = qrCodeEncoder.Encode(code);
            string fileName = string.Format("{0}.jpg", Guid.NewGuid().ToString());
            string relatePath = string.Format("/FileUpload/QCode/{0}", fileName);
            image.Save(context.Server.MapPath(relatePath));

            if (bll.WebsiteOwner!="songhe")
            {
                relatePath = bll.CompoundImageLogoToOss(relatePath, bll.WebsiteOwner);

            }
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                qrcode_url = relatePath
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }
    }
}