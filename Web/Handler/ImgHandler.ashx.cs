using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ThoughtWorks.QRCode.Codec;

namespace ZentCloud.JubitIMP.Web.Handler
{
    /// <summary>
    /// 二维码生成处理文件
    /// </summary>
    public class ImgHandler : IHttpHandler, IReadOnlySessionState
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/jpeg";
            string code = context.Request["v"];
            string isLogo=context.Request["is_logo"];
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 4;
            qrCodeEncoder.QRCodeVersion =10;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

            String data =code;
            System.Drawing.Bitmap image = qrCodeEncoder.Encode(data);
           
            //image.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);

            string filename = string.Format("{0}.jpg", Guid.NewGuid().ToString());
            string relatePath = string.Format("/FileUpload/QCode/{0}", filename);
            image.Save(context.Server.MapPath(relatePath));
            if (string.IsNullOrEmpty(isLogo))
            {
                relatePath = bll.CompoundImageLogo(relatePath);
            }
           // System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            context.Response.ClearContent();
            context.Response.ContentType = "image/Gif";
            //context.Response.BinaryWrite(memStream.ToArray());
            //根据图片文件的路径使用文件流打开，并保存为byte[]   
            FileStream fs = new FileStream(context.Server.MapPath(relatePath), FileMode.Open);//可以是其他重载方法   
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
            fs.Close();
            context.Response.BinaryWrite(byData);
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