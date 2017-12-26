using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Wap
{
    public partial class UpgradeMember : System.Web.UI.Page
    {
        protected WebsiteInfo website = new WebsiteInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!MemberCenter.checkUser(this.Context))
            {
                return;
            }
            BLLUser bllUser = new BLLUser();
            website = bllUser.GetColByKey<WebsiteInfo>("WebsiteOwner", bllUser.WebsiteOwner, "WebsiteOwner,TotalAmountShowName");
        }
    }
}