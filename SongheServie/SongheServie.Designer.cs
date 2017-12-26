namespace SongheServie
{
    partial class SongheServie
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
            this.timerBuildAmout = new System.Timers.Timer();
            this.timerUnLockScore = new System.Timers.Timer();
            this.timerBuildPerformance = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timerBuildAmout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerUnLockScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerBuildPerformance)).BeginInit();
            // 
            // timerBuildAmout
            // 
            this.timerBuildAmout.Enabled = true;
            this.timerBuildAmout.Interval = 14400000D;
            this.timerBuildAmout.Elapsed += new System.Timers.ElapsedEventHandler(this.timerBuildAmout_Elapsed);
            // 
            // timerUnLockScore
            // 
            this.timerUnLockScore.Enabled = true;
            this.timerUnLockScore.Interval = 10000D;
            this.timerUnLockScore.Elapsed += new System.Timers.ElapsedEventHandler(this.timerUnLockScore_Elapsed);
            // 
            // timerBuildPerformance
            // 
            this.timerBuildPerformance.Enabled = true;
            this.timerBuildPerformance.Interval = 43200000D;
            this.timerBuildPerformance.Elapsed += new System.Timers.ElapsedEventHandler(this.timerBuildPerformance_Elapsed);
            // 
            // SongheServie
            // 
            this.ServiceName = "SongheServie";
            ((System.ComponentModel.ISupportInitialize)(this.timerBuildAmout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerUnLockScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerBuildPerformance)).EndInit();

        }

        #endregion

        private System.Timers.Timer timerBuildAmout;
        private System.Timers.Timer timerUnLockScore;
        private System.Timers.Timer timerBuildPerformance;
    }
}
