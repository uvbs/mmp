using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class successwbh : System.Web.UI.Page
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        /// <summary>
        /// 当前用户
        /// </summary>
        public BLLJIMP.Model.UserInfo uinfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            uinfo = bllUser.GetCurrentUserInfo();
        }
    }
}