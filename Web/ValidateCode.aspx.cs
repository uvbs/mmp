using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Web
{
    public partial class ValidateCode : System.Web.UI.Page
    {
        private void Page_Load(object sender, EventArgs e)
        {
            int maxLength = 6;
            string onlyNumber;
            if (!string.IsNullOrWhiteSpace(Request["maxLength"])) maxLength = Convert.ToInt32(Request["maxLength"]);
            onlyNumber = Request["onlyNumber"];

            //每次载入时执行创建验证码图像并填写验证码字符函数，保证每次刷新的验证码都不同。
            this.CreateCheckCodeImage(GenerateCheckCode(maxLength, onlyNumber));
        }

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion

        public string GenerateCheckCode(int maxLength, string onlyNumber) //产生随机验证码字符函数
        {
            int number;
            char code;
            string checkCode = String.Empty;

            System.Random random = new Random();

            for (int i = 0; i < maxLength; i++) //字符和数字的混合.长度为5。其实i的大小可以自由设置
            {
                number = random.Next();

                //code = (char)('M');
                if (onlyNumber == "1")
                {
                    code = (char)('0' + (char)(number % 10));
                }
                else
                {
                    if (number % 2 == 0) //偶数
                        code = (char)('0' + (char)(number % 10));
                    else
                        code = (char)('A' + (char)(number % 26));
                }

                checkCode += code.ToString();
            }

            Session["CheckCode"] = checkCode.ToLower();
            //把产生的验证码保存到COOKIE中

            return checkCode;//返回结果以供CreateCheckCodeImage()函数使用
        }

        //以下函数是创建验证码图像并填写验证码字符

        private void CreateCheckCodeImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;//先判断传入的验证码是否有效或非空

            Bitmap image = new Bitmap((int)Math.Ceiling((checkCode.Length * 25.0) + 8), 35);//用指定的大小初始化Bitmap类创建图像对象
            Graphics g = Graphics.FromImage(image);//表示在指定的图像上写字

            try
            {
                //生成随机生成器
                Random random = new Random();

                //清空图片背景色
                g.Clear(Color.White);

                //画图片的背景噪音线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }//用银色笔在图像区域内划线形成背景噪音线

                Font font = new Font("Arial", 21, (FontStyle.Bold | FontStyle.Italic));//设置验证码的字体属性
                //Brush brush = new SolidBrush(Color.Blue);
                Brush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.4f, true);
                StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                //用画笔画出高级的2D向量图形字体
                g.DrawString(checkCode, font, brush, new Rectangle(0, 2, image.Width, image.Height), sf);//以字符串的形式输出

                //画图片的前景噪音点
                for (int i = 0; i < 100; i++)//i的大小可以自由设置
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                } //随机设置图像的像素

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //用银色笔画出边框线

                System.IO.MemoryStream ms = new System.IO.MemoryStream();//申明一个内存流对象
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                //把生成的gif以字节数组的形式保存在内存里
                Response.ClearContent();//清空缓冲区中的内容
                Response.ContentType = "image/Gif";//输出内容的类型，即以gif的文件格式输出
                Response.BinaryWrite(ms.ToArray());
                //把字节数组一二进制的形式输出，即以基本数据类型形式输出
            }
            finally
            {
                g.Dispose();//释放绘图对象
                image.Dispose();//释放图像对象
            }
        }
    }
}
