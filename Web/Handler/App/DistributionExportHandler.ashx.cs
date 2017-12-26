using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    ///分销提现 导出excel
    /// </summary>
    public class DistributionExportHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");

        BLLJIMP.BLLLog bllLog = new BLLJIMP.BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            if (bllUser.IsLogin)
            {
                var currentUserInfo=bllUser.GetCurrentUserInfo();
                if (currentUserInfo.UserType.Equals(1)||currentUserInfo.UserID.Equals(bllUser.WebsiteOwner))
                {
                    bllLog.Add(BLLJIMP.Enums.EnumLogType.DistributionOffLine, BLLJIMP.Enums.EnumLogTypeAction.Export, bllLog.GetCurrUserID(), "导出分销申请提现数据");
                    string tranIds = context.Request["tranids"];
                    System.Data.DataTable dt = new System.Data.DataTable();
                    StringBuilder sbSql = new StringBuilder();
                    sbSql.AppendFormat("SELECT AutoID as 商户流水号,AccountName as 收款银行户名,BankAccount as 收款银行账号,BankName as 收款开户银行, AccountBranchName as 收款开户网点名称,AccountBranchProvince as 开户行省份,AccountBranchCity as 开户行所在市,RealAmount as 金额,IsPublic as 对公私标识,Remark as 备注 from ZCJ_WithdrawCash where AutoID in({0}) ", tranIds);
                    dt = ZentCloud.ZCBLLEngine.BLLBase.Query(sbSql.ToString(), "WithdrawCash").Tables[0];
                    DataLoadTool.ExportDataTable(dt, string.Format("提现申请IE浏览器请把后缀名改为xls{0}.xls", DateTime.Now.ToString("yyyyMMddHHmm")));
                }
            }




        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}