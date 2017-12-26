using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.SportsShow
{
    public partial class info : System.Web.UI.Page
    {
        BLLUser bllUser = new BLLUser();
        protected UserInfo user = new UserInfo();
        protected string type;
        protected void Page_Load(object sender, EventArgs e)
        {
            user = bllUser.GetCurrentUserInfo();
            if (user == null)
            {
                this.Response.Redirect(Common.ConfigHelper.GetConfigString("noPmsUrl").ToLower(), true);
                return;
            }
            type = this.Request["type"];
            if (string.IsNullOrWhiteSpace(user.Ex1) && user.WXSex.HasValue)
            {
                if (user.WXSex == 1)
                {
                    user.Ex1 ="男";
                }
                else if (user.WXSex == 2)
                {
                    user.Ex1 = "女";
                }
            }

        }
        protected void btnPost_Click(object sender, EventArgs e)
        {
            UserInfo user = bllUser.GetCurrentUserInfo();
            if (user == null)
            {
                Common.WebMessageBox.Show(this, "登录失败！");
                return;
            }
            user.Phone = hdnPhone.Value;
            if (!Common.MyRegex.PhoneNumLogicJudge(user.Phone))
            {
                Common.WebMessageBox.Show(this, "手机号码错误！");
                return;
            }
            user.Ex1 = hdnSex.Value;
            user.Ex2 = hdnAge.Value;
            user.Ex3 = hdnSportMember.Value;

            if (bllUser.Update(user,string.Format("Phone='{0}',Ex1='{1}',Ex2='{2}',Ex3='{3}'",user.Phone,user.Ex1,user.Ex2,user.Ex3)
                ,string.Format("AutoID={0}",user.AutoID))>0)
            {
                this.Response.Redirect("beforCanceled.aspx", true);
            }
            else
            {
                Common.WebMessageBox.Show(this, "提交失败！");
            }
        }
    }
}