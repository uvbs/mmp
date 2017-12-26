using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class UserList : System.Web.UI.Page
    {
        protected string moduleName = "积分";
        protected UserInfo curUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.Request["moduleName"])) moduleName = this.Request["moduleName"];
            BLLUser bllUser = new BLLUser();
            curUser = bllUser.GetCurrentUserInfo();
        }
    }
}