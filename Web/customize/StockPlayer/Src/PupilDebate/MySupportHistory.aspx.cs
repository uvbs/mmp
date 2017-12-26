using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.PupilDebate
{
    public partial class MySupportHistory : System.Web.UI.Page
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        protected UserInfo curUser = null;
        //protected string rootId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (bllUser.GetCurrentUserInfo() != null)
            {
                curUser = bllUser.GetCurrentUserInfo();
            }
            //rootId = Request["rootId"];
        }
    }
}