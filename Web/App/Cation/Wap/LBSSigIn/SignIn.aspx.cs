using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.LBSSigIn
{
    public partial class SignIn : System.Web.UI.Page
    {
        BLLJIMP.BLLSignIn bllSignIn = new BLLJIMP.BLLSignIn();
        public int addressId = 0;
        protected SignInAddress model = new SignInAddress();
        protected void Page_Load(object sender, EventArgs e)
        {
            addressId = int.Parse(Request["addressId"]);
            model = bllSignIn.GetSignInAddress(bllSignIn.WebsiteOwner, Request["addressId"]);
        }
    }
}