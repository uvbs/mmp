using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Admin.Score
{
    public partial class List : System.Web.UI.Page
    {
        protected bool canTotalAmountExport = false;
        protected bool canTotalAmountPrint = false;
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        BLLPermission.BLLMenuPermission bll = new BLLPermission.BLLMenuPermission("");
        protected void Page_Load(object sender, EventArgs e)
        {
            string curUserID = bllUser.GetCurrUserID();
            canTotalAmountExport = bll.CheckUserAndPmsKey(curUserID, BLLPermission.Enums.PermissionSysKey.TotalAmountExport);
            canTotalAmountPrint = bll.CheckUserAndPmsKey(curUserID, BLLPermission.Enums.PermissionSysKey.TotalAmountPrint);
        }
    }
}