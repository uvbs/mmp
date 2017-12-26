using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    public partial class MasterDetails : System.Web.UI.Page
    {
        public JuMasterInfo masterInfo = new JuMasterInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            string masterId = Request["masterId"];

            if (!string.IsNullOrWhiteSpace(masterId))
            {
                masterInfo = new BLLJuActivity("手机访问").Get<JuMasterInfo>("MasterID = '" + masterId.ToString() + "'");
            }

            if (masterInfo == null)
                masterInfo = new JuMasterInfo();
        }
    }
}