using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class Scorehistory : System.Web.UI.Page
    {
        /// <summary>
        /// 用户逻辑层
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();

        protected UserInfo model = new UserInfo();

        protected string scoreType = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            model = bllUser.GetUserInfoByAutoID(int.Parse(Request["autoid"]));

            if (!string.IsNullOrEmpty(Request["type"]))
            {
                scoreType = Request["type"];
            }
        }
    }
}