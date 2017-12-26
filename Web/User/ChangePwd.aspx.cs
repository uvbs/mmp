using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.User
{
    public partial class ChangePwd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            ZentCloud.BLLJIMP.Model.UserInfo user = Comm.DataLoadTool.GetCurrUserModel();
            user.Password = this.txtPwd.Text.Trim();

            BLLJIMP.BLLUser userBll = new BLLJIMP.BLLUser(user.UserID);

            if (userBll.Update(user))
            {
                Tool.AjaxMessgeBox.ShowMessgeBoxForAjax(this.UpdatePanel1, this.GetType(), "更新成功，稍后请重新登录!", Common.ConfigHelper.GetConfigString("logoutUrl"));
            }
            else
            {
                Tool.AjaxMessgeBox.ShowMessgeBoxForAjax(this.UpdatePanel1, this.GetType(), "更新失败!");
            }
        }
    }
}