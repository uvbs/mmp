using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.kuanqiao
{
    public partial class SubmitInfoWeb : System.Web.UI.Page
    {
        /// 微信OPenID
        /// </summary>
        public string WxOpenId = null;
        BLLUser bllUser = new BLLUser("");
        public UserInfo userInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.UserAgent.ToLower().Contains("micromessenger"))
            {
                Response.Redirect("/customize/kuanqiao/wap/SubmitInfo.aspx");
                return;
            }
            if (!bllUser.IsLogin)
            {

                StringBuilder redirecturl = new StringBuilder();
                redirecturl.AppendFormat("/customize/kuanqiao/SubmitInfoWeb.aspx");
                Response.Redirect(string.Format("/QLogin.aspx?redirecturl={0}", redirecturl));

            }
            else
            {
                userInfo = bllUser.GetCurrentUserInfo();
                WxOpenId = userInfo.WXOpenId;
                //if (string.IsNullOrEmpty(WxOpenId))
                //{
                //     StringBuilder redirecturl = new StringBuilder();
                //     redirecturl.AppendFormat("/customize/kuanqiao/SubmitInfoWeb.aspx");
                //     Response.Redirect(string.Format("/QLogin.aspx?redirecturl={0}", redirecturl));

                //}

            }

        }
    }
}