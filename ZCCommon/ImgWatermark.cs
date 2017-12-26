using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ZentCloud.Common
{

    /*
      调用 im.SaveWatermark(原图地址, 水印地址, 透明度, 水印位置, 边距,保存位置); 
      Uinatlex.ToolBox.ImageManager im = new Uinatlex.ToolBox.ImageManager();
      im.SaveWatermark(Server.MapPath("/原图.jpg"), Server.MapPath("/水印.jpg"), 0.5f, Uinatlex.ToolBox.ImageManager.WatermarkPosition.RigthBottom, 10, Server.MapPath("/原图.jpg"));
     */

    /// <summary>
    /// 图片水印处理
    /// </summary>
    public class ImgWatermarkHelper
    {
        #region 变量声明开始
        /// <summary>
        /// 枚举: 水印位置
        /// </summary>
        public enum WatermarkPosition
        {
            /// <summary>
            /// 左上
            /// </summary>
            LeftTop,
            /// <summary>
            /// 左中
            /// </summary>
            Left,
            /// <summary>
            /// 左下
            /// </summary>
            LeftBottom,
            /// <summary>
            /// 正上
            /// </summary>
            Top,
            /// <summary>
            /// 正中
            /// </summary>
            Center,
            /// <summary>
            /// 正下
            /// </summary>
            Bottom,
            /// <summary>
            /// 右上
            /// </summary>
            RightTop,
            /// <summary>
            /// 右中
            /// </summary>
            RightCenter,
            /// <summary>
            /// 右下
            /// </summary>
            RigthBottom
        }
        #endregion 变量声明结束
        #region 构造函数开始
        /// <summary>
        /// 构造函数: 默认
        /// </summary>
        public ImgWatermarkHelper()
        { }
        #endregion 构造函数结束
        #region 私有函数开始
        /// <summary>
        /// 获取: 图片去扩展名(包含完整路径及其文件名)小写字符串
        /// </summary>
        /// <param name="path">图片路径(包含完整路径,文件名及其扩展名): string</param>
        /// <returns>返回: 图片去扩展名(包含完整路径及其文件名)小写字符串: string</returns>
        private string GetFileName(string path)
        {
            return path.Remove(path.LastIndexOf('.')).ToLower();
        }
        /// <summary>
        /// 获取: 图片以'.'开头的小写字符串扩展名
        /// </summary>
        /// <param name="path">图片路径(包含完整路径,文件名及其扩展名): string</param>
        /// <returns>返回: 图片以'.'开头的小写字符串扩展名: string</returns>
        private string GetExtension(string path)
        {
            return path.Remove(0, path.LastIndexOf('.')).ToLower();
        }
        /// <summary>
        /// 获取: 图片以 '.' 开头的小写字符串扩展名对应的 System.Drawing.Imaging.ImageFormat 对象
        /// </summary>
        /// <param name="format">以 '. '开头的小写字符串扩展名: string</param>
        /// <returns>返回: 图片以 '.' 开头的小写字符串扩展名对应的 System.Drawing.Imaging.ImageFormat 对象: System.Drawing.Imaging.ImageFormat</returns>
        public ImageFormat GetImageFormat(string format)
        {
            switch (format)
            {
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".emf":
                    return ImageFormat.Emf;
                case ".exif":
                    return ImageFormat.Exif;
                case ".gif":
                    return ImageFormat.Gif;
                case ".ico":
                    return ImageFormat.Icon;
                case ".png":
                    return ImageFormat.Png;
                case ".tif":
                    return ImageFormat.Tiff;
                case ".tiff":
                    return ImageFormat.Tiff;
                case ".wmf":
                    return ImageFormat.Wmf;
                default:
                    return ImageFormat.Jpeg;
            }
        }
        /// <summary>
        /// 获取: 枚举 Uinatlex.ToolBox.ImageManager.WatermarkPosition 对应的 System.Drawing.Rectangle 对象
        /// </summary>
        /// <param name="positon">枚举 Uinatlex.ToolBox.ImageManager.WatermarkPosition: Uinatlex.ToolBox.ImageManager.WatermarkPosition</param>
        /// <param name="X">原图宽度: int</param>
        /// <param name="Y">原图高度: int</param>
        /// <param name="x">水印宽度: int</param>
        /// <param name="y">水印高度: int</param>
        /// <param name="i">边距: int</param>
        /// <returns>返回: 枚举 Uinatlex.ToolBox.ImageManager.WatermarkPosition 对应的 System.Drawing.Rectangle 对象: System.Drawing.Rectangle</returns>
        private Rectangle GetWatermarkRectangle(WatermarkPosition positon, int X, int Y, int x, int y, int i)
        {
            switch (positon)
            {
                case WatermarkPosition.LeftTop:
                    return new Rectangle(i, i, x, y);
                case WatermarkPosition.Left:
                    return new Rectangle(i, (Y - y) / 2, x, y);
                case WatermarkPosition.LeftBottom:
                    return new Rectangle(i, Y - y - i, x, y);
                case WatermarkPosition.Top:
                    return new Rectangle((X - x) / 2, i, x, y);
                case WatermarkPosition.Center:
                    return new Rectangle((X - x) / 2, (Y - y) / 2, x, y);
                case WatermarkPosition.Bottom:
                    return new Rectangle((X - x) / 2, Y - y - i, x, y);
                case WatermarkPosition.RightTop:
                    return new Rectangle(X - x - i, i, x, y);
                case WatermarkPosition.RightCenter:
                    return new Rectangle(X - x - i, (Y - y) / 2, x, y);
                default:
                    return new Rectangle(X - x - i, Y - y - i, x, y);
            }
        }
        #endregion 私有函数结束

        /// <summary>
        /// 会产生graphics异常的PixelFormat
        /// </summary>
        private static PixelFormat[] indexedPixelFormats = { PixelFormat.Undefined, PixelFormat.DontCare,
            PixelFormat.Format16bppArgb1555, PixelFormat.Format1bppIndexed, PixelFormat.Format4bppIndexed,
            PixelFormat.Format8bppIndexed
        };

        /// <summary>
        /// 判断图片的PixelFormat 是否在 引发异常的 PixelFormat 之中
        /// </summary>
        /// <param name="imgPixelFormat">原图片的PixelFormat</param>
        /// <returns></returns>
        private static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
        {
            foreach (PixelFormat pf in indexedPixelFormats)
            {
                if (pf.Equals(imgPixelFormat)) return true;
            }

            return false;
        }
        #region 文字生成开始
        #endregion 文字生成结束
        #region 设置透明度开始
        /// <summary>
        /// 设置: 图片 System.Drawing.Bitmap 对象透明度
        /// </summary>
        /// <param name="sBitmap">图片 System.Drawing.Bitmap 对象: System.Drawing.Bitmap</param>
        /// <param name="transparence">水印透明度(值越高透明度越低,范围在0.0f~1.0f之间): float</param>
        /// <returns>图片 System.Drawing.Bitmap: System.Drawing.Bitmap</returns>
        public Bitmap SetTransparence(Bitmap bm, float transparence)
        {
            if (transparence == 0.0f || transparence == 1.0f)
                throw new ArgumentException("透明度值只能在0.0f~1.0f之间");
            float[][] floatArray = 
            {
                new float[] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f }, 
                new float[] { 0.0f, 1.0f, 0.0f, 0.0f, 0.0f }, 
                new float[] { 0.0f, 0.0f, 1.0f, 0.0f, 0.0f }, 
                new float[] { 0.0f, 0.0f, 0.0f, transparence, 0.0f },
                new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 1.0f } 
            };
            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(new ColorMatrix(floatArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            Bitmap bitmap = new Bitmap(bm.Width, bm.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawImage(bm, new Rectangle(0, 0, bm.Width, bm.Height), 0, 0, bm.Width, bm.Height, GraphicsUnit.Pixel, imageAttributes);
            graphics.Dispose();
            imageAttributes.Dispose();
            bm.Dispose();
            return bitmap;
        }
        /// <summary>
        ///  设置: 图片 System.Drawing.Bitmap 对象透明度
        /// </summary>
        /// <param name="readpath">图片路径(包含完整路径,文件名及其扩展名): string</param>
        /// <param name="transparence">水印透明度(值越高透明度越低,范围在0.0f~1.0f之间): float</param>
        /// <returns>图片 System.Drawing.Bitmap: System.Drawing.Bitmap</returns>
        public Bitmap SetTransparence(string readpath, float transparence)
        {
            return SetTransparence(new Bitmap(readpath), transparence);
        }
        #endregion 设置透明度结束
        #region 添加水印开始
        /// <summary>
        /// 生成: 原图绘制水印的 System.Drawing.Bitmap 对象
        /// </summary>
        /// <param name="sBitmap">原图 System.Drawing.Bitmap 对象: System.Drawing.Bitmap</param>
        /// <param name="wBitmap">水印 System.Drawing.Bitmap 对象: System.Drawing.Bitmap</param>
        /// <param name="position">枚举 Uinatlex.ToolBox.ImageManager.WatermarkPosition : Uinatlex.ToolBox.ImageManager.WatermarkPosition</param>
        /// <param name="margin">水印边距: int</param>
        /// <returns>返回: 原图绘制水印的 System.Drawing.Bitmap 对象 System.Drawing.Bitmap</returns>
        public Bitmap CreateWatermark(Bitmap sBitmap, Bitmap wBitmap, WatermarkPosition position, int margin)
        {
            Graphics graphics = Graphics.FromImage(sBitmap);
            graphics.DrawImage(wBitmap, GetWatermarkRectangle(position, sBitmap.Width, sBitmap.Height, wBitmap.Width, wBitmap.Height, margin));
            graphics.Dispose();
            wBitmap.Dispose();
            return sBitmap;
        }
        #endregion 添加水印结束
        #region 图片切割开始
        #endregion 图片切割结束
        #region 图片缩放开始
        #endregion 图片缩放结束
        #region 保存图片到文件开始
        #region 普通保存开始
        /// <summary>
        /// 保存: System.Drawing.Bitmap 对象到图片文件
        /// </summary>
        /// <param name="bitmap">System.Drawing.Bitmap 对象: System.Drawing.Bitmap</param>
        /// <param name="writepath">保存路径(包含完整路径,文件名及其扩展名): string</param>
        public void Save(Bitmap bitmap, string writepath, long imgByteValue = 100L)
        {
            try
            {

                //bitmap.Save(writepath, GetImageFormat(GetExtension(writepath)));
                //bitmap.Dispose();
                //return;

                EncoderParameter p;
                EncoderParameters ps;

                ps = new EncoderParameters(1);

                p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, imgByteValue);

                ps.Param[0] = p;

                ImageCodecInfo ii = GetCodecInfo("image/jpeg");

                bitmap.Save(writepath, ii, ps);
                bitmap.Dispose();
            }
            catch
            {
                throw new ArgumentException("图片保存错误");
            }
        }

        private ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        /// <summary>
        /// 保存: 对象到图片文件
        /// </summary>
        /// <param name="readpath">原图路径(包含完整路径,文件名及其扩展名): string</param>
        /// <param name="writepath">保存路径(包含完整路径,文件名及其扩展名): string</param>
        public void Save(string readpath, string writepath)
        {
            if (string.Compare(readpath, writepath) == 0)
                throw new ArgumentException("源图片与目标图片地址相同");
            try
            {
                Save(new Bitmap(readpath), writepath);
            }
            catch
            {
                throw new ArgumentException("图片读取错误");
            }
        }
        #endregion 普通保存结束
        #region 文字绘图保存开始
        #endregion 文字绘图保存结束
        #region 透明度调整保存开始
        /// <summary>
        /// 保存: 设置透明度的对象到图片文件
        /// </summary>
        /// <param name="sBitmap">图片 System.Drawing.Bitmap 对象: System.Drawing.Bitmap</param>
        /// <param name="transparence">水印透明度(值越高透明度越低,范围在0.0f~1.0f之间): float</param>
        /// <param name="writepath">保存路径(包含完整路径,文件名及其扩展名): string</param>
        public void SaveTransparence(Bitmap bitmap, float transparence, string writepath)
        {
            Save(SetTransparence(bitmap, transparence), writepath);
        }
        /// <summary>
        /// 保存: 设置透明度的象到图片文件
        /// </summary>
        /// <param name="readpath">原图路径(包含完整路径,文件名及其扩展名): string</param>
        /// <param name="transparence">水印透明度(值越高透明度越低,范围在0.0f~1.0f之间): float</param>
        /// <param name="writepath">保存路径(包含完整路径,文件名及其扩展名): string</param>
        public void SaveTransparence(string readpath, float transparence, string writepath)
        {
            Save(SetTransparence(readpath, transparence), writepath);
        }
        #endregion 透明度调整保存结束
        #region 水印图片保存开始
        /// <summary>
        /// 保存: 绘制水印的对象到图片文件
        /// </summary>
        /// <param name="sBitmap">原图 System.Drawing.Bitmap 对象: System.Drawing.Bitmap</param>
        /// <param name="wBitmap">水印 System.Drawing.Bitmap 对象: System.Drawing.Bitmap</param>
        /// <param name="position">枚举 Uinatlex.ToolBox.ImageManager.WatermarkPosition : Uinatlex.ToolBox.ImageManager.WatermarkPosition</param>
        /// <param name="margin">水印边距: int</param>
        /// <param name="writepath">保存路径(包含完整路径,文件名及其扩展名): string</param>
        public void SaveWatermark(Bitmap sBitmap, Bitmap wBitmap, WatermarkPosition position, int margin, string writepath, long imgByteValue = 100L)
        {
            Save(CreateWatermark(sBitmap, wBitmap, position, margin), writepath, imgByteValue);
        }
        /// <summary>
        /// 保存: 绘制水印的对象到图片文件
        /// </summary>
        /// <param name="readpath">图片路径(包含完整路径,文件名及其扩展名): string</param>
        /// <param name="watermarkpath">水印图片路径(包含完整路径,文件名及其扩展名): string</param>
        /// <param name="transparence">水印透明度(值越高透明度越低,范围在0.0f~1.0f之间): float</param>
        /// <param name="position">枚举 Uinatlex.ToolBox.ImageManager.WatermarkPosition : Uinatlex.ToolBox.ImageManager.WatermarkPosition</param>
        /// <param name="margin">水印边距: int</param>
        /// <param name="writepath">保存路径(包含完整路径,文件名及其扩展名): string</param>
        public void SaveWatermark(string readpath, string watermarkpath, float transparence, WatermarkPosition position, int margin, string writepath, float watermarkSizeScale = 0, long imgByteValue = 100L)
        {
            if (string.Compare(readpath, writepath) == 0)
                throw new ArgumentException("源图片与目标图片地址相同");

            using (Image img = Image.FromFile(readpath))
            {
                if (IsPixelFormatIndexed(img.PixelFormat))
                {
                    Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        g.DrawImage(img, 0, 0);
                    }
                    //下面的水印操作，就直接对 bmp 进行了
                    if (transparence == 0.0f)
                        Save(readpath, writepath);
                    else if (transparence == 1.0f)
                    {
                        Bitmap imgWatermark = new Bitmap(watermarkpath);
                        if (watermarkSizeScale != 0)
                        {
                            int newWidth = (int)((float)bmp.Width * watermarkSizeScale);
                            int newHeigth = (int)((float)bmp.Width * watermarkSizeScale);
                            Image imgTmp = byteArrayToImage(BitmapToBytes(imgWatermark));
                            imgWatermark = BytesToBitmap(ImageToBytes(PhotoSizeChange(imgTmp, newWidth, newHeigth), ImageFormat.Png));
                        }

                        SaveWatermark(bmp, imgWatermark, position, margin, writepath, imgByteValue);
                    }
                    else
                        SaveWatermark(new Bitmap(readpath), SetTransparence(watermarkpath, transparence), position, margin, writepath);
                }
                else //否则直接操作
                {
                    //直接对img进行水印操作
                    if (transparence == 0.0f)
                        Save(readpath, writepath);
                    else if (transparence == 1.0f)
                    {
                        Bitmap imgRead = new Bitmap(readpath);
                        Bitmap imgWatermark = new Bitmap(watermarkpath);

                        if (watermarkSizeScale != 0)
                        {
                            int newWidth = (int)((float)imgRead.Width * watermarkSizeScale);
                            int newHeigth = (int)((float)imgRead.Width * watermarkSizeScale);
                            Image imgTmp = byteArrayToImage(BitmapToBytes(imgWatermark));
                            imgWatermark = BytesToBitmap(ImageToBytes(PhotoSizeChange(imgTmp, newWidth, newHeigth), ImageFormat.Png));
                        }

                        SaveWatermark(imgRead, imgWatermark, position, margin, writepath, imgByteValue);
                    }
                    else
                        SaveWatermark(new Bitmap(readpath), SetTransparence(watermarkpath, transparence), position, margin, writepath);
                }
            }

            
        }
        #endregion 水印图片保存结束
        #region 图片切割保存开始
        #endregion 图片切割保存结束
        #region 图片缩放保存开始
        #endregion 图片缩放保存开始
        #endregion 保存图片到文件结束



        public void ImgAddBord(string path, string savePath)
        {
            Image img = Bitmap.FromFile(path);
            int bordwidth = Convert.ToInt32(img.Width * 0.1);
            int bordheight = Convert.ToInt32(img.Height * 0.1);

            int newheight = img.Height + bordheight;
            int newwidth = img.Width + bordwidth;

            Color bordcolor = Color.White;
            Bitmap bmp = new Bitmap(newwidth, newheight);
            Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            int Style = 0;     //New: 绘制边框的类型, 手动修改0,1,2 可改变边框类型
            if (Style == 0)   //New: 整个边框.
            {
                //Changed: 修改rec区域, 将原图缩放. 适合边框内
                System.Drawing.Rectangle rec = new Rectangle(bordwidth / 2, bordwidth / 2, newwidth - bordwidth / 2, newheight - bordwidth / 2);
                g.DrawImage(img, rec, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.DrawRectangle(new Pen(bordcolor, bordheight), 0, 0, newwidth, newheight);
            }
            else if (Style == 1)   //New: 上下边框.
            {
                System.Drawing.Rectangle rec = new Rectangle(0, bordwidth / 2, newwidth, newheight - bordwidth / 2);
                g.DrawImage(img, rec, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.DrawLine(new Pen(bordcolor, bordheight), 0, 0, newwidth, 0);
                g.DrawLine(new Pen(bordcolor, bordheight), 0, newheight, newwidth, newheight);
            }
            else if (Style == 2)   //New: 左右边框.
            {
                System.Drawing.Rectangle rec = new Rectangle(bordwidth / 2, 0, newwidth - bordwidth / 2, newheight);
                g.DrawImage(img, rec, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.DrawLine(new Pen(bordcolor, bordheight), 0, 0, 0, newheight);
                g.DrawLine(new Pen(bordcolor, bordheight), newwidth, 0, newwidth, newheight);
            }

            bmp.Save(savePath);
            g.Dispose();



        }


        public double GetRatio(int width, int height, int maxWidth = 0, int maxHeight = 0)
        {
            double hRatio;
            double wRatio;
            double Ratio = 1;
            var w = width;
            var h = height;

            wRatio = Convert.ToDouble(maxWidth) / w;
            hRatio = Convert.ToDouble(maxHeight) / h;
            if (maxWidth == 0 && maxHeight == 0)
            {
                Ratio = 1;
            }
            else if (maxWidth == 0)
            {//
                if (hRatio < 1) Ratio = hRatio;
            }
            else if (maxHeight == 0)
            {
                if (wRatio < 1) Ratio = wRatio;
            }
            else if (wRatio < 1 || hRatio < 1)
            {
                Ratio = (wRatio <= hRatio ? wRatio : hRatio);
            }
            return Ratio<1 ? Ratio: 1;
        }

        public System.Drawing.Image PhotoSizeChange(Image image, int width, int height)
        {
            //创建指定大小的图   
            //Image newImage = image.GetThumbnailImage(width, height, null, new IntPtr());
            Bitmap newImage = new Bitmap(width, height);
            newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                //将原图画到指定的图上  
                g.DrawImage(image, 0, 0, width, height);
            }
            return newImage;
        }

        /// <summary>
        /// 将图片Image转换成Byte[]
        /// </summary>
        /// <param name="Image">image对象</param>
        /// <param name="imageFormat">后缀名</param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image Image, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            if (Image == null) { return null; }
            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap Bitmap = new Bitmap(Image))
                {
                    Bitmap.Save(ms, imageFormat);
                    ms.Position = 0;
                    data = new byte[ms.Length];
                    ms.Read(data, 0, Convert.ToInt32(ms.Length));
                    ms.Flush();
                }
            }
            return data;
        }


        /// <summary>
        /// byte[]转换成Image
        /// </summary>
        /// <param name="byteArrayIn">二进制图片流</param>
        /// <returns>Image</returns>
        public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null)
                return null;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArrayIn))
            {
                System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
                ms.Flush();
                return returnImage;
            }
        }

        //byte[] 转换 Bitmap
        public static Bitmap BytesToBitmap(byte[] Bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(Bytes);
                return new Bitmap((Image)new Bitmap(stream));
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }

        //Bitmap转byte[]  
        public static byte[] BitmapToBytes(Bitmap Bitmap)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                Bitmap.Save(ms, Bitmap.RawFormat);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }
        }

    }
}
