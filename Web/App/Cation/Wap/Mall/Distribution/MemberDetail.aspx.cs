using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution
{
    public partial class MemberDetail : System.Web.UI.Page
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo = new UserInfo();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["id"]))
            {
                UserInfo = bllUser.GetUserInfoByAutoID(int.Parse(Request["id"]));
            }
        }
    }
}