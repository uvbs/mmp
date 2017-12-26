using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Sys
{
    public partial class CompanyWebsiteTemplateCompile : System.Web.UI.Page
    {
        public string aid = "0";
        public string webAction = "add";
        public string actionStr = "";
        BLLWebSite bll = new BLLWebSite();
        public CompanyWebsiteTemplate model = new CompanyWebsiteTemplate();
        protected void Page_Load(object sender, EventArgs e)
        {
            aid = Request["aid"];
            webAction = Request["Action"];
            actionStr = webAction == "add" ? "添加" : "编辑";
            if (webAction == "edit")
            {
                model = this.bll.GetCompanyWebsiteTemplateById(aid);

                if (model == null)
                {
                    Response.End();
                }
                else
                {

                }
            }




        }

    }
}