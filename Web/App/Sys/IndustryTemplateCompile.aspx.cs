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
    public partial class IndustryTemplateCompile : System.Web.UI.Page
    {
        public int aid = 0;
        public string webAction = "add";
        public string actionStr = "";
        BLL bll = new BLL("");
        public IndustryTemplate CurrTemplateModel = new IndustryTemplate();
        protected void Page_Load(object sender, EventArgs e)
        {
            aid = Convert.ToInt32(Request["aid"]);
            webAction = Request["Action"];
            actionStr = webAction == "add" ? "添加" : "编辑";
            if (webAction == "edit")
            {
                CurrTemplateModel = bll.Get<IndustryTemplate>(string.Format("AutoID={0}",aid));
                if (CurrTemplateModel == null)
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