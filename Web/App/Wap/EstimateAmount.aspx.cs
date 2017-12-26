using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Wap
{
    public partial class EstimateAmount : System.Web.UI.Page
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
            this.Title = string.Format("账面{0}", website.TotalAmountShowName);
        }
    }
}