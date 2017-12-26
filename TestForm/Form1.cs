
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using ZCJson.Linq;
//using ZentCloud.BLLJIMP;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            DateTime nowDt = DateTime.Now;
            nowDt = DateTime.Parse("2015-11-24 18:34:41.2133322");
            label1.Text = (nowDt - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds.ToString();
            label2.Text = (nowDt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds.ToString();

            label3.Text = (nowDt - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds.ToString();
            label4.Text = Convert.ToInt64((nowDt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds).ToString();
            long tt = Convert.ToInt64(1448361281286);
            label5.Text = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(tt).ToLocalTime().ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //BLLJuActivity bllJuActivity = new BLLJuActivity();
            //bllJuActivity.CheckEndAppointment();

            //BLLWeixin bllWeixin = new BLLWeixin();
            //string accessToken = "JY-4GzqEL5akbOz359unYY7_V6ZD5wHuPIjOqp8goXFrpFbh36bV8KUpfEnXXUswd1CKT4Pg7G1dStm2nBNoi4Kdl8t4N1NSJ0EY9B00_kgUUBrAKACCJ"; //bllWeixin.GetAccessToken("shanghuiyigou");
            //JToken SendData = JToken.Parse("{}");
            //SendData["touser"] = "o6gBIxMQKA3YNa8Pwq7F7GYOsLGU";
            //SendData["url"] = "http://www.baidu.com";
            //SendData["first"] = "要约编号：20151027174200";
            //SendData["keyword1"] = "中远国际";
            //SendData["keyword2"] = "2015-10-27 14:32";
            //SendData["remark"] = "标题：甲苯1000T\n要约内容：甲苯1000T，6420元/T，宁兴库，买家保证金5%，卖家保证金10%，交割日期2015-10-26/2015-11-10是否同意此次交易";

           // bllWeixin.SendTemplateMessage(accessToken, "7431", "shanghuiyigou", SendData);
        }
    }
}
