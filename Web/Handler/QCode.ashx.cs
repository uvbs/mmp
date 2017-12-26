using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ThoughtWorks.QRCode.Codec;

namespace ZentCloud.JubitIMP.Web.Handler
{
    /// <summary>
    /// QCode 二维码处理文件生成图片 返回图片相对路径
    /// </summary>
    public class QCode : IHttpHandler,IRequiresSessionState
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 4;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            String data = context.Request["code"];
            System.Drawing.Bitmap image = qrCodeEncoder.Encode(data);

            string filename = string.Format("{0}.jpg", Guid.NewGuid().ToString());
            string relatePath = string.Format("/FileUpload/QCode/{0}", filename);
            image.Save(context.Server.MapPath(relatePath));

            relatePath = bll.CompoundImageLogoToOss(relatePath, bll.WebsiteOwner);

            context.Response.Write(relatePath);
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