using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace ZentCloud.BLLModule
{
    public class ImageHandler : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string filePath = context.Request.FilePath.ToLower();
            string widthRequest = context.Request["w"];
            string heightRequest = context.Request["h"];
            string mapPath = context.Server.MapPath(filePath);
            if (!File.Exists(mapPath))
            {
                context.Response.End();
                //context.Response.WriteFile(context.Server.MapPath("/img/nopms.png"));
                return;
            }
            string extension = Path.GetExtension(filePath);
            string contentType = FileContentTypeHelper.GetMimeType(extension);
            context.Response.ContentType = contentType;
            if (string.IsNullOrWhiteSpace(widthRequest) && string.IsNullOrWhiteSpace(heightRequest))
            {
                context.Response.ClearContent();
                context.Response.WriteFile(filePath);
                context.Response.End();
                return;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap pic = new Bitmap(mapPath))
                {
                    //原图宽度和高度    
                    int width = pic.Width;
                    int height = pic.Height;
                    int smallWidth;
                    int smallHeight;

                    if (string.IsNullOrWhiteSpace(heightRequest))
                    {
                        smallWidth = Convert.ToInt32(widthRequest);
                        smallHeight = height * smallWidth / width;
                        if (smallWidth >= width) {
                            smallWidth = width;
                            smallHeight = height;
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(widthRequest))
                    {
                        smallHeight = Convert.ToInt32(heightRequest);
                        smallWidth = width * smallHeight / height;
                        if (smallHeight >= height)
                        {
                            smallWidth = width;
                            smallHeight = height;
                        }
                    }
                    else{
                        if (Convert.ToInt32(widthRequest) / Convert.ToInt32(heightRequest) > width / height)
                        {
                            smallHeight = Convert.ToInt32(heightRequest);
                            smallWidth = width * smallHeight / height;
                            if (smallHeight >= height)
                            {
                                smallWidth = width;
                                smallHeight = height;
                            }
                        }
                        else
                        {
                            smallWidth = Convert.ToInt32(widthRequest);
                            smallHeight = height * smallWidth / width;
                            if (smallWidth >= width)
                            {
                                smallWidth = width;
                                smallHeight = height;
                            }
                        }
                    }
                    
                    Image newPic = pic.GetThumbnailImage(smallWidth, smallHeight, null, IntPtr.Zero);
                    if (contentType.Contains("jpeg"))
                    {
                        newPic.Save(ms, ImageFormat.Jpeg);
                    }
                    else if (contentType.Contains("png"))
                    {
                        newPic.Save(ms, ImageFormat.Png);
                    }
                    else if (contentType.Contains("bmp"))
                    {
                        newPic.Save(ms, ImageFormat.Bmp);
                    }
                    else if (contentType.Contains("gif"))
                    {
                        newPic.Save(ms, ImageFormat.Gif);
                    }
                    else if (contentType.Contains("icon"))
                    {
                        newPic.Save(ms, ImageFormat.Icon);
                    }
                    else{
                        newPic.Save(ms, ImageFormat.Jpeg);
                    }
                }
                byte[] buffer = new byte[ms.Length];
                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);

                context.Response.ClearContent();
                context.Response.BinaryWrite(buffer);
                context.Response.End();
                return;
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }

    }
}
