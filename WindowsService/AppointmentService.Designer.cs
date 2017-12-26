namespace ZentCloud.WindowsService
{
    partial class AppointmentService
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.timerAppointmentEndCheck = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timerAppointmentEndCheck)).BeginInit();
            // 
            // timerAppointmentEndCheck
            // 
            this.timerAppointmentEndCheck.Enabled = true;
            this.timerAppointmentEndCheck.Interval = 600000D;
            this.timerAppointmentEndCheck.Elapsed += new System.Timers.ElapsedEventHandler(this.timerAppointmentEndCheck_Elapsed);
            // 
            // AppointmentService
            // 
            this.ServiceName = "AppointmentService";
            ((System.ComponentModel.ISupportInitialize)(this.timerAppointmentEndCheck)).EndInit();

        }

        #endregion

        private System.Timers.Timer timerAppointmentEndCheck;
    }
}
