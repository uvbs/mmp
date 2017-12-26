using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Review
{
    public partial class ReviewConfig : System.Web.UI.Page
    {
        protected WebsiteInfo currentWebsiteInfo;
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            currentWebsiteInfo =bll.GetWebsiteInfoModelFromDataBase();
            if (currentWebsiteInfo == null)
            {
                currentWebsiteInfo = new WebsiteInfo();
            }
            else
            {


            }

        }

    }
}