using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Home
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lbtnogin_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = this.account.Value.Trim();
                string pwd = this.password.Value.Trim();

                if (userName.Length.Equals(0) || pwd.Length.Equals(0))
                {
                    ZentCloud.Common.WebMessageBox.Show(this, "用户名或密码输入有误!");
                    return;
                }

                ZentCloud.BLLJIMP.BLLUser bllUser = new ZentCloud.BLLJIMP.BLLUser(userName);
                ZentCloud.BLLJIMP.Model.UserInfo modelUserInfo;
                string msg;
                if (bllUser.Login(userName, pwd, out modelUserInfo, out msg))
                {
                    this.Session[Comm.SessionKey.LoginStatu] = 1;
                    this.Session[Comm.SessionKey.SelectMenu] = "1";
                    this.Session[Comm.SessionKey.UserID] = modelUserInfo.UserID;
                    this.Session[Comm.SessionKey.UserType] = modelUserInfo.UserType;


                    #region 微博权限初始化
                    this.Session[Comm.SessionKey.AccessToken] = modelUserInfo.WeiboAccessToken;
                    this.Session[Comm.SessionKey.WeiboID] = modelUserInfo.WeiboID; 
                    #endregion

                    //this.Response.Redirect("/");
                    //this.Response.Redirect("/Main.aspx");
                    this.Response.Redirect(ZentCloud.Common.ConfigHelper.GetConfigString("mainUrl"));
                }
                else
                {
                    //Page.ClientScript.RegisterStartupScript(typeof(Index), "openAjaxPopup", "<script type='text/javascript'>openAjaxPopup();</script>");
                    ZentCloud.Common.WebMessageBox.Show(this, "登录失败，请检查用户名密码是否正确!");
                    //this.ShowMessge("登录失败，请检查用户名密码是否正确!");
                }
            }
            catch (Exception ex)
            {
                ZentCloud.Common.WebMessageBox.Show(this, "异常：" + ex.Message);
            }
        }

        protected void lbtnReg_Click(object sender, EventArgs e)
        {
            //ZentCloud.Common.MessageBox.Show(this, "暂时只接受特约用户试用!");
            Response.Redirect("/User/Reg.aspx");
        }
    }
}