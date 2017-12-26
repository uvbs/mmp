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
using ZCJson;

namespace ZentCloud.StockChartService
{
    partial class GetDataService : ServiceBase
    {
        private string aliAppKey;
        private string aliAppSecret;
        private ZentCloud.BLLJIMP.BLLStockHistory bllStock = new ZentCloud.BLLJIMP.BLLStockHistory();
        public GetDataService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var websiteInfo = bllStock.GetByKey<ZentCloud.BLLJIMP.Model.WebsiteInfo>("WebsiteOwner", "stockplayer");
                //LogHelper.WriteLog("股票指数获取", "获取配置：" + JsonConvert.SerializeObject(websiteInfo), true);
                aliAppKey = websiteInfo.AliAppKey;
                aliAppSecret = websiteInfo.AliAppSecret;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("股票指数获取", "获取配置出错：" + ex.Message, true);
            }

            timerGetData.Start();
            timerGetData_Elapsed(null, null);//第一次执行

        }

        protected override void OnStop()
        {
            timerGetData.Stop();
        }

        private void timerGetData_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 09:10")) || DateTime.Now > DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 15:05")))
            {
                return;
            }
            if (DateTime.Now > DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 11:35")) && DateTime.Now < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 12:55")))
            {
                return;
            }

            try
            {
                LogHelper.WriteLog("股票指数获取", "开始获取沪指指数", true);
                //timerGetData.Stop();
                string str=bllStock.GetAliStockInfo(aliAppKey, aliAppSecret);
                LogHelper.WriteLog("股票指数获取", "获取沪指数据：" + str, true);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("股票指数获取", "获取沪指指数出错：" + ex.Message, true);
            }
            finally
            {
                //timerGetData.Start();
            }
        }
    }
}
