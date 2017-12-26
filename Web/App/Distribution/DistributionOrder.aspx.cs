using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Distribution
{
    
    public partial class DistributionOrder : System.Web.UI.Page
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        public UserInfo userInfo = new UserInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            userInfo = bllUser.GetUserInfoByAutoID(int.Parse(Request["uid"]));
        }
    }
}