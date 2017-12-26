using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Distribution
{
    public partial class WithdrawCashMgr : System.Web.UI.Page
    {
        BLLPermission.BLLPermission bllPer = new BLLPermission.BLLPermission();
        /// <summary>
        /// 审核打款配置
        /// </summary>
        protected bool PmsTransfersAudit;
        protected void Page_Load(object sender, EventArgs e)
        {
            PmsTransfersAudit = bllPer.CheckPermissionKey(bllPer.WebsiteOwner, ZentCloud.BLLPermission.Enums.PermissionSysKey.PMS_TRANSFERSAUDIT);

        }
    }
}