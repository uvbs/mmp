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
    public partial class Index : System.Web.UI.Page
    {
        BLLForbesQuestion bllQuestion = new BLLForbesQuestion();
        private string ActivityName = "2015上海体博会体商测试";
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["userID"] = "jubit";
            //WebsiteInfo webSiteModel = bllQuestion.Get<WebsiteInfo>(string.Format(" WebsiteOwner = '{0}' ", "hf"));
            //Session["WebsiteInfoModel"] = webSiteModel;
            UserInfo user = bllQuestion.GetCurrentUserInfo();
            if (user == null)
            {
                //Common.WebMessageBox.ShowAndRedirect(this, "登录失败", Common.ConfigHelper.GetConfigString("noPmsUrl").ToLower());
                return;
            }
            int ResultCount = bllQuestion.GetQuestionResultCount(user.UserID, bllQuestion.WebsiteOwner, ActivityName);
            if (ResultCount > 0)
            {
                this.Response.Redirect("beforCanceled.aspx", true);
            }
        }
        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (DateTime.Now < Convert.ToDateTime("2015-11-09"))
            {
                this.Response.Redirect("question.aspx",true);
            }
        }
    }
}