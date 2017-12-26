using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class AlipayConfig : System.Web.UI.Page
    {
        BLLJIMP.BLL bll=new BLLJIMP.BLL("");
        public ZentCloud.BLLJIMP.Model.PayConfig model;
        protected void Page_Load(object sender, EventArgs e)
        {
            model = bll.Get<ZentCloud.BLLJIMP.Model.PayConfig>(string.Format(" WebsiteOwner='{0}'", bll.WebsiteOwner));


        }
    }
}