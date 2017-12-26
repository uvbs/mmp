using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Admin.KeyVauleData
{
    public partial class KeyVauleDataManage : System.Web.UI.Page
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        BLLPermission.BLLMenuPermission bllMenupermission = new BLLPermission.BLLMenuPermission("");

        protected bool isHide = false;
        protected string isAutoKey = "";
        protected string redirect = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            isAutoKey = this.Request["isAutoKey"];
            redirect = this.Request["redirect"];
            isHide = bllMenupermission.CheckPerRelationByaccount(bllUser.GetCurrUserID(), -1);
        }
    }
}