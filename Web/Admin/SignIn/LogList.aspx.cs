using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZCJson;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.SignIn
{
    public partial class LogList : System.Web.UI.Page
    {
        BLLJIMP.BLLSignIn bllSignIn = new BLLJIMP.BLLSignIn();
        protected SignInAddress model = new SignInAddress();
        public string addressId = "";
        public List<SignInTime> tempList = new List<SignInTime>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["addressid"]))
            {
                addressId = Request["addressid"];
                int totalCount = 0;
                List<SignInLog> list = bllSignIn.GetSignLogList(1, int.MaxValue, "", addressId, out totalCount, "", "", "", "");
                var userList = list.Select(p => p.UserID).Distinct().ToList();
                if (!string.IsNullOrEmpty(addressId))
                {
                    model = bllSignIn.GetSignInAddress(bllSignIn.WebsiteOwner, addressId);
                }
                if (!string.IsNullOrEmpty(model.SignInTime))
                {
                     tempList = JsonConvert.DeserializeObject<List<SignInTime>>(model.SignInTime);
                }

            }
        }




        public class SignInTime
        {
            public string name { get; set; }
            public DateTime start { get; set; }
            public DateTime stop { get; set; }
        }
    }
}