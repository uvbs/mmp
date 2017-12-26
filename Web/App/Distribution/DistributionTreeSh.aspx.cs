using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Distribution
{
    public partial class DistributionTreeSh : System.Web.UI.Page
    {
        protected bool canTeamExport = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
            BLLPermission.BLLMenuPermission bll = new BLLPermission.BLLMenuPermission("");
            string curUserID = bllUser.GetCurrUserID();
            canTeamExport = bll.CheckUserAndPmsKey(curUserID, BLLPermission.Enums.PermissionSysKey.TeamExport);
        }
    }
}