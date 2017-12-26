using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using System.Threading;

namespace SongheServie
{
    partial class SongheServie : ServiceBase
    {
        BLLUser bllUser = new BLLUser();
        BLLDistribution bllDis = new BLLDistribution();
        string startUserAutoID = "1000004";
        string websiteOwner = "songhe";
        int lockDayNum = 7;
        int lastYearMonth = 0;
        public SongheServie()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO:  在此处添加代码以启动服务。
            timerBuildAmout.Interval = new TimeSpan(4, 0, 0).TotalMilliseconds;
            timerBuildAmout.Start();
            timerBuildAmout_Elapsed(null, null);//第一次执行

            timerUnLockScore.Interval = new TimeSpan(0,1,0).TotalMilliseconds;
            timerUnLockScore.Start();
            timerUnLockScore_Elapsed(null, null);//第一次执行

            timerBuildPerformance.Interval = new TimeSpan(12, 0, 0).TotalMilliseconds;
            timerBuildPerformance.Start();
            timerBuildPerformance_Elapsed(null, null);//第一次执行

        }

        protected override void OnStop()
        {
            // TODO:  在此处添加代码以执行停止服务所需的关闭操作。
        }

        /// <summary>
        /// 汇总金额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerBuildAmout_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timerBuildAmout.Stop();
                LogHelper.WriteLog("汇总金额", "开始汇总金额", true);
                List<UserInfo> uList = bllUser.GetColList<UserInfo>(int.MaxValue, 1,
                    string.Format("MemberLevel>=10 And WebsiteOwner='{0}' ", websiteOwner),"AutoID");
                foreach (UserInfo item in uList)
                {
                    bllDis.CheckTotalAmount(item.AutoID, websiteOwner, lockDayNum);
                    //Thread.Sleep(2000);
                }
                LogHelper.WriteLog("汇总金额", "完成汇总金额", true);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("汇总金额", "汇总金额出错：" + ex.Message, true);
            }
            finally
            {
                timerBuildAmout.Start();
            }
        }
        /// <summary>
        /// 解冻金额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerUnLockScore_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timerUnLockScore.Stop();
                LogHelper.WriteLog("解冻金额", "开始解冻金额", true);
                bllDis.UnLockTotalAmount();
                LogHelper.WriteLog("解冻金额", "完成解冻金额", true);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("解冻金额", "解冻金额出错：" + ex.Message, true);
            }
            finally
            {
                timerUnLockScore.Start();
            }
        }

        private void timerBuildPerformance_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timerBuildPerformance.Stop();
                LogHelper.WriteLog("计算业绩", "开始计算业绩", true);
                string yearMonthString = DateTime.Now.ToString("yyyyMM");
                int yearMonth = Convert.ToInt32(yearMonthString);
                bllDis.BuildMonthPerformance(yearMonth, websiteOwner);

                string lastyearMonthString = DateTime.Now.AddMonths(-1).ToString("yyyyMM");
                int curLastyearMonth = Convert.ToInt32(lastyearMonthString);
                if (curLastyearMonth != lastYearMonth)
                {
                    bllDis.BuildMonthPerformance(curLastyearMonth, websiteOwner);
                    lastYearMonth = curLastyearMonth;
                }

                LogHelper.WriteLog("计算业绩", "完成计算业绩", true);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("计算业绩", "计算业绩出错：" + ex.Message, true);
            }
            finally
            {
                timerBuildPerformance.Start();
            }
        }
    }
}
