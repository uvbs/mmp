using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using ZentCloud.BLLJIMP;

namespace ZentCloud.WindowsService
{
    partial class AppointmentService : ServiceBase
    {
        private string logPath = ZentCloud.Common.ConfigHelper.GetConfigString("logPath");
        private BLLJuActivity bllJuActivity = new BLLJuActivity();
        public AppointmentService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO:  在此处添加代码以启动服务。

            timerAppointmentEndCheck.Start();
            timerAppointmentEndCheck_Elapsed(null, null);
        }

        protected override void OnStop()
        {
            // TODO:  在此处添加代码以执行停止服务所需的关闭操作。
            timerAppointmentEndCheck.Stop();
        }

        private void timerAppointmentEndCheck_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                LogHelper.WriteLog("约会过期检查", "开始检查", true);
                timerAppointmentEndCheck.Stop();
                bllJuActivity.CheckEndAppointment();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("约会过期检查", "检查出错：" + ex.Message, true);
            }
            finally
            {
                timerAppointmentEndCheck.Start();
            }
        }
    }
}
