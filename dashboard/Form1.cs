using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace dashboard
{
    public partial class Form1 : Form
    {
        List<string> websiteList = new List<string>();
        bool isInBuild = false;
        bool isAutoBuild = false;
        int lastBuildDay = 0;

        public Form1()
        {
            InitializeComponent();
            timerShowNowTime.Start();
            timerShowNowTime_Tick(null, null);
            button4.Enabled = false;
        }
        BLLDashboard bllDashboard = new BLLDashboard();

        private void SetEnableFalse()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            dateTimePicker1.Enabled = true;
            dateTimePicker2.Enabled = true;
        }

        private void SetEnable(object enabled)
        {
            if (button1.InvokeRequired)
            {
                D d = new D(SetEnable);
                button1.Invoke(d, enabled);

            }
            else
            {
                button1.Enabled = Convert.ToBoolean(enabled);
                button2.Enabled = Convert.ToBoolean(enabled);
                button3.Enabled = Convert.ToBoolean(enabled);
                button4.Enabled = Convert.ToBoolean(enabled);
                dateTimePicker1.Enabled = Convert.ToBoolean(enabled);
                dateTimePicker2.Enabled = Convert.ToBoolean(enabled);
                dateTimePicker3.Enabled = Convert.ToBoolean(enabled);
            }
        }

        private void SetAutoEnable(object enabled)
        {
            if (button1.InvokeRequired)
            {
                D d = new D(SetAutoEnable);
                button1.Invoke(d, enabled);

            }
            else
            {
                isAutoBuild = Convert.ToBoolean(enabled);
                button1.Enabled = !Convert.ToBoolean(enabled);
                button2.Enabled = !Convert.ToBoolean(enabled);
                button3.Enabled = !Convert.ToBoolean(enabled);
                button4.Enabled = Convert.ToBoolean(enabled);
                dateTimePicker1.Enabled = !Convert.ToBoolean(enabled);
                dateTimePicker2.Enabled = !Convert.ToBoolean(enabled);
                dateTimePicker3.Enabled = !Convert.ToBoolean(enabled);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (dateTimePicker2.Value <= dateTimePicker1.Value)
            {
                MessageBox.Show("开始时间必须小于结束时间");
                return;
            }
            listView1.Items.Clear();
            SetEnable(false);
            new Thread(new ThreadStart(BuildDashboardLog)).Start();
        }
        private void BuildDashboardLog()
        {
            int sdate = ZentCloud.Common.DateTimeHelper.ToDateInt8ByDateTime(dateTimePicker1.Value);
            int edate = ZentCloud.Common.DateTimeHelper.ToDateInt8ByDateTime(dateTimePicker2.Value);
            new Thread(new ParameterizedThreadStart(WriteInfo)).Start(new LogInfo() { status = true, datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), txt = "BuildDashboardLog：开始：sdate：" + sdate.ToString() });
            new Thread(new ParameterizedThreadStart(WriteInfo)).Start(new LogInfo() { status = true, datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), txt = "BuildDashboardLog：开始：edate：" + edate.ToString() });

            try
            {
                websiteList = new List<string>();
                List<WebsiteInfo> WebsiteInfoList = bllDashboard.GetColList<WebsiteInfo>(int.MaxValue, 1, "1=1", "WebsiteOwner");
                if (WebsiteInfoList.Count > 0) websiteList = WebsiteInfoList.Where(p=>!p.WebsiteOwner.Contains("'")).Select(p => p.WebsiteOwner).ToList();

                bllDashboard.BuildDashboardLog(websiteList, dateTimePicker1.Value, dateTimePicker2.Value);
                WriteInfo(new LogInfo() { status = true, datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), txt = "BuildDashboardLog：成功" });
            }
            catch (Exception ex)
            {
                WriteInfo(new LogInfo() { status = false, datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), txt = "BuildDashboardLog：出错："+ex.Message });
            }
            finally
            {
                //Thread.Sleep(2000);
                SetEnable(true);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (dateTimePicker3.Value >= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
            {
                MessageBox.Show("日期必须小于当前日期");
                return;
            }
            listView1.Items.Clear();
            SetEnable(false);
            new Thread(new ThreadStart(BuildDashboardInfo)).Start();
        }
        
        private void BuildDashboardInfo(){
            int date = ZentCloud.Common.DateTimeHelper.ToDateInt8ByDateTime(dateTimePicker3.Value);
            new Thread(new ParameterizedThreadStart(WriteInfo)).Start(new LogInfo() { status = true, datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), txt = "BuildDashboardInfo：开始：date：" + date.ToString() });

            try
            {
                websiteList = new List<string>();
                List<WebsiteInfo> WebsiteInfoList = bllDashboard.GetColList<WebsiteInfo>(int.MaxValue, 1, "1=1", "WebsiteOwner");
                if (WebsiteInfoList.Count > 0) websiteList = WebsiteInfoList.Where(p => !p.WebsiteOwner.Contains("'")).Select(p => p.WebsiteOwner).ToList();

                bllDashboard.BuildDashboardInfo(websiteList, dateTimePicker3.Value);
                WriteInfo(new LogInfo() { status = true, datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), txt = "BuildDashboardInfo：成功"});
            }
            catch (Exception ex)
            {
                WriteInfo(new LogInfo() { status = false, datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), txt = "BuildDashboardInfo：出错：" + ex.Message });
            }
            finally
            {
                //Thread.Sleep(2000);
                SetEnable(true);
            }
        }
        delegate void D(object obj);
        private void WriteInfo(object log)
        {
            if (listView1.InvokeRequired)
            {
                D d = new D(WriteInfo);
                listView1.Invoke(d, log);

            }
            else
            {
                LogInfo nlog = (LogInfo)log;
                ListViewItem lvi = new ListViewItem();
                if (!nlog.status)
                {
                    lvi.ForeColor = System.Drawing.Color.Red;
                }
                lvi.Text = nlog.datetime;
                lvi.SubItems.Add(nlog.txt);
                listView1.Items.Add(lvi);
                listView1.EnsureVisible(listView1.Items.Count - 1);
                //if (this.listView1.Items..TopIndex == this.listBox1.Items.Count - (int)(this.listBox1.Height / this.listBox1.ItemHeight))
                //    scroll = true; 


                //if (scroll) 
                //    this.listBox1.TopIndex = this.listBox1.Items.Count - (int)(this.listBox1.Height / this.listBox1.ItemHeight); 
            }
        }

        delegate void DelegateShowNowTime();
        private void ShowNowDateTime()
        {
            if (label5.InvokeRequired)
            {
                DelegateShowNowTime d = new DelegateShowNowTime(ShowNowDateTime);
                label5.Invoke(d);
            }
            else
            {
                label5.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public class LogInfo{
            public bool status{ get; set; }
            public string datetime { get; set; }
            public string txt { get; set; }
        }

        private void AutoBuild()
        {
            DateTime nDate = DateTime.Now.AddDays(-1);
            int nDateInt = ZentCloud.Common.DateTimeHelper.ToDateInt8ByDateTime(nDate);
            if (nDateInt <= lastBuildDay && nDate.Hour < 3)
            {
                isInBuild = false;
                return; 
            }

            new Thread(new ParameterizedThreadStart(WriteInfo)).Start(new LogInfo() { status = true, datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), txt = "AutoBuildDashboard：开始：date：" + nDateInt });
            try
            {
                websiteList = new List<string>();
                List<WebsiteInfo> WebsiteInfoList = bllDashboard.GetColList<WebsiteInfo>(int.MaxValue, 1, "1=1", "WebsiteOwner");
                if (WebsiteInfoList.Count > 0) websiteList = WebsiteInfoList.Where(p => !p.WebsiteOwner.Contains("'")).Select(p => p.WebsiteOwner).ToList();

                bllDashboard.BuildDashboardLog(websiteList, nDate, nDate);
                bllDashboard.BuildDashboardInfo(websiteList, nDate);

                lastBuildDay = nDateInt;
                WriteInfo(new LogInfo() { status = true, datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), txt = "AutoBuildDashboard：成功" });
            }
            catch (Exception ex)
            {
                WriteInfo(new LogInfo() { status = false, datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), txt = "AutoBuildDashboard：出错：" + ex.Message });
            }
            finally
            {
                isInBuild = false;
                //Thread.Sleep(2000);
            }
        }

        private void timerShowNowTime_Tick(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(ShowNowDateTime)).Start();
            if (isAutoBuild && !isInBuild)
            {
                DateTime nDate = DateTime.Now.AddDays(-1);
                int nDateInt = ZentCloud.Common.DateTimeHelper.ToDateInt8ByDateTime(nDate);
                if (lastBuildDay == 0 || (nDateInt > lastBuildDay && nDate.Hour >= 3))
                {
                    isInBuild = true;
                    new Thread(new ThreadStart(AutoBuild)).Start();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lastBuildDay = 0;
            SetAutoEnable(true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetAutoEnable(false);
        }
    }
}
