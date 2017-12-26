using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// 微信二维码登录-生成登录二维码图片
    /// </summary>
    public class QRCodeLogin : IHttpHandler
    {
        DefaultResponse resp = new DefaultResponse();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 4;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

            try
            {

            
            String data = context.Request["code"];
            System.Drawing.Bitmap image = qrCodeEncoder.Encode(data);
            string filename = string.Format("{0}.jpg", Guid.NewGuid().ToString());
            string qrCodeImgUrl = string.Format("http://{0}/FileUpload/QCode/{0}", context.Request.Url.Host,filename);
            image.Save(context.Server.MapPath(qrCodeImgUrl));



            resp.imgurl = qrCodeImgUrl;
            }
            catch (Exception ex)
            {
                resp.errcode = -1;
                resp.errmsg = ex.Message;
            }
            context.Response.Write(Common.JSONHelper.ObjectToJson(resp));

        }

        public class DefaultResponse
        {

            /// <summary>
            /// 错误码
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }
            /// <summary>
            /// 登录凭据
            /// </summary>
            public string access_token { get; set; }
            /// <summary>
            /// 二维码登录图片地址
            /// </summary>
            public string imgurl { get; set; }

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