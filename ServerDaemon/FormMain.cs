using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ServerDaemon
{
    public partial class FormMain : Form
    {
        //更新界面委托
        public delegate void UpdateLogDel(string str);
        public void updateMessageLog(string str)
        {
            this.lstBoxLog.Items.Add(DateTime.Now.ToString() + "  " + str);
            this.lstBoxLog.SelectedIndex = this.lstBoxLog.Items.Count - 1;

            if (this.lstBoxLog.Items.Count > 1000)
                this.lstBoxLog.Items.Clear(); ;
            return;
        }

        Working work;
        public FormMain()
        {
            InitializeComponent();
            

        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要退出吗？", "退出", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (work!=null)
                {
                    this.work.StopWork();
                }
               
                this.Dispose();
                System.Environment.Exit(0);

            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            work = new Working(this);
            this.work.StartWork();
            this.btnStart.Visible = false;
            this.btnStop.Visible = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.work.StopWork();
            this.btnStart.Visible = true;
            this.btnStop.Visible = false;
        }

        private void txtInterval_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Working.WorkInterval = (int)(decimal.Parse(this.txtInterval.Text) * 1000);
            }
            catch
            {
                Working.WorkInterval = 5000;
            }
        }

        private void txtInterval_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                Working.WorkInterval = (int)(decimal.Parse(this.txtInterval.Text) * 1000);
            }
            catch
            {
                Working.WorkInterval = 5000;
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (work != null)
            {
                this.work.StopWork();
            }

            this.Dispose();
            System.Environment.Exit(0);
        }

       

       

    }
}
