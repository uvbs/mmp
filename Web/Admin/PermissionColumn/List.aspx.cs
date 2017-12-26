using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Admin.PermissionColumn
{
    public partial class List : System.Web.UI.Page
    {
        BLLPermission.BLLMenuPermission bllMenuPermission = new BLLPermission.BLLMenuPermission("");
        BLLPermission.BLLMenuInfo bllMenu = new BLLPermission.BLLMenuInfo();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}