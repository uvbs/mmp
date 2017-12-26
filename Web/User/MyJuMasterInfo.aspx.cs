using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.User
{
    public partial class MyJuMasterInfo : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL("");
        public JuMasterInfo model=new JuMasterInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            model = bll.Get<JuMasterInfo>(string.Format("MasterID='{0}'", Comm.DataLoadTool.GetCurrUserID()));


        }
    }
}