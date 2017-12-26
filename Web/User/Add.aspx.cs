using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.User
{
    public partial class Add : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void ShowMessge(string str)
        {
            Tool.AjaxMessgeBox.ShowMessgeBoxForAjax(this.UpdatePanel1, this.GetType(), str);
        }

        private void ShowMessge(string str, string url)
        {
            Tool.AjaxMessgeBox.ShowMessgeBoxForAjax(this.UpdatePanel1, this.GetType(), str, url);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ZentCloud.BLLJIMP.BLLUser bllUser = new ZentCloud.BLLJIMP.BLLUser(Session[Comm.SessionKey.UserID].ToString());

                string UserID = this.txtUserID.Text;
                string Password = this.txtPassword.Text;
                string TrueName = this.txtTrueName.Text;
                string Company = this.txtCompany.Text;
                string Phone = this.txtPhone.Text;
                string Email = this.txtEmail.Text;
                DateTime Regtime = DateTime.Now;
                decimal Account = 0;
                int Points = 0;
                int userType = int.Parse(this.wucUserType.SelectValue);

                ZentCloud.BLLJIMP.Model.UserInfo model = new ZentCloud.BLLJIMP.Model.UserInfo();
                model.UserID = UserID;
                model.Password = Password;
                model.TrueName = TrueName;
                model.Company = Company;
                model.Phone = Phone;
                model.Email = Email;
                model.Regtime = Regtime;
                model.Account = Account;
                model.Points = Points;
                model.UserType = userType;

                model.RegIP = ZentCloud.Common.MySpider.GetClientIP();
                model.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
                model.LastLoginDate = DateTime.Now;
                model.LoginTotalCount = 0;

                if (bllUser.Add(model))
                {
                    


                    this.ShowMessge("添加成功!", "List.aspx");
                }
                else
                {
                    this.ShowMessge("添加失败!");
                }

            }
            catch (Exception ex)
            {
                this.ShowMessge("异常:" + ex.Message);
            }
        }
    }
}