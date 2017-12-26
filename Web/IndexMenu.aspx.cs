using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace ZentCloud.JubitIMP.Web
{
    public partial class IndexMenu : System.Web.UI.Page
    {
        public ZentCloud.BLLJIMP.Model.UserInfo currUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            currUser = DataLoadTool.GetCurrUserModel();
            if (currUser.UserType != 1)
            {
                this.Response.Redirect(Common.ConfigHelper.GetConfigString("logoutUrl"));
            }
        }




    }
}