using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Admin.AdInfo
{
    public partial class AdInfoManage : System.Web.UI.Page
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        BLLPermission.BLLMenuPermission bllMenupermission = new BLLPermission.BLLMenuPermission("");

        public bool isHide = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            isHide = bllMenupermission.CheckPerRelationByaccount(bllUser.GetCurrUserID(), -1);
        }
    }
}