using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.open
{
    public partial class WXOAuth : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var url = Request["url"];
            var openidkey = Request["openid"];
            url = Common.Base64Change.DecodeBase64ByUTF8(url);

            if (BLLJIMP.BLLStatic.bll.IsLogin)
            {
                var user = BLLJIMP.BLLStatic.bll.GetCurrentUserInfo();

                if (url.IndexOf("?") == -1)
                {
                    url += "?";
                }

                url = string.Format("{0}userId={1}&openId={2}", url, user.UserID, user.WXOpenId);

                Response.Redirect(url);
            }

        }
    }
}