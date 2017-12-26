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
    public partial class CourseCompile : System.Web.UI.Page
    {
        public int aid = 0;
        public string webAction = "add";
        public string actionStr = "";
        BLLJuActivity juActivityBll = new BLLJuActivity(Comm.DataLoadTool.GetCurrUserID());
        public JuActivityInfo CurrActivityModel = new JuActivityInfo();
        public bool Pms_Advanced;
        protected void Page_Load(object sender, EventArgs e)
        {
            aid = Convert.ToInt32(Request["aid"]);
            webAction = Request["Action"];
            Pms_Advanced = Comm.DataLoadTool.CheckCurrUserPms(ZentCloud.BLLPermission.PermissionKey.Pms_JuActivity_Advanced);
            actionStr = webAction == "add" ? "添加" : "编辑";
            if (webAction == "edit")
            {
                CurrActivityModel = this.juActivityBll.GetJuActivity(aid);

                if (CurrActivityModel == null)
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