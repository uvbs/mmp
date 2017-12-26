﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Admin.Performance
{
    public partial class List : System.Web.UI.Page
    {
        protected bool canPerformanceExport = false;
        protected bool canComputeReward = false;
        protected bool canPerformanceConfrimExport = false;
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        BLLPermission.BLLMenuPermission bll = new BLLPermission.BLLMenuPermission("");
        protected void Page_Load(object sender, EventArgs e)
        {
            string curUserID = bllUser.GetCurrUserID();
            canPerformanceExport = bll.CheckUserAndPmsKey(curUserID, BLLPermission.Enums.PermissionSysKey.PerformanceExport);
            canComputeReward = bll.CheckUserAndPmsKey(curUserID, BLLPermission.Enums.PermissionSysKey.ComputeReward);
            canPerformanceConfrimExport = bll.CheckUserAndPmsKey(curUserID, BLLPermission.Enums.PermissionSysKey.PerformanceConfrimExport);
            
            
        }
    }
}