using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.WuBuHui.Tutor
{
    public partial class WXTutorProblem : UserPage
    {
        public BLLJIMP.Model.UserInfo uinfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            uinfo = DataLoadTool.GetCurrUserModel();
            
        }
    }
}