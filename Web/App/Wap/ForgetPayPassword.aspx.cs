using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.App.Wap
{
    public partial class ForgetPayPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!MemberCenter.checkUser(this.Context))
            {
                return;
            }
        }
    }
}