using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 图片操作类
    /// </summary>
    public class BLLImage
    {

        /// <summary>
        ///图片压缩
        /// </summary>
        /// <param name="filePath">原文件路径 类似 /fileupload/img.jpg</param>
        /// <param name="intThumbWidth">压缩后的宽度</param>
        /// <param name="intThumbHeight">压缩后的高度</param>
        /// <returns></returns>
        public string CreateThumbImage(string filePath, int intThumbWidth, int intThumbHeight)
        {
            string newfilepath = "";
            try
            {
                //原图加载    
                using (System.Drawing.Image sourceImage = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath(filePath)))
                {
                    //原图宽度和高度    
                    int width = sourceImage.Width;
                    int height = sourceImage.Height;
                    int smallWidth;
                    int smallHeight;
                    if (width<=intThumbWidth)
                    {
                        return filePath;
                    }
                    ////获取第一张绘制图的大小,(比较 原图的宽/缩略图的宽 和 原图的高/缩略图的高)    
                    //if (((decimal)width) / height <= ((decimal)intThumbWidth) / intThumbHeight)
                    //{
                    //    smallWidth = intThumbWidth;
                    //    smallHeight = intThumbWidth * height / width;
                    //}
                    //else
                    //{
                    //    smallWidth = intThumbHeight * width / height;
                    //    smallHeight = intThumbHeight;
                    //}
                    double scale = ((double)width) / ((double)height);
                    smallWidth = intThumbWidth;
                    smallHeight = (int)(intThumbWidth/scale);

                   string dirPath=System.IO.Path.GetDirectoryName(filePath);//保存目录

                   string smmallfilename = System.IO.Path.GetFileNameWithoutExtension(filePath) + "_thumb" + System.IO.Path.GetExtension(filePath);//新文件名

                   newfilepath = dirPath+"\\" + smmallfilename;
                    //缩略图保存的绝对路径    
                   string smallImagePath = System.Web.HttpContext.Current.Server.MapPath(newfilepath);
                    //新建一个图板,以最小等比例压缩大小绘制原图    
                    using (System.Drawing.Image bitmap = new System.Drawing.Bitmap(smallWidth, smallHeight))
                    {
                        //绘制中间图    
                        using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                        {
                            //高清,平滑    
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            g.Clear(Color.Black);
                            g.DrawImage(sourceImage,
                            new System.Drawing.Rectangle(0, 0, smallWidth, smallHeight),
                            new System.Drawing.Rectangle(0, 0, width, height),
                            System.Drawing.GraphicsUnit.Pixel
                            );
                        }
                        //新建一个图板,以缩略图大小绘制中间图    
                        using (System.Drawing.Image bitmap1 = new System.Drawing.Bitmap(intThumbWidth, intThumbHeight))
                        {
                            //绘制缩略图    
                            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap1))
                            {
                                //高清,平滑    
                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                g.Clear(Color.Black);
                                int lwidth = (smallWidth - intThumbWidth) / 2;
                                int bheight = (smallHeight - intThumbHeight) / 2;
                                g.DrawImage(bitmap, new Rectangle(0, 0, intThumbWidth, intThumbHeight), lwidth, bheight, intThumbWidth, intThumbHeight, GraphicsUnit.Pixel);
                                g.Dispose();
                                bitmap.Save(smallImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                               
                            }
                        }
                    }
                }
            }
            catch
            {
               return filePath;
            }
            //返回缩略图路径    
            return newfilepath.Replace("\\","/");
        
        }

        /// <summary>
        /// BASE64字符串转成图片
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public Image ConverToImage(string base64Str)
        {

            byte[] imageBytes = Convert.FromBase64String(base64Str);
            //读入MemoryStream对象
            MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
            memoryStream.Write(imageBytes, 0, imageBytes.Length);
            //转成图片
           return Image.FromStream(memoryStream);
        

        
        }

        /// <summary>
        /// 获取图片宽高
        /// </summary>
        /// <param name="filePath">文件物理路径 示例 D:\img.jpg</param>
        /// <param name="width">图片宽</param>
        /// <param name="height">图片高</param>
        public void GetImageWidthHeight( string filePath,out int width,out int height){

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(fs);
                width = image.Width;
                height = image.Height;
            }
        
        }

  

    }
}
