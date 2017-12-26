using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Admin.Score
{
    public partial class PrintList : System.Web.UI.Page
    {
        protected bool canWithdrawExport = false;
        protected bool canWithdrawPrint = false;
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        BLLPermission.BLLMenuPermission bll = new BLLPermission.BLLMenuPermission("");
        protected void Page_Load(object sender, EventArgs e)
        {
            string curUserID = bllUser.GetCurrUserID();
            canWithdrawExport = bll.CheckUserAndPmsKey(curUserID, BLLPermission.Enums.PermissionSysKey.WithdrawExport);
            canWithdrawPrint = bll.CheckUserAndPmsKey(curUserID, BLLPermission.Enums.PermissionSysKey.WithdrawPrint);
        }
    }
}