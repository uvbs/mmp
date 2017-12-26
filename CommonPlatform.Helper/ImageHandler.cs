using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace CommonPlatform.Helper
{
    /// <summary>
    /// 图片处理类
    /// </summary>
    public class ImageHandler
    {
        #region 图片旋转函数


        /// <summary>
        /// 以逆时针为方向对图像进行旋转
        /// </summary>
        /// <param name="img">位图流</param>
        /// <param name="angle">旋转角度[0,360](前台给的)</param>
        /// <param name="savePath">保存路径</param>
        /// <returns></returns>
        public Image RotateImg(Image img, int angle, string savePath)
        {
            try
            {

            
            angle = angle % 360;

            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);

            //原图的宽和高
            int w = img.Width;
            int h = img.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));


            //目标位图
            Bitmap dsImage = new Bitmap(W, H);
            Graphics g = Graphics.FromImage(dsImage);
            g.InterpolationMode = InterpolationMode.Bilinear;
            g.SmoothingMode = SmoothingMode.HighQuality;


            //计算偏移量
            Point offset = new Point((W - w) / 2, (H - h) / 2);
            //构造图像显示区域：让图像的中心与窗口的中心点一致
            Rectangle rect = new Rectangle(offset.X, offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);


            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(img, rect);

            //重至绘图的所有变换
            g.ResetTransform();

            g.Save();
            g.Dispose();

            //保存旋转后的图片
            img.Dispose();
            string fileEx = System.IO.Path.GetExtension(savePath);
            switch (fileEx.ToLower())
            {
                case ".jpg":
                    dsImage.Save(savePath, ImageFormat.Jpeg);
                    break;
                case ".png":
                    dsImage.Save(savePath, ImageFormat.Png);
                    break;
                case ".gif":
                    dsImage.Save(savePath, ImageFormat.Gif);
                    break;
                case ".bmp":
                    dsImage.Save(savePath, ImageFormat.Bmp);
                    break;
                case ".ico":
                    dsImage.Save(savePath, ImageFormat.Icon);
                    break;
                default:
                    dsImage.Save(savePath, ImageFormat.Jpeg);
                    break;
            }
            
            dsImage.Dispose();
            return dsImage;
            }
            catch (Exception)
            {
                return img;

            }
        }


        public Image RotateImg(string filename, int angle, string savePath)
        {
            return RotateImg(GetSourceImg(filename), angle, savePath);
        }


        public Image GetSourceImg(string filename)
        {
            Image img;
            img = Bitmap.FromFile(filename);
            return img;
        }

        #endregion 图片旋转函数
        #region 创建文章图片
        public static Image CreateErrorStringImage(string content, int _w, int _h, Color color, float fontSize = 18)
        {
            Image bitmap = new Bitmap(_w, _h);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Brush redBrush = new SolidBrush(color);
                Font font = new Font("宋体", fontSize,FontStyle.Bold);    //定义字体
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center; 
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;//去黑边
                g.DrawString(content, font, redBrush, new Rectangle(0, 0, _w, _h), sf);
            }
            return bitmap;
        }
        #endregion 图片旋转函数
    }
}
