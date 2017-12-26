using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.SignIn
{
    public partial class AddressByAdd : System.Web.UI.Page
    {
        BLLJIMP.BLLSignIn bllSignIn = new BLLJIMP.BLLSignIn();
        public SignInAddress model = new SignInAddress();
        public string action = "add";
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request["id"];
            if (!string.IsNullOrEmpty(id))
            {
                model = bllSignIn.GetSignInAddress(bllSignIn.WebsiteOwner, id);
                action = "update";
            }
            else
            {
                model.Range = 100;
            }

            //if (action == "update")
            //{
            //    List<SignInDate> list = ZentCloud.Common.JSONHelper.JsonToObjectList<SignInDate>(model.SignInTime);
            //}
        }


        //public class SignInDate
        //{
        //    public string name { get; set; }
        //    public string start { get; set; }
        //    public string stop { get; set; }
        //}
    }
}