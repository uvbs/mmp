using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.User
{
    public partial class Reg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lbtnQuit_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Login.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                BLLUser bllUser = new BLLUser(this.txtUserID.Text.Trim());

                ZentCloud.BLLJIMP.Model.UserInfo model = new ZentCloud.BLLJIMP.Model.UserInfo();
                model.UserID = bllUser.UserID;
                model.Password = this.txtPassword.Text;
                model.TrueName = this.txtTrueName.Text;
                model.Company = this.txtCompany.Text;
                model.Phone = this.txtPhone.Text;
                model.Email = this.txtEmail.Text;
                model.Regtime = DateTime.Now;;
                model.Account = 0;
                model.Points = 0;
                model.EmailPoints = 0;
                model.UserType = 2;//注册用户默认为普通用户

                //判断用户是否已存在
                if (bllUser.Exists(model, "UserID"))
                {
                    ZentCloud.Common.WebMessageBox.Show(this.Page, "用户已存在!");
                    return;
                }

                if (bllUser.Add(model))
                {
                    //将用户添加到微博用户组
                    ZentCloud.BLLPermission.BLLMenuPermission pmsBll = new BLLPermission.BLLMenuPermission(model.UserID);
                    pmsBll.SetUserPmsGroup(model.UserID, 508);

                    ZentCloud.Common.WebMessageBox.ShowAndRedirect(this.Page, "注册成功!", Common.ConfigHelper.GetConfigString("loginUrl"));
                    
                }
                else
                {
                    ZentCloud.Common.WebMessageBox.Show(this.Page, "注册失败!");
                    
                }

            }
            catch (Exception ex)
            {
                ZentCloud.Common.WebMessageBox.Show(this.Page, "添加失败!");
            }
        }
    }
}