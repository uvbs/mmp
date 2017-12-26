using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Admin.User
{
    public partial class UserList : System.Web.UI.Page
    {
        protected string vipEndDate;
        protected void Page_Load(object sender, EventArgs e)
        {
            vipEndDate = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
        }
    }
}