using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;


namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class MasterCompile : System.Web.UI.Page
    {
        public int mid = 0;
        public string webAction = "add";
        public string actionStr = "";
        public JuMasterInfo model = new JuMasterInfo();
        public BLLJuActivity juActivityBll = new BLLJuActivity(Comm.DataLoadTool.GetCurrUserID());

        protected void Page_Load(object sender, EventArgs e)
        {
            mid = Convert.ToInt32(Request["mid"]);
            webAction = Request["Action"];
            actionStr = webAction == "add" ? "添加" : "编辑";

            if (webAction == "edit")
            {
                model = this.juActivityBll.Get<JuMasterInfo>(string.Format(" MasterID = '{0}' ", mid));

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