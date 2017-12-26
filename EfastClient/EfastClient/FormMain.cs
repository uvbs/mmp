using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EfastClient
{
    public partial class FormMain : Form
    {
                //更新界面委托
        public delegate void UpdateLogDel(string str);
        public void updateMessageLog(string str)
        {
            this.lstBoxLog.Items.Add(DateTime.Now.ToString() + "  " + str);
            this.lstBoxLog.SelectedIndex = this.lstBoxLog.Items.Count - 1;

            if (this.lstBoxLog.Items.Count > 10000)
                this.lstBoxLog.Items.Clear(); 
            return;
        }

        Working w;
        public FormMain()
        {
            InitializeComponent();
            

        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要退出吗？", "退出", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.w.StopWork();
                this.Dispose();
                System.Environment.Exit(0);

            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            w = new Working(this);
            this.w.StartWork();
            this.btnStart.Visible = false;
            this.btnStop.Visible = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.w.StopWork();
            this.btnStart.Visible = true;
            this.btnStop.Visible = false;
        }

        //private void txtInterval_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Working.WorkInterval = (int)(decimal.Parse(this.txtInterval.Text) * 1000);
        //        //Working.WorkInterval = (int)(decimal.Parse(this.txtInterval.Text));
        //    }
        //    catch
        //    {
        //        Working.WorkInterval = 100;
        //    }
        //}



        private void btnStart_Click_1(object sender, EventArgs e)
        {
            w = new Working(this);
            this.w.StartWork();
            this.btnStart.Visible = false;
            this.btnStop.Visible = true;

        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            w = new Working(this);
            this.w.StartWork();
        }


    }
}
