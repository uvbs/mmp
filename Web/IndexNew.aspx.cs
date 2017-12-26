using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace ZentCloud.JubitIMP.Web
{
    public partial class IndexNew : System.Web.UI.Page
    {

        public ZentCloud.BLLJIMP.Model.UserInfo currUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            currUser = DataLoadTool.GetCurrUserModel();

        }
    }
}