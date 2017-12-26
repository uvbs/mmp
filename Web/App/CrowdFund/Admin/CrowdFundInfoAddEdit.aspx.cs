using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.CrowdFund.Admin
{
    public partial class CrowdFundInfoAddEdit : System.Web.UI.Page
    {
        BLLJIMP.BLL bllBase = new BLLJIMP.BLL();
        public string id = "0";
        public string actionStr = "添加";
        public string currAction = "add";
        public CrowdFundInfo model = new CrowdFundInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Request["id"];
            if (!string.IsNullOrEmpty(Request["id"]))
            {
                actionStr = "编辑";
                currAction = "edit";
                model = bllBase.Get<CrowdFundInfo>(string.Format("WebSiteOwner='{0}' And AutoID={1}",bllBase.WebsiteOwner,id));
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