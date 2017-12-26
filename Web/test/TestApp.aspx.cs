using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Test
{
    public partial class TestApp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string userId = TextBox1.Text.Trim();
            string title = TextBox2.Text.Trim();
            string text = TextBox3.Text.Trim();
            string link = TextBox4.Text.Trim();
            if(string.IsNullOrWhiteSpace(userId) || 
                string.IsNullOrWhiteSpace(title) || 
                string.IsNullOrWhiteSpace(text) || 
                string.IsNullOrWhiteSpace(link)){
                Label1.Text = "请填写完内容";
                return;
            }
            BLLUser bllUser = new BLLUser();
            BLLAppManage bllApp = new BLLAppManage();
            string websiteOwner = bllUser.WebsiteOwner;
            UserInfo user = bllUser.GetUserInfo(userId, websiteOwner);
            if (user == null)
            {
                Label1.Text = "用户未找到";
                return;
            }
            WebsiteInfo website = bllUser.GetWebsiteInfoModelFromDataBase();
            string msg = "";
            bool result = bllApp.PushMassage(website, title, text, link, user, out msg);
        }
    }
}