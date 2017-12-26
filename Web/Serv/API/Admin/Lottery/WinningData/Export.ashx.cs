
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery.WinningData
{
    /// <summary>
    /// 导出中奖信息
    /// </summary>
    public class Export : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BllLottery bllLottery = new BLLJIMP.BllLottery();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLLog bllLog = new BLLJIMP.BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            int lotteryId = int.Parse(context.Request["LotteryId"]);
            bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing,BLLJIMP.Enums.EnumLogTypeAction.Export, bllLog.GetCurrUserID(), "导出中奖名单");
            DataTable dt = bllLottery.QueryLotteryData(lotteryId);
            foreach (DataRow dr in dt.Rows)
            {
                UserInfo userInfo = bllUser.GetUserInfo(dr["UserID"].ToString());
                if (userInfo!=null)
                {
                    if (string.IsNullOrEmpty(dr["姓名"].ToString()))
                    {
                        dr["姓名"] = userInfo.TrueName;
                    }
                    if (string.IsNullOrEmpty(dr["手机"].ToString()))
                    {
                        dr["手机"] = userInfo.Phone;
                    }
                }

            }
            dt.Columns.Remove("UserID");
            DataLoadTool.ExportDataTable(dt, string.Format("{0}_{1}data.xls", "中奖名单", DateTime.Now.ToString()));
        }
    }
}