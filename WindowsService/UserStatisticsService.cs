using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using ZentCloud.BLLJIMP;
using CommonPlatform.Helper;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.WindowsService
{
    partial class UserStatisticsService : ServiceBase
    {
        Thread th;

        public UserStatisticsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            /*
            每隔一小时执行一次方法
            */

            LogHelper.WriteLog("会员扩展任务", "开始会员扩展任务服务", true);
            if (th != null)
            {
                th.Abort();
            }

            th = new Thread(new ThreadStart(procUserStatistics)); //          
            th.Start(); //启动线程  


        }

        protected override void OnStop()
        {
            th.Abort();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        static void procUserStatistics()
        {
            do
            {
                BLLMall bll = new BLLMall();
                BLLUser bllUser = new BLLUser();

                var list = bll.GetList<MemberExActionInfo>(" IsAction = 0 AND ActionTime < GETDATE() ");

                if (list != null)
                {
                    LogHelper.WriteLog("会员扩展任务", "取到数据条数：" + list.Count, true);


                    for (int i = 0; i < list.Count; i++)
                    {
                        LogHelper.WriteLog("会员扩展任务", "正在处理索引：" + i, true);

                        var modelOrderId = list[i].ModelId;//模板id

                        var data = bll.GetOrderInfo(modelOrderId);
                        var details = bll.GetOrderDetailsList(modelOrderId);

                        var user = bllUser.GetUserInfoByAutoID(int.Parse(list[i].MemberId), data.WebsiteOwner);

                        if (user == null)
                        {
                            continue;
                        }

                        var newData = data;
                        newData.OrderID = bll.GetGUID(BLLJIMP.TransacType.AddMallOrder);
                        newData.InsertDate = list[i].ActionTime;
                        newData.OrderUserID = user.UserID;
                        newData.Consignee = user.WXNickname;
                        newData.PayTime = list[i].ActionTime;

                        if (bll.Add(newData))
                        {

                            for (int j = 0; j < details.Count; j++)
                            {
                                var newDetail = details[j];
                                newDetail.AutoID = null;
                                newDetail.OrderID = newData.OrderID;

                                bll.Add(newDetail);

                            }

                            list[i].ProcessTime = DateTime.Now;
                            list[i].IsAction = 1;

                            if (bll.Update(list[i]))
                            {
                                LogHelper.WriteLog("会员扩展任务", "处理并更新状态成功：" + i, true);

                            }

                        }

                    }


                }

                Thread.Sleep(new TimeSpan(0, 1, 0, 0));

            } while (true);
        }

    }
}
